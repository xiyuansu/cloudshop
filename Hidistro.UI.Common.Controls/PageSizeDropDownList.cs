using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class PageSizeDropDownList : Literal
	{
		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.WriteLine("<select name=\"pagesize_dropdown\" id=\"pagesize_dropdown\">");
			writer.WriteLine("<option value=\"10\">10</option>");
			writer.WriteLine("<option value=\"20\">20</option>");
			writer.WriteLine("<option value=\"40\">40</option>");
			writer.WriteLine("<option value=\"200\">200</option>");
			writer.WriteLine("<option value=\"500\">500</option>");
			writer.WriteLine("<option value=\"1000\">1000</option>");
			writer.WriteLine("<option value=\"2000\">2000</option>");
			writer.WriteLine("</select>");
			base.RenderControl(writer);
		}
	}
}
