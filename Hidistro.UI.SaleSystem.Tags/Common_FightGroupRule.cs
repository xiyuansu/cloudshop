using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_FightGroupRule : WAPTemplatedWebControl
	{
		private Literal litRule;

		public int FightGroupActivityId
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-FightGroupRule.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litRule = (Literal)this.FindControl("litRule");
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(this.FightGroupActivityId);
			if (fightGroupActivitieInfo != null)
			{
				this.litRule.Text = $"邀请{fightGroupActivitieInfo.JoinNumber}人即可成团，人数不足自动退款";
			}
		}
	}
}
