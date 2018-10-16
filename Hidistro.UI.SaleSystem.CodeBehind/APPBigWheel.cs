using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class APPBigWheel : AppshopTemplatedWebControl
	{
		private HtmlInputHidden hdIsLogined;

		private HtmlInputHidden hidPoint;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-BigWheel.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = this.Page.Request["ActivityId"].ToInt(0);
			if (num <= 0)
			{
				base.GotoResourceNotFound("活动不存在");
			}
			else
			{
				this.hdIsLogined = (HtmlInputHidden)this.FindControl("hdIsLogined");
				this.hidPoint = (HtmlInputHidden)this.FindControl("hidPoint");
				this.hdIsLogined.Value = ((HiContext.Current.UserId == 0) ? "0" : "1");
				ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(num);
				if (activityInfo == null || activityInfo.ActivityType != 1)
				{
					base.GotoResourceNotFound("活动不存在");
				}
				else
				{
					if (HiContext.Current.User != null)
					{
						this.hidPoint.Value = HiContext.Current.User.Points.ToString();
					}
					PageTitle.AddSiteNameTitle(activityInfo.ActivityName);
				}
			}
		}
	}
}
