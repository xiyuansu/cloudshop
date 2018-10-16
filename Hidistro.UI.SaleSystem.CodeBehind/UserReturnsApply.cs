using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserReturnsApply : MemberTemplatedWebControl
	{
		private TextBox txtOrderId;

		private TextBox txtAfterSaleId;

		private Button imgbtnSearch;

		private Common_OrderManage_ReturnsApply listReturns;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReturnsApply.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtOrderId = (TextBox)this.FindControl("txtOrderId");
			this.imgbtnSearch = (Button)this.FindControl("imgbtnSearch");
			this.txtAfterSaleId = (TextBox)this.FindControl("txtAfterSaleId");
			this.listReturns = (Common_OrderManage_ReturnsApply)this.FindControl("Common_OrderManage_ReturnsApply");
			this.pager = (Pager)this.FindControl("pager");
			this.imgbtnSearch.Click += this.imgbtnSearch_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindReturns();
			}
		}

		private void imgbtnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadReturns(true);
		}

		private void BindReturns()
		{
			ReturnsApplyQuery returnsQuery = this.GetReturnsQuery();
			returnsQuery.UserId = HiContext.Current.UserId;
			PageModel<ReturnInfo> returnsApplys = TradeHelper.GetReturnsApplys(returnsQuery);
			this.listReturns.DataSource = returnsApplys.Models;
			this.listReturns.DataBind();
			this.pager.TotalRecords = returnsApplys.Total;
			this.txtOrderId.Text = (string.IsNullOrEmpty(returnsQuery.OrderId) ? returnsQuery.ProductName : returnsQuery.OrderId);
			if (returnsQuery.ReturnId.HasValue)
			{
				this.txtAfterSaleId.Text = returnsQuery.ReturnId.Value.ToString();
			}
		}

		private ReturnsApplyQuery GetReturnsQuery()
		{
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(Globals.StripAllTags(this.Page.Request.QueryString["OrderId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					returnsApplyQuery.HandleStatus = num;
				}
			}
			if (this.Page.Request.QueryString["AfterSaleId"].ToInt(0) > 0)
			{
				returnsApplyQuery.ReturnId = this.Page.Request.QueryString["AfterSaleId"].ToInt(0);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				returnsApplyQuery.ProductName = Globals.UrlDecode(Globals.StripAllTags(this.Page.Request.QueryString["productName"]));
			}
			returnsApplyQuery.PageIndex = this.pager.PageIndex;
			returnsApplyQuery.PageSize = this.pager.PageSize;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			return returnsApplyQuery;
		}

		private void ReloadReturns(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			string text = Globals.StripAllTags(this.txtOrderId.Text.Trim());
			if (TradeHelper.IsOrderId(text))
			{
				nameValueCollection.Add("orderId", text);
			}
			else
			{
				nameValueCollection.Add("productName", Globals.UrlEncode(text));
			}
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
			if (this.txtAfterSaleId.Text.ToInt(0) > 0)
			{
				NameValueCollection nameValueCollection4 = nameValueCollection;
				num = this.txtAfterSaleId.Text.ToInt(0);
				nameValueCollection4.Add("AfterSaleId", num.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
