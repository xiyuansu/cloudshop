using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppStoreAbout : AppshopTemplatedWebControl
	{
		private Literal litSupplierName;

		private Literal litSupplierAbout;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-StoreAbout.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("门店介绍");
			this.litSupplierName = (Literal)this.FindControl("litSupplierName");
			this.litSupplierAbout = (Literal)this.FindControl("litSupplierAbout");
			int storeId = HttpContext.Current.Request.QueryString["StoreId"].ToInt(0);
			StoresInfo storeById = DepotHelper.GetStoreById(storeId);
			if (storeById == null)
			{
				base.GotoResourceNotFound("");
			}
			else
			{
				this.litSupplierName.SetWhenIsNotNull(storeById.StoreName);
				this.litSupplierAbout.SetWhenIsNotNull(storeById.Introduce);
			}
		}
	}
}
