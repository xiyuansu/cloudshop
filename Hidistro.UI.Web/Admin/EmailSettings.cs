using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EmailSettings)]
	public class EmailSettings : AdminPage
	{
		protected Button btnChangeEmailSettings;

		protected TextBox txtTestEmail;

		protected Button btnTestEmailSettings;

		protected HiddenField txtSelectedName;

		protected HiddenField txtConfigData;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnChangeEmailSettings.Click += this.btnChangeEmailSettings_Click;
			this.btnTestEmailSettings.Click += this.btnTestEmailSettings_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.EmailEnabled)
				{
					this.txtSelectedName.Value = masterSettings.EmailSender.ToLower();
					string text = HiCryptographer.TryDecypt(masterSettings.EmailSettings);
					if (text.StartsWith("<xml>") || text.StartsWith("&lt;xml>"))
					{
						ConfigData configData = new ConfigData(text);
						this.txtConfigData.Value = configData.SettingsXml;
					}
				}
			}
		}

		private ConfigData LoadConfig(out string selectedName)
		{
			selectedName = base.Request.Form["ddlEmails"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = EmailSender.CreateInstance(selectedName);
			if (configablePlugin == null)
			{
				return null;
			}
			ConfigData configData = configablePlugin.GetConfigData(base.Request.Form);
			if (configData != null)
			{
				this.txtConfigData.Value = configData.SettingsXml;
			}
			return configData;
		}

		private void btnChangeEmailSettings_Click(object sender, EventArgs e)
		{
			string text = default(string);
			ConfigData configData = this.LoadConfig(out text);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(text) || configData == null)
			{
				masterSettings.EmailSender = string.Empty;
				masterSettings.EmailSettings = string.Empty;
			}
			else
			{
				if (!configData.IsValid)
				{
					string text2 = "";
					foreach (string errorMsg in configData.ErrorMsgs)
					{
						text2 += Formatter.FormatErrorMessage(errorMsg);
					}
					this.ShowMsg(text2, false);
					return;
				}
				masterSettings.EmailSender = text;
				masterSettings.EmailSettings = HiCryptographer.Encrypt(configData.SettingsXml);
			}
			SettingsManager.Save(masterSettings);
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
		}

		private void btnTestEmailSettings_Click(object sender, EventArgs e)
		{
			string text = default(string);
			ConfigData configData = this.LoadConfig(out text);
			if (string.IsNullOrEmpty(text) || configData == null)
			{
				this.ShowMsg("请先选择发送方式并填写配置信息", false);
			}
			else if (!configData.IsValid)
			{
				string text2 = "";
				foreach (string errorMsg in configData.ErrorMsgs)
				{
					text2 += Formatter.FormatErrorMessage(errorMsg);
				}
				this.ShowMsg(text2, false);
			}
			else if (string.IsNullOrEmpty(this.txtTestEmail.Text) || this.txtTestEmail.Text.Trim().Length == 0)
			{
				this.ShowMsg("请填写接收测试邮件的邮箱地址", false);
			}
			else if (!Regex.IsMatch(this.txtTestEmail.Text.Trim(), "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
			{
				this.ShowMsg("请填写正确的邮箱地址", false);
			}
			else
			{
				MailMessage mailMessage = new MailMessage
				{
					IsBodyHtml = true,
					Priority = MailPriority.High,
					Body = "Success",
					Subject = "This is a test mail"
				};
				mailMessage.To.Add(this.txtTestEmail.Text.Trim());
				EmailSender emailSender = EmailSender.CreateInstance(text, configData.SettingsXml);
				try
				{
					if (emailSender.Send(mailMessage, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
					{
						this.ShowMsg("发送测试邮件成功", true);
					}
					else
					{
						this.ShowMsg("发送测试邮件失败", false);
					}
				}
				catch (Exception ex)
				{
					Globals.WriteExceptionLog(ex, null, "EmailSend");
					this.ShowMsg("邮件配置错误", false);
				}
			}
		}
	}
}
