using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class MemberGroups : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptMemberGroups;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-MemberGroups.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptMemberGroups = (WapTemplatedRepeater)this.FindControl("rptMemberGroups");
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			FightGroupQuery fightGroupQuery = new FightGroupQuery();
			fightGroupQuery.PageIndex = 1;
			fightGroupQuery.PageSize = 2147483647;
			fightGroupQuery.SortBy = "StartTime";
			fightGroupQuery.SortOrder = SortAction.Asc;
			fightGroupQuery.UserId = HiContext.Current.UserId;
			fightGroupQuery.OrderStatus = list;
			this.rptMemberGroups.ItemDataBound += this.rptMemberGroups_ItemDataBound;
			PageModel<UserFightGroupActivitiyModel> myFightGroups = VShopHelper.GetMyFightGroups(fightGroupQuery);
			List<UserFightGroupActivitiyModel> list2 = myFightGroups.Models.ToList();
			foreach (UserFightGroupActivitiyModel item in list2)
			{
				if (item.EndTime <= DateTime.Now && item.GroupStatus == FightGroupStatus.FightGroupIn)
				{
					VShopHelper.DealFightGroupFail(item.FightGroupId);
					item.GroupStatus = FightGroupStatus.FightGroupFail;
				}
				if (string.IsNullOrEmpty(item.ImageUrl1))
				{
					item.ImageUrl1 = base.site.DefaultProductImage;
				}
			}
			this.rptMemberGroups.DataSource = myFightGroups.Models;
			this.rptMemberGroups.DataBind();
			if (myFightGroups.Total == 0)
			{
				this.Page.Response.Redirect("MemberGroupsNever.aspx");
			}
		}

		protected void rptMemberGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (DataBinder.Eval(e.Item.DataItem, "ProductId").ToInt(0) == 110)
				{
					int num = DataBinder.Eval(e.Item.DataItem, "ProductId").ToInt(0);
				}
				int fightGroupId = DataBinder.Eval(e.Item.DataItem, "FightGroupId").ToInt(0);
				int num2 = DataBinder.Eval(e.Item.DataItem, "JoinNumber").ToInt(0);
				IList<FightGroupUserModel> fightGroupUsers = VShopHelper.GetFightGroupUsers(fightGroupId);
				int num3 = fightGroupUsers.Count();
				if (fightGroupUsers.Count < num2)
				{
					for (int i = 0; i < num2 - num3; i++)
					{
						FightGroupUserModel item = new FightGroupUserModel();
						fightGroupUsers.Add(item);
					}
				}
				Repeater repeater = (Repeater)e.Item.Controls[0].FindControl("rptMemberGroupsUsers");
				Repeater repeater2 = (Repeater)e.Item.Controls[0].FindControl("rptMemberGroupsUsersSuccessOrFail");
				if (repeater != null)
				{
					repeater.DataSource = fightGroupUsers;
					repeater.DataBind();
				}
				if (repeater2 != null)
				{
					repeater2.DataSource = fightGroupUsers;
					repeater2.DataBind();
				}
			}
		}
	}
}
