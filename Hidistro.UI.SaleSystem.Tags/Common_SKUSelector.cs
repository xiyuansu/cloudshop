using Hidistro.Core;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SKUSelector : WebControl
	{
		public int ProductId
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DataTable skus = ProductBrowser.GetSkus(this.ProductId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenSkuId\" value=\"{0}_0\"  />", this.ProductId).AppendLine();
			if (skus != null && skus.Rows.Count > 0)
			{
				IList<string> list = new List<string>();
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"hiddenProductId\" value=\"{0}\" />", this.ProductId).AppendLine();
				stringBuilder.AppendLine("<div class=\"spec_pro\">");
				foreach (DataRow row in skus.Rows)
				{
					if (!list.Contains((string)row["AttributeName"]))
					{
						list.Add((string)row["AttributeName"]);
						stringBuilder.AppendFormat("<div class=\"text-muted\">{0}：</div><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", row["AttributeName"], row["AttributeId"]);
						stringBuilder.AppendFormat("<div class=\"list clearfix\" id=\"skuRow_{0}\">", row["AttributeId"]);
						IList<string> list2 = new List<string>();
						foreach (DataRow row2 in skus.Rows)
						{
							if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list2.Contains((string)row2["ValueStr"]))
							{
								string text = "skuValueId_" + row["AttributeId"] + "_" + row2["ValueId"];
								list2.Add((string)row2["ValueStr"]);
								if ((bool)row["UseAttributeImage"])
								{
									stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" ImgUrl=\"{4}\">{3}</div>", text, row["AttributeId"], row2["ValueId"], row2["ValueStr"], Globals.GetImageServerUrl("http://", (row2["ThumbnailUrl410"] == DBNull.Value) ? "" : ((string)row2["ThumbnailUrl410"])));
								}
								else
								{
									stringBuilder.AppendFormat("<div class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\">{3}</div>", text, row["AttributeId"], row2["ValueId"], row2["ValueStr"]);
								}
							}
						}
						stringBuilder.AppendLine("</div>");
					}
				}
				stringBuilder.AppendLine("</div>");
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
