using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPGroupBuyList : WAPTemplatedWebControl
	{
		private int categoryId;

		private string keyWord;

		private HiImage imgUrl;

		private Literal litContent;

		private WapTemplatedRepeater rptProducts;

		private WapTemplatedRepeater rptCategories;

		private HtmlInputHidden txtTotal;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VGroupBuyList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
			this.keyWord = this.Page.Request.QueryString["keyWord"];
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (Literal)this.FindControl("litContent");
			this.rptProducts = (WapTemplatedRepeater)this.FindControl("rptGroupBuyProducts");
			this.txtTotal = (HtmlInputHidden)this.FindControl("txtTotal");
			this.rptCategories = (WapTemplatedRepeater)this.FindControl("rptCategories");
			if (this.rptCategories != null)
			{
				IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(this.categoryId);
				this.rptCategories.DataSource = subCategories;
				this.rptCategories.DataBind();
			}
			int page = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["page"], out page))
			{
				page = 1;
			}
			int size = default(int);
			if (!int.TryParse(this.Page.Request.QueryString["size"], out size))
			{
				size = 10;
			}
			int num = default(int);
			this.rptProducts.DataSource = ProductBrowser.GetGroupBuyProducts(this.categoryId, this.keyWord, page, size, out num, true);
			this.rptProducts.DataBind();
			this.txtTotal.SetWhenIsNotNull(num.ToString());
			PageTitle.AddSiteNameTitle("团购搜索页");
		}
	}
}
