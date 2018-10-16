using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Promotions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_AppDownloadCouponInfo : Literal
	{
		public int Width
		{
			get;
			set;
		}

		public bool ShowText
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.SiteSettings.IsOpenAppPromoteCoupons)
			{
				decimal couponsAmount = CouponHelper.GetCouponsAmount(HiContext.Current.SiteSettings.AppPromoteCouponList);
				if (couponsAmount > decimal.Zero)
				{
					string text = "/Storage/master/QRCode/" + HttpContext.Current.Request.Url.Host + "_appdownload.png";
					string siteUrl = SettingsManager.GetMasterSettings().SiteUrl;
					string str = (siteUrl.Contains("http://") || siteUrl.Contains("https://")) ? siteUrl : ("http://" + siteUrl);
					str += (siteUrl.EndsWith("/") ? "" : "/WapShop/AppDownload");
					Globals.CreateQRCode(str, text, false, ImageFormats.Png);
					base.Text = string.Format("<img src=\"{0}\" CouponsAmount=\"{1}\" {2}/>", text, couponsAmount, (this.Width > 0) ? ("width=\"" + this.Width + "px\"") : "");
					if (this.ShowText)
					{
						base.Text += string.Format("<p>首次下载APP<br>即可享<span>{0}</span>元红包</p>", couponsAmount.F2ToString("f2"));
					}
					base.Render(writer);
				}
				else
				{
					base.Text = "";
				}
			}
			else
			{
				base.Text = "";
			}
		}
	}
}
