using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.SaleSystem.Commodities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class TaobaoProductHandler : IHttpHandler
	{
		private HttpContext myContext;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			this.myContext = context;
			context.Response.ContentType = "text/plain";
			GzipExtention.Gzip(context);
			string text = context.Request.Form["action"];
			switch (text)
			{
			case "ProductSearch":
				this.ProcessProductSearch(context);
				break;
			case "ProductDetails":
				this.ProcessProductDetails(context);
				break;
			case "TaobaoProductMake":
				this.ProcessTaobaoProductMake(context);
				break;
			case "TaobaoProductDetails":
				this.ProcessTaobaoProductDetails(context);
				break;
			case "TaobaoProductIdAdd":
				this.ProcessTaobaoProductIdAdd(context);
				break;
			case "TaobaoProductIsExit":
				this.ProcessTaobaoProductIsExit(context);
				break;
			case "TaobaoProductDown":
				this.ProcessTaobaoProductDown(context);
				break;
			default:
				context.Response.Write("error");
				break;
			}
		}

		private void ProcessProductSearch(HttpContext context)
		{
			try
			{
				string str = "http://" + HttpContext.Current.Request.Url.Host;
				ProductQuery productQuery = new ProductQuery();
				productQuery.PageIndex = int.Parse(context.Request.Form["pageIndex"]);
				productQuery.PageSize = int.Parse(context.Request.Form["pageSize"]);
				productQuery.Keywords = context.Request.Form["productName"];
				productQuery.ProductCode = context.Request.Form["productCode"];
				if (!string.IsNullOrEmpty(context.Request.Form["publishStatus"]))
				{
					productQuery.PublishStatus = (PublishStatus)int.Parse(context.Request.Form["publishStatus"]);
				}
				if (!string.IsNullOrEmpty(context.Request.Form["IsMakeTaobao"]))
				{
					productQuery.IsMakeTaobao = int.Parse(context.Request.Form["IsMakeTaobao"]);
				}
				if (!string.IsNullOrEmpty(context.Request.Form["startDate"]))
				{
					productQuery.StartDate = DateTime.Parse(context.Request.Form["startDate"]);
				}
				if (!string.IsNullOrEmpty(context.Request.Form["endDate"]))
				{
					productQuery.EndDate = DateTime.Parse(context.Request.Form["endDate"]);
				}
				Globals.EntityCoding(productQuery, true);
				DbQueryResult toTaobaoProducts = ProductHelper.GetToTaobaoProducts(productQuery);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Products\":[");
				DataTable data = toTaobaoProducts.Data;
				if (data.Rows.Count > 0)
				{
					string text = "";
					foreach (DataRow row in data.Rows)
					{
						stringBuilder.Append("{");
						stringBuilder.AppendFormat("\"ProductId\":{0},", row["ProductId"]);
						stringBuilder.AppendFormat("\"ProductDetailLink\":\"{0}\",", str + RouteConfig.GetRouteUrl(this.myContext, "productDetails", new
						{
							ProductId = row["ProductId"]
						}));
						stringBuilder.AppendFormat("\"DisplaySequence\":{0},", row["DisplaySequence"]);
						text = ((row["ThumbnailUrl40"] == DBNull.Value) ? HiContext.Current.SiteSettings.DefaultProductImage : ((string)row["ThumbnailUrl40"]));
						if (!text.StartsWith("http://") && !text.StartsWith("https://"))
						{
							text = str + text;
						}
						stringBuilder.AppendFormat("\"ThumbnailUrl40\":\"{0}\",", text);
						stringBuilder.AppendFormat("\"ProductName\":\"{0}\",", HttpUtility.UrlEncode(row["ProductName"].ToString()));
						stringBuilder.AppendFormat("\"ProductCode\":\"{0}\",", row["ProductCode"]);
						stringBuilder.AppendFormat("\"Stock\":{0},", row["Stock"]);
						stringBuilder.AppendFormat("\"MarketPrice\":{0},", (row["MarketPrice"] != DBNull.Value) ? ((decimal)row["MarketPrice"]).F2ToString("f2") : "0");
						stringBuilder.AppendFormat("\"SalePrice\":{0},", (row["SalePrice"] != DBNull.Value) ? ((decimal)row["SalePrice"]).F2ToString("f2") : "0");
						stringBuilder.AppendFormat("\"IsMakeTaobao\":{0},", (row["IsMakeTaobao"] != DBNull.Value) ? ((int)row["IsMakeTaobao"]) : 0);
						stringBuilder.AppendFormat("\"TaobaoProductId\":{0}", (row["TaobaoProductId"] != DBNull.Value) ? ((long)row["TaobaoProductId"]) : 0);
						stringBuilder.Append("},");
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
				stringBuilder.Append("],");
				stringBuilder.AppendFormat("\"TotalResults\":{0}", toTaobaoProducts.TotalRecords);
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				NameValueCollection nameValueCollection = new NameValueCollection
				{
					HttpContext.Current.Request.Form,
					HttpContext.Current.Request.QueryString
				};
				nameValueCollection.Add("ErrorMessage", ex.Message);
				nameValueCollection.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					nameValueCollection.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					nameValueCollection.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					nameValueCollection.Add("TargetSite", ex.TargetSite.ToString());
				}
				nameValueCollection.Add("ExSource", ex.Source);
				Globals.AppendLog(nameValueCollection, "", "", HttpContext.Current.Request.Url.ToString(), "TaobaoProduct");
			}
		}

		private void ProcessProductDetails(HttpContext context)
		{
			try
			{
				int productId = int.Parse(context.Request.Form["productId"]);
				DataSet taobaoProductDetails = ProductHelper.GetTaobaoProductDetails(productId);
				StringBuilder stringBuilder = new StringBuilder();
				DataTable dataTable = taobaoProductDetails.Tables[0];
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"HasSKU\":\"{0}\",", dataTable.Rows[0]["HasSKU"]);
				stringBuilder.AppendFormat("\"ProductName\":\"{0}\",", HttpUtility.UrlEncode(dataTable.Rows[0]["ProductName"].ToString()));
				stringBuilder.AppendFormat("\"ProductCode\":\"{0}\",", dataTable.Rows[0]["ProductCode"]);
				stringBuilder.AppendFormat("\"CategoryName\":\"{0}\",", dataTable.Rows[0]["CategoryName"]);
				stringBuilder.AppendFormat("\"BrandName\":\"{0}\",", dataTable.Rows[0]["BrandName"]);
				stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", dataTable.Rows[0]["SalePrice"]);
				stringBuilder.AppendFormat("\"MarketPrice\":\"{0}\",", (dataTable.Rows[0]["MarketPrice"] == DBNull.Value) ? "0.00" : dataTable.Rows[0]["MarketPrice"]);
				stringBuilder.AppendFormat("\"CostPrice\":\"{0}\",", (dataTable.Rows[0]["CostPrice"] == DBNull.Value) ? "0.00" : dataTable.Rows[0]["CostPrice"]);
				stringBuilder.AppendFormat("\"Stock\":\"{0}\",", dataTable.Rows[0]["Stock"]);
				stringBuilder.AppendFormat("\"Attributes\":\"{0}\",", this.GetProductAttribute(taobaoProductDetails.Tables[1]));
				stringBuilder.AppendFormat("\"Skus\":\"{0}\"", this.GetProductSKU(taobaoProductDetails.Tables[2]));
				DataTable dataTable2 = taobaoProductDetails.Tables[3];
				if (dataTable2.Rows.Count > 0)
				{
					stringBuilder.AppendFormat(",\"Cid\":\"{0}\",", dataTable2.Rows[0]["Cid"]);
					stringBuilder.AppendFormat("\"StuffStatus\":\"{0}\",", dataTable2.Rows[0]["StuffStatus"]);
					stringBuilder.AppendFormat("\"ProTitle\":\"{0}\",", dataTable2.Rows[0]["ProTitle"]);
					stringBuilder.AppendFormat("\"Num\":\"{0}\",", dataTable2.Rows[0]["Num"]);
					stringBuilder.AppendFormat("\"LocationState\":\"{0}\",", dataTable2.Rows[0]["LocationState"]);
					stringBuilder.AppendFormat("\"LocationCity\":\"{0}\",", dataTable2.Rows[0]["LocationCity"]);
					stringBuilder.AppendFormat("\"FreightPayer\":\"{0}\",", dataTable2.Rows[0]["FreightPayer"]);
					stringBuilder.AppendFormat("\"PostFee\":\"{0}\",", dataTable2.Rows[0]["PostFee"]);
					stringBuilder.AppendFormat("\"ExpressFee\":\"{0}\",", dataTable2.Rows[0]["ExpressFee"]);
					stringBuilder.AppendFormat("\"EMSFee\":\"{0}\",", dataTable2.Rows[0]["EMSFee"]);
					stringBuilder.AppendFormat("\"HasInvoice\":\"{0}\",", dataTable2.Rows[0]["HasInvoice"]);
					stringBuilder.AppendFormat("\"HasWarranty\":\"{0}\",", dataTable2.Rows[0]["HasWarranty"]);
					stringBuilder.AppendFormat("\"HasDiscount\":\"{0}\",", dataTable2.Rows[0]["HasDiscount"]);
					stringBuilder.AppendFormat("\"FoodAttributes\":\"{0}\",", HttpUtility.UrlEncode(dataTable2.Rows[0]["FoodAttributes"].ToString()));
					stringBuilder.AppendFormat("\"PropertyAlias\":\"{0}\",", dataTable2.Rows[0]["PropertyAlias"]);
					stringBuilder.AppendFormat("\"SkuProperties\":\"{0}\",", dataTable2.Rows[0]["SkuProperties"]);
					stringBuilder.AppendFormat("\"SkuQuantities\":\"{0}\",", dataTable2.Rows[0]["SkuQuantities"]);
					stringBuilder.AppendFormat("\"SkuPrices\":\"{0}\",", dataTable2.Rows[0]["SkuPrices"]);
					stringBuilder.AppendFormat("\"SkuOuterIds\":\"{0}\",", dataTable2.Rows[0]["SkuOuterIds"]);
					stringBuilder.AppendFormat("\"Inputpids\":\"{0}\",", dataTable2.Rows[0]["Inputpids"]);
					stringBuilder.AppendFormat("\"Inputstr\":\"{0}\"", dataTable2.Rows[0]["Inputstr"]);
				}
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				NameValueCollection nameValueCollection = new NameValueCollection
				{
					HttpContext.Current.Request.Form,
					HttpContext.Current.Request.QueryString
				};
				nameValueCollection.Add("ErrorMessage", ex.Message);
				nameValueCollection.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					nameValueCollection.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					nameValueCollection.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					nameValueCollection.Add("TargetSite", ex.TargetSite.ToString());
				}
				nameValueCollection.Add("ExSource", ex.Source);
				Globals.AppendLog(nameValueCollection, "", "", HttpContext.Current.Request.Url.ToString(), "TaobaoProduct");
			}
		}

		private string GetProductAttribute(DataTable productAttribute)
		{
			string text = string.Empty;
			if (productAttribute.Rows.Count > 0)
			{
				foreach (DataRow row in productAttribute.Rows)
				{
					text = text + row["AttributeName"] + ":" + row["ValueStr"] + ";";
				}
				text = text.Remove(text.Length - 1);
			}
			return text;
		}

		private string GetProductSKU(DataTable productSKU)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (productSKU.Rows.Count > 0)
			{
				for (int num = productSKU.Columns.Count - 1; num >= 0; num--)
				{
					if (productSKU.Columns[num].ColumnName != "SkuId")
					{
						stringBuilder2.Append(productSKU.Columns[num].ColumnName).Append(";");
					}
				}
				foreach (DataRow row in productSKU.Rows)
				{
					for (int num2 = productSKU.Columns.Count - 1; num2 >= 0; num2--)
					{
						if (productSKU.Columns[num2].ColumnName != "SkuId")
						{
							stringBuilder.Append(row[productSKU.Columns[num2].ColumnName]).Append(";");
						}
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1).Append(",");
				}
				stringBuilder2.Remove(stringBuilder2.Length - 1, 1).Append(",").Append(stringBuilder.Remove(stringBuilder.Length - 1, 1));
			}
			return stringBuilder2.ToString();
		}

		private void ProcessTaobaoProductMake(HttpContext context)
		{
			try
			{
				long num = 0L;
				long.TryParse(context.Request.Form["Num"].ToNullString(), out num);
				TaobaoProductInfo taobaoProduct = this.GetTaobaoProduct(context);
				taobaoProduct.ProductId = int.Parse(context.Request.Form["ProductId"]);
				taobaoProduct.ProTitle = context.Request.Form["ProTitle"];
				taobaoProduct.Num = num;
				taobaoProduct.FoodAttributes = context.Request.Form["FoodAttributes"];
				bool flag = ProductHelper.UpdateToaobProduct(taobaoProduct);
				context.Response.Write(flag.ToString());
			}
			catch (Exception ex)
			{
				NameValueCollection nameValueCollection = new NameValueCollection
				{
					HttpContext.Current.Request.Form,
					HttpContext.Current.Request.QueryString
				};
				nameValueCollection.Add("ErrorMessage", ex.Message);
				nameValueCollection.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					nameValueCollection.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					nameValueCollection.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					nameValueCollection.Add("TargetSite", ex.TargetSite.ToString());
				}
				nameValueCollection.Add("ExSource", ex.Source);
				Globals.AppendLog(nameValueCollection, "", "", HttpContext.Current.Request.Url.ToString(), "TaobaoProduct");
			}
		}

		private void ProcessTaobaoProductDetails(HttpContext context)
		{
			string text = "http://" + HttpContext.Current.Request.Url.Host;
			int productId = int.Parse(context.Request.Form["productId"]);
			PublishToTaobaoProductInfo taobaoProduct = ProductHelper.GetTaobaoProduct(productId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.AppendFormat("\"Cid\":\"{0}\",", taobaoProduct.Cid);
			stringBuilder.AppendFormat("\"StuffStatus\":\"{0}\",", taobaoProduct.StuffStatus);
			stringBuilder.AppendFormat("\"ProductId\":\"{0}\",", taobaoProduct.ProductId);
			stringBuilder.AppendFormat("\"ProTitle\":\"{0}\",", HttpUtility.UrlEncode(taobaoProduct.ProTitle));
			stringBuilder.AppendFormat("\"Num\":\"{0}\",", taobaoProduct.Num);
			stringBuilder.AppendFormat("\"LocationState\":\"{0}\",", taobaoProduct.LocationState);
			stringBuilder.AppendFormat("\"LocationCity\":\"{0}\",", taobaoProduct.LocationCity);
			stringBuilder.AppendFormat("\"FreightPayer\":\"{0}\",", taobaoProduct.FreightPayer);
			stringBuilder.AppendFormat("\"PostFee\":\"{0}\",", taobaoProduct.PostFee.F2ToString("f2"));
			stringBuilder.AppendFormat("\"ExpressFee\":\"{0}\",", taobaoProduct.ExpressFee.F2ToString("f2"));
			stringBuilder.AppendFormat("\"EMSFee\":\"{0}\",", taobaoProduct.EMSFee.F2ToString("f2"));
			stringBuilder.AppendFormat("\"HasInvoice\":\"{0}\",", taobaoProduct.HasInvoice);
			stringBuilder.AppendFormat("\"HasWarranty\":\"{0}\",", taobaoProduct.HasWarranty);
			stringBuilder.AppendFormat("\"HasDiscount\":\"{0}\",", taobaoProduct.HasDiscount);
			stringBuilder.AppendFormat("\"ValidThru\":\"{0}\",", taobaoProduct.ValidThru);
			stringBuilder.AppendFormat("\"ListTime\":\"{0}\",", taobaoProduct.ListTime);
			stringBuilder.AppendFormat("\"PropertyAlias\":\"{0}\",", taobaoProduct.PropertyAlias);
			stringBuilder.AppendFormat("\"InputPids\":\"{0}\",", taobaoProduct.InputPids);
			stringBuilder.AppendFormat("\"InputStr\":\"{0}\",", taobaoProduct.InputStr);
			stringBuilder.AppendFormat("\"SkuProperties\":\"{0}\",", taobaoProduct.SkuProperties);
			stringBuilder.AppendFormat("\"SkuQuantities\":\"{0}\",", taobaoProduct.SkuQuantities);
			stringBuilder.AppendFormat("\"SkuPrices\":\"{0}\",", taobaoProduct.SkuPrices);
			stringBuilder.AppendFormat("\"SkuOuterIds\":\"{0}\",", taobaoProduct.SkuOuterIds);
			stringBuilder.AppendFormat("\"TaobaoProductId\":\"{0}\",", taobaoProduct.TaobaoProductId);
			stringBuilder.AppendFormat("\"ProductCode\":\"{0}\",", taobaoProduct.ProductCode);
			if (string.IsNullOrEmpty(taobaoProduct.Description))
			{
				taobaoProduct.Description = "暂无描述信息";
			}
			stringBuilder.AppendFormat("\"Description\":\"{0}\",", HttpUtility.UrlEncode(taobaoProduct.Description.Replace("src=\"/Storage/master/gallery", $"src=\"{text}/Storage/master/gallery").Replace('"', '“')));
			string text2 = text + taobaoProduct.ImageUrl1;
			if (File.Exists(Globals.MapPath(taobaoProduct.ImageUrl1)) && (text2.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || text2.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) || text2.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || text2.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName1\":\"{0}\",", text2);
			}
			string text3 = text + taobaoProduct.ImageUrl2;
			if (File.Exists(Globals.MapPath(taobaoProduct.ImageUrl2)) && (text3.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || text3.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) || text3.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || text3.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName2\":\"{0}\",", text3);
			}
			string text4 = text + taobaoProduct.ImageUrl3;
			if (File.Exists(Globals.MapPath(taobaoProduct.ImageUrl3)) && (text4.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || text4.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) || text4.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || text4.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName3\":\"{0}\",", text4);
			}
			string text5 = text + taobaoProduct.ImageUrl4;
			if (File.Exists(Globals.MapPath(taobaoProduct.ImageUrl4)) && (text5.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || text5.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) || text5.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || text5.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName4\":\"{0}\",", text5);
			}
			string text6 = text + taobaoProduct.ImageUrl5;
			if (File.Exists(Globals.MapPath(taobaoProduct.ImageUrl5)) && (text6.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || text6.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) || text6.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || text6.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase)))
			{
				stringBuilder.AppendFormat("\"ImageName5\":\"{0}\",", text6);
			}
			stringBuilder.AppendFormat("\"SalePrice\":\"{0}\",", taobaoProduct.SalePrice.F2ToString("f2"));
			stringBuilder.AppendFormat("\"FoodAttributes\":\"{0}\"", HttpUtility.UrlEncode(taobaoProduct.FoodAttributes));
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
		}

		private void ProcessTaobaoProductIdAdd(HttpContext context)
		{
			int productId = int.Parse(context.Request.Form["productId"]);
			long taobaoProductId = long.Parse(context.Request.Form["taobaoProductId"]);
			bool flag = ProductHelper.UpdateTaobaoProductId(productId, taobaoProductId);
			context.Response.Write(flag.ToString());
		}

		private void ProcessTaobaoProductIsExit(HttpContext context)
		{
			long taobaoProductId = long.Parse(context.Request.Form["taobaoProductId"]);
			bool flag = ProductHelper.IsExitTaobaoProduct(taobaoProductId);
			context.Response.Write(flag.ToString());
		}

		private void ProcessTaobaoProductDown(HttpContext context)
		{
			long taobaoProductId = 0L;
			if (long.TryParse(context.Request.Form["TaobaoProductId"], out taobaoProductId) && ProductHelper.IsExitTaobaoProduct(taobaoProductId))
			{
				return;
			}
			ProductInfo productInfo = new ProductInfo();
			productInfo.AuditStatus = ProductAuditStatus.Pass;
			productInfo.CategoryId = 0;
			productInfo.BrandId = 0;
			productInfo.ProductName = HttpUtility.UrlDecode(context.Request.Form["ProductName"]).ToString().Replace("\\", "");
			productInfo.ProductCode = context.Request.Form["ProductCode"];
			productInfo.Description = HttpUtility.UrlDecode(context.Request.Form["Description"]).ToString().Replace("\\", "");
			if (context.Request.Form["SaleStatus"] == "onsale")
			{
				productInfo.SaleStatus = ProductSaleStatus.OnSale;
			}
			else
			{
				productInfo.SaleStatus = ProductSaleStatus.OnStock;
			}
			productInfo.AddedDate = DateTime.Parse(context.Request.Form["AddedDate"]);
			productInfo.TaobaoProductId = taobaoProductId;
			string text = context.Request.Form["ImageUrls"];
			if (!string.IsNullOrEmpty(text))
			{
				this.DownloadImage(productInfo, text, context);
			}
			productInfo.TypeId = ProductTypeHelper.GetTypeId(context.Request.Form["TypeName"]);
			decimal weight = decimal.Parse(context.Request.Form["Weight"]);
			Dictionary<string, SKUItem> skus = this.GetSkus(productInfo, weight, context);
			ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, skus, null, null, null, true, "");
			if (productActionStatus == ProductActionStatus.Success)
			{
				TaobaoProductInfo taobaoProduct = this.GetTaobaoProduct(context);
				taobaoProduct.ProductId = productInfo.ProductId;
				taobaoProduct.ProTitle = productInfo.ProductName;
				taobaoProduct.Num = productInfo.Stock;
				ProductHelper.UpdateToaobProduct(taobaoProduct);
			}
			context.Response.ContentType = "text/string";
			context.Response.Write(productActionStatus.ToString());
		}

		private void DownloadImage(ProductInfo product, string imageUrls, HttpContext context)
		{
			imageUrls = HttpUtility.UrlDecode(imageUrls);
			string[] array = imageUrls.Split(';');
			int num = 1;
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = string.Format("/Storage/master/product/images/{0}", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + text.Substring(text.LastIndexOf('.')));
				string text3 = text2.Replace("/images/", "/thumbs40/40_");
				string text4 = text2.Replace("/images/", "/thumbs60/60_");
				string text5 = text2.Replace("/images/", "/thumbs100/100_");
				string text6 = text2.Replace("/images/", "/thumbs160/160_");
				string text7 = text2.Replace("/images/", "/thumbs180/180_");
				string text8 = text2.Replace("/images/", "/thumbs220/220_");
				string text9 = text2.Replace("/images/", "/thumbs310/310_");
				string text10 = text2.Replace("/images/", "/thumbs410/410_");
				string text11 = HttpContext.Current.Request.MapPath(text2);
				WebClient webClient = new WebClient();
				try
				{
					webClient.DownloadFile(text, text11);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text3), 40, 40);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text4), 60, 60);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text5), 100, 100);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text6), 160, 160);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text7), 180, 180);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text8), 220, 220);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text9), 310, 310);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(text10), 410, 410);
					switch (num)
					{
					case 1:
						product.ImageUrl1 = text2;
						product.ThumbnailUrl40 = text3;
						product.ThumbnailUrl60 = text4;
						product.ThumbnailUrl100 = text5;
						product.ThumbnailUrl160 = text6;
						product.ThumbnailUrl180 = text7;
						product.ThumbnailUrl220 = text8;
						product.ThumbnailUrl310 = text9;
						product.ThumbnailUrl410 = text10;
						break;
					case 2:
						product.ImageUrl2 = text2;
						break;
					case 3:
						product.ImageUrl3 = text2;
						break;
					case 4:
						product.ImageUrl4 = text2;
						break;
					case 5:
						product.ImageUrl5 = text2;
						break;
					}
					num++;
				}
				catch
				{
				}
			}
		}

		private Dictionary<string, SKUItem> GetSkus(ProductInfo product, decimal weight, HttpContext context)
		{
			Dictionary<string, SKUItem> dictionary = null;
			string text = context.Request.Form["SkuString"];
			if (string.IsNullOrEmpty(text))
			{
				product.HasSKU = false;
				dictionary = new Dictionary<string, SKUItem>
				{
					{
						"0",
						new SKUItem
						{
							SkuId = "0",
							SKU = product.ProductCode,
							SalePrice = decimal.Parse(context.Request.Form["SalePrice"]),
							CostPrice = decimal.Zero,
							Stock = int.Parse(context.Request.Form["Stock"]),
							Weight = weight
						}
					}
				};
			}
			else
			{
				product.HasSKU = true;
				dictionary = new Dictionary<string, SKUItem>();
				text = HttpUtility.UrlDecode(text);
				string[] array = text.Split('|');
				foreach (string text2 in array)
				{
					string[] array2 = text2.Split(',');
					SKUItem sKUItem = new SKUItem();
					sKUItem.SKU = array2[0].Replace("_", "-");
					sKUItem.Weight = weight;
					sKUItem.Stock = int.Parse(array2[1]);
					sKUItem.SalePrice = decimal.Parse(array2[2]);
					string text3 = array2[3];
					string text4 = "";
					string[] array3 = text3.Split(';');
					foreach (string text5 in array3)
					{
						string[] array4 = text5.Split(':');
						int specificationId = ProductTypeHelper.GetSpecificationId(product.TypeId.Value, array4[0]);
						int specificationValueId = ProductTypeHelper.GetSpecificationValueId(specificationId, array4[1].Replace("\\", "/"));
						text4 = text4 + specificationValueId + "_";
						sKUItem.SkuItems.Add(specificationId, specificationValueId);
					}
					sKUItem.SkuId = text4.Substring(0, text4.Length - 1);
					dictionary.Add(sKUItem.SkuId, sKUItem);
				}
			}
			return dictionary;
		}

		private TaobaoProductInfo GetTaobaoProduct(HttpContext context)
		{
			TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
			taobaoProductInfo.Cid = long.Parse(context.Request.Form["Cid"]);
			taobaoProductInfo.StuffStatus = context.Request.Form["StuffStatus"];
			taobaoProductInfo.LocationState = context.Request.Form["LocationState"];
			taobaoProductInfo.LocationCity = context.Request.Form["LocationCity"];
			taobaoProductInfo.FreightPayer = context.Request.Form["FreightPayer"];
			if (!string.IsNullOrEmpty(context.Request.Form["PostFee"]))
			{
				taobaoProductInfo.PostFee = decimal.Parse(context.Request.Form["PostFee"]);
			}
			if (!string.IsNullOrEmpty(context.Request.Form["ExpressFee"]))
			{
				taobaoProductInfo.ExpressFee = decimal.Parse(context.Request.Form["ExpressFee"]);
			}
			if (!string.IsNullOrEmpty(context.Request.Form["EMSFee"]))
			{
				taobaoProductInfo.EMSFee = decimal.Parse(context.Request.Form["EMSFee"]);
			}
			taobaoProductInfo.HasInvoice = bool.Parse(context.Request.Form["HasInvoice"]);
			taobaoProductInfo.HasWarranty = bool.Parse(context.Request.Form["HasWarranty"]);
			taobaoProductInfo.HasDiscount = false;
			taobaoProductInfo.ListTime = DateTime.Now;
			taobaoProductInfo.PropertyAlias = context.Request.Form["PropertyAlias"];
			taobaoProductInfo.InputPids = context.Request.Form["InputPids"];
			taobaoProductInfo.InputStr = context.Request.Form["InputStr"];
			taobaoProductInfo.SkuProperties = context.Request.Form["SkuProperties"];
			taobaoProductInfo.SkuQuantities = context.Request.Form["SkuQuantities"];
			taobaoProductInfo.SkuPrices = context.Request.Form["SkuPrices"];
			taobaoProductInfo.SkuOuterIds = context.Request.Form["SkuOuterIds"];
			return taobaoProductInfo;
		}
	}
}
