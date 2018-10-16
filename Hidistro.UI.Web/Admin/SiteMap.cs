using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SiteMap)]
	public class SiteMap : AdminPage
	{
		protected HyperLink Hysitemap;

		protected TextBox tbsitemaptime;

		protected TextBox tbsitemapnum;

		protected Button btnSaveMapSettings;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindSiteMap();
			}
			string text = HiContext.Current.HostPath + "/sitemapindex.xml";
			this.Hysitemap.Text = "本站为您自动生成的网站地图地址如下:" + text;
			this.Hysitemap.NavigateUrl = text;
			this.Hysitemap.Target = "_blank";
			StreamReader streamReader = new StreamReader(base.Server.MapPath("/robots.txt"), Encoding.Default);
			string text2 = streamReader.ReadToEnd();
			streamReader.Close();
			if (text2.Contains("Sitemap"))
			{
				text2 = text2.Substring(0, text2.IndexOf("Sitemap"));
			}
			FileStream fileStream = new FileStream(base.Server.MapPath("/robots.txt"), FileMode.OpenOrCreate, FileAccess.Write);
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default))
				{
					streamWriter.Flush();
					streamWriter.Write(text2);
					streamWriter.WriteLine("Sitemap: " + text);
					streamWriter.Flush();
					streamWriter.Dispose();
					streamWriter.Close();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				fileStream.Dispose();
				fileStream.Close();
			}
		}

		protected void BindSiteMap()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(masterSettings.SiteMapTime))
			{
				this.tbsitemaptime.Text = masterSettings.SiteMapTime;
			}
			if (!string.IsNullOrEmpty(masterSettings.SiteMapNum))
			{
				this.tbsitemapnum.Text = masterSettings.SiteMapNum;
			}
		}

		protected void btnSaveMapSettings_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(this.tbsitemaptime.Text) && !string.IsNullOrEmpty(this.tbsitemapnum.Text))
			{
				masterSettings.SiteMapNum = this.tbsitemapnum.Text;
				masterSettings.SiteMapTime = this.tbsitemaptime.Text;
				SettingsManager.Save(masterSettings);
				this.BindSiteMap();
				this.ShowMsg("保存成功。", true);
			}
			else
			{
				this.ShowMsg("参数错误。", false);
			}
		}
	}
}
