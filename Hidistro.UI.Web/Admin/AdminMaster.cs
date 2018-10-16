using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class AdminMaster : MasterPage
	{
		protected PageTitle PageTitle1;

		protected ContentPlaceHolder headHolder;

		protected ContentPlaceHolder validateHolder;

		protected HtmlForm thisForm;

		protected Image imgLogo;

		protected HyperLink hlinkDefault;

		protected HyperLink hlinkAdminDefault;

		protected Label lblUserName;

		protected HyperLink hlinkLogout;

		protected HyperLink hlinkService;

		protected Literal mainMenuHolder;

		protected Literal subMenuHolder;

		protected ContentPlaceHolder contentHolder;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			PageTitle.AddTitle(HiContext.Current.SiteSettings.SiteName, this.Context);
			foreach (Control control in this.Page.Header.Controls)
			{
				if (control is HtmlLink)
				{
					HtmlLink htmlLink = control as HtmlLink;
					if (htmlLink.Href.StartsWith("/"))
					{
						htmlLink.Href = htmlLink.Href;
					}
					else
					{
						htmlLink.Href = "/" + htmlLink.Href;
					}
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
