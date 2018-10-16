using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class SKUSelector : WebControl
	{
		public const string TagID = "SKUSelector";

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

		public DataTable DataSource
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int? ProductTypeId
		{
			get;
			set;
		}

		public string ProductTypeRemark
		{
			get;
			set;
		}

		public SKUSelector()
		{
			base.ID = "SKUSelector";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DataTable dataSource = this.DataSource;
			if (dataSource != null && dataSource.Rows.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				IList<string> list = new List<string>();
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"{0}\" value=\"{1}\" />", "hiddenProductType", this.ProductTypeRemark).AppendLine();
				stringBuilder.AppendFormat("<input type=\"hidden\" id=\"{0}\" value=\"{1}\" />", "hiddenProductId", this.ProductId).AppendLine();
				stringBuilder.AppendFormat("<div id=\"productSkuSelector\" class=\"{0}\">", this.CssClass).AppendLine();
				string str = string.Empty;
				foreach (DataRow row in dataSource.Rows)
				{
					if (!list.Contains((string)row["AttributeName"]))
					{
						list.Add((string)row["AttributeName"]);
						str = str + "\"" + (string)row["AttributeName"] + "\" ";
						stringBuilder.AppendLine("<div class=\"SKURowClass\">");
						stringBuilder.AppendFormat("<span>{0}：</span><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", row["AttributeName"], row["AttributeId"]);
						stringBuilder.AppendFormat("<dl id=\"skuRow_{0}\">", row["AttributeId"]);
						IList<string> list2 = new List<string>();
						foreach (DataRow row2 in dataSource.Rows)
						{
							if (string.Compare((string)row["AttributeName"], (string)row2["AttributeName"]) == 0 && !list2.Contains((string)row2["ValueStr"]))
							{
								string text = "skuValueId_" + row["AttributeId"] + "_" + row2["ValueId"];
								list2.Add((string)row2["ValueStr"]);
								if ((bool)row["UseAttributeImage"])
								{
									if (row2["ThumbnailUrl40"] == DBNull.Value)
									{
										stringBuilder.AppendFormat("<dd><input type=\"button\" class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" value=\"{3}\" /></dd> ", text, row["AttributeId"], row2["ValueId"], row2["ValueStr"]);
									}
									else
									{
										stringBuilder.AppendFormat("<dd><a class=\"cloud-zoom-gallery\" rel=\"useZoom: 'zoom1', smallImage: '{5}'\" href=\"{6}\" title=\"{7}\"><img class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" value=\"{3}\" src=\"{4}\" /></a></dd> ", text, row["AttributeId"], row2["ValueId"], row2["ValueStr"], (row2["ThumbnailUrl40"] == DBNull.Value) ? "" : ((string)row2["ThumbnailUrl40"]), (row2["ThumbnailUrl410"] == DBNull.Value) ? "" : ((string)row2["ThumbnailUrl410"]), (row2["ImageUrl"] == DBNull.Value) ? "" : ((string)row2["ImageUrl"]), (string)row2["ValueStr"]);
									}
								}
								else
								{
									stringBuilder.AppendFormat("<dd><input type=\"button\" class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" value=\"{3}\" /></dd> ", text, row["AttributeId"], row2["ValueId"], row2["ValueStr"]);
								}
							}
						}
						stringBuilder.AppendLine("</dl></div>");
					}
				}
				stringBuilder.AppendLine("</div>");
				writer.Write(stringBuilder.ToString());
			}
		}
	}
}
