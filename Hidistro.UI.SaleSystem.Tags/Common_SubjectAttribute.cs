using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectAttribute : WebControl
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
			StringBuilder stringBuilder = new StringBuilder();
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "attribute");
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"attribute_bd cssEdite\" type=\"attribute\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				int categoryId = 0;
				int num = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out num);
				string text = null;
				CategoryInfo category = CatalogHelper.GetCategory(categoryId);
				if (category != null)
				{
					text = category.RewriteName;
				}
				IList<AttributeInfo> attributeInfoByCategoryId = ProductTypeHelper.GetAttributeInfoByCategoryId(categoryId, 1000);
				string text2 = (!string.IsNullOrWhiteSpace(text)) ? base.GetRouteUrl("subCategory_Rewrite", new
				{
					rewrite = text,
					categoryId = categoryId
				}) : base.GetRouteUrl("subCategory", new
				{
					categoryId
				});
				if (attributeInfoByCategoryId != null && attributeInfoByCategoryId.Count > 0)
				{
					foreach (AttributeInfo item in attributeInfoByCategoryId)
					{
						stringBuilder.AppendLine("<dl class=\"attribute_dl\">");
						stringBuilder.AppendFormat("<dt class=\"attribute_name\">{0}：</dt>", item.AttributeName).AppendLine();
						stringBuilder.AppendLine("<dd class=\"attribute_val\">");
						stringBuilder.AppendLine("<div class=\"h_chooselist\">");
						foreach (AttributeValueInfo attributeValue in item.AttributeValues)
						{
							stringBuilder.AppendFormat("<a href=\"{0}\" >{1}</a>", text2 + "?valueStr=" + attributeValue.AttributeId + "_" + attributeValue.ValueId, attributeValue.ValueStr).AppendLine();
						}
						stringBuilder.AppendLine("</div>");
						stringBuilder.AppendLine("</dd>");
						stringBuilder.AppendLine("</dl>");
					}
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
