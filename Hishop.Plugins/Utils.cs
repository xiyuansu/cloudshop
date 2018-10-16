using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace Hishop.Plugins
{
	public static class Utils
	{
		public static string ApplicationPath
		{
			get
			{
				string text = "/";
				if (HttpContext.Current != null)
				{
					text = HttpContext.Current.Request.ApplicationPath;
				}
				if (text == "/")
				{
					return string.Empty;
				}
				return text;
			}
		}

		public static string GetResourceContent(string sFileName)
		{
			try
			{
				string result = "";
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sFileName))
				{
					using (StreamReader streamReader = new StreamReader(stream))
					{
						result = streamReader.ReadToEnd();
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception("Could not read resource \"" + sFileName + "\": " + ex);
			}
		}
	}
}
