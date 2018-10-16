using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class CountDownProducts : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProduct;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-CountDownProducts.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
			this.rptProduct.ItemDataBound += this.rptProduct_ItemDataBound;
			this.pager = (Pager)this.FindControl("pager");
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}

		protected void rptProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DataRowView dataRowView = (DataRowView)e.Item.DataItem;
				HtmlContainerControl htmlContainerControl = e.Item.Controls[0].FindControl("countDownBackGroundImage") as HtmlContainerControl;
				ProductDetailsLink productDetailsLink = e.Item.Controls[0].FindControl("ProductDetailsLink3") as ProductDetailsLink;
				Image image = e.Item.Controls[0].FindControl("imageOver") as Image;
				image.Visible = false;
				DateTime? nullable = dataRowView["StartDate"].ToDateTime();
				DateTime? nullable2 = dataRowView["EndDate"].ToDateTime();
				int num = dataRowView["TotalCount"].ToInt(0);
				int num2 = dataRowView["BoughtCount"].ToInt(0);
				if (htmlContainerControl != null && productDetailsLink != null)
				{
					string text = "url(/Templates/pccommon/images/sub/buy/countdownpcover.png) no-repeat";
					string text2 = "url(/Templates/pccommon/images/sub/buy/countdownpcwill.png) no-repeat";
					string imageUrl = "/Templates/pccommon/images/sub/buy/countdownovertip.png";
					if (nullable.Value >= DateTime.Now)
					{
						htmlContainerControl.Style.Add("background", "#66bb6a");
						productDetailsLink.Text = "即将开始";
					}
					else if (nullable.Value <= DateTime.Now && nullable2.Value >= DateTime.Now && num <= num2)
					{
						htmlContainerControl.Style.Add("background", "#b9b9b9");
						productDetailsLink.Text = "已抢完";
						image.ImageUrl = imageUrl;
						image.Visible = true;
					}
				}
			}
		}

		private void BindProduct()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult countDownProductList = PromoteHelper.GetCountDownProductList(productBrowseQuery);
			this.rptProduct.DataSource = countDownProductList.Data;
			this.rptProduct.DataBind();
			this.pager.TotalRecords = countDownProductList.TotalRecords;
		}

		private ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			productBrowseQuery.IsCount = true;
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			productBrowseQuery.SortBy = "DisplaySequence";
			productBrowseQuery.SortOrder = SortAction.Desc;
			productBrowseQuery.StoreId = 0;
			return productBrowseQuery;
		}
	}
}
