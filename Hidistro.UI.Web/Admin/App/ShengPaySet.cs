using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.App
{
	[PrivilegeCheck(Privilege.AppShengPaySet)]
	public class ShengPaySet : AdminPage
	{
		protected OnOff radEnableAppShengPay;

		protected TextBox txtPartner;

		protected TextBox txtKey;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnOK.Click += this.btnOK_Click;
			this.radEnableAppShengPay.Parameter.Add("onSwitchChange", "fuCheckEnableShengPay");
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.radEnableAppShengPay.SelectedValue = masterSettings.EnableAppShengPay;
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
				if (paymentMode != null)
				{
					string xml = HiCryptographer.Decrypt(paymentMode.Settings);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					try
					{
						this.txtPartner.Text = xmlDocument.GetElementsByTagName("SenderId")[0].InnerText;
						this.txtKey.Text = xmlDocument.GetElementsByTagName("SellerKey")[0].InnerText;
					}
					catch
					{
						this.txtPartner.Text = "";
						this.txtKey.Text = "";
					}
				}
				PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
				if (paymentMode2 != null)
				{
					string xml2 = HiCryptographer.Decrypt(paymentMode2.Settings);
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.LoadXml(xml2);
				}
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.radEnableAppShengPay.SelectedValue)
			{
				if (string.IsNullOrEmpty(this.txtPartner.Text))
				{
					this.ShowMsg("商户号不能为空", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtKey.Text))
				{
					this.ShowMsg("商户密钥不能为空", false);
					return;
				}
			}
			masterSettings.EnableAppShengPay = this.radEnableAppShengPay.SelectedValue;
			SettingsManager.Save(masterSettings);
			string text = $"<xml><SenderId>{this.txtPartner.Text}</SenderId><SellerKey>{this.txtKey.Text}</SellerKey><Seller_account_name></Seller_account_name></xml>";
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
			if (paymentMode == null)
			{
				PaymentModeInfo paymentMode2 = new PaymentModeInfo
				{
					Name = "盛付通手机网页支付",
					Gateway = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",
					Description = string.Empty,
					IsUseInpour = false,
					ApplicationType = PayApplicationType.payOnWAP,
					Settings = HiCryptographer.Encrypt(text)
				};
				SalesHelper.CreatePaymentMode(paymentMode2);
			}
			else
			{
				PaymentModeInfo paymentModeInfo = paymentMode;
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				paymentModeInfo.ApplicationType = PayApplicationType.payOnWAP;
				SalesHelper.UpdatePaymentMode(paymentModeInfo);
			}
			this.ShowMsg("修改成功", true);
		}
	}
}
