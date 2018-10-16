using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPTicket : WAPMemberTemplatedWebControl
	{
		private Literal litActivityDesc;

		private Common_PrizeNames litPrizeNames;

		private Literal litStartDate;

		private Literal litEndDate;

		private HtmlInputHidden hdTitle;

		private HtmlInputHidden hdDesc;

		private HtmlInputHidden hdAppId;

		private HtmlInputHidden hdImgUrl;

		private HtmlInputHidden hdLink;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vTicket.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = default(int);
			int.TryParse(HttpContext.Current.Request.QueryString.Get("id"), out num);
			LotteryTicketInfo lotteryTicket = VshopBrowser.GetLotteryTicket(num);
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				HttpContext.Current.Response.Redirect("/Vshop/login.aspx?ReturnUrl=/Vshop/Ticket.aspx?id=" + num);
			}
			else if (lotteryTicket == null)
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
			}
			else
			{
				if (lotteryTicket != null && !VshopBrowser.HasSignUp(num, HiContext.Current.UserId) && DateTime.Now >= lotteryTicket.StartTime)
				{
					HttpContext.Current.Response.Redirect($"~/vshop/SignUp.aspx?id={num}");
				}
				if (lotteryTicket.StartTime > DateTime.Now || DateTime.Now > lotteryTicket.EndTime)
				{
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){hideSignUpBtn();alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				}
				if (lotteryTicket.OpenTime > DateTime.Now)
				{
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){hideSignUpBtn();alert_h(\"抽奖还未开始！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				}
				this.litActivityDesc = (Literal)this.FindControl("litActivityDesc");
				this.litPrizeNames = (Common_PrizeNames)this.FindControl("litPrizeNames");
				this.litStartDate = (Literal)this.FindControl("litStartDate");
				this.litEndDate = (Literal)this.FindControl("litEndDate");
				this.hdTitle = (HtmlInputHidden)this.FindControl("hdTitle");
				this.hdAppId = (HtmlInputHidden)this.FindControl("hdAppId");
				this.hdDesc = (HtmlInputHidden)this.FindControl("hdDesc");
				this.hdImgUrl = (HtmlInputHidden)this.FindControl("hdImgUrl");
				this.hdLink = (HtmlInputHidden)this.FindControl("hdLink");
				if (base.ClientType == ClientType.VShop)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string local = (!string.IsNullOrWhiteSpace(lotteryTicket.ActivityPic)) ? lotteryTicket.ActivityPic : Globals.FullPath(masterSettings.LogoUrl);
					this.hdImgUrl.Value = Globals.FullPath(local);
					this.hdTitle.Value = lotteryTicket.ActivityName;
					this.hdDesc.Value = lotteryTicket.ActivityDesc;
					this.hdLink.Value = Globals.FullPath($"/vshop/Ticket?id={lotteryTicket.ActivityId}");
					this.hdAppId.Value = masterSettings.WeixinAppId;
				}
				this.litActivityDesc.Text = lotteryTicket.ActivityDesc;
				this.litPrizeNames.Activity = lotteryTicket;
				Literal literal = this.litStartDate;
				DateTime dateTime = lotteryTicket.OpenTime;
				literal.Text = dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
				Literal literal2 = this.litEndDate;
				dateTime = lotteryTicket.EndTime;
				literal2.Text = dateTime.ToString("yyyy年MM月dd日 HH:mm:ss");
				PageTitle.AddSiteNameTitle("微抽奖");
			}
		}
	}
}
