using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_Categories : IHttpHandler
	{
		private string dataName = "";

		private StringBuilder listjosn = new StringBuilder();

		private bool isSearch = false;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			this.dataName = context.Request.Form["client"].ToNullString();
			if (string.IsNullOrEmpty(this.dataName))
			{
				this.dataName = "wapshop";
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(HttpContext context)
		{
			IList<CategoryInfo> categoriesTable = this.GetCategoriesTable(context);
			if (categoriesTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGraphicesListJson(categoriesTable, context) + ",";
				str += "\"page\":\"\"";
				return str + "}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetPageHtml(int pageCount, HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"]);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetItemJson(CategoryInfo item)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"item_id\":\"" + item.CategoryId + "\",");
			stringBuilder.Append("\"depth\":\"" + item.Depth + "\",");
			stringBuilder.Append("\"title\":\"" + item.Name + "\",");
			stringBuilder.Append("\"search\":\"" + (this.isSearch ? 1 : 0) + "\",");
			stringBuilder.Append("\"create_time\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",");
			if (this.dataName.ToLower().Trim() == "pctopic")
			{
				stringBuilder.Append("\"link\":\"/browse/category/" + item.CategoryId + "\",");
			}
			else if (this.dataName.ToLower() == "xcxshop")
			{
				stringBuilder.Append("\"link\":\"../searchresult/searchresult?cid=" + item.CategoryId + "\",");
			}
			else if (this.dataName.ToLower() == "apptopic")
			{
				stringBuilder.Append("\"link\":\"javascript:goSerachResult(''," + item.CategoryId + ")\",");
			}
			else if (this.dataName.ToLower() == "appshop")
			{
				stringBuilder.Append("\"link\":\"" + HiContext.Current.HostPath + "/searchresult.html?cid=" + item.CategoryId + "\",");
			}
			else
			{
				stringBuilder.Append("\"link\":\"/" + this.dataName + "/ProductList.aspx?categoryId=" + item.CategoryId + "\",");
			}
			stringBuilder.Append("\"pic\":\"\"");
			stringBuilder.Append("},");
			return stringBuilder.ToString();
		}

		public void LoopToChildren(IList<CategoryInfo> alldata, CategoryInfo curItem, int depth = 1)
		{
			List<CategoryInfo> list = (from c in alldata
			where c.ParentCategoryId == curItem.CategoryId
			select c).ToList();
			foreach (CategoryInfo item in list)
			{
				this.listjosn.Append(this.GetItemJson(item));
				this.LoopToChildren(alldata, item, depth + 1);
			}
		}

		public string GetGraphicesListJson(IList<CategoryInfo> data, HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			if (data != null && data.Count > 0)
			{
				foreach (CategoryInfo datum in data)
				{
					this.listjosn.Append(this.GetItemJson(datum));
				}
			}
			stringBuilder.Append(this.listjosn.ToString());
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public IList<CategoryInfo> GetCategoriesTable(HttpContext context)
		{
			string text = Globals.StripAllTags(context.Request["title"].ToNullString().Trim());
			if (!string.IsNullOrEmpty(text))
			{
				this.isSearch = true;
			}
			return CatalogHelper.GetSequenceCategories(text);
		}

		public CategoriesQuery GetCategoriesSearch(HttpContext context)
		{
			return new CategoriesQuery
			{
				Name = ((context.Request.Form["title"] == null) ? "" : context.Request.Form["title"]),
				PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"])),
				SortOrder = SortAction.Desc,
				SortBy = "CategoryId"
			};
		}
	}
}
