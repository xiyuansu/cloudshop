using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppShoppingCart : AppshopTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptCartProducts;

		private AppshopTemplatedRepeater rptCartGifts;

		private HtmlInputHidden hidUserPoints;

		private Literal litTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VShoppingCart.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptCartProducts = (AppshopTemplatedRepeater)this.FindControl("rptCartProducts");
			this.litTotal = (Literal)this.FindControl("litTotal");
			this.hidUserPoints = (HtmlInputHidden)this.FindControl("hidUserPoints");
			this.rptCartGifts = (AppshopTemplatedRepeater)this.FindControl("rptCartGifts");
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(null, false, false, -1);
			if (shoppingCart != null)
			{
				this.rptCartProducts.DataSource = shoppingCart.LineItems;
				this.rptCartProducts.DataBind();
				this.litTotal.Text = shoppingCart.GetAmount(false).F2ToString("f2");
				if (shoppingCart.LineGifts.Count > 0)
				{
					IEnumerable<ShoppingCartGiftInfo> dataSource = from s in shoppingCart.LineGifts
					where s.PromoType == 0
					select s;
					this.rptCartGifts.DataSource = dataSource;
					this.rptCartGifts.DataBind();
				}
				this.hidUserPoints.Value = HiContext.Current.User.Points.ToString();
			}
			PageTitle.AddSiteNameTitle("购物车");
		}
	}
}
