using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SubMembers : MemberTemplatedWebControl
	{
		private TextBox txtSearchText;

		private TextBox txtRealName;

		private TextBox txtCellPhone;

		private Button btnSearchButton;

		private Common_Referral_MemberList grdReferralmembers;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SubMembers.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!HiContext.Current.User.IsReferral())
			{
				this.Page.Response.Redirect("/User/ReferralRegisterAgreement", true);
			}
			this.txtSearchText = (TextBox)this.FindControl("txtSearchText");
			this.txtRealName = (TextBox)this.FindControl("txtRealName");
			this.txtCellPhone = (TextBox)this.FindControl("txtCellPhone");
			this.btnSearchButton = (Button)this.FindControl("btnSearchButton");
			this.grdReferralmembers = (Common_Referral_MemberList)this.FindControl("Common_Referral_MemberList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnSearchButton.Click += this.btnSearchButton_Click;
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("下级会员");
				MemberQuery memberQuery = new MemberQuery();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["username"]))
				{
					memberQuery.UserName = this.Page.Server.UrlDecode(this.Page.Request.QueryString["username"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realname"]))
				{
					memberQuery.RealName = this.Page.Server.UrlDecode(this.Page.Request.QueryString["realname"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["cellPhone"]))
				{
					memberQuery.CellPhone = this.Page.Server.UrlDecode(this.Page.Request.QueryString["cellPhone"]);
				}
				memberQuery.PageIndex = this.pager.PageIndex;
				memberQuery.PageSize = this.pager.PageSize;
				DbQueryResult mySubUsers = MemberProcessor.GetMySubUsers(memberQuery);
				this.grdReferralmembers.DataSource = mySubUsers.Data;
				this.grdReferralmembers.DataBind();
				this.txtSearchText.Text = memberQuery.UserName;
				this.txtRealName.Text = memberQuery.RealName;
				this.txtCellPhone.Text = memberQuery.CellPhone;
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
			nameValueCollection.Add("realname", this.txtRealName.Text.Trim());
			nameValueCollection.Add("cellPhone", this.txtCellPhone.Text.Trim());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
