using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FavourableDetails : HtmlTemplatedWebControl
	{
		private int activityId = 0;

		private Literal litHelpTitle;

		private Literal litHelpDescription;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-FavourableDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("activityId", false), out this.activityId))
			{
				base.GotoResourceNotFound();
			}
			this.litHelpDescription = (Literal)this.FindControl("litHelpDescription");
			this.litHelpTitle = (Literal)this.FindControl("litHelpTitle");
			if (!this.Page.IsPostBack)
			{
				PromotionInfo promote = CommentBrowser.GetPromote(this.activityId);
				if (promote == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					PageTitle.AddSiteNameTitle(promote.Name);
					this.litHelpTitle.Text = promote.Name;
					this.litHelpDescription.Text = promote.Description;
				}
			}
		}
	}
}
