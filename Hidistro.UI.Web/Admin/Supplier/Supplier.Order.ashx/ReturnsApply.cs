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
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void ExportToExcel(HttpContext context)
		{
			ReturnsApplyQuery dataQuery = this.GetDataQuery(context);
			dataQuery.ReturnIds = context.Request["Ids"].ToNullString();
			IList<ReturnInfo> returnApplysNoPage = OrderHelper.GetReturnApplysNoPage(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>编号</th>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>申请时间</th>");
			stringBuilder.Append("<th>供应商</th>");
			stringBuilder.Append("<th>处理状态</th>");
			stringBuilder.Append("<th>退款/退货原因</th>");
			stringBuilder.Append("<th>退款商品</th>");
			stringBuilder.Append("<th>退款金额</th>");
			stringBuilder.Append("<th>处理时间</th>");
			stringBuilder.Append("<th>退款途径</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			DateTime dateTime;
			foreach (ReturnInfo item in returnApplysNoPage)
			{
				DateTime? handleTime = this.GetHandleTime(item);
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReturnId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.OrderId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ApplyForTime, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD((item.SupplierId > 0) ? SupplierHelper.GetSupplierName(item.SupplierId) : "平台店", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.HandleStatus, 0), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReturnReason, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ShopName + "(" + item.Quantity + ")", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RefundAmount, false));
				StringBuilder stringBuilder3 = stringBuilder2;
				object argFields;
				if (!handleTime.HasValue)
				{
					dateTime = handleTime.Value;
					argFields = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				}
				else
				{
					argFields = "";
				}
				stringBuilder3.Append(ExcelHelper.GetXLSFieldsTD(argFields, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.RefundType, 0), true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			HttpResponse response = context.Response;
			StringBuilder stringBuilder4 = stringWriter.GetStringBuilder();
			dateTime = DateTime.Now;
			DownloadHelper.DownloadFile(response, stringBuilder4, "ReturnApplys" + dateTime.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		public void GetList(HttpContext context)
		{
			ReturnsApplyQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private ReturnsApplyQuery GetDataQuery(HttpContext context)
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			returnsApplyQuery.OrderId = base.GetParameter(context, "OrderId", true);
			returnsApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			int? intParam = base.GetIntParam(context, "SupplierId", true);
			if (intParam.HasValue)
			{
				returnsApplyQuery.SupplierId = intParam.Value;
			}
			else
			{
				returnsApplyQuery.SupplierId = -1;
			}
			returnsApplyQuery.PageIndex = base.CurrentPageIndex;
			returnsApplyQuery.PageSize = base.CurrentPageSize;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			return returnsApplyQuery;
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
				if (num2 == 1)
				{
					base.ReturnSuccessResult(context, "删除退货申请单成功", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, $"成功删除了{num}个退货申请单,待处理的申请不能删除", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除退退货申请单失败");
		}
	}
}
