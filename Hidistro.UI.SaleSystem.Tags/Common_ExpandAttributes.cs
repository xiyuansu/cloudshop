using Hidistro.SaleSystem.Catalog;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ExpandAttributes : WebControl
	{
		public int ProductId
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DataTable expandAttributes = ProductBrowser.GetExpandAttributes(this.ProductId);
			StringBuilder stringBuilder = new StringBuilder();
			if (expandAttributes != null && expandAttributes.Rows.Count > 0)
			{
				stringBuilder.Append("<table class=\"ext-attr\">");
				foreach (DataRow row in expandAttributes.Rows)
				{
					stringBuilder.AppendFormat("<tr><th>{0}:</th><td>{1}</td></tr>", row["AttributeName"], row["ValueStr"]);
				}
				stringBuilder.Append("</table>");
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
