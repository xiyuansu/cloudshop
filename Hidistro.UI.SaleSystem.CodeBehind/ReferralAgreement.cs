using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ReferralAgreement : HtmlTemplatedWebControl
	{
		private Literal litlAgreemen;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ReferralAgreement.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litlAgreemen = (Literal)this.FindControl("litlAgreemen");
			this.litlAgreemen = (Literal)this.FindControl("litlAgreemen");
			PageTitle.AddSiteNameTitle("我要成为分销员");
			this.litlAgreemen.Text = HiContext.Current.SiteSettings.ReferralIntroduction;
		}
	}
}
