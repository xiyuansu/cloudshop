using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.UpdateMemberPoint)]
	public class MemberPointList : AdminPage
	{
		private int? rankId;

		private string searchKey;

		protected MemberGradeDropDownList rankList;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GradeId"]))
				{
					this.rankId = this.Page.Request.QueryString["GradeId"].ToInt(0);
				}
				this.rankList.DataBind();
				this.rankList.SelectedValue = this.rankId;
			}
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["rankId"], out value))
				{
					this.rankId = value;
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				this.rankList.SelectedValue = this.rankId;
			}
			else
			{
				this.rankId = this.rankList.SelectedValue;
			}
		}
	}
}
