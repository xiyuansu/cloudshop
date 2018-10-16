using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class CountDownsDetails : AdminPage
	{
		protected int countDownId;

		protected HiddenField hidOpenMultStore;

		protected HtmlAnchor aReturnUrl;

		protected Panel pnlStoreDetail;

		protected Literal ltStoreNames;

		protected DropDownList ddlStores;

		protected DropDownList ddlStatus;

		protected HiddenField hidState;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["countDownId"], out this.countDownId))
			{
				base.GotoResourceNotFound();
			}
			else if (SettingsManager.GetMasterSettings().OpenMultStore)
			{
				CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(this.countDownId, 0);
				List<StoreBase> activityStores = StoreActivityHelper.GetActivityStores(countDownInfo.CountDownId, 2, countDownInfo.StoreType);
				this.ddlStores.DataSource = activityStores;
				this.ddlStores.DataBind();
				this.ddlStores.Items.Insert(0, new ListItem
				{
					Text = "请选择",
					Value = "-1"
				});
				this.pnlStoreDetail.Visible = true;
				this.ltStoreNames.Text = string.Format("已参加活动门店({0}家):{1}", activityStores.Count, (from t in activityStores
				select t.StoreName).Aggregate((string t, string n) => t + "、" + n));
				this.hidOpenMultStore.Value = "1";
			}
			else
			{
				this.hidOpenMultStore.Value = "0";
				this.pnlStoreDetail.Visible = false;
			}
		}
	}
}
