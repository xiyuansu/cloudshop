using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Affiches)]
	public class EditAffiche : AdminPage
	{
		private int afficheId;

		protected TextBox txtAfficheTitle;

		protected Ueditor fcContent;

		protected Button btnEditAffiche;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["afficheId"], out this.afficheId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditAffiche.Click += this.btnEditAffiche_Click;
				if (!this.Page.IsPostBack)
				{
					AfficheInfo affiche = NoticeHelper.GetAffiche(this.afficheId);
					if (affiche == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						Globals.EntityCoding(affiche, false);
						this.txtAfficheTitle.Text = affiche.Title;
						this.fcContent.Text = affiche.Content;
					}
				}
			}
		}

		private void btnEditAffiche_Click(object sender, EventArgs e)
		{
			AfficheInfo afficheInfo = new AfficheInfo();
			afficheInfo.AfficheId = this.afficheId;
			afficheInfo.Title = this.txtAfficheTitle.Text.Trim();
			afficheInfo.Content = this.fcContent.Text;
			afficheInfo.AddedDate = DateTime.Now;
			ValidationResults validationResults = Validation.Validate(afficheInfo, "ValAfficheInfo");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			else
			{
				afficheInfo.AfficheId = this.afficheId;
				if (NoticeHelper.UpdateAffiche(afficheInfo))
				{
					this.ShowMsg("成功修改了当前公告信息", true);
				}
				else
				{
					this.ShowMsg("修改公告信息错误", false);
				}
			}
		}
	}
}
