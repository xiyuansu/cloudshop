using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MessageTemplets)]
	public class SendMessageTemplets : AdminPage
	{
		protected Repeater grdEmailTempletsNew;

		protected Button btnSaveSendSetting;

		protected HiddenField hfOpenMultStore;

		protected HiddenField hidOpenReferral;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSaveSendSetting.Click += this.btnSaveSendSetting_Click;
			if (!this.Page.IsPostBack)
			{
				this.grdEmailTempletsNew.DataSource = MessageTemplateHelper.GetMessageTemplates();
				this.grdEmailTempletsNew.DataBind();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.hfOpenMultStore.Value = masterSettings.OpenMultStore.ToString().ToLower();
				this.hidOpenReferral.Value = ((masterSettings.OpenReferral == 1) ? "true" : "false");
			}
		}

		private void btnSaveSendSetting_Click(object sender, EventArgs e)
		{
			IList<MessageTemplate> messageTemplates = MessageTemplateHelper.GetMessageTemplates();
			List<MessageTemplate> list = new List<MessageTemplate>();
			for (int i = 0; i < this.grdEmailTempletsNew.Items.Count; i++)
			{
				HiddenField hiddenField = this.grdEmailTempletsNew.Items[i].FindControl("hfMessageType") as HiddenField;
				string MessageType = hiddenField.Value;
				MessageTemplate messageTemplate = messageTemplates.FirstOrDefault((MessageTemplate t) => t.MessageType == MessageType);
				if (messageTemplate != null)
				{
					CheckBox checkBox = (CheckBox)this.grdEmailTempletsNew.Items[i].FindControl("chkSendEmail");
					messageTemplate.SendEmail = checkBox.Checked;
					CheckBox checkBox2 = (CheckBox)this.grdEmailTempletsNew.Items[i].FindControl("chkInnerMessage");
					messageTemplate.SendInnerMessage = checkBox2.Checked;
					CheckBox checkBox3 = (CheckBox)this.grdEmailTempletsNew.Items[i].FindControl("chkCellPhoneMessage");
					messageTemplate.SendSMS = checkBox3.Checked;
					CheckBox checkBox4 = (CheckBox)this.grdEmailTempletsNew.Items[i].FindControl("chkWeixinMessage");
					messageTemplate.SendWeixin = checkBox4.Checked;
					list.Add(messageTemplate);
				}
			}
			MessageTemplateHelper.UpdateSettings(list);
			this.ShowMsg("保存设置成功", true);
		}
	}
}
