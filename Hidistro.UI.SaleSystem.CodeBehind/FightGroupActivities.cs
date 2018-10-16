using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FightGroupActivities : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptFightGroupActivities;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-FightGroupActivities.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = this.Page.Request["PageIndex"].ToInt(0);
			int num2 = this.Page.Request["PageSize"].ToInt(0);
			FightGroupActivityQuery fightGroupActivityQuery = new FightGroupActivityQuery();
			fightGroupActivityQuery.PageIndex = ((num == 0) ? 1 : num);
			fightGroupActivityQuery.PageSize = ((num2 == 0) ? 3 : num2);
			fightGroupActivityQuery.SortBy = "DisplaySequence DESC,FightGroupActivityId";
			fightGroupActivityQuery.SortOrder = SortAction.Asc;
			fightGroupActivityQuery.IsCount = true;
			this.rptFightGroupActivities = (WapTemplatedRepeater)this.FindControl("rptFightGroupActivities");
			this.rptFightGroupActivities.ItemDataBound += this.rptFightGroupActivities_ItemDataBound;
			PageModel<FightGroupActivitiyModel> fightGroupActivitieLists = VShopHelper.GetFightGroupActivitieLists(fightGroupActivityQuery);
			this.rptFightGroupActivities.DataSource = fightGroupActivitieLists.Models;
			this.rptFightGroupActivities.DataBind();
			if (fightGroupActivitieLists.Total == 0)
			{
				this.Page.Response.Redirect("FightGroupNever.aspx");
			}
		}

		protected void rptFightGroupActivities_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DateTime? t = DataBinder.Eval(e.Item.DataItem, "StartDate").ToDateTime();
				DateTime? nullable = DataBinder.Eval(e.Item.DataItem, "EndDate").ToDateTime();
				Control control = e.Item.Controls[0].FindControl("div_comeing_soon");
				control.Visible = (t > (DateTime?)DateTime.Now);
			}
		}
	}
}
