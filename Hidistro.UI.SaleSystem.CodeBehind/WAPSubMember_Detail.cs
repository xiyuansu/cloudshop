using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPSubMember_Detail : WAPMemberTemplatedWebControl
	{
		private Literal litUsername;

		private Literal litTrueName;

		private Literal litTelphone;

		private Literal litOrderCount;

		private Literal litCreateTime;

		private Literal litLastOrderTime;

		private FormatedMoneyLabel litAmount;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubMember_Detail.html";
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
			this.litCreateTime = (Literal)this.FindControl("litCreateTime");
			this.litLastOrderTime = (Literal)this.FindControl("litLastOrderTime");
			PageTitle.AddSiteNameTitle("下级会员详情");
			int userId = 0;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserID"]))
			{
				int.TryParse(this.Page.Request.QueryString["UserID"], out userId);
			}
			SubMember mySubUser = MemberProcessor.GetMySubUser(userId);
			if (mySubUser == null)
			{
				this.ShowMessage("错误的会员ID", false, "", 1);
			}
			this.litUsername.Text = mySubUser.UserName;
			this.litTrueName.Text = mySubUser.RealName;
			this.litOrderCount.Text = mySubUser.OrderNumber.ToString();
			this.litTelphone.Text = mySubUser.CellPhone;
			this.litCreateTime.Text = mySubUser.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
		}
	}
}
