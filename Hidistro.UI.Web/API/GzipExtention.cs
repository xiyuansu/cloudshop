using System.IO.Compression;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class GzipExtention
	{
		public static void Gzip(HttpContext context)
		{
			string text = context.Request.Headers["Accept-Encoding"].ToString().ToUpperInvariant();
			if (text.Length > 0)
			{
				if (text.Contains("GZIP"))
				{
					context.Response.AppendHeader("Content-encoding", "gzip");
					context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
				}
				else if (text.Contains("DEFLATE"))
				{
					context.Response.AppendHeader("Content-encoding", "deflate");
					context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
				}
			}
		}
	}
}
