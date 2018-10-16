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
	[PrivilegeCheck(Privilege.AppBankUnionSet)]
	public class AppBankUnionSet : AdminPage
	{
		protected OnOff radEnableWapBankUnionPay;

		protected TextBox txtPartner;

		protected FileUpload fileBankUnionCert;

		protected TextBox txtCerFileName;

		protected TextBox txtKey;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnOK.Click += this.btnOK_Click;
			this.radEnableWapBankUnionPay.Parameter.Add("onSwitchChange", "fuCheckEnableUnionPay");
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.radEnableWapBankUnionPay.SelectedValue = masterSettings.EnableAPPBankUnionPay;
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest");
				if (paymentMode != null)
				{
					string xml = HiCryptographer.Decrypt(paymentMode.Settings);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					try
					{
						this.txtPartner.Text = xmlDocument.GetElementsByTagName("Vmid")[0].InnerText;
						this.txtKey.Text = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
						this.txtCerFileName.Text = xmlDocument.GetElementsByTagName("SignCertFileName")[0].InnerText;
					}
					catch
					{
						this.txtPartner.Text = "";
						this.txtKey.Text = "";
					}
				}
				PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest");
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
			if (this.radEnableWapBankUnionPay.SelectedValue)
			{
				if (string.IsNullOrEmpty(this.txtPartner.Text))
				{
					this.ShowMsg("商户号不能为空", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtKey.Text))
				{
					this.ShowMsg("证书密码不能为空", false);
					return;
				}
			}
			masterSettings.EnableAPPBankUnionPay = this.radEnableWapBankUnionPay.SelectedValue;
			SettingsManager.Save(masterSettings);
			string arg = this.txtCerFileName.Text;
			if (this.fileBankUnionCert.HasFile)
			{
				if (!Globals.ValidateCertFile(this.fileBankUnionCert.PostedFile.FileName))
				{
					this.ShowMsg("非法的证书文件", false);
					return;
				}
				this.fileBankUnionCert.PostedFile.SaveAs(base.Server.MapPath("~/config/") + this.fileBankUnionCert.PostedFile.FileName);
				arg = this.fileBankUnionCert.PostedFile.FileName;
			}
			string text = $"<xml><Vmid>{this.txtPartner.Text.Trim()}</Vmid><SignCertFileName>{arg}</SignCertFileName><Key>{this.txtKey.Text.Trim()}</Key></xml>";
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest");
			if (paymentMode == null)
			{
				PaymentModeInfo paymentMode2 = new PaymentModeInfo
				{
					Name = "银联全渠道支付",
					Gateway = "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest",
					Description = string.Empty,
					IsUseInpour = false,
					ApplicationType = PayApplicationType.payOnPC,
					Settings = HiCryptographer.Encrypt(text)
				};
				SalesHelper.CreatePaymentMode(paymentMode2);
			}
			else
			{
				PaymentModeInfo paymentModeInfo = paymentMode;
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				paymentModeInfo.ApplicationType = PayApplicationType.payOnPC;
				SalesHelper.UpdatePaymentMode(paymentModeInfo);
			}
			this.ShowMsg("修改成功", true, "");
		}
	}
}
