using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class StoresList : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (base.action.ToLower())
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "deletes":
				this.Deletes(context);
				break;
			case "release":
				this.Release(context);
				break;
			case "setstorestate":
				this.SetStoreState(context);
				break;
			case "exportexcel":
				this.ExportToExcel(context);
				break;
			case "cleartradepassword":
				this.ClearTradePassword(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void ClearTradePassword(HttpContext context)
		{
			int value = base.GetIntParam(context, "StoreId", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			StoresInfo storeById = StoresHelper.GetStoreById(value);
			if (storeById == null)
			{
				throw new HidistroAshxException("错误的门店ID");
			}
			storeById.TradePassword = "";
			storeById.TradePasswordSalt = "";
			if (StoresHelper.UpdateStore(storeById))
			{
				base.ReturnSuccessResult(context, "清空成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("清空失败！");
		}

		public void ExportToExcel(HttpContext context)
		{
			StoresQuery dataQuery = this.GetDataQuery(context);
			IList<StoresModel> storeExportData = ManagerHelper.GetStoreExportData(dataQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<table border='1'>");
			stringBuilder.Append("<thead><tr>");
			stringBuilder.Append("<th>门店名称</th>");
			stringBuilder.Append("<th>抽佣比例</th>");
			stringBuilder.Append("<th>用户名</th>");
			stringBuilder.Append("<th>联系人</th>");
			stringBuilder.Append("<th>联系电话</th>");
			stringBuilder.Append("<th>所在区域</th>");
			stringBuilder.Append("<th>详细地址</th>");
			stringBuilder.Append("<th>门店标签</th>");
			stringBuilder.Append("<th>配送方式</th>");
			stringBuilder.Append("<th>营业时间</th>");
			stringBuilder.Append("<th>门店LOGO</th>");
			stringBuilder.Append("</tr></thead>");
			StringBuilder stringBuilder2 = new StringBuilder();
			DateTime dateTime;
			foreach (StoresModel item in storeExportData)
			{
				stringBuilder2.Append("<tr>");
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.StoreName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.CommissionRate + "%", false));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.UserName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.ContactMan, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.Tel, false));
				string fullRegion = RegionHelper.GetFullRegion(item.RegionId, " ", true, 0);
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(fullRegion, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(fullRegion + " " + item.Address, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.TagsName, true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(item.DeliveryTypes, true));
				StringBuilder stringBuilder3 = stringBuilder2;
				dateTime = item.OpenStartDate;
				string str = dateTime.ToString("HH:mm:ss");
				dateTime = item.OpenEndDate;
				stringBuilder3.Append(ExcelHelper.GetXLSFieldsTD(str + "-" + dateTime.ToString("HH:mm:ss"), true));
				stringBuilder2.Append(ExcelHelper.GetXLSFieldsTD(Globals.FullPath(item.StoreImages), true));
				stringBuilder2.Append("</tr>");
			}
			stringBuilder.AppendFormat("<tbody>{0}</tbody></table>", stringBuilder2.ToString());
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write(stringBuilder);
			HttpResponse response = context.Response;
			StringBuilder stringBuilder4 = stringWriter.GetStringBuilder();
			dateTime = DateTime.Now;
			DownloadHelper.DownloadFile(response, stringBuilder4, "StoreList" + dateTime.ToString("yyyyMMddhhmmss") + ".xls");
			stringWriter.Close();
			context.Response.End();
		}

		public void SetStoreState(HttpContext context)
		{
			int value = base.GetIntParam(context, "StoreId", false).Value;
			StoresInfo storeById = StoresHelper.GetStoreById(value);
			if (storeById == null)
			{
				throw new HidistroAshxException("错误的参数" + value);
			}
			storeById.State = ((storeById.State != 1) ? 1 : 0);
			StoresHelper.UpdateStore(storeById);
			HiCache.Remove("DataCache-StoreInfoDataKey");
			base.ReturnSuccessResult(context, "设置门店状态成功", 0, true);
		}

		public StoresQuery GetDataQuery(HttpContext context)
		{
			StoresQuery storesQuery = new StoresQuery();
			int num = 1;
			int num2 = 10;
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			storesQuery.UserName = context.Request["UserName"];
			storesQuery.StoreName = context.Request["StoreName"];
			if (!string.IsNullOrEmpty(context.Request["RegionId"]))
			{
				int num3 = context.Request["RegionID"].ToInt(0);
				storesQuery.RegionID = num3;
				storesQuery.RegionName = RegionHelper.GetFullRegion(num3, " ", true, 0);
			}
			if (!string.IsNullOrEmpty(context.Request["tagId"]))
			{
				storesQuery.tagId = context.Request["tagId"].ToInt(0);
			}
			storesQuery.SortOrder = SortAction.Desc;
			storesQuery.PageIndex = num;
			storesQuery.PageSize = num2;
			return storesQuery;
		}

		private void GetList(HttpContext context)
		{
			StoresQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(dataQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(StoresQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult storeManagers = ManagerHelper.GetStoreManagers(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(storeManagers.Data);
				dataGridViewModel.total = storeManagers.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					ManagerInfo managerInfo = row.ToObject<ManagerInfo>();
					row.Add("StoreDeliveryScop", ManagerHelper.GetStoreDeliveryScop(managerInfo.StoreId));
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "articleId", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (ArticleHelper.DeleteArticle(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败！");
		}

		private void Deletes(HttpContext context)
		{
			string text = context.Request["ids"];
			IList<int> list = new List<int>();
			if (text.Length < 0)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			string[] array = text.Split(',');
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					list.Add(text2.ToInt(0));
				}
			}
			int num = ArticleHelper.DeleteArticles(list);
			if (num > 0)
			{
				HiCache.Remove("DataCache-StoreInfoDataKey");
				base.ReturnSuccessResult(context, $"成功删除{num}个门店", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败！");
		}

		private void Release(HttpContext context)
		{
			bool value = base.GetBoolParam(context, "IsRelease", false).Value;
			int value2 = base.GetIntParam(context, "ArticleId", false).Value;
			bool isRelease = false;
			string arg = "取消";
			if (!value)
			{
				isRelease = true;
				arg = "发布";
			}
			if (ArticleHelper.UpdateRelease(value2, isRelease))
			{
				base.ReturnSuccessResult(context, $"{arg}当前文章成功", 0, true);
				return;
			}
			throw new HidistroAshxException($"{arg}当前文章失败");
		}
	}
}
