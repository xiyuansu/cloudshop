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
	public class ReturnsApply : StoreAdminBaseHandler
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
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			returnsApplyQuery.ReturnIds = context.Request["Ids"].ToNullString();
			returnsApplyQuery.SupplierId = 0;
			returnsApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			returnsApplyQuery.HandleStatus = base.GetIntParam(context, "HandleStatus", true);
			returnsApplyQuery.StoreId = base.CurrentManager.StoreId;
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
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(EnumDescription.GetEnumDescription((Enum)(object)item.HandleStatus, 0), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ReturnReason, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ShopName + "(" + item.Quantity + ")", true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.RefundAmount, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(this.GetHandleTime(item), true));
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

		private void GetList(HttpContext context)
		{
			int pageIndex = 1;
			int pageSize = 10;
			string empty = string.Empty;
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(context.Request["OrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(context.Request["OrderId"]);
			}
			empty = context.Request["HandleStatus"];
			if (!string.IsNullOrWhiteSpace(empty))
			{
				int value = base.GetIntParam(context, "HandleStatus", false).Value;
				if (value > -1)
				{
					returnsApplyQuery.HandleStatus = value;
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
			returnsApplyQuery.PageIndex = pageIndex;
			returnsApplyQuery.PageSize = pageSize;
			returnsApplyQuery.StoreId = base.CurrentManager.StoreId;
			returnsApplyQuery.SupplierId = 0;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<ReturnInfo> dataList = this.GetDataList(returnsApplyQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<ReturnInfo> GetDataList(ReturnsApplyQuery query)
		{
			DataGridViewModel<ReturnInfo> dataGridViewModel = new DataGridViewModel<ReturnInfo>();
			PageModel<ReturnInfo> returnsApplys = OrderHelper.GetReturnsApplys(query);
			dataGridViewModel.rows = returnsApplys.Models.ToList();
			dataGridViewModel.total = returnsApplys.Total;
			foreach (ReturnInfo row in dataGridViewModel.rows)
			{
				row.ReturnStatusStr = this.GetReturnStatus(true, row.AfterSaleType, row.HandleStatus);
				row.handleTimeStr = this.GetHandleTime(row);
				row.OperText = this.GetOperText(row.HandleStatus);
			}
			return dataGridViewModel;
		}

		private string GetHandleTime(ReturnInfo model)
		{
			string result = "";
			DateTime value;
			if (model.HandleStatus == ReturnStatus.MerchantsAgreed)
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
			else if (model.HandleStatus == ReturnStatus.Deliverying)
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
			else if (model.HandleStatus == ReturnStatus.GetGoods)
			{
				object obj3;
				if (!model.ConfirmGoodsTime.HasValue)
				{
					obj3 = "";
				}
				else
				{
					value = model.ConfirmGoodsTime.Value;
					obj3 = value.ToString("yyyy-MM-dd HH:mm:ss");
				}
				result = (string)obj3;
			}
			else if (model.HandleStatus == ReturnStatus.Returned)
			{
				object obj4;
				if (model.FinishTime.HasValue && !(model.FinishTime.Value == DateTime.MinValue))
				{
					value = model.FinishTime.Value;
					obj4 = value.ToString("yyyy-MM-dd HH:mm:ss");
				}
				else
				{
					obj4 = "";
				}
				result = (string)obj4;
			}
			return result;
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

		private string GetReturnStatus(bool ShowInAdmin, AfterSaleTypes AfterSaleType, ReturnStatus Status)
		{
			string result = string.Empty;
			foreach (ReturnStatus value in Enum.GetValues(typeof(ReturnStatus)))
			{
				if (value == Status)
				{
					result = ((ShowInAdmin || value != ReturnStatus.GetGoods) ? ((AfterSaleType != AfterSaleTypes.OnlyRefund) ? EnumDescription.GetEnumDescription((Enum)(object)value, 0) : EnumDescription.GetEnumDescription((Enum)(object)value, 3)) : EnumDescription.GetEnumDescription((Enum)(object)ReturnStatus.Deliverying, 0));
					break;
				}
			}
			return result;
		}
	}
}
