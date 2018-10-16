using Hidistro.Core;
using Hidistro.SaleSystem.Sales;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	public class EditXmlData : Page
	{
		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = base.Request.Form["xmlname"];
			string s = base.Request.Form["xmldata"];
			string text2 = Globals.StripAllTags(base.Request.Form["expressname"]);
			string text3 = Globals.StripAllTags(base.Request.Form["expressid"]);
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3) && text.ToLower().EndsWith(".xml"))
			{
				int expressId = 0;
				if (int.TryParse(text3, out expressId) && SalesHelper.UpdateExpressTemplate(expressId, text2))
				{
					string path = HttpContext.Current.Request.MapPath($"/Storage/master/flex/{text}");
					FileStream fileStream = new FileStream(path, FileMode.Create);
					byte[] bytes = new UTF8Encoding().GetBytes(s);
					fileStream.Write(bytes, 0, bytes.Length);
					fileStream.Flush();
					fileStream.Close();
				}
			}
		}
	}
}
