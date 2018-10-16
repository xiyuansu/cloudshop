using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapSubReferral_Detail : WAPMemberTemplatedWebControl
	{
		private Literal litUsername;

		private Literal litTrueName;

		private Literal litTelphone;

		private Literal litOrderCount;

		private Literal litAuditTime;

		private Literal litLastOrderTime;

		private FormatedMoneyLabel litAmount;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubReferral_Detail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litAmount = (FormatedMoneyLabel)this.FindControl("litAmount");
			this.litUsername = (Literal)this.FindControl("litUsername");
			this.litTrueName = (Literal)this.FindControl("litTrueName");
			this.litOrderCount = (Literal)this.FindControl("litOrderCount");
			this.litTelphone = (Literal)this.FindControl("litTelphone");
			this.litAuditTime = (Literal)this.FindControl("litAuditTime");
			this.litLastOrderTime = (Literal)this.FindControl("litLastOrderTime");
			PageTitle.AddSiteNameTitle("下级分销员详情");
			int userId = 0;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserID"]))
			{
				int.TryParse(this.Page.Request.QueryString["UserID"], out userId);
			}
			SubReferralUser myReferralSubUser = MemberProcessor.GetMyReferralSubUser(userId);
			if (myReferralSubUser == null)
			{
				this.ShowMessage("错误的分销员ID", false, "", 1);
			}
			this.litAmount.Money = myReferralSubUser.SubReferralSplittin;
			this.litUsername.Text = myReferralSubUser.UserName;
			this.litTrueName.Text = myReferralSubUser.RealName;
			this.litOrderCount.Text = myReferralSubUser.ReferralOrderNumber.ToString();
			this.litTelphone.Text = myReferralSubUser.CellPhone;
			Literal literal = this.litAuditTime;
			object text;
			DateTime value;
			if (!myReferralSubUser.ReferralAuditDate.HasValue)
			{
				text = "";
			}
			else
			{
				value = myReferralSubUser.ReferralAuditDate.Value;
				text = value.ToString("yyyy-MM-dd HH:mm:ss");
			}
			literal.Text = (string)text;
			Literal literal2 = this.litLastOrderTime;
			object text2;
			if (!myReferralSubUser.LastReferralDate.HasValue)
			{
				text2 = "";
			}
			else
			{
				value = myReferralSubUser.LastReferralDate.Value;
				text2 = value.ToString("yyyy-MM-dd HH:mm:ss");
			}
			literal2.Text = (string)text2;
		}
	}
}
