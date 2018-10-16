using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web
{
	public class PicRar : Page
	{
		public string label_html = string.Empty;

		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				string text = string.Empty;
				int maxWidth = 0;
				int maxHeight = 0;
				if (PicRar.IsQueryString("P", "S"))
				{
					text = base.Request.QueryString["P"];
				}
				if (PicRar.IsQueryString("W"))
				{
					maxWidth = int.Parse(base.Request.QueryString["W"]);
				}
				if (PicRar.IsQueryString("H"))
				{
					maxHeight = int.Parse(base.Request.QueryString["H"]);
				}
				if (!(text == string.Empty))
				{
					PIC pIC = new PIC();
					if (!text.StartsWith("/"))
					{
						text = "/" + text;
					}
					text = text;
					pIC.SendSmallImage(base.Request.MapPath(text), maxHeight, maxWidth);
					string watermarkFilename = base.Request.MapPath("/Admin/images/watermark.gif");
					MemoryStream memoryStream = pIC.AddImageSignPic(pIC.OutBMP, watermarkFilename, 9, 60, 4);
					pIC.Dispose();
					base.Response.ClearContent();
					base.Response.ContentType = "image/gif";
					base.Response.BinaryWrite(memoryStream.ToArray());
					base.Response.End();
					memoryStream.Dispose();
				}
			}
			catch (Exception ex)
			{
				this.label_html = ex.Message;
			}
		}

		public static bool IsQueryString(string strQuery, string Q)
		{
			bool result = false;
			if (!(Q == "N"))
			{
				if (Q == "S" && HttpContext.Current.Request.QueryString[strQuery] != null && HttpContext.Current.Request.QueryString[strQuery].ToString() != string.Empty)
				{
					result = true;
				}
			}
			else if (HttpContext.Current.Request.QueryString[strQuery] != null && PicRar.IsNumeric(HttpContext.Current.Request.QueryString[strQuery].ToString()))
			{
				result = true;
			}
			return result;
		}

		public static bool IsQueryString(string strQuery)
		{
			return PicRar.IsQueryString(strQuery, "N");
		}

		public static bool IsNumeric(string strNumeric)
		{
			Regex regex = new Regex("^\\d+$");
			Match match = regex.Match(strNumeric);
			return match.Success;
		}
	}
}
