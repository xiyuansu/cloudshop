using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectArticle : WebControl
	{
		public bool IsDelayedLoading
		{
			get;
			set;
		}

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
			string arg = this.IsDelayedLoading ? "data-url" : "src";
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "article");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"article cssEdite\" type=\"article\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendLine("<div class=\"article_bd\">");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["AdImage"].Value))
				{
					stringBuilder.AppendFormat("<div class=\"article_ad\"><img {1}=\"{0}\" /></div>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["AdImage"].Value), arg).AppendLine();
				}
				int categoryId = 0;
				int maxNum = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out maxNum);
				IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(categoryId, maxNum);
				if (articleList != null && articleList.Count > 0)
				{
					stringBuilder.AppendLine("<div class=\"article_list\">");
					stringBuilder.AppendLine("<ul>");
					foreach (ArticleInfo item in articleList)
					{
						stringBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", base.GetRouteUrl("ArticleDetails", new
						{
							articleId = item.ArticleId
						}), item.Title).AppendLine();
					}
					stringBuilder.AppendLine("</ul>");
					stringBuilder.AppendLine("</div>");
				}
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}

		private void RenderHeader(XmlNode node, StringBuilder sb)
		{
			string arg = this.IsDelayedLoading ? "data-url" : "src";
			sb.AppendLine("<div class=\"article_hd\">");
			sb.AppendLine("<h2>");
			if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
			{
				sb.AppendFormat("<img {1}=\"{0}\" />", Globals.GetImageServerUrl("http://", node.Attributes["ImageTitle"].Value), arg);
			}
			if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
			{
				sb.Append(node.Attributes["Title"].Value);
			}
			sb.AppendLine("</h2>");
			sb.AppendLine("</div>");
		}
	}
}
