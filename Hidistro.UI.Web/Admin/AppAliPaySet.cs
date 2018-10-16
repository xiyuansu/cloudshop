using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppAliPaySet)]
	public class AppAliPaySet : AdminPage
	{
		protected OnOff radEnableAppAliPay;

		protected AccountNumbersTextBox txtAppPartner;

		protected AccountNumbersTextBox txtAppKey;

		protected AccountNumbersTextBox txtAppAccount;

		protected OnOff radEnableWapAliPay;

		protected AccountNumbersTextBox txtPartner;

		protected AccountNumbersTextBox txtKey;

		protected AccountNumbersTextBox txtPublicKey;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.radEnableAppAliPay.Parameter.Add("onSwitchChange", "fuCheckEnableZFBPay");
			this.radEnableWapAliPay.Parameter.Add("onSwitchChange", "fuCheckEnableZFBPage");
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.radEnableAppAliPay.SelectedValue = masterSettings.EnableAppAliPay;
				this.radEnableWapAliPay.SelectedValue = masterSettings.EnableAppWapAliPay;
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
				if (paymentMode != null)
				{
					string xml = HiCryptographer.Decrypt(paymentMode.Settings);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					this.txtPartner.Text = xmlDocument.FirstChild.SelectSingleNode("Partner").InnerText;
					if (xmlDocument.FirstChild.SelectSingleNode("Key") != null)
					{
						this.txtKey.Text = xmlDocument.FirstChild.SelectSingleNode("Key").InnerText;
					}
					if (xmlDocument.FirstChild.SelectSingleNode("PublicKey") != null)
					{
						this.txtPublicKey.Text = xmlDocument.FirstChild.SelectSingleNode("PublicKey").InnerText;
					}
				}
				PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_apppay.wswappayrequest");
				if (paymentMode2 != null)
				{
					string xml2 = HiCryptographer.Decrypt(paymentMode2.Settings);
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.LoadXml(xml2);
					this.txtAppPartner.Text = xmlDocument2.GetElementsByTagName("Partner")[0].InnerText;
					this.txtAppKey.Text = xmlDocument2.GetElementsByTagName("Key")[0].InnerText;
					this.txtAppAccount.Text = xmlDocument2.GetElementsByTagName("Seller_account_name")[0].InnerText;
				}
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><PublicKey>{2}</PublicKey><Seller_account_name>{3}</Seller_account_name></xml>", this.txtPartner.Text.Trim(), this.txtKey.Text.Trim(), this.txtPublicKey.Text.Trim(), "");
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
			if (this.radEnableWapAliPay.SelectedValue && (string.IsNullOrEmpty(this.txtPartner.Text.Trim()) || string.IsNullOrEmpty(this.txtKey.Text.Trim()) || string.IsNullOrEmpty(this.txtPublicKey.Text.Trim())))
			{
				this.ShowMsg("支付宝H5网页支付是开启状态,请填写正确的配置信息", false);
			}
			else
			{
				if (paymentMode == null)
				{
					PaymentModeInfo paymentMode2 = new PaymentModeInfo
					{
						Name = "支付宝H5网页支付",
						Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest",
						Description = string.Empty,
						IsUseInpour = false,
						Settings = HiCryptographer.Encrypt(text),
						ModeType = ((!masterSettings.EnableAppWapAliPay) ? PayModeType.NoUsed : PayModeType.Pay)
					};
					SalesHelper.CreatePaymentMode(paymentMode2);
				}
				else
				{
					PaymentModeInfo paymentModeInfo = paymentMode;
					paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
					paymentModeInfo.ApplicationType = PayApplicationType.payOnWAP;
					paymentModeInfo.ModeType = ((!masterSettings.EnableWapAliPay && !masterSettings.EnableAppWapAliPay) ? PayModeType.NoUsed : PayModeType.Pay);
					string empty = string.Empty;
					SalesHelper.UpdatePaymentModeSync(paymentModeInfo, out empty);
				}
				string text2 = $"<xml><Partner>{this.txtAppPartner.Text.Trim()}</Partner><Key>{this.txtAppKey.Text.Trim()}</Key><Seller_account_name>{this.txtAppAccount.Text.Trim()}</Seller_account_name></xml>";
				if (this.radEnableAppAliPay.SelectedValue && (string.IsNullOrEmpty(this.txtAppPartner.Text.Trim()) || string.IsNullOrEmpty(this.txtAppKey.Text.Trim()) || string.IsNullOrEmpty(this.txtAppAccount.Text.Trim())))
				{
					this.ShowMsg("支付宝app支付是开启状态,请填写正确的配置信息", false);
				}
				else
				{
					PaymentModeInfo paymentMode3 = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_apppay.wswappayrequest");
					if (paymentMode3 == null)
					{
						PaymentModeInfo paymentMode4 = new PaymentModeInfo
						{
							Name = "支付宝app支付",
							Gateway = "hishop.plugins.payment.ws_apppay.wswappayrequest",
							Description = string.Empty,
							IsUseInpour = false,
							ApplicationType = PayApplicationType.payOnApp,
							Settings = HiCryptographer.Encrypt(text2),
							ModeType = ((!masterSettings.EnableAppAliPay) ? PayModeType.NoUsed : PayModeType.Pay)
						};
						SalesHelper.CreatePaymentMode(paymentMode4);
					}
					else
					{
						PaymentModeInfo paymentModeInfo2 = paymentMode3;
						paymentModeInfo2.Settings = HiCryptographer.Encrypt(text2);
						paymentModeInfo2.ApplicationType = PayApplicationType.payOnApp;
						paymentModeInfo2.ModeType = ((!masterSettings.EnableAppAliPay) ? PayModeType.NoUsed : PayModeType.Pay);
						SalesHelper.UpdatePaymentMode(paymentModeInfo2);
					}
					masterSettings.EnableAppAliPay = this.radEnableAppAliPay.SelectedValue;
					masterSettings.EnableAppWapAliPay = this.radEnableWapAliPay.SelectedValue;
					SettingsManager.Save(masterSettings);
					this.ShowMsg("修改成功", true, "");
				}
			}
		}
	}
}
