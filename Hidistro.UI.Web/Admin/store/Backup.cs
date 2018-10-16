using Hidistro.Context;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.store
{
	[AdministerCheck(true)]
	public class Backup : AdminPage
	{
		protected Button btnBackup;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnBackup.Click += this.btnBackup_Click;
		}

		private void btnBackup_Click(object sender, EventArgs e)
		{
			string text = StoreHelper.BackupData();
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = HttpContext.Current.Request.MapPath("/Storage/data/Backup/" + text);
				FileInfo fileInfo = new FileInfo(text2);
				if (StoreHelper.InserBackInfo(text, HiContext.Current.Config.Version, fileInfo.Length))
				{
					this.ShowMsg("备份数据成功", true);
				}
				else
				{
					File.Delete(text2);
					this.ShowMsg("备份数据失败，可能是同时备份的人太多，请重试", false);
				}
			}
			else
			{
				this.ShowMsg("备份数据失败，可能是您的数据库服务器和web服务器不是同一台服务器", false);
			}
		}
	}
}
