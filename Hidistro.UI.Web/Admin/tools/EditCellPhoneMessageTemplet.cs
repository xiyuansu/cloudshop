using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MessageTemplets)]
	public class EditCellPhoneMessageTemplet : AdminPage
	{
		private string messageType;

		protected Label litEmailType;

		protected HiddenField hidTagDescription;

		protected Literal litTagDescription;

		protected TextBox txtContent;
        protected TextBox txtSMSTemplateCode;
        protected TextBox txtSMSTemplateContent;

        protected HtmlGenericControl txtContentTip;

		protected Button btnSaveCellPhoneMessageTemplet;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.messageType = this.Page.Request.QueryString["MessageType"];
			this.btnSaveCellPhoneMessageTemplet.Click += this.btnSaveCellPhoneMessageTemplet_Click;
			if (!base.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.messageType))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
					if (messageTemplate == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.litEmailType.Text = messageTemplate.Name;
						this.litTagDescription.Text = messageTemplate.TagDescription;
						this.txtContent.Text = messageTemplate.SMSBody;
                        this.txtSMSTemplateCode.Text = messageTemplate.SMSTemplateCode;
                        this.txtSMSTemplateContent.Text = messageTemplate.SMSTemplateContent;
                        this.hidTagDescription.Value = messageTemplate.TagDescription;
					}
				}
			}
		}

		private void btnSaveCellPhoneMessageTemplet_Click(object sender, EventArgs e)
		{
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
			if (messageTemplate != null)
			{
				if (string.IsNullOrEmpty(this.txtContent.Text))
				{
					this.ShowMsg("短信内容不能为空", false);
				}
				else if (this.txtContent.Text.Trim().Length < 1 || this.txtContent.Text.Trim().Length > 300)
				{
					this.ShowMsg("长度限制在1-300个字符之间", false);
				}
				else
				{
                    messageTemplate.SMSTemplateCode = this.txtSMSTemplateCode.Text.Trim();
                    messageTemplate.SMSBody = this.txtContent.Text.Trim();
                    messageTemplate.SMSTemplateContent = this.txtSMSTemplateContent.Text.Trim();
                    MessageTemplateHelper.UpdateTemplate(messageTemplate);
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
				}
			}
		}
	}
}
