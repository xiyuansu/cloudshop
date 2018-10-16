using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShoppingCart_StoreList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_ShoppingCart_StoreList";

		private Repeater dataListStoreShoppingCart;

		private IList<ShoppingCartItemInfo> listCartItems;

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

		public IList<ShoppingCartItemInfo> ListCartItems
		{
			get
			{
				return this.listCartItems;
			}
			set
			{
				this.listCartItems = value;
			}
		}

		public Common_ShoppingCart_StoreList()
		{
			base.ID = "Common_ShoppingCart_StoreList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_StoreList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dataListStoreShoppingCart = (Repeater)this.FindControl("dataListStoreShoppingCart");
			if (this.dataListStoreShoppingCart != null && this.ListCartItems != null)
			{
				this.dataListStoreShoppingCart.ItemDataBound += this.dataListStoreShoppingCart_ItemDataBound;
				IOrderedEnumerable<ShoppingCartItemInfo> dataSource = from x in this.listCartItems
				where x.StoreId > 0
				orderby x.StoreId descending
				orderby x.IsValid descending
				select x;
				this.dataListStoreShoppingCart.DataSource = dataSource;
				this.dataListStoreShoppingCart.DataBind();
			}
		}

		private void dataListStoreShoppingCart_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			Control control = e.Item.FindControl("divInValid");
			Control control2 = e.Item.FindControl("divStore");
			ShoppingCartItemInfo shoppingCartItemInfo = e.Item.DataItem as ShoppingCartItemInfo;
			if (!shoppingCartItemInfo.IsValid)
			{
				control.Visible = true;
				control2.Visible = false;
			}
			else
			{
				control.Visible = false;
				control2.Visible = true;
			}
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListStoreShoppingCart.DataSource != null)
			{
				this.dataListStoreShoppingCart.DataBind();
			}
		}
	}
}
