using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class EditPaymentType : AdminPage
	{
		private int modeId;

		private PaymentModeInfo item = null;

		private string configXml = "";

		private SiteSettings siteSettings;

		protected TextBox txtName;

		protected OnOff radiIsUseInpour;

		protected Ueditor fcContent;

		protected Button btnUpdate;

		protected HiddenField txtSelectedName;

		protected HiddenField txtConfigData;

		protected FileUpload fileBankUnionCert;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.siteSettings = SettingsManager.GetMasterSettings();
			if (this.siteSettings.IsDemoSite)
			{
				this.ShowMsg("演示站点，不允许编辑支付方式", true);
			}
			else if (!int.TryParse(this.Page.Request.QueryString["modeId"], out this.modeId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.item = SalesHelper.GetPaymentMode(this.modeId);
				if (this.item == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					this.btnUpdate.Click += this.btnUpdate_Click;
					if (!this.Page.IsPostBack)
					{
						Globals.EntityCoding(this.item, false);
						this.txtSelectedName.Value = this.item.Gateway.ToLower();
						ConfigablePlugin configablePlugin = PaymentRequest.CreateInstance(this.item.Gateway.ToLower());
						if (configablePlugin == null)
						{
							base.GotoResourceNotFound();
						}
						else
						{
							Globals.EntityCoding(this.item, false);
							this.txtSelectedName.Value = this.item.Gateway.ToLower();
							ConfigData configData = new ConfigData(HiCryptographer.Decrypt(this.item.Settings));
							this.configXml = configData.SettingsXml;
							this.txtConfigData.Value = this.EncodeingConfig(configData.SettingsXml, configablePlugin.GetHiddenPartConfigNames());
							this.txtName.Text = this.item.Name;
							this.fcContent.Text = this.item.Description;
							this.radiIsUseInpour.SelectedValue = this.item.IsUseInpour;
						}
					}
				}
			}
		}

		public string EncodeingConfig(string configXML, IList<string> nodeNameList)
		{
			string text = "";
			if (nodeNameList == null || nodeNameList.Count == 0)
			{
				return this.configXml;
			}
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.LoadXml(configXML);
				XmlNode documentElement = xmlDocument.DocumentElement;
				foreach (string nodeName in nodeNameList)
				{
					XmlNode xmlNode = documentElement.SelectSingleNode(nodeName);
					if (xmlNode != null)
					{
						string innerText = xmlNode.InnerText;
						if (innerText.Length > 6)
						{
							int num = innerText.Length - 6;
							if (num < 6)
							{
								num = 6;
							}
							xmlNode.InnerText = innerText.Substring(0, 3) + Globals.GetHiddenStr(num, "*") + innerText.Substring(innerText.Length - 3);
						}
					}
				}
				return xmlDocument.OuterXml.ToString();
			}
			catch
			{
				return "";
			}
		}

		private ConfigData LoadConfig(out string selectedName)
		{
			selectedName = base.Request.Form["ddlPayments"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			ConfigData configData = new ConfigData(HiCryptographer.Decrypt(this.item.Settings));
			this.configXml = configData.SettingsXml;
			if (selectedName.Equals("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest"))
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
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = PaymentRequest.CreateInstance(selectedName);
			if (configablePlugin == null)
			{
				return null;
			}
			IList<string> hiddenPartConfigNames = configablePlugin.GetHiddenPartConfigNames();
			NameValueCollection nameValueCollection = new NameValueCollection();
			string[] allKeys = base.Request.Form.AllKeys;
			foreach (string text2 in allKeys)
			{
				if (text2 == "SignCertFileName" && this.fileBankUnionCert.HasFile)
				{
					nameValueCollection.Add(text2, this.fileBankUnionCert.PostedFile.FileName);
				}
				else if (hiddenPartConfigNames != null && hiddenPartConfigNames.Count > 0 && hiddenPartConfigNames.Contains(text2))
				{
					string text3 = base.Request.Form[text2];
					if (text3.Replace("*", "").Length == 6)
					{
						text3 = Globals.GetXmlNodeValue(this.configXml, text2);
					}
					nameValueCollection.Add(text2, text3);
				}
				else
				{
					string value = base.Request.Form[text2];
					nameValueCollection.Add(text2, value);
				}
			}
			IList<string> fileConfigNames = configablePlugin.GetFileConfigNames();
			if (fileConfigNames != null && files.Count >= 2 && files.Count > fileConfigNames.Count)
			{
				string text4 = "/pay/cert/" + text;
				if (!Globals.PathExist(text4, false))
				{
					Globals.CreatePath(text4);
				}
				for (int j = 1; j < files.Count; j++)
				{
					if (files[j].ContentLength > 0)
					{
						if (Globals.ValidateCertFile(files[j].FileName))
						{
							files[j].SaveAs(base.Server.MapPath(text4 + files[j].FileName));
							nameValueCollection.Add(fileConfigNames[j - 1], text4 + files[j].FileName);
							continue;
						}
						this.ShowMsg("非法的证书文件", false);
						return null;
					}
					if (nameValueCollection[fileConfigNames[j - 1]] == null)
					{
						nameValueCollection.Add(fileConfigNames[j - 1], "");
					}
				}
			}
			ConfigData configData2 = configablePlugin.GetConfigData(nameValueCollection);
			if (configData2 != null)
			{
				this.txtConfigData.Value = configData2.SettingsXml;
			}
			return configData2;
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			string text = default(string);
			ConfigData configData = default(ConfigData);
			if (this.siteSettings.IsDemoSite)
			{
				this.ShowMsg("演示站点，不允许编辑支付方式", true);
			}
			else if (this.ValidateValues(out text, out configData))
			{
				PaymentModeInfo paymentMode = new PaymentModeInfo
				{
					ModeId = this.modeId,
					Name = this.txtName.Text.Trim(),
					Description = this.fcContent.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", ""),
					Gateway = text,
					IsUseInpour = this.radiIsUseInpour.SelectedValue,
					ApplicationType = PayApplicationType.payOnPC,
					Settings = HiCryptographer.Encrypt(configData.SettingsXml)
				};
				if (SalesHelper.UpdatePaymentMode(paymentMode))
				{
					if (text == "hishop.plugins.payment.wxqrcode.wxqrcoderequest")
					{
						PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode(this.modeId);
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
							if (string.IsNullOrEmpty(xmlNodeValue4) && string.IsNullOrEmpty(xmlNodeValue5))
							{
								this.siteSettings.WeixinAppId = xmlNodeValue;
								this.siteSettings.WeixinPartnerID = xmlNodeValue2;
								this.siteSettings.Main_AppId = "";
								this.siteSettings.Main_Mch_ID = "";
							}
							else
							{
								this.siteSettings.WeixinAppId = xmlNodeValue4;
								this.siteSettings.WeixinPartnerID = xmlNodeValue5;
								this.siteSettings.Main_Mch_ID = xmlNodeValue2;
								this.siteSettings.Main_AppId = xmlNodeValue;
							}
							this.siteSettings.WeixinPartnerKey = xmlNodeValue3;
							this.siteSettings.WeixinCertPath = xmlNodeValue6;
							this.siteSettings.WeixinCertPassword = xmlNodeValue7;
							SettingsManager.Save(this.siteSettings);
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
	}
}
