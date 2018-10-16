using Hidistro.Core;
using Hidistro.Core.Urls;
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
	public class Common_SubjectBrand : WebControl
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
			string text = this.IsDelayedLoading ? "data-url" : "src";
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "brand");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"brand cssEdite\" type=\"brand\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				int categoryId = 0;
				int maxNum = 0;
				bool flag = true;
				bool flag2 = true;
				string text2 = "";
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out maxNum);
				bool.TryParse(xmlNode.Attributes["IsShowLogo"].Value, out flag);
				bool.TryParse(xmlNode.Attributes["IsShowTitle"].Value, out flag2);
				text2 = xmlNode.Attributes["ImageSize"].Value;
				IEnumerable<BrandMode> brandCategories = CatalogHelper.GetBrandCategories(categoryId, maxNum);
				if (brandCategories != null)
				{
					stringBuilder.AppendLine("<ul>");
					foreach (BrandMode item in brandCategories)
					{
						stringBuilder.AppendLine("<li>");
						if (flag)
						{
							stringBuilder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img {3}=\"{1}\" width=\"{2}\"></a></div>", RouteConfig.SubBrandDetails(item.BrandId, item.RewriteName), Globals.GetImageServerUrl("http://", item.Logo), text2.Split('*')[0], text).AppendLine();
						}
						if (flag2)
						{
							stringBuilder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", RouteConfig.SubBrandDetails(item.BrandId, item.RewriteName), item.BrandName).AppendLine();
						}
						stringBuilder.AppendLine("</li>");
					}
					stringBuilder.AppendLine("</ul>");
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
