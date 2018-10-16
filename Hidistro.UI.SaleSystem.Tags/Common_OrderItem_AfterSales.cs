using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_OrderItem_AfterSales : WAPTemplatedWebControl
	{
		public delegate void DataBindEventHandler(object sender, RepeaterItemEventArgs e);

		public const string TagID = "Common_OrderItemAfterSales";

		private Repeater listOrderItems;

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		public bool IsUseInApp
		{
			get;
			set;
		}

		public OrderInfo order
		{
			get;
			set;
		}

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.listOrderItems.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.listOrderItems.DataSource = value;
			}
		}

		public SortAction SortOrder
		{
			get
			{
				return SortAction.Desc;
			}
		}

		public event DataBindEventHandler ItemDataBound;

		public Common_OrderItem_AfterSales()
		{
			base.ID = "Common_OrderItemAfterSales";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/tags/Skin-Common_OrderItem_AfterSales.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.listOrderItems = (Repeater)this.FindControl("listOrderItems");
			this.listOrderItems.ItemDataBound += this.listOrderItems_ItemDataBound;
		}

		private void listOrderItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lnkProductReview");
			Literal literal = (Literal)e.Item.FindControl("litPrice");
			Literal literal2 = (Literal)e.Item.FindControl("ltlProductCount");
			HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnApplyAfterSale");
			HtmlAnchor htmlAnchor3 = (HtmlAnchor)e.Item.FindControl("hylinkProductName");
			Literal literal3 = (Literal)e.Item.FindControl("ltlSKUContent");
			HtmlAnchor htmlAnchor4 = (HtmlAnchor)e.Item.FindControl("lkbtnItemStatus");
			string key = DataBinder.Eval(e.Item.DataItem, "SkuId").ToString();
			int num = 0;
			int.TryParse(DataBinder.Eval(e.Item.DataItem, "ProductId").ToString(), out num);
			LineItemInfo lineItemInfo = this.order.LineItems[key];
			literal.Text = lineItemInfo.ItemAdjustedPrice.F2ToString("f2");
			literal2.Text = lineItemInfo.Quantity.ToString();
			htmlAnchor3.InnerText = lineItemInfo.ItemDescription;
			if (this.order.StoreId > 0)
			{
				HtmlAnchor htmlAnchor5 = htmlAnchor;
				HtmlAnchor htmlAnchor6 = htmlAnchor3;
				string text3 = htmlAnchor5.HRef = (htmlAnchor6.HRef = $"StoreProductDetails.aspx?ProductId={num}&StoreId={this.order.StoreId}");
				if (this.IsUseInApp)
				{
					HtmlAnchor htmlAnchor7 = htmlAnchor;
					HtmlAnchor htmlAnchor8 = htmlAnchor3;
					text3 = (htmlAnchor7.HRef = (htmlAnchor8.HRef = $"javascript:showStoreProductDetail({num},{this.order.StoreId})"));
				}
			}
			else
			{
				HtmlAnchor htmlAnchor9 = htmlAnchor;
				HtmlAnchor htmlAnchor10 = htmlAnchor3;
				string text3 = htmlAnchor9.HRef = (htmlAnchor10.HRef = $"ProductDetails.aspx?ProductId={num}");
				if (this.IsUseInApp)
				{
					HtmlAnchor htmlAnchor11 = htmlAnchor;
					HtmlAnchor htmlAnchor12 = htmlAnchor3;
					text3 = (htmlAnchor11.HRef = (htmlAnchor12.HRef = $"javascript:showProductDetail({num});"));
				}
			}
			literal3.Text = lineItemInfo.SKUContent;
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			this.listOrderItems.DataSource = this.DataSource;
			this.listOrderItems.DataBind();
		}
	}
}
