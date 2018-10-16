using Hidistro.Context;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPProductReview : WAPMemberTemplatedWebControl
	{
		private int productId;

		private HtmlInputHidden txtTotal;

		private HtmlInputHidden hidden_productId;

		private string orderId;

		private string skuId;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductReview.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("错误的商品信息");
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidden_productId = (HtmlInputHidden)this.FindControl("hidden_productId");
			ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
			if (productSimpleInfo == null)
			{
				base.GotoResourceNotFound("该件商品已经被管理员删除");
			}
			this.hidden_productId.Value = this.Page.Request.QueryString["productId"];
			PageTitle.AddSiteNameTitle("商品评价");
		}
	}
}
