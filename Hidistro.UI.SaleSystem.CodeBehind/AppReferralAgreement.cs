using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppReferralAgreement : AppshopTemplatedWebControl
	{
		private Literal litReferralRegisterAgreement;

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
			this.litReferralRegisterAgreement = (Literal)this.FindControl("litReferralRegisterAgreement");
			PageTitle.AddSiteNameTitle("我要成为分销员");
			this.litReferralRegisterAgreement.Text = HiContext.Current.SiteSettings.ReferralIntroduction;
		}
	}
}
