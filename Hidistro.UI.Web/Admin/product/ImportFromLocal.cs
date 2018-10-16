using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.ProductBatchUpload)]
	public class ImportFromLocal : AdminPage
	{
		private string _dataPath;

		private readonly Encoding _encoding = Encoding.UTF8;

		private readonly DirectoryInfo _baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/App_Data/data/homemade"));

		private DirectoryInfo _workDir;

		private DataTable _exportData = new DataTable();

		private string csvPath = "";

		private string uploadPath = HiContext.Current.GetStoragePath() + "/product";

		protected DropDownList dropFiles;

		protected FileUpload fileUploader;

		protected Button btnUpload;

		protected Button btnImport;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this._dataPath = this.Page.Request.MapPath("~/App_Data/data/homemade");
			this.btnUpload.Click += this.btnUpload_Click;
			this.btnImport.Click += this.btnImport_Click;
			if (!this.Page.IsPostBack)
			{
				this.BindFiles();
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		private void BindFiles()
		{
			this.dropFiles.Items.Clear();
			this.dropFiles.Items.Add(new ListItem("请选择文件", ""));
			DirectoryInfo directoryInfo = new DirectoryInfo(this._dataPath);
			FileInfo[] files = directoryInfo.GetFiles("*.zip", SearchOption.TopDirectoryOnly);
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				string name = fileInfo.Name;
				this.dropFiles.Items.Add(new ListItem(name, name));
			}
		}

		private void GetImg(string fileName, ref ProductInfo product, int index)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			string str = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(fileName);
			string text = this.uploadPath + "/images/" + str;
			string text2 = this.uploadPath + "/thumbs40/40_" + str;
			string text3 = this.uploadPath + "/thumbs60/60_" + str;
			string text4 = this.uploadPath + "/thumbs100/100_" + str;
			string text5 = this.uploadPath + "/thumbs160/160_" + str;
			string text6 = this.uploadPath + "/thumbs180/180_" + str;
			string text7 = this.uploadPath + "/thumbs220/220_" + str;
			string text8 = this.uploadPath + "/thumbs310/310_" + str;
			string text9 = this.uploadPath + "/thumbs410/410_" + str;
			fileInfo.CopyTo(base.Request.MapPath(text));
			string sourceFilename = base.Request.MapPath(text);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text2), 40, 40);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text3), 60, 60);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text4), 100, 100);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text5), 160, 160);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text6), 180, 180);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text7), 220, 220);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text8), 310, 310);
			ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(text9), 410, 410);
			switch (index)
			{
			case 1:
				product.ImageUrl1 = text;
				product.ThumbnailUrl40 = text2;
				product.ThumbnailUrl60 = text3;
				product.ThumbnailUrl100 = text4;
				product.ThumbnailUrl160 = text5;
				product.ThumbnailUrl180 = text6;
				product.ThumbnailUrl220 = text7;
				product.ThumbnailUrl310 = text8;
				product.ThumbnailUrl410 = text9;
				break;
			case 2:
				product.ImageUrl2 = text;
				break;
			case 3:
				product.ImageUrl3 = text;
				break;
			case 4:
				product.ImageUrl4 = text;
				break;
			case 5:
				product.ImageUrl5 = text;
				break;
			}
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			string selectedValue = this.dropFiles.SelectedValue;
			selectedValue = Path.Combine(this._dataPath, selectedValue);
			if (!File.Exists(selectedValue))
			{
				this.ShowMsg("选择的数据包文件有问题！", false);
			}
			else
			{
				int num = 0;
				int num2 = 0;
				this.PrepareDataFiles(selectedValue);
				List<List<string>> list = this.ReadCsv(this.csvPath, true, '\t', Encoding.GetEncoding("GB2312"));
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					ProductInfo productInfo = new ProductInfo();
					productInfo.AuditStatus = ProductAuditStatus.Pass;
					try
					{
						List<string> list2 = list[i];
						if (list2[18] != "")
						{
							DataTable brandCategories = CatalogHelper.GetBrandCategories(list2[18]);
							if (brandCategories.Rows.Count > 0)
							{
								productInfo.BrandId = Convert.ToInt32(brandCategories.Rows[0]["BrandId"]);
							}
						}
						if (list2[1] != "")
						{
							DataTable categoryes = CatalogHelper.GetCategoryes(list2[1]);
							if (categoryes.Rows.Count > 0)
							{
								productInfo.CategoryId = Convert.ToInt32(categoryes.Rows[0]["CategoryId"]);
							}
							else
							{
								productInfo.CategoryId = 0;
							}
						}
						else
						{
							productInfo.CategoryId = 0;
						}
						if (list2[7] != "")
						{
							string path = Path.Combine(this.csvPath.Replace(".csv", ""), list2[7]);
							using (StreamReader streamReader = new StreamReader(path, Encoding.GetEncoding("gb2312")))
							{
								productInfo.Description = streamReader.ReadToEnd();
							}
						}
						if (productInfo.CategoryId > 0)
						{
							productInfo.MainCategoryPath = CatalogHelper.GetCategory(productInfo.CategoryId).Path + "|";
						}
						productInfo.HasSKU = (int.Parse(list2[19]) == 1);
						productInfo.ImageUrl1 = "";
						productInfo.ImageUrl2 = "";
						productInfo.ImageUrl3 = "";
						productInfo.ImageUrl4 = "";
						productInfo.ImageUrl5 = "";
						if (list2[12] != "")
						{
							FileInfo fileInfo = new FileInfo(Path.Combine(this.csvPath.Replace(".csv", ""), list2[12]));
							if (fileInfo.Exists)
							{
								this.GetImg(fileInfo.FullName, ref productInfo, 1);
							}
						}
						if (list2[13] != "")
						{
							FileInfo fileInfo2 = new FileInfo(Path.Combine(this.csvPath.Replace(".csv", ""), list2[13]));
							if (fileInfo2.Exists)
							{
								this.GetImg(fileInfo2.FullName, ref productInfo, 2);
							}
						}
						if (list2[14] != "")
						{
							FileInfo fileInfo3 = new FileInfo(Path.Combine(this.csvPath.Replace(".csv", ""), list2[14]));
							if (fileInfo3.Exists)
							{
								this.GetImg(fileInfo3.FullName, ref productInfo, 3);
							}
						}
						if (list2[15] != "")
						{
							FileInfo fileInfo4 = new FileInfo(Path.Combine(this.csvPath.Replace(".csv", ""), list2[15]));
							if (fileInfo4.Exists)
							{
								this.GetImg(fileInfo4.FullName, ref productInfo, 4);
							}
						}
						if (list2[16] != "")
						{
							FileInfo fileInfo5 = new FileInfo(Path.Combine(this.csvPath.Replace(".csv", ""), list2[16]));
							if (fileInfo5.Exists)
							{
								this.GetImg(fileInfo5.FullName, ref productInfo, 5);
							}
						}
						if (list2[17] != "")
						{
							productInfo.MarketPrice = decimal.Parse(list2[17]);
						}
						if (list2[9] != "")
						{
							productInfo.Meta_Description = list2[9];
						}
						if (list2[10] != "")
						{
							productInfo.Meta_Keywords = list2[10];
						}
						if (list2[4] != "")
						{
							productInfo.ProductCode = list2[4];
						}
						productInfo.ProductName = list2[3].Replace("\\", "");
						string text = list2[11];
						switch (text)
						{
						case "出售中":
							productInfo.SaleStatus = ProductSaleStatus.OnSale;
							break;
						case "下架区":
							productInfo.SaleStatus = ProductSaleStatus.UnSale;
							break;
						case "仓库中":
							productInfo.SaleStatus = ProductSaleStatus.OnStock;
							break;
						}
						if (list2[5] != "")
						{
							productInfo.ShortDescription = list2[5].Replace("\\", "");
						}
						if (list2[8] != "")
						{
							productInfo.Title = list2[8].Replace("\\", "");
						}
						if (list2[2] != "")
						{
							int typeId = ProductTypeHelper.GetTypeId(list2[2]);
							if (typeId > 0)
							{
								productInfo.TypeId = typeId;
							}
						}
						if (!productInfo.TypeId.HasValue)
						{
							productInfo.HasSKU = false;
						}
						if (list2[6] != "")
						{
							productInfo.Unit = list2[6];
						}
						Dictionary<string, SKUItem> dictionary = null;
						Dictionary<int, IList<int>> dictionary2 = null;
						IList<int> list3 = new List<int>();
						if (list2[20] == "")
						{
							dictionary = new Dictionary<string, SKUItem>();
							SKUItem sKUItem = new SKUItem();
							sKUItem.SkuId = "0";
							sKUItem.CostPrice = decimal.Parse(list2[24].Split(';')[0]);
							sKUItem.SalePrice = decimal.Parse(list2[25].Split(';')[0]);
							sKUItem.SKU = list2[21].Split(';')[0];
							sKUItem.Stock = int.Parse(list2[23].Split(';')[0]);
							sKUItem.Weight = decimal.Parse(list2[22].Split(';')[0]);
							dictionary.Add(sKUItem.SkuId, sKUItem);
						}
						else if (productInfo.TypeId.HasValue)
						{
							dictionary = new Dictionary<string, SKUItem>();
							int value = productInfo.TypeId.Value;
							if (productInfo.HasSKU)
							{
								IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(value, AttributeUseageMode.Choose);
								string[] array = list2[20].Split(';');
								int num3 = array.Length;
								for (int j = 0; j < num3; j++)
								{
									SKUItem sKUItem2 = new SKUItem();
									sKUItem2.CostPrice = decimal.Parse(list2[24].Split(';')[j]);
									sKUItem2.SalePrice = decimal.Parse(list2[25].Split(';')[j]);
									sKUItem2.SKU = list2[21].Split(';')[j];
									sKUItem2.Stock = int.Parse(list2[23].Split(';')[j]);
									sKUItem2.Weight = decimal.Parse(list2[22].Split(';')[j]);
									string text2 = array[j];
									Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
									string[] array2 = text2.Split(',');
									int num4 = 0;
									while (num4 < array2.Length)
									{
										string text3 = array2[num4];
										string specificationName = text3.Split(':')[0];
										string valueStr = text3.Split(':')[1];
										int specificationId = ProductTypeHelper.GetSpecificationId(value, specificationName);
										if (specificationId > 0)
										{
											int specificationValueId = ProductTypeHelper.GetSpecificationValueId(specificationId, valueStr);
											if (specificationValueId > 0)
											{
												dictionary3.Add(specificationId, specificationValueId);
												num4++;
												continue;
											}
											productInfo.HasSKU = false;
										}
										else
										{
											productInfo.HasSKU = false;
										}
										break;
									}
									if (productInfo.HasSKU && dictionary3.Count > 0)
									{
										string text4 = "";
										foreach (KeyValuePair<int, int> item in dictionary3)
										{
											sKUItem2.SkuItems.Add(item.Key, item.Value);
											text4 = text4 + item.Value + "_";
										}
										sKUItem2.SkuId = text4.Substring(0, text4.Length - 1);
										dictionary.Add(sKUItem2.SkuId, sKUItem2);
									}
								}
								if (dictionary.Count > 0)
								{
									productInfo.HasSKU = true;
								}
							}
							else
							{
								SKUItem sKUItem3 = new SKUItem();
								sKUItem3.SkuId = "0";
								sKUItem3.CostPrice = decimal.Parse(list2[24].Split(';')[0]);
								sKUItem3.SalePrice = decimal.Parse(list2[25].Split(';')[0]);
								sKUItem3.SKU = list2[21].Split(';')[0];
								sKUItem3.Stock = int.Parse(list2[23].Split(';')[0]);
								sKUItem3.Weight = int.Parse(list2[22].Split(';')[0]);
								dictionary.Add(sKUItem3.SkuId, sKUItem3);
							}
						}
						if (list2[26] != "" && productInfo.TypeId.HasValue)
						{
							int value2 = productInfo.TypeId.Value;
							dictionary2 = new Dictionary<int, IList<int>>();
							IList<AttributeInfo> attributes2 = ProductTypeHelper.GetAttributes(value2, AttributeUseageMode.View);
							foreach (AttributeInfo attribute in ProductTypeHelper.GetAttributes(value2, AttributeUseageMode.MultiView))
							{
								attributes2.Add(attribute);
							}
							string[] array3 = list2[26].Split(',');
							foreach (string text5 in array3)
							{
								string value3 = text5.Split(':')[0];
								string valueStr2 = text5.Split(':')[1];
								bool flag = false;
								int num5 = 0;
								foreach (AttributeInfo item2 in attributes2)
								{
									if (item2.AttributeName.Equals(value3))
									{
										num5 = item2.AttributeId;
										flag = true;
										break;
									}
								}
								if (flag)
								{
									int specificationValueId2 = ProductTypeHelper.GetSpecificationValueId(num5, valueStr2);
									if (specificationValueId2 > 0)
									{
										if (dictionary2.ContainsKey(num5))
										{
											dictionary2[num5].Add(specificationValueId2);
										}
										else
										{
											dictionary2.Add(num5, new List<int>
											{
												specificationValueId2
											});
										}
									}
								}
							}
						}
						if (list2[27] != "")
						{
							list3 = new List<int>();
							IList<TagInfo> tags = CatalogHelper.GetTags();
							string[] array4 = list2[27].Split(',');
							foreach (string value4 in array4)
							{
								foreach (TagInfo item3 in tags)
								{
									if (item3.TagName.Equals(value4))
									{
										list3.Add(item3.TagID);
										break;
									}
								}
							}
						}
						productInfo.AddedDate = DateTime.Now;
						if (list2.Count >= 28)
						{
							productInfo.ProductType = ((!(list2[28].Trim() == "实物商品")) ? 1 : 0);
						}
						if (list2.Count >= 29)
						{
							productInfo.IsValid = (list2[29].ToInt(0) == 1);
						}
						if (list2.Count >= 30)
						{
							productInfo.ValidStartDate = ((!productInfo.IsValid) ? list2[30].ToDateTime() : null);
						}
						if (list2.Count >= 31)
						{
							productInfo.ValidEndDate = ((!productInfo.IsValid) ? list2[31].ToDateTime() : null);
						}
						if (list2.Count >= 32)
						{
							productInfo.IsRefund = (list2[32].ToInt(0) == 1);
						}
						if (list2.Count >= 33)
						{
							productInfo.IsOverRefund = (list2[33].ToInt(0) == 1);
						}
						if (list2.Count >= 34)
						{
							productInfo.IsGenerateMore = (list2[34].ToInt(0) == 1);
						}
						switch (ProductHelper.AddProduct(productInfo, dictionary, dictionary2, list3, null, false, ""))
						{
						case ProductActionStatus.Success:
							num++;
							break;
						case ProductActionStatus.AttributeError:
							num2++;
							break;
						case ProductActionStatus.DuplicateName:
							num2++;
							break;
						case ProductActionStatus.DuplicateSKU:
							num2++;
							break;
						case ProductActionStatus.SKUError:
							num2++;
							break;
						default:
							num2++;
							break;
						}
					}
					catch (Exception ex)
					{
						Globals.WriteExceptionLog(ex, null, "ImportFromLocal");
						num2++;
					}
				}
				File.Delete(this.csvPath);
				File.Delete(selectedValue);
				this.BindFiles();
				if (num2 == 0)
				{
					this.ShowMsg("此次商品批量导入操作已成功！", true);
				}
				else
				{
					this.ShowMsg("此次商品批量导入操作," + num2 + "件商品导入失败！", false);
				}
			}
		}

		public string PrepareDataFiles(params object[] initParams)
		{
			string path = (string)initParams[0];
			this._workDir = this._baseDir.CreateSubdirectory("product");
			using (ZipFile zipFile = ZipFile.Read(Path.Combine(this._baseDir.FullName, path)))
			{
				foreach (ZipEntry item in zipFile)
				{
					item.Extract(this._workDir.FullName, ExtractExistingFileAction.OverwriteSilently);
				}
			}
			FileInfo[] files = this._workDir.GetFiles("*.csv", SearchOption.TopDirectoryOnly);
			int num = 0;
			if (num < files.Length)
			{
				FileInfo fileInfo = files[num];
				this.csvPath = fileInfo.FullName;
			}
			return this._workDir.FullName;
		}

		public List<List<string>> ReadCsv(string csvName, bool hasHeader, char colSplit, Encoding encoding)
		{
			List<List<string>> list = new List<List<string>>();
			string[] array = File.ReadAllLines(csvName, encoding);
			int i = 0;
			if (hasHeader)
			{
				i = 1;
			}
			for (; i < array.Length; i++)
			{
				string text = array[i];
				List<string> list2 = new List<string>();
				string[] array2 = text.Split(colSplit);
				for (int j = 0; j < array2.Length; j++)
				{
					list2.Add(array2[j].Replace("\"", "").Replace("'", ""));
				}
				list.Add(list2);
			}
			return list;
		}

		protected void btnUpload_Click(object sender, EventArgs e)
		{
			if (!this.fileUploader.HasFile)
			{
				this.ShowMsg("请先选择一个数据包文件", false);
			}
			else if (this.fileUploader.PostedFile.ContentLength == 0 || Path.GetExtension(this.fileUploader.FileName).Trim('.').ToLower() != "zip")
			{
				this.ShowMsg("请上传正确的数据包文件", false);
			}
			else
			{
				string fileName = Path.GetFileName(this.fileUploader.PostedFile.FileName);
				this.fileUploader.PostedFile.SaveAs(Path.Combine(this._dataPath, fileName));
				this.BindFiles();
				this.dropFiles.SelectedValue = fileName;
			}
		}
	}
}
