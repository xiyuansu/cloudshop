using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapSubMembers : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptSubMembers;

		private HtmlInputHidden txtTotalPages;

		private Literal litExpandMemberInMonth;

		private Literal litExpandMemberAll;

		private FormatedMoneyLabel fmlNextMemberTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubMembers.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litExpandMemberInMonth = (Literal)this.FindControl("litExpandMemberInMonth");
			this.litExpandMemberAll = (Literal)this.FindControl("litExpandMemberAll");
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("pageTotal");
			this.fmlNextMemberTotal = (FormatedMoneyLabel)this.FindControl("fmlNextMemberTotal");
			this.rptSubMembers = (WapTemplatedRepeater)this.FindControl("rptSubMembers");
			PageTitle.AddSiteNameTitle("下级分销员");
			this.BindSubReferrals();
		}

		private void BindSubReferrals()
		{
			int userId = HiContext.Current.User.UserId;
			this.litExpandMemberInMonth.Text = MemberProcessor.GetLowerNumByUserIdNowMonth(userId).ToNullString();
			this.litExpandMemberAll.Text = MemberProcessor.GetLowerNumByUserId(userId).ToNullString();
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
			this.fmlNextMemberTotal.Text = MemberProcessor.GetLowerSaleTotalByUserId(userId).F2ToString("f2");
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.PageIndex = pageIndex;
			memberQuery.PageSize = pageSize;
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
			if (mySubUsers.Data != null)
			{
				mySubUsers.Data.Columns.Add("ShowUserName");
				DataTable data = mySubUsers.Data;
				foreach (DataRow row in data.Rows)
				{
					if (!string.IsNullOrEmpty(row["NickName"].ToNullString()))
					{
						row["ShowUserName"] = row["NickName"];
					}
					else if (!string.IsNullOrEmpty(row["ReferralCellPhone"].ToNullString()))
					{
						row["ShowUserName"] = row["ReferralCellPhone"];
					}
					else if (!string.IsNullOrEmpty(row["CellPhone"].ToNullString()))
					{
						row["ShowUserName"] = row["CellPhone"];
					}
					else
					{
						row["ShowUserName"] = row["UserName"];
					}
				}
			}
			this.rptSubMembers.DataSource = mySubUsers.Data;
			this.rptSubMembers.DataBind();
			int totalRecords = mySubUsers.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
	}
}
