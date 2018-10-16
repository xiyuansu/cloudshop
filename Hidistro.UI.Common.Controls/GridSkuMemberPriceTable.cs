using Hidistro.Core;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class GridSkuMemberPriceTable : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			string productids = this.Page.Request.QueryString["productIds"];
			string text = this.Page.Request.QueryString["supplierId"];
			productids = ProductHelper.RemoveEffectiveActivityProductId(productids);
			if (!string.IsNullOrEmpty(productids))
			{
				DataTable skuMemberPrices = ProductHelper.GetSkuMemberPrices(productids);
				if (skuMemberPrices != null && skuMemberPrices.Rows.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("<table cellspacing=\"0\" border=\"0\" class=\"table table-striped\">");
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<th >货号</th>");
					stringBuilder.AppendLine("<th >商品</th>");
					stringBuilder.AppendLine("<th >市场价</th>");
					if (!string.IsNullOrEmpty("supplierId"))
					{
						stringBuilder.AppendLine("<th >供货价</th>");
					}
					else
					{
						stringBuilder.AppendLine("<th >成本价</th>");
					}
					stringBuilder.AppendLine("<th >一口价</th>");
					for (int i = 8; i < skuMemberPrices.Columns.Count; i++)
					{
						string columnName = skuMemberPrices.Columns[i].ColumnName;
						columnName = columnName.Substring(columnName.IndexOf("_") + 1) + "价";
						stringBuilder.AppendFormat("<th  style=\"width:100px;\">{0}</th>", columnName).AppendLine();
					}
					stringBuilder.AppendLine("</tr>");
					foreach (DataRow row in skuMemberPrices.Rows)
					{
						this.CreateRow(row, skuMemberPrices, stringBuilder);
					}
					stringBuilder.AppendLine("</table>");
					writer.Write(stringBuilder.ToString());
				}
			}
		}

		private void CreateRow(DataRow row, DataTable dtSkus, StringBuilder sb)
		{
			string text = row["SkuId"].ToString();
			sb.AppendFormat("<tr class=\"SkuPriceRow\" skuId=\"{0}\" >", text).AppendLine();
			sb.AppendFormat("<td>&nbsp;{0}</td>", row["SKU"]).AppendLine();
			sb.AppendFormat("<td>{0} {1}</td>", row["ProductName"], row["SKUContent"]).AppendLine();
			sb.AppendFormat("<td>&nbsp;{0}</td>", (row["MarketPrice"] != DBNull.Value) ? decimal.Parse(row["MarketPrice"].ToString()).F2ToString("f2") : "").AppendLine();
			if (row["SupplierId"] == null || row["SupplierId"].ToString() == "0")
			{
				sb.AppendFormat("<td><input type=\"text\" id=\"tdCostPrice_{0}\" style=\"width:65px\" value=\"{1}\" class=\"forminput form-control\" /></td>", text, (row["CostPrice"] != DBNull.Value) ? decimal.Parse(row["CostPrice"].ToString()).F2ToString("f2") : "").AppendLine();
			}
			else
			{
				sb.AppendFormat("<td>&nbsp;{1}</td><input type=\"text\" id=\"tdCostPrice_{0}\" style=\"width:65px;display:none;\" value=\"{1}\"  />", text, (row["CostPrice"] != DBNull.Value) ? decimal.Parse(row["CostPrice"].ToString()).F2ToString("f2") : "").AppendLine();
			}
			sb.AppendFormat("<td><input type=\"text\" id=\"tdSalePrice_{0}\" style=\"width:65px\" value=\"{1}\" class=\"forminput form-control\" />", text, decimal.Parse(row["SalePrice"].ToString()).F2ToString("f2")).AppendLine();
			for (int i = 8; i < dtSkus.Columns.Count; i++)
			{
				string columnName = dtSkus.Columns[i].ColumnName;
				string[] array = row[columnName].ToString().Split('|');
				string arg = "";
				string text2 = "";
				if (array[0].ToString() != "")
				{
					arg = decimal.Parse(array[0].ToString()).F2ToString("f2");
				}
				text2 = array[1].ToString();
				sb.AppendFormat("<td><input type=\"text\" id=\"{0}_tdMemberPrice_{1}\" name=\"tdMemberPrice_{1}\" style=\"width:65px\" value=\"{2}\" class=\"forminput form-control\" /></td>", columnName.Substring(0, columnName.IndexOf("_")), text, arg).AppendLine();
			}
			sb.AppendLine("</tr>");
		}
	}
}
