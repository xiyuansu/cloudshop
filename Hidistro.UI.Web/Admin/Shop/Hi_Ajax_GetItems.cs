using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetItems : IHttpHandler
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
			string text = context.Request.Form["p"];
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(HttpContext context)
		{
			string text = context.Request["client"];
			int clientType = 1;
			if (!string.IsNullOrEmpty(text) && text.ToLower().Trim() == "appshop")
			{
				clientType = 2;
			}
			else if (!string.IsNullOrEmpty(text) && text.ToLower().Trim() == "appshoptopic")
			{
				clientType = 3;
			}
			else if (!string.IsNullOrEmpty(text) && text.ToLower().Trim() == "xcxshop")
			{
				clientType = 4;
			}
			DbQueryResult goodsTable = this.GetGoodsTable(context, clientType);
			int pageCount = TemplatePageControl.GetPageCount(goodsTable.TotalRecords, 8);
			if (goodsTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetGoodsListJson(goodsTable, clientType) + ",";
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

		public string GetGoodsListJson(DbQueryResult GoodsTable, int ClientType)
		{
			AppVersionRecordInfo latestAppVersionRecord = APPHelper.GetLatestAppVersionRecord("android");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"list\":[");
			DataTable data = GoodsTable.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"item_id\":\"" + data.Rows[i]["ProductId"] + "\",");
				stringBuilder.Append("\"title\":\"" + data.Rows[i]["ProductName"] + "\",");
				stringBuilder.Append("\"price\":\"" + data.Rows[i]["SalePrice"].ToDecimal(0).F2ToString("f2") + "\",");
				stringBuilder.Append("\"original_price\":\"" + data.Rows[i]["MarketPrice"].ToDecimal(0).F2ToString("f2") + "\",");
				stringBuilder.Append("\"create_time\":\"" + Convert.ToDateTime(data.Rows[i]["AddedDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "\",");
				switch (ClientType)
				{
				case 2:
					if (latestAppVersionRecord == null || latestAppVersionRecord.Version.ToDecimal_MoreDot(0) > 3.3m)
					{
						stringBuilder.Append("\"link\":\"" + HiContext.Current.HostPath + "/productdetail.html?id=" + data.Rows[i]["ProductId"] + "\",");
					}
					else
					{
						stringBuilder.Append("\"link\":\"hishop://webShowProduct/null/" + data.Rows[i]["ProductId"] + "\",");
					}
					break;
				case 1:
					stringBuilder.Append("\"link\":\"/ProductDetails?productId=" + data.Rows[i]["ProductId"] + "\",");
					break;
				case 4:
					stringBuilder.Append("\"link\":\"../productdetail/productdetail?id=" + data.Rows[i]["ProductId"] + "\",");
					break;
				default:
					stringBuilder.Append("\"link\":\"javascript:showProductDetail(" + data.Rows[i]["ProductId"] + ")\",");
					break;
				}
				string empty = string.Empty;
				empty = ((!string.IsNullOrEmpty(data.Rows[i]["ThumbnailUrl310"].ToString())) ? data.Rows[i]["ThumbnailUrl310"].ToString() : SettingsManager.GetMasterSettings().DefaultProductImage);
				if ((empty.IndexOf("http") < 0 || empty.IndexOf("https") < 0) && ClientType == 4)
				{
					empty = Globals.FullPath(empty);
				}
				stringBuilder.Append("\"pic\":\"" + empty + "\",");
				stringBuilder.Append("\"is_compress\":0");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(',');
			return str + "]";
		}

		public DbQueryResult GetGoodsTable(HttpContext context, int clientType)
		{
			return ProductHelper.GetProducts(this.GetProductQuery(context, clientType));
		}

		public ProductQuery GetProductQuery(HttpContext context, int clientType)
		{
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = context.Request.Form["title"],
				PageSize = 8,
				PageIndex = ((context.Request.Form["p"] == null) ? 1 : Convert.ToInt32(context.Request.Form["p"])),
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				SaleStatus = (ProductSaleStatus)((context.Request.Form["status"] == null) ? 1 : Convert.ToInt32(context.Request.Form["status"]))
			};
			ProductQuery productQuery2 = productQuery;
			ProductType productType;
			int hashCode;
			if (clientType != 2 && clientType != 3 && clientType != 4)
			{
				productType = ProductType.All;
				hashCode = productType.GetHashCode();
			}
			else
			{
				productType = ProductType.PhysicalProduct;
				hashCode = productType.GetHashCode();
			}
			productQuery2.ProductType = hashCode;
			if (!string.IsNullOrEmpty(context.Request.Form["category"]))
			{
				productQuery.CategoryId = context.Request.Form["category"].ToInt(0);
				CategoryInfo category = CatalogHelper.GetCategory(productQuery.CategoryId.Value);
				productQuery.MaiCategoryPath = category.Path;
			}
			string absolutePath = context.Request.UrlReferrer.AbsolutePath;
			return productQuery;
		}
	}
}
