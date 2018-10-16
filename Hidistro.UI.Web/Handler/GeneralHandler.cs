using Hidistro.Context;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class GeneralHandler : IHttpHandler
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
			context.Response.ContentType = "text/json";
			string text = context.Request["action"];
			string a = text;
			if (!(a == "SaveDeliveryScop"))
			{
				if (a == "GetDeliveryScop")
				{
					this.GetDeliveryScop(context);
				}
			}
			else
			{
				this.SaveDeliveryScop(context);
			}
		}

		public void SaveDeliveryScop(HttpContext context)
		{
			int deliveryScopRegionId = 0;
			if (context.Request["RegionID"] != null)
			{
				int.TryParse(context.Request["RegionID"], out deliveryScopRegionId);
				HiContext.Current.DeliveryScopRegionId = deliveryScopRegionId;
				context.Response.Write("{\"status\":\"true\",\"msg\":\"设置成功\"}");
			}
			else
			{
				context.Response.Write("{\"status\":\"false\",\"msg\":\"错误的地区编号\"}");
			}
		}

		public void GetDeliveryScop(HttpContext context)
		{
			string str = $"\"Status\":\"True\",\"DeliveryScopRegionId\":\"{HiContext.Current.DeliveryScopRegionId}\"";
			context.Response.Write("{" + str + "}");
			context.Response.End();
		}
	}
}
