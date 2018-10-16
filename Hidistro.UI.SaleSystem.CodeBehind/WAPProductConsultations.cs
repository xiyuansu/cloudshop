using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPProductConsultations : WAPMemberTemplatedWebControl
	{
		private int productId;

		private WapTemplatedRepeater rptProducts;

		private Literal litDescription;

		private HtmlInputHidden txtTotal;

		private Literal litProductTitle;

		private Literal litShortDescription;

		private Literal litSalePrice;

		private Literal litSoldCount;

		private HtmlImage imgProductImage;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductConsultations.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("");
			}
			ProductConsultationAndReplyQuery productConsultationAndReplyQuery = new ProductConsultationAndReplyQuery();
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
			productConsultationAndReplyQuery.ProductId = this.productId;
			productConsultationAndReplyQuery.IsCount = true;
			productConsultationAndReplyQuery.PageIndex = pageIndex;
			productConsultationAndReplyQuery.PageSize = pageSize;
			productConsultationAndReplyQuery.SortBy = "ConsultationId";
			productConsultationAndReplyQuery.SortOrder = SortAction.Desc;
			productConsultationAndReplyQuery.HasReplied = true;
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptProducts");
			this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
			DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(productConsultationAndReplyQuery);
			this.rptProducts.DataSource = productConsultations.Data;
			this.rptProducts.DataBind();
			HtmlInputHidden control = this.txtTotal;
			int num = productConsultations.TotalRecords;
			control.SetWhenIsNotNull(num.ToString());
			this.litProductTitle = (Literal)this.FindControl("litProductTitle");
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.litSoldCount = (Literal)this.FindControl("litSoldCount");
			this.litSalePrice = (Literal)this.FindControl("litSalePrice");
			this.imgProductImage = (HtmlImage)this.FindControl("imgProductImage");
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
			this.litProductTitle.SetWhenIsNotNull(productSimpleInfo.ProductName);
			this.litShortDescription.SetWhenIsNotNull(productSimpleInfo.ShortDescription);
			Literal control2 = this.litSoldCount;
			num = productSimpleInfo.ShowSaleCounts;
			control2.SetWhenIsNotNull(num.ToString());
			this.litSalePrice.SetWhenIsNotNull(productSimpleInfo.MinSalePrice.F2ToString("f2"));
			this.imgProductImage.Src = (string.IsNullOrEmpty(productSimpleInfo.ThumbnailUrl60) ? Globals.FullPath(this.siteSettings.DefaultProductThumbnail4) : productSimpleInfo.ThumbnailUrl60);
			PageTitle.AddSiteNameTitle("商品咨询");
		}
	}
}
