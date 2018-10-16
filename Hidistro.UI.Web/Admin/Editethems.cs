using Hidistro.Context;
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
	[PrivilegeCheck(Privilege.EditeThems)]
	public class Editethems : AdminPage
	{
		protected Literal litThemeName;

		protected HtmlAnchor aDefaultDesig;

		protected Repeater rp_custom;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.litThemeName.Text = HiContext.Current.SiteSettings.Theme;
			this.DataBindCustomTheme();
			int homePageTopicId = HiContext.Current.SiteSettings.HomePageTopicId;
			if (HiContext.Current.SiteSettings.HomePageTopicId > 0)
			{
				this.aDefaultDesig.HRef = "PcTopicTempEdit.aspx?TopicId=" + homePageTopicId;
			}
		}

		private void DataBindCustomTheme()
		{
			this.rp_custom.DataSource = this.GetCustomDocument();
			this.rp_custom.DataBind();
		}

		private XmlNodeList GetCustomDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + HiContext.Current.SiteSettings.Theme + ".xml");
			XmlDocument xmlDocument = null;
			xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			return xmlDocument.SelectNodes("//CustomTheme/Theme");
		}
	}
}
