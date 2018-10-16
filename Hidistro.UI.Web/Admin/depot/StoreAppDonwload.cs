using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Ionic.Zip;
using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.depot
{
	[PrivilegeCheck(Privilege.AppAndroidUpgrade)]
	public class StoreAppDonwload : AdminPage
	{
		public string androidDownloadUrl = "";

		protected Literal litVersion;

		protected HtmlInputHidden hidIsForcibleUpgrade;

		protected Literal litDescription;

		protected FileUpload fileUpload;

		protected HtmlGenericControl txtAndroidDownloadUrl;

		protected TextBox txtIosDownloadUrl;

		protected Button btnUpoad;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpoad.Click += this.btnUpoad_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.txtAndroidDownloadUrl.InnerText = masterSettings.StoreAppAndroidUrl.ToNullString();
				this.litVersion.Text = masterSettings.StoreAppVersion.ToNullString();
				this.litDescription.Text = masterSettings.StoreAppDescription.ToNullString();
				this.txtIosDownloadUrl.Text = masterSettings.StoreAppIOSUrl.ToNullString();
			}
		}

		private void ClearDirectory(string srcPath)
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(srcPath);
				FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
				FileSystemInfo[] array = fileSystemInfos;
				foreach (FileSystemInfo fileSystemInfo in array)
				{
					if (fileSystemInfo is DirectoryInfo)
					{
						DirectoryInfo directoryInfo2 = new DirectoryInfo(fileSystemInfo.FullName);
						directoryInfo2.Delete(true);
					}
					else
					{
						File.Delete(fileSystemInfo.FullName);
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "ClearDirectory");
			}
		}

		private void btnUpoad_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (this.fileUpload.FileContent.Length > 0 && this.fileUpload.PostedFile.ContentType != "application/x-zip-compressed" && this.fileUpload.PostedFile.ContentType != "application/zip" && this.fileUpload.PostedFile.ContentType != "application/octet-stream")
			{
				this.ShowMsg("请上传正确的数据包文件", false);
			}
			else
			{
				string text = masterSettings.StoreAppAndroidUrl;
				if (this.fileUpload.FileContent.Length > 0)
				{
					string text2 = this.Page.Request.MapPath("~/App_Data/data/storeapp/android");
					string str = this.Page.Request.MapPath("~/storage/data/storeapp/android");
					string fileName = Path.GetFileName(this.fileUpload.PostedFile.FileName);
					string text3 = Path.Combine(text2, fileName);
					this.ClearDirectory(text2);
					this.fileUpload.PostedFile.SaveAs(text3);
					DirectoryInfo directoryInfo = new DirectoryInfo(text2);
					using (ZipFile zipFile = ZipFile.Read(Path.Combine(directoryInfo.FullName, fileName)))
					{
						foreach (ZipEntry item in zipFile)
						{
							item.Extract(directoryInfo.FullName, ExtractExistingFileAction.OverwriteSilently);
						}
					}
					if (!File.Exists(text2 + "/AndroidUpgrade.xml"))
					{
						this.ShowMsg("压缩包中不包含版本信息说明的xml文件,请重新打包。", false);
						return;
					}
					this.LoadVersion();
					decimal num = this.litVersion.Text.ToDecimal(0);
					if (num < masterSettings.StoreAppVersion)
					{
						this.ShowMsg("当前版本号小于之前的版本号，更新之后将无法升级，请更改版本号后重新上传！", false);
						return;
					}
					masterSettings.StoreAppVersion = num;
					masterSettings.StoreAppDescription = Globals.StripAllTags(this.litDescription.Text);
					File.Copy(text2 + "/AndroidUpgrade.xml", str + "/AndroidUpgrade.xml", true);
					File.Delete(text2 + "/AndroidUpgrade.xml");
					string[] files = Directory.GetFiles(text2, "*.apk", SearchOption.TopDirectoryOnly);
					if (files.Length != 0)
					{
						string[] array = files;
						foreach (string text4 in array)
						{
							FileInfo fileInfo = new FileInfo(text4);
							File.Copy(text4, str + "/" + fileInfo.Name, true);
							text = "/storage/data/storeapp/android/" + fileInfo.Name;
							File.Delete(text2 + "/" + fileInfo.Name);
						}
					}
					File.Delete(text3);
					this.androidDownloadUrl = HiContext.Current.SiteUrl + (HiContext.Current.SiteUrl.EndsWith("/") ? "" : "/") + text;
				}
				string text6 = masterSettings.StoreAppIOSUrl = Globals.StripAllTags(this.txtIosDownloadUrl.Text);
				masterSettings.StoreAppAndroidUrl = text;
				SettingsManager.Save(masterSettings);
				this.ShowMsg("保存成功！", true);
			}
		}

		private void LoadVersion()
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(HttpContext.Current.Request.MapPath("/App_Data/data/storeapp/android/AndroidUpgrade.xml"));
				this.litVersion.Text = xmlDocument.SelectSingleNode("root/Version").InnerText;
				this.litDescription.Text = xmlDocument.SelectSingleNode("root/Description").InnerText;
				this.txtAndroidDownloadUrl.InnerText = this.androidDownloadUrl;
			}
			catch (Exception ex)
			{
				this.ShowMsg("加载版本信息错误,请确定版本文件正确,Error:" + ex.Message, false);
				Globals.WriteExceptionLog(ex, null, "StoreAppLoadVersion");
			}
		}
	}
}
