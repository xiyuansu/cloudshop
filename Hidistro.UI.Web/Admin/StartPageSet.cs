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
	[PrivilegeCheck(Privilege.AppStartPageSet)]
	public class StartPageSet : AdminPage
	{
		protected HtmlGenericControl liParent;

		protected Button btnAdd;

		protected HiddenField hfAndroidImage;

		protected HiddenField hfIOSImage;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnAdd_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.hfAndroidImage.Value = masterSettings.AndroidStartImg;
				this.hfIOSImage.Value = masterSettings.IOSStartImg;
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			bool flag = false;
			string value = this.hfIOSImage.Value;
			string value2 = this.hfAndroidImage.Value;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(value) && value.IndexOf("/temp/") >= 0)
			{
				masterSettings.IOSStartImg = this.UploadImage(value);
				flag = true;
			}
			else if (string.IsNullOrEmpty(value))
			{
				string path = Globals.PhysicalPath(masterSettings.IOSStartImg);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				masterSettings.IOSStartImg = string.Empty;
			}
			if (!string.IsNullOrEmpty(value2) && value2.IndexOf("/temp/") >= 0)
			{
				masterSettings.AndroidStartImg = this.UploadImage(value2);
				flag = true;
			}
			else if (string.IsNullOrEmpty(value2))
			{
				string path2 = Globals.PhysicalPath(masterSettings.AndroidStartImg);
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
				masterSettings.AndroidStartImg = string.Empty;
			}
			SettingsManager.Save(masterSettings);
			this.ShowMsg("上传成功。", true, "StartPageSet.aspx");
		}

		private string UploadImage(string imageUrl)
		{
			string text = (imageUrl.Split('/').Length == 5) ? imageUrl.Split('/')[4] : imageUrl.Split('/')[3];
			string text2 = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/temp/" + text);
			string destFileName = HttpContext.Current.Server.MapPath("/Utility/pics/App_" + text);
			File.Copy(text2, destFileName, true);
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
			return "/Utility/pics/App_" + text;
		}
	}
}
