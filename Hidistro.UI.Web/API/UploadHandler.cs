using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class UploadHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			HttpRequest request = context.Request;
			string text = request["action"];
			string a = text;
			if (!(a == "upload"))
			{
				if (a == "delete")
				{
					this.DeleteImage();
				}
				else
				{
					context.Response.Write("false");
				}
			}
			else
			{
				this.UploadImage();
			}
		}

		private void UploadImage()
		{
			try
			{
				HttpPostedFile httpPostedFile = HttpContext.Current.Request.Files["Filedata"];
				string str = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
				string text = HttpContext.Current.Request["uploadpath"];
				string str2 = str + Path.GetExtension(httpPostedFile.FileName);
				if (string.IsNullOrEmpty(text))
				{
					ClientType clientType = ClientType.VShop;
					string text2 = HttpContext.Current.Request["clientType"];
					if (!string.IsNullOrWhiteSpace(text2))
					{
						clientType = (ClientType)int.Parse(text2);
					}
					text = HiContext.Current.GetCommonSkinPath() + "/master/user/";
					str2 = "imgCustomBg" + Path.GetExtension(httpPostedFile.FileName);
					string[] files = Directory.GetFiles(Globals.MapPath(text), "imgCustomBg.*");
					string[] array = files;
					foreach (string path in array)
					{
						File.Delete(path);
					}
				}
				if (!Directory.Exists(Globals.MapPath(text)))
				{
					Directory.CreateDirectory(Globals.MapPath(text));
				}
				httpPostedFile.SaveAs(Globals.MapPath(text + str2));
				HttpContext.Current.Response.Write(text + str2);
			}
			catch (Exception ex)
			{
				HttpContext.Current.Response.Write("服务器错误" + ex.Message);
				HttpContext.Current.Response.End();
			}
		}

		private void DeleteImage()
		{
			string path = HttpContext.Current.Request.Form["del"];
			string path2 = Globals.PhysicalPath(path);
			try
			{
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
				HttpContext.Current.Response.Write("true");
			}
			catch (Exception)
			{
				HttpContext.Current.Response.Write("false");
			}
		}
	}
}
