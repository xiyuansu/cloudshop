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
	public class RefundApply : AdminBaseHandler
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
			string idList = context.Request["RefundIds"].ToNullString();
			idList = Globals.GetSafeIDList(idList, ',', true);
			if (string.IsNullOrEmpty(idList))
			{
				throw new HidistroAshxException("请选择要退款的ID");
			}
			IList<RefundInfo> list = OrderHelper.GetRefundListOfRefundIds(idList);
			if (list != null && list.Count > 0)
			{
				string Operator = HiContext.Current.Manager.UserName;
				Task.Factory.StartNew(delegate
				{
					int num = 0;
					foreach (RefundInfo item in list)
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(item.OrderId);
						if (orderInfo != null && RefundHelper.IsBackReturn(orderInfo.Gateway) && item.RefundType == RefundTypes.BackReturn && !TradeHelper.AlipayCanRefundGateway.Contains(orderInfo.Gateway) && item.HandleStatus == RefundStatus.Applied)
						{
							MemberInfo user = Users.GetUser(orderInfo.UserId);
							string text = RefundHelper.SendRefundRequest(orderInfo, item.RefundAmount, item.RefundOrderId, true);
							if (text == "")
							{
								if (OrderHelper.CheckRefund(orderInfo, item, item.RefundAmount, Operator, item.AdminRemark, true, false))
								{
									VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
									Messenger.OrderRefund(user, orderInfo, "");
								}
								num++;
							}
							else
							{
								TradeHelper.SaveRefundErr(item.RefundId, text, true);
							}
							Thread.Sleep(5000);
						}
					}
					base.ReturnSuccessResult(context, "退款自动处理提交成功,请稍后刷新页面", 0, true);
				});
				return;
			}
			throw new HidistroAshxException("未找到退款记录");
		}

		public void ExportToExcel(HttpContext context)
		{
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			refundApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			refundApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			refundApplyQuery.RefundIds = context.Request["Ids"].ToNullString();
			refundApplyQuery.SortBy = "ApplyForTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			IList<RefundModel> refundApplysNoPage = OrderHelper.GetRefundApplysNoPage(refundApplyQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>售后编号</th>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>申请时间</th>");
			stringBuilder.Append("<th>匹配门店</th>");
			stringBuilder.Append("<th>处理状态</th>");
			stringBuilder.Append("<th>退款原因</th>");
			stringBuilder.Append("<th>退款金额</th>");
			stringBuilder.Append("<th>处理时间</th>");
			stringBuilder.Append("<th>退款途径</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (RefundModel item in refundApplysNoPage)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RefundId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.OrderId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ApplyForTime, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD((item.StoreId.HasValue && item.StoreId.Value > 0) ? DepotHelper.GetStoreNameByStoreId(item.StoreId.Value) : "平台店", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.HandleStatus, 0), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RefundReason, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RefundAmount, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.GetDealTime, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.RefundType, 0), true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "RefundApplys" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
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
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			refundApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			refundApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			refundApplyQuery.PageIndex = num;
			refundApplyQuery.PageSize = num2;
			refundApplyQuery.SortBy = "ApplyForTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(refundApplyQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(RefundApplyQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<RefundModel> refundApplys = OrderHelper.GetRefundApplys(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = refundApplys.Total;
				foreach (RefundModel model in refundApplys.Models)
				{
					Dictionary<string, object> dictionary = model.ToDictionary();
					dictionary.Add("HandleTime", this.GetHandleTime(model));
					dictionary.Add("OperText", this.GetOperText(model.HandleStatus));
					dictionary.Add("StatusStr", this.GetStatusText(model.HandleStatus, model.ExceptionInfo.ToNullString()));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		private string GetStatusText(RefundStatus Status, string exceptionInfo)
		{
			string text = "";
			if (!string.IsNullOrEmpty(exceptionInfo) && Status == RefundStatus.Applied)
			{
				text = "异常";
			}
			return EnumDescription.GetEnumDescription((Enum)(object)Status, 0);
		}

		private string GetOperText(RefundStatus status)
		{
			string result = "处理";
			if (status == RefundStatus.Refused || status == RefundStatus.Refunded)
			{
				result = "详情";
			}
			return result;
		}

		public DateTime? GetHandleTime(RefundModel model)
		{
			DateTime? result = null;
			if (model.HandleStatus == RefundStatus.Refunded || model.HandleStatus == RefundStatus.Refused)
			{
				return model.FinishTime.HasValue ? model.FinishTime : model.AgreedOrRefusedTime;
			}
			return result;
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("请选要删除的退款申请单");
			}
			string[] array = text.Split(',');
			int num = 0;
			int num2 = array.Count();
			if (OrderHelper.DelRefundApply(array, out num))
			{
				num++;
			}
			if (num > 0)
			{
				if (num2 == 1)
				{
					base.ReturnSuccessResult(context, "删除退款申请单成功", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, $"成功删除了{num}个退款申请单", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除退款申请单失败");
		}
	}
}
