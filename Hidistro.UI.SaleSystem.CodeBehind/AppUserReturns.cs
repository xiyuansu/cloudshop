using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppUserReturns : AppshopMemberTemplatedWebControl
	{
		private AppshopTemplatedRepeater rptUserRefunds;

		private HtmlInputHidden txtTotalPages;

		private int Status = -1;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-UserReturns.html";
			}
			base.OnInit(e);
		}

		protected void rptUserRefunds_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				HtmlInputHidden htmlInputHidden = new HtmlInputHidden();
				htmlInputHidden.Name = "hfHandleStatus";
				htmlInputHidden.ID = "hfHandleStatus";
				htmlInputHidden.Value = DataBinder.Eval(e.Item.DataItem, "HandleStatus").ToString();
				e.Item.Controls.Add(htmlInputHidden);
			}
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("售后记录");
			int.TryParse(this.Page.Request.QueryString["Status"], out this.Status);
			this.rptUserRefunds = (AppshopTemplatedRepeater)this.FindControl("rptUserReturns");
			this.rptUserRefunds.ItemDataBound += this.rptUserRefunds_ItemDataBound;
			this.txtTotalPages = (HtmlInputHidden)this.FindControl("txtTotal");
			int pageIndex = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			ReturnsApplyQuery returnsApplyQuery = new ReturnsApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				returnsApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (int.TryParse(this.Page.Request.QueryString["Status"], out this.Status) && this.Status > -1)
			{
				returnsApplyQuery.HandleStatus = this.Status;
			}
			returnsApplyQuery.PageIndex = pageIndex;
			returnsApplyQuery.PageSize = pageSize;
			returnsApplyQuery.SortBy = "ApplyForTime";
			returnsApplyQuery.SortOrder = SortAction.Desc;
			returnsApplyQuery.UserId = HiContext.Current.UserId;
			int num = 0;
			PageModel<ReturnInfo> returnsApplys = TradeHelper.GetReturnsApplys(returnsApplyQuery);
			this.rptUserRefunds.DataSource = returnsApplys.Models;
			this.rptUserRefunds.DataBind();
			num = returnsApplys.Total;
			this.txtTotalPages.SetWhenIsNotNull(num.ToString());
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
