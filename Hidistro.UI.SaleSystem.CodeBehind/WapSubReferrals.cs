using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapSubReferrals : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptSubReferrals;

		private HtmlInputHidden txtTotalPages;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubReferrals.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
			this.rptSubReferrals = (WapTemplatedRepeater)this.FindControl("rptSubReferrals");
			PageTitle.AddSiteNameTitle("下级分销员");
			this.BindSubReferrals();
		}

		private void BindSubReferrals()
		{
			int pageIndex = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 10;
			}
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.PageIndex = pageIndex;
			memberQuery.PageSize = pageSize;
			memberQuery.ReferralStatus = 2;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyword"]))
			{
				memberQuery.UserName = this.Page.Server.UrlDecode(this.Page.Request.QueryString["keyword"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realname"]))
			{
				memberQuery.RealName = this.Page.Server.UrlDecode(this.Page.Request.QueryString["realname"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cellPhone"]))
			{
				memberQuery.CellPhone = this.Page.Server.UrlDecode(this.Page.Request.QueryString["cellPhone"]);
			}
			DbQueryResult mySubUsers = MemberProcessor.GetMySubUsers(memberQuery);
			this.rptSubReferrals.DataSource = mySubUsers.Data;
			this.rptSubReferrals.DataBind();
			int totalRecords = mySubUsers.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
	}
}
