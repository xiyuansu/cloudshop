using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier.Order.ashx
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
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void ExportToExcel(HttpContext context)
		{
			RefundApplyQuery dataQuery = this.GetDataQuery(context);
			dataQuery.RefundIds = context.Request["Ids"].ToNullString();
			IList<RefundModel> refundApplysNoPage = OrderHelper.GetRefundApplysNoPage(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>编号</th>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>申请时间</th>");
			stringBuilder.Append("<th>供应商</th>");
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
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD((item.SupplierId > 0) ? SupplierHelper.GetSupplierName(item.SupplierId) : "平台店", true));
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
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "SupplierRefundApplys" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		public void GetList(HttpContext context)
		{
			RefundApplyQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private RefundApplyQuery GetDataQuery(HttpContext context)
		{
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			refundApplyQuery.OrderId = base.GetParameter(context, "OrderId", true);
			refundApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			int? intParam = base.GetIntParam(context, "SupplierId", true);
			if (intParam.HasValue)
			{
				refundApplyQuery.SupplierId = intParam.Value;
			}
			else
			{
				refundApplyQuery.SupplierId = -1;
			}
			refundApplyQuery.PageIndex = base.CurrentPageIndex;
			refundApplyQuery.PageSize = base.CurrentPageSize;
			refundApplyQuery.SortBy = "ApplyForTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			return refundApplyQuery;
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
					dictionary.Add("StatusStr", this.GetStatusText(model.HandleStatus));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		private string GetStatusText(RefundStatus Status)
		{
			string text = "";
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
				if (num2 == 1)
				{
					base.ReturnSuccessResult(context, "删除退款申请单成功", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, $"成功删除了{num}个退款申请单,待处理的申请不能删除", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除退款申请单失败");
		}
	}
}
