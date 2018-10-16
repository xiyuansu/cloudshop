using Hidistro.Context;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class HelpDetails : HtmlTemplatedWebControl
	{
		private int helpId = 0;

		private Literal litHelpTitle;

		private Literal litHelpDescription;

		private Literal litHelpContent;

		private FormatedTimeLabel litHelpAddedDate;

		private Label lblFront;

		private Label lblNext;

		private Label lblFrontTitle;

		private Label lblNextTitle;

		private HtmlAnchor aFront;

		private HtmlAnchor aNext;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-HelpDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("helpId", false), out this.helpId))
			{
				base.GotoResourceNotFound();
			}
			this.litHelpAddedDate = (FormatedTimeLabel)this.FindControl("litHelpAddedDate");
			this.litHelpDescription = (Literal)this.FindControl("litHelpDescription");
			this.litHelpContent = (Literal)this.FindControl("litHelpContent");
			this.litHelpTitle = (Literal)this.FindControl("litHelpTitle");
			this.lblFront = (Label)this.FindControl("lblFront");
			this.lblNext = (Label)this.FindControl("lblNext");
			this.lblFrontTitle = (Label)this.FindControl("lblFrontTitle");
			this.lblNextTitle = (Label)this.FindControl("lblNextTitle");
			this.aFront = (HtmlAnchor)this.FindControl("front");
			this.aNext = (HtmlAnchor)this.FindControl("next");
			if (!this.Page.IsPostBack)
			{
				HelpInfo help = CommentBrowser.GetHelp(this.helpId);
				if (help != null)
				{
					PageTitle.AddSiteNameTitle(help.Title);
					if (!string.IsNullOrEmpty(help.Meta_Keywords))
					{
						MetaTags.AddMetaKeywords(help.Meta_Keywords, HiContext.Current.Context);
					}
					if (!string.IsNullOrEmpty(help.Meta_Description))
					{
						MetaTags.AddMetaDescription(help.Meta_Description, HiContext.Current.Context);
					}
					this.litHelpTitle.Text = help.Title;
					this.litHelpDescription.Text = help.Description;
					string str = HiContext.Current.HostPath + base.GetRouteUrl("HelpDetails", new
					{
						this.helpId
					});
					this.litHelpContent.Text = help.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
					this.litHelpAddedDate.Time = help.AddedDate;
					HelpInfo frontOrNextHelp = CommentBrowser.GetFrontOrNextHelp(this.helpId, help.CategoryId, "Front");
					if (frontOrNextHelp != null && frontOrNextHelp.HelpId > 0)
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = true;
							this.aFront.HRef = "HelpDetails.aspx?helpId=" + frontOrNextHelp.HelpId;
							this.lblFrontTitle.Text = frontOrNextHelp.Title;
						}
					}
					else if (this.lblFront != null)
					{
						this.lblFront.Visible = false;
					}
					HelpInfo frontOrNextHelp2 = CommentBrowser.GetFrontOrNextHelp(this.helpId, help.CategoryId, "Next");
					if (frontOrNextHelp2 != null && frontOrNextHelp2.HelpId > 0)
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = true;
							this.aNext.HRef = "HelpDetails.aspx?helpId=" + frontOrNextHelp2.HelpId;
							this.lblNextTitle.Text = frontOrNextHelp2.Title;
						}
					}
					else if (this.lblNext != null)
					{
						this.lblNext.Visible = false;
					}
				}
			}
		}
	}
}
