using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Store;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetImgList : IHttpHandler
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
			string listJson = this.GetListJson(context);
			context.Response.Write(listJson);
		}

		public string GetListJson(HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\"status\":1,");
			stringBuilder.Append("\"data\":[");
			int num = 1;
			PhotoListOrder order = PhotoListOrder.UpdateTimeDesc;
			if (!string.IsNullOrEmpty(context.Request.Form["sortby"]))
			{
				order = (PhotoListOrder)Enum.Parse(typeof(PhotoListOrder), context.Request.Form["sortby"]);
			}
			int supplierId = 0;
			if (!string.IsNullOrEmpty(context.Request.Form["supplierId"]))
			{
				supplierId = context.Request.Form["supplierId"].ToInt(0);
			}
			DbQueryResult photoList = GalleryHelper.GetPhotoList(context.Request.Form["file_Name"], context.Request.Form["id"].ToInt(0), context.Request.Form["p"].ToInt(0), 36, order, supplierId);
			int pageCount = TemplatePageControl.GetPageCount(photoList.TotalRecords, 36);
			stringBuilder.Append(this.GetImgItemsJson(photoList, context));
			string str = stringBuilder.ToString().TrimEnd(',');
			str += "],";
			str = str + "\"page\": \"" + this.GetPageHtml(pageCount, context) + "\",";
			str += "\"msg\": \"\"";
			return str + "}";
		}

		public string GetPageHtml(int pageCount, HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : context.Request.Form["p"].ToInt(0);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetImgItemsJson(DbQueryResult mamagerRecordset, HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			DataTable data = mamagerRecordset.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"id\":\"" + data.Rows[i]["PhotoId"] + "\",");
				stringBuilder.Append("\"file\":\"" + HiContext.Current.HostPath + data.Rows[i]["PhotoPath"] + "\",");
				stringBuilder.Append("\"name\":\"" + data.Rows[i]["PhotoName"] + "\"");
				stringBuilder.Append("},");
			}
			return stringBuilder.ToString().TrimEnd(',');
		}

		public string GetImgItemJson()
		{
			return "";
		}
	}
}
