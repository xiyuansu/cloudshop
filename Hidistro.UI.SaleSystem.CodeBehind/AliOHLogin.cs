using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AliOHLogin : WAPTemplatedWebControl
	{
		private HtmlInputControl client;

		private HtmlGenericControl fastlogin;

		private HtmlGenericControl fastlogin0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VLogin.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("登录");
			this.client = (HtmlInputHidden)this.FindControl("client");
			this.client.Value = base.ClientType.ToString().ToLower();
		}
	}
}
