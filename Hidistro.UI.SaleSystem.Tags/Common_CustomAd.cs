using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CustomAd : WebControl
	{
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
			XmlNode xmlNode = TagsHelper.FindAdNode(this.AdId, "custom");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"ad_custom cssEdite\" type=\"custom\" id=\"ads_{0}\" >{1}</div>", this.AdId, Globals.HtmlDecode(xmlNode.Attributes["Html"].Value)).AppendLine();
			}
			return stringBuilder.ToString();
		}
	}
}
