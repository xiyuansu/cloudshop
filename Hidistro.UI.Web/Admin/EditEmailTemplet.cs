using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MessageTemplets)]
	public class EditEmailTemplet : AdminPage
	{
		private string emailType;

		protected Label litEmailType;

		protected Literal litEmailDescription;

		protected TextBox txtEmailSubject;

		protected HtmlGenericControl txtEmailSubjectTip;

		protected Ueditor fcContent;

		protected Button btnSaveEmailTemplet;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSaveEmailTemplet.Click += this.btnSaveEmailTemplet_Click;
			this.emailType = this.Page.Request.QueryString["MessageType"];
			if (!this.Page.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.emailType))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.emailType);
					if (messageTemplate == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.litEmailType.Text = messageTemplate.Name;
						this.litEmailDescription.Text = messageTemplate.TagDescription;
						this.txtEmailSubject.Text = messageTemplate.EmailSubject;
						this.fcContent.Text = messageTemplate.EmailBody;
					}
				}
			}
		}

		private void btnSaveEmailTemplet_Click(object sender, EventArgs e)
		{
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.emailType);
			if (messageTemplate != null)
			{
				string text = string.Empty;
				bool flag = true;
				if (string.IsNullOrEmpty(this.txtEmailSubject.Text))
				{
					text += Formatter.FormatErrorMessage("邮件标题不能为空");
					flag = false;
				}
				else if (this.txtEmailSubject.Text.Trim().Length < 1 || this.txtEmailSubject.Text.Trim().Length > 60)
				{
					text += Formatter.FormatErrorMessage("邮件标题长度限制在1-60个字符之间");
					flag = false;
				}
				if (string.IsNullOrEmpty(this.fcContent.Text) || this.fcContent.Text.Trim().Length == 0)
				{
					text += Formatter.FormatErrorMessage("邮件内容不能为空");
					flag = false;
				}
				if (!flag)
				{
					this.ShowMsg(text, false);
				}
				else
				{
					string text2 = this.fcContent.Text;
					Regex regex = new Regex("<img\\b[^>]*?\\bsrc[\\s]*=[\\s]*[\"']?[\\s]*(?<imgUrl>[^\"'>]*)[^>]*?/?[\\s]*>", RegexOptions.IgnoreCase);
					foreach (Match item in regex.Matches(text2))
					{
						string value = item.Groups["imgUrl"].Value;
						if (value.StartsWith("/"))
						{
							text2 = text2.Replace(value, $"http://{base.Request.Url.Host}{value}");
						}
					}
					messageTemplate.EmailBody = text2;
					messageTemplate.EmailSubject = this.txtEmailSubject.Text.Trim();
					MessageTemplateHelper.UpdateTemplate(messageTemplate);
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
				}
			}
		}
	}
}
