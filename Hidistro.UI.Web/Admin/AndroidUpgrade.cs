using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
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

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.StoreAppDownLoad)]
	public class AndroidUpgrade : AdminPage
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		protected Literal litVersion;

		protected HtmlInputHidden hidIsForcibleUpgrade;

		protected Literal litDescription;

		protected FileUpload fileUpload;

		protected HtmlGenericControl txtAndroidDownloadUrl;

		protected TextBox txtIosDownloadUrl;

		protected TextBox txtAppAuditAPIUrl;

		protected OnOff ooOpen;

		protected Button btnUpoad;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpoad.Click += this.btnUpoad_Click;
			if (!this.Page.IsPostBack)
			{
				this.txtIosDownloadUrl.Text = this.siteSettings.AppIOSDownLoadUrl;
				this.txtAppAuditAPIUrl.Text = this.siteSettings.AppAuditAPIUrl.ToNullString();
				this.LoadVersion();
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
				string text2 = masterSettings.AppAndroidDownLoadUrl = Globals.StripAllTags(this.txtAndroidDownloadUrl.InnerText);
				if (this.fileUpload.FileContent.Length > 0)
				{
					string text3 = this.Page.Request.MapPath("~/App_Data/data/app/android");
					string str = this.Page.Request.MapPath("~/storage/data/app/android");
					string fileName = Path.GetFileName(this.fileUpload.PostedFile.FileName);
					string text4 = Path.Combine(text3, fileName);
					this.ClearDirectory(text3);
					this.fileUpload.PostedFile.SaveAs(text4);
					DirectoryInfo directoryInfo = new DirectoryInfo(text3);
					using (ZipFile zipFile = ZipFile.Read(Path.Combine(directoryInfo.FullName, fileName)))
					{
						foreach (ZipEntry item in zipFile)
						{
							item.Extract(directoryInfo.FullName, ExtractExistingFileAction.OverwriteSilently);
						}
					}
					if (!File.Exists(text3 + "/AndroidUpgrade.xml"))
					{
						this.ShowMsg("压缩包中不包含版本信息的xml文件,请重新打包。", false);
						return;
					}
					File.Copy(text3 + "/AndroidUpgrade.xml", str + "/AndroidUpgrade.xml", true);
					File.Delete(text3 + "/AndroidUpgrade.xml");
					string[] files = Directory.GetFiles(text3, "*.apk");
					if (files.Length != 0)
					{
						string[] array = files;
						foreach (string text5 in array)
						{
							FileInfo fileInfo = new FileInfo(text5);
							File.Copy(text5, str + "/" + fileInfo.Name, true);
						}
					}
					File.Delete(text4);
					this.LoadVersion();
					AppVersionRecordInfo appVersionRecordInfo = APPHelper.GetLatestAppVersionRecord("android");
					if (appVersionRecordInfo == null)
					{
						appVersionRecordInfo = new AppVersionRecordInfo();
						appVersionRecordInfo.Device = "android";
						appVersionRecordInfo.Version = "0.00";
					}
					string text6 = this.litVersion.Text;
					if (APPHelper.IsExistNewVersion(text6, appVersionRecordInfo.Version))
					{
						bool isForcibleUpgrade = false;
						bool.TryParse(this.hidIsForcibleUpgrade.Value, out isForcibleUpgrade);
						appVersionRecordInfo.Version = text6;
						appVersionRecordInfo.IsForcibleUpgrade = isForcibleUpgrade;
						appVersionRecordInfo.Description = this.litDescription.Text;
						appVersionRecordInfo.UpgradeUrl = this.txtAndroidDownloadUrl.InnerText;
						APPHelper.AddAppVersionRecord(appVersionRecordInfo);
					}
				}
				masterSettings.EnableAppDownload = this.ooOpen.SelectedValue;
				masterSettings.AppAuditAPIUrl = this.txtAppAuditAPIUrl.Text.ToNullString();
				string text8 = masterSettings.AppIOSDownLoadUrl = Globals.StripAllTags(this.txtIosDownloadUrl.Text);
				SettingsManager.Save(masterSettings);
				this.ShowMsg("保存成功！", true);
			}
		}

		private void LoadVersion()
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				this.ooOpen.SelectedValue = this.siteSettings.EnableAppDownload;
				xmlDocument.Load(HttpContext.Current.Request.MapPath("/Storage/data/app/android/AndroidUpgrade.xml"));
				this.litVersion.Text = xmlDocument.SelectSingleNode("root/Version").InnerText;
				this.litDescription.Text = xmlDocument.SelectSingleNode("root/Description").InnerText;
				this.txtAndroidDownloadUrl.InnerText = xmlDocument.SelectSingleNode("root/UpgradeUrl").InnerText;
				this.hidIsForcibleUpgrade.Value = xmlDocument.SelectSingleNode("root/IsForcibleUpgrade").InnerText;
			}
			catch
			{
			}
		}
	}
}
