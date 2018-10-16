using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppRequestBalanceDrawRecords : AppshopMemberTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptDrawRecodes;

		private HtmlInputHidden txtTotalPages;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-RequestBalanceRecords.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
			this.rptDrawRecodes = (AppshopTemplatedRepeater)this.FindControl("rptDrawRecodes");
			PageTitle.AddSiteNameTitle("提现记录");
			this.BindDrawRecords();
		}

		private void BindDrawRecords()
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
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.UserId = HiContext.Current.User.UserId;
			balanceDrawRequestQuery.PageIndex = pageIndex;
			balanceDrawRequestQuery.PageSize = pageSize;
			DbQueryResult balanceDrawRequests = MemberHelper.GetBalanceDrawRequests(balanceDrawRequestQuery, false);
			this.rptDrawRecodes.DataSource = balanceDrawRequests.Data;
			this.rptDrawRecodes.DataBind();
			int totalRecords = balanceDrawRequests.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
	}
}
