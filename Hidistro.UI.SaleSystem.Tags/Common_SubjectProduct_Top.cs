using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Top : WebControl
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "top");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"sale_top{0} cssEdite\" type=\"top\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId);
				if (xmlNode.Attributes["SkinName"] != null && !string.IsNullOrEmpty(xmlNode.Attributes["SkinName"].Value))
				{
					this.SkinName = xmlNode.Attributes["SkinName"].Value;
					this.RenderWriteSkin(stringBuilder, xmlNode);
				}
				else
				{
					this.RenderNoSkin(stringBuilder, xmlNode);
				}
				stringBuilder.Append("</div>");
			}
			return stringBuilder.ToString();
		}

		private void RenderWriteSkin(StringBuilder sb, XmlNode node)
		{
			int num = 0;
			int.TryParse(node.Attributes["ImageNum"].Value, out num);
			bool flag = false;
			bool.TryParse(node.Attributes["IsShowPrice"].Value, out flag);
			bool flag2 = false;
			bool.TryParse(node.Attributes["IsShowSaleCounts"].Value, out flag2);
			bool flag3 = false;
			bool.TryParse(node.Attributes["IsImgShowPrice"].Value, out flag3);
			bool flag4 = false;
			bool.TryParse(node.Attributes["IsImgShowSaleCounts"].Value, out flag4);
			DataTable productList = this.GetProductList(node);
			string empty = string.Empty;
			string url = Globals.PhysicalPath(HiContext.Current.GetPCHomePageSkinPath() + "/" + this.SkinName);
			HtmlDocument htmlDocument = this.GetHtmlDocument(url);
			string text = string.Empty;
			string text2 = string.Empty;
			HtmlNode htmlNode = htmlDocument.DocumentNode.SelectSingleNode("//foreach[@data='Images']");
			HtmlNode htmlNode2 = htmlDocument.DocumentNode.SelectSingleNode("//foreach[@data='Text']");
			if (productList != null && productList.Rows.Count > 0 && htmlNode != null && htmlNode2 != null)
			{
				int num2 = 0;
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				foreach (DataRow row in productList.Rows)
				{
					string newValue = masterSettings.DefaultProductImage;
					if (!string.IsNullOrEmpty(row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToNullString()))
					{
						newValue = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
					}
					num2++;
					if (num2 <= num && htmlNode != null)
					{
						HtmlNode htmlNode3 = htmlNode.SelectSingleNode("//display[@type='Price']");
						HtmlNode htmlNode4 = htmlNode.SelectSingleNode("//display[@type='SaleCount']");
						string newValue2 = "";
						string newValue3 = "";
						if (flag3 && num2 <= num && htmlNode3 != null)
						{
							string newValue4 = string.Empty;
							if (row["MarketPrice"] != DBNull.Value)
							{
								newValue4 = Globals.FormatMoney((decimal)row["MarketPrice"]);
							}
							newValue2 = ((!(row["MarketPrice"].ToDecimal(0) > decimal.Zero)) ? htmlNode3.InnerHtml.Replace("<em>￥</em>$MarketPrice$", "<em></em>").Replace("$MarketPrice$", newValue4).Replace("$SalePrice$", Globals.FormatMoney(row["SalePrice"].ToDecimal(0))) : htmlNode3.InnerHtml.Replace("$MarketPrice$", newValue4).Replace("$SalePrice$", Globals.FormatMoney(row["SalePrice"].ToDecimal(0))));
						}
						if (flag4 && htmlNode4 != null)
						{
							newValue3 = htmlNode4.InnerHtml.Replace("$SaleCount$", row["SaleCounts"].ToString());
						}
						text += htmlNode.InnerHtml.Replace("$Number$", num2.ToString()).Replace("$Url$", base.GetRouteUrl("productDetails", new
						{
							ProductId = row["ProductId"]
						})).Replace("$ThumbnailUrl$", newValue)
							.Replace("$ProductName$", row["ProductName"].ToString())
							.Replace(htmlNode3.OuterHtml, newValue2)
							.Replace(htmlNode4.OuterHtml, newValue3);
					}
					else if (htmlNode2 != null)
					{
						HtmlNode htmlNode5 = htmlNode2.SelectNodes("//display[@type='Price']")[1];
						HtmlNode htmlNode6 = htmlNode2.SelectNodes("//display[@type='SaleCount']")[1];
						string newValue5 = "";
						string newValue6 = "";
						string newValue7 = string.Empty;
						if (row["MarketPrice"] != DBNull.Value)
						{
							newValue7 = Globals.FormatMoney((decimal)row["MarketPrice"]);
						}
						if (flag2 && htmlNode6 != null)
						{
							newValue6 = htmlNode6.InnerHtml.Replace("$SaleCount$", row["SaleCounts"].ToString());
						}
						if (flag && htmlNode5 != null)
						{
							newValue5 = htmlNode5.InnerHtml.Replace("$MarketPrice$", newValue7).Replace("$SalePrice$", Globals.FormatMoney(row["SalePrice"].ToDecimal(0)));
						}
						text2 += htmlNode2.InnerHtml.Replace("$Number$", num2.ToString()).Replace("$Url$", base.GetRouteUrl("productDetails", new
						{
							ProductId = row["ProductId"]
						})).Replace("$ThumbnailUrl$", newValue)
							.Replace("$ProductName$", row["ProductName"].ToString())
							.Replace(htmlNode5.OuterHtml, newValue5)
							.Replace(htmlNode6.OuterHtml, newValue6);
					}
				}
				sb.AppendLine(htmlDocument.DocumentNode.OuterHtml.Replace(htmlNode.OuterHtml, text).Replace(htmlNode2.OuterHtml, text2));
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

		private void RenderNoSkin(StringBuilder sb, XmlNode node)
		{
			int num = 0;
			int.TryParse(node.Attributes["ImageNum"].Value, out num);
			bool flag = false;
			bool.TryParse(node.Attributes["IsShowPrice"].Value, out flag);
			bool flag2 = false;
			bool.TryParse(node.Attributes["IsShowSaleCounts"].Value, out flag2);
			bool flag3 = false;
			bool.TryParse(node.Attributes["IsImgShowPrice"].Value, out flag3);
			bool flag4 = false;
			bool.TryParse(node.Attributes["IsImgShowSaleCounts"].Value, out flag4);
			DataTable productList = this.GetProductList(node);
			if (productList != null && productList.Rows.Count > 0)
			{
				int num2 = 0;
				sb.AppendLine("<ul>");
				{
					IEnumerator enumerator = productList.Rows.GetEnumerator();
					try
					{
						for (; enumerator.MoveNext(); sb.AppendLine("</li>"))
						{
							DataRow dataRow = (DataRow)enumerator.Current;
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							string arg = masterSettings.DefaultProductImage;
							if (!string.IsNullOrEmpty(dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToNullString()))
							{
								arg = dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
							}
							num2++;
							sb.AppendFormat("<li class=\"sale_top{0}\">", num2).AppendLine();
							sb.AppendFormat("<em>{0}</em>", num2).AppendLine();
							if (num2 <= num)
							{
								sb.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img data-url=\"{1}\" alt=\"{2}\" /></a></div>", base.GetRouteUrl("productDetails", new
								{
									ProductId = dataRow["ProductId"]
								}), arg, dataRow["ProductName"]).AppendLine();
								sb.AppendLine("<div class=\"info\">");
								sb.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", base.GetRouteUrl("productDetails", new
								{
									ProductId = dataRow["ProductId"]
								}), dataRow["ProductName"]).AppendLine();
								if (flag && num2 > num)
								{
									goto IL_0231;
								}
								if (flag3 && num2 <= num)
								{
									goto IL_0231;
								}
								goto IL_0302;
							}
							sb.AppendFormat("<div class=\"txt\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", base.GetRouteUrl("productDetails", new
							{
								ProductId = dataRow["ProductId"]
							}), dataRow["ProductName"]).AppendLine();
							if (flag && num2 > num)
							{
								string empty = string.Empty;
								if (dataRow["MarketPrice"] != DBNull.Value)
								{
									empty = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
								}
								sb.AppendFormat("<div class=\"price\" style=\"float:left;margin-left:28px\"><b>￥{0}</b></div>", Globals.FormatMoney(dataRow["SalePrice"].ToDecimal(0))).AppendLine();
							}
							if (flag2 && num2 > num)
							{
								sb.AppendFormat("<div class=\"sale\" style=\"float:right;margin-right:14px\">已售<b>{0}</b>件 </div>", dataRow["SaleCounts"]).AppendLine();
							}
							continue;
							IL_0313:
							sb.AppendFormat("<div class=\"sale\">已售<b>{0}</b>件 </div>", dataRow["SaleCounts"]).AppendLine();
							goto IL_0330;
							IL_0231:
							string arg2 = string.Empty;
							if (dataRow["MarketPrice"] != DBNull.Value)
							{
								arg2 = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
							}
							if (dataRow["MarketPrice"].ToDecimal(0) > decimal.Zero)
							{
								sb.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney(dataRow["SalePrice"].ToDecimal(0)), arg2).AppendLine();
							}
							else
							{
								sb.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em></em></span></div>", Globals.FormatMoney(dataRow["SalePrice"].ToDecimal(0))).AppendLine();
							}
							sb.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详细</a>", base.GetRouteUrl("productDetails", new
							{
								ProductId = dataRow["ProductId"]
							})).AppendLine();
							goto IL_0302;
							IL_0330:
							sb.Append("</div>");
							continue;
							IL_0302:
							if (flag2 && num2 > num)
							{
								goto IL_0313;
							}
							if (flag4 && num2 <= num)
							{
								goto IL_0313;
							}
							goto IL_0330;
						}
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				sb.AppendLine("</ul>");
			}
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
			subjectListQuery.SortBy = "ShowSaleCounts";
			subjectListQuery.SortOrder = SortAction.Desc;
			int categoryId = default(int);
			if (int.TryParse(node.Attributes["CategoryId"].Value, out categoryId))
			{
				subjectListQuery.Category = CatalogHelper.GetCategory(categoryId);
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
