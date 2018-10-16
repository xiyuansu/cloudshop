using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppWeixinPay)]
	public class WeiXinPay : AdminPage
	{
		protected OnOff radEnableHtmRewrite;

		protected AccountNumbersTextBox txtAppId;

		protected AccountNumbersTextBox txtAppSecret;

		protected AccountNumbersTextBox txtPartnerID;

		protected AccountNumbersTextBox txtPartnerKey;

		protected OnOff OnOffServiceMode;

		protected AccountNumbersTextBox txtMainMchID;

		protected AccountNumbersTextBox txtMainAppID;

		protected FileUpload file_UploadCert;

		protected HtmlGenericControl liCertFileName;

		protected TrimTextBox txtCerFileName;

		protected AccountNumbersTextBox txtCertPassword;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.btnOK.Click += this.btnOK_Click;
			this.radEnableHtmRewrite.Parameter.Add("onSwitchChange", "fuCheckEnableWXPay");
			this.OnOffServiceMode.Parameter.Add("onSwitchChange", "funCheckOnOffServiceMode");
			if (!base.IsPostBack)
			{
				this.txtAppId.Text = masterSettings.AppWxAppId;
				string appWxPartnerKey = masterSettings.AppWxPartnerKey;
				if (appWxPartnerKey.Length > 6)
				{
					this.txtPartnerKey.Text = appWxPartnerKey.Substring(0, 3) + Globals.GetHiddenStr(appWxPartnerKey.Length - 6, "*") + appWxPartnerKey.Substring(appWxPartnerKey.Length - 3);
				}
				this.txtMainMchID.Text = masterSettings.AppWX_Main_MchID;
				this.txtMainAppID.Text = masterSettings.AppWX_Main_AppId;
				this.txtPartnerID.Text = masterSettings.AppWxMchId;
				this.txtAppSecret.Text = masterSettings.AppWxAppSecret;
				if (string.IsNullOrEmpty(masterSettings.AppWX_Main_MchID))
				{
					this.OnOffServiceMode.SelectedValue = false;
				}
				else
				{
					this.OnOffServiceMode.SelectedValue = true;
				}
				if (string.IsNullOrEmpty(masterSettings.AppWxCertPath))
				{
					this.liCertFileName.Visible = false;
				}
				else
				{
					this.liCertFileName.Visible = true;
					this.txtCerFileName.Text = masterSettings.AppWxCertPath;
				}
				this.txtCertPassword.Text = masterSettings.AppWxCertPass;
				this.radEnableHtmRewrite.SelectedValue = masterSettings.OpenAppWxPay;
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
				if (this.radEnableHtmRewrite.SelectedValue)
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
				string appWxCertPath = this.txtCerFileName.Text;
				if (this.file_UploadCert.HasFile)
				{
					if (!Globals.ValidateCertFile(this.file_UploadCert.PostedFile.FileName))
					{
						this.ShowMsg("非法的证书文件", false);
						return;
					}
					string newFileName = this.GetNewFileName(this.file_UploadCert.PostedFile.FileName);
					this.file_UploadCert.PostedFile.SaveAs(base.Server.MapPath("~/pay/cert/") + newFileName);
					appWxCertPath = "/pay/cert/" + newFileName;
				}
				string appWxAppId = masterSettings.AppWxAppId;
				masterSettings.AppWxAppId = this.txtAppId.Text;
				masterSettings.AppWxAppSecret = this.txtAppSecret.Text;
				masterSettings.AppWxMchId = this.txtPartnerID.Text;
				if (this.txtPartnerKey.Text.Replace("*", "").Length != 6)
				{
					masterSettings.AppWxPartnerKey = this.txtPartnerKey.Text;
				}
				masterSettings.OpenAppWxPay = this.radEnableHtmRewrite.SelectedValue;
				masterSettings.AppWxCertPath = appWxCertPath;
				if (this.OnOffServiceMode.SelectedValue)
				{
					string text = Globals.StripAllTags(this.txtMainAppID.Text);
					string text2 = Globals.StripAllTags(this.txtMainMchID.Text);
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						this.ShowMsg("开启了服务商模式,请输入服务商商户ID和服务商AppID", false);
						return;
					}
					masterSettings.AppWX_Main_AppId = text;
					masterSettings.AppWX_Main_MchID = text2;
				}
				else
				{
					masterSettings.AppWX_Main_AppId = "";
					masterSettings.AppWX_Main_MchID = "";
				}
				if (string.IsNullOrEmpty(masterSettings.AppWX_Main_MchID))
				{
					masterSettings.AppWxCertPass = this.txtPartnerID.Text;
				}
				else
				{
					masterSettings.AppWxCertPass = masterSettings.AppWX_Main_MchID;
				}
				SettingsManager.Save(masterSettings);
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
