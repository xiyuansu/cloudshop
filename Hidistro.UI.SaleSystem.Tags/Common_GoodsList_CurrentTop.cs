using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_GoodsList_CurrentTop : WebControl
	{
		private int imageSize = 60;

		public int MaxNum
		{
			get;
			set;
		}

		public int ImageNum
		{
			get;
			set;
		}

		public bool IsShowPrice
		{
			get;
			set;
		}

		public bool IsShowSaleCounts
		{
			get;
			set;
		}

		public int ImageSize
		{
			get
			{
				return this.imageSize;
			}
			set
			{
				this.imageSize = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.RendHtml());
		}

		public string RendHtml()
		{
			StringBuilder stringBuilder = new StringBuilder();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			stringBuilder.AppendFormat("<div class=\"sale_top\" >");
			DataTable productList = this.GetProductList();
			if (productList != null && productList.Rows.Count > 0)
			{
				int num = 0;
				stringBuilder.AppendLine("<ul>");
				foreach (DataRow row in productList.Rows)
				{
					num++;
					stringBuilder.AppendFormat("<li class=\"saleitem{0}\">", num).AppendLine();
					stringBuilder.AppendFormat("<em>{0}</em>", num).AppendLine();
					if (num <= this.ImageNum)
					{
						string text = (row["ThumbnailUrl" + this.ImageSize] == DBNull.Value) ? "" : row["ThumbnailUrl" + this.ImageSize].ToString();
						text = ((!string.IsNullOrEmpty(text)) ? Globals.FullPath(text) : Globals.FullPath(masterSettings.DefaultProductImage));
						stringBuilder.AppendFormat("<div class=\"img\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", base.GetRouteUrl("productDetails", new
						{
							ProductId = row["ProductId"]
						}), text).AppendLine();
					}
					stringBuilder.AppendLine("<div class=\"info\">");
					stringBuilder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", base.GetRouteUrl("productDetails", new
					{
						ProductId = row["ProductId"]
					}), row["ProductName"]).AppendLine();
					if (this.IsShowPrice)
					{
						string arg = string.Empty;
						if (row["MarketPrice"] != DBNull.Value)
						{
							arg = Globals.FormatMoney(row["MarketPrice"].ToDecimal(0));
						}
						stringBuilder.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney(row["SalePrice"].ToDecimal(0)), arg).AppendLine();
					}
					if (this.IsShowSaleCounts)
					{
						stringBuilder.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", row["SaleCounts"]).AppendLine();
					}
					stringBuilder.Append("</div>");
					stringBuilder.AppendLine("</li>");
				}
				stringBuilder.AppendLine("</ul>");
			}
			stringBuilder.Append("</div>");
			return stringBuilder.ToString();
		}

		private DataTable GetProductList()
		{
			SubjectListQuery subjectListQuery = new SubjectListQuery();
			int categoryId = default(int);
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out categoryId))
			{
				subjectListQuery.Category = CatalogHelper.GetCategory(categoryId);
			}
			int value = default(int);
			if (int.TryParse(this.Page.Request.QueryString["brand"], out value))
			{
				subjectListQuery.BrandCategoryId = value;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
			{
				IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
				string textToFormat = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
				textToFormat = Globals.HtmlEncode(textToFormat);
				string[] array = textToFormat.Split('-');
				if (!string.IsNullOrEmpty(textToFormat))
				{
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Split('_');
						if (array2.Length != 0 && !string.IsNullOrEmpty(array2[1]) && array2[1] != "0")
						{
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.AttributeId = Convert.ToInt32(array2[0]);
							attributeValueInfo.ValueId = Convert.ToInt32(array2[1]);
							list.Add(attributeValueInfo);
						}
					}
				}
				subjectListQuery.AttributeValues = list;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				subjectListQuery.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value2 = default(decimal);
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value2))
				{
					subjectListQuery.MinPrice = value2;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value3 = default(decimal);
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value3))
				{
					subjectListQuery.MaxPrice = value3;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				int tagId = 0;
				if (int.TryParse(this.Page.Request.QueryString["TagIds"], out tagId))
				{
					subjectListQuery.TagId = tagId;
				}
			}
			subjectListQuery.MaxNum = this.MaxNum;
			subjectListQuery.SortBy = "ShowSaleCounts";
			subjectListQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(subjectListQuery, true);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
