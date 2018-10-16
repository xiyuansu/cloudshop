using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SetSynJDOrder)]
	public class JDOrder : AdminPage
	{
		private string appKey;

		private string appSecret;

		private string accessToken;

		protected CalendarPanel cldStartDate;

		protected CalendarPanel cldEndDate;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HtmlInputHidden hidOrderId;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.appKey = masterSettings.JDAppKey;
			this.appSecret = masterSettings.JDAppSecret;
			this.accessToken = masterSettings.JDAccessToken;
			if (string.IsNullOrWhiteSpace(this.appKey) || string.IsNullOrWhiteSpace(this.appSecret) || string.IsNullOrWhiteSpace(this.accessToken))
			{
				base.Response.Redirect("./JDSettings");
			}
		}
	}
}
