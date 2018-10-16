using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShoppingCart_GiftList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_ShoppingCart_GiftList";

		private Panel pnlShopGiftCart;

		private Repeater dataListGiftShoppingCrat;

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

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.dataListGiftShoppingCrat.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataListGiftShoppingCrat.DataSource = value;
			}
		}

		public event DataListCommandEventHandler ItemCommand;

		public Common_ShoppingCart_GiftList()
		{
			base.ID = "Common_ShoppingCart_GiftList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_GiftList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.dataListGiftShoppingCrat = (Repeater)this.FindControl("dataListGiftShoppingCrat");
			this.pnlShopGiftCart = (Panel)this.FindControl("pnlShopGiftCart");
			this.pnlShopGiftCart.Visible = false;
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataListGiftShoppingCrat.DataSource != null)
			{
				this.dataListGiftShoppingCrat.DataBind();
			}
		}

		public void ShowGiftCart(bool pointgift)
		{
			this.pnlShopGiftCart.Visible = pointgift;
		}
	}
}
