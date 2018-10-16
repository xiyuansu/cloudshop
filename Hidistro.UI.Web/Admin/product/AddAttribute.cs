using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.product.ascx;
using System;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddAttribute : AdminPage
	{
		protected int typeId;

		protected AttributeView attributeView;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.typeId = this.Page.Request.QueryString["typeId"].ToInt(0);
			if (this.typeId <= 0)
			{
				base.GotoResourceNotFound();
			}
		}
	}
}
