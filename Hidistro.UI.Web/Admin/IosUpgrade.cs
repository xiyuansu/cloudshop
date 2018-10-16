using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppIosUpgrade)]
	public class IosUpgrade : AdminPage
	{
		protected TextBox txtDownloadUrl;

		protected Button btnUpoad;

		protected Literal litVersion;

		protected HtmlInputHidden hidIsForcibleUpgrade;

		protected Literal litDescription;

		protected Literal litUpgradeUrl;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpoad.Click += this.btnUpoad_Click;
			if (!this.Page.IsPostBack)
			{
				this.txtDownloadUrl.Text = HiContext.Current.SiteSettings.AppIOSDownLoadUrl;
				this.LoadVersion();
			}
		}

		private void btnUpoad_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text2 = masterSettings.AppIOSDownLoadUrl = Globals.StripAllTags(this.txtDownloadUrl.Text);
			SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功！", true);
		}

		private void LoadVersion()
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(HttpContext.Current.Request.MapPath("/Storage/data/app/ios/IosUpgrade.xml"));
				this.litVersion.Text = xmlDocument.SelectSingleNode("root/Version").InnerText;
				this.litDescription.Text = xmlDocument.SelectSingleNode("root/Description").InnerText;
				this.litUpgradeUrl.Text = xmlDocument.SelectSingleNode("root/UpgradeUrl").InnerText;
			}
			catch
			{
			}
		}
	}
}
