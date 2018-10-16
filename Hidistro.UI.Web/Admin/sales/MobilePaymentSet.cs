using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class MobilePaymentSet : AdminPage
	{
		private SiteSettings setting = SettingsManager.GetMasterSettings();

		protected OnOff radEnableAppAliPay;

		protected AccountNumbersTextBox txtPartner;

		protected AccountNumbersTextBox txtKey;

		protected AccountNumbersTextBox txtPublicKey;

		protected AccountNumbersTextBox txtAccount;

		protected HtmlGenericControl divOnlyForWap;

		protected OnOff OnoffYLQQD;

		protected AccountNumbersTextBox txtYLQQDPartner;

		protected FileUpload fileBankUnionCert;

		protected AccountNumbersTextBox txtYLQQDCerFileName;

		protected AccountNumbersTextBox txtYLQQDKey;

		protected OnOff OnoffKJZFB;

		protected AccountNumbersTextBox txtKJZFBKey;

		protected AccountNumbersTextBox txtSellerEmail;

		protected AccountNumbersTextBox txtKJZFBPartner;

		protected OnOff ooRMB;

		protected AccountNumbersTextBox txtCurrency;

		protected OnOff OnoffSFT;

		protected AccountNumbersTextBox txtSPPartner;

		protected AccountNumbersTextBox txtSPKey;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.setting.OpenVstore == 0 && this.setting.OpenWap == 0 && this.setting.OpenAliho == 0)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied?errormsg=抱歉，您暂未开通此服务！"), true);
			}
			if (this.setting.OpenWap == 0)
			{
				this.divOnlyForWap.Visible = false;
			}
			this.radEnableAppAliPay.Parameter.Add("onSwitchChange", "funCheckEnableZFBPay");
			this.OnoffYLQQD.Parameter.Add("onSwitchChange", "funCheckEnableYLQQD");
			this.OnoffSFT.Parameter.Add("onSwitchChange", "funCheckEnableSFT");
			this.OnoffKJZFB.Parameter.Add("onSwitchChange", "funCheckEnableKJZFB");
			if (!this.Page.IsPostBack)
			{
				this.radEnableAppAliPay.SelectedValue = (this.setting.EnableWapAliPay || this.setting.EnableWeixinWapAliPay);
				this.OnoffYLQQD.SelectedValue = this.setting.EnableBankUnionPay;
				this.OnoffSFT.SelectedValue = this.setting.EnableWapShengPay;
				this.OnoffKJZFB.SelectedValue = this.setting.EnableWapAliPayCrossBorder;
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
				if (paymentMode != null && (this.setting.EnableWapAliPay || this.setting.EnableWeixinWapAliPay))
				{
					string xml = HiCryptographer.Decrypt(paymentMode.Settings);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					this.txtPartner.Text = xmlDocument.FirstChild.SelectSingleNode("Partner").InnerText;
					this.txtKey.Text = xmlDocument.FirstChild.SelectSingleNode("Key").InnerText;
					if (xmlDocument.FirstChild.SelectSingleNode("Seller_account_name") != null)
					{
						this.txtAccount.Text = xmlDocument.FirstChild.SelectSingleNode("Seller_account_name").InnerText;
					}
					if (xmlDocument.FirstChild.SelectSingleNode("PublicKey") != null)
					{
						this.txtPublicKey.Text = xmlDocument.FirstChild.SelectSingleNode("PublicKey").InnerText;
					}
				}
				PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest");
				if (paymentMode2 != null)
				{
					string xml2 = HiCryptographer.Decrypt(paymentMode2.Settings);
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.LoadXml(xml2);
					try
					{
						this.txtYLQQDPartner.Text = xmlDocument2.GetElementsByTagName("Vmid")[0].InnerText;
						this.txtYLQQDKey.Text = xmlDocument2.GetElementsByTagName("Key")[0].InnerText;
						this.txtYLQQDCerFileName.Text = xmlDocument2.GetElementsByTagName("SignCertFileName")[0].InnerText;
					}
					catch
					{
						this.txtYLQQDPartner.Text = "";
						this.txtYLQQDKey.Text = "";
					}
				}
				PaymentModeInfo paymentMode3 = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
				if (paymentMode3 != null)
				{
					string xml3 = HiCryptographer.Decrypt(paymentMode3.Settings);
					XmlDocument xmlDocument3 = new XmlDocument();
					xmlDocument3.LoadXml(xml3);
					try
					{
						this.txtSPPartner.Text = xmlDocument3.GetElementsByTagName("SenderId")[0].InnerText;
						this.txtSPKey.Text = xmlDocument3.GetElementsByTagName("SellerKey")[0].InnerText;
					}
					catch
					{
						this.txtSPPartner.Text = "";
						this.txtSPKey.Text = "";
					}
				}
				PaymentModeInfo paymentMode4 = SalesHelper.GetPaymentMode("hishop.plugins.payment.alipaycrossbordermobilepayment.alipaycrossbordermobilepaymentrequest");
				if (paymentMode4 != null)
				{
					string xml4 = HiCryptographer.Decrypt(paymentMode4.Settings);
					XmlDocument xmlDocument4 = new XmlDocument();
					xmlDocument4.LoadXml(xml4);
					this.txtKJZFBKey.Text = xmlDocument4.GetElementsByTagName("Key")[0].InnerText;
					this.txtKJZFBPartner.Text = xmlDocument4.GetElementsByTagName("Partner")[0].InnerText;
					this.txtSellerEmail.Text = xmlDocument4.GetElementsByTagName("SellerEmail")[0].InnerText;
					this.ooRMB.SelectedValue = (xmlDocument4.GetElementsByTagName("RMB")[0].InnerText == "1");
					this.txtCurrency.Text = xmlDocument4.GetElementsByTagName("Currency")[0].InnerText;
				}
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			string text = string.Format("<xml><Partner>{0}</Partner><Key>{1}</Key><PublicKey>{3}</PublicKey><Seller_account_name>{2}</Seller_account_name></xml>", this.txtPartner.Text.Trim(), this.txtKey.Text.Trim(), this.txtAccount.Text.Trim(), this.txtPublicKey.Text.Trim());
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
			if (this.radEnableAppAliPay.SelectedValue)
			{
				if (string.IsNullOrEmpty(this.txtPartner.Text.Trim()) || string.IsNullOrEmpty(this.txtKey.Text.Trim()) || string.IsNullOrEmpty(this.txtPublicKey.Text.Trim()))
				{
					this.ShowMsg("支付宝H5网页支付为开启状态,请填写正确的配置信息", false);
					return;
				}
				if (paymentMode == null)
				{
					PaymentModeInfo paymentMode2 = new PaymentModeInfo
					{
						Name = "支付宝H5网页支付",
						Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest",
						Description = string.Empty,
						IsUseInpour = false,
						ApplicationType = PayApplicationType.payOnWAP,
						Settings = HiCryptographer.Encrypt(text),
						ModeType = ((!this.setting.EnableWapAliPay) ? PayModeType.NoUsed : PayModeType.Pay)
					};
					SalesHelper.CreatePaymentMode(paymentMode2);
					this.setting.EnableWapAliPay = this.radEnableAppAliPay.SelectedValue;
					this.setting.EnableWeixinWapAliPay = this.radEnableAppAliPay.SelectedValue;
					this.setting.EnableAppWapAliPay = this.radEnableAppAliPay.SelectedValue;
				}
				else
				{
					PaymentModeInfo paymentModeInfo = paymentMode;
					paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
					paymentModeInfo.ApplicationType = PayApplicationType.payOnWAP;
					paymentModeInfo.ModeType = ((!this.setting.EnableWapAliPay && !this.setting.EnableAppWapAliPay && !this.setting.EnableWeixinWapAliPay) ? PayModeType.NoUsed : PayModeType.Pay);
					this.setting.EnableWapAliPay = this.radEnableAppAliPay.SelectedValue;
					this.setting.EnableWeixinWapAliPay = this.radEnableAppAliPay.SelectedValue;
					SalesHelper.UpdatePaymentMode(paymentModeInfo);
				}
				string gateway = "hishop.plugins.payment.alipaywx.alipaywxrequest";
				string text2 = "<xml><Key>{Key}</Key><SellerEmail>{SellerEmail}</SellerEmail><Partner>{Partner}</Partner><PublicKey>{PublicKey}</PublicKey></xml>";
				string text3 = text2;
				text3 = text3.Replace("{Key}", this.txtKey.Text.Trim());
				text3 = text3.Replace("{SellerEmail}", this.txtAccount.Text.Trim());
				text3 = text3.Replace("{Partner}", this.txtPartner.Text.Trim());
				text3 = text3.Replace("{PublicKey}", this.txtPublicKey.Text.Trim());
				PaymentModeInfo paymentMode3 = SalesHelper.GetPaymentMode(gateway);
				if (paymentMode3 == null)
				{
					PaymentModeInfo paymentMode4 = new PaymentModeInfo
					{
						Name = "支付宝微信端支付",
						Gateway = gateway,
						Description = string.Empty,
						IsUseInpour = false,
						ApplicationType = PayApplicationType.payOnVX,
						Settings = HiCryptographer.Encrypt(text3)
					};
					SalesHelper.CreatePaymentMode(paymentMode4);
					this.setting.EnableWeixinWapAliPay = this.radEnableAppAliPay.SelectedValue;
				}
				else
				{
					PaymentModeInfo paymentModeInfo2 = paymentMode3;
					paymentModeInfo2.Settings = HiCryptographer.Encrypt(text3);
					paymentModeInfo2.ApplicationType = PayApplicationType.payOnVX;
					SalesHelper.UpdatePaymentMode(paymentModeInfo2);
					this.setting.EnableWeixinWapAliPay = this.radEnableAppAliPay.SelectedValue;
					string empty = string.Empty;
				}
			}
			else
			{
				this.setting.EnableWeixinWapAliPay = this.radEnableAppAliPay.SelectedValue;
				this.setting.EnableWapAliPay = this.radEnableAppAliPay.SelectedValue;
				this.setting.EnableAppWapAliPay = this.radEnableAppAliPay.SelectedValue;
			}
			if (this.OnoffYLQQD.SelectedValue)
			{
				if (string.IsNullOrEmpty(this.txtYLQQDPartner.Text))
				{
					this.ShowMsg("银联全渠道支付商户号不能为空", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtYLQQDKey.Text))
				{
					this.ShowMsg("银联全渠道支付商户密钥不能为空", false);
					return;
				}
				this.setting.EnableBankUnionPay = this.OnoffYLQQD.SelectedValue;
				string arg = this.txtYLQQDCerFileName.Text;
				if (this.fileBankUnionCert.HasFile)
				{
					if (!Globals.ValidateCertFile(this.fileBankUnionCert.PostedFile.FileName))
					{
						this.ShowMsg("银联全渠道支付非法的证书文件", false);
						return;
					}
					this.fileBankUnionCert.PostedFile.SaveAs(base.Server.MapPath("~/config/") + this.fileBankUnionCert.PostedFile.FileName);
					arg = this.fileBankUnionCert.PostedFile.FileName;
				}
				string text4 = $"<xml><Vmid>{this.txtYLQQDPartner.Text.Trim()}</Vmid><SignCertFileName>{arg}</SignCertFileName><Key>{this.txtYLQQDKey.Text.Trim()}</Key></xml>";
				PaymentModeInfo paymentMode5 = SalesHelper.GetPaymentMode("hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest");
				if (paymentMode5 == null)
				{
					PaymentModeInfo paymentMode6 = new PaymentModeInfo
					{
						Name = "银联全渠道支付",
						Gateway = "hishop.plugins.payment.bankuniongateway.bankuniongetwayrequest",
						Description = string.Empty,
						IsUseInpour = false,
						ApplicationType = PayApplicationType.payOnPC,
						Settings = HiCryptographer.Encrypt(text4)
					};
					SalesHelper.CreatePaymentMode(paymentMode6);
				}
				else
				{
					PaymentModeInfo paymentModeInfo3 = paymentMode5;
					paymentModeInfo3.Settings = HiCryptographer.Encrypt(text4);
					paymentModeInfo3.ApplicationType = PayApplicationType.payOnPC;
					SalesHelper.UpdatePaymentMode(paymentModeInfo3);
				}
			}
			else
			{
				this.setting.EnableBankUnionPay = this.OnoffYLQQD.SelectedValue;
			}
			if (this.OnoffSFT.SelectedValue)
			{
				if (string.IsNullOrEmpty(this.txtSPPartner.Text))
				{
					this.ShowMsg("盛付通支付商户号不能为空", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtSPKey.Text))
				{
					this.ShowMsg("盛付通支付商户密钥不能为空", false);
					return;
				}
				this.setting.EnableWapShengPay = this.OnoffSFT.SelectedValue;
				string text5 = $"<xml><SenderId>{this.txtSPPartner.Text}</SenderId><SellerKey>{this.txtSPKey.Text}</SellerKey><Seller_account_name></Seller_account_name></xml>";
				PaymentModeInfo paymentMode7 = SalesHelper.GetPaymentMode("Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest");
				if (paymentMode7 == null)
				{
					PaymentModeInfo paymentMode8 = new PaymentModeInfo
					{
						Name = "盛付通手机网页支付",
						Gateway = "Hishop.Plugins.Payment.ShengPayMobile.ShengPayMobileRequest",
						Description = string.Empty,
						IsUseInpour = false,
						ApplicationType = PayApplicationType.payOnWAP,
						Settings = HiCryptographer.Encrypt(text5)
					};
					SalesHelper.CreatePaymentMode(paymentMode8);
				}
				else
				{
					PaymentModeInfo paymentModeInfo4 = paymentMode7;
					paymentModeInfo4.Settings = HiCryptographer.Encrypt(text5);
					paymentModeInfo4.ApplicationType = PayApplicationType.payOnWAP;
					SalesHelper.UpdatePaymentMode(paymentModeInfo4);
				}
			}
			else
			{
				this.setting.EnableWapShengPay = false;
			}
			if (this.OnoffKJZFB.SelectedValue)
			{
				if (string.IsNullOrEmpty(this.txtKJZFBKey.Text))
				{
					this.ShowMsg("跨境支付宝支付安全校验码不能为空！", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtSellerEmail.Text))
				{
					this.ShowMsg("跨境支付宝支付收款支付宝账号不能为空！", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtKJZFBPartner.Text))
				{
					this.ShowMsg("跨境支付宝支付合作者身份不能为空！", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtCurrency.Text))
				{
					this.ShowMsg("跨境支付宝支付币种不能为空！", false);
					return;
				}
				string text6 = "<xml><Key>{Key}</Key><SellerEmail>{SellerEmail}</SellerEmail><Partner>{Partner}</Partner><RMB>{RMB}</RMB><Currency>{Currency}</Currency></xml>";
				text6 = text6.Replace("{Key}", this.txtKJZFBKey.Text.Trim());
				text6 = text6.Replace("{SellerEmail}", this.txtSellerEmail.Text.Trim());
				text6 = text6.Replace("{Partner}", this.txtKJZFBPartner.Text.Trim());
				text6 = text6.Replace("{RMB}", this.ooRMB.SelectedValue ? "1" : "0");
				text6 = text6.Replace("{Currency}", this.txtCurrency.Text.Trim());
				PaymentModeInfo paymentMode9 = SalesHelper.GetPaymentMode("hishop.plugins.payment.alipaycrossbordermobilepayment.alipaycrossbordermobilepaymentrequest");
				if (paymentMode9 == null)
				{
					PaymentModeInfo paymentMode10 = new PaymentModeInfo
					{
						Name = "跨境支付宝支付",
						Gateway = "hishop.plugins.payment.alipaycrossbordermobilepayment.alipaycrossbordermobilepaymentrequest",
						Description = string.Empty,
						IsUseInpour = false,
						ApplicationType = PayApplicationType.payOnVX,
						Settings = HiCryptographer.Encrypt(text6)
					};
					SalesHelper.CreatePaymentMode(paymentMode10);
				}
				else
				{
					PaymentModeInfo paymentModeInfo5 = paymentMode9;
					paymentModeInfo5.Settings = HiCryptographer.Encrypt(text6);
					paymentModeInfo5.ApplicationType = PayApplicationType.payOnVX;
					SalesHelper.UpdatePaymentMode(paymentModeInfo5);
				}
				this.setting.EnableWapAliPayCrossBorder = this.OnoffKJZFB.SelectedValue;
			}
			else
			{
				this.setting.EnableWapAliPayCrossBorder = this.OnoffKJZFB.SelectedValue;
			}
			SettingsManager.Save(this.setting);
			this.ShowMsg("保存支付设置成功", true, "");
		}

		public string GetNewFileName(string filename)
		{
			string result = filename;
			int num = filename.LastIndexOf(".");
			if (num > -1 && num < filename.Length - 1)
			{
				string str = filename.Substring(num + 1);
				string str2 = filename.Substring(0, num);
				result = str2 + DateTime.Now.ToString("MMddHHmmssfff") + "." + str;
			}
			return result;
		}
	}
}
