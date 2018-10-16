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
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.SiteSettings)]
	public class CashSetting : AdminPage
	{
		protected HtmlGenericControl formitem;

		protected TextBox txtMinimumSingleShot;

		protected OnOff ooEnableBulkPaymentAliPay;

		protected TextBox txtAlipayPartner;

		protected TextBox txtAlipayKey;

		protected TextBox txtAlipayEmail;

		protected TextBox txtAlipayAccountName;

		protected OnOff ooEnableBulkPaymentWeixin;

		protected TextBox txtWeixinMchAppid;

		protected TextBox txtWeixinMchid;

		protected TextBox txtWeixinKey;

		protected DropDownList ddlWeixinCheckName;

		protected FileUpload fileWeixinCertPath;

		protected HtmlGenericControl divWeixinCertPathHtml;

		protected HiddenField hidWeixinCertPath;

		protected Button btnSave;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += this.btnSave_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.ddlWeixinCheckName.Items.Add(new ListItem("不校验真实姓名", "NO_CHECK"));
				this.ddlWeixinCheckName.Items.Add(new ListItem("强制校验", "FORCE_CHECK"));
				this.ddlWeixinCheckName.Items.Add(new ListItem("通过实名验证则校验", "OPTION_CHECK"));
				this.BeLoad();
			}
		}

		private void BeLoad()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.ooEnableBulkPaymentAliPay.SelectedValue = masterSettings.EnableBulkPaymentAliPay;
			this.ooEnableBulkPaymentWeixin.SelectedValue = masterSettings.EnableBulkPaymentWeixin;
			this.txtMinimumSingleShot.Text = masterSettings.MinimumSingleShot.F2ToString("f2");
			IList<PaymentModeInfo> outPaymentModes = SalesHelper.GetOutPaymentModes();
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (PaymentModeInfo item in outPaymentModes)
			{
				if (item.Gateway.ToLower().Contains(".outpay.alipay."))
				{
					text = item.Settings;
				}
				if (item.Gateway.ToLower().Contains(".outpay.weixin."))
				{
					text2 = item.Settings;
				}
			}
			if (!string.IsNullOrEmpty(text.Trim()))
			{
				ConfigData configData = new ConfigData(HiCryptographer.Decrypt(text));
				string settingsXml = configData.SettingsXml;
				this.txtAlipayAccountName.Text = Globals.GetXmlNodeValue(settingsXml, "account_name").ToNullString();
				this.txtAlipayEmail.Text = Globals.GetXmlNodeValue(settingsXml, "email").ToNullString();
				this.txtAlipayKey.Text = Globals.GetXmlNodeValue(settingsXml, "key").ToNullString();
				this.txtAlipayPartner.Text = Globals.GetXmlNodeValue(settingsXml, "partner").ToNullString();
			}
			if (!string.IsNullOrEmpty(text2.Trim()))
			{
				ConfigData configData2 = new ConfigData(HiCryptographer.Decrypt(text2));
				string settingsXml2 = configData2.SettingsXml;
				string text3 = Globals.GetXmlNodeValue(settingsXml2, "Key").ToNullString();
				if (!string.IsNullOrEmpty(text3))
				{
					if (text3.Length > 6)
					{
						this.txtWeixinKey.Text = "";
						TextBox textBox = this.txtWeixinKey;
						textBox.Text += text3.Substring(0, 3);
						for (int i = 0; i < text3.Length - 6; i++)
						{
							TextBox textBox2 = this.txtWeixinKey;
							textBox2.Text += "*";
						}
						TextBox textBox3 = this.txtWeixinKey;
						textBox3.Text += text3.Substring(text3.Length - 3);
					}
					else
					{
						for (int j = 0; j < text3.Length; j++)
						{
							TextBox textBox4 = this.txtWeixinKey;
							textBox4.Text += "*";
						}
					}
				}
				this.ViewState["WeixinKey"] = text3;
				this.ViewState["WeixinKeyCover"] = this.txtWeixinKey.Text;
				this.txtWeixinMchAppid.Text = Globals.GetXmlNodeValue(settingsXml2, "mch_appid").ToNullString();
				this.txtWeixinMchid.Text = Globals.GetXmlNodeValue(settingsXml2, "mchid").ToNullString();
				this.ddlWeixinCheckName.SelectedValue = Globals.GetXmlNodeValue(settingsXml2, "check_name").ToNullString();
				string text4 = Globals.GetXmlNodeValue(settingsXml2, "certPath").ToNullString();
				if (!string.IsNullOrEmpty(text4.Trim()))
				{
					string path = base.Server.MapPath(text4);
					if (File.Exists(path))
					{
						this.divWeixinCertPathHtml.InnerHtml = string.Format("<a href=\"{0}\" target=\"_blank\">{0}&nbsp;&nbsp;</a><a href=\"javascript:void(0)\" fileurl=\"{0}\" class=\"delCertFile\">删除重传</a>", text4);
						this.hidWeixinCertPath.Value = text4;
					}
				}
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			decimal num = default(decimal);
			if (string.IsNullOrEmpty(this.txtMinimumSingleShot.Text.Trim()))
			{
				this.ShowMsg("单次提现最小限额不可为空", false);
				return;
			}
			if (!decimal.TryParse(this.txtMinimumSingleShot.Text.Trim(), out num))
			{
				this.ShowMsg("单次提现最小限额，请填写正整数！", false);
				return;
			}
			if (num < decimal.One)
			{
				this.ShowMsg("单次提现最小限额，必须大于等于1！", false);
				return;
			}
			masterSettings.MinimumSingleShot = num;
			masterSettings.EnableBulkPaymentAliPay = this.ooEnableBulkPaymentAliPay.SelectedValue;
			masterSettings.EnableBulkPaymentWeixin = this.ooEnableBulkPaymentWeixin.SelectedValue;
			SettingsManager.Save(masterSettings);
			string text = "";
			string text2 = "";
			PaymentModeInfo paymentModeInfo = new PaymentModeInfo();
			string text3;
			string text4;
			string text5;
			string selectedValue;
			string obj;
			string empty;
			if (masterSettings.EnableBulkPaymentWeixin)
			{
				text3 = this.txtWeixinMchAppid.Text;
				text4 = this.txtWeixinMchid.Text;
				text5 = this.txtWeixinKey.Text;
				string text6 = (this.ViewState["WeixinKeyCover"] == null) ? "" : ((string)this.ViewState["WeixinKeyCover"]);
				string text7 = (this.ViewState["WeixinKey"] == null) ? "" : ((string)this.ViewState["WeixinKey"]);
				if (!string.IsNullOrEmpty(text6))
				{
					if (text5.Equals(text6))
					{
						text5 = text7;
					}
					else if (text5.Contains("*"))
					{
						this.ShowMsg("请清除原有的Key再输入正确的Key(您在商户中心自己设置的32位字符串)！", false);
						this.txtWeixinKey.Text = text6;
						return;
					}
				}
				else if (string.IsNullOrEmpty(text5))
				{
					this.ShowMsg("请输入Key(您在商户中心自己设置的32位字符串)！", false);
					return;
				}
				selectedValue = this.ddlWeixinCheckName.SelectedValue;
				empty = string.Empty;
				obj = text4;
				if (this.fileWeixinCertPath.HasFile)
				{
					if (Globals.ValidateCertFile(this.fileWeixinCertPath.PostedFile.FileName))
					{
						empty = "/config/" + this.fileWeixinCertPath.PostedFile.FileName;
						this.fileWeixinCertPath.PostedFile.SaveAs(base.Server.MapPath("~/config/") + this.fileWeixinCertPath.PostedFile.FileName);
						goto IL_02b6;
					}
					this.ShowMsg("非法的证书文件！", false);
					return;
				}
				empty = this.hidWeixinCertPath.Value;
				goto IL_02b6;
			}
			goto IL_03f2;
			IL_02b6:
			if (string.IsNullOrEmpty(empty))
			{
				this.ShowMsg("请上传证书！", false);
				return;
			}
			text = $"<xml><mch_appid>{text3.ToNullString()}</mch_appid><mchid>{text4.ToNullString()}</mchid><check_name>{selectedValue.ToNullString()}</check_name><certPath>{empty.ToNullString()}</certPath><certPassword>{obj.ToNullString()}</certPassword><Key>{text5.ToNullString()}</Key></xml>";
			text2 = "Hishop.Plugins.Outpay.Weixin.WeixinRequest";
			paymentModeInfo = SalesHelper.GetPaymentMode(text2);
			if (paymentModeInfo == null)
			{
				paymentModeInfo = new PaymentModeInfo();
				paymentModeInfo.Gateway = text2;
				paymentModeInfo.Description = "微信批量放款";
				paymentModeInfo.ApplicationType = PayApplicationType.payOnAll;
				paymentModeInfo.Name = "微信批量放款";
				paymentModeInfo.ModeType = PayModeType.Outpay;
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				paymentModeInfo.IsUseInpour = false;
				SalesHelper.CreatePaymentMode(paymentModeInfo);
			}
			else
			{
				paymentModeInfo.Gateway = text2;
				paymentModeInfo.Description = "微信批量放款";
				paymentModeInfo.ApplicationType = PayApplicationType.payOnAll;
				paymentModeInfo.Name = "微信批量放款";
				paymentModeInfo.ModeType = PayModeType.Outpay;
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				paymentModeInfo.IsUseInpour = false;
				SalesHelper.UpdatePaymentMode(paymentModeInfo);
			}
			goto IL_03f2;
			IL_03f2:
			if (masterSettings.EnableBulkPaymentAliPay)
			{
				string text8 = this.txtAlipayAccountName.Text;
				string text9 = this.txtAlipayEmail.Text;
				string text10 = this.txtAlipayKey.Text;
				string text11 = this.txtAlipayPartner.Text;
				text = $"<xml><partner>{text11.ToNullString()}</partner><key>{text10.ToNullString()}</key><email>{text9.ToNullString()}</email><account_name>{text8.ToNullString()}</account_name></xml>";
				text2 = "Hishop.Plugins.Outpay.Alipay.AlipayRequest";
				paymentModeInfo = SalesHelper.GetPaymentMode(text2);
				if (paymentModeInfo == null)
				{
					paymentModeInfo = new PaymentModeInfo();
					paymentModeInfo.Gateway = text2;
					paymentModeInfo.Description = "支付宝批量放款";
					paymentModeInfo.ApplicationType = PayApplicationType.payOnAll;
					paymentModeInfo.Name = "支付宝批量放款";
					paymentModeInfo.ModeType = PayModeType.Outpay;
					paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
					paymentModeInfo.IsUseInpour = false;
					SalesHelper.CreatePaymentMode(paymentModeInfo);
				}
				else
				{
					paymentModeInfo.Gateway = text2;
					paymentModeInfo.Description = "支付宝批量放款";
					paymentModeInfo.ApplicationType = PayApplicationType.payOnAll;
					paymentModeInfo.Name = "支付宝批量放款";
					paymentModeInfo.ModeType = PayModeType.Outpay;
					paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
					paymentModeInfo.IsUseInpour = false;
					SalesHelper.UpdatePaymentMode(paymentModeInfo);
				}
			}
			this.ShowMsg("修改成功！", true);
			this.BeLoad();
		}
	}
}
