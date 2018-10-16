using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SplittinDetails : MemberTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptReferralSplitin;

		private Pager pager;

		private CalendarPanel calendarStart;

		private CalendarPanel calendarEnd;

		private SplittingTypesDropDownList dropSplittingType;

		private HtmlAnchor imgbtnSearch;

		private FormatedMoneyLabel litAllSplittin;

		private FormatedMoneyLabel litUseSplittin;

		private FormatedMoneyLabel litNoUseSplittin;

		private FormatedMoneyLabel litCanGet;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-SplittinDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!HiContext.Current.User.IsReferral())
			{
				this.Page.Response.Redirect("/User/ReferralRegisterAgreement", true);
			}
			this.rptReferralSplitin = (ThemedTemplatedRepeater)this.FindControl("rptReferralSplitin");
			this.pager = (Pager)this.FindControl("pager");
			this.calendarStart = (CalendarPanel)this.FindControl("calendarStart");
			this.calendarEnd = (CalendarPanel)this.FindControl("calendarEnd");
			this.dropSplittingType = (SplittingTypesDropDownList)this.FindControl("SplittingTypeList");
			this.imgbtnSearch = (HtmlAnchor)this.FindControl("abtnSearch");
			this.litAllSplittin = (FormatedMoneyLabel)this.FindControl("litAllSplittin");
			this.litUseSplittin = (FormatedMoneyLabel)this.FindControl("litUseSplittin");
			this.litNoUseSplittin = (FormatedMoneyLabel)this.FindControl("litNoUseSplittin");
			this.litCanGet = (FormatedMoneyLabel)this.FindControl("litCanGet");
			this.imgbtnSearch.ServerClick += this.imgbtnSearch_Click;
			PageTitle.AddSiteNameTitle("我的奖励");
			if (!this.Page.IsPostBack)
			{
				this.BindSplittins();
				int userId = HiContext.Current.UserId;
				this.litAllSplittin.Money = MemberProcessor.GetUserAllSplittin(userId);
				this.litUseSplittin.Money = MemberProcessor.GetUserUseSplittin(userId);
				this.litNoUseSplittin.Money = MemberProcessor.GetUserNoUseSplittin(userId);
				this.litCanGet.Money = MemberProcessor.GetUserUseSplittin(userId);
			}
		}

		private void imgbtnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadMyBalanceDetails(true);
		}

		private void BindSplittins()
		{
			BalanceDetailQuery query = this.GetQuery();
			this.BindSplittingType();
			DbQueryResult mySplittinDetails = MemberProcessor.GetMySplittinDetails(query, null);
			this.rptReferralSplitin.DataSource = mySplittinDetails.Data;
			this.rptReferralSplitin.DataBind();
			this.calendarStart.SelectedDate = query.FromDate;
			this.calendarEnd.SelectedDate = query.ToDate;
			this.pager.TotalRecords = mySplittinDetails.TotalRecords;
		}

		private void BindSplittingType()
		{
			int value = 0;
			int.TryParse(this.Page.Request.QueryString["splittingtype"], out value);
			this.dropSplittingType.DataBind();
			this.dropSplittingType.SelectedValue = value;
		}

		private BalanceDetailQuery GetQuery()
		{
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
			balanceDetailQuery.PageIndex = this.pager.PageIndex;
			balanceDetailQuery.PageSize = this.pager.PageSize;
			return balanceDetailQuery;
		}

		private void ReloadMyBalanceDetails(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			NameValueCollection nameValueCollection2 = nameValueCollection;
			DateTime? selectedDate = this.calendarStart.SelectedDate;
			nameValueCollection2.Add("dataStart", selectedDate.ToString());
			NameValueCollection nameValueCollection3 = nameValueCollection;
			selectedDate = this.calendarEnd.SelectedDate;
			nameValueCollection3.Add("dataEnd", selectedDate.ToString());
			nameValueCollection.Add("splittingtype", this.dropSplittingType.SelectedValue.ToString());
			base.ReloadPage(nameValueCollection);
		}
	}
}
