using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WAPShop
{
	public class VServerConfig : AdminPage
	{
		protected Literal txtUrl;

		protected Literal txtToken;

		protected TextBox txtAppId;

		protected TextBox txtAppSecret;

		protected HtmlInputCheckBox chkIsValidationService;

		protected FileUpload fileUpload;

		protected Button btnUpoad;

		protected HiImage imgPic;

		protected ImageLinkButton btnPicDelete;

		protected TextBox txtWeixinNumber;

		protected TextBox txtWeixinLoginUrl;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpoad.Click += this.btnUpoad_Click;
			this.btnPicDelete.Click += this.btnPicDelete_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (string.IsNullOrEmpty(masterSettings.WeixinToken))
				{
					masterSettings.WeixinToken = this.CreateKey(8);
					SettingsManager.Save(masterSettings);
				}
				this.txtAppId.Text = masterSettings.WeixinAppId;
				this.txtAppSecret.Text = masterSettings.WeixinAppSecret;
				this.txtToken.Text = masterSettings.WeixinToken;
				this.chkIsValidationService.Checked = masterSettings.IsValidationService;
				this.imgPic.ImageUrl = masterSettings.WeiXinCodeImageUrl;
				this.txtWeixinNumber.Text = masterSettings.WeixinNumber;
				this.txtWeixinLoginUrl.Text = masterSettings.WeixinLoginUrl;
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl);
				this.txtUrl.Text = string.Format("http://{0}/api/wx.ashx", base.Request.Url.Host, this.txtToken.Text);
			}
		}

		private void btnPicDelete_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
			{
				ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
				this.btnPicDelete.Visible = false;
				SiteSettings siteSettings = masterSettings;
				HiImage hiImage = this.imgPic;
				string text2 = siteSettings.WeiXinCodeImageUrl = (hiImage.ImageUrl = string.Empty);
				SettingsManager.Save(masterSettings);
			}
		}

		private void btnUpoad_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.fileUpload.HasFile)
			{
				try
				{
					if (!string.IsNullOrEmpty(masterSettings.WeiXinCodeImageUrl))
					{
						ResourcesHelper.DeleteImage(masterSettings.WeiXinCodeImageUrl);
					}
					HiImage hiImage = this.imgPic;
					SiteSettings siteSettings = masterSettings;
					string text3 = hiImage.ImageUrl = (siteSettings.WeiXinCodeImageUrl = VShopHelper.UploadWeiXinCodeImage(this.fileUpload.PostedFile));
					this.btnPicDelete.Visible = true;
					SettingsManager.Save(masterSettings);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
				}
			}
		}

		private string CreateKey(int len)
		{
			byte[] array = new byte[len];
			new RNGCryptoServiceProvider().GetBytes(array);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append($"{array[i]:X2}");
			}
			return stringBuilder.ToString();
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.WeixinAppId = this.txtAppId.Text;
			masterSettings.WeixinAppSecret = this.txtAppSecret.Text;
			masterSettings.IsValidationService = this.chkIsValidationService.Checked;
			masterSettings.WeixinNumber = this.txtWeixinNumber.Text;
			masterSettings.WeixinLoginUrl = this.txtWeixinLoginUrl.Text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true, "");
		}
	}
}
