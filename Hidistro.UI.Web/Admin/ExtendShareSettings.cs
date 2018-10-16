using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ReferralSettings)]
	public class ExtendShareSettings : AdminPage
	{
		protected TextBox txtExtendShareTitle;

		protected HtmlGenericControl txtExtendShareTitleTip;

		protected HiddenField hidUploadLogo;

		protected TextBox txtExtendShareDetail;

		protected HtmlGenericControl txtExtendShareDetailTip;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnOK.Click += this.btnOK_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.hidUploadLogo.Value = masterSettings.ExtendSharePic;
				this.txtExtendShareTitle.Text = masterSettings.ExtendShareTitle;
				this.txtExtendShareDetail.Text = masterSettings.ExtendShareDetail;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.ExtendShareTitle = this.txtExtendShareTitle.Text.Trim();
			masterSettings.ExtendShareDetail = this.txtExtendShareDetail.Text.Trim();
			string text = this.UploadSiteLogo();
			this.hidUploadLogo.Value = text;
			masterSettings.ExtendSharePic = text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true, "");
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
			File.Copy(HttpContext.Current.Server.MapPath(this.hidUploadLogo.Value), text + str2);
			string path = HttpContext.Current.Server.MapPath(str + str2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return "/utility/pics/" + str2;
		}
	}
}
