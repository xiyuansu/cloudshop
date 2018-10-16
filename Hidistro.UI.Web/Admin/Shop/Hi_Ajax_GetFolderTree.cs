using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetFolderTree : IHttpHandler
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
			context.Response.Write(this.GetTreeListJson(context));
		}

		public string GetTreeListJson(HttpContext context)
		{
			try
			{
				int supplierId = 0;
				if (!string.IsNullOrEmpty(context.Request.Form["supplierId"]))
				{
					supplierId = context.Request.Form["supplierId"].ToInt(0);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{\"status\":1,");
				stringBuilder.Append("\"data\":{");
				stringBuilder.Append("\"total\":" + GalleryHelper.GetPhotoList("", 0, 10, PhotoListOrder.UploadTimeDesc, supplierId).TotalRecords + ",");
				stringBuilder.Append("\"tree\":[");
				stringBuilder.Append(this.GetImgTypeJson(supplierId));
				stringBuilder.Append("]");
				stringBuilder.Append("},");
				stringBuilder.Append("\"msg\":\"\"");
				stringBuilder.Append("}");
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				NameValueCollection param = new NameValueCollection
				{
					context.Request.Form,
					context.Request.QueryString
				};
				Globals.WriteExceptionLog_Page(ex, param, "GetTreeListJson");
				return "";
			}
		}

		public string GetImgTypeJson(int supplierId)
		{
			string text = "";
			DataTable photoCategories = GalleryHelper.GetPhotoCategories(supplierId);
			for (int i = 0; i < photoCategories.Rows.Count; i++)
			{
				text += "{";
				text = text + "\"name\":\"" + photoCategories.Rows[i]["CategoryName"] + "\",";
				text += "\"parent_id\":0,";
				text = text + "\"id\":" + photoCategories.Rows[i]["CategoryId"] + ",";
				text += "\"picNum\":" + photoCategories.Rows[i]["PhotoCounts"];
				text += "},";
			}
			return text.TrimEnd(',');
		}
	}
}
