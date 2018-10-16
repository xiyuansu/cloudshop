using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	public class OrderGifts : AdminBaseHandler, IRequiresSessionState
	{
		private string orderId;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			this.orderId = base.GetParameter(context, "id", true);
			switch (base.action)
			{
			case "getgiftslist":
				this.GetGiftsList(context);
				break;
			case "getaddedlist":
				this.GetAddedList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "clear":
				this.Clear(context);
				break;
			case "add":
				this.Add(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetGiftsList(HttpContext context)
		{
			GiftQuery giftsQuery = this.GetGiftsQuery(context);
			DataGridViewModel<Dictionary<string, object>> giftses = this.GetGiftses(giftsQuery);
			string s = base.SerializeObjectToJson(giftses);
			context.Response.Write(s);
			context.Response.End();
		}

		private GiftQuery GetGiftsQuery(HttpContext context)
		{
			GiftQuery giftQuery = new GiftQuery();
			giftQuery.Name = base.GetParameter(context, "Name", true);
			giftQuery.Page.PageSize = base.CurrentPageSize;
			giftQuery.Page.PageIndex = base.CurrentPageIndex;
			Globals.EntityCoding(giftQuery, true);
			return giftQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetGiftses(GiftQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult gifts = OrderHelper.GetGifts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(gifts.Data);
				dataGridViewModel.total = gifts.TotalRecords;
			}
			return dataGridViewModel;
		}

		private void GetAddedList(HttpContext context)
		{
			OrderGiftQuery addedQuery = this.GetAddedQuery(context);
			DataGridViewModel<Dictionary<string, object>> addedGifts = this.GetAddedGifts(addedQuery);
			string s = base.SerializeObjectToJson(addedGifts);
			context.Response.Write(s);
			context.Response.End();
		}

		private OrderGiftQuery GetAddedQuery(HttpContext context)
		{
			OrderGiftQuery orderGiftQuery = new OrderGiftQuery();
			orderGiftQuery.PageSize = base.CurrentPageSize;
			orderGiftQuery.PageIndex = base.CurrentPageIndex;
			orderGiftQuery.OrderId = this.orderId;
			Globals.EntityCoding(orderGiftQuery, true);
			return orderGiftQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetAddedGifts(OrderGiftQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null && !string.IsNullOrWhiteSpace(this.orderId))
			{
				DbQueryResult orderGifts = OrderHelper.GetOrderGifts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(orderGifts.Data);
				dataGridViewModel.total = orderGifts.TotalRecords;
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "giftId", false).Value;
			if (string.IsNullOrWhiteSpace(this.orderId))
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (!orderInfo.CheckAction(OrderActions.MASTER_SELLER_MODIFY_GIFTS))
			{
				throw new HidistroAshxException("当前订单状态没有订单礼品操作");
			}
			if (!OrderHelper.DeleteOrderGift(orderInfo, value))
			{
				throw new HidistroAshxException("删除订单礼品失败");
			}
			base.ReturnSuccessResult(context, "操作成功！", 0, true);
		}

		private void Clear(HttpContext context)
		{
			if (string.IsNullOrWhiteSpace(this.orderId))
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (!orderInfo.CheckAction(OrderActions.MASTER_SELLER_MODIFY_GIFTS))
			{
				throw new HidistroAshxException("当前订单状态没有订单礼品操作");
			}
			if (!OrderHelper.ClearOrderGifts(orderInfo))
			{
				throw new HidistroAshxException("清空礼品列表失败");
			}
			base.ReturnSuccessResult(context, "操作成功！", 0, true);
		}

		private void Add(HttpContext context)
		{
			int value = base.GetIntParam(context, "giftId", false).Value;
			int value2 = base.GetIntParam(context, "quantity", false).Value;
			if (string.IsNullOrWhiteSpace(this.orderId))
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			if (value2 <= 0)
			{
				throw new HidistroAshxException("礼品数量填写错误");
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (!orderInfo.CheckAction(OrderActions.MASTER_SELLER_MODIFY_GIFTS))
			{
				throw new HidistroAshxException("当前订单状态没有订单礼品操作");
			}
			GiftInfo giftDetails = GiftHelper.GetGiftDetails(value);
			if (giftDetails == null)
			{
				throw new HidistroAshxException("错误的数据编号");
			}
			if (!OrderHelper.AddOrderGift(orderInfo, giftDetails, value2, 15.GetHashCode()))
			{
				throw new HidistroAshxException("添加订单礼品失败,可能礼品己存在");
			}
			base.ReturnSuccessResult(context, "操作成功！", 0, true);
		}
	}
}
