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
	public class ReplaceApply : AdminBaseHandler
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
			ReplaceApplyQuery dataQuery = this.GetDataQuery(context);
			dataQuery.ReplaceIds = context.Request["Ids"].ToNullString();
			IList<ReplaceInfo> replaceApplysNoPage = OrderHelper.GetReplaceApplysNoPage(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>编号</th>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>申请时间</th>");
			stringBuilder.Append("<th>供应商</th>");
			stringBuilder.Append("<th>换货原因</th>");
			stringBuilder.Append("<th>换货商品</th>");
			stringBuilder.Append("<th>处理状态</th>");
			stringBuilder.Append("<th>处理时间</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (ReplaceInfo item in replaceApplysNoPage)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReplaceId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.OrderId, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ApplyForTime, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD((item.SupplierId > 0) ? SupplierHelper.GetSupplierName(item.SupplierId) : "平台店", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReplaceReason, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ShopName + "(" + item.Quantity + ")", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.HandleStatus, 0), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(this.GetHandleTime(item), false));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "ReplaceApplys" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		public void GetList(HttpContext context)
		{
			ReplaceApplyQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private ReplaceApplyQuery GetDataQuery(HttpContext context)
		{
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			replaceApplyQuery.OrderId = base.GetParameter(context, "OrderId", true);
			replaceApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			int? intParam = base.GetIntParam(context, "SupplierId", true);
			if (intParam.HasValue)
			{
				replaceApplyQuery.SupplierId = intParam.Value;
			}
			else
			{
				replaceApplyQuery.SupplierId = -1;
			}
			replaceApplyQuery.PageIndex = base.CurrentPageIndex;
			replaceApplyQuery.PageSize = base.CurrentPageSize;
			replaceApplyQuery.SortBy = "ApplyForTime";
			replaceApplyQuery.SortOrder = SortAction.Desc;
			return replaceApplyQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ReplaceApplyQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<ReplaceInfo> replaceApplys = OrderHelper.GetReplaceApplys(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = replaceApplys.Total;
				foreach (ReplaceInfo model in replaceApplys.Models)
				{
					model.OperText = this.GetOperText(model.HandleStatus);
					model.ReplaceStatusStr = this.GetStatusText(model.HandleStatus);
					Dictionary<string, object> dictionary = model.ToDictionary();
					dictionary.Add("HandleTime", this.GetHandleTime(model));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		private string GetStatusText(ReplaceStatus Status)
		{
			string text = "";
			return EnumDescription.GetEnumDescription((Enum)(object)Status, 0);
		}

		private string GetOperText(ReplaceStatus status)
		{
			string result = "处理";
			if (status == ReplaceStatus.Refused || status == ReplaceStatus.Replaced || status == ReplaceStatus.MerchantsDelivery || status == ReplaceStatus.MerchantsAgreed)
			{
				result = "详情";
			}
			return result;
		}

		public DateTime? GetHandleTime(ReplaceInfo model)
		{
			DateTime? result = null;
			switch (model.HandleStatus)
			{
			case ReplaceStatus.MerchantsAgreed:
				return model.AgreedOrRefusedTime;
			case ReplaceStatus.UserDelivery:
				return model.UserSendGoodsTime;
			case ReplaceStatus.MerchantsDelivery:
				return model.MerchantsConfirmGoodsTime;
			case ReplaceStatus.Replaced:
				return model.UserConfirmGoodsTime;
			default:
				return result;
			}
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("请选要删除的换货申请单");
			}
			string[] array = text.Split(',');
			int num = 0;
			int num2 = array.Count();
			if (OrderHelper.DelReplaceApply(array, out num))
			{
				if (num2 == 1)
				{
					base.ReturnSuccessResult(context, "删除换货申请单成功", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, $"成功删除了{num}个换货申请单,待处理的申请不能删除", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除退换货申请单失败");
		}
	}
}
