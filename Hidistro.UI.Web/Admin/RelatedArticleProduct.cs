using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class RelatedArticleProduct : AdminPage
	{
		private int? categoryId;

		protected int articId;

		protected string hasRelatedId;

		protected Panel Panel1;

		protected ProductCategoriesDropDownList dropCategories;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (this.articId <= 0)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				List<int> values = ArticleHelper.GetRelatedProductsId(this.articId).ToList();
				this.hasRelatedId = string.Join(",", values);
				if (!this.Page.IsPostBack)
				{
					this.dropCategories.DataBind();
				}
			}
		}

		private void LoadParameters()
		{
			int.TryParse(base.Request.QueryString["ArticleId"], out this.articId);
			if (!string.IsNullOrEmpty(base.Request.QueryString["CategoryId"]))
			{
				int value = 0;
				if (int.TryParse(base.Request.QueryString["CategoryId"], out value))
				{
					this.categoryId = value;
				}
			}
		}
	}
}
