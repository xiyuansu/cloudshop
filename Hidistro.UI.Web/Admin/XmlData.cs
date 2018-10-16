using Hidistro.SaleSystem.Sales;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	public class XmlData : Page
	{
		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = base.Request.Form["xmlname"];
			string s = base.Request.Form["xmldata"];
			string text2 = base.Request.Form["expressname"];
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && SalesHelper.AddExpressTemplate(text2, text + ".xml"))
			{
				string path = HttpContext.Current.Request.MapPath($"/Storage/master/flex/{text}.xml");
				FileStream fileStream = new FileStream(path, FileMode.Create);
				byte[] bytes = new UTF8Encoding().GetBytes(s);
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Flush();
				fileStream.Close();
			}
		}
	}
}
