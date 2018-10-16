using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectTitle : WebControl
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
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "title");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"title cssEdite\" type=\"title\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				stringBuilder.AppendLine("<div>");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["ImageTitle"].Value))
				{
					stringBuilder.AppendFormat("<span class=\"icon\"><img {1}=\"{0}\" /></span>", Globals.GetImageServerUrl("http://", xmlNode.Attributes["ImageTitle"].Value), arg);
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Title"].Value))
				{
					stringBuilder.AppendFormat("<span class=\"title\">{0}</span>", xmlNode.Attributes["Title"].Value);
				}
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
