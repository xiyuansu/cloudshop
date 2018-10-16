using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class AddPaymentType : AdminPage
	{
		protected TextBox txtName;

		protected FileUpload fileBankUnionCert;

		protected OnOff radiIsUseInpour;

		protected Ueditor fcContent;

		protected Button btnCreate;

		protected ImageList ImageList;

		protected HiddenField txtSelectedName;

		protected HiddenField txtConfigData;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnCreate.Click += this.btnCreate_Click;
			if (this.Page.IsPostBack)
			{
				return;
			}
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			string text = default(string);
			ConfigData configData = default(ConfigData);
			if (this.ValidateValues(out text, out configData))
			{
				PaymentModeInfo paymentMode = new PaymentModeInfo
				{
					Name = Globals.StripAllTags(this.txtName.Text),
					Description = this.fcContent.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", ""),
					Gateway = text,
					IsUseInpour = this.radiIsUseInpour.SelectedValue,
					ApplicationType = PayApplicationType.payOnPC,
					Settings = HiCryptographer.Encrypt(configData.SettingsXml),
					DisplaySequence = 0
				};
				if (SalesHelper.CreatePaymentMode(paymentMode))
				{
					if (text == "hishop.plugins.payment.wxqrcode.wxqrcoderequest")
					{
						PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode(text);
						if (paymentMode2 != null)
						{
							ConfigData configData2 = new ConfigData(HiCryptographer.Decrypt(paymentMode2.Settings));
							string settingsXml = configData2.SettingsXml;
							string xmlNodeValue = Globals.GetXmlNodeValue(settingsXml, "AppId");
							string xmlNodeValue2 = Globals.GetXmlNodeValue(settingsXml, "MCHID");
							string xmlNodeValue3 = Globals.GetXmlNodeValue(settingsXml, "AppSecret");
							string xmlNodeValue4 = Globals.GetXmlNodeValue(settingsXml, "Sub_AppId");
							string xmlNodeValue5 = Globals.GetXmlNodeValue(settingsXml, "Sub_mch_Id");
							string xmlNodeValue6 = Globals.GetXmlNodeValue(settingsXml, "CertPath");
							string xmlNodeValue7 = Globals.GetXmlNodeValue(settingsXml, "MCHID");
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							if (string.IsNullOrEmpty(xmlNodeValue4) && string.IsNullOrEmpty(xmlNodeValue5))
							{
								masterSettings.WeixinAppId = xmlNodeValue;
								masterSettings.WeixinPartnerID = xmlNodeValue2;
								masterSettings.Main_AppId = "";
								masterSettings.Main_Mch_ID = "";
							}
							else
							{
								masterSettings.WeixinAppId = xmlNodeValue4;
								masterSettings.WeixinPartnerID = xmlNodeValue5;
								masterSettings.Main_Mch_ID = xmlNodeValue2;
								masterSettings.Main_AppId = xmlNodeValue;
							}
							masterSettings.WeixinPartnerKey = xmlNodeValue3;
							masterSettings.WeixinCertPath = xmlNodeValue6;
							masterSettings.WeixinCertPassword = xmlNodeValue7;
							SettingsManager.Save(masterSettings);
						}
					}
					base.Response.Redirect(Globals.GetAdminAbsolutePath("sales/PaymentTypes.aspx"));
				}
				else
				{
					this.ShowMsg("未知错误", false);
				}
			}
		}

		private ConfigData LoadConfig(out string selectedName)
		{
			selectedName = base.Request.Form["ddlPayments"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			if (selectedName.Equals("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest") && this.fileBankUnionCert.HasFile)
			{
				if (!Globals.ValidateCertFile(this.fileBankUnionCert.PostedFile.FileName))
				{
					this.ShowMsg("非法的证书文件", false);
					return null;
				}
				this.fileBankUnionCert.PostedFile.SaveAs(base.Server.MapPath("~/config/") + this.fileBankUnionCert.PostedFile.FileName);
			}
			string text = "";
			if (selectedName.StartsWith("hishop.plugins.payment."))
			{
				text = selectedName.Replace("hishop.plugins.payment.", "");
			}
			if (text.LastIndexOf(".") > 0)
			{
				text = text.Substring(0, text.LastIndexOf("."));
			}
			if (!string.IsNullOrEmpty(text))
			{
				text += "/";
			}
			HttpFileCollection files = base.Request.Files;
			NameValueCollection nameValueCollection = new NameValueCollection();
			ConfigablePlugin configablePlugin = PaymentRequest.CreateInstance(selectedName);
			if (configablePlugin == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			string[] allKeys = base.Request.Form.AllKeys;
			foreach (string text2 in allKeys)
			{
				if (text2 == "SignCertFileName")
				{
					nameValueCollection.Add(text2, this.fileBankUnionCert.PostedFile.FileName);
				}
				else
				{
					nameValueCollection.Add(text2, base.Request.Form[text2].Trim());
				}
			}
			IList<string> fileConfigNames = configablePlugin.GetFileConfigNames();
			if (fileConfigNames != null && files.Count >= 2 && files.Count > fileConfigNames.Count)
			{
				string text3 = "/pay/cert/" + text;
				if (!Globals.PathExist(text3, false))
				{
					Globals.CreatePath(text3);
				}
				for (int j = 1; j < files.Count; j++)
				{
					if (files[j].ContentLength > 0)
					{
						if (Globals.ValidateCertFile(files[j].FileName))
						{
							files[j].SaveAs(base.Server.MapPath(text3 + files[j].FileName));
							nameValueCollection.Add(fileConfigNames[j - 1], text3 + files[j].FileName);
							continue;
						}
						this.ShowMsg("非法的证书文件", false);
						return null;
					}
					nameValueCollection.Add(fileConfigNames[j - 1], "");
				}
			}
			ConfigData configData = configablePlugin.GetConfigData(nameValueCollection);
			if (configData != null)
			{
				this.txtConfigData.Value = configData.SettingsXml;
			}
			return configData;
		}

		private bool ValidateValues(out string selectedPlugin, out ConfigData data)
		{
			string text = string.Empty;
			data = this.LoadConfig(out selectedPlugin);
			if (data == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(selectedPlugin))
			{
				this.ShowMsg("请先选择一个支付接口类型", false);
				return false;
			}
			if (!data.IsValid)
			{
				foreach (string errorMsg in data.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(errorMsg);
				}
			}
			if (string.IsNullOrEmpty(this.txtName.Text) || this.txtName.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("支付方式名称不能为空，长度限制在1-60个字符之间");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}

		private void UploadCertFile()
		{
		}
	}
}
