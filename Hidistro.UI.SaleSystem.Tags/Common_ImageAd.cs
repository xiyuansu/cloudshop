using Hidistro.UI.Common.Controls;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ImageAd : WebControl
	{
		public bool IsDelayedLoading
		{
			get;
			set;
		}

		public int AdId
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
			XmlNode xmlNode = TagsHelper.FindAdNode(this.AdId, "image");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"adv_image cssEdite\" type=\"image\" id=\"ads_{0}\" >", this.AdId).AppendLine();
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Url"].Value))
				{
					stringBuilder.AppendFormat("<a target=\"_blank\" href=\"{0}\"><img {2}=\"{1}\" /></a>", xmlNode.Attributes["Url"].Value, xmlNode.Attributes["Image"].Value, text).AppendLine();
				}
				else
				{
					stringBuilder.AppendFormat("<a target=\"_blank\"><img {1}=\"{0}\" /></a>", xmlNode.Attributes["Image"].Value, text).AppendLine();
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
