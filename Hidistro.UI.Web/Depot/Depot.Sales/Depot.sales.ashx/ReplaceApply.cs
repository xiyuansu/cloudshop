using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Depot.sales.ashx
{
	public class ReplaceApply : StoreAdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "exporttoexcel")
				{
					this.ExportToExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void ExportToExcel(HttpContext context)
		{
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			replaceApplyQuery.ReplaceIds = context.Request["Ids"].ToNullString();
			replaceApplyQuery.SupplierId = 0;
			replaceApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			replaceApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			replaceApplyQuery.StoreId = base.CurrentManager.StoreId;
			replaceApplyQuery.SortBy = "ApplyForTime";
			replaceApplyQuery.SortOrder = SortAction.Desc;
			IList<ReplaceInfo> replaceApplysNoPage = OrderHelper.GetReplaceApplysNoPage(replaceApplyQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>编号</th>");
			stringBuilder.Append("<th>会员名</th>");
			stringBuilder.Append("<th>订单编号</th>");
			stringBuilder.Append("<th>申请时间</th>");
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
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReplaceReason, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ShopName + "(" + item.Quantity + ")", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.HandleStatus, 0), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(this.GetHandleTime(item), true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "ReplaceApplys" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		private void GetList(HttpContext context)
		{
			int pageIndex = 1;
			int pageSize = 10;
			string empty = string.Empty;
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			if (!string.IsNullOrEmpty(context.Request["OrderId"]))
			{
				replaceApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			}
			empty = context.Request["HandleStatus"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				int value = base.GetIntParam(context, "HandleStatus", false).Value;
				if (value > -1)
				{
					replaceApplyQuery.HandleStatus = value;
				}
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
			replaceApplyQuery.PageIndex = pageIndex;
			replaceApplyQuery.PageSize = pageSize;
			replaceApplyQuery.StoreId = base.CurrentManager.StoreId;
			replaceApplyQuery.SupplierId = 0;
			replaceApplyQuery.SortBy = "ApplyForTime";
			replaceApplyQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<ReplaceInfo> dataList = this.GetDataList(replaceApplyQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<ReplaceInfo> GetDataList(ReplaceApplyQuery query)
		{
			DataGridViewModel<ReplaceInfo> dataGridViewModel = new DataGridViewModel<ReplaceInfo>();
			PageModel<ReplaceInfo> replaceApplys = OrderHelper.GetReplaceApplys(query);
			dataGridViewModel.rows = replaceApplys.Models.ToList();
			dataGridViewModel.total = replaceApplys.Total;
			foreach (ReplaceInfo row in dataGridViewModel.rows)
			{
				row.ReplaceStatusStr = this.GetReplaceStatus(row.HandleStatus);
				row.handleTimeStr = this.GetHandleTime(row);
				row.OperText = this.GetOperText(row.HandleStatus);
			}
			return dataGridViewModel;
		}

		private string GetHandleTime(ReplaceInfo model)
		{
			string result = "";
			DateTime value;
			if (model.HandleStatus == ReplaceStatus.MerchantsAgreed)
			{
				object obj;
				if (!model.AgreedOrRefusedTime.HasValue)
				{
					obj = "";
				}
				else
				{
					value = model.AgreedOrRefusedTime.Value;
					obj = value.ToString("yyyy-MM-dd HH:mm:ss");
				}
				result = (string)obj;
			}
			else if (model.HandleStatus == ReplaceStatus.UserDelivery)
			{
				object obj2;
				if (!model.UserSendGoodsTime.HasValue)
				{
					obj2 = "";
				}
				else
				{
					value = model.UserSendGoodsTime.Value;
					obj2 = value.ToString("yyyy-MM-dd HH:mm:ss");
				}
				result = (string)obj2;
			}
			else if (model.HandleStatus == ReplaceStatus.MerchantsDelivery)
			{
				object obj3;
				if (!model.MerchantsConfirmGoodsTime.HasValue)
				{
					obj3 = "";
				}
				else
				{
					value = model.MerchantsConfirmGoodsTime.Value;
					obj3 = value.ToString("yyyy-MM-dd HH:mm:ss");
				}
				result = (string)obj3;
			}
			else if (model.HandleStatus == ReplaceStatus.Replaced)
			{
				object obj4;
				if (!model.UserConfirmGoodsTime.HasValue)
				{
					obj4 = "";
				}
				else
				{
					value = model.UserConfirmGoodsTime.Value;
					obj4 = value.ToString("yyyy-MM-dd HH:mm:ss");
				}
				result = (string)obj4;
			}
			return result;
		}

		public string GetOperText(ReplaceStatus status)
		{
			string result = "处理";
			if (status == ReplaceStatus.Refused || status == ReplaceStatus.Replaced || status == ReplaceStatus.MerchantsDelivery || status == ReplaceStatus.MerchantsAgreed)
			{
				result = "详情";
			}
			return result;
		}

		private string GetReplaceStatus(ReplaceStatus Status)
		{
			string result = string.Empty;
			foreach (ReplaceStatus value in Enum.GetValues(typeof(ReplaceStatus)))
			{
				if (value == Status)
				{
					result = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					break;
				}
			}
			return result;
		}
	}
}
