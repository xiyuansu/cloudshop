using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_Coupons : IHttpHandler
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
			string a = context.Request["action"].ToNullString();
			context.Response.ContentType = "text/plain";
			if (a == "")
			{
				context.Response.Write(this.GetModelJson(context));
			}
			else if (a == "GetAppCouponList")
			{
				context.Response.Write(this.GetAppModelJson(context));
			}
		}

		public string GetAppModelJson(HttpContext context)
		{
			CouponsSearch couponsSearch = new CouponsSearch
			{
				CouponName = ((context.Request.Form["Name"] == null) ? "" : context.Request.Form["Name"].ToNullString()),
				PageIndex = 1,
				IsValid = true,
				ObtainWay = 0
			};
			couponsSearch.PageSize = 1000;
			DataTable noPageCouponInfos = CouponHelper.GetNoPageCouponInfos(couponsSearch);
			int pageCount = 1;
			if (noPageCouponInfos != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetCouponsListJson(noPageCouponInfos, context) + ",";
				str = str + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"";
				return str + "}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetModelJson(HttpContext context)
		{
			DbQueryResult couponsTable = this.GetCouponsTable(context);
			int pageCount = TemplatePageControl.GetPageCount(couponsTable.TotalRecords, 10);
			if (couponsTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetCouponsListJson(couponsTable, context) + ",";
				str = str + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"";
				return str + "}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetPageHtml(int pageCount, HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetCouponsListJson(DbQueryResult CouponsTable, HttpContext context)
		{
			string text = context.Request.Form["client"].ToNullString();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			DataTable data = CouponsTable.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"CouponId\":\"" + data.Rows[i]["CouponId"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + data.Rows[i]["CouponName"].ToString() + "\",");
				stringBuilder.Append("\"Price\":\"" + data.Rows[i]["Price"].ToDecimal(0).F2ToString("f2") + "\",");
				stringBuilder.Append("\"Surplus\":\"" + CouponHelper.GetCouponSurplus(data.Rows[i]["CouponId"].ToInt(0)).ToString() + "\",");
				stringBuilder.Append("\"OrderUseLimit\":\"" + ((data.Rows[i]["OrderUseLimit"].ToDecimal(0) == decimal.Zero) ? "无限制" : ("满" + data.Rows[i]["OrderUseLimit"].ToDecimal(0).F2ToString("f2") + "元使用")) + "\",");
				StringBuilder stringBuilder2 = stringBuilder;
				string[] obj = new string[5]
				{
					"\"Use_time\":\"",
					null,
					null,
					null,
					null
				};
				object obj2;
				DateTime value;
				if (!data.Rows[i]["StartTime"].ToDateTime().HasValue)
				{
					obj2 = "";
				}
				else
				{
					value = data.Rows[i]["StartTime"].ToDateTime().Value;
					obj2 = value.ToString("yyyy.MM.dd");
				}
				obj[1] = (string)obj2;
				obj[2] = "至";
				object obj3;
				if (!data.Rows[i]["ClosingTime"].ToDateTime().HasValue)
				{
					obj3 = "";
				}
				else
				{
					value = data.Rows[i]["ClosingTime"].ToDateTime().Value;
					obj3 = value.ToString("yyyy.MM.dd");
				}
				obj[3] = (string)obj3;
				obj[4] = "\",";
				stringBuilder2.Append(string.Concat(obj));
				stringBuilder.Append("\"type\":\"1\",");
				if (text.ToLower().Trim() == "pctopic")
				{
					stringBuilder.Append("\"link\":\"/CouponDetails.aspx?couponId=" + data.Rows[i]["CouponId"].ToString() + "\"");
				}
				else if (text.ToLower().Trim() == "xcxshop")
				{
					stringBuilder.Append("\"link\":\"../coupondetail/coupondetail?CouponId=" + data.Rows[i]["CouponId"].ToString() + "\"");
				}
				else
				{
					stringBuilder.Append("\"link\":\"/" + text + "/CouponDetails.aspx?couponId=" + data.Rows[i]["CouponId"].ToString() + "\"");
				}
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public string GetCouponsListJson(DataTable dt, HttpContext context)
		{
			string text = context.Request.Form["client"].ToNullString();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"CouponId\":\"" + dt.Rows[i]["CouponId"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + dt.Rows[i]["CouponName"].ToString() + "\",");
				stringBuilder.Append("\"Price\":\"" + dt.Rows[i]["Price"].ToDecimal(0).F2ToString("f2") + "\",");
				stringBuilder.Append("\"Surplus\":\"" + CouponHelper.GetCouponSurplus(dt.Rows[i]["CouponId"].ToInt(0)).ToString() + "\",");
				stringBuilder.Append("\"OrderUseLimit\":\"" + ((dt.Rows[i]["OrderUseLimit"].ToDecimal(0) == decimal.Zero) ? "无限制" : ("满" + dt.Rows[i]["OrderUseLimit"].ToDecimal(0).F2ToString("f2") + "元使用")) + "\",");
				StringBuilder stringBuilder2 = stringBuilder;
				string[] obj = new string[5]
				{
					"\"Use_time\":\"",
					null,
					null,
					null,
					null
				};
				object obj2;
				DateTime value;
				if (!dt.Rows[i]["StartTime"].ToDateTime().HasValue)
				{
					obj2 = "";
				}
				else
				{
					value = dt.Rows[i]["StartTime"].ToDateTime().Value;
					obj2 = value.ToString("yyyy.MM.dd");
				}
				obj[1] = (string)obj2;
				obj[2] = "至";
				object obj3;
				if (!dt.Rows[i]["ClosingTime"].ToDateTime().HasValue)
				{
					obj3 = "";
				}
				else
				{
					value = dt.Rows[i]["ClosingTime"].ToDateTime().Value;
					obj3 = value.ToString("yyyy.MM.dd");
				}
				obj[3] = (string)obj3;
				obj[4] = "\",";
				stringBuilder2.Append(string.Concat(obj));
				stringBuilder.Append("\"type\":\"1\",");
				stringBuilder.Append("\"link\":\"/" + text + "/CouponDetails.aspx?couponId=" + dt.Rows[i]["CouponId"].ToString() + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public DbQueryResult GetCouponsTable(HttpContext context)
		{
			return CouponHelper.GetCouponInfos(this.GetCouponsSearch(context), "");
		}

		public CouponsSearch GetCouponsSearch(HttpContext context)
		{
			return new CouponsSearch
			{
				CouponName = ((context.Request.Form["Name"] == null) ? "" : context.Request.Form["Name"].ToNullString()),
				PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"])),
				IsValid = true,
				ObtainWay = 0
			};
		}
	}
}
