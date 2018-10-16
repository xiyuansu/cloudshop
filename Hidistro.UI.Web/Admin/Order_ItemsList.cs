using Hidistro.Context;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class Order_ItemsList : UserControl
	{
		public int UserStoreId = 0;

		private OrderInfo order;

		private bool _ShowAllItem = true;

		protected Repeater dlstOrderItems;

		protected Repeater grdOrderGift;

		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		public bool ShowAllItem
		{
			get
			{
				return this._ShowAllItem;
			}
			set
			{
				this._ShowAllItem = value;
			}
		}

		public bool ShowCostPrice
		{
			get;
			set;
		}

		public bool IsPrize
		{
			get
			{
				return this.Order.UserAwardRecordsId > 0;
			}
		}

		private void dlstOrderItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				string key = (string)DataBinder.Eval(e.Item.DataItem, "SkuId");
				if (this.order.LineItems.ContainsKey(key))
				{
					LineItemInfo lineItemInfo = this.order.LineItems[key];
					string statusText = lineItemInfo.StatusText;
					statusText = ((lineItemInfo.Status == LineItemStatus.Normal) ? "" : TradeHelper.GetOrderItemSatusText(lineItemInfo.Status));
					Literal literal = (Literal)e.Item.FindControl("litStatusText");
					if (literal != null)
					{
						literal.Text = statusText;
					}
					if (lineItemInfo.Status != 0)
					{
						if (!base.Request.RawUrl.ToLower().Contains("supplier"))
						{
							if (lineItemInfo.ReturnInfo != null)
							{
								literal.Text = "<a href=\"ReturnApplyDetail?ReturnId=" + lineItemInfo.ReturnInfo.ReturnId + "\">" + statusText + "</a>";
							}
							if (lineItemInfo.ReplaceInfo != null)
							{
								literal.Text = "<a href=\"ReplaceApplyDetail?ReplaceId=" + lineItemInfo.ReplaceInfo.ReplaceId + "\">" + statusText + "</a>";
							}
						}
						else
						{
							if (lineItemInfo.ReturnInfo != null)
							{
								literal.Text = "<a href=\"Order/ReturnApplyDetail?ReturnId=" + lineItemInfo.ReturnInfo.ReturnId + "\">" + statusText + "</a>";
							}
							if (lineItemInfo.ReplaceInfo != null)
							{
								literal.Text = "<a href=\"Order/ReplaceApplyDetail?ReplaceId=" + lineItemInfo.ReplaceInfo.ReplaceId + "\">" + statusText + "</a>";
							}
						}
					}
					OrderStatus orderStatus = this.order.OrderStatus;
					DateTime finishDate = this.order.FinishDate;
					string gateway = this.order.Gateway;
					int storeId = this.order.StoreId;
					bool isStoreCollect = this.order.IsStoreCollect;
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			if (this.ShowAllItem)
			{
				this.dlstOrderItems.ItemDataBound += this.dlstOrderItems_ItemDataBound;
			}
			if (this.order.LineItems.Count > 0)
			{
				IDictionary<string, LineItemInfo> dictionary = new Dictionary<string, LineItemInfo>();
				foreach (string key in this.order.LineItems.Keys)
				{
					LineItemInfo lineItemInfo = this.order.LineItems[key];
					if (!this.ShowAllItem)
					{
						if (lineItemInfo.Status != LineItemStatus.Returned && lineItemInfo.Status != LineItemStatus.Refunded)
						{
							dictionary.Add(key, this.order.LineItems[key]);
						}
					}
					else
					{
						dictionary.Add(key, this.order.LineItems[key]);
					}
				}
				this.dlstOrderItems.DataSource = dictionary.Values;
				this.dlstOrderItems.DataBind();
			}
			else
			{
				this.dlstOrderItems.Visible = false;
			}
			if (this.order.Gifts.Count == 0)
			{
				this.grdOrderGift.Visible = false;
			}
			else
			{
				this.grdOrderGift.DataSource = this.order.Gifts;
				this.grdOrderGift.DataBind();
			}
		}
	}
}
