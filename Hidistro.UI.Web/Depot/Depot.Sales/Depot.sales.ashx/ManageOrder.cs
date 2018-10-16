using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Web.Depot.sales.ashx
{
	public class ManageOrder : StoreAdminBaseHandler
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
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "remark":
				this.Remark(context);
				break;
			case "closeorder":
				this.CloseOrder(context);
				break;
			case "confirmorder":
				this.ConfirmOrder(context);
				break;
			case "finishtrade":
				this.FinishTrade(context);
				break;
			case "confirmpay":
				this.ConfirmPayOrder(context);
				break;
			case "exportexcel":
				this.ExportExcel(context);
				break;
			case "CancelSendGoods":
				this.CancelSendGoods(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void CancelSendGoods(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = context.Request["order_id"].ToNullString();
			int num = context.Request["cancel_reason_id"].ToInt(0);
			string text2 = context.Request["cancel_reason"].ToNullString();
			string text3 = DadaHelper.orderFormalCancel(masterSettings.DadaSourceID, text, num, text2);
			if (string.IsNullOrEmpty(text))
			{
				base.ReturnFailResult(context, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription(), 112, true);
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					base.ReturnFailResult(context, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription(), 113, true);
				}
				else if (orderInfo.StoreId != base.CurrentManager.StoreId)
				{
					base.ReturnFailResult(context, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription(), 507, true);
				}
				else if (num == 0)
				{
					base.ReturnFailResult(context, ((Enum)(object)ApiErrorCode.CancelSendGoodsReasonEmpty).ToDescription(), 138, true);
				}
				else
				{
					orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					orderInfo.CloseReason = text2;
					orderInfo.DadaStatus = DadaStatus.Cancel;
					TradeHelper.UpdateOrderInfo(orderInfo);
					base.ReturnSuccessResult(context, "取消发单成功", 0, true);
				}
			}
		}

		private void ExportExcel(HttpContext context)
		{
			OrderQuery orderQuery = this.GetOrderQuery(context);
			List<OrderInfo> exportOrders = OrderHelper.GetExportOrders(orderQuery);
			this.xlsContent(context, exportOrders);
		}

		private void xlsContent(HttpContext context, List<OrderInfo> orderInfos)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>下单时间</th>");
			stringBuilder.Append("<th>付款时间</th>");
			stringBuilder.Append("<th>完成时间</th>");
			stringBuilder.Append("<th>用户名</th>");
			stringBuilder.Append("<th>等级</th>");
			stringBuilder.Append("<th>收货地址</th>");
			stringBuilder.Append("<th>详细地址</th>");
			stringBuilder.Append("<th>收货人姓名</th>");
			stringBuilder.Append("<th>电话号码</th>");
			stringBuilder.Append("<th>手机号码</th>");
			stringBuilder.Append("<th>发货时间</th>");
			stringBuilder.Append("<th>配送方式</th>");
			stringBuilder.Append("<th>物流公司</th>");
			stringBuilder.Append("<th>发货单号</th>");
			stringBuilder.Append("<th>实付金额</th>");
			stringBuilder.Append("<th>退款金额</th>");
			stringBuilder.Append("<th>运费</th>");
			stringBuilder.Append("<th>订单奖励积分</th>");
			stringBuilder.Append("<th>商品总金额</th>");
			stringBuilder.Append("<th>支付方式</th>");
			stringBuilder.Append("<th>匹配门店</th>");
			stringBuilder.Append("<th>订单状态</th>");
			stringBuilder.Append("<th>税金</th>");
			stringBuilder.Append("<th>积分抵扣</th>");
			stringBuilder.Append("<th>优惠券抵扣</th>");
			stringBuilder.Append("<th>优惠金额</th>");
			stringBuilder.Append("<th>优惠活动</th>");
			stringBuilder.Append("<th>购买件数</th>");
			stringBuilder.Append("<th>发货件数</th>");
			stringBuilder.Append("<th>用户备注</th>");
			stringBuilder.Append("<th>商品明细</th>");
			if (HiContext.Current.SiteSettings.IsOpenCertification)
			{
				stringBuilder.Append("<th>身份证号码</th>");
			}
			stringBuilder.Append("<th>发票类型</th>");
			stringBuilder.Append("<th>抬头/单位名称</th>");
			stringBuilder.Append("<th>纳税人识别号</th>");
			stringBuilder.Append("<th>注册地址</th>");
			stringBuilder.Append("<th>注册电话</th>");
			stringBuilder.Append("<th>开户银行</th>");
			stringBuilder.Append("<th>银行帐户</th>");
			stringBuilder.Append("<th>收票人姓名</th>");
			stringBuilder.Append("<th>收票人手机</th>");
			stringBuilder.Append("<th>收票人邮箱</th>");
			stringBuilder.Append("<th>收票人地区</th>");
			stringBuilder.Append("<th>收票人详细地址</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (OrderInfo orderInfo in orderInfos)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.PayOrderId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.OrderDate, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.PayDate, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.FinishDate, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.Username, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(MemberHelper.GetMemberGradeName(orderInfo.UserId), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.ShippingRegion, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.Address, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.ShipTo, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.TelPhone, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.CellPhone, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.ShipToDate, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.ModeName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.ExpressCompanyName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.ShipOrderNumber, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.GetTotal(false), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.RefundAmount, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.Freight, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.Points, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.GetAmount(false), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.PaymentType, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(string.IsNullOrEmpty(orderInfo.StoreName) ? "平台店" : orderInfo.StoreName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)orderInfo.OrderStatus, 0), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.Tax, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.DeductionMoney.HasValue ? orderInfo.DeductionMoney.Value : decimal.Zero, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.CouponValue, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.ReducedPromotionAmount, false));
				string text = "";
				if (!string.IsNullOrEmpty(orderInfo.ReducedPromotionName))
				{
					text = text + orderInfo.ReducedPromotionName + ";";
				}
				if (!string.IsNullOrEmpty(orderInfo.SentTimesPointPromotionName))
				{
					text = text + orderInfo.SentTimesPointPromotionName + ";";
				}
				if (!string.IsNullOrEmpty(orderInfo.FreightFreePromotionName))
				{
					text = text + orderInfo.FreightFreePromotionName + ";";
				}
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(text, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.GetAllQuantity(true), false));
				int num = 0;
				if (orderInfo.LineItems != null && orderInfo.LineItems.Count > 0)
				{
					num = orderInfo.LineItems.Values.Sum((LineItemInfo i) => i.ShipmentQuantity);
				}
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(num, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(orderInfo.Remark, true));
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
				if (HiContext.Current.SiteSettings.IsOpenCertification)
				{
					string argFields = string.Empty;
					if (!string.IsNullOrWhiteSpace(orderInfo.IDNumber) && orderInfo.IsincludeCrossBorderGoods)
					{
						try
						{
							argFields = HiCryptographer.Decrypt(orderInfo.IDNumber);
						}
						catch
						{
						}
					}
					stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(argFields, true));
				}
				UserInvoiceDataInfo userInvoiceDataInfo = orderInfo.InvoiceInfo;
				if (userInvoiceDataInfo == null)
				{
					userInvoiceDataInfo = new UserInvoiceDataInfo
					{
						InvoiceType = orderInfo.InvoiceType,
						InvoiceTitle = orderInfo.InvoiceTitle,
						InvoiceTaxpayerNumber = orderInfo.InvoiceTaxpayerNumber
					};
				}
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(string.IsNullOrEmpty(userInvoiceDataInfo.InvoiceTitle.ToNullString()) ? "" : userInvoiceDataInfo.InvoceTypeText.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.InvoiceTitle.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.InvoiceTaxpayerNumber.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.RegisterAddress.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.RegisterTel.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.OpenBank.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.BankAccount.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.ReceiveName.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.ReceivePhone.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.ReceiveEmail.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.ReceiveRegionName.ToNullString(), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(userInvoiceDataInfo.ReceiveAddress.ToNullString(), true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "StoreOrderList" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
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
				row.Add("OrderStatusText", OrderHelper.GetOrderStatusText(order.OrderStatus, order.ShippingModeId, order.IsConfirm, order.Gateway, 0, order.PreSaleId, order.DepositDate, false, order.ItemStatus, order.OrderType));
				row.Add("CanConfirmOrder", order.CanConfirmOrder());
				row.Add("canCheckTake", order.CanConfirmTakeCode());
				row.Add("canCloseOrder", order.CanClose(base.CurrentSiteSetting.OpenMultStore, true));
				row.Add("canOfflineReceipt", OrderHelper.CanConfirmOfflineReceipt(order, true));
				row.Add("canSendGoods", order.CanSendGoods(base.CurrentSiteSetting.OpenMultStore));
				row.Add("canFinishTrade", order.OrderStatus == OrderStatus.SellerAlreadySent && order.ItemStatus == OrderItemStatus.Nomarl);
				row.Add("canCancelSendGoods", order.OrderStatus == OrderStatus.SellerAlreadySent && (order.DadaStatus == DadaStatus.WaitOrder || order.DadaStatus == DadaStatus.WaitTake));
				row.Add("InvoiceTypeText", string.IsNullOrEmpty(order.InvoiceTitle) ? "" : EnumDescription.GetEnumDescription((Enum)(object)order.InvoiceType, 0));
				row.Add("dadaState", (order.DadaStatus != 0) ? ((Enum)(object)order.DadaStatus).ToDescription() : "");
				RefundInfo refundInfo = refundInfos.FirstOrDefault((RefundInfo d) => d.OrderId == order.OrderId);
				if (refundInfo != null)
				{
					row.Add("RefundId", refundInfo.RefundId);
				}
				if (order.IsStoreCollect)
				{
					row.Add("isShowCheckRefund", order.OrderStatus == OrderStatus.ApplyForRefund && order.StoreId == base.CurrentManager.StoreId);
				}
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
			}
			return dataGridViewModel;
		}

		private OrderQuery GetOrderQuery(HttpContext context)
		{
			int pageIndex = 1;
			int pageSize = 10;
			string empty = string.Empty;
			OrderQuery orderQuery = new OrderQuery();
			string parameter = base.GetParameter(context, "InvoiceType", false);
			int num;
			if (parameter.ToNullString().Trim() != "")
			{
				InvoiceType invoiceType;
				if (parameter == "0")
				{
					OrderQuery orderQuery2 = orderQuery;
					invoiceType = InvoiceType.Personal;
					num = invoiceType.GetHashCode();
					string arg = num.ToString();
					invoiceType = InvoiceType.Enterprise;
					num = invoiceType.GetHashCode();
					orderQuery2.InvoiceTypes = $"{arg},{num.ToString()}";
				}
				else if (parameter == "2")
				{
					OrderQuery orderQuery3 = orderQuery;
					invoiceType = InvoiceType.Personal_Electronic;
					num = invoiceType.GetHashCode();
					string arg2 = num.ToString();
					invoiceType = InvoiceType.Enterprise_Electronic;
					num = invoiceType.GetHashCode();
					orderQuery3.InvoiceTypes = $"{arg2},{num.ToString()}";
				}
				else
				{
					OrderQuery orderQuery4 = orderQuery;
					invoiceType = InvoiceType.VATInvoice;
					num = invoiceType.GetHashCode();
					orderQuery4.InvoiceTypes = num.ToString();
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
			if (!string.IsNullOrEmpty(context.Request["OrderStatus"]))
			{
				int num2 = 0;
				if (int.TryParse(context.Request["OrderStatus"], out num2))
				{
					if (num2 != 999)
					{
						orderQuery.Status = (OrderStatus)num2;
						if (num2 != 0 && num2 != 4 && num2 != 5)
						{
							orderQuery.TakeOnStore = false;
						}
					}
					else
					{
						orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
						orderQuery.TakeOnStore = true;
					}
				}
			}
			int num3 = default(int);
			if (!string.IsNullOrEmpty(context.Request["region"]) && int.TryParse(context.Request["region"], out num3))
			{
				orderQuery.RegionId = num3;
				orderQuery.FullRegionName = RegionHelper.GetFullRegion(num3, ",", true, 0);
			}
			empty = context.Request["StartDate"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				orderQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			}
			empty = context.Request["EndDate"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				orderQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			}
			empty = context.Request["GroupBuyId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				orderQuery.GroupBuyId = base.GetIntParam(context, "GroupBuyId", false);
			}
			empty = context.Request["IsPrinted"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				orderQuery.IsPrinted = base.GetIntParam(context, "IsPrinted", false);
			}
			empty = context.Request["ModeId"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				orderQuery.ShippingModeId = base.GetIntParam(context, "ModeId", false);
			}
			empty = context.Request["sourceorder"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				orderQuery.SourceOrder = base.GetIntParam(context, "sourceorder", false);
			}
			empty = context.Request["isTickit"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				OrderQuery orderQuery5 = orderQuery;
				int? intParam = base.GetIntParam(context, "isTickit", false);
				num = 0;
				orderQuery5.IsTickit = (intParam > num);
			}
			empty = context.Request["takecode"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				orderQuery.TakeCode = empty;
			}
			empty = context.Request["page"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageIndex = int.Parse(empty);
				}
				catch
				{
					pageIndex = 1;
				}
			}
			empty = context.Request["rows"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					pageSize = int.Parse(empty);
				}
				catch
				{
					pageSize = 10;
				}
			}
			orderQuery.Type = (OrderType?)base.GetIntParam(context, "OrderType", true);
			orderQuery.PageIndex = pageIndex;
			orderQuery.PageSize = pageSize;
			orderQuery.StoreId = base.CurrentManager.StoreId;
			orderQuery.SupplierId = 0;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}

		private void Remark(HttpContext context)
		{
			string text = context.Request.Form["orderId"];
			int value = base.GetIntParam(context, "remarkFlag", false).Value;
			string text2 = context.Request.Form["remarkTxt"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			if (text2.Length > 300)
			{
				throw new HidistroAshxException("备忘录长度限制在300个字符以内");
			}
			Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
			if (!regex.IsMatch(text2))
			{
				throw new HidistroAshxException("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾");
			}
			text2 = Globals.HtmlEncode(text2);
			if (value > 0)
			{
				OrderMark value2 = (OrderMark)value;
				orderInfo.ManagerMark = value2;
			}
			orderInfo.ManagerRemark = text2;
			if (OrderHelper.SaveRemark(orderInfo))
			{
				base.ReturnResult(context, true, "操作成功", 0, true);
				return;
			}
			throw new HidistroAshxException("保存失败");
		}

		private void CloseOrder(HttpContext context)
		{
			string text = context.Request.Form["orderId"];
			string closeReason = context.Request.Form["CloseReason"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			orderInfo.CloseReason = closeReason;
			if (OrderHelper.CloseTransaction(orderInfo))
			{
				if (orderInfo.ShippingModeId == -2 && orderInfo.IsConfirm)
				{
					OrderHelper.CloseDeportOrderReturnStock(orderInfo, "");
				}
				MemberInfo user = Users.GetUser(orderInfo.UserId);
				Messenger.OrderClosed(user, orderInfo, orderInfo.CloseReason);
				orderInfo.OnClosed();
				base.ReturnResult(context, true, "操作成功", 0, true);
				return;
			}
			throw new HidistroAshxException("关闭订单失败");
		}

		private void ConfirmOrder(HttpContext context)
		{
			SiteSettings currentSiteSetting = base.CurrentSiteSetting;
			string text = context.Request.Form["orderId"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			if (orderInfo.IsConfirm || (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid && orderInfo.PaymentTypeId != -3))
			{
				return;
			}
			string empty = string.Empty;
			if (OrderHelper.ConfirmTakeOnStoreOrder(orderInfo, out empty, true, "", false))
			{
				StoresInfo storeById = DepotHelper.GetStoreById(orderInfo.StoreId);
				MemberInfo user = Users.GetUser(orderInfo.UserId);
				if (user != null)
				{
					if (!string.IsNullOrEmpty(currentSiteSetting.HiPOSAppId) && !string.IsNullOrEmpty(currentSiteSetting.HiPOSAppSecret) && !string.IsNullOrEmpty(currentSiteSetting.HiPOSMerchantId) && !string.IsNullOrEmpty(currentSiteSetting.HiPOSExpireAt) && currentSiteSetting.HiPOSExpireAt.ToDateTime() > (DateTime?)DateTime.Now)
					{
						string empty2 = string.Empty;
						string siteUrl = currentSiteSetting.SiteUrl;
						string text2 = Globals.HIPOSTAKECODEPREFIX + orderInfo.TakeCode;
						empty2 = ((siteUrl.IndexOf("http") >= 0) ? (currentSiteSetting.SiteUrl + "/QRTakeCode.aspx?takeCode=" + text2) : ("http://" + currentSiteSetting.SiteUrl + "/QRTakeCode.aspx?takeCode=" + text2));
						Messenger.OrderConfirmTakeOnStore(orderInfo, user, storeById, empty2);
					}
					else
					{
						Messenger.OrderConfirmTakeOnStore(orderInfo, user, storeById, "");
					}
					base.ReturnResult(context, true, empty, 0, true);
				}
				return;
			}
			throw new HidistroAshxException(empty);
		}

		private void FinishTrade(HttpContext context)
		{
			SiteSettings currentSiteSetting = base.CurrentSiteSetting;
			string text = context.Request.Form["orderId"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			if (!orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
			{
				return;
			}
			if (OrderHelper.ConfirmOrderFinish(orderInfo))
			{
				base.ReturnResult(context, true, "成功的完成了该订单", 0, true);
				return;
			}
			throw new HidistroAshxException("完成订单失败");
		}

		private void ConfirmPayOrder(HttpContext context)
		{
			SiteSettings currentSiteSetting = base.CurrentSiteSetting;
			string text = context.Request.Form["orderId"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				throw new HidistroAshxException("异常的参数：订单编号");
			}
			if (!orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
			{
				return;
			}
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.bankrequest");
			if (paymentMode != null)
			{
				orderInfo.Gateway = paymentMode.Gateway;
				orderInfo.PaymentType = paymentMode.Name;
				orderInfo.PaymentTypeId = paymentMode.ModeId;
			}
			else
			{
				orderInfo.Gateway = "hishop.plugins.payment.bankrequest";
				orderInfo.PaymentType = "线下支付";
				orderInfo.PaymentTypeId = 0;
			}
			orderInfo.IsStoreCollect = true;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (orderInfo.CountDownBuyId > 0)
			{
				string empty = string.Empty;
				foreach (KeyValuePair<string, LineItemInfo> lineItem in orderInfo.LineItems)
				{
					CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(lineItem.Value.ProductId, orderInfo.CountDownBuyId, lineItem.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, out empty, orderInfo.StoreId);
					if (countDownInfo == null)
					{
						throw new HidistroAshxException(empty);
					}
				}
			}
			if (orderInfo.GroupBuyId > 0)
			{
				GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(orderInfo.GroupBuyId);
				if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
				{
					throw new HidistroAshxException("当前的订单为团购订单，此团购活动已结束，所以不能支付");
				}
				num2 = PromoteHelper.GetOrderCount(orderInfo.GroupBuyId);
				num = groupBuy.MaxCount;
				num3 = orderInfo.GetGroupBuyOerderNumber();
				if (num < num2 + num3)
				{
					throw new HidistroAshxException("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付");
				}
			}
			if (orderInfo.PreSaleId > 0)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
				if (productPreSaleInfo == null)
				{
					throw new HidistroAshxException("预售活动不存在");
				}
				if (!orderInfo.DepositDate.HasValue && productPreSaleInfo.PreSaleEndDate < DateTime.Now)
				{
					throw new HidistroAshxException("预售活动已结束不能支付定金，所以不能确认收款");
				}
			}
			if (OrderHelper.ConfirmPay(orderInfo))
			{
				if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
				{
					PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
				}
				if (orderInfo.ParentOrderId == "-1")
				{
					OrderQuery orderQuery = new OrderQuery();
					orderQuery.ParentOrderId = orderInfo.OrderId;
					IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(orderInfo.UserId, orderQuery);
					foreach (OrderInfo item in listUserOrder)
					{
						OrderHelper.OrderConfirmPaySendMessage(item);
					}
				}
				else
				{
					OrderHelper.OrderConfirmPaySendMessage(orderInfo);
				}
				base.ReturnResult(context, true, "成功的确认了订单收款", 0, true);
				return;
			}
			throw new HidistroAshxException("确认订单收款失败");
		}
	}
}
