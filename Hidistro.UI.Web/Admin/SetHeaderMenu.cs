using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SetHeaderMenu)]
	public class SetHeaderMenu : AdminPage
	{
		protected string themName;

		protected Literal litThemName;

		protected HyperLink hlinkAddHeaderMenu;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.themName = HiContext.Current.SiteSettings.Theme;
			if (!this.Page.IsPostBack)
			{
				this.litThemName.Text = this.themName;
				this.hlinkAddHeaderMenu.NavigateUrl = Globals.GetAdminAbsolutePath("/store/AddHeaderMenu.aspx?ThemName=" + this.themName);
			}
		}
	}
}
