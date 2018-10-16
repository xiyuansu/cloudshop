using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_GoodsList_Browsed : AscxTemplatedWebControl
	{
		public const string TagID = "rpt_Common_GoodsList_Browsed";

		private Repeater rptBrowsedProduct;

		private int maxNum = 6;

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		public int MaxNum
		{
			get
			{
				return this.maxNum;
			}
			set
			{
				this.maxNum = value;
			}
		}

		public Common_GoodsList_Browsed()
		{
			base.ID = "rpt_Common_GoodsList_Browsed";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_GoodsList/Skin-Common_GoodsList_Browsed.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptBrowsedProduct = (Repeater)this.FindControl("rptBrowsedProduct");
			this.BindList();
		}

		private void BindList()
		{
			IList<int> browedProductList = BrowsedProductQueue.GetBrowedProductList(this.MaxNum);
			this.rptBrowsedProduct.DataSource = ProductBrowser.GetVistiedProducts(browedProductList);
			this.rptBrowsedProduct.DataBind();
		}
	}
}
