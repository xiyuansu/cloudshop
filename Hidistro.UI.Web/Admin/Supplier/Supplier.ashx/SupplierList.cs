using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Supplier;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier.ashx
{
	public class SupplierList : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "exportexcel")
				{
					this.ExportToExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void ExportToExcel(HttpContext context)
		{
			SupplierQuery dataQuery = this.GetDataQuery(context);
			IList<SupplierExportModel> supplierExportData = SupplierHelper.GetSupplierExportData(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>用户名</th>");
			stringBuilder.Append("<th>供应商名称</th>");
			stringBuilder.Append("<th>联系人</th>");
			stringBuilder.Append("<th>联系电话</th>");
			stringBuilder.Append("<th>上架商品数</th>");
			stringBuilder.Append("<th>订单数</th>");
			stringBuilder.Append("<th>状态</th>");
			stringBuilder.Append("<th>详细地址</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (SupplierExportModel item in supplierExportData)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.SupplierName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ContactMan, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Tel, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ProductNums, false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.OrderNums, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.StatusText, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(RegionHelper.GetFullRegion(item.RegionId, " ", true, 0) + " " + item.Address, true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			DownloadHelper.DownloadFile(context.Response, stringWriter.GetStringBuilder(), "SupplierList" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		public void GetList(HttpContext context)
		{
			SupplierQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private SupplierQuery GetDataQuery(HttpContext context)
		{
			SupplierQuery supplierQuery = new SupplierQuery();
			supplierQuery.UserName = base.GetParameter(context, "UserName", true);
			supplierQuery.SupplierName = base.GetParameter(context, "SupplierName", true);
			supplierQuery.Status = base.GetIntParam(context, "Status", true);
			supplierQuery.PageIndex = base.CurrentPageIndex;
			supplierQuery.PageSize = base.CurrentPageSize;
			return supplierQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(SupplierQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult supplierManagers = SupplierHelper.GetSupplierManagers(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(supplierManagers.Data);
				dataGridViewModel.total = supplierManagers.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
