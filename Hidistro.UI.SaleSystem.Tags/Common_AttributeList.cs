using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_AttributeList : WebControl
	{
		private int categoryId;

		private int brandId;

		private string valueStr = string.Empty;

		protected string GetParameter(string name)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}

		protected override void OnInit(EventArgs e)
		{
			this.categoryId = this.GetParameter("categoryId").ToInt(0);
			this.brandId = this.GetParameter("brand").ToInt(0);
			this.valueStr = this.GetParameter("valueStr").ToNullString();
			base.OnInit(e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<div class=\"attribute_bd\">");
			this.RendeBrand(stringBuilder);
			this.RendeAttribute(stringBuilder);
			stringBuilder.AppendLine("</div>");
			writer.Write(stringBuilder.ToString());
		}

		private void RendeBrand(StringBuilder sb)
		{
			IEnumerable<BrandMode> brandCategories = CatalogHelper.GetBrandCategories(this.categoryId, 1000);
			if (brandCategories != null)
			{
				sb.AppendLine("<dl class=\"attribute_dl\">");
				sb.AppendLine("<dt class=\"attribute_name\">品牌：</dt>");
				sb.AppendLine("<dd class=\"attribute_val\">");
				sb.AppendLine("<div class=\"h_chooselist\">");
				string text = "all";
				if (this.brandId == 0)
				{
					text += " select";
				}
				sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", text, this.CreateUrl("brand", "")).AppendLine();
				foreach (BrandMode item in brandCategories)
				{
					text = string.Empty;
					if (this.brandId == item.BrandId)
					{
						text += " select";
					}
					sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", text, this.CreateUrl("brand", item.BrandId.ToString()), item.BrandName).AppendLine();
				}
				sb.AppendLine("</div>");
				sb.AppendLine("</dd>");
				sb.AppendLine("</dl>");
			}
		}

		private void RendeAttribute(StringBuilder sb)
		{
			IList<AttributeInfo> attributeInfoByCategoryId = ProductTypeHelper.GetAttributeInfoByCategoryId(this.categoryId, 1000);
			if (attributeInfoByCategoryId != null && attributeInfoByCategoryId.Count > 0)
			{
				foreach (AttributeInfo item in attributeInfoByCategoryId)
				{
					sb.AppendLine("<dl class=\"attribute_dl\">");
					if (item.AttributeValues.Count > 0)
					{
						sb.AppendFormat("<dt class=\"attribute_name\">{0}：</dt>", item.AttributeName).AppendLine();
						sb.AppendLine("<dd class=\"attribute_val\">");
						sb.AppendLine("<div class=\"h_chooselist\">");
						string text = "all";
						string paraValue = this.RemoveAttribute(this.valueStr, item.AttributeId, 0);
						string arg = "all select";
						if (!string.IsNullOrEmpty(this.valueStr) && new Regex($"{item.AttributeId}_[1-9]+").IsMatch(this.valueStr))
						{
							arg = "all";
						}
						sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", arg, this.CreateUrl("valuestr", paraValue)).AppendLine();
						foreach (AttributeValueInfo attributeValue in item.AttributeValues)
						{
							text = string.Empty;
							paraValue = this.RemoveAttribute(this.valueStr, item.AttributeId, attributeValue.ValueId);
							if (!string.IsNullOrEmpty(this.valueStr))
							{
								string[] source = this.valueStr.Split('-');
								if (source.Contains(item.AttributeId + "_" + attributeValue.ValueId))
								{
									text = "select";
								}
							}
							sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", text, this.CreateUrl("valuestr", paraValue), attributeValue.ValueStr).AppendLine();
						}
						sb.AppendLine("</div>");
						sb.AppendLine("</dd>");
					}
					sb.AppendLine("</dl>");
				}
			}
		}

		private string RemoveAttribute(string paraValue, int attributeId, int valueId)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(paraValue))
			{
				string[] array = paraValue.Split('-');
				if (array != null && array.Length != 0)
				{
					string[] array2 = array;
					foreach (string text2 in array2)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string[] array3 = text2.Split('_');
							if (array3 != null && array3.Length != 0 && array3[0] != attributeId.ToString())
							{
								text = text + text2 + "-";
							}
						}
					}
				}
			}
			return text + attributeId + "_" + valueId;
		}

		private string CreateUrl(string paraName, string paraValue)
		{
			string rawUrl = this.Context.Request.RawUrl;
			if (rawUrl.IndexOf("?") >= 0)
			{
				string text = rawUrl.Substring(rawUrl.IndexOf("?") + 1);
				string[] array = text.Split(Convert.ToChar("&"));
				rawUrl = rawUrl.Replace(text, "");
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text2.ToLower().StartsWith(paraName + "=") && !text2.ToLower().StartsWith("pageindex="))
					{
						rawUrl = rawUrl + text2 + "&";
					}
				}
				return rawUrl + paraName + "=" + Globals.UrlEncode(paraValue);
			}
			return rawUrl + "?" + paraName + "=" + Globals.UrlEncode(paraValue);
		}
	}
}
