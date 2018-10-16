using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin
{
	public class UploadFile : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			HttpFileCollection files = base.Request.Files;
			if (files.Count > 0)
			{
				string str = HttpContext.Current.Request.MapPath("/Storage/master/flex");
				HttpPostedFile httpPostedFile = files[0];
				string text = Path.GetExtension(httpPostedFile.FileName).ToLower();
				if (text != ".jpg" && text != ".gif" && text != ".jpeg" && text != ".png" && text != ".bmp")
				{
					base.Response.Write("1");
				}
				else
				{
					string str2 = DateTime.Now.ToString("yyyyMMdd") + new Random().Next(10000, 99999).ToString(CultureInfo.InvariantCulture);
					str2 += text;
					string filename = str + "/" + str2;
					try
					{
						httpPostedFile.SaveAs(filename);
						base.Response.Write(str2);
					}
					catch
					{
						base.Response.Write("0");
					}
				}
			}
			else
			{
				base.Response.Write("2");
			}
		}
	}
}
