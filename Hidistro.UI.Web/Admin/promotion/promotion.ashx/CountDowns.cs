using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class CountDowns : AdminBaseHandler
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
			case "sequence":
				this.Sequence(context);
				break;
			case "setover":
				this.SetOver(context);
				break;
			case "getstores":
				this.GetStores(context);
				break;
			case "getenablestores":
				this.GetEnableStores(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetEnableStores(HttpContext context)
		{
			int productId = context.Request.QueryString["productId"].ToInt(0);
			StoreEntityQuery query = new StoreEntityQuery
			{
				ProductId = productId,
				TagId = context.Request.QueryString["tagId"].ToInt(0),
				RegionId = context.Request.QueryString["regionId"].ToInt(0),
				Key = context.Request.QueryString["key"].ToString(),
				ActivityId = context.Request.QueryString["activityId"].ToInt(0)
			};
			List<int> allStoresForCountDowns = StoreListHelper.GetAllStoresForCountDowns(query);
			string s = base.SerializeObjectToJson(allStoresForCountDowns);
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetStores(HttpContext context)
		{
			int productId = context.Request.QueryString["productId"].ToInt(0);
			StoreEntityQuery query = new StoreEntityQuery
			{
				ProductId = productId,
				TagId = context.Request.QueryString["tagId"].ToInt(0),
				RegionId = context.Request.QueryString["regionId"].ToInt(0),
				PageIndex = context.Request.QueryString["pageIndex"].ToInt(0),
				PageSize = context.Request.QueryString["pageSize"].ToInt(0),
				Key = context.Request.QueryString["key"],
				ActivityId = context.Request.QueryString["activityId"].ToInt(0)
			};
			PageModel<StoreForPromotion> storesForCountDowns = StoreListHelper.GetStoresForCountDowns(query);
			string s = base.SerializeObjectToJson(storesForCountDowns);
			context.Response.Write(s);
			context.Response.End();
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
			CountDownQuery countDownQuery = new CountDownQuery();
			countDownQuery.ProductName = context.Request["ProductName"];
			countDownQuery.State = base.GetIntParam(context, "State", false).Value;
			countDownQuery.PageIndex = num;
			countDownQuery.PageSize = num2;
			countDownQuery.SortBy = "DisplaySequence";
			countDownQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(countDownQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(CountDownQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult countDownList = PromoteHelper.GetCountDownList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(countDownList.Data);
				dataGridViewModel.total = countDownList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					CountDownInfo countDownInfo = row.ToObject<CountDownInfo>();
					int num = 0;
					if (countDownInfo.StartDate > DateTime.Now)
					{
						num = 1;
					}
					else if (countDownInfo.EndDate < DateTime.Now)
					{
						num = 2;
					}
					row.Add("State", num);
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request["CountDownIds"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			int[] array = (from d in text.Split(',')
			select int.Parse(d)).ToArray();
			int num = 0;
			int num2 = array.Count();
			int[] array2 = array;
			foreach (int countDownId in array2)
			{
				CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(countDownId, 0);
				if (DateTime.Now >= countDownInfo.StartDate && DateTime.Now <= countDownInfo.EndDate)
				{
					if (num2 == 1)
					{
						throw new HidistroAshxException("活动正在进行中，不能删除!");
					}
				}
				else
				{
					num++;
					PromoteHelper.DeleteCountDown(countDownId);
				}
			}
			if (num > 0)
			{
				base.ReturnSuccessResult(context, $"成功删除{num}条活动!", 0, true);
				return;
			}
			throw new HidistroAshxException("选择的活动暂不可删除!");
		}

		private void Sequence(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "CountDownId", true);
			int? intParam2 = base.GetIntParam(context, "sequence", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (!intParam2.HasValue)
			{
				throw new HidistroAshxException("错误的参数：排序值");
			}
			PromoteHelper.SwapCountDownSequence(intParam.Value, intParam2.Value);
			base.ReturnSuccessResult(context, "更新排序成功", 0, true);
		}

		private void SetOver(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "CountDownId", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(intParam.Value, 0);
			if (countDownInfo == null)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (countDownInfo.StartDate > DateTime.Now)
			{
				throw new HidistroAshxException("该活动尚未开始");
			}
			if (countDownInfo.EndDate < DateTime.Now)
			{
				throw new HidistroAshxException("该活动已经结束");
			}
			PromoteHelper.SetOverCountDown(intParam.Value);
			base.ReturnSuccessResult(context, "活动提前结束成功", 0, true);
		}
	}
}
