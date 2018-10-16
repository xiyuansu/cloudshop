using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	[PrivilegeCheck(Privilege.ProductBatchExport)]
	public class ExportToLocal : AdminBaseHandler
	{
		private string _flag;

		private DirectoryInfo _baseDir;

		private readonly Encoding _encoding = Encoding.Unicode;

		private DirectoryInfo _workDir;

		private string _zipFilename;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "export")
				{
					this.Export(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private void GetList(HttpContext context)
		{
			AdvancedProductQuery query = this.GetQuery(context);
			DataGridViewModel<Dictionary<string, object>> products = this.GetProducts(query, context);
			string s = base.SerializeObjectToJson(products);
			context.Response.Write(s);
			context.Response.End();
		}

		private AdvancedProductQuery GetQuery(HttpContext context)
		{
			AdvancedProductQuery advancedProductQuery = new AdvancedProductQuery();
			int num = 1;
			int num2 = 10;
			int num3 = 0;
			string empty = string.Empty;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			advancedProductQuery.Keywords = context.Request["productName"];
			advancedProductQuery.ProductCode = context.Request["productCode"];
			advancedProductQuery.CategoryId = base.GetIntParam(context, "categoryId", true);
			advancedProductQuery.SaleStatus = ProductSaleStatus.OnSale;
			if (advancedProductQuery.CategoryId.HasValue)
			{
				advancedProductQuery.MaiCategoryPath = CatalogHelper.GetCategory(advancedProductQuery.CategoryId.Value).Path;
			}
			advancedProductQuery.IncludeInStock = base.GetBoolParam(context, "IncludeInStock", false).Value;
			advancedProductQuery.IncludeOnSales = base.GetBoolParam(context, "IncludeOnSales", false).Value;
			advancedProductQuery.IncludeUnSales = base.GetBoolParam(context, "IncludeUnSales", false).Value;
			if (!advancedProductQuery.IncludeInStock && !advancedProductQuery.IncludeOnSales && !advancedProductQuery.IncludeUnSales)
			{
				throw new HidistroAshxException("至少要选择包含一个商品状态");
			}
			advancedProductQuery.IsMakeTaobao = base.GetIntParam(context, "IsMakeTaobao", true);
			advancedProductQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			advancedProductQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			advancedProductQuery.PageSize = base.CurrentPageSize;
			advancedProductQuery.PageIndex = base.CurrentPageIndex;
			advancedProductQuery.SortOrder = SortAction.Desc;
			advancedProductQuery.SortBy = "DisplaySequence";
			Globals.EntityCoding(advancedProductQuery, true);
			return advancedProductQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetProducts(AdvancedProductQuery query, HttpContext context)
		{
			string empty = string.Empty;
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				empty = (string.IsNullOrEmpty(context.Request["RemoveProductId"]) ? "" : context.Request["RemoveProductId"]);
				DbQueryResult exportProducts = ProductHelper.GetExportProducts(query, empty);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(exportProducts.Data);
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					if (string.IsNullOrEmpty(row["ThumbnailUrl40"].ToNullString()))
					{
						row["ThumbnailUrl40"] = HiContext.Current.SiteSettings.DefaultProductImage;
					}
				}
				dataGridViewModel.total = exportProducts.TotalRecords;
			}
			return dataGridViewModel;
		}

		private void Export(HttpContext context)
		{
			this._baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/local"));
			this._flag = DateTime.Now.ToString("yyyyMMddHHmmss");
			this._zipFilename = $"local{this._flag}.zip";
			string removeProductIds = string.Empty;
			if (!string.IsNullOrEmpty(context.Request["RemoveProductId"]))
			{
				removeProductIds = context.Request["RemoveProductId"];
			}
			string text = "http://" + HttpContext.Current.Request.Url.Host;
			string empty = string.Empty;
			AdvancedProductQuery query = this.GetQuery(context);
			DataTable dataTable = ProductHelper.GetExportProducts(query, true, true, removeProductIds).Tables[3];
			List<ProductDetail> list = new List<ProductDetail>();
			foreach (DataRow row in dataTable.Rows)
			{
				int productId = Convert.ToInt32(row["ProductId"]);
				IList<int> tagIds = null;
				Dictionary<int, IList<int>> attrs = default(Dictionary<int, IList<int>>);
				ProductInfo productDetails = ProductHelper.GetProductDetails(productId, out attrs, out tagIds);
				ProductDetail productDetail = new ProductDetail();
				productDetail.pi = productDetails;
				productDetail.attrs = attrs;
				productDetail.tagIds = tagIds;
				list.Add(productDetail);
			}
			this._workDir = this._baseDir.CreateSubdirectory(this._flag);
			string empty2 = string.Empty;
			string text2 = this._workDir.FullName + $"\\local{this._flag}.csv";
			empty2 = text2.Replace(".csv", "");
			if (!Directory.Exists(empty2))
			{
				Directory.CreateDirectory(empty2);
			}
			this.DoExportForHishop(text2, empty2, list, context);
		}

		private void DownZip(string dirname, string zipfile, int level = 6, string password = "")
		{
			MemoryStream memoryStream = new MemoryStream();
			ZipOutputStream zipOutputStream = new ZipOutputStream(memoryStream);
			if (password != "")
			{
				zipOutputStream.Password = password;
			}
			zipOutputStream.CompressionLevel = (CompressionLevel)level;
			this.AddZipEntry(dirname, zipOutputStream, dirname);
			zipOutputStream.Close();
			HttpResponse response = HttpContext.Current.Response;
			response.Clear();
			response.ContentType = "application/octet-stream";
			response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlDecode(zipfile, Encoding.UTF8) + ".zip");
			response.BinaryWrite(memoryStream.ToArray());
			memoryStream.Close();
			response.Flush();
			response.End();
		}

		private void AddZipEntry(string strPath, ZipOutputStream zos, string baseDirName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(strPath);
			FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
			foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
			{
				if ((fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					this.AddZipEntry(fileSystemInfo.FullName, zos, baseDirName);
				}
				else
				{
					FileInfo fileInfo = (FileInfo)fileSystemInfo;
					using (FileStream fileStream = fileInfo.OpenRead())
					{
						byte[] array = new byte[(int)fileStream.Length];
						fileStream.Read(array, 0, array.Length);
						zos.PutNextEntry(fileInfo.FullName.Replace(baseDirName, ""));
						zos.Write(array, 0, array.Length);
					}
				}
			}
		}

		private void DoExportForHishop(string csvFilename, string imagePath, List<ProductDetail> list, HttpContext context)
		{
			using (FileStream fileStream = new FileStream(csvFilename, FileMode.Create, FileAccess.Write))
			{
				string productCSVForHishop = this.GetProductCSVForHishop(imagePath, list, context);
				UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
				int byteCount = unicodeEncoding.GetByteCount(productCSVForHishop);
				byte[] preamble = unicodeEncoding.GetPreamble();
				byte[] array = new byte[preamble.Length + byteCount];
				Buffer.BlockCopy(preamble, 0, array, 0, preamble.Length);
				unicodeEncoding.GetBytes(productCSVForHishop.ToCharArray(), 0, productCSVForHishop.Length, array, preamble.Length);
				fileStream.Write(array, 0, array.Length);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(imagePath);
			this.DownZip(this._workDir.FullName, directoryInfo.Name, 6, "");
		}

		public string ConvertNull(object obj)
		{
			if (obj is DBNull || obj == null)
			{
				obj = "";
			}
			return obj.ToString().Replace("\t", " ").Replace("\r\n", "");
		}

		private string GetProductCSVForHishop(string imagePath, List<ProductDetail> list, HttpContext context)
		{
			bool value = base.GetBoolParam(context, "ExportImages", true).Value;
			StringBuilder stringBuilder = new StringBuilder();
			string text = "\r\n-1\t\"{0}\"\t\"{1}\"\t\"{2}\"\t\"{3}\"\t\"{4}\"\t\"{5}\"\t\"{6}\"\t\"{7}\"\t\"{8}\"\t\"{9}\"\t\"{10}\"\t\"{11}\"\t\"{12}\"\t\"{13}\"\t\"{14}\"\t\"{15}\"\t\"{16}\"\t\"{17}\"\t{18}\t\"{19}\"\t\"{20}\"\t\"{21}\"\t\"{22}\"\t\"{23}\"\t\"{24}\"\t\"{25}\"\t\"{26}\"\t\"{27}\"\t\"{28}\"\t\"{29}\"\t\"{30}\"\t\"{31}\"\t\"{32}\"\t\"{33}\"";
			stringBuilder.Append("\"id\"\t\"所属分类\"\t\"商品类型\"\t\"商品名称\"\t\"商家编码\"\t\"简单描述\"\t\"计量单位\"\t");
			stringBuilder.Append("\"详细描述\"\t\"详细页标题\"\t\"详细页描述\"\t\"详细页搜索关键字\"\t\"销售状态\"\t");
			stringBuilder.Append("\"图片\"\t\"图片2\"\t\"图片3\"\t\"图片4\"\t\"图片5\"\t");
			stringBuilder.Append("\"市场价\"\t\"品牌\"\t\"是否有规格\"\t");
			stringBuilder.Append("\"规格属性\"\t\"货号\"\t\"重量\"\t\"库存\"\t成本价\"\t\"一口价\"\t");
			stringBuilder.Append("\"商品属性\"\t\"商品标签\"");
			stringBuilder.Append("\t\"商品种类\"");
			stringBuilder.Append("\t\"是否长期有效\"");
			stringBuilder.Append("\t\"有效期开始时间\"");
			stringBuilder.Append("\t\"有效期结束时间\"");
			stringBuilder.Append("\t\"是否支持退款\"");
			stringBuilder.Append("\t\"是否过期退款\"");
			stringBuilder.Append("\t\"是否生成多份\"");
			foreach (ProductDetail item in list)
			{
				string text2 = "{" + Guid.NewGuid().ToString() + "}.htm";
				string path = Path.Combine(imagePath, text2);
				using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding("gb2312")))
				{
					if (!string.IsNullOrEmpty(item.pi.Description))
					{
						string description = item.pi.Description;
						streamWriter.Write(description);
					}
				}
				string text3;
				if (!string.IsNullOrEmpty(item.pi.ImageUrl1) && !item.pi.ImageUrl1.StartsWith("http://") && !item.pi.ImageUrl1.StartsWith("https://"))
				{
					text3 = item.pi.ImageUrl1;
					if (File.Exists(context.Server.MapPath("~" + text3)))
					{
						FileInfo fileInfo = new FileInfo(context.Server.MapPath("~" + text3));
						text3 = fileInfo.Name.ToLower();
						if (value)
						{
							fileInfo.CopyTo(Path.Combine(imagePath, text3), true);
						}
					}
				}
				else
				{
					text3 = "";
				}
				string text4;
				if (!string.IsNullOrEmpty(item.pi.ImageUrl2) && !item.pi.ImageUrl2.StartsWith("http://") && !item.pi.ImageUrl2.StartsWith("https://"))
				{
					text4 = item.pi.ImageUrl2;
					if (File.Exists(context.Server.MapPath("~" + text4)))
					{
						FileInfo fileInfo2 = new FileInfo(context.Server.MapPath("~" + text4));
						text4 = fileInfo2.Name.ToLower();
						if (value)
						{
							fileInfo2.CopyTo(Path.Combine(imagePath, text4), true);
						}
					}
				}
				else
				{
					text4 = "";
				}
				string text5;
				if (!string.IsNullOrEmpty(item.pi.ImageUrl3) && !item.pi.ImageUrl3.StartsWith("http://") && !item.pi.ImageUrl3.StartsWith("https://"))
				{
					text5 = item.pi.ImageUrl3;
					if (File.Exists(context.Server.MapPath("~" + text5)))
					{
						FileInfo fileInfo3 = new FileInfo(context.Server.MapPath("~" + text5));
						text5 = fileInfo3.Name.ToLower();
						if (value)
						{
							fileInfo3.CopyTo(Path.Combine(imagePath, text5), true);
						}
					}
				}
				else
				{
					text5 = "";
				}
				string text6;
				if (!string.IsNullOrEmpty(item.pi.ImageUrl4) && !item.pi.ImageUrl4.StartsWith("http://") && !item.pi.ImageUrl4.StartsWith("https://"))
				{
					text6 = item.pi.ImageUrl4;
					if (File.Exists(context.Server.MapPath("~" + text6)))
					{
						FileInfo fileInfo4 = new FileInfo(context.Server.MapPath("~" + text6));
						text6 = fileInfo4.Name.ToLower();
						if (value)
						{
							fileInfo4.CopyTo(Path.Combine(imagePath, text6), true);
						}
					}
				}
				else
				{
					text6 = "";
				}
				string text7;
				if (!string.IsNullOrEmpty(item.pi.ImageUrl5) && !item.pi.ImageUrl5.StartsWith("http://") && !item.pi.ImageUrl5.StartsWith("https://"))
				{
					text7 = item.pi.ImageUrl5;
					if (File.Exists(context.Server.MapPath("~" + text7)))
					{
						FileInfo fileInfo5 = new FileInfo(context.Server.MapPath("~" + text7));
						text7 = fileInfo5.Name.ToLower();
						if (value)
						{
							fileInfo5.CopyTo(Path.Combine(imagePath, text7), true);
						}
					}
				}
				else
				{
					text7 = "";
				}
				string text8 = "";
				string text9 = "";
				string text10 = "";
				string text11 = "";
				string text12 = "";
				string text13 = "";
				string text14 = "";
				string text15 = "";
				string text16 = "";
				string skuId = item.pi.SkuId;
				foreach (SKUItem value4 in item.pi.Skus.Values)
				{
					text9 = text9 + value4.SKU + ";";
					text10 = text10 + ((this.ConvertNull(value4.Weight) == "") ? "0" : this.ConvertNull(value4.Weight)) + ";";
					text11 = text11 + value4.Stock + ";";
					text12 = text12 + ((this.ConvertNull(value4.CostPrice) == "") ? "0" : this.ConvertNull(value4.CostPrice)) + ";";
					text13 = text13 + value4.SalePrice + ";";
				}
				if (text12 == "")
				{
					text12 = "0";
				}
				if (text11 == "")
				{
					text11 = "0";
				}
				if (!value)
				{
					text3 = (text4 = (text5 = (text6 = (text7 = ""))));
				}
				int num;
				if (item.pi.TypeId.HasValue)
				{
					int value2 = item.pi.TypeId.Value;
					foreach (KeyValuePair<int, IList<int>> attr in item.attrs)
					{
						string attributeName = ProductTypeHelper.GetAttribute(attr.Key).AttributeName;
						foreach (int item2 in attr.Value)
						{
							string valueStr = ProductTypeHelper.GetAttributeValueInfo(item2).ValueStr;
							text14 = text14 + attributeName + ":" + valueStr + ",";
						}
					}
					IList<TagInfo> tags = CatalogHelper.GetTags();
					foreach (int tagId in item.tagIds)
					{
						foreach (TagInfo item3 in tags)
						{
							num = item3.TagID;
							if (num.Equals(tagId))
							{
								text15 = text15 + item3.TagName + ",";
								break;
							}
						}
					}
				}
				switch (item.pi.SaleStatus)
				{
				case ProductSaleStatus.OnSale:
					text16 = "出售中";
					break;
				case ProductSaleStatus.UnSale:
					text16 = "下架区";
					break;
				case ProductSaleStatus.OnStock:
					text16 = "仓库中";
					break;
				}
				bool hasSKU = item.pi.HasSKU;
				if (hasSKU)
				{
					foreach (SKUItem value5 in item.pi.Skus.Values)
					{
						foreach (KeyValuePair<int, int> skuItem in value5.SkuItems)
						{
							AttributeInfo attribute = ProductTypeHelper.GetAttribute(skuItem.Key);
							if (attribute != null)
							{
								string attributeName2 = ProductTypeHelper.GetAttribute(skuItem.Key).AttributeName;
								foreach (AttributeValueInfo attributeValue in ProductTypeHelper.GetAttribute(skuItem.Key).AttributeValues)
								{
									num = attributeValue.ValueId;
									if (num.Equals(skuItem.Value))
									{
										string valueStr2 = attributeValue.ValueStr;
										text8 = text8 + attributeName2 + ":" + valueStr2;
										break;
									}
								}
							}
							text8 += ",";
						}
						text8 = text8.Trim(',') + ";";
					}
				}
				string obj = "";
				CategoryInfo category = CatalogHelper.GetCategory(item.pi.CategoryId);
				if (category != null)
				{
					obj = category.Name;
				}
				string obj2 = "";
				if (item.pi.TypeId.HasValue)
				{
					obj2 = ProductTypeHelper.GetProductType(item.pi.TypeId.Value).TypeName;
				}
				string obj3 = "";
				if (item.pi.BrandId.HasValue)
				{
					BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(item.pi.BrandId.Value);
					obj3 = ((brandCategory != null) ? brandCategory.BrandName : string.Empty);
				}
				StringBuilder stringBuilder2 = stringBuilder;
				string format = text;
				object[] obj4 = new object[34]
				{
					this.ConvertNull(obj),
					this.ConvertNull(obj2),
					item.pi.ProductName,
					this.ConvertNull(item.pi.ProductCode),
					this.ConvertNull(item.pi.ShortDescription),
					this.ConvertNull(item.pi.Unit),
					text2,
					this.ConvertNull(item.pi.Title),
					this.ConvertNull(item.pi.Meta_Description),
					this.ConvertNull(item.pi.Meta_Keywords),
					text16,
					text3,
					text4,
					text5,
					text6,
					text7,
					this.ConvertNull(item.pi.MarketPrice),
					this.ConvertNull(obj3),
					hasSKU ? "1" : "0",
					text8.Trim(';'),
					text9.Trim(';'),
					text10.Trim(';'),
					text11.Trim(';'),
					text12.Trim(';'),
					text13.Trim(';'),
					text14.Trim(','),
					text15.Trim(','),
					(item.pi.ProductType == 0) ? "实物商品" : "服务类商品",
					item.pi.IsValid ? "1" : "0",
					null,
					null,
					null,
					null,
					null
				};
				object obj5;
				DateTime value3;
				if (!item.pi.ValidStartDate.HasValue)
				{
					obj5 = "";
				}
				else
				{
					value3 = item.pi.ValidStartDate.Value;
					obj5 = value3.ToString("yyyy-MM-dd HH:mm:ss");
				}
				obj4[29] = obj5;
				object obj6;
				if (!item.pi.ValidEndDate.HasValue)
				{
					obj6 = "";
				}
				else
				{
					value3 = item.pi.ValidEndDate.Value;
					obj6 = value3.ToString("yyyy-MM-dd HH:mm:ss");
				}
				obj4[30] = obj6;
				obj4[31] = (item.pi.IsRefund ? "1" : "0");
				obj4[32] = (item.pi.IsOverRefund ? "1" : "0");
				obj4[33] = (item.pi.IsGenerateMore ? "1" : "0");
				stringBuilder2.AppendFormat(format, obj4);
			}
			return stringBuilder.ToString();
		}
	}
}
