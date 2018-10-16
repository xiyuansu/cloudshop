using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapRecruitmentAgreement : WAPTemplatedWebControl
	{
		private Literal litAgreement;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RecruitmentAgreement.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litAgreement = (Literal)this.FindControl("litAgreement");
			if (this.litAgreement != null)
			{
				this.litAgreement.Text = HiContext.Current.SiteSettings.RecruitmentAgreement;
			}
		}
	}
}
