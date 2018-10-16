using Hidistro.Context;
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
	public class WAPSplittinDrawRecords : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptDrawRecodes;

		private HtmlInputHidden txtTotalPages;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDrawRecords.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
			this.rptDrawRecodes = (WapTemplatedRepeater)this.FindControl("rptDrawRecodes");
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
			balanceDrawRequestQuery.PageIndex = pageIndex;
			balanceDrawRequestQuery.PageSize = pageSize;
			balanceDrawRequestQuery.UserId = HiContext.Current.UserId;
			DbQueryResult mySplittinDraws = MemberProcessor.GetMySplittinDraws(balanceDrawRequestQuery, null);
			this.rptDrawRecodes.DataSource = mySplittinDraws.Data;
			this.rptDrawRecodes.DataBind();
			int totalRecords = mySplittinDraws.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}
	}
}
