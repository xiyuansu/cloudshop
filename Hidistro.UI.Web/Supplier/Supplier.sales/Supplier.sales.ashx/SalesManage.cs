using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Supplier.sales.ashx
{
	[AdministerCheck(true)]
	public class SalesManage : SupplierAdminHandler
	{
		private new ManagerInfo CurrentManager;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			this.CurrentManager = HiContext.Current.Manager;
			string text = context.Request["action"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string a = text;
			if (!(a == "getlistReturn"))
			{
				if (a == "getlistReplace")
				{
					this.GetlistReplace(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetListReturn(context);
		}

		private void GetlistReplace(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			int? nullable = null;
			string empty2 = string.Empty;
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			empty2 = context.Request["OrderId"];
			empty = context.Request["HandleStatus"];
			if (!string.IsNullOrWhiteSpace(empty) && !string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					nullable = int.Parse(empty);
					if (nullable > -1)
					{
						nullable = ((nullable == 0) ? new int?(4) : nullable);
					}
				}
				catch
				{
					nullable = null;
				}
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
			replaceApplyQuery.SupplierId = HiContext.Current.Manager.StoreId;
			replaceApplyQuery.PageIndex = num;
			replaceApplyQuery.PageSize = num2;
			replaceApplyQuery.OrderId = empty2;
			replaceApplyQuery.HandleStatus = nullable;
			DataGridViewModel<ReplaceInfo> replaceOrder = this.GetReplaceOrder(replaceApplyQuery);
			List<ReplaceInfo> list = new List<ReplaceInfo>();
			foreach (ReplaceInfo row in replaceOrder.rows)
			{
				row.ReplaceStatusStr = this.GetReplaceStatus((int)row.HandleStatus);
				row.handleTimeStr = this.GetHandleTime(row);
				row.OperText = this.GetOperText(row.HandleStatus);
				list.Add(row);
			}
			replaceOrder.rows = list;
			string s = JsonConvert.SerializeObject(replaceOrder);
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetListReturn(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			string empty = string.Empty;
			int? nullable = null;
			string empty2 = string.Empty;
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			empty2 = context.Request["OrderId"];
			empty = context.Request["HandleStatus"];
			if (!string.IsNullOrWhiteSpace(empty) && !string.IsNullOrWhiteSpace(empty))
			{
				try
				{
					nullable = int.Parse(empty);
					if (nullable > -1)
					{
						nullable = ((nullable == 0) ? new int?(4) : nullable);
					}
				}
				catch
				{
					nullable = null;
				}
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
			returnsApplyQuery.SupplierId = HiContext.Current.Manager.StoreId;
			returnsApplyQuery.PageIndex = num;
			returnsApplyQuery.PageSize = num2;
			returnsApplyQuery.OrderId = empty2;
			returnsApplyQuery.HandleStatus = nullable;
			DataGridViewModel<ReturnInfo> returnOrder = this.GetReturnOrder(returnsApplyQuery);
			List<ReturnInfo> list = new List<ReturnInfo>();
			foreach (ReturnInfo row in returnOrder.rows)
			{
				row.ReturnStatusStr = this.GetReturnStatus(true, (int)row.AfterSaleType, (int)row.HandleStatus);
				row.handleTimeStr = this.GetHandleTime(row);
				row.OperText = this.GetOperText(row.HandleStatus);
				list.Add(row);
			}
			returnOrder.rows = list;
			string s = base.SerializeObjectToJson(returnOrder);
			context.Response.Write(s);
			context.Response.End();
		}

		public DataGridViewModel<ReturnInfo> GetReturnOrder(ReturnsApplyQuery query)
		{
			DataGridViewModel<ReturnInfo> dataGridViewModel = new DataGridViewModel<ReturnInfo>();
			if (query != null)
			{
				PageModel<ReturnInfo> returnsApplys = OrderHelper.GetReturnsApplys(query);
				dataGridViewModel.rows = returnsApplys.Models.ToList();
				dataGridViewModel.total = returnsApplys.Total;
			}
			return dataGridViewModel;
		}

		public DataGridViewModel<ReplaceInfo> GetReplaceOrder(ReplaceApplyQuery query)
		{
			DataGridViewModel<ReplaceInfo> dataGridViewModel = new DataGridViewModel<ReplaceInfo>();
			if (query != null)
			{
				PageModel<ReplaceInfo> replaceApplys = OrderHelper.GetReplaceApplys(query);
				dataGridViewModel.rows = replaceApplys.Models.ToList();
				dataGridViewModel.total = replaceApplys.Total;
			}
			return dataGridViewModel;
		}

		private string GetReturnStatus(bool ShowInAdmin, int AfterSaleType, int Status)
		{
			string result = string.Empty;
			foreach (ReturnStatus value in Enum.GetValues(typeof(ReturnStatus)))
			{
				if (value == (ReturnStatus)Status)
				{
					result = ((ShowInAdmin || value != ReturnStatus.GetGoods) ? ((AfterSaleType != 3) ? EnumDescription.GetEnumDescription((Enum)(object)value, 0) : EnumDescription.GetEnumDescription((Enum)(object)value, 3)) : EnumDescription.GetEnumDescription((Enum)(object)ReturnStatus.Deliverying, 0));
					break;
				}
			}
			return result;
		}

		private string GetReplaceStatus(int Status)
		{
			string result = string.Empty;
			foreach (ReplaceStatus value in Enum.GetValues(typeof(ReplaceStatus)))
			{
				if (value == (ReplaceStatus)Status)
				{
					result = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					break;
				}
			}
			return result;
		}

		public string GetHandleTime(ReturnInfo model)
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

		public string GetHandleTime(ReplaceInfo model)
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

		public string GetOperText(ReturnStatus status)
		{
			return (status == ReturnStatus.Deliverying) ? "处理" : "详情";
		}

		public string GetOperText(ReplaceStatus status)
		{
			return (status == ReplaceStatus.UserDelivery) ? "处理" : "详情";
		}
	}
}
