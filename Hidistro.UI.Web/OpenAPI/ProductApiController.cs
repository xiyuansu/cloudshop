using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SqlDal.Commodities;
using Hishop.Open.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Hidistro.UI.Web.OpenAPI
{
	public class ProductApiController : ApiController
	{
		public HttpResponseMessage GetSoldProducts()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				sortedDictionary.Add(text, nameValueCollection.Get(text));
			}
			DateTime? start_modified = null;
			DateTime? end_modified = null;
			string approve_status = "";
			string content = "";
			int page_no = 0;
			int page_size = 0;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (this.CheckSoldProductsParameters(sortedDictionary, out start_modified, out end_modified, out approve_status, out page_no, out page_size, out content) && OpenApiSign.CheckSign(sortedDictionary, siteSettings.CheckCode, ref content))
			{
				content = this.GetSoldProducts(start_modified, end_modified, approve_status, sortedDictionary["q"], sortedDictionary["order_by"], page_no, page_size);
			}
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		public HttpResponseMessage GetProduct()
		{
			int num_iid = 0;
			string content = "";
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				sortedDictionary.Add(text, nameValueCollection.Get(text));
			}
			if (this.CheckProductParameters(sortedDictionary, out num_iid, out content))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (OpenApiSign.CheckSign(sortedDictionary, siteSettings.CheckCode, ref content))
				{
					content = this.GetProduct(num_iid);
				}
			}
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpGet]
		public HttpResponseMessage UpdateProductQuantity()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			ProductQuantityParam productQuantityParam = new ProductQuantityParam();
			if (nameValueCollection.AllKeys.Contains("app_key"))
			{
				productQuantityParam.app_key = nameValueCollection["app_key"];
			}
			if (nameValueCollection.AllKeys.Contains("timestamp"))
			{
				productQuantityParam.timestamp = nameValueCollection["timestamp"];
			}
			if (nameValueCollection.AllKeys.Contains("sign"))
			{
				productQuantityParam.sign = nameValueCollection["sign"];
			}
			if (nameValueCollection.AllKeys.Contains("num_iid"))
			{
				productQuantityParam.num_iid = Convert.ToInt32(nameValueCollection["num_iid"]);
			}
			if (nameValueCollection.AllKeys.Contains("sku_id"))
			{
				productQuantityParam.sku_id = nameValueCollection["sku_id"];
			}
			if (nameValueCollection.AllKeys.Contains("quantity"))
			{
				productQuantityParam.quantity = Convert.ToInt32(nameValueCollection["quantity"]);
			}
			if (nameValueCollection.AllKeys.Contains("type"))
			{
				productQuantityParam.type = Convert.ToInt32(nameValueCollection["type"]);
			}
			string content = this._updateProductQuantity(productQuantityParam);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpPost]
		public HttpResponseMessage UpdateProductQuantity(ProductQuantityParam data)
		{
			string content = this._updateProductQuantity(data);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		private string _updateProductQuantity(ProductQuantityParam data)
		{
			string result = "";
			if (this.CheckUpdateQuantityParameters(data, out result))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string text = OpenApiSign.Sign(data.SignStr(siteSettings.CheckCode), "MD5", "utf-8");
				if (text.Equals(data.sign))
				{
					if (data.type == 0)
					{
						data.type = 1;
					}
					result = this.lastUpdateProductQuantity(data);
				}
				else
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign");
				}
			}
			return result;
		}

		[HttpGet]
		public HttpResponseMessage UpdateProductApproveStatus()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			ProductApproveStatusParam productApproveStatusParam = new ProductApproveStatusParam();
			if (nameValueCollection.AllKeys.Contains("app_key"))
			{
				productApproveStatusParam.app_key = nameValueCollection["app_key"];
			}
			if (nameValueCollection.AllKeys.Contains("timestamp"))
			{
				productApproveStatusParam.timestamp = nameValueCollection["timestamp"];
			}
			if (nameValueCollection.AllKeys.Contains("sign"))
			{
				productApproveStatusParam.sign = nameValueCollection["sign"];
			}
			if (nameValueCollection.AllKeys.Contains("num_iid"))
			{
				productApproveStatusParam.num_iid = Convert.ToInt32(nameValueCollection["num_iid"]);
			}
			if (nameValueCollection.AllKeys.Contains("approve_status"))
			{
				productApproveStatusParam.approve_status = nameValueCollection["approve_status"];
			}
			string content = this._updateProductApproveStatus(productApproveStatusParam);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpPost]
		public HttpResponseMessage UpdateProductApproveStatus(ProductApproveStatusParam data)
		{
			string content = this._updateProductApproveStatus(data);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		private string _updateProductApproveStatus(ProductApproveStatusParam data)
		{
			string result = "";
			if (this.CheckUpdateApproveStatusParameters(data, out result))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string text = OpenApiSign.Sign(data.SignStr(siteSettings.CheckCode), "MD5", "utf-8");
				result = ((!text.Equals(data.sign)) ? OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign") : this.lastUpdateProductApproveStatus(data.num_iid, data.approve_status));
			}
			return result;
		}

		private bool CheckSoldProductsParameters(SortedDictionary<string, string> parameters, out DateTime? start_modified, out DateTime? end_modified, out string status, out int page_no, out int page_size, out string result)
		{
			start_modified = null;
			end_modified = null;
			status = string.Empty;
			page_no = 1;
			page_size = 10;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!OpenApiHelper.CheckSystemParameters(parameters, siteSettings.AppKey, out result))
			{
				return false;
			}
			status = DataHelper.CleanSearchString(parameters["approve_status"]);
			if (!string.IsNullOrWhiteSpace(status) && status != "On_Sale" && status != "Un_Sale" && status != "In_Stock")
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_Status_is_Invalid, "approve_status");
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["start_modified"]) && !OpenApiHelper.IsDate(parameters["start_modified"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "start_modified");
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["end_modified"]) && !OpenApiHelper.IsDate(parameters["end_modified"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "end_modified");
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["start_modified"]))
			{
				DateTime dateTime = default(DateTime);
				DateTime.TryParse(parameters["start_modified"], out dateTime);
				start_modified = dateTime;
				if (dateTime > DateTime.Now)
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_Now, "start_modified and currenttime");
					return false;
				}
				if (!string.IsNullOrEmpty(parameters["end_modified"]))
				{
					DateTime dateTime2 = default(DateTime);
					DateTime.TryParse(parameters["end_modified"], out dateTime2);
					end_modified = dateTime2;
					if (dateTime > dateTime2)
					{
						result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_End, "start_modified and end_created");
						return false;
					}
				}
			}
			if (!string.IsNullOrEmpty(parameters["order_by"]))
			{
				if (parameters["order_by"].Split(':').Length != 2)
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Format, "order_by");
					return false;
				}
				string[] array = parameters["order_by"].Split(':');
				string text = DataHelper.CleanSearchString(array[0]);
				string a = DataHelper.CleanSearchString(array[1]);
				if (string.IsNullOrEmpty(text))
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Format, "order_by");
					return false;
				}
				if (text != "display_sequence" || text != "create_time" || text != "sold_quantity")
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Format, "order_by");
					return false;
				}
				if (a != "desc" || a != "asc")
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Format, "order_by");
					return false;
				}
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && !int.TryParse(parameters["page_size"].ToString(), out page_size))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_size");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && (page_size <= 0 || page_size > 100))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_size");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && !int.TryParse(parameters["page_no"].ToString(), out page_no))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_no");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && page_no <= 0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_no");
				return false;
			}
			return true;
		}

		private string GetSoldProducts(DateTime? start_modified, DateTime? end_modified, string approve_status, string q, string order_by, int page_no, int page_size)
		{
			string format = "{{\"products_get_response\":{{\"total_results\":\"{0}\",\"items\":{1}}}}}";
			ProductQuery productQuery = new ProductQuery
			{
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc,
				PageIndex = 1,
				PageSize = 40,
				SaleStatus = ProductSaleStatus.All
			};
			if (start_modified.HasValue)
			{
				productQuery.StartDate = start_modified;
			}
			if (end_modified.HasValue)
			{
				productQuery.EndDate = end_modified;
			}
			if (!string.IsNullOrEmpty(q))
			{
				productQuery.Keywords = DataHelper.CleanSearchString(q);
			}
			if (!string.IsNullOrEmpty(approve_status))
			{
				ProductSaleStatus saleStatus = ProductSaleStatus.All;
				EnumDescription.GetEnumValue(approve_status, ref saleStatus);
				productQuery.SaleStatus = saleStatus;
			}
			DbQueryResult productsApiByQuery = new ProductApiDao().GetProductsApiByQuery(productQuery);
			return string.Format(format, productsApiByQuery.TotalRecords, this.ConvertProductSold(productsApiByQuery.Data));
		}

		private string ConvertProductSold(DataTable dt)
		{
			List<product_list_model> list = new List<product_list_model>();
			foreach (DataRow row in dt.Rows)
			{
				product_list_model product_list_model = new product_list_model();
				product_list_model.cid = (int)row["CategoryId"];
				if (row["CategoryName"] != DBNull.Value)
				{
					product_list_model.cat_name = (string)row["CategoryName"];
				}
				if (row["BrandId"] != DBNull.Value)
				{
					product_list_model.brand_id = (int)row["BrandId"];
				}
				if (row["BrandName"] != DBNull.Value)
				{
					product_list_model.brand_name = (string)row["BrandName"];
				}
				if (row["TypeId"] != DBNull.Value)
				{
					product_list_model.type_id = (int)row["TypeId"];
				}
				if (row["TypeName"] != DBNull.Value)
				{
					product_list_model.type_name = (string)row["TypeName"];
				}
				product_list_model.num_iid = (int)row["ProductId"];
				product_list_model.title = (string)row["ProductName"];
				if (row["ProductCode"] != DBNull.Value)
				{
					product_list_model.outer_id = (string)row["ProductCode"];
				}
				if (row["ImageUrl1"] != DBNull.Value && !string.IsNullOrEmpty((string)row["ImageUrl1"]))
				{
					product_list_model.pic_url.Add((string)row["ImageUrl1"]);
				}
				if (row["ImageUrl2"] != DBNull.Value && !string.IsNullOrEmpty((string)row["ImageUrl2"]))
				{
					product_list_model.pic_url.Add((string)row["ImageUrl2"]);
				}
				if (row["ImageUrl3"] != DBNull.Value && !string.IsNullOrEmpty((string)row["ImageUrl3"]))
				{
					product_list_model.pic_url.Add((string)row["ImageUrl3"]);
				}
				if (row["ImageUrl4"] != DBNull.Value && !string.IsNullOrEmpty((string)row["ImageUrl4"]))
				{
					product_list_model.pic_url.Add((string)row["ImageUrl4"]);
				}
				if (row["ImageUrl5"] != DBNull.Value && !string.IsNullOrEmpty((string)row["ImageUrl5"]))
				{
					product_list_model.pic_url.Add((string)row["ImageUrl5"]);
				}
				product_list_model.list_time = (DateTime)row["AddedDate"];
				product_list_model.modified = (DateTime)row["UpdateDate"];
				switch ((ProductSaleStatus)row["SaleStatus"])
				{
				case ProductSaleStatus.OnSale:
					product_list_model.approve_status = "On_Sale";
					break;
				case ProductSaleStatus.UnSale:
					product_list_model.approve_status = "Un_Sale";
					break;
				default:
					product_list_model.approve_status = "In_Stock";
					break;
				}
				product_list_model.sold_quantity = (int)row["SaleCounts"];
				product_list_model.num = (int)row["Stock"];
				product_list_model.price = (decimal)row["SalePrice"];
				list.Add(product_list_model);
			}
			return JsonConvert.SerializeObject(list);
		}

		private string GetProduct(int num_iid)
		{
			ProductApiDao productApiDao = new ProductApiDao();
			product_item_model product = productApiDao.GetProduct(num_iid);
			if (product == null)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_Not_Exists, "num_iid");
			}
			product.props_name = productApiDao.GetProps(num_iid);
			product.skus = productApiDao.GetSkus(num_iid);
			string format = "{{\"product_get_response\":{{\"item\":{0}}}}}";
			return string.Format(format, JsonConvert.SerializeObject(product));
		}

		private bool CheckProductParameters(SortedDictionary<string, string> parameters, out int num_iid, out string result)
		{
			num_iid = 0;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!OpenApiHelper.CheckSystemParameters(parameters, siteSettings.AppKey, out result))
			{
				return false;
			}
			if (!int.TryParse(parameters["num_iid"], out num_iid))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "num_iid");
				return false;
			}
			return true;
		}

		private bool CheckUpdateQuantityParameters(ProductQuantityParam parameter, out string result)
		{
			if (!OpenApiHelper.CheckSystemParameters(parameter.app_key, parameter.timestamp, parameter.sign, out result))
			{
				return false;
			}
			if (parameter.num_iid <= 0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "num_iid");
				return false;
			}
			return true;
		}

		private string lastUpdateProductQuantity(ProductQuantityParam param)
		{
			ProductApiDao productApiDao = new ProductApiDao();
			product_item_model product = productApiDao.GetProduct(param.num_iid);
			if (product == null)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_Not_Exists, "num_iid");
			}
			if (productApiDao.UpdateProductQuantity(param.num_iid, param.sku_id, param.quantity, param.type) <= 0)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_UpdateeQuantity_Faild, "update_quantity");
			}
			product.props_name = productApiDao.GetProps(param.num_iid);
			product.skus = productApiDao.GetSkus(param.num_iid);
			string format = "{{\"product_get_response\":{{\"item\":{0}}}}}";
			return string.Format(format, JsonConvert.SerializeObject(product));
		}

		private bool CheckUpdateApproveStatusParameters(ProductApproveStatusParam parameter, out string result)
		{
			if (!OpenApiHelper.CheckSystemParameters(parameter.app_key, parameter.timestamp, parameter.sign, out result))
			{
				return false;
			}
			if (parameter.num_iid <= 0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "num_iid");
				return false;
			}
			parameter.approve_status = DataHelper.CleanSearchString(parameter.approve_status);
			if (parameter.approve_status != "On_Sale" && parameter.approve_status != "Un_Sale" && parameter.approve_status != "In_Stock")
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_Status_is_Invalid, "approve_status");
				return false;
			}
			return true;
		}

		public string lastUpdateProductApproveStatus(int num_iid, string approve_status)
		{
			ProductApiDao productApiDao = new ProductApiDao();
			product_item_model product = productApiDao.GetProduct(num_iid);
			if (product == null)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_Not_Exists, "num_iid");
			}
			if (productApiDao.UpdateProductApproveStatus(num_iid, approve_status) <= 0)
			{
				return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Product_ApproveStatus_Faild, "update_approve_status");
			}
			product.approve_status = approve_status;
			product.props_name = productApiDao.GetProps(num_iid);
			product.skus = productApiDao.GetSkus(num_iid);
			string format = "{{\"product_get_response\":{{\"item\":{0}}}}}";
			return string.Format(format, JsonConvert.SerializeObject(product));
		}
	}
}
