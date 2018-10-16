using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPResourceNotFound : WAPTemplatedWebControl
	{
		private Literal litMessage;

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
			this.litMessage = (Literal)this.FindControl("litMessage");
			if (this.litMessage != null)
			{
				this.litMessage.Text = this.Page.Request["msg"].ToNullString();
			}
		}
	}
}
