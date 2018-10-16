using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.FightGroupManage)]
	public class FightGroupShareLink : AdminPage
	{
		public int fightGroupActivityId;

		protected Literal ltActivityUrl;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.fightGroupActivityId = this.Page.Request["fightGroupActivityId"].ToInt(0);
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string text = "/Vshop/FightGroupActivityDetails.aspx?fightGroupActivityId=" + this.fightGroupActivityId;
				this.ltActivityUrl.Text = ((masterSettings.SiteUrl.IndexOf("http://") >= 0) ? (masterSettings.SiteUrl + text) : ("http://" + masterSettings.SiteUrl + text));
			}
		}
	}
}
