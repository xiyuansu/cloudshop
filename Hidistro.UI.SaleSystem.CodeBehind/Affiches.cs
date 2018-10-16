using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Affiches : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptAffiches;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Affiches.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptAffiches = (ThemedTemplatedRepeater)this.FindControl("rptAffiches");
			if (!this.Page.IsPostBack)
			{
				this.rptAffiches.DataSource = CommentBrowser.GetAfficheList(false);
				this.rptAffiches.DataBind();
			}
		}
	}
}
