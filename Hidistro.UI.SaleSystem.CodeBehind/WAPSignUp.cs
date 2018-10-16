using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPSignUp : WAPMemberTemplatedWebControl
	{
		private Panel pnlInfo;

		private Literal litActivityDesc;

		private Common_PrizeNames litPrizeNames;

		private Literal litStartDate;

		private Literal litEndDate;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vSignUp.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = default(int);
			int.TryParse(HttpContext.Current.Request.QueryString.Get("id"), out num);
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				HttpContext.Current.Response.Redirect("/Vshop/login.aspx?ReturnUrl=/Vshop/SignUp.aspx?id=" + num);
			}
			else
			{
				LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
				if (lotteryTicket == null)
				{
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){hideSignUpBtn();alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				}
				else
				{
					if (lotteryTicket != null && VshopBrowser.HasSignUp(num, HiContext.Current.UserId) && DateTime.Now >= lotteryTicket.OpenTime)
					{
						HttpContext.Current.Response.Redirect($"~/vshop/ticket.aspx?id={num}");
					}
					if (lotteryTicket.StartTime > DateTime.Now || DateTime.Now > lotteryTicket.EndTime)
					{
						this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){hideSignUpBtn();alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
					}
					this.pnlInfo = (Panel)this.FindControl("pnlInfo");
					this.litActivityDesc = (Literal)this.FindControl("litActivityDesc");
					this.litPrizeNames = (Common_PrizeNames)this.FindControl("litPrizeNames");
					this.litStartDate = (Literal)this.FindControl("litStartDate");
					this.litEndDate = (Literal)this.FindControl("litEndDate");
					this.pnlInfo.Visible = !string.IsNullOrEmpty(lotteryTicket.InvitationCode);
					this.litActivityDesc.Text = lotteryTicket.ActivityDesc;
					this.litPrizeNames.Activity = lotteryTicket;
					Literal literal = this.litStartDate;
					DateTime dateTime = lotteryTicket.OpenTime;
					literal.Text = dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
					Literal literal2 = this.litEndDate;
					dateTime = lotteryTicket.EndTime;
					literal2.Text = dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
					PageTitle.AddSiteNameTitle("抽奖报名");
				}
			}
		}
	}
}
