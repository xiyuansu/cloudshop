using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppResourceNotFound : AppshopTemplatedWebControl
	{
		private Literal litMsg;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ResourceNotFound.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("错误页面");
			this.litMsg = (Literal)this.FindControl("litMsg");
			if (this.litMsg != null)
			{
				this.litMsg.Text = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["errorMsg"]));
			}
		}
	}
}
