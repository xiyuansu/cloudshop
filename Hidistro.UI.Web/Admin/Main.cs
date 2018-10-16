using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Summary)]
	public class Main : AdminPage
	{
		protected ClassShowOnDataLitl lblTodayOrderAmout;

		protected ClassShowOnDataLitl lblTodayFinishOrder;

		protected ClassShowOnDataLitl ltrTodayAddMemberNumber;

		protected HyperLink ltrWaitSendOrdersNumber;

		protected HtmlGenericControl liUserDraw;

		protected ClassShowOnDataLitl lblMemberBlancedrawRequest;

		protected HtmlGenericControl liMessage;

		protected HyperLink hpkMessages;

		protected HyperLink hpkZiXun;

		protected HyperLink hpkIsWarningStockNum;

		protected HtmlGenericControl liSupplierApply1;

		protected HyperLink hpkSupplierPdAuditNum;

		protected HtmlGenericControl liSupplierApply2;

		protected HyperLink hpkSupplierDrawNum;

		protected HtmlGenericControl liBirthday;

		protected HyperLink hpkBirthdayNum;

		protected ClassShowOnDataLitl lblTotalMembers;

		protected ClassShowOnDataLitl lblTotalProducts;

		protected HtmlGenericControl liUserBalance;

		protected ClassShowOnDataLitl lblMembersBalanceTotal;

		protected ClassShowOnDataLitl lblOrderPriceMonth;

		protected HtmlGenericControl liSupplierProduct;

		protected ClassShowOnDataLitl lblSupplierProductNum;

		protected HtmlGenericControl liSupplierDrawRequest;

		protected ClassShowOnDataLitl lblSupplierDrawTotal;

		protected HtmlGenericControl divUserBalance;

		protected HtmlGenericControl divGroupbuy;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				AdminStatisticsInfo statistics = SalesHelper.GetStatistics(siteSettings.MemberBirthDaySetting);
				this.BindStatistics(statistics);
				if (!siteSettings.OpenPcShop)
				{
					this.liMessage.Visible = false;
				}
				if (!siteSettings.OpenSupplier)
				{
					this.liSupplierDrawRequest.Visible = false;
					this.liSupplierProduct.Visible = false;
					this.liSupplierApply1.Visible = false;
					this.liSupplierApply2.Visible = false;
				}
				if (!siteSettings.OpenPcShop && siteSettings.OpenWap == 0 && siteSettings.OpenVstore == 0 && siteSettings.OpenAliho == 0 && siteSettings.OpenMobbile == 0)
				{
					this.liUserBalance.Visible = false;
					this.liUserDraw.Visible = false;
					this.divGroupbuy.Visible = false;
					this.divUserBalance.Visible = false;
				}
			}
		}

		private void BindStatistics(AdminStatisticsInfo statistics)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (statistics.OrderNumbWaitConsignment > 0)
			{
				this.ltrWaitSendOrdersNumber.NavigateUrl = "javascript:ShowSecondMenuLeft('订 单','sales/manageorder.aspx','/Admin/sales/ManageOrder.aspx?orderStatus=2')";
			}
			HyperLink hyperLink = this.ltrWaitSendOrdersNumber;
			object text;
			int num;
			if (statistics.OrderNumbWaitConsignment <= 0)
			{
				text = "<font style=\"color:#2d2d2d\">0条</font>";
			}
			else
			{
				num = statistics.OrderNumbWaitConsignment;
				text = num.ToString() + "条";
			}
			hyperLink.Text = (string)text;
			HyperLink hyperLink2 = this.hpkZiXun;
			object text2;
			if (statistics.ProductConsultations <= 0)
			{
				text2 = "<font style=\"color:#2d2d2d\">0条</font>";
			}
			else
			{
				num = statistics.ProductConsultations;
				text2 = num.ToString() + "条";
			}
			hyperLink2.Text = (string)text2;
			HyperLink hyperLink3 = this.hpkMessages;
			object text3;
			if (statistics.Messages <= 0)
			{
				text3 = "<font style=\"color:#2d2d2d\">0条</font>";
			}
			else
			{
				num = statistics.Messages;
				text3 = num.ToString() + "条";
			}
			hyperLink3.Text = (string)text3;
			this.hpkZiXun.NavigateUrl = "javascript:ShowSecondMenuLeft('会员','comment/productconsultations.aspx',null)";
			this.hpkMessages.NavigateUrl = "javascript:ShowSecondMenuLeft('会员','comment/receivedmessages.aspx','/Admin/comment/ReceivedMessages.aspx?MessageStatus=3')";
			this.hpkIsWarningStockNum.NavigateUrl = "javascript:ShowSecondMenuLeft('商 品','product/productonsales.aspx','/Admin/product/ProductOnSales.aspx?isWarningStock=1')";
			this.hpkIsWarningStockNum.Text = ProductHelper.GetProductIsWarningStockNum() + "件";
			this.lblTodayOrderAmout.Text = ((statistics.OrderPriceToday > decimal.Zero) ? ("￥" + Globals.FormatMoney(statistics.OrderPriceToday)) : string.Empty);
			ClassShowOnDataLitl classShowOnDataLitl = this.ltrTodayAddMemberNumber;
			object text4;
			if (statistics.UserNewAddToday <= 0)
			{
				text4 = string.Empty;
			}
			else
			{
				num = statistics.UserNewAddToday;
				text4 = num.ToString();
			}
			classShowOnDataLitl.Text = (string)text4;
			this.lblMembersBalanceTotal.Text = "￥" + Globals.FormatMoney(statistics.MembersBalance);
			ClassShowOnDataLitl classShowOnDataLitl2 = this.lblMemberBlancedrawRequest;
			object text5;
			if (statistics.MemberBlancedrawRequest <= 0)
			{
				text5 = string.Empty;
			}
			else
			{
				num = statistics.MemberBlancedrawRequest;
				text5 = "<a href=\"javascript:ShowSecondMenuLeft('会员','member/balancedrawrequest.aspx','member/balancedrawrequest.aspx')\">" + num.ToString() + "条</a>";
			}
			classShowOnDataLitl2.Text = (string)text5;
			ClassShowOnDataLitl classShowOnDataLitl3 = this.lblTodayFinishOrder;
			object text6;
			if (statistics.TodayFinishOrder <= 0)
			{
				text6 = string.Empty;
			}
			else
			{
				num = statistics.TodayFinishOrder;
				text6 = num.ToString();
			}
			classShowOnDataLitl3.Text = (string)text6;
			ClassShowOnDataLitl classShowOnDataLitl4 = this.lblTotalMembers;
			object text7;
			if (statistics.TotalMembers <= 0)
			{
				text7 = string.Empty;
			}
			else
			{
				num = statistics.TotalMembers;
				text7 = num.ToString() + "位";
			}
			classShowOnDataLitl4.Text = (string)text7;
			ClassShowOnDataLitl classShowOnDataLitl5 = this.lblTotalProducts;
			object text8;
			if (statistics.TotalProducts <= 0)
			{
				text8 = string.Empty;
			}
			else
			{
				num = statistics.TotalProducts;
				text8 = num.ToString() + "条";
			}
			classShowOnDataLitl5.Text = (string)text8;
			this.lblOrderPriceMonth.Text = ((statistics.OrderPriceMonth > decimal.Zero) ? ("￥" + statistics.OrderPriceMonth.F2ToString("f2")) : string.Empty);
			this.hpkSupplierPdAuditNum.Text = statistics.SupplierProducts4Audit + "件";
			this.hpkSupplierPdAuditNum.NavigateUrl = "javascript:ShowSecondMenuLeft('供应商','Supplier/Product/AuditProductList.aspx','/Admin/Supplier/Product/AuditProductList.aspx')";
			this.hpkSupplierDrawNum.Text = statistics.SupplierBlancedrawRequestNum + "条";
			this.hpkSupplierDrawNum.NavigateUrl = "javascript:ShowSecondMenuLeft('供应商','Supplier/Balance/DrawRequest.aspx','/Admin/Supplier/Balance/DrawRequest.aspx')";
			this.hpkBirthdayNum.Text = statistics.MemberBirthdayNum + "个";
			this.hpkBirthdayNum.NavigateUrl = "javascript:ShowSecondMenuLeft('会员','member/managemembers.aspx','member/managemembers.aspx?orderBy=orderbrithDay')";
			this.lblSupplierProductNum.Text = statistics.SupplierTotalProducts + "件";
			this.lblSupplierDrawTotal.Text = ((statistics.SupplierBlancedrawTotal > decimal.Zero) ? ("￥" + statistics.SupplierBlancedrawTotal.F2ToString("f2")) : string.Empty);
		}
	}
}
