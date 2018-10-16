using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Supplier.sales.ashx
{
	[AdministerCheck(true)]
	public class ManageOrder : SupplierAdminHandler
	{
		private const string PAYMENT_POD_NAME = "hishop.plugins.payment.podrequest";

		private const string PAYMENT_BANK_NAME = "hishop.plugins.payment.bankrequest";

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "exportexcel")
				{
					this.ExportExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private void GetList(HttpContext context)
		{
			OrderQuery orderQuery = this.GetOrderQuery(context);
			DataGridViewModel<Dictionary<string, object>> orderList = this.GetOrderList(orderQuery);
			string s = base.SerializeObjectToJson(orderList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetOrderList(OrderQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DbQueryResult orders = OrderHelper.GetOrders(query);
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(orders.Data);
			dataGridViewModel.total = orders.TotalRecords;
			string[] orderIds = (from d in dataGridViewModel.rows
			select d["OrderId"].ToString()).ToArray();
			List<RefundInfo> refundInfos = TradeHelper.GetRefundInfos(orderIds);
			foreach (Dictionary<string, object> row in dataGridViewModel.rows)
			{
				OrderInfo order = TradeHelper.GetOrderInfo(row["OrderId"].ToString());
				row.Add("OrderStatusText", OrderHelper.GetOrderStatusText(order.OrderStatus, order.ShippingModeId, order.IsConfirm, order.Gateway, 0, order.PreSaleId, order.DepositDate, false, order.ItemStatus, OrderType.NormalOrder));
				row.Add("CanConfirmOrder", order.CanConfirmOrder());
				row.Add("canCheckTake", order.CanConfirmTakeCode());
				row.Add("canCloseOrder", order.CanClose(base.CurrentSiteSetting.OpenMultStore, true));
				row.Add("canOfflineReceipt", OrderHelper.CanConfirmOfflineReceipt(order, true));
				row.Add("canSendGoods", order.CanSendGoods(base.CurrentSiteSetting.OpenMultStore));
				row.Add("canFinishTrade", order.OrderStatus == OrderStatus.SellerAlreadySent && order.ItemStatus == OrderItemStatus.Nomarl);
				row.Add("SupplierOrderTotals", order.OrderCostPrice + order.Freight);
				if (order.IsStoreCollect)
				{
					row.Add("isShowCheckRefund", order.OrderStatus == OrderStatus.ApplyForRefund && order.StoreId == base.CurrentManager.StoreId);
				}
				row.Add("InvoiceTypeText", string.IsNullOrEmpty(order.InvoiceTitle) ? "" : EnumDescription.GetEnumDescription((Enum)(object)order.InvoiceType, 0));
				bool flag = false;
				if (order.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(order.FightGroupId);
					if (fightGroup != null)
					{
						row.Add("FightGroupActivityId", fightGroup.FightGroupActivityId);
						if (fightGroup.Status == FightGroupStatus.FightGroupIn && order.OrderStatus != OrderStatus.WaitBuyerPay && order.OrderStatus != OrderStatus.Closed)
						{
							flag = true;
						}
					}
				}
				row.Add("FightGrouping", flag);
				row.Add("isGiftOrder", order.LineItems.Count <= 0);
				row["IsError"] = order.IsError;
				row["ErrorMessage"] = order.ErrorMessage;
				RefundInfo refundInfo = refundInfos.FirstOrDefault((RefundInfo d) => d.OrderId == order.OrderId);
				if (refundInfo != null)
				{
					row.Add("RefundId", refundInfo.RefundId);
				}
				int num = 0;
				int num2 = 0;
				bool flag2 = false;
				string value = "订单中有商品正在进行退货/退款";
				AfterSaleTypes? nullable = null;
				if (order.ItemStatus != 0 || order.OrderStatus == OrderStatus.ApplyForRefund)
				{
					if (order.OrderStatus == OrderStatus.ApplyForRefund)
					{
						RefundInfo refundInfo2 = TradeHelper.GetRefundInfo(order.OrderId);
						if (refundInfo2 != null)
						{
							flag2 = true;
							value = "订单已申请退款";
						}
					}
					else
					{
						int num3 = 0;
						AfterSaleTypes afterSaleTypes = AfterSaleTypes.ReturnAndRefund;
						int num4 = 0;
						foreach (LineItemInfo value2 in order.LineItems.Values)
						{
							if (value2.ReturnInfo != null || value2.ReplaceInfo != null)
							{
								ReturnInfo returnInfo = value2.ReturnInfo;
								ReplaceInfo replaceInfo = value2.ReplaceInfo;
								if (num3 == 0 || (returnInfo != null && returnInfo.HandleStatus != ReturnStatus.Refused && returnInfo.HandleStatus != ReturnStatus.Returned) || (replaceInfo != null && (replaceInfo.HandleStatus != ReplaceStatus.Refused || replaceInfo.HandleStatus != ReplaceStatus.Replaced)))
								{
									if (value2.ReturnInfo != null)
									{
										afterSaleTypes = AfterSaleTypes.ReturnAndRefund;
										num4 = value2.ReturnInfo.ReturnId;
									}
									else
									{
										afterSaleTypes = AfterSaleTypes.Replace;
										num4 = value2.ReplaceInfo.ReplaceId;
									}
								}
								num3++;
							}
						}
						if (order.ItemStatus == OrderItemStatus.HasReturnOrReplace)
						{
							value = "订单中有商品正在退货/换货中";
						}
						else if (order.ItemStatus == OrderItemStatus.HasReplace)
						{
							value = "订单中有商品正在进行换货";
						}
						else if (order.ItemStatus == OrderItemStatus.HasReturn)
						{
							value = "订单中有商品在进行退货/退款操作";
						}
						else if (order.ReturnedCount > 0)
						{
							value = "订单中有商品已退货完成";
						}
						if (num3 > 0)
						{
							flag2 = true;
							nullable = afterSaleTypes;
							if (afterSaleTypes == AfterSaleTypes.ReturnAndRefund)
							{
								num = num4;
							}
							else
							{
								num2 = num4;
							}
						}
					}
				}
				row.Add("ReturnId", num);
				row.Add("ReplaceId", num2);
				row.Add("AfterSaleType", nullable);
				row.Add("isShowRefund", flag2);
				row.Add("RefundStatus", value);
			}
			return dataGridViewModel;
		}

		private OrderQuery GetOrderQuery(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			OrderQuery orderQuery = new OrderQuery();
			string parameter = base.GetParameter(context, "InvoiceType", false);
			int num3;
			if (parameter.ToNullString().Trim() != "")
			{
				InvoiceType invoiceType;
				if (parameter == "0")
				{
					OrderQuery orderQuery2 = orderQuery;
					invoiceType = InvoiceType.Personal;
					num3 = invoiceType.GetHashCode();
					string arg = num3.ToString();
					invoiceType = InvoiceType.Enterprise;
					num3 = invoiceType.GetHashCode();
					orderQuery2.InvoiceTypes = $"{arg},{num3.ToString()}";
				}
				else if (parameter == "2")
				{
					OrderQuery orderQuery3 = orderQuery;
					invoiceType = InvoiceType.Personal_Electronic;
					num3 = invoiceType.GetHashCode();
					string arg2 = num3.ToString();
					invoiceType = InvoiceType.Enterprise_Electronic;
					num3 = invoiceType.GetHashCode();
					orderQuery3.InvoiceTypes = $"{arg2},{num3.ToString()}";
				}
				else
				{
					OrderQuery orderQuery4 = orderQuery;
					invoiceType = InvoiceType.VATInvoice;
					num3 = invoiceType.GetHashCode();
					orderQuery4.InvoiceTypes = num3.ToString();
				}
			}
			orderQuery.ItemStatus = 0;
			if (!string.IsNullOrEmpty(context.Request["OrderId"]))
			{
				orderQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			}
			if (!string.IsNullOrEmpty(context.Request["ProductName"]))
			{
				orderQuery.ProductName = Globals.UrlDecode(context.Request["ProductName"]);
			}
			if (!string.IsNullOrEmpty(context.Request["ShipTo"]))
			{
				orderQuery.ShipTo = Globals.UrlDecode(context.Request["ShipTo"]);
			}
			if (!string.IsNullOrEmpty(context.Request["UserName"]))
			{
				orderQuery.UserName = Globals.UrlDecode(context.Request["UserName"]);
			}
			orderQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			orderQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			orderQuery.GroupBuyId = base.GetIntParam(context, "GroupBuyId", true);
			int? intParam = base.GetIntParam(context, "OrderStatus", true);
			int? nullable;
			if (intParam.HasValue)
			{
				nullable = intParam;
				num3 = 999;
				if (nullable == num3)
				{
					orderQuery.TakeOnStore = true;
					orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
				}
				else
				{
					nullable = intParam;
					num3 = 2;
					if (nullable == num3)
					{
						orderQuery.TakeOnStore = false;
					}
					orderQuery.Status = (OrderStatus)intParam.Value;
				}
			}
			orderQuery.IsPrinted = base.GetIntParam(context, "IsPrinted", true);
			orderQuery.ShippingModeId = base.GetIntParam(context, "ModeId", true);
			orderQuery.IsAllotStore = base.GetIntParam(context, "StoreDistribution", true);
			int num4 = default(int);
			if (!string.IsNullOrEmpty(context.Request["region"]) && int.TryParse(context.Request["region"], out num4))
			{
				orderQuery.RegionId = num4;
				orderQuery.FullRegionName = RegionHelper.GetFullRegion(num4, ",", true, 0);
			}
			orderQuery.SourceOrder = base.GetIntParam(context, "sourceorder", true);
			orderQuery.StoreId = base.GetIntParam(context, "storeId", true);
			nullable = orderQuery.StoreId;
			num3 = -2;
			if (nullable == num3)
			{
				orderQuery.StoreId = null;
			}
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			orderQuery.PageIndex = num;
			orderQuery.PageSize = num2;
			orderQuery.SupplierId = base.CurrentManager.StoreId;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}

		private void ExportExcel(HttpContext context)
		{
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.SupplierId = HiContext.Current.Manager.StoreId;
			string text = "";
			if (!string.IsNullOrEmpty(context.Request["orderIds"]))
			{
				text = context.Request["orderIds"];
				text = (orderQuery.OrderIds = "'" + text.Replace(",", "','") + "'");
			}
			else
			{
				orderQuery = this.GetOrderQuery(context);
			}
			orderQuery.PageSize = 100000;
			List<OrderInfo> exportOrders = OrderHelper.GetExportOrders(orderQuery);
			this.xlsContent(context, exportOrders);
		}

		private void xlsContent(HttpContext context, List<OrderInfo> orderInfos)
		{
			StringBuilder stringBuilder = new StringBuilder(200);
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>下单时间</th>");
			stringBuilder.Append("<th>付款时间</th>");
			stringBuilder.Append("<th>完成时间</th>");
			stringBuilder.Append("<th>用户名</th>");
			stringBuilder.Append("<th>等级</th>");
			stringBuilder.Append("<th>买家真实姓名</th>");
			stringBuilder.Append("<th>收货地址</th>");
			stringBuilder.Append("<th>详细地址</th>");
			stringBuilder.Append("<th>收货人姓名</th>");
			stringBuilder.Append("<th>电话号码</th>");
			stringBuilder.Append("<th>手机号码</th>");
			stringBuilder.Append("<th>发货时间</th>");
			stringBuilder.Append("<th>物流公司</th>");
			stringBuilder.Append("<th>运费</th>");
			stringBuilder.Append("<th>发货单号</th>");
			stringBuilder.Append("<th>购买数量</th>");
			stringBuilder.Append("<th>发货数量</th>");
			stringBuilder.Append("<th>用户备注</th>");
			stringBuilder.Append("<th>商品供货价</th>");
			stringBuilder.Append("<th>结算价</th>");
			stringBuilder.Append("<th>商品明细</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (OrderInfo orderInfo in orderInfos)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.OrderId, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.OrderDate, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.PayDate, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.FinishDate, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.Username, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(MemberHelper.GetMemberGradeName(orderInfo.UserId), true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.RealName, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.ShippingRegion, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.Address, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.ShipTo, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.TelPhone, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.CellPhone, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.ShipToDate, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.ExpressCompanyName, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.Freight, false));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.ShipOrderNumber, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(orderInfo.GetAllQuantity(true), false));
				int num = 0;
				decimal num2 = default(decimal);
				if (orderInfo.LineItems != null && orderInfo.LineItems.Count > 0)
				{
					num = orderInfo.LineItems.Values.Sum((LineItemInfo i) => i.ShipmentQuantity);
					num2 = orderInfo.LineItems.Values.Sum((LineItemInfo i) => i.ItemCostPrice * (decimal)i.Quantity);
				}
				stringBuilder2.Append(this.GetXLSFieldsTD(num, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.Remark, true));
				stringBuilder2.Append(this.GetXLSFieldsTD(num2, false));
				stringBuilder2.Append(this.GetXLSFieldsTD((orderInfo.OrderCostPrice + orderInfo.Freight).F2ToString("f2"), false));
				StringBuilder stringBuilder3 = new StringBuilder();
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					stringBuilder3.Append(value.ItemDescription.ToNullString());
					stringBuilder3.Append("[" + value.Quantity.ToNullString() + "]");
					if (!string.IsNullOrEmpty(value.SKUContent))
					{
						stringBuilder3.Append("[" + value.SKUContent + "]");
					}
					if (!string.IsNullOrEmpty(value.PromotionName))
					{
						stringBuilder3.Append("(" + value.PromotionName + ")");
					}
					stringBuilder3.Append("；");
				}
				foreach (OrderGiftInfo gift in orderInfo.Gifts)
				{
					stringBuilder3.Append(gift.GiftName.ToNullString());
					stringBuilder3.Append("[" + gift.Quantity.ToNullString() + "]");
					stringBuilder3.Append("；");
				}
				stringBuilder2.AppendFormat("<td>{0}</td>", stringBuilder3.ToString());
				UserInvoiceDataInfo invoiceInfo = orderInfo.InvoiceInfo;
				if (invoiceInfo == null)
				{
					invoiceInfo = new UserInvoiceDataInfo
					{
						InvoiceType = orderInfo.InvoiceType,
						InvoiceTitle = orderInfo.InvoiceTitle,
						InvoiceTaxpayerNumber = orderInfo.InvoiceTaxpayerNumber
					};
				}
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			ManageOrder.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "OrderList" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private string GetXLSFieldsTD(object argFields, bool istext)
		{
			if (argFields == null)
			{
				argFields = string.Empty;
			}
			else
			{
				string a = argFields.GetType().ToString();
				if (a == "System.DateTime")
				{
					DateTime? nullable = argFields.ToDateTime();
					argFields = ((!nullable.HasValue || nullable.Equals("0001/1/1 0:00:00")) ? "" : argFields);
				}
			}
			string arg = istext ? " style='vnd.ms-excel.numberformat:@'" : "";
			return $"<td{arg}>{argFields}</td>";
		}

		public static void DownloadFile(HttpResponse argResp, StringBuilder argFileStream, string strFileName)
		{
			try
			{
				string value = "attachment; filename=" + strFileName;
				argResp.Clear();
				argResp.Buffer = true;
				argResp.AppendHeader("Content-Disposition", value);
				argResp.ContentType = "application/ms-excel";
				argResp.ContentEncoding = Encoding.GetEncoding("utf-8");
				argResp.Write(argFileStream);
				argResp.Flush();
				argResp.End();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
