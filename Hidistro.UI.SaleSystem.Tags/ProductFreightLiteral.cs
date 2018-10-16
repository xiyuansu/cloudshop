using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Shopping;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class ProductFreightLiteral : Literal
	{
		public int ShippingTemplateId
		{
			get;
			set;
		}

		public decimal Volume
		{
			get;
			set;
		}

		public decimal Weight
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.ShippingTemplateId > 0)
			{
				int deliveryScopRegionId = HiContext.Current.DeliveryScopRegionId;
				base.Text = ShoppingProcessor.CalcProductFreight(deliveryScopRegionId, this.ShippingTemplateId, this.Volume, this.Weight, 1, decimal.Zero).F2ToString("f2");
			}
			else
			{
				base.Text = "0";
			}
			base.Render(writer);
		}
	}
}
