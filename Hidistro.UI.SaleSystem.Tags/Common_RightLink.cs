using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_RightLink : AscxTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Skin-Common_RightLink.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			HtmlGenericControl htmlGenericControl = this.FindControl("divappdownload") as HtmlGenericControl;
			htmlGenericControl.Visible = HiContext.Current.SiteSettings.IsOpenAppPromoteCoupons;
		}
	}
}
