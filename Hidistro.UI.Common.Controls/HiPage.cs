using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class HiPage : Page
	{
		private static readonly Regex viewStateRegex = new Regex("<div>(\\s+)<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(?<data>.*?)\" />(\\s+)</div>(\\r\\n)+", RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.EnableViewState)
			{
				base.Render(writer);
			}
			else
			{
				using (StringWriter stringWriter = new StringWriter())
				{
					using (HtmlTextWriter writer2 = new HtmlTextWriter(stringWriter))
					{
						base.Render(writer2);
						string text = stringWriter.ToString();
						Match match = HiPage.viewStateRegex.Match(text);
						if (match.Success)
						{
							text = text.Remove(match.Index, match.Length);
						}
						writer.Write(text);
					}
				}
			}
			writer.WriteLine();
		}
	}
}
