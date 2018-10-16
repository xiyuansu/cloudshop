using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.CountDown)]
	public class CountDowns : AdminPage
	{
		protected int state;

		protected HiddenField hidOpenMultStore;

		protected HiddenField hidState;

		protected HtmlGenericControl divCheckAll;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.state = this.Page.Request["State"].ToInt(0);
			this.hidState.Value = this.state.ToString();
			this.hidOpenMultStore.Value = (SettingsManager.GetMasterSettings().OpenMultStore ? "1" : "0");
		}
	}
}
