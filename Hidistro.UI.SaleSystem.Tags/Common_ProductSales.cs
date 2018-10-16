using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ProductSales : AscxTemplatedWebControl
	{
		private Repeater rp_productsales;

		public int maxNum = 6;

		public int ProductId
		{
			get;
			set;
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

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_ViewProduct/Skin-Common_ProductSales.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rp_productsales = (Repeater)this.FindControl("rp_productsales");
			if (RouteConfig.GetParameter(this.Page, "productId", false).ToInt(0) > 0)
			{
				this.ProductId = RouteConfig.GetParameter(this.Page, "productId", false).ToInt(0);
			}
			DataTable lineItems = ProductBrowser.GetLineItems(this.ProductId, this.maxNum);
			foreach (DataRow row in lineItems.Rows)
			{
				string text = (string)row["Username"];
				if (text.ToLower() == "anonymous")
				{
					row["Username"] = "匿名用户";
				}
				else
				{
					row["Username"] = text.Substring(0, 1) + "**" + text.Substring(text.Length - 1);
				}
			}
			this.rp_productsales.DataSource = lineItems;
			this.rp_productsales.DataBind();
		}
	}
}
