using Hidistro.Context;
using Hidistro.SaleSystem.Store;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class CnzzStatisticTotal : Page
	{
		protected HtmlGenericControl framcnz;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!string.IsNullOrEmpty(siteSettings.CnzzPassword) && !string.IsNullOrEmpty(siteSettings.CnzzUsername))
			{
				this.framcnz.Attributes["src"] = "https://wss.cnzz.com/user/companion/92hi_login.php?site_id=" + siteSettings.CnzzUsername + "&password=" + siteSettings.CnzzPassword;
			}
			else
			{
				this.Page.Response.Redirect("cnzzstatisticsset.aspx");
			}
		}
	}
}
