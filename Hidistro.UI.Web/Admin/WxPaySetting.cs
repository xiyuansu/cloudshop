using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class WxPaySetting : AdminPage
	{
		private SiteSettings setting = SettingsManager.GetMasterSettings();

		private PaymentModeInfo wxQrCodePay = null;

		protected OnOff radEnableWxPay;

		protected OnOff radEnableWxH5Pay;

		protected OnOff radEnableAppWxPay;

		protected OnOff radEnableWxQrcodePay;

		protected AccountNumbersTextBox txtAppId;

		protected AccountNumbersTextBox txtAppSecret;

		protected AccountNumbersTextBox txtPartnerID;

		protected AccountNumbersTextBox txtPartnerKey;

		protected OnOff OnOffServiceMode;

		protected AccountNumbersTextBox txtMainMchID;

		protected AccountNumbersTextBox txtMainAppID;

		protected AccountNumbersTextBox txtPaySignKey;

		protected FileUpload file_UploadCert;

		protected HtmlGenericControl liCertFileName;

		protected TrimTextBox txtCerFileName;

		protected AccountNumbersTextBox txtCertPassword;

		protected OnOff OnOffPcInpour;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.setting.OpenVstore == 0 && this.setting.OpenWap == 0 && this.setting.OpenAliho == 0 && !this.setting.OpenPcShop && this.setting.OpenMobbile != 1)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied?errormsg=抱歉，您暂未开通此服务！"), true);
			}
			if (this.setting.OpenVstore == 0)
			{
				this.radEnableWxPay.SelectedValue = false;
				this.radEnableWxPay.Enabled = false;
			}
			if (this.setting.OpenWap == 0 && this.setting.OpenAliho == 0)
			{
				this.radEnableWxH5Pay.SelectedValue = false;
				this.radEnableWxH5Pay.Enabled = false;
			}
			if (!this.setting.OpenPcShop)
			{
				this.radEnableWxQrcodePay.SelectedValue = false;
				this.radEnableWxQrcodePay.Enabled = false;
			}
			this.radEnableWxPay.Parameter.Add("onSwitchChange", "funCheckEnableWxPay");
			this.radEnableAppWxPay.Parameter.Add("onSwitchChange", "funCheckEnableAppWxPay");
			this.radEnableWxH5Pay.Parameter.Add("onSwitchChange", "funCheckEnableWxH5Pay");
			this.radEnableWxQrcodePay.Parameter.Add("onSwitchChange", "funCheckEnableWxQrcodePay");
			this.OnOffServiceMode.Parameter.Add("onSwitchChange", "funCheckOnOffServiceMode");
			this.wxQrCodePay = SalesHelper.GetPaymentMode("hishop.plugins.payment.wxqrcode.wxqrcoderequest");
			if (!this.Page.IsPostBack)
			{
				this.radEnableWxPay.SelectedValue = (this.setting.EnableWeiXinRequest && this.setting.OpenVstore == 1);
				this.radEnableWxH5Pay.SelectedValue = (this.setting.EnableWapWeiXinPay && (this.setting.OpenWap == 1 || this.setting.OpenAliho == 1));
				this.radEnableAppWxPay.SelectedValue = (this.setting.OpenAppWxPay && this.setting.OpenMobbile == 1);
				if (string.IsNullOrEmpty(this.setting.Main_Mch_ID))
				{
					this.OnOffServiceMode.SelectedValue = false;
				}
				else
				{
					this.OnOffServiceMode.SelectedValue = true;
				}
				if (this.wxQrCodePay != null && this.setting.OpenPcShop)
				{
					this.radEnableWxQrcodePay.SelectedValue = true;
					if (this.wxQrCodePay.IsUseInpour)
					{
						this.OnOffPcInpour.SelectedValue = true;
					}
					else
					{
						this.OnOffPcInpour.SelectedValue = false;
					}
				}
				else
				{
					this.radEnableWxQrcodePay.SelectedValue = false;
				}
				this.txtAppId.Text = this.setting.WeixinAppId;
				string weixinPartnerKey = this.setting.WeixinPartnerKey;
				if (weixinPartnerKey.Length > 6)
				{
					this.txtPartnerKey.Text = weixinPartnerKey.Substring(0, 3) + Globals.GetHiddenStr(weixinPartnerKey.Length - 6, "*") + weixinPartnerKey.Substring(weixinPartnerKey.Length - 3);
				}
				this.txtMainMchID.Text = this.setting.Main_Mch_ID;
				this.txtMainAppID.Text = this.setting.Main_AppId;
				this.txtPartnerID.Text = this.setting.WeixinPartnerID;
				this.txtAppSecret.Text = this.setting.WeixinAppSecret;
				this.txtPaySignKey.Text = this.setting.WeixinPaySignKey;
				if (string.IsNullOrEmpty(this.setting.WeixinCertPath))
				{
					this.liCertFileName.Visible = false;
				}
				else
				{
					this.liCertFileName.Visible = true;
					this.txtCerFileName.Text = this.setting.WeixinCertPath;
				}
				this.txtCertPassword.Text = this.setting.WeixinCertPassword;
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsDemoSite)
			{
				this.ShowMsg("演示站不允许修改支付配置信息", false);
			}
			else
			{
				if (this.radEnableWxPay.SelectedValue || this.radEnableWxH5Pay.SelectedValue)
				{
					if (string.IsNullOrEmpty(this.txtAppId.Text))
					{
						this.ShowMsg("微信支付AppId不能为空！", false);
						return;
					}
					if (string.IsNullOrEmpty(this.txtAppSecret.Text))
					{
						this.ShowMsg("微信支付AppSecret不能为空！", false);
						return;
					}
					if (string.IsNullOrEmpty(this.txtPartnerID.Text))
					{
						this.ShowMsg("微信支付mch_id不能为空！", false);
						return;
					}
					if (string.IsNullOrEmpty(this.txtPartnerKey.Text))
					{
						this.ShowMsg("微信支付Key不能为空！", false);
						return;
					}
				}
				string weixinCertPath = this.txtCerFileName.Text;
				if (this.file_UploadCert.HasFile)
				{
					if (!Globals.ValidateCertFile(this.file_UploadCert.PostedFile.FileName))
					{
						this.ShowMsg("非法的证书文件", false);
						return;
					}
					string newFileName = this.GetNewFileName(this.file_UploadCert.PostedFile.FileName);
					this.file_UploadCert.PostedFile.SaveAs(base.Server.MapPath("~/pay/cert/") + newFileName);
					weixinCertPath = "/pay/cert/" + newFileName;
				}
				string weixinAppId = this.setting.WeixinAppId;
				this.setting.WeixinAppId = this.txtAppId.Text;
				this.setting.WeixinAppSecret = this.txtAppSecret.Text;
				this.setting.WeixinPartnerID = this.txtPartnerID.Text;
				if (this.txtPartnerKey.Text.Replace("*", "").Length != 6)
				{
					this.setting.WeixinPartnerKey = this.txtPartnerKey.Text;
				}
				this.setting.WeixinPaySignKey = this.txtPaySignKey.Text;
				this.setting.EnableWeiXinRequest = this.radEnableWxPay.SelectedValue;
				this.setting.EnableWapWeiXinPay = this.radEnableWxH5Pay.SelectedValue;
				this.setting.OpenAppWxPay = this.radEnableAppWxPay.SelectedValue;
				this.setting.WeixinCertPath = weixinCertPath;
				if (this.OnOffServiceMode.SelectedValue)
				{
					string text = Globals.StripAllTags(this.txtMainAppID.Text);
					string text2 = Globals.StripAllTags(this.txtMainMchID.Text);
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						this.ShowMsg("开启了服务商模式,请输入服务商商户ID和服务商AppID", false);
						return;
					}
					this.setting.Main_AppId = text;
					this.setting.Main_Mch_ID = text2;
				}
				else
				{
					this.setting.Main_AppId = "";
					this.setting.Main_Mch_ID = "";
				}
				if (string.IsNullOrEmpty(this.setting.Main_Mch_ID))
				{
					this.setting.WeixinCertPassword = this.txtPartnerID.Text;
				}
				else
				{
					this.setting.WeixinCertPassword = this.setting.Main_Mch_ID;
				}
				if (weixinAppId != this.setting.WeixinAppId)
				{
					WXFansHelper.ClearFansData();
					this.setting.IsInitWXFansInteractData = false;
					this.setting.IsInitWXFansData = false;
				}
				if (this.radEnableWxQrcodePay.SelectedValue)
				{
					string text3 = "";
					text3 = ((string.IsNullOrEmpty(this.setting.Main_AppId) || string.IsNullOrEmpty(this.setting.Main_Mch_ID)) ? $"<xml><AppId>{this.setting.WeixinAppId}</AppId><MCHID>{this.setting.WeixinPartnerID}</MCHID><AppSecret>{this.setting.WeixinPartnerKey}</AppSecret><CertPath>{this.setting.WeixinCertPath}</CertPath></xml>" : $"<xml><AppId>{this.setting.Main_AppId}</AppId><MCHID>{this.setting.Main_Mch_ID}</MCHID><AppSecret>{this.setting.WeixinPartnerKey}</AppSecret><Sub_AppId>{this.setting.WeixinAppId}</Sub_AppId><Sub_mch_Id>{this.setting.WeixinPartnerID}</Sub_mch_Id><CertPath>{this.setting.WeixinCertPath}</CertPath></xml>");
					if (this.wxQrCodePay == null)
					{
						PaymentModeInfo paymentMode = new PaymentModeInfo
						{
							Name = "微信扫码支付",
							Gateway = "hishop.plugins.payment.wxqrcode.wxqrcoderequest",
							Description = string.Empty,
							IsUseInpour = this.OnOffPcInpour.SelectedValue,
							ApplicationType = PayApplicationType.payOnPC,
							Settings = HiCryptographer.Encrypt(text3)
						};
						SalesHelper.CreatePaymentMode(paymentMode);
					}
					else
					{
						PaymentModeInfo paymentModeInfo = this.wxQrCodePay;
						paymentModeInfo.Settings = HiCryptographer.Encrypt(text3);
						paymentModeInfo.ApplicationType = PayApplicationType.payOnPC;
						paymentModeInfo.IsUseInpour = this.OnOffPcInpour.SelectedValue;
						SalesHelper.UpdatePaymentMode(paymentModeInfo);
					}
				}
				else if (this.wxQrCodePay != null)
				{
					SalesHelper.DeletePaymentMode(this.wxQrCodePay.ModeId);
				}
				SettingsManager.Save(this.setting);
				this.ShowMsg("保存支付设置成功", true, "");
			}
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
