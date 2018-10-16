using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserRefundApply : MemberTemplatedWebControl
	{
		private HtmlInputText txtOrderId;

		private Button imgbtnSearch;

		private Common_OrderManage_RefundApply RefundList;

		private RefundStatusDropDownList handleStatus;

		private TextBox txtAfterSaleId;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserRefundApply.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtOrderId = (HtmlInputText)this.FindControl("txtOrderId");
			this.txtAfterSaleId = (TextBox)this.FindControl("txtAfterSaleId");
			this.imgbtnSearch = (Button)this.FindControl("imgbtnSearch");
			this.RefundList = (Common_OrderManage_RefundApply)this.FindControl("Common_OrderManage_RefundApply");
			this.handleStatus = (RefundStatusDropDownList)this.FindControl("ddlHandleStatus");
			this.pager = (Pager)this.FindControl("pager");
			this.imgbtnSearch.Click += this.imgbtnSearch_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindRefund();
			}
		}

		private void imgbtnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadRefunds(true);
		}

		private void BindRefund()
		{
			RefundApplyQuery refundQuery = this.GetRefundQuery();
			refundQuery.UserId = HiContext.Current.UserId;
			PageModel<RefundModel> refundApplys = TradeHelper.GetRefundApplys(refundQuery);
			this.RefundList.DataSource = refundApplys.Models;
			this.RefundList.DataBind();
			this.pager.TotalRecords = refundApplys.Total;
			this.txtOrderId.Value = refundQuery.OrderId;
			if (refundQuery.RefundId.HasValue)
			{
				this.txtAfterSaleId.Text = refundQuery.RefundId.Value.ToString();
			}
			if (refundQuery.HandleStatus.HasValue && refundQuery.HandleStatus.Value > -1)
			{
				this.handleStatus.SelectedValue = refundQuery.HandleStatus.Value;
			}
		}

		private RefundApplyQuery GetRefundQuery()
		{
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			int num = this.Page.Request.QueryString["afterSaleId"].ToInt(0);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				refundApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num2 = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num2) && num2 > -1)
				{
					refundApplyQuery.HandleStatus = num2;
				}
			}
			if (num > 0)
			{
				refundApplyQuery.RefundId = num;
			}
			refundApplyQuery.PageIndex = this.pager.PageIndex;
			refundApplyQuery.PageSize = this.pager.PageSize;
			refundApplyQuery.SortBy = "ApplyForTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			return refundApplyQuery;
		}

		private void ReloadRefunds(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Value);
			NameValueCollection nameValueCollection2 = nameValueCollection;
			int num = this.pager.PageSize;
			nameValueCollection2.Add("PageSize", num.ToString());
			if (!isSearch)
			{
				NameValueCollection nameValueCollection3 = nameValueCollection;
				num = this.pager.PageIndex;
				nameValueCollection3.Add("pageIndex", num.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			if (this.handleStatus.SelectedValue.HasValue)
			{
				NameValueCollection nameValueCollection4 = nameValueCollection;
				num = this.handleStatus.SelectedValue.Value;
				nameValueCollection4.Add("HandleStatus", num.ToString());
			}
			if (this.txtAfterSaleId.Text.ToInt(0) > 0)
			{
				NameValueCollection nameValueCollection5 = nameValueCollection;
				num = this.txtAfterSaleId.Text.ToInt(0);
				nameValueCollection5.Add("AfterSaleId", num.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
