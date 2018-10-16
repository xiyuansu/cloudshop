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
	public class AddAffiche : AdminPage
	{
		protected TextBox txtAfficheTitle;

		protected Ueditor fcContent;

		protected Button btnAddAffiche;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddAffiche.Click += this.btnAddAffiche_Click;
		}

		private void btnAddAffiche_Click(object sender, EventArgs e)
		{
			AfficheInfo afficheInfo = new AfficheInfo();
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
			else if (NoticeHelper.CreateAffiche(afficheInfo))
			{
				this.txtAfficheTitle.Text = string.Empty;
				this.fcContent.Text = string.Empty;
				this.ShowMsg("成功发布了一条公告", true);
			}
			else
			{
				this.ShowMsg("添加公告失败", false);
			}
		}
	}
}
