using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin.tools.ashx
{
	public class RestoreDatabase : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "restore":
				this.Restore(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataTable backupFiles = StoreHelper.GetBackupFiles();
			if (backupFiles != null && backupFiles.Rows != null && backupFiles.Rows.Count > 0)
			{
				backupFiles.DefaultView.Sort = "BackupTime DESC";
				backupFiles = backupFiles.DefaultView.ToTable();
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(backupFiles);
				dataGridViewModel.total = backupFiles.Rows.Count;
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			string parameter = base.GetParameter(context, "BackupName", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("错误的参数");
			}
			parameter = parameter.Trim();
			string path = HttpContext.Current.Request.MapPath("/Storage/data/Backup/" + parameter);
			if (StoreHelper.DeleteBackupFile(parameter))
			{
				File.Delete(path);
				base.ReturnSuccessResult(context, "成功删除了选择的备份文件", 0, true);
				return;
			}
			throw new HidistroAshxException("未知错误");
		}

		public void Restore(HttpContext context)
		{
			string parameter = base.GetParameter(context, "BackupName", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("错误的参数");
			}
			parameter = parameter.Trim();
			string bakFullName = HttpContext.Current.Request.MapPath("/Storage/data/Backup/" + parameter);
			if (StoreHelper.RestoreData(bakFullName))
			{
				base.ReturnSuccessResult(context, "数据库已恢复完毕", 0, true);
				return;
			}
			throw new HidistroAshxException("数据库恢复失败，请重试");
		}
	}
}
