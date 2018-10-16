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
	[PrivilegeCheck(Privilege.Helps)]
	public class AddHelp : AdminPage
	{
		protected HelpCategoryDropDownList dropHelpCategory;

		protected TextBox txtHelpTitle;

		protected TrimTextBox txtMetaDescription;

		protected TrimTextBox txtMetaKeywords;

		protected TextBox txtShortDesc;

		protected OnOff ooShowFooter;

		protected Ueditor fcContent;

		protected Button btnAddHelp;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAddHelp.Click += this.btnAddHelp_Click;
			if (!this.Page.IsPostBack)
			{
				this.ooShowFooter.SelectedValue = true;
				this.dropHelpCategory.DataBind();
				if (this.Page.Request.QueryString["categoryId"] != null)
				{
					int value = 0;
					int.TryParse(this.Page.Request.QueryString["categoryId"], out value);
					this.dropHelpCategory.SelectedValue = value;
				}
			}
		}

		private void btnAddHelp_Click(object sender, EventArgs e)
		{
			HelpInfo helpInfo = new HelpInfo();
			if (!this.dropHelpCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择帮助分类", false);
			}
			else
			{
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
					if (ArticleHelper.CreateHelp(helpInfo))
					{
						this.txtHelpTitle.Text = string.Empty;
						this.txtShortDesc.Text = string.Empty;
						this.fcContent.Text = string.Empty;
						this.ShowMsg("成功添加了一个帮助主题", true);
					}
					else
					{
						this.ShowMsg("添加帮助主题错误", false);
					}
				}
			}
		}
	}
}
