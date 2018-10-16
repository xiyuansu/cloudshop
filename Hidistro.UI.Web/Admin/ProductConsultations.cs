using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductConsultationsManage)]
	public class ProductConsultations : AdminPage
	{
		protected ProductCategoriesDropDownList dropCategories;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.dropCategories.DataBind();
		}
	}
}
