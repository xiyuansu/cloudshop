using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.TransferManager;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.ProductBatchExport)]
	public class ExportToTB : AdminPage
	{
		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDates;

		protected ProductCategoriesDropDownList dropCategories;

		protected DropDownList dpTaoBao;

		protected DropDownList dropExportVersions;

		protected OnOff chkExportStock;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.dropCategories.DataBind();
			this.BindExporter();
		}

		private void BindExporter()
		{
			this.dropExportVersions.Items.Clear();
			this.dropExportVersions.Items.Add(new ListItem("请选择版本", ""));
			Dictionary<string, string> exportAdapters = TransferHelper.GetExportAdapters(new YfxTarget("1.2"), "淘宝助理");
			foreach (string key in exportAdapters.Keys)
			{
				this.dropExportVersions.Items.Add(new ListItem(exportAdapters[key], key));
			}
		}
	}
}
