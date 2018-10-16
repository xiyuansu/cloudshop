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
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Floor : WebControl
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
			StringBuilder stringBuilder = new StringBuilder();
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "floor");
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"floor{0} cssEdite\" type=\"floor\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
				if (xmlNode.Attributes["SkinName"] != null && !string.IsNullOrEmpty(xmlNode.Attributes["SkinName"].Value))
				{
					this.SkinName = xmlNode.Attributes["SkinName"].Value;
					this.RenderNoSkinWrite(stringBuilder, xmlNode);
				}
				else
				{
					this.RenderWrite(stringBuilder, xmlNode);
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}

		private void RenderWrite(StringBuilder sb, XmlNode node)
		{
			this.RenderHeader(node, sb);
			sb.AppendLine("<div class=\"floor_bd\">");
			if (!string.IsNullOrEmpty(node.Attributes["AdImage"].Value))
			{
				sb.AppendFormat("<div class=\"floor_ad\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"  /></a></div>", node.Attributes["AdUrl"].Value, node.Attributes["AdImage"].Value).AppendLine();
			}
			else
			{
				sb.AppendFormat("<div class=\"floor_ad\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"  /></a></div>", node.Attributes["AdUrl"].Value, SettingsManager.GetMasterSettings().DefaultProductImage).AppendLine();
			}
			sb.AppendLine("<div class=\"floor_pro\">");
			DataTable productList = this.GetProductList(node);
			if (productList != null && productList.Rows.Count > 0)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string text = this.ShowDefaultProductImage(node.Attributes["ImageSize"].Value, masterSettings);
				sb.AppendLine("<ul>");
				foreach (DataRow row in productList.Rows)
				{
					string arg = text;
					if (!string.IsNullOrEmpty(row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToNullString()))
					{
						arg = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
					}
					sb.AppendLine("<li>");
					sb.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", base.GetRouteUrl("productDetails", new
					{
						ProductId = row["ProductId"]
					}), arg).AppendLine();
					sb.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", base.GetRouteUrl("productDetails", new
					{
						ProductId = row["ProductId"]
					}), row["ProductName"]).AppendLine();
					string arg2 = "0";
					if (row["MarketPrice"] != DBNull.Value)
					{
						arg2 = Globals.FormatMoney((decimal)row["MarketPrice"]);
					}
					if (row["MarketPrice"].ToDecimal(0) > decimal.Zero)
					{
						sb.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney(row["SalePrice"].ToDecimal(0)), arg2).AppendLine();
					}
					else
					{
						sb.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em></em></span></div>", Globals.FormatMoney(row["SalePrice"].ToDecimal(0))).AppendLine();
					}
					sb.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详情</a>", base.GetRouteUrl("productDetails", new
					{
						ProductId = row["ProductId"]
					}), row["ProductName"]).AppendLine();
					sb.AppendLine("</li>");
				}
				sb.AppendLine("</ul>");
			}
			sb.AppendLine("</div>");
			sb.AppendLine("</div>");
		}

		private void RenderNoSkinWrite(StringBuilder sb, XmlNode node)
		{
			string text = string.Empty;
			string url = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + this.SkinName);
			HtmlDocument htmlDocument = this.GetHtmlDocument(url);
			text = htmlDocument.DocumentNode.OuterHtml;
			this.RenderSubCategory(node, ref text, htmlDocument);
			this.RenderProducts(node, ref text, htmlDocument);
			text = (string.IsNullOrEmpty(node.Attributes["AdImage"].Value) ? text.Replace("$AdImage$", SettingsManager.GetMasterSettings().DefaultProductImage) : text.Replace("$AdImage$", node.Attributes["AdImage"].Value));
			text = (string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value) ? text.Replace(htmlDocument.DocumentNode.SelectSingleNode("//img[@src='$ImageTitle$']").OuterHtml, "") : text.Replace("$ImageTitle$", node.Attributes["ImageTitle"].Value));
			text = text.Replace("$Title$", node.Attributes["Title"].Value).Replace("$TitelUrl$", node.Attributes["TitelUrl"].Value.Contains("http://") ? node.Attributes["TitelUrl"].Value : ("http://" + node.Attributes["TitelUrl"].Value)).Replace("$AdUrl$", node.Attributes["AdUrl"].Value.Contains("http://") ? node.Attributes["AdUrl"].Value : ("http://" + node.Attributes["AdUrl"].Value));
			sb.AppendLine(text);
		}

		private void RenderSubCategory(XmlNode node, ref string outerhtml, HtmlDocument floordocument)
		{
			StringBuilder stringBuilder = new StringBuilder();
			HtmlNode htmlNode = floordocument.DocumentNode.SelectSingleNode("//foreach[@data='SubCategory']");
			int num = 0;
			if (int.TryParse(node.Attributes["CategoryId"].Value, out num) && htmlNode != null && num > 0)
			{
				IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(num);
				if (subCategories != null)
				{
					IEnumerable<CategoryInfo> enumerable = subCategories.Take(int.Parse(node.Attributes["SubCategoryNum"].Value));
					foreach (CategoryInfo item in enumerable)
					{
						string newValue = (!string.IsNullOrWhiteSpace(item.RewriteName)) ? base.GetRouteUrl("subCategory_Rewrite", new
						{
							rewrite = item.RewriteName,
							categoryId = item.CategoryId
						}) : base.GetRouteUrl("subCategory", new
						{
							categoryId = item.CategoryId
						});
						stringBuilder.AppendLine(htmlNode.InnerHtml.Replace("$SubUrl$", newValue).Replace("$SubName$", item.Name));
					}
					outerhtml = outerhtml.Replace(htmlNode.OuterHtml, stringBuilder.ToString());
				}
				else
				{
					outerhtml = outerhtml.Replace("$SubUrl$", "").Replace("$SubName$", "");
				}
			}
			else if (htmlNode != null)
			{
				outerhtml = outerhtml.Replace(htmlNode.OuterHtml, "");
			}
			htmlNode?.RemoveAll();
			HtmlNode htmlNode2 = floordocument.DocumentNode.SelectSingleNode("//a[@href='$MoreLink$']");
			if (htmlNode2 != null)
			{
				string newValue2 = "";
				if (node.Attributes["IsShowMoreLink"].Value == "true")
				{
					newValue2 = htmlNode2.OuterHtml.Replace("$MoreLink$", base.GetRouteUrl("subCategory", new
					{
						categoryId = num
					}));
				}
				outerhtml = outerhtml.Replace(htmlNode2.OuterHtml, newValue2);
				htmlNode2.RemoveAll();
			}
		}

		private void RenderProducts(XmlNode node, ref string outerhtml, HtmlDocument floordocument)
		{
			HtmlNode htmlNode = floordocument.DocumentNode.SelectSingleNode("//foreach[@data='Products']");
			string text = string.Empty;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (htmlNode != null)
			{
				DataTable productList = this.GetProductList(node);
				foreach (DataRow row in productList.Rows)
				{
					string text2 = masterSettings.DefaultProductImage;
					if (!string.IsNullOrEmpty(row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToNullString()))
					{
						text2 = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
					}
					string routeUrl = base.GetRouteUrl("productDetails", new
					{
						ProductId = row["ProductId"]
					});
					string newValue = text2;
					string newValue2 = string.Empty;
					if (row["MarketPrice"] != DBNull.Value)
					{
						newValue2 = Globals.FormatMoney((decimal)row["MarketPrice"]);
					}
					text = ((!(row["MarketPrice"].ToDecimal(0) > decimal.Zero)) ? (text + htmlNode.InnerHtml.Replace("$Url$", routeUrl).Replace("$ThumbnailUrl$", newValue).Replace("$ProductName$", row["ProductName"].ToString())
						.Replace("$SalePrice$", Globals.FormatMoney(row["SalePrice"].ToDecimal(0)))
						.Replace("<em>￥</em>$MarketPrice$", "<em></em>")
						.Replace("$MarketPrice$", newValue2)) : (text + htmlNode.InnerHtml.Replace("$Url$", routeUrl).Replace("$ThumbnailUrl$", newValue).Replace("$ProductName$", row["ProductName"].ToString())
						.Replace("$SalePrice$", Globals.FormatMoney(row["SalePrice"].ToDecimal(0)))
						.Replace("$MarketPrice$", newValue2)));
				}
				outerhtml = outerhtml.Replace(htmlNode.OuterHtml, text);
				htmlNode.RemoveAll();
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

		private void RenderHeader(XmlNode node, StringBuilder sb)
		{
			sb.AppendLine("<div class=\"floor_hd\">");
			sb.AppendLine("<div>");
			string text = string.Empty;
			if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
			{
				text = string.Format("<span class=\"icon\"><img src=\"{0}\" /></span>", node.Attributes["ImageTitle"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
			{
				text += string.Format("<span class=\"title\">{0}</span>", node.Attributes["Title"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["TitelUrl"].Value))
			{
				sb.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a>", node.Attributes["TitelUrl"].Value, text);
			}
			else
			{
				sb.Append(text);
			}
			sb.AppendLine("</div>");
			int num = 0;
			if (int.TryParse(node.Attributes["CategoryId"].Value, out num))
			{
				IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(num);
				if (subCategories != null)
				{
					IEnumerable<CategoryInfo> enumerable = subCategories.Take(int.Parse(node.Attributes["SubCategoryNum"].Value));
					sb.AppendLine("<ul>");
					foreach (CategoryInfo item in enumerable)
					{
						string arg = (!string.IsNullOrWhiteSpace(item.RewriteName)) ? base.GetRouteUrl("subCategory_Rewrite", new
						{
							rewrite = item.RewriteName,
							categoryId = num
						}) : base.GetRouteUrl("subCategory", new
						{
							categoryId = num
						});
						sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", arg, item.Name).AppendLine("");
					}
					sb.AppendLine("</ul>");
				}
				if (node.Attributes["IsShowMoreLink"].Value == "true")
				{
					sb.AppendFormat("<em><a href=\"{0}\">更多>></a></em>", base.GetRouteUrl("subCategory", new
					{
						categoryId = num
					})).AppendLine();
				}
			}
			sb.AppendLine("</div>");
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
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
