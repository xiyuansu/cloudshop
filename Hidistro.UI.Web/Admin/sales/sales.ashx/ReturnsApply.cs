using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.App_Code;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	public class ReturnsApply : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "exporttoexcel":
				this.ExportToExcel(context);
				break;
			case "batchdealexception":
				this.BatchDealException(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void BatchDealException(HttpContext context)
		{
			string idList = context.Request["ReturnIds"].ToNullString();
			idList = Globals.GetSafeIDList(idList, ',', true);
			if (string.IsNullOrEmpty(idList))
			{
				throw new HidistroAshxException("请选择要退款/退货的ID");
			}
			IList<ReturnInfo> list = OrderHelper.GetReturnListOfReturnIds(idList);
			if (list != null && list.Count > 0)
			{
				string Operator = HiContext.Current.Manager.UserName;
				Task.Factory.StartNew(delegate
				{
					int num = 0;
					foreach (ReturnInfo item in list)
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(item.OrderId);
						if (orderInfo != null && RefundHelper.IsBackReturn(orderInfo.Gateway) && item.RefundType == RefundTypes.BackReturn && !TradeHelper.AlipayCanRefundGateway.Contains(orderInfo.Gateway))
						{
							if (item.AfterSaleType == AfterSaleTypes.OnlyRefund)
							{
								if (item.HandleStatus == ReturnStatus.Applied)
								{
									goto IL_00cc;
								}
							}
							else if (item.HandleStatus == ReturnStatus.GetGoods || item.HandleStatus == ReturnStatus.Deliverying)
							{
								goto IL_00cc;
							}
						}
						continue;
						IL_00cc:
						MemberInfo user = Users.GetUser(orderInfo.UserId);
						string text = RefundHelper.SendRefundRequest(orderInfo, item.RefundAmount, item.RefundOrderId, true);
						if (text == "")
						{
							if (item.AfterSaleType == AfterSaleTypes.ReturnAndRefund)
							{
								if (OrderHelper.CheckReturn(item, orderInfo, Operator, item.RefundAmount, item.AdminRemark, true, false))
								{
									Messenger.OrderRefund(user, orderInfo, item.SkuId);
								}
							}
							else if (OrderHelper.AgreedReturns(item.ReturnId, item.RefundAmount, item.AdminRemark, orderInfo, item.SkuId, item.AdminShipAddress, item.AdminShipTo, item.AdminShipAddress, true, false))
							{
								VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, item.SkuId, EnumPushOrderAction.OrderReturnConfirm);
								Messenger.OrderRefund(user, orderInfo, item.SkuId);
							}
							num++;
						}
						else
						{
							TradeHelper.SaveRefundErr(item.ReturnId, text, false);
						}
						Thread.Sleep(5000);
					}
					base.ReturnSuccessResult(context, "退款自动处理提交成功,请稍后刷新页面", 0, true);
				});
				return;
			}
			throw new HidistroAshxException("未找到退款记录");
		}

		public void ExportToExcel(HttpContext context)
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			returnsApplyQuery.ReturnIds = context.Request["Ids"].ToNullString();
			returnsApplyQuery.SupplierId = 0;
			returnsApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			returnsApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			IList<ReturnInfo> returnApplysNoPage = OrderHelper.GetReturnApplysNoPage(returnsApplyQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>编号</th>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>申请时间</th>");
			stringBuilder.Append("<th>匹配门店</th>");
			stringBuilder.Append("<th>处理状态</th>");
			stringBuilder.Append("<th>退款/退货原因</th>");
			stringBuilder.Append("<th>退款商品</th>");
			stringBuilder.Append("<th>退款金额</th>");
			stringBuilder.Append("<th>处理时间</th>");
			stringBuilder.Append("<th>退款途径</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (ReturnInfo item in returnApplysNoPage)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReturnId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.OrderId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ApplyForTime, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD((item.StoreId.HasValue && item.StoreId.Value > 0) ? DepotHelper.GetStoreNameByStoreId(item.StoreId.Value) : "平台店", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.HandleStatus, 0), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReturnReason, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ShopName + "(" + item.Quantity + ")", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RefundAmount, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(this.GetHandleTime(item), false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.RefundType, 0), true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "ReturnApplys" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		public void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
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
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			returnsApplyQuery.SupplierId = 0;
			returnsApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			returnsApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			returnsApplyQuery.PageIndex = num;
			returnsApplyQuery.PageSize = num2;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(returnsApplyQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ReturnsApplyQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<ReturnInfo> returnsApplys = OrderHelper.GetReturnsApplys(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = returnsApplys.Total;
				foreach (ReturnInfo model in returnsApplys.Models)
				{
					model.OperText = this.GetOperText(model.HandleStatus);
					Dictionary<string, object> dictionary = model.ToDictionary();
					dictionary.Add("HandleTime", this.GetHandleTime(model));
					dictionary.Add("StatusStr", this.GetStatusText(model.HandleStatus));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		private string GetStatusText(ReturnStatus Status)
		{
			string text = "";
			return EnumDescription.GetEnumDescription((Enum)(object)Status, 0);
		}

		private string GetOperText(ReturnStatus status)
		{
			string result = "处理";
			if (status == ReturnStatus.Refused || status == ReturnStatus.Returned || status == ReturnStatus.MerchantsAgreed)
			{
				result = "详情";
			}
			return result;
		}

		public DateTime? GetHandleTime(ReturnInfo model)
		{
			DateTime? result = null;
			switch (model.HandleStatus)
			{
			case ReturnStatus.MerchantsAgreed:
				return model.AgreedOrRefusedTime;
			case ReturnStatus.Deliverying:
				return model.UserSendGoodsTime;
			case ReturnStatus.GetGoods:
				return model.ConfirmGoodsTime;
			case ReturnStatus.Returned:
				return model.FinishTime;
			default:
				return result;
			}
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("请选要删除的退货申请单");
			}
			string[] array = text.Split(',');
			int num = 0;
			int num2 = array.Count();
			if (OrderHelper.DelReturnsApply(array, out num))
			{
				num++;
			}
			if (num > 0)
			{
				if (num2 == 1)
				{
					base.ReturnSuccessResult(context, "删除退货申请单成功", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, $"成功删除了{num}个退货申请单", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除退货申请单失败");
		}
	}
}
