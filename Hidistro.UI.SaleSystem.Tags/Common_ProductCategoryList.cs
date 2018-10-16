using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ProductCategoryList : ThemedTemplatedRepeater
	{
		private int categoryId;

		public bool IsShowSubCategory
		{
			get;
			set;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.ItemDataBound += this.Common_ProductCategoryList_ItemDataBound;
			if (this.IsShowSubCategory)
			{
				this.categoryId = RouteConfig.GetParameter(this.Page, "categoryId", false).ToInt(0);
			}
			this.BindList();
		}

		private void Common_ProductCategoryList_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			int parentCategoryId = ((CategoryInfo)e.Item.DataItem).CategoryId;
			Repeater repeater = (Repeater)e.Item.Controls[0].FindControl("rptSubCategries");
			if (repeater != null)
			{
				repeater.DataSource = CatalogHelper.GetSubCategories(parentCategoryId);
				repeater.DataBind();
			}
		}

		private void BindList()
		{
			IEnumerable<CategoryInfo> subCategories;
			if (this.categoryId != 0)
			{
				subCategories = CatalogHelper.GetSubCategories(this.categoryId);
				if (subCategories == null)
				{
					CategoryInfo category = CatalogHelper.GetCategory(this.categoryId);
					if (category != null)
					{
						subCategories = CatalogHelper.GetSubCategories(category.ParentCategoryId);
						goto IL_0033;
					}
					return;
				}
				goto IL_0033;
			}
			base.DataSource = CatalogHelper.GetMainCategories();
			base.DataBind();
			return;
			IL_0033:
			base.DataSource = subCategories;
			base.DataBind();
		}
	}
}
