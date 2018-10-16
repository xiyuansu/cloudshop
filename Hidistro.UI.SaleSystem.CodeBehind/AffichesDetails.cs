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
	public class AffichesDetails : HtmlTemplatedWebControl
	{
		private int affichesId = 0;

		private Literal litTilte;

		private Literal litContent;

		private FormatedTimeLabel litAffichesAddedDate;

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
				this.SkinName = "Skin-AffichesDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(base.GetParameter("AfficheId", false), out this.affichesId))
			{
				base.GotoResourceNotFound();
			}
			this.litAffichesAddedDate = (FormatedTimeLabel)this.FindControl("litAffichesAddedDate");
			this.litContent = (Literal)this.FindControl("litContent");
			this.litTilte = (Literal)this.FindControl("litTilte");
			this.lblFront = (Label)this.FindControl("lblFront");
			this.lblNext = (Label)this.FindControl("lblNext");
			this.aFront = (HtmlAnchor)this.FindControl("front");
			this.aNext = (HtmlAnchor)this.FindControl("next");
			this.lblFrontTitle = (Label)this.FindControl("lblFrontTitle");
			this.lblNextTitle = (Label)this.FindControl("lblNextTitle");
			if (!this.Page.IsPostBack)
			{
				AfficheInfo affiche = CommentBrowser.GetAffiche(this.affichesId);
				if (affiche != null)
				{
					PageTitle.AddSiteNameTitle(affiche.Title);
					this.litTilte.Text = affiche.Title;
					string str = HiContext.Current.HostPath + base.GetRouteUrl("AffichesDetails", new
					{
						afficheId = this.affichesId
					});
					this.litContent.Text = affiche.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
					this.litAffichesAddedDate.Time = affiche.AddedDate;
					AfficheInfo frontOrNextAffiche = CommentBrowser.GetFrontOrNextAffiche(this.affichesId, "Front");
					AfficheInfo frontOrNextAffiche2 = CommentBrowser.GetFrontOrNextAffiche(this.affichesId, "Next");
					if (frontOrNextAffiche != null && frontOrNextAffiche.AfficheId > 0)
					{
						if (this.lblFront != null)
						{
							this.lblFront.Visible = true;
							this.aFront.HRef = base.GetRouteUrl("AffichesDetails", new
							{
								afficheId = frontOrNextAffiche.AfficheId
							});
							this.lblFrontTitle.Text = frontOrNextAffiche.Title;
						}
					}
					else if (this.lblFront != null)
					{
						this.lblFront.Visible = false;
					}
					if (frontOrNextAffiche2 != null && frontOrNextAffiche2.AfficheId > 0)
					{
						if (this.lblNext != null)
						{
							this.lblNext.Visible = true;
							this.aNext.HRef = base.GetRouteUrl("AffichesDetails", new
							{
								afficheId = frontOrNextAffiche2.AfficheId
							});
							this.lblNextTitle.Text = frontOrNextAffiche2.Title;
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
