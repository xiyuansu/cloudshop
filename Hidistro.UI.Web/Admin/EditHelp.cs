using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Helps)]
	public class EditHelp : AdminPage
	{
		private int helpId;

		protected Label lblEditHelp;

		protected HelpCategoryDropDownList dropHelpCategory;

		protected TextBox txtHelpTitle;

		protected TrimTextBox txtMetaDescription;

		protected TrimTextBox txtMetaKeywords;

		protected TextBox txtShortDesc;

		protected OnOff ooShowFooter;

		protected Ueditor fcContent;

		protected Button btnEditHelp;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["helpId"], out this.helpId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditHelp.Click += this.btnEditHelp_Click;
				if (!this.Page.IsPostBack)
				{
					this.dropHelpCategory.DataBind();
					HelpInfo help = ArticleHelper.GetHelp(this.helpId);
					if (help == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						Globals.EntityCoding(help, false);
						this.txtHelpTitle.Text = help.Title;
						this.txtMetaDescription.Text = help.Meta_Description;
						this.txtMetaKeywords.Text = help.Meta_Keywords;
						this.txtShortDesc.Text = help.Description;
						this.fcContent.Text = help.Content;
						this.lblEditHelp.Text = help.HelpId.ToString(CultureInfo.InvariantCulture);
						this.dropHelpCategory.SelectedValue = help.CategoryId;
						this.ooShowFooter.SelectedValue = help.IsShowFooter;
					}
				}
			}
		}

		private void btnEditHelp_Click(object sender, EventArgs e)
		{
			HelpInfo helpInfo = new HelpInfo();
			if (!this.dropHelpCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择帮助分类", false);
			}
			else
			{
				helpInfo.HelpId = this.helpId;
				helpInfo.AddedDate = DateTime.Now;
				helpInfo.CategoryId = this.dropHelpCategory.SelectedValue.Value;
				helpInfo.Title = this.txtHelpTitle.Text.Trim();
				helpInfo.Meta_Description = this.txtMetaDescription.Text.Trim();
				helpInfo.Meta_Keywords = this.txtMetaKeywords.Text.Trim();
				helpInfo.Description = this.txtShortDesc.Text.Trim();
				helpInfo.Content = this.fcContent.Text;
				helpInfo.IsShowFooter = this.ooShowFooter.SelectedValue;
				ValidationResults validationResults = Validation.Validate(helpInfo, "ValHelpInfo");
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
					if (this.ooShowFooter.SelectedValue)
					{
						HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(helpInfo.CategoryId);
						if (!helpCategory.IsShowFooter)
						{
							this.ShowMsg("当选中的帮助分类设置不在底部帮助显示时，此分类下的帮助主题就不能设置在底部帮助显示", false);
							return;
						}
					}
					if (ArticleHelper.UpdateHelp(helpInfo))
					{
						this.ShowMsg("已经成功修改当前帮助", true);
					}
					else
					{
						this.ShowMsg("编辑底部帮助错误", false);
					}
				}
			}
		}
	}
}
