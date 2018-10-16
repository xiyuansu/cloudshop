using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.TransferManager;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.ProductBatchUpload)]
	public class ImportFromTB : AdminPage
	{
		private string _dataPath;

		protected DropDownList dropImportVersions;

		protected DropDownList dropFiles;

		protected FileUpload fileUploader;

		protected Button btnUpload;

		protected ProductCategoriesDropDownList dropCategories;

		protected BrandCategoriesDropDownList dropBrandList;

		protected RadioButton radOnSales;

		protected RadioButton radUnSales;

		protected RadioButton radInStock;

		protected Button btnImport;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this._dataPath = this.Page.Request.MapPath("~/App_Data/data/taobao");
			this.btnImport.Click += this.btnImport_Click;
			this.btnUpload.Click += this.btnUpload_Click;
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropBrandList.DataBind();
				this.BindImporters();
				this.BindFiles();
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		private void BindImporters()
		{
			this.dropImportVersions.Items.Clear();
			this.dropImportVersions.Items.Add(new ListItem("请选择插件版本", ""));
			Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "淘宝助理");
			foreach (string key in importAdapters.Keys)
			{
				this.dropImportVersions.Items.Add(new ListItem(importAdapters[key], key));
			}
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

		private void btnUpload_Click(object sender, EventArgs e)
		{
			if (!this.fileUploader.HasFile)
			{
				this.ShowMsg("请先选择一个数据包文件", false);
			}
			else if (this.fileUploader.PostedFile.ContentLength == 0 || (this.fileUploader.PostedFile.ContentType != "application/x-zip-compressed" && this.fileUploader.PostedFile.ContentType != "application/zip" && this.fileUploader.PostedFile.ContentType != "application/octet-stream"))
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

		private void btnImport_Click(object sender, EventArgs e)
		{
			if (this.CheckItems())
			{
				string selectedValue = this.dropFiles.SelectedValue;
				string text = Path.Combine(this._dataPath, Path.GetFileNameWithoutExtension(selectedValue));
				ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue);
				int value = this.dropCategories.SelectedValue.Value;
				int? selectedValue2 = this.dropBrandList.SelectedValue;
				ProductSaleStatus saleStatus = ProductSaleStatus.Delete;
				if (this.radInStock.Checked)
				{
					saleStatus = ProductSaleStatus.OnStock;
				}
				if (this.radUnSales.Checked)
				{
					saleStatus = ProductSaleStatus.UnSale;
				}
				if (this.radOnSales.Checked)
				{
					saleStatus = ProductSaleStatus.OnSale;
				}
				selectedValue = Path.Combine(this._dataPath, selectedValue);
				if (!File.Exists(selectedValue))
				{
					this.ShowMsg("选择的数据包文件有问题！", false);
				}
				else
				{
					this.PrepareDataFiles(text, selectedValue);
					object[] array = importer.ParseProductData(text);
					ProductHelper.ImportProducts((DataTable)array[0], value, selectedValue2, saleStatus, true);
					File.Delete(selectedValue);
					Directory.Delete(text, true);
					this.BindFiles();
					this.ShowMsg("此次商品批量导入操作已成功！", true);
				}
			}
		}

		private void PrepareDataFiles(string dir, string filename)
		{
			using (ZipFile zipFile = ZipFile.Read(Path.Combine(filename)))
			{
				foreach (ZipEntry item in zipFile)
				{
					item.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
				}
			}
		}

		private bool CheckItems()
		{
			string text = "";
			if (this.dropImportVersions.SelectedValue.Length == 0)
			{
				text += Formatter.FormatErrorMessage("请选择一个导入插件！");
			}
			if (this.dropFiles.SelectedValue.Length == 0)
			{
				text += Formatter.FormatErrorMessage("请选择要导入的数据包文件！");
			}
			if (!this.dropCategories.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择要导入的商品分类！");
			}
			if (!string.IsNullOrEmpty(text) || text.Length > 0)
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
