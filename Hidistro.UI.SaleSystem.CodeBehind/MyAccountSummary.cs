using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class MyAccountSummary : MemberTemplatedWebControl
	{
		private Common_Advance_AccountList accountList;

		private Pager pager;

		private CalendarPanel calendarStart;

		private CalendarPanel calendarEnd;

		private TradeTypeDropDownList dropTradeType;

		private Button imgbtnSearch;

		private FormatedMoneyLabel litAccountAmount;

		private FormatedMoneyLabel litRequestBalance;

		private FormatedMoneyLabel litUseableBalance;

		private HtmlGenericControl spaccountamount;

		private HtmlGenericControl sprequestbalace;

		private HtmlAnchor link_balancedraw;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-MyAccountSummary.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.accountList = (Common_Advance_AccountList)this.FindControl("Common_Advance_AccountList");
			this.pager = (Pager)this.FindControl("pager");
			this.calendarStart = (CalendarPanel)this.FindControl("calendarStart");
			this.calendarEnd = (CalendarPanel)this.FindControl("calendarEnd");
			this.dropTradeType = (TradeTypeDropDownList)this.FindControl("dropTradeType");
			this.imgbtnSearch = (Button)this.FindControl("imgbtnSearch");
			this.litAccountAmount = (FormatedMoneyLabel)this.FindControl("litAccountAmount");
			this.litRequestBalance = (FormatedMoneyLabel)this.FindControl("litRequestBalance");
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			this.spaccountamount = (HtmlGenericControl)this.FindControl("spaccountamount");
			this.sprequestbalace = (HtmlGenericControl)this.FindControl("sprequestbalace");
			this.link_balancedraw = (HtmlAnchor)this.FindControl("link_balancedraw");
			this.imgbtnSearch.Click += this.imgbtnSearch_Click;
			PageTitle.AddSiteNameTitle("预付款账户");
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (string.IsNullOrWhiteSpace(user.TradePassword))
				{
					this.Page.Response.Redirect($"/user/OpenBalance.aspx?ReturnUrl={HttpContext.Current.Request.Url}");
				}
				this.BindBalanceDetails();
				this.litAccountAmount.Money = user.Balance;
				this.litRequestBalance.Money = user.RequestBalance;
				this.litUseableBalance.Money = user.Balance - user.RequestBalance;
				HtmlGenericControl htmlGenericControl = this.spaccountamount;
				HtmlGenericControl htmlGenericControl2 = this.sprequestbalace;
				HtmlAnchor htmlAnchor = this.link_balancedraw;
				bool flag = htmlAnchor.Visible = HiContext.Current.SiteSettings.EnableBulkPaymentAdvance;
				bool visible = htmlGenericControl2.Visible = flag;
				htmlGenericControl.Visible = visible;
			}
		}

		private void imgbtnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadMyBalanceDetails(true);
		}

		private void BindBalanceDetails()
		{
			BalanceDetailQuery balanceDetailQuery = this.GetBalanceDetailQuery();
			DbQueryResult balanceDetails = MemberProcessor.GetBalanceDetails(balanceDetailQuery);
			this.accountList.DataSource = balanceDetails.Data;
			this.accountList.DataBind();
			this.dropTradeType.DataBind();
			this.dropTradeType.SelectedValue = balanceDetailQuery.TradeType;
			this.calendarStart.SelectedDate = balanceDetailQuery.FromDate;
			this.calendarEnd.SelectedDate = balanceDetailQuery.ToDate;
			this.pager.TotalRecords = balanceDetails.TotalRecords;
		}

		private BalanceDetailQuery GetBalanceDetailQuery()
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
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tradeType"]))
			{
				balanceDetailQuery.TradeType = (TradeTypes)Convert.ToInt32(this.Page.Server.UrlDecode(this.Page.Request.QueryString["tradeType"]));
			}
			balanceDetailQuery.PageIndex = this.pager.PageIndex;
			balanceDetailQuery.PageSize = this.pager.PageSize;
			return balanceDetailQuery;
		}

		private void ReloadMyBalanceDetails(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			int num;
			if (!isSearch)
			{
				NameValueCollection nameValueCollection2 = nameValueCollection;
				num = this.pager.PageIndex;
				nameValueCollection2.Add("pageIndex", num.ToString());
			}
			NameValueCollection nameValueCollection3 = nameValueCollection;
			DateTime? selectedDate = this.calendarStart.SelectedDate;
			nameValueCollection3.Add("dataStart", selectedDate.ToString());
			NameValueCollection nameValueCollection4 = nameValueCollection;
			selectedDate = this.calendarEnd.SelectedDate;
			nameValueCollection4.Add("dataEnd", selectedDate.ToString());
			NameValueCollection nameValueCollection5 = nameValueCollection;
			num = (int)this.dropTradeType.SelectedValue;
			nameValueCollection5.Add("tradeType", num.ToString());
			base.ReloadPage(nameValueCollection);
		}
	}
}
