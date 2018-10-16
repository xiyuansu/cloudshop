using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Catalog;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class UserProductReferLabel : Literal
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private string _txtClass = "";

		private string _priceClass = "";

		private string _style = "";

		public ProductInfo product
		{
			get;
			set;
		}

		public decimal MobileExclusive
		{
			get;
			set;
		}

		public string txtClass
		{
			get
			{
				return this._txtClass;
			}
			set
			{
				this._txtClass = value;
			}
		}

		public string PriceClass
		{
			get
			{
				return this._priceClass;
			}
			set
			{
				this._priceClass = value;
			}
		}

		public string Style
		{
			get
			{
				return this._style;
			}
			set
			{
				this._style = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.siteSettings.OpenReferral != 1)
			{
				base.Text = "";
				base.Render(writer);
			}
			else
			{
				int num = 0;
				if (this.Page.Request.QueryString["ProductId"] != null && !string.IsNullOrEmpty(this.Page.Request.QueryString["ProductId"].ToString()))
				{
					int.TryParse(this.Page.Request.QueryString["ProductId"], out num);
				}
				MemberInfo user = HiContext.Current.User;
				if (user == null || !user.IsReferral() || (this.product == null && num == 0))
				{
					base.Text = "";
				}
				else
				{
					if (this.product == null)
					{
						this.product = ProductBrowser.GetProductSimpleInfo(num);
					}
					if (this.product == null)
					{
						base.Text = "";
					}
					decimal d = (this.product.MinSalePrice - this.MobileExclusive > decimal.Zero) ? (this.product.MinSalePrice - this.MobileExclusive) : decimal.Zero;
					decimal d2 = this.product.SubMemberDeduct.HasValue ? this.product.SubMemberDeduct.Value : this.siteSettings.SubMemberDeduct;
					bool flag = true;
					if (string.IsNullOrEmpty(this.txtClass) && string.IsNullOrEmpty(this.Style) && string.IsNullOrEmpty(this.PriceClass))
					{
						flag = false;
					}
					if (d2 > decimal.Zero)
					{
						string text = (d * (d2 / 100m)).F2ToString("f2");
						base.Text = (flag ? ("<div " + (string.IsNullOrEmpty(this.txtClass) ? "" : ("class=\"" + this.txtClass + "\"")) + ((this.Style == "") ? "" : (" style=\"" + this.Style + "\"")) + ">分销奖励：<span " + (string.IsNullOrEmpty(this.PriceClass) ? "" : ("class=\"" + this.PriceClass + "\"")) + "  id=\"referSpan\">￥" + text + "</span></div>") : ("<span id=\"referSpan\" style=\"color:#ff5417;\">" + text + "</span>"));
					}
					else
					{
						base.Text = (flag ? "" : "0");
					}
					base.Render(writer);
				}
			}
		}
	}
}
