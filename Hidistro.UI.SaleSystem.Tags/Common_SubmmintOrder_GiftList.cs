using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubmmintOrder_GiftList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_SubmmintOrder_GiftList";

		private Panel pnlAllGift;

		private Repeater dataShopGiftCart;

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
				return this.dataShopGiftCart.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.dataShopGiftCart.DataSource = value;
			}
		}

		public Common_SubmmintOrder_GiftList()
		{
			base.ID = "Common_SubmmintOrder_GiftList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_SubmmintOrder_GiftList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.pnlAllGift = (Panel)this.FindControl("pnlAllGift");
			this.dataShopGiftCart = (Repeater)this.FindControl("dataShopGiftCart");
			this.pnlAllGift.Visible = false;
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.dataShopGiftCart.DataSource != null)
			{
				this.dataShopGiftCart.DataBind();
			}
		}

		public void ShowGiftCart(bool allgift, bool pointgift = false, bool freegift = false)
		{
			this.pnlAllGift.Visible = allgift;
		}
	}
}
