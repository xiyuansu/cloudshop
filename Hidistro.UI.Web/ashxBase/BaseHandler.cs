using Newtonsoft.Json;
using System.Web;

namespace Hidistro.UI.Web.ashxBase
{
	public class BaseHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public virtual void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
		}

		public string GetErrorJson(int code, string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				error_response = new
				{
					code = code,
					sub_msg = errorMsg
				}
			});
		}

		public string GetOKJson(string okMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				response = new
				{
					is_success = true,
					sub_msg = okMsg
				}
			});
		}
	}
}
