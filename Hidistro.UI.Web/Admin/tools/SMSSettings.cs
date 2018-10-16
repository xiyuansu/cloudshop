using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using Ionic.Zlib;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SMSSettings)]
	public class SMSSettings : AdminPage
	{
		protected Label lbNum;

		protected Button btnSaveSMSSettings;

		protected TextBox txtTestCellPhone;

        protected TextBox txtTestSMSTemplateCode;


        protected TextBox txtTestSubject;

		protected Button btnTestSend;

		protected HiddenField txtSelectedName;

		protected HiddenField txtConfigData;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSaveSMSSettings.Click += this.btnSaveSMSSettings_Click;
			this.btnTestSend.Click += this.btnTestSend_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.SMSEnabled)
				{
					string text = HiCryptographer.TryDecypt(masterSettings.SMSSettings);
					if (text.StartsWith("<xml>") || text.StartsWith("&lt;xml>"))
					{
						ConfigData configData = new ConfigData(text);
						this.txtConfigData.Value = configData.SettingsXml;
						if (masterSettings != null)
						{
							this.lbNum.Text = this.GetAmount(masterSettings);
						}
					}
				}
				this.txtSelectedName.Value = "hishop.plugins.sms.ymsms";
			}
		}

		private ConfigData LoadConfig(out string selectedName)
		{
			selectedName = base.Request.Form["ddlSms"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = SMSSender.CreateInstance(selectedName);
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

		private void btnSaveSMSSettings_Click(object sender, EventArgs e)
		{
			string text = default(string);
			ConfigData configData = this.LoadConfig(out text);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(text) || configData == null)
			{
				masterSettings.SMSSender = string.Empty;
				masterSettings.SMSSettings = string.Empty;
			}
			else
			{
				if (!configData.IsValid)
				{
					masterSettings.SMSSettings = "";
				}
				else
				{
					masterSettings.SMSSettings = configData.SettingsXml;
				}
				masterSettings.SMSSender = text;
			}
			SettingsManager.Save(masterSettings);
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
		}

		private void btnTestSend_Click(object sender, EventArgs e)
		{
			string text = default(string);
			ConfigData configData = this.LoadConfig(out text);
            string templateCode = this.txtTestSMSTemplateCode.Text.Trim();//短信模板编码

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
			else if (string.IsNullOrEmpty(this.txtTestCellPhone.Text) || string.IsNullOrEmpty(this.txtTestSubject.Text) || this.txtTestCellPhone.Text.Trim().Length == 0 || this.txtTestSubject.Text.Trim().Length == 0)
			{
				this.ShowMsg("接收手机号和发送内容不能为空", false);
			}
			else if (!Regex.IsMatch(this.txtTestCellPhone.Text.Trim(), "^(13|14|15|18)\\d{9}$"))
			{
				this.ShowMsg("请填写正确的手机号码", false);
			}
            else if (string.IsNullOrEmpty(templateCode) || templateCode.Trim().Length == 0 )
            {
                this.ShowMsg("请填写正确的模板code", false);
            }
            else
			{
				SMSSender sMSSender = SMSSender.CreateInstance(text, configData.SettingsXml);
				string msg = default(string);
				bool success = sMSSender.Send(this.txtTestCellPhone.Text.Trim(), templateCode, this.txtTestSubject.Text.Trim(), out msg);
				this.ShowMsg(msg, success);
			}
		}

		protected string GetAmount(SiteSettings settings)
		{
            return "您的短信剩余条数请到阿里云官方查看";

            //if (!string.IsNullOrEmpty(settings.SMSSettings))
            //{
            //	string xml = HiCryptographer.TryDecypt(settings.SMSSettings);
            //	XmlDocument xmlDocument = new XmlDocument();
            //	xmlDocument.LoadXml(xml);
            //	if (xmlDocument.SelectSingleNode("xml/Appkey") == null)
            //	{
            //		return "未配置";
            //	}
            //	string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
            //	string postData = "method=getAmount&Appkey=" + innerText;
            //	string text = this.PostData("http://sms.huz.cn/getAmount.aspx", postData);
            //	int num = default(int);
            //	if (int.TryParse(text, out num))
            //	{
            //		return "您的短信剩余条数为：" + text.ToString();
            //	}
            //	return "获取短信条数发生错误，请检查Appkey是否输入正确!";
            //}
            //return "";
        }
 
	}
}
