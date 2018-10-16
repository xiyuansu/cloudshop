using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SubReferrals : MemberTemplatedWebControl
	{
		private TextBox txtSearchText;

		private HtmlAnchor abtnSearch;

		private ThemedTemplatedRepeater grdReferralmembers;

		private Pager pager;

		private Literal litExpandMemberInMonth;

		private Literal litExpandMemberAll;

		private FormatedMoneyLabel fmlNextMemberTotal;

		private CalendarPanel calendarStart;

		private CalendarPanel calendarEnd;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SubReferrals.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!HiContext.Current.User.IsReferral())
			{
				this.Page.Response.Redirect("/User/ReferralRegisterAgreement", true);
			}
			this.txtSearchText = (TextBox)this.FindControl("txtNextUserName");
			this.abtnSearch = (HtmlAnchor)this.FindControl("abtnSearch");
			this.grdReferralmembers = (ThemedTemplatedRepeater)this.FindControl("grdReferralmembers");
			this.pager = (Pager)this.FindControl("pager");
			this.litExpandMemberInMonth = (Literal)this.FindControl("litExpandMemberInMonth");
			this.litExpandMemberAll = (Literal)this.FindControl("litExpandMemberAll");
			this.fmlNextMemberTotal = (FormatedMoneyLabel)this.FindControl("fmlNextMemberTotal");
			this.calendarStart = (CalendarPanel)this.FindControl("calendarStart");
			this.calendarEnd = (CalendarPanel)this.FindControl("calendarEnd");
			this.abtnSearch.ServerClick += this.btnSearchButton_Click;
			if (!this.Page.IsPostBack)
			{
				int userId = HiContext.Current.User.UserId;
				PageTitle.AddSiteNameTitle("下级分销员");
				this.litExpandMemberInMonth.Text = MemberProcessor.GetLowerNumByUserIdNowMonth(userId).ToNullString();
				this.litExpandMemberAll.Text = MemberProcessor.GetLowerNumByUserId(userId).ToNullString();
				this.fmlNextMemberTotal.Text = MemberProcessor.GetLowerSaleTotalByUserId(userId).F2ToString("f2");
				MemberQuery memberQuery = new MemberQuery();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["username"]))
				{
					memberQuery.UserName = this.Page.Server.UrlDecode(this.Page.Request.QueryString["username"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
				{
					memberQuery.StartTime = Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataStart"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
				{
					memberQuery.EndTime = Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"]));
				}
				memberQuery.PageIndex = this.pager.PageIndex;
				memberQuery.PageSize = this.pager.PageSize;
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
				this.grdReferralmembers.DataSource = mySubUsers.Data;
				this.grdReferralmembers.DataBind();
				this.txtSearchText.Text = memberQuery.UserName;
				this.calendarStart.SelectedDate = memberQuery.StartTime;
				this.calendarEnd.SelectedDate = memberQuery.EndTime;
				this.pager.TotalRecords = mySubUsers.TotalRecords;
			}
		}

		private void btnSearchButton_Click(object sender, EventArgs e)
		{
			this.ReloadReferralMembers(true);
		}

		private void ReloadReferralMembers(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("username", this.txtSearchText.Text.Trim());
			NameValueCollection nameValueCollection2 = nameValueCollection;
			DateTime? selectedDate = this.calendarStart.SelectedDate;
			nameValueCollection2.Add("dataStart", selectedDate.ToString());
			NameValueCollection nameValueCollection3 = nameValueCollection;
			selectedDate = this.calendarEnd.SelectedDate;
			nameValueCollection3.Add("dataEnd", selectedDate.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
