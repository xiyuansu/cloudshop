using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SiteSettings)]
	public class SiteContent : AdminPage
	{
		protected TextBox txtSiteName;

		protected HtmlGenericControl txtSiteNameTip;

		protected HiddenField hidOldLogo;

		protected HiddenField hidUploadLogo;

		protected TextBox txtServicePhone;

		protected TextBox txtDomainName;

		protected HtmlGenericControl txtDomainNameTip;

		protected Ueditor fkFooter;

		protected Ueditor fckRegisterAgreement;

		protected Literal litKeycode;

		protected Literal litappsecret;

		protected Literal litwdgjapi;

		protected Literal litwdgjapi_new;

		protected Literal litappsecret2;

		protected TextBox txtPolyapiAppId;

		protected HtmlGenericControl txtPolyapiAppIdTip;

		protected TextBox txtPolyapiKey;

		protected HtmlGenericControl txtPolyapiKeyTip;

		protected DecimalLengthDropDownList dropBitNumber;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtSiteDescription;

		protected HtmlGenericControl txtSiteDescriptionTip;

		protected TextBox txtSearchMetaDescription;

		protected HtmlGenericControl txtSearchMetaDescriptionTip;

		protected TextBox txtSearchMetaKeywords;

		protected HtmlGenericControl txtSearchMetaKeywordsTip;

		protected TextBox txtQQMapAppKey;

		protected HtmlGenericControl txtQQMapAppKeyTip;

		protected HiddenField txtAutoRedirectClient;

		protected Button btnOK;

		protected Button btnClear;

		protected ImageList ImageList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnOK.Click += this.btnOK_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.LoadSiteContent(masterSettings);
			}
		}

		private void LoadSiteContent(SiteSettings siteSettings)
		{
			this.txtSiteName.Text = siteSettings.SiteName;
			this.txtServicePhone.Text = siteSettings.ServicePhone;
			this.txtDomainName.Text = siteSettings.SiteUrl;
			this.hidOldLogo.Value = siteSettings.LogoUrl;
			this.fkFooter.Text = siteSettings.Footer;
			this.fckRegisterAgreement.Text = siteSettings.RegisterAgreement;
			this.litKeycode.Text = siteSettings.AppKey;
			this.litappsecret.Text = siteSettings.CheckCode;
			this.litappsecret2.Text = siteSettings.CheckCode;
			this.litwdgjapi.Text = siteSettings.SiteUrl + "/wdgj_api/API.ashx";
			this.litwdgjapi_new.Text = siteSettings.SiteUrl + "/wdgj_api/API_new.ashx";
			this.dropBitNumber.SelectedValue = siteSettings.DecimalLength;
			this.hidOldImages.Value = siteSettings.DefaultProductImage;
			this.txtSiteDescription.Text = siteSettings.SiteDescription;
			this.txtSearchMetaDescription.Text = siteSettings.SearchMetaDescription;
			this.txtSearchMetaKeywords.Text = siteSettings.SearchMetaKeywords;
			this.txtAutoRedirectClient.Value = siteSettings.AutoRedirectClient.ToNullString();
			this.txtQQMapAppKey.Text = siteSettings.QQMapAPIKey.ToNullString();
			this.txtPolyapiAppId.Text = siteSettings.PolyapiAppId.ToNullString();
			this.txtPolyapiKey.Text = siteSettings.PolyapiKey.ToNullString();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.SiteName = this.txtSiteName.Text.Trim();
			masterSettings.ServicePhone = this.txtServicePhone.Text.Trim();
			masterSettings.SiteUrl = this.txtDomainName.Text.Trim();
			masterSettings.Footer = this.fkFooter.Text;
			masterSettings.RegisterAgreement = this.fckRegisterAgreement.Text;
			ProductImagesInfo productImagesInfo = this.SaveDefaultImage();
			if (productImagesInfo != null)
			{
				masterSettings.DefaultProductImage = productImagesInfo.ImageUrl1;
				this.hidOldImages.Value = productImagesInfo.ImageUrl1;
				masterSettings.DefaultProductThumbnail1 = productImagesInfo.ThumbnailUrl40;
				masterSettings.DefaultProductThumbnail2 = productImagesInfo.ThumbnailUrl60;
				masterSettings.DefaultProductThumbnail3 = productImagesInfo.ThumbnailUrl100;
				masterSettings.DefaultProductThumbnail4 = productImagesInfo.ThumbnailUrl160;
				masterSettings.DefaultProductThumbnail5 = productImagesInfo.ThumbnailUrl180;
				masterSettings.DefaultProductThumbnail6 = productImagesInfo.ThumbnailUrl220;
				masterSettings.DefaultProductThumbnail7 = productImagesInfo.ThumbnailUrl310;
				masterSettings.DefaultProductThumbnail8 = productImagesInfo.ThumbnailUrl410;
			}
			else if (this.hidUploadImages.Value.Trim().Length == 0)
			{
				masterSettings.DefaultProductImage = string.Empty;
				this.hidOldImages.Value = string.Empty;
				masterSettings.DefaultProductThumbnail1 = string.Empty;
				masterSettings.DefaultProductThumbnail2 = string.Empty;
				masterSettings.DefaultProductThumbnail3 = string.Empty;
				masterSettings.DefaultProductThumbnail4 = string.Empty;
				masterSettings.DefaultProductThumbnail5 = string.Empty;
				masterSettings.DefaultProductThumbnail6 = string.Empty;
				masterSettings.DefaultProductThumbnail7 = string.Empty;
				masterSettings.DefaultProductThumbnail8 = string.Empty;
			}
			string text = this.UploadSiteLogo();
			this.hidOldLogo.Value = text;
			masterSettings.LogoUrl = text;
			masterSettings.DecimalLength = this.dropBitNumber.SelectedValue;
			masterSettings.SiteDescription = Globals.StripAllTags(this.txtSiteDescription.Text.Trim());
			masterSettings.SearchMetaDescription = Globals.StripAllTags(this.txtSearchMetaDescription.Text.Trim());
			masterSettings.SearchMetaKeywords = Globals.StripAllTags(this.txtSearchMetaKeywords.Text.Trim());
			masterSettings.AutoRedirectClient = this.txtAutoRedirectClient.Value.ToBool();
			masterSettings.PolyapiAppId = Globals.StripAllTags(this.txtPolyapiAppId.Text);
			masterSettings.PolyapiKey = Globals.StripAllTags(this.txtPolyapiKey.Text);
			if (!string.IsNullOrEmpty(this.txtQQMapAppKey.Text))
			{
				masterSettings.QQMapAPIKey = Globals.StripAllTags(this.txtQQMapAppKey.Text);
			}
			if (this.ValidationSettings(masterSettings))
			{
				Globals.EntityCoding(masterSettings, true);
				SettingsManager.Save(masterSettings);
				HiCache.Clear();
				this.ShowMsg("成功修改了商城基本信息", true);
				masterSettings = SettingsManager.GetMasterSettings();
				this.LoadSiteContent(masterSettings);
			}
		}

		private string UploadSiteLogo()
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath("/utility/pics/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string value = this.hidUploadLogo.Value;
			if (value.Trim().Length == 0)
			{
				return string.Empty;
			}
			value = value.Replace("//", "/");
			string str2 = (value.Split('/').Length == 5) ? value.Split('/')[4] : value.Split('/')[3];
			if (File.Exists(text + str2))
			{
				return "/utility/pics/" + str2;
			}
			if (File.Exists(HttpContext.Current.Server.MapPath(this.hidUploadLogo.Value)))
			{
				File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadLogo.Value), text + str2);
				string path = HttpContext.Current.Server.MapPath(str + str2);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			return "/utility/pics/" + str2;
		}

		private ProductImagesInfo SaveDefaultImage()
		{
			string text = Globals.GetStoragePath() + "/product/";
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			string str = Globals.GetStoragePath() + "/temp/";
			string str2 = HttpContext.Current.Server.MapPath(text + "/images/");
			string value = this.hidUploadImages.Value;
			if (!value.Contains("/temp/"))
			{
				return null;
			}
			if (value.Trim().Length == 0)
			{
				return null;
			}
			value = value.Replace("//", "/");
			string text2 = (value.Split('/').Length == 6) ? value.Split('/')[5] : value.Split('/')[4];
			if (File.Exists(str2 + text2))
			{
				return null;
			}
			ProductImagesInfo productImagesInfo = new ProductImagesInfo();
			if (File.Exists(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value)))
			{
				File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadImages.Value), str2 + text2);
			}
			string text3 = text + "thumbs40/40_" + text2;
			string text4 = text + "thumbs60/60_" + text2;
			string text5 = text + "thumbs100/100_" + text2;
			string text6 = text + "thumbs160/160_" + text2;
			string text7 = text + "thumbs180/180_" + text2;
			string text8 = text + "thumbs220/220_" + text2;
			string text9 = text + "thumbs310/310_" + text2;
			string text10 = text + "thumbs410/410_" + text2;
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text3), 40, 40);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text4), 60, 60);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text5), 100, 100);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text6), 160, 160);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text7), 180, 180);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text8), 220, 220);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text9), 310, 310);
			ResourcesHelper.CreateThumbnail(str2 + text2, HttpContext.Current.Server.MapPath(text10), 410, 410);
			productImagesInfo.ImageUrl1 = text + "images/" + text2;
			productImagesInfo.ThumbnailUrl40 = text3;
			productImagesInfo.ThumbnailUrl60 = text4;
			productImagesInfo.ThumbnailUrl100 = text5;
			productImagesInfo.ThumbnailUrl160 = text6;
			productImagesInfo.ThumbnailUrl180 = text7;
			productImagesInfo.ThumbnailUrl220 = text8;
			productImagesInfo.ThumbnailUrl310 = text9;
			productImagesInfo.ThumbnailUrl410 = text10;
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return productImagesInfo;
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

		private bool ValidationSettings(SiteSettings setting)
		{
			ValidationResults validationResults = Validation.Validate(setting, "ValMasterSettings");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}

		protected void btnClear_Click(object sender, EventArgs e)
		{
			HiCache.Clear();
			this.ShowMsg("清空缓存成功", true);
		}
	}
}
