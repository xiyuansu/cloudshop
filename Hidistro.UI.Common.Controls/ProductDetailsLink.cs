using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ProductDetailsLink : HyperLink
	{
		public const string TagID = "ProductDetailsLink";

		public bool IsUnSale
		{
			get;
			set;
		}

		public bool ImageLink
		{
			get;
			set;
		}

		public bool IsGroupBuyProduct
		{
			get;
			set;
		}

		public bool IsCountDownProduct
		{
			get;
			set;
		}

		public object ProductName
		{
			get;
			set;
		}

		public object ProductId
		{
			get;
			set;
		}

		public int? StringLenth
		{
			get;
			set;
		}

		public ProductDetailsLink()
		{
			base.ID = "ProductDetailsLink";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.ProductId != null && this.ProductId != DBNull.Value)
			{
				if (this.IsGroupBuyProduct)
				{
					base.NavigateUrl = base.GetRouteUrl("groupBuyProductDetails", new
					{
						this.ProductId
					});
				}
				else if (this.IsCountDownProduct)
				{
					base.NavigateUrl = base.GetRouteUrl("countdownProductsDetails", new
					{
						this.ProductId
					});
				}
				else if (this.IsUnSale)
				{
					base.NavigateUrl = base.GetRouteUrl("unproductdetails", new
					{
						this.ProductId
					});
				}
				else
				{
					base.NavigateUrl = base.GetRouteUrl("productDetails", new
					{
						this.ProductId
					});
				}
			}
			if (!this.ImageLink && this.ProductId != null && this.ProductId != DBNull.Value)
			{
				if (this.StringLenth.HasValue && this.ProductName.ToString().Length > this.StringLenth.Value)
				{
					base.Text = this.ProductName.ToString().Substring(0, this.StringLenth.Value) + "...";
				}
				else
				{
					base.Text = this.ProductName.ToString();
				}
			}
			base.Target = "_blank";
			base.Render(writer);
		}
	}
}
