using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_Brands : IHttpHandler
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
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(HttpContext context)
		{
			DbQueryResult brandsTable = this.GetBrandsTable(context);
			int pageCount = TemplatePageControl.GetPageCount(brandsTable.TotalRecords, 10);
			if (brandsTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGraphicesListJson(brandsTable, context) + ",";
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

		public string GetGraphicesListJson(DbQueryResult GraphicesTable, HttpContext context)
		{
			string text = context.Request.Form["client"];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			DataTable data = GraphicesTable.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"item_id\":\"" + data.Rows[i]["BrandId"] + "\",");
				stringBuilder.Append("\"title\":\"" + data.Rows[i]["BrandName"] + "\",");
				stringBuilder.Append("\"create_time\":\"" + DateTime.Now + "\",");
				if (text.ToLower().Trim() == "pctopic")
				{
					stringBuilder.Append("\"link\":\"/brand/brand_detail/" + data.Rows[i]["BrandId"] + "\",");
				}
				else
				{
					stringBuilder.Append("\"link\":\"/" + context.Request["client"] + "/BrandDetail.aspx?BrandId=" + data.Rows[i]["BrandId"] + "\",");
				}
				stringBuilder.Append("\"pic\":\"" + data.Rows[i]["Logo"] + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public DbQueryResult GetBrandsTable(HttpContext context)
		{
			return CatalogHelper.GetBrandQuery(this.GetBrandSearch(context));
		}

		public BrandQuery GetBrandSearch(HttpContext context)
		{
			return new BrandQuery
			{
				Name = ((context.Request.Form["title"] == null) ? "" : context.Request.Form["title"]),
				PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"])),
				SortOrder = SortAction.Desc,
				SortBy = "BrandId"
			};
		}
	}
}
