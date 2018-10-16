using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserDefault : MemberTemplatedWebControl
	{
		private Literal litUserName;

		private Literal litUserPoint;

		private Literal litUserRank;

		private Literal litNoPayOrderNum;

		private Literal litWaitSendOrderNum;

		private Literal litPayOrderNum;

		private Literal litWaitReviewNum;

		private Literal litNoReplyLeaveWordNum;

		private Literal litemmailverfice;

		private Literal litcellphoneverfice;

		private Literal litUserCouponCount;

		private Literal litUserRedCount;

		private Literal litRefundNum;

		private Literal litReturnNum;

		private FormatedMoneyLabel litAccountAmount;

		private FormatedMoneyLabel litUseableBalance;

		private FormatedMoneyLabel litRequestBalance;

		private Image userPicture;

		private HyperLink hpMes;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserDefault.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litRefundNum = (Literal)this.FindControl("litRefundNum");
			this.litReturnNum = (Literal)this.FindControl("litReturnNum");
			this.litWaitSendOrderNum = (Literal)this.FindControl("litWaitSendOrderNum");
			this.litUserRedCount = (Literal)this.FindControl("litUserRedCount");
			this.litUserCouponCount = (Literal)this.FindControl("litUserCouponCount");
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.litUserPoint = (Literal)this.FindControl("litUserPoint");
			this.litUserRank = (Literal)this.FindControl("litUserRank");
			this.litemmailverfice = (Literal)this.FindControl("litemmailverfice");
			this.litcellphoneverfice = (Literal)this.FindControl("litcellphoneverfice");
			this.litNoPayOrderNum = (Literal)this.FindControl("litNoPayOrderNum");
			this.litPayOrderNum = (Literal)this.FindControl("litPayOrderNum");
			this.litWaitReviewNum = (Literal)this.FindControl("litWaitReviewNum");
			this.litNoReplyLeaveWordNum = (Literal)this.FindControl("litNoReplyLeaveWordNum");
			this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
			this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			this.userPicture = (Image)this.FindControl("userPicture");
			this.hpMes = (HyperLink)this.FindControl("hpMes");
			PageTitle.AddSiteNameTitle("会员中心首页");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				Literal literal = this.litUserPoint;
				int num = user.Points;
				literal.Text = num.ToString();
				this.litUserName.Text = user.UserName;
				if (string.IsNullOrEmpty(user.CellPhone))
				{
					this.litcellphoneverfice.Text = "手机未绑定<a href=\"UserCellPhoneVerification.aspx\">去绑定</a>";
				}
				else
				{
					this.litcellphoneverfice.Text = (user.CellPhoneVerification ? "手机验证完成" : "手机未验证<a href=\"UserCellPhoneVerification.aspx\">去验证</a>");
				}
				if (string.IsNullOrEmpty(user.Email))
				{
					this.litemmailverfice.Text = "邮箱未绑定<a href=\"UserEmailVerification.aspx\">去绑定</a>";
				}
				else
				{
					this.litemmailverfice.Text = (user.EmailVerification ? "邮箱验证完成" : "邮箱未验证<a href=\"UserEmailVerification.aspx\">去验证</a>");
				}
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(user.GradeId);
				if (memberGrade != null)
				{
					this.litUserRank.Text = memberGrade.Name;
				}
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				IDictionary<string, int> statisticsNum = MemberProcessor.GetStatisticsNum();
				Literal literal2 = this.litNoPayOrderNum;
				object[] obj = new object[5]
				{
					"<a href=\"UserOrders.aspx?orderStatus=",
					1,
					"\">",
					null,
					null
				};
				num = statisticsNum["noPayOrderNum"];
				obj[3] = num.ToString();
				obj[4] = "</a>";
				literal2.Text = string.Concat(obj);
				Literal literal3 = this.litNoReplyLeaveWordNum;
				num = statisticsNum["noReplyLeaveCommentNum"];
				literal3.Text = num.ToString();
				Literal literal4 = this.litPayOrderNum;
				object[] obj2 = new object[7]
				{
					"<a href=\"UserOrders.aspx?orderStatus=",
					3,
					"&itemStatus=",
					0,
					"\">",
					null,
					null
				};
				num = statisticsNum["payOrderNum"];
				obj2[5] = num.ToString();
				obj2[6] = "</a>";
				literal4.Text = string.Concat(obj2);
				Literal literal5 = this.litWaitSendOrderNum;
				object[] obj3 = new object[7]
				{
					"<a href=\"UserOrders.aspx?orderStatus=",
					2,
					"&itemStatus=",
					0,
					"\">",
					null,
					null
				};
				num = statisticsNum["waitSendOrderNum"];
				obj3[5] = num.ToString();
				obj3[6] = "</a>";
				literal5.Text = string.Concat(obj3);
				Literal literal6 = this.litWaitReviewNum;
				num = statisticsNum["WaitReviewNum"];
				literal6.Text = num.ToString();
				HyperLink hyperLink = this.hpMes;
				num = statisticsNum["noReadMessageNum"];
				hyperLink.Text = num.ToString();
				this.litAccountAmount.Money = user.Balance;
				this.litRequestBalance.Money = user.RequestBalance;
				this.litUseableBalance.Money = user.Balance - user.RequestBalance;
				if (!string.IsNullOrEmpty(user.Picture))
				{
					this.userPicture.ImageUrl = user.Picture;
				}
				this.hpMes.NavigateUrl = "UserReceivedMessages.aspx";
				Literal literal7 = this.litUserCouponCount;
				num = CouponHelper.GetUserObtainCouponNum(user.UserId);
				literal7.Text = num.ToString();
				Literal literal8 = this.litUserRedCount;
				num = CouponHelper.GetUserObtainRedENum(user.UserId);
				literal8.Text = num.ToString();
				Literal literal9 = this.litRefundNum;
				num = OrderHelper.GetRefundApplys(new RefundApplyQuery
				{
					UserId = user.UserId,
					PageIndex = 1,
					PageSize = 2147483647,
					HandleStatus = 0
				}).Total;
				literal9.Text = num.ToString();
				Literal literal10 = this.litReturnNum;
				num = OrderHelper.GetReturnsApplys(new ReturnsApplyQuery
				{
					UserId = user.UserId,
					PageIndex = 1,
					PageSize = 2147483647,
					IsNoCompleted = true
				}).Total;
				literal10.Text = num.ToString();
			}
		}
	}
}
