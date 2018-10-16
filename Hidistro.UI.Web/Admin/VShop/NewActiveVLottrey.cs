using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class NewActiveVLottrey : IHttpHandler
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
			string text = context.Request["act"];
			string a = text;
			if (!(a == "GetYHQ"))
			{
				if (!(a == "GetLP"))
				{
					if (a == "addData")
					{
						this.addData(context);
					}
				}
				else
				{
					this.GetLP(context);
				}
			}
			else
			{
				this.GetYHQ(context);
			}
		}

		public void addData(HttpContext context)
		{
			string input = context.Request["itmes"];
			List<ActivityAwardItemInfo> list = new List<ActivityAwardItemInfo>();
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			list = javaScriptSerializer.Deserialize<List<ActivityAwardItemInfo>>(input);
		}

		public void GetYHQ(HttpContext context)
		{
			string text = context.Request["StartDate"];
			List<CouponInfo> allCoupons = CouponHelper.GetAllCoupons();
			StringBuilder stringBuilder = new StringBuilder();
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			javaScriptSerializer.Serialize(allCoupons, stringBuilder);
			context.Response.Write(stringBuilder);
			context.Response.End();
		}

		public void GetLP(HttpContext context)
		{
			List<GiftInfo> allGift = GiftHelper.GetAllGift();
			StringBuilder stringBuilder = new StringBuilder();
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			javaScriptSerializer.Serialize(allGift, stringBuilder);
			context.Response.Write(stringBuilder);
			context.Response.End();
		}
	}
}
