using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Simple : WebControl
	{
		public int SubjectId
		{
			get;
			set;
		}

		public string SkinName
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.RendHtml());
		}

		public string RendHtml()
		{
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "simple");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"pro_simple{0} cssEdite\" type=\"simple\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId);
				DataTable productList = this.GetProductList(xmlNode);
				if (productList != null && productList.Rows.Count > 0)
				{
					if (xmlNode.Attributes["SkinName"] != null && !string.IsNullOrEmpty(xmlNode.Attributes["SkinName"].Value))
					{
						this.SkinName = xmlNode.Attributes["SkinName"].Value;
						this.WriteSkin(productList, stringBuilder, xmlNode);
					}
					else
					{
						this.WriteNoSkin(productList, stringBuilder, xmlNode);
					}
				}
				stringBuilder.Append("</div>");
			}
			return stringBuilder.ToString();
		}

		private void WriteNoSkin(DataTable tbProducts, StringBuilder sb, XmlNode node)
		{
			sb.AppendLine("<ul>");
			foreach (DataRow row in tbProducts.Rows)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string arg = masterSettings.DefaultProductImage;
				if (!string.IsNullOrEmpty(row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToNullString()))
				{
					arg = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
				}
				sb.AppendLine("<li>");
				sb.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img data-url=\"{1}\"  alt=\"{2}\" /></a></div>", base.GetRouteUrl("productDetails", new
				{
					ProductId = row["ProductId"]
				}), arg, row["ProductName"]).AppendLine();
				sb.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", base.GetRouteUrl("productDetails", new
				{
					ProductId = row["ProductId"]
				}), row["ProductName"]).AppendLine();
				string empty = string.Empty;
				if (row["MarketPrice"] != DBNull.Value)
				{
					empty = Globals.FormatMoney((decimal)row["MarketPrice"]);
				}
				if (row["MarketPrice"].ToDecimal(0) > decimal.Zero)
				{
					sb.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney(row["SalePrice"].ToDecimal(0)), Globals.FormatMoney(row["MarketPrice"].ToDecimal(0))).AppendLine();
				}
				else
				{
					sb.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em></em></span></div>", Globals.FormatMoney(row["SalePrice"].ToDecimal(0))).AppendLine();
				}
				sb.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详细</a>", base.GetRouteUrl("productDetails", new
				{
					ProductId = row["ProductId"]
				})).AppendLine();
				sb.AppendLine("</li>");
			}
			sb.AppendLine("</ul>");
		}

		private void WriteSkin(DataTable tbProducts, StringBuilder sb, XmlNode node)
		{
			string url = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + this.SkinName);
			HtmlDocument htmlDocument = this.GetHtmlDocument(url);
			HtmlNode htmlNode = htmlDocument.DocumentNode.SelectSingleNode("//foreach");
			string innerHtml = htmlDocument.DocumentNode.InnerHtml;
			if (htmlNode != null)
			{
				sb.AppendLine(innerHtml.Substring(0, innerHtml.IndexOf("<foreach>")));
				foreach (DataRow row in tbProducts.Rows)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					string text = masterSettings.DefaultProductImage;
					if (!string.IsNullOrEmpty(row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToNullString()))
					{
						text = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
					}
					string newValue = text;
					string routeUrl = base.GetRouteUrl("productDetails", new
					{
						ProductId = row["ProductId"]
					});
					string newValue2 = row["ProductName"].ToString();
					string newValue3 = string.Empty;
					if (row["MarketPrice"] != DBNull.Value)
					{
						newValue3 = Globals.FormatMoney((decimal)row["MarketPrice"]);
					}
					string newValue4 = Globals.FormatMoney(row["SalePrice"].ToDecimal(0));
					if (row["MarketPrice"].ToDecimal(0) > decimal.Zero)
					{
						sb.AppendLine(htmlNode.InnerHtml.Replace("$Url$", routeUrl).Replace("$ThumbnailUrl$", newValue).Replace("$ProductName$", newValue2)
							.Replace("$MarketPrice$", newValue3)
							.Replace("$SalePrice$", newValue4));
					}
					else
					{
						sb.AppendLine(htmlNode.InnerHtml.Replace("$Url$", routeUrl).Replace("$ThumbnailUrl$", newValue).Replace("$ProductName$", newValue2)
							.Replace("¥$MarketPrice$", "")
							.Replace("$MarketPrice$", newValue3)
							.Replace("$SalePrice$", newValue4));
					}
				}
				if (innerHtml.IndexOf("</foreach>") > -1)
				{
					sb.AppendLine(innerHtml.Substring(innerHtml.IndexOf("</foreach>") + 10));
				}
			}
		}

		private HtmlDocument GetHtmlDocument(string url)
		{
			HtmlDocument htmlDocument = null;
			if (url != "")
			{
				htmlDocument = new HtmlDocument();
				htmlDocument.Load(url);
			}
			return htmlDocument;
		}

		private string ShowDefaultProductImage(string thumbnailsize, SiteSettings settings)
		{
			string result = settings.DefaultProductImage;
			switch (thumbnailsize)
			{
			case "40":
				result = settings.DefaultProductThumbnail1;
				break;
			case "60":
				result = settings.DefaultProductThumbnail2;
				break;
			case "100":
				result = settings.DefaultProductThumbnail3;
				break;
			case "160":
				result = settings.DefaultProductThumbnail4;
				break;
			case "180":
				result = settings.DefaultProductThumbnail5;
				break;
			case "220":
				result = settings.DefaultProductThumbnail6;
				break;
			case "310":
				result = settings.DefaultProductThumbnail7;
				break;
			case "410":
				result = settings.DefaultProductThumbnail8;
				break;
			}
			return result;
		}

		private DataTable GetProductList(XmlNode node)
		{
			SubjectListQuery subjectListQuery = new SubjectListQuery();
			subjectListQuery.SortBy = "DisplaySequence";
			subjectListQuery.SortOrder = SortAction.Desc;
			int categoryId = default(int);
			if (int.TryParse(node.Attributes["CategoryId"].Value, out categoryId))
			{
				subjectListQuery.Category = CatalogHelper.GetCategory(categoryId);
			}
			if (!string.IsNullOrEmpty(node.Attributes["TagId"].Value))
			{
				subjectListQuery.TagId = int.Parse(node.Attributes["TagId"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["BrandId"].Value))
			{
				subjectListQuery.BrandCategoryId = int.Parse(node.Attributes["BrandId"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["TypeId"].Value))
			{
				subjectListQuery.ProductTypeId = int.Parse(node.Attributes["TypeId"].Value);
			}
			string value = node.Attributes["AttributeString"].Value;
			if (!string.IsNullOrEmpty(value))
			{
				IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
				string[] array = value.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split('_');
					AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
					attributeValueInfo.AttributeId = Convert.ToInt32(array2[0]);
					attributeValueInfo.ValueId = Convert.ToInt32(array2[1]);
					list.Add(attributeValueInfo);
				}
				subjectListQuery.AttributeValues = list;
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
