using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShoppingCart_PromoGiftList : AscxTemplatedWebControl
	{
		public const string TagID = "Common_ShoppingCart_PromoGiftList";

		private Repeater rp_promogift;

		private Panel pnlPromoGift;

		private Literal lit_promonum;

		private Literal lit_promosumnum;

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

		public int SumNum
		{
			get
			{
				return int.Parse(this.lit_promosumnum.Text);
			}
		}

		public int CurentNum
		{
			get
			{
				return int.Parse(this.lit_promonum.Text);
			}
		}

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.rp_promogift.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.rp_promogift.DataSource = value;
			}
		}

		public event RepeaterCommandEventHandler ItemCommand;

		public Common_ShoppingCart_PromoGiftList()
		{
			base.ID = "Common_ShoppingCart_PromoGiftList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_PromoGiftList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rp_promogift = (Repeater)this.FindControl("rp_promogift");
			this.pnlPromoGift = (Panel)this.FindControl("pnlPromoGift");
			this.lit_promonum = (Literal)this.FindControl("lit_promonum");
			this.lit_promosumnum = (Literal)this.FindControl("lit_promosumnum");
			this.pnlPromoGift.Visible = false;
			this.rp_promogift.ItemCommand += this.rp_promogift_ItemCommand;
		}

		private void rp_promogift_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.rp_promogift.DataSource != null)
			{
				this.rp_promogift.DataBind();
			}
		}

		public void ShowPromoGift(int pronum, int sumnum)
		{
			if (this.DataSource == null)
			{
				this.pnlPromoGift.Visible = false;
			}
			else
			{
				this.pnlPromoGift.Visible = true;
				this.lit_promonum.Text = pronum.ToString();
				this.lit_promosumnum.Text = sumnum.ToString();
			}
		}
	}
}
