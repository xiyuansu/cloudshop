using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Promotions;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class WapAppDownloadInfo : Literal
	{
		[Bindable(true)]
		protected override void Render(HtmlTextWriter writer)
		{
			decimal couponsAmount = CouponHelper.GetCouponsAmount(HiContext.Current.SiteSettings.AppPromoteCouponList);
			if (HiContext.Current.SiteSettings.IsOpenAppPromoteCoupons && couponsAmount > decimal.Zero)
			{
				writer.WriteLine(string.Format("<div class=\"appcouponinfo\"><em></em>首次下载APP即可享<span>{0}</span>元红包<input type=\"button\" onclick=\"location.href='AppDownLoad'\" value=\"立即下载\" /></div>", couponsAmount.F2ToString("f2")));
				base.Render(writer);
			}
		}
	}
}
