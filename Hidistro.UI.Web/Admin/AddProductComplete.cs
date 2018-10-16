using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddProducts)]
	public class AddProductComplete : AdminPage
	{
		private int categoryId = 0;

		private int productId = 0;

		private int isEdit = 0;

		protected Literal txtAction;

		protected HtmlGenericControl spanEdit;

		protected HyperLink hlinkProductDetails;

		protected HyperLink hlinkProductEdit;

		protected HtmlGenericControl spanAdd;

		protected HyperLink hlinkAddProduct;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
			}
			else if (!int.TryParse(base.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				int.TryParse(base.Request.QueryString["isEdit"], out this.isEdit);
				if (!this.Page.IsPostBack)
				{
					if (this.isEdit == 1)
					{
						this.txtAction.Text = "编辑";
						this.spanAdd.Visible = false;
						this.hlinkProductDetails.NavigateUrl = base.GetRouteUrl("productDetails", new
						{
							ProductId = this.productId
						});
						this.hlinkProductEdit.NavigateUrl = Globals.GetAdminAbsolutePath($"/product/EditProduct.aspx?productId={this.productId}");
					}
					else
					{
						this.txtAction.Text = "添加";
						this.spanEdit.Visible = false;
						this.hlinkAddProduct.NavigateUrl = Globals.GetAdminAbsolutePath($"/product/AddProduct.aspx?categoryId={this.categoryId}");
					}
				}
			}
		}
	}
}
