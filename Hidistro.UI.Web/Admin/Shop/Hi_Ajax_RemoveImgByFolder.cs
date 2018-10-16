using Hidistro.Core.Entities;
using Hidistro.SaleSystem.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_RemoveImgByFolder : IHttpHandler
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
			context.Response.Write(this.MoveImgByFolder(context));
		}

		public string MoveImgByFolder(HttpContext context)
		{
			DbQueryResult photoList = GalleryHelper.GetPhotoList("", Convert.ToInt32(context.Request.Form["cid"]), 1, 100000000, PhotoListOrder.UploadTimeDesc, 0);
			List<int> list = new List<int>();
			DataTable data = photoList.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				list.Add(Convert.ToInt32(data.Rows[i]["PhotoId"]));
			}
			if (GalleryHelper.MovePhotoType(list, Convert.ToInt32(context.Request.Form["cate_id"])) > 0)
			{
				return "{\"status\":1,\"msg\":\"\"}";
			}
			return "{\"status\":0,\"msg\":\"请选择一个分类\"}";
		}
	}
}
