using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Applet
{
	[PrivilegeCheck(Privilege.AppletPayConfig)]
	public class XCXPaySetting : AdminPage
	{
		protected OnOff ooEnableHtmRewrite;

		protected AccountNumbersTextBox txtAppId;

		protected AccountNumbersTextBox txtAppSecret;

		protected AccountNumbersTextBox txtPartnerID;

		protected AccountNumbersTextBox txtPartnerKey;

		protected FileUpload file_UploadCert;

		protected HtmlGenericControl liCertFileName;

		protected TrimTextBox txtCerFileName;

		protected AccountNumbersTextBox txtCertPassword;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.ooEnableHtmRewrite.Parameter.Add("onSwitchChange", "fuCheckEnableWXPay");
			this.btnOK.Click += this.btnOK_Click;
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.txtAppId.Text = masterSettings.WxAppletAppId;
				string wxApplectKey = masterSettings.WxApplectKey;
				if (wxApplectKey.Length > 6)
				{
					this.txtPartnerKey.Text = wxApplectKey.Substring(0, 3) + Globals.GetHiddenStr(wxApplectKey.Length - 6, "*") + wxApplectKey.Substring(wxApplectKey.Length - 3);
				}
				this.txtPartnerID.Text = masterSettings.WxApplectMchId;
				this.txtAppSecret.Text = masterSettings.WxAppletAppSecrect;
				this.ooEnableHtmRewrite.SelectedValue = masterSettings.OpenWxAppletWxPay;
				if (string.IsNullOrEmpty(masterSettings.WxApplectPayCert))
				{
					this.liCertFileName.Visible = false;
				}
				else
				{
					this.liCertFileName.Visible = true;
					this.txtCerFileName.Text = masterSettings.WxApplectPayCert;
				}
				this.txtCertPassword.Text = masterSettings.WxApplectPayCertPassword;
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsDemoSite)
			{
				this.ShowMsg("演示站不允许修改配置信息", false);
			}
			else
			{
				if (this.ooEnableHtmRewrite.SelectedValue)
				{
					if (string.IsNullOrEmpty(this.txtAppId.Text))
					{
						this.ShowMsg("AppId不能为空！", false);
						return;
					}
					if (string.IsNullOrEmpty(this.txtAppSecret.Text))
					{
						this.ShowMsg("AppSecret不能为空！", false);
						return;
					}
					if (string.IsNullOrEmpty(this.txtPartnerID.Text))
					{
						this.ShowMsg("mch_id不能为空！", false);
						return;
					}
					if (string.IsNullOrEmpty(this.txtPartnerKey.Text))
					{
						this.ShowMsg("Key不能为空！", false);
						return;
					}
				}
				string wxApplectPayCert = this.txtCerFileName.Text;
				if (this.file_UploadCert.HasFile)
				{
					if (!Globals.ValidateCertFile(this.file_UploadCert.PostedFile.FileName))
					{
						this.ShowMsg("非法的证书文件", false);
						return;
					}
					string newFileName = this.GetNewFileName(this.file_UploadCert.PostedFile.FileName);
					this.file_UploadCert.PostedFile.SaveAs(base.Server.MapPath("~/pay/cert/") + newFileName);
					wxApplectPayCert = "/pay/cert/" + newFileName;
				}
				string wxAppletAppId = masterSettings.WxAppletAppId;
				masterSettings.WxAppletAppId = this.txtAppId.Text;
				masterSettings.WxAppletAppSecrect = this.txtAppSecret.Text;
				masterSettings.WxApplectMchId = this.txtPartnerID.Text;
				if (this.txtPartnerKey.Text.Replace("*", "").Length != 6)
				{
					masterSettings.WxApplectKey = this.txtPartnerKey.Text;
				}
				masterSettings.OpenWxAppletWxPay = this.ooEnableHtmRewrite.SelectedValue;
				masterSettings.WxApplectPayCert = wxApplectPayCert;
				masterSettings.WxApplectPayCertPassword = masterSettings.WxApplectMchId;
				SettingsManager.Save(masterSettings);
				this.ShowMsg("设置成功", true, "XCXPaySetting");
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
