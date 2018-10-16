using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectKeyword : WebControl
	{
		public int CommentId
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
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "keyword");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				int categoryId = 0;
				int categoryId2 = 0;
				int hotKeywordsNum = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId2);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out hotKeywordsNum);
				CategoryInfo category = CatalogHelper.GetCategory(categoryId2);
				if (category != null)
				{
					categoryId = category.TopCategoryId;
				}
				List<HotkeywordInfo> hotKeywords = CommentBrowser.GetHotKeywords(categoryId, hotKeywordsNum);
				stringBuilder.AppendFormat("<ul class=\"keyword cssEdite\" type=\"keyword\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				if (hotKeywords != null && hotKeywords.Count > 0)
				{
					foreach (HotkeywordInfo item in hotKeywords)
					{
						stringBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", base.GetRouteUrl("subCategory", new
						{
							categoryId = item.CategoryId
						}) + "?keywords=" + Globals.UrlEncode(item.Keywords), item.Keywords).AppendLine();
					}
				}
				stringBuilder.AppendLine("</ul>");
			}
			return stringBuilder.ToString();
		}
	}
}
