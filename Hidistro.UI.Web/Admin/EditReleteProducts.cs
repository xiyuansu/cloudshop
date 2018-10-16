using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditReleteProducts : AdminPage
	{
		protected string keywords;

		protected int? categoryId;

		protected int productId;

		protected string hasRelatedId;

		protected Panel Panel1;

		protected ProductCategoriesDropDownList dropCategories;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productId = base.Request.QueryString["ProductId"].ToInt(0);
			if (this.productId <= 0)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				List<int> list = ProductHelper.GetRelatedProductsId(this.productId).ToList();
				list.Add(this.productId);
				this.hasRelatedId = string.Join(",", list);
				if (!this.Page.IsPostBack)
				{
					this.LoadParameters();
					this.dropCategories.DataBind();
				}
			}
		}

		private void LoadParameters()
		{
			int.TryParse(base.Request.QueryString["productId"], out this.productId);
			if (!string.IsNullOrEmpty(base.Request.QueryString["Keywords"]))
			{
				this.keywords = base.Request.QueryString["Keywords"];
			}
			if (!string.IsNullOrEmpty(base.Request.QueryString["CategoryId"]))
			{
				int value = 0;
				if (int.TryParse(base.Request.QueryString["CategoryId"], out value))
				{
					this.categoryId = value;
				}
			}
			this.dropCategories.SelectedValue = this.categoryId;
		}
	}
}
