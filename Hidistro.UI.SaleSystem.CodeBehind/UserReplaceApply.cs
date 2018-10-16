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
	public class UserReplaceApply : MemberTemplatedWebControl
	{
		private TextBox txtOrderId;

		private Button imgbtnSearch;

		private Common_OrderManage_ReplaceApply listReplace;

		private ReplaceStatusDropDownList ddlHandleStatus;

		private ExpressDropDownList expressDropDownList1;

		private TextBox txtShipOrderNumber;

		private TextBox txtAfterSaleId;

		private HiddenField hidExpressCompanyName;

		private HiddenField hidShipOrderNumber;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReplaceApply.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtOrderId = (TextBox)this.FindControl("txtOrderId");
			this.imgbtnSearch = (Button)this.FindControl("imgbtnSearch");
			this.listReplace = (Common_OrderManage_ReplaceApply)this.FindControl("Common_OrderManage_ReplaceApply");
			this.ddlHandleStatus = (ReplaceStatusDropDownList)this.FindControl("ddlHandleStatus");
			this.pager = (Pager)this.FindControl("pager");
			this.expressDropDownList1 = (ExpressDropDownList)this.FindControl("expressDropDownList1");
			this.txtShipOrderNumber = (TextBox)this.FindControl("txtShipOrderNumber");
			this.txtAfterSaleId = (TextBox)this.FindControl("txtAfterSaleId");
			this.hidExpressCompanyName = (HiddenField)this.FindControl("hidExpressCompanyName");
			this.hidShipOrderNumber = (HiddenField)this.FindControl("hidShipOrderNumber");
			this.imgbtnSearch.Click += this.imgbtnSearch_Click;
			if (!this.Page.IsPostBack)
			{
				if (this.expressDropDownList1 != null)
				{
					this.expressDropDownList1.DataBind();
				}
				this.BindRefund();
			}
		}

		private void listReplace_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				RefundStatusLable refundStatusLable = (RefundStatusLable)e.Item.FindControl("lblHandleStatus");
				if (refundStatusLable.Status == 0)
				{
					htmlAnchor.Visible = true;
				}
				else
				{
					htmlAnchor.Visible = false;
				}
			}
		}

		private void imgbtnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadReplace(true);
		}

		private void BindRefund()
		{
			ReplaceApplyQuery replaceQuery = this.GetReplaceQuery();
			replaceQuery.UserId = HiContext.Current.UserId;
			PageModel<ReplaceInfo> replaceApplys = TradeHelper.GetReplaceApplys(replaceQuery);
			this.listReplace.DataSource = replaceApplys.Models;
			this.listReplace.DataBind();
			this.pager.TotalRecords = replaceApplys.Total;
			this.txtOrderId.Text = (string.IsNullOrEmpty(replaceQuery.OrderId) ? replaceQuery.ProductName : replaceQuery.OrderId);
			if (replaceQuery.ReplaceId.HasValue)
			{
				this.txtAfterSaleId.Text = replaceQuery.ReplaceId.Value.ToString();
			}
			this.ddlHandleStatus.SelectedIndex = 0;
			if (replaceQuery.HandleStatus.HasValue && replaceQuery.HandleStatus.Value > -1)
			{
				this.ddlHandleStatus.SelectedValue = replaceQuery.HandleStatus.Value;
			}
		}

		private ReplaceApplyQuery GetReplaceQuery()
		{
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				replaceApplyQuery.OrderId = Globals.UrlDecode(Globals.StripAllTags(this.Page.Request.QueryString["OrderId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out num) && num > -1)
				{
					replaceApplyQuery.HandleStatus = num;
				}
			}
			if (this.Page.Request.QueryString["AfterSaleId"].ToInt(0) > 0)
			{
				replaceApplyQuery.ReplaceId = this.Page.Request.QueryString["AfterSaleId"].ToInt(0);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				replaceApplyQuery.ProductName = Globals.UrlDecode(Globals.StripAllTags(this.Page.Request.QueryString["productName"]));
			}
			replaceApplyQuery.PageIndex = this.pager.PageIndex;
			replaceApplyQuery.PageSize = this.pager.PageSize;
			replaceApplyQuery.SortBy = "ApplyForTime";
			replaceApplyQuery.SortOrder = SortAction.Desc;
			return replaceApplyQuery;
		}

		private void ReloadReplace(bool isSearch)
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
			if (this.ddlHandleStatus.SelectedValue.HasValue)
			{
				NameValueCollection nameValueCollection4 = nameValueCollection;
				num = this.ddlHandleStatus.SelectedValue.Value;
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
