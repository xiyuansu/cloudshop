using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.App_Code;
using HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.HIPOSSetting)]
	[HiPOSCheck(true)]
	public class SetHiPOS : AdminPage
	{
		private string configXml = "";

		protected Literal txtSellerName;

		protected Literal txtContactName;

		protected Literal txtContactPhone;

		protected Literal txtExpireAt;

		protected OnOff ooZFB;

		protected HtmlGenericControl ulZFB;

		protected TextBox txtZFBPID;

		protected TextBox txtZFBKey;

		protected OnOff ooWX;

		protected HtmlGenericControl ulWX;

		protected TextBox txtWXAppId;

		protected TextBox txtWXMchId;

		protected TextBox txtWXAPIKey;

		protected FileUpload fuWXCertPath;

		protected Label lblShowCertPath;

		protected LinkButton btnAdd;

		protected HiddenField txtZFBConfigData;

		protected HiddenField txtWXConfigData;

		protected HiddenField hdCertPath;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.ooZFB.Parameter.Add("onSwitchChange", "fuCheckEnableZFBPay");
			this.ooWX.Parameter.Add("onSwitchChange", "fuCheckEnableWXPay");
			if (!base.IsPostBack)
			{
				this.BindControl();
			}
		}

		public string GetNodeValue(string xmlString, string nodeName)
		{
			string result = string.Empty;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlString);
			if (!xmlDocument.HasChildNodes)
			{
				return result;
			}
			XmlNode xmlNode = xmlDocument.ChildNodes[0].SelectSingleNode(nodeName);
			if (xmlNode != null)
			{
				result = xmlNode.InnerText;
			}
			return result;
		}

		private void BindControl()
		{
			SiteSettings siteSettings = SettingsManager.GetMasterSettings();
			this.txtContactName.Text = siteSettings.HiPOSContactName;
			this.txtContactPhone.Text = siteSettings.HiPOSContactPhone;
			this.txtSellerName.Text = siteSettings.HiPOSSellerName;
			this.txtExpireAt.Text = siteSettings.HiPOSExpireAt;
			PaymentModeInfo paymentModeInfo = null;
			IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnAll);
			(from c in paymentModes
			where c.Gateway.Contains("alipay")
			select c).ForEach(delegate(PaymentModeInfo alipayPayment)
			{
				paymentModeInfo = SalesHelper.GetPaymentMode(alipayPayment.ModeId);
				Globals.EntityCoding(paymentModeInfo, false);
				ConfigablePlugin configablePlugin2 = PaymentRequest.CreateInstance(paymentModeInfo.Gateway.ToLower());
				if (configablePlugin2 == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					Globals.EntityCoding(paymentModeInfo, false);
					ConfigData configData2 = new ConfigData(HiCryptographer.Decrypt(paymentModeInfo.Settings));
					this.configXml = configData2.SettingsXml;
					this.txtZFBConfigData.Value = configData2.SettingsXml;
					string text5 = this.GetNodeValue(this.txtZFBConfigData.Value, "Partner").Trim();
					if (string.IsNullOrEmpty(this.txtZFBPID.Text))
					{
						this.txtZFBPID.Text = text5;
					}
				}
			});
			(from c in paymentModes
			where c.Gateway.Contains(".wx")
			select c).ForEach(delegate(PaymentModeInfo wxPayment)
			{
				paymentModeInfo = SalesHelper.GetPaymentMode(wxPayment.ModeId);
				Globals.EntityCoding(paymentModeInfo, false);
				ConfigablePlugin configablePlugin = PaymentRequest.CreateInstance(paymentModeInfo.Gateway.ToLower());
				if (configablePlugin == null)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					Globals.EntityCoding(paymentModeInfo, false);
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(paymentModeInfo.Settings));
					this.txtWXConfigData.Value = configData.SettingsXml;
					string text = (!string.IsNullOrEmpty(siteSettings.HiPOSWXAppId)) ? siteSettings.HiPOSWXAppId : this.GetNodeValue(this.txtWXConfigData.Value, "AppId").Trim();
					string text2 = (!string.IsNullOrEmpty(siteSettings.HiPOSWXMchId)) ? siteSettings.HiPOSWXMchId : this.GetNodeValue(this.txtWXConfigData.Value, "MCHID").Trim();
					string text3 = (!string.IsNullOrEmpty(siteSettings.HiPOSWXCertPath)) ? siteSettings.HiPOSWXCertPath : this.GetNodeValue(this.txtWXConfigData.Value, "CertPath").Trim();
					if (string.IsNullOrEmpty(this.txtWXAppId.Text))
					{
						this.txtWXAppId.Text = text;
					}
					if (string.IsNullOrEmpty(this.txtWXMchId.Text))
					{
						this.txtWXMchId.Text = text2;
					}
					if (string.IsNullOrEmpty(this.hdCertPath.Value) && !string.IsNullOrEmpty(text3))
					{
						string text4 = text3.Replace("\\", "/").Replace("//", "/").Replace("/", "\\");
						if (!File.Exists(text4))
						{
							text4 = Globals.PhysicalPath(text3).Replace("\\", "/").Replace("//", "/")
								.Replace("/", "\\");
						}
						this.hdCertPath.Value = text4 + " 删除重传";
						this.fuWXCertPath.Style.Add("display", "none");
					}
				}
			});
			this.ooZFB.SelectedValue = siteSettings.EnableHiPOSZFB;
			this.ooWX.SelectedValue = siteSettings.EnableHiPOSWX;
			if (siteSettings.EnableHiPOSZFB)
			{
				this.txtZFBPID.Text = siteSettings.HiPOSZFBPID;
			}
			else
			{
				this.ulZFB.Style.Add("display", "none");
			}
			if (siteSettings.EnableHiPOSWX)
			{
				this.txtWXAppId.Text = siteSettings.HiPOSWXAppId;
				this.txtWXMchId.Text = siteSettings.HiPOSWXMchId;
				this.txtWXAPIKey.Text = siteSettings.HiPOSWXAPIKey;
			}
			else
			{
				this.ulWX.Style.Add("display", "none");
			}
			if (string.IsNullOrEmpty(this.txtZFBKey.Text.Trim()))
			{
				this.txtZFBKey.Text = this.CreatePubKeyContent();
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			bool selectedValue = this.ooZFB.SelectedValue;
			bool selectedValue2 = this.ooWX.SelectedValue;
			string empty = string.Empty;
			string empty2 = string.Empty;
			string text = this.txtZFBPID.Text;
			string text2 = this.txtZFBKey.Text;
			string text3 = this.txtWXAppId.Text.Trim();
			string text4 = this.txtWXMchId.Text.Trim();
			string text5 = this.txtWXAPIKey.Text.Trim();
			bool flag = false;
			if (string.IsNullOrEmpty(this.hdCertPath.Value))
			{
				flag = true;
			}
			if (selectedValue)
			{
				if (string.IsNullOrEmpty(text))
				{
					this.ShowMsg("支付宝APPID不能为空", false);
					return;
				}
				if (string.IsNullOrEmpty(text2))
				{
					this.ShowMsg("支付宝开发者公钥不能为空", false);
					return;
				}
			}
			if (selectedValue2)
			{
				if (string.IsNullOrEmpty(text3))
				{
					this.ShowMsg("微信AppId不能为空", false);
					return;
				}
				if (string.IsNullOrEmpty(text5))
				{
					this.ShowMsg("微信API密钥不能为空", false);
					return;
				}
				if (string.IsNullOrEmpty(text4))
				{
					this.ShowMsg("微信商户号不能为空", false);
					return;
				}
				if (flag && this.fuWXCertPath.PostedFile.ContentLength == 0)
				{
					this.ShowMsg("请上传微信API证书", false);
					return;
				}
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.EnableHiPOSWX = selectedValue2;
			masterSettings.EnableHiPOSZFB = selectedValue;
			if (masterSettings.EnableHiPOSWX)
			{
				this.ulWX.Style.Add("display", "block");
				masterSettings.HiPOSWXAppId = text3;
				masterSettings.HiPOSWXAPIKey = text5;
				masterSettings.HiPOSWXMchId = text4;
				if (this.fuWXCertPath.PostedFile.ContentLength > 0)
				{
					string text6 = Globals.PhysicalPath("\\Storage\\master\\HiPOS\\");
					if (!Globals.PathExist(text6, false))
					{
						Globals.CreatePath(text6);
					}
					if (Globals.ValidateCertFile(this.fuWXCertPath.PostedFile.FileName))
					{
						string text7 = text6 + this.fuWXCertPath.FileName;
						this.fuWXCertPath.PostedFile.SaveAs(text7);
						masterSettings.HiPOSWXCertPath = text7.Replace("\\", "/").Replace("//", "/").Replace("/", "\\");
						goto IL_0340;
					}
					this.ShowMsg("非法的证书文件", false);
					return;
				}
				if (!string.IsNullOrEmpty(this.hdCertPath.Value) && string.IsNullOrEmpty(masterSettings.HiPOSWXCertPath))
				{
					string text9 = masterSettings.HiPOSWXCertPath = this.hdCertPath.Value.Replace("删除重传", string.Empty).Trim();
				}
			}
			else
			{
				masterSettings.HiPOSWXAppId = string.Empty;
				masterSettings.HiPOSWXCertPath = string.Empty;
				masterSettings.HiPOSWXMchId = string.Empty;
				masterSettings.HiPOSWXAPIKey = string.Empty;
			}
			goto IL_0340;
			IL_0340:
			if (masterSettings.EnableHiPOSZFB)
			{
				this.ulZFB.Style.Add("display", "block");
				masterSettings.HiPOSZFBKey = text2;
				masterSettings.HiPOSZFBPID = text;
			}
			else
			{
				masterSettings.HiPOSZFBKey = string.Empty;
				masterSettings.HiPOSZFBPID = string.Empty;
			}
			SettingsManager.Save(masterSettings);
			string wxPayCert = string.Empty;
			if (File.Exists(masterSettings.HiPOSWXCertPath))
			{
				FileStream fileStream = File.OpenRead(masterSettings.HiPOSWXCertPath);
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				fileStream.Close();
				wxPayCert = Globals.UrlEncode(Convert.ToBase64String(array));
			}
			HiPOSHelper hiPOSHelper = new HiPOSHelper();
			string filename = base.Server.MapPath("~/config/rsa_private_key.pem");
			PaymentsResult paymentsResult = hiPOSHelper.SetPayments(masterSettings.HiPOSAppId, masterSettings.HiPOSMerchantId, masterSettings.HiPOSZFBPID, masterSettings.HiPOSWXAppId, masterSettings.HiPOSWXMchId, masterSettings.HiPOSWXAPIKey, wxPayCert, filename);
			if (paymentsResult.error == null)
			{
				this.ShowMsg("提交商户支付方式成功", true, "SetHiPOS.aspx");
			}
			else
			{
				this.ShowMsg(paymentsResult.error.message, false);
			}
		}

		private string CreateRsaKey()
		{
			string keyDirectory = base.Server.MapPath("~/config");
			string generatorPath = base.Server.MapPath("~/config/RSAGenerator/Rsa.exe");
			return RsaKeyHelper.CreateRSAKeyFile(generatorPath, keyDirectory);
		}

		private string CreatePubKeyContent()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string path = base.Server.MapPath("~/config/rsa_public_key.pem");
			string result;
			if (!File.Exists(path))
			{
				result = this.CreateRsaKey();
				SettingsManager.Save(masterSettings);
			}
			else
			{
				result = RsaKeyHelper.GetRSAKeyContent(path, true);
			}
			return result;
		}
	}
}
