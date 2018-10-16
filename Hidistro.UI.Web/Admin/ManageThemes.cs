using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Themes)]
	public class ManageThemes : AdminPage
	{
		protected Literal litThemeName;

		protected Image imgThemeImgUrl;

		protected Image Image1;

		protected Literal lblThemeCount;

		protected Repeater dtManageThemes;

		protected FileUpload fileTemplate;

		protected Button btnUpload2;

		protected HtmlInputHidden hdtempname;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!HiContext.Current.SiteSettings.OpenPcShop)
			{
				base.Response.Redirect("WapThemeSettings");
			}
			this.litThemeName.Text = HiContext.Current.SiteSettings.Theme;
			this.dtManageThemes.ItemCommand += this.dtManageThemes_ItemCommand;
			this.btnUpload2.Click += this.btnUpload2_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				this.BindData(siteSettings);
			}
		}

		private void dtManageThemes_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DisplayThemesImages displayThemesImages = e.Item.FindControl("themeImg") as DisplayThemesImages;
				string themeName = displayThemesImages.ThemeName;
				string directoryName = this.Page.Request.MapPath("/Templates/master/") + themeName;
				string text = this.Page.Request.MapPath("/Templates/master/") + themeName;
				if (e.CommandName == "btnUse")
				{
					this.UserThems(themeName);
					this.ShowMsg("成功修改了商城模板", true);
				}
				if (e.CommandName == "download")
				{
					try
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(text);
						Encoding uTF = Encoding.UTF8;
						using (ZipFile zipFile = new ZipFile())
						{
							zipFile.CompressionLevel = CompressionLevel.Default;
							if (Directory.Exists(text))
							{
								zipFile.AddDirectory(text);
							}
							else
							{
								zipFile.AddDirectory(directoryName);
							}
							HttpResponse response = HttpContext.Current.Response;
							response.ContentType = "application/zip";
							response.ContentEncoding = uTF;
							response.AddHeader("Content-Disposition", "attachment;filename=" + themeName + ".zip");
							response.Clear();
							zipFile.Save(response.OutputStream);
							response.Flush();
							response.Close();
						}
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}
		}

		private void BindData(SiteSettings siteSettings)
		{
			IList<ManageThemeInfo> list = this.LoadThemes(siteSettings.Theme);
			this.dtManageThemes.DataSource = list;
			this.dtManageThemes.DataBind();
			this.lblThemeCount.Text = list.Count.ToString();
		}

		protected void UserThems(string name)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.Theme = name;
			SettingsManager.Save(masterSettings);
			HiCache.Remove("AdFileCache-Admin");
			HiCache.Remove("SubjectProductFileCache-Admin");
			HiCache.Remove("DataCache-Articles-{0}-{1}");
			HiCache.Remove("CommentFileCache-Admin");
			this.BindData(masterSettings);
		}

		private void CopyDir(string srcPath, string aimPath)
		{
			try
			{
				if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
				{
					aimPath += Path.DirectorySeparatorChar.ToString();
				}
				if (!Directory.Exists(aimPath))
				{
					Directory.CreateDirectory(aimPath);
				}
				string[] fileSystemEntries = Directory.GetFileSystemEntries(srcPath);
				string[] array = fileSystemEntries;
				foreach (string text in array)
				{
					if (Directory.Exists(text))
					{
						this.CopyDir(text, aimPath + Path.GetFileName(text));
					}
					else
					{
						File.Copy(text, aimPath + Path.GetFileName(text), true);
					}
				}
			}
			catch
			{
				this.ShowMsg("无法复制!", false);
			}
		}

		protected void btnUpload2_Click(object sender, EventArgs e)
		{
			string text = this.hdtempname.Value.Trim();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("无法获取对应模板名称,请重新上传", false);
			}
			else if (this.fileTemplate.PostedFile.ContentLength == 0 || (this.fileTemplate.PostedFile.ContentType != "application/x-zip-compressed" && this.fileTemplate.PostedFile.ContentType != "application/zip" && this.fileTemplate.PostedFile.ContentType != "application/octet-stream"))
			{
				this.ShowMsg("请上传正确的数据包文件", false);
			}
			else
			{
				string fileName = Path.GetFileName(this.fileTemplate.PostedFile.FileName);
				if (!fileName.Equals(text + ".zip"))
				{
					this.ShowMsg("上传的模板压缩名与原模板名不一致", false);
				}
				else
				{
					string text2 = this.Page.Request.MapPath("/Templates/master/");
					string text3 = Path.Combine(text2, fileName);
					this.fileTemplate.PostedFile.SaveAs(text3);
					this.PrepareDataFiles(text2, text3);
					File.Delete(text3);
					this.ShowMsg("上传成功！", true);
					this.UserThems(text);
					this.hdtempname.Value = "";
				}
			}
		}

		public string PrepareDataFiles(string _datapath, params object[] initParams)
		{
			string text = (string)initParams[0];
			DirectoryInfo directoryInfo = new DirectoryInfo(_datapath);
			DirectoryInfo directoryInfo2 = directoryInfo.CreateSubdirectory(Path.GetFileNameWithoutExtension(text));
			using (ZipFile zipFile = ZipFile.Read(Path.Combine(directoryInfo.FullName, text)))
			{
				foreach (ZipEntry item in zipFile)
				{
					item.Extract(directoryInfo2.FullName, ExtractExistingFileAction.OverwriteSilently);
				}
			}
			return directoryInfo2.FullName;
		}

		protected IList<ManageThemeInfo> LoadThemes(string currentThemeName)
		{
			HttpContext context = HiContext.Current.Context;
			XmlDocument xmlDocument = new XmlDocument();
			IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
			string path = context.Request.PhysicalApplicationPath + "\\Templates\\master";
			string[] array = Directory.Exists(path) ? Directory.GetDirectories(path) : null;
			ManageThemeInfo manageThemeInfo = null;
			string[] array2 = array;
			foreach (string path2 in array2)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path2);
				string text = directoryInfo.Name.ToLower(CultureInfo.InvariantCulture);
				if (text.Length > 0 && !text.StartsWith("_"))
				{
					FileInfo[] files = directoryInfo.GetFiles(text + ".xml");
					FileInfo[] array3 = files;
					foreach (FileInfo fileInfo in array3)
					{
						manageThemeInfo = new ManageThemeInfo();
						FileStream fileStream = fileInfo.OpenRead();
						xmlDocument.Load(fileStream);
						fileStream.Close();
						manageThemeInfo.Name = xmlDocument.SelectSingleNode("ManageTheme/Name").InnerText;
						manageThemeInfo.ThemeImgUrl = xmlDocument.SelectSingleNode("ManageTheme/ImageUrl").InnerText;
						manageThemeInfo.ThemeName = text;
						if (string.Compare(manageThemeInfo.ThemeName, currentThemeName) == 0)
						{
							this.litThemeName.Text = manageThemeInfo.ThemeName;
							this.imgThemeImgUrl.ImageUrl = "/Templates/master/" + text + "/" + xmlDocument.SelectSingleNode("ManageTheme/ImageUrl").InnerText;
							this.Image1.ImageUrl = "/Templates/master/" + text + "/" + xmlDocument.SelectSingleNode("ManageTheme/BigImageUrl").InnerText;
						}
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
		}
	}
}
