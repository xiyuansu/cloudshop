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
	public class WapSplittinDetails : WAPMemberTemplatedWebControl
	{
		private FormatedMoneyLabel litSplittinDraws;

		private FormatedMoneyLabel litAllSplittin;

		private WapTemplatedRepeater rptSplittin;

		private HtmlInputHidden txtTotalPages;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SplittinDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
			this.litSplittinDraws = (FormatedMoneyLabel)this.FindControl("litSplittinDraws");
			this.litAllSplittin = (FormatedMoneyLabel)this.FindControl("litAllSplittin");
			this.rptSplittin = (WapTemplatedRepeater)this.FindControl("rptSplittin");
			PageTitle.AddSiteNameTitle("奖励明细");
			int userId = HiContext.Current.UserId;
			this.litAllSplittin.Money = MemberProcessor.GetUserAllSplittin(userId);
			this.litSplittinDraws.Money = MemberProcessor.GetUserUseSplittin(userId);
			this.BindSplittins();
		}

		private void BindSplittins()
		{
			BalanceDetailQuery query = this.GetQuery();
			bool value = false;
			bool? isUse;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["usestatus"]))
			{
				bool.TryParse(this.Page.Request.QueryString["usestatus"], out value);
				isUse = value;
			}
			else
			{
				isUse = null;
			}
			DbQueryResult mySplittinDetails = MemberProcessor.GetMySplittinDetails(query, isUse);
			this.rptSplittin.DataSource = mySplittinDetails.Data;
			this.rptSplittin.DataBind();
			int totalRecords = mySplittinDetails.TotalRecords;
			this.txtTotalPages.SetWhenIsNotNull(totalRecords.ToString());
		}

		private BalanceDetailQuery GetQuery()
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
			BalanceDetailQuery balanceDetailQuery = new BalanceDetailQuery();
			balanceDetailQuery.UserId = HiContext.Current.UserId;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
			{
				balanceDetailQuery.FromDate = Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataStart"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
			{
				balanceDetailQuery.ToDate = Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["splittingtype"]))
			{
				balanceDetailQuery.SplittingTypes = (SplittingTypes)Convert.ToInt32(this.Page.Server.UrlDecode(this.Page.Request.QueryString["splittingtype"]));
			}
			balanceDetailQuery.PageIndex = pageIndex;
			balanceDetailQuery.PageSize = pageSize;
			return balanceDetailQuery;
		}
	}
}
