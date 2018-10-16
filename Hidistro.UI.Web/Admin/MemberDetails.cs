using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Members)]
	public class MemberDetails : AdminPage
	{
		protected int userId;

		protected Literal litUserName;

		protected Image userPicture;

		protected Literal litShowUserName;

		protected Literal litGrade;

		protected HtmlGenericControl divTags;

		protected Literal litRealName;

		protected Literal litNickName;

		protected Literal litGender;

		protected Literal litEmail;

		protected Literal litBirthDate;

		protected Literal litQQ;

		protected Literal litCellPhone;

		protected Literal litReferral;

		protected Literal litCreateDate;

		protected Literal litAddress;

		protected Literal litConsumeAmount;

		protected Literal litConsumeTimes;

		protected Literal litMoney;

		protected Literal litPoints;

		protected Literal litCouponCount;

		protected Literal litDormancyDay;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				this.userId = 0;
			}
			if (this.userId <= 0)
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				this.LoadMemberInfo();
			}
		}

		private void LoadMemberInfo()
		{
			MemberInfo user = Users.GetUser(this.userId);
			if (user == null)
			{
				string text = base.Request.UrlReferrer.ToNullString();
				if (string.IsNullOrEmpty(text))
				{
					text = "/Admin/member/managemembers";
				}
				this.ShowMsg("会员已被删除", false, text);
			}
			else
			{
				if (!string.IsNullOrEmpty(user.Picture.ToNullString()))
				{
					this.userPicture.ImageUrl = user.Picture;
				}
				else
				{
					this.userPicture.ImageUrl = HiContext.Current.GetSkinPath() + "/images/users/hyzx_25.jpg";
				}
				Literal literal = this.litUserName;
				Literal literal2 = this.litShowUserName;
				string text3 = literal.Text = (literal2.Text = user.UserName);
				this.litGrade.Text = MemberHelper.GetMemberGrade(user.GradeId).Name;
				this.litRealName.Text = user.RealName;
				this.litEmail.Text = user.Email;
				if (user.Gender == Gender.Female)
				{
					this.litGender.Text = "女";
				}
				else if (user.Gender == Gender.Male)
				{
					this.litGender.Text = "男";
				}
				else
				{
					this.litGender.Text = "保密";
				}
				this.litCellPhone.Text = user.CellPhone;
				Literal literal3 = this.litBirthDate;
				DateTime dateTime;
				object text4;
				if (user.BirthDate.HasValue)
				{
					dateTime = user.BirthDate.Value;
					text4 = dateTime.ToString("yyyy-MM-dd");
				}
				else
				{
					text4 = "";
				}
				literal3.Text = (string)text4;
				this.litNickName.Text = user.NickName;
				this.litQQ.Text = user.QQ;
				Literal literal4 = this.litCreateDate;
				dateTime = user.CreateDate;
				literal4.Text = dateTime.ToString();
				this.litAddress.Text = RegionHelper.GetFullRegion(user.RegionId, "", true, 0) + user.Address;
				if (user.ReferralUserId > 0)
				{
					MemberInfo user2 = Users.GetUser(user.ReferralUserId);
					if (user2 != null)
					{
						this.litReferral.Text = user2.UserName;
					}
				}
				string tagIds = user.TagIds;
				if (!string.IsNullOrWhiteSpace(tagIds))
				{
					if (tagIds == ",")
					{
						tagIds = "";
					}
					else
					{
						tagIds = tagIds.TrimStart(',');
						tagIds = tagIds.TrimEnd(',');
					}
					if (tagIds != "")
					{
						IList<MemberTagInfo> tagByMember = MemberTagHelper.GetTagByMember(tagIds);
						StringBuilder stringBuilder = new StringBuilder();
						foreach (MemberTagInfo item in tagByMember)
						{
							stringBuilder.AppendFormat("<span>{0}</span>", item.TagName);
						}
						this.divTags.InnerHtml = stringBuilder.ToString();
					}
				}
				this.litConsumeAmount.Text = user.Expenditure.F2ToString("f2");
				Literal literal5 = this.litConsumeTimes;
				int num = user.OrderNumber;
				literal5.Text = num.ToString();
				this.litMoney.Text = user.Balance.F2ToString("f2");
				Literal literal6 = this.litPoints;
				num = user.Points;
				literal6.Text = num.ToString();
				Literal literal7 = this.litCouponCount;
				num = CouponHelper.GetUserObtainCouponNum(user.UserId);
				literal7.Text = num.ToString();
				Literal literal8 = this.litDormancyDay;
				num = MemberHelper.GetUserDormancyDays(user.UserId);
				literal8.Text = num.ToString();
			}
		}
	}
}
