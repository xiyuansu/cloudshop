using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class CombinationBuy : AdminBaseHandler
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
			case "end":
				this.End(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
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
			CombinationBuyInfoQuery combinationBuyInfoQuery = new CombinationBuyInfoQuery();
			combinationBuyInfoQuery.ProductName = context.Request["ProductName"];
			combinationBuyInfoQuery.Status = -1;
			int? intParam = base.GetIntParam(context, "Status", true);
			if (intParam.HasValue)
			{
				combinationBuyInfoQuery.Status = intParam.Value;
			}
			combinationBuyInfoQuery.PageIndex = num;
			combinationBuyInfoQuery.PageSize = num2;
			combinationBuyInfoQuery.SortBy = "CombinationId";
			combinationBuyInfoQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(combinationBuyInfoQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(CombinationBuyInfoQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult combinationBuyList = CombinationBuyHelper.GetCombinationBuyList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(combinationBuyList.Data);
				dataGridViewModel.total = combinationBuyList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					DataTable otherProductsImgs = CombinationBuyHelper.GetOtherProductsImgs(row["OtherProductIds"].ToString());
					List<Dictionary<string, object>> value = DataHelper.DataTableToDictionary(otherProductsImgs);
					row.Add("OtherProductsImg", value);
					DateTime t = (DateTime)row["StartDate"];
					DateTime t2 = (DateTime)row["EndDate"];
					DateTime now = DateTime.Now;
					string value2 = "";
					if (t > now)
					{
						value2 = "即将开始";
					}
					else if (t <= now && now <= t2)
					{
						value2 = "正在进行";
					}
					else if (now > t2)
					{
						value2 = "已结束";
					}
					row.Add("StatusText", value2);
					row["ThumbnailUrl40"] = base.GetImageOrDefaultImage(row["ThumbnailUrl40"], base.CurrentSiteSetting.DefaultProductImage);
					List<Dictionary<string, object>> list = row["OtherProductsImg"] as List<Dictionary<string, object>>;
					foreach (Dictionary<string, object> item in list)
					{
						item["ThumbnailUrl40"] = base.GetImageOrDefaultImage(item["ThumbnailUrl40"], base.CurrentSiteSetting.DefaultProductImage);
					}
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "CombinationId", false);
			if (intParam < 1)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (CombinationBuyHelper.DeleteCombinationBuy(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功!", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}

		public void End(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "CombinationId", false);
			if (intParam < 1)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (CombinationBuyHelper.EndCombinationBuy(intParam.Value))
			{
				base.ReturnSuccessResult(context, "结束活动成功!", 0, true);
				return;
			}
			throw new HidistroAshxException("结束活动失败!");
		}
	}
}
