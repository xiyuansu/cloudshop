using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectCategory : WebControl
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
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "category");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"category cssEdite\" type=\"category\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				int parentCategoryId = 0;
				int count = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out parentCategoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out count);
				IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(parentCategoryId);
				if (subCategories != null)
				{
					stringBuilder.AppendLine("<ul>");
					IEnumerable<CategoryInfo> enumerable = subCategories.Take(count);
					foreach (CategoryInfo item in enumerable)
					{
						string arg = (!string.IsNullOrEmpty(item.RewriteName)) ? base.GetRouteUrl("subCategory_Rewrite", new
						{
							rewrite = item.RewriteName,
							categoryId = item.CategoryId
						}) : base.GetRouteUrl("subCategory", new
						{
							categoryId = item.CategoryId
						});
						stringBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", arg, item.Name).AppendLine();
					}
					stringBuilder.AppendLine("</ul>");
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
