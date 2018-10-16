using Hidistro.SaleSystem.Shopping;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SkuContentLabel : Literal
	{
		protected override void Render(HtmlTextWriter writer)
		{
			DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(base.Text);
			string text = string.Empty;
			foreach (DataRow row in productInfoBySku.Rows)
			{
				if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
				{
					text = text + row["AttributeName"] + ":" + row["ValueStr"] + "; ";
				}
			}
			base.Text = text;
			base.Render(writer);
		}
	}
}
