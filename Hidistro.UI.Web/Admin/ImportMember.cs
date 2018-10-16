using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Members)]
	public class ImportMember : AdminPage
	{
		protected RadioButton Excel;

		protected RadioButton Csv;

		protected FileUpload fileUploader;

		protected Button btnUpload;

		protected HiddenField hiddfullPath;

		protected Button btnImport;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpload.Click += this.btnUpload_Click;
		}

		private void btnUpload_Click(object sender, EventArgs e)
		{
			if (this.Excel.Checked)
			{
				if (!this.fileUploader.HasFile)
				{
					this.ShowMsg("请浏览选择本地Excel文件！", false);
					return;
				}
				if (!UploadHelper.IsExcelFile(this.fileUploader.FileName))
				{
					this.ShowMsg("您选择的文件非Excel文件！", false);
					return;
				}
			}
			else
			{
				if (!this.fileUploader.HasFile)
				{
					this.ShowMsg("请浏览选择本地Csv文件！", false);
					return;
				}
				string extension = Path.GetExtension(this.fileUploader.FileName);
				if (".csv" != extension.ToLower())
				{
					this.ShowMsg("您选择的文件非csv文件！", false);
					return;
				}
			}
			try
			{
				string value = UploadHelper.FileUpload("~/Storage/master/ImportMember", this.fileUploader);
				this.hiddfullPath.Value = value;
			}
			catch (Exception ex)
			{
				this.ShowMsg("文件上传失败！请联系管理员.错误" + ex.Message, false);
			}
		}
	}
}
