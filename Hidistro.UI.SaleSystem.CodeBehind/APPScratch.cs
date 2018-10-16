using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class APPScratch : AppshopMemberTemplatedWebControl
	{
		private int activityid;

		private HtmlImage bgimg;

		private Literal litActivityDesc;

		private Common_PrizeNames litPrizeNames;

		private Common_PrizeUsers litPrizeUsers;

		private Literal litStartDate;

		private Literal litEndDate;

		private HtmlInputHidden hidIsUsePoint;

		private HtmlInputHidden hidFreeTimes;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VScratch.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityid"], out this.activityid))
			{
				base.GotoResourceNotFound("");
			}
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"请先登入后抽奖！\",function(){ToLogin();});});</script>");
			}
			else
			{
				this.bgimg = (HtmlImage)this.FindControl("bgimg");
				this.litActivityDesc = (Literal)this.FindControl("litActivityDesc");
				this.litPrizeNames = (Common_PrizeNames)this.FindControl("litPrizeNames");
				this.litPrizeUsers = (Common_PrizeUsers)this.FindControl("litPrizeUsers");
				this.litStartDate = (Literal)this.FindControl("litStartDate");
				this.litEndDate = (Literal)this.FindControl("litEndDate");
				this.hidIsUsePoint = (HtmlInputHidden)this.FindControl("hidIsUsePoint");
				this.hidFreeTimes = (HtmlInputHidden)this.FindControl("hidFreeTimes");
				PageTitle.AddSiteNameTitle("刮刮卡");
				ActivityInfo activityInfo = ActivityHelper.GetActivityInfo(this.activityid);
				if (activityInfo == null || activityInfo.ActivityType != 2)
				{
					base.GotoResourceNotFound("活动不存在");
				}
				else
				{
					Literal literal = this.litStartDate;
					DateTime dateTime = activityInfo.StartDate;
					literal.Text = dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
					Literal literal2 = this.litEndDate;
					dateTime = activityInfo.EndDate;
					literal2.Text = dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
					this.litActivityDesc.Text = activityInfo.Description;
					this.litPrizeNames.ActivityId = this.activityid;
					this.litPrizeUsers.ActivityId = this.activityid;
					if (activityInfo.StartDate < DateTime.Now && DateTime.Now < activityInfo.EndDate)
					{
						string arg = "您一共有";
						int num = activityInfo.FreeTimes;
						ActivityJoinStatisticsInfo currUserActivityStatisticsInfo = ActivityHelper.GetCurrUserActivityStatisticsInfo(user.UserId, this.activityid);
						if (currUserActivityStatisticsInfo == null)
						{
							num = activityInfo.FreeTimes;
						}
						else if (activityInfo.ResetType == 2)
						{
							arg = "您每天有";
							dateTime = DateTime.Now;
							DateTime date = dateTime.Date;
							dateTime = currUserActivityStatisticsInfo.LastJoinDate;
							num = ((!(date == dateTime.Date)) ? activityInfo.FreeTimes : (activityInfo.FreeTimes - currUserActivityStatisticsInfo.FreeNum));
						}
						else
						{
							num = activityInfo.FreeTimes - currUserActivityStatisticsInfo.FreeNum;
						}
						this.hidFreeTimes.Value = num.ToString();
						if (num <= 0)
						{
							this.hidIsUsePoint.Value = "1";
							Literal literal3 = this.litActivityDesc;
							literal3.Text += $"{arg}{activityInfo.FreeTimes}次免费参与机会，目前还剩0次,如要继续抽奖，则会消耗{activityInfo.ConsumptionIntegral}积分一次";
						}
						else
						{
							this.hidIsUsePoint.Value = "0";
							Literal literal4 = this.litActivityDesc;
							literal4.Text += $"{arg}{activityInfo.FreeTimes}次免费参与机会，目前还剩{num}次。";
						}
					}
					else
					{
						this.ShowMessage("活动还未开始或者已经结束！", false, "", 1);
					}
				}
			}
		}
	}
}
