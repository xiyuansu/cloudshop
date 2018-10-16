using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class ActivityDetail : AdminPage
	{
		protected VActivityInfo _act;

		protected bool nodata = false;

		protected Repeater rpt;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				int urlIntParam = base.GetUrlIntParam("id");
				VActivityInfo activity = VShopHelper.GetActivity(urlIntParam);
				if (activity != null)
				{
					this._act = activity;
					IList<ActivitySignUpInfo> activitySignUpById = VShopHelper.GetActivitySignUpById(urlIntParam);
					this.rpt.DataSource = activitySignUpById;
					this.rpt.DataBind();
					this.nodata = (this.rpt.Items.Count == 0);
				}
			}
		}
	}
}
