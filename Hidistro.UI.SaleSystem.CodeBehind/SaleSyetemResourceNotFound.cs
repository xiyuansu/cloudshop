using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SaleSyetemResourceNotFound : HtmlTemplatedWebControl
	{
		private Literal litMsg;

		private Literal litNotFindMsg;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SaleSyetemResourceNotFound.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litMsg = (Literal)this.FindControl("litMsg");
			this.litNotFindMsg = (Literal)this.FindControl("litNotFindMsg");
			if (this.litMsg != null)
			{
				this.litMsg.Text = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["errorMsg"]));
			}
			if (this.litNotFindMsg != null)
			{
				this.litNotFindMsg.Visible = string.IsNullOrEmpty(this.Page.Request.QueryString["errorMsg"]);
			}
		}
	}
}
