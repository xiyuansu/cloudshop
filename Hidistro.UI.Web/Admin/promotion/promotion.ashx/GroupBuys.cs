using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class GroupBuys : AdminBaseHandler
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
			GroupBuyQuery groupBuyQuery = new GroupBuyQuery();
			groupBuyQuery.ProductName = context.Request["ProductName"];
			groupBuyQuery.PageIndex = num;
			groupBuyQuery.PageSize = num2;
			groupBuyQuery.SortBy = "DisplaySequence";
			groupBuyQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(groupBuyQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(GroupBuyQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult groupBuyList = PromoteHelper.GetGroupBuyList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(groupBuyList.Data);
				dataGridViewModel.total = groupBuyList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					GroupBuyInfo groupBuyInfo = row.ToObject<GroupBuyInfo>();
					decimal currentPrice = PromoteHelper.GetCurrentPrice(groupBuyInfo.GroupBuyId);
					row.Add("CurrentPrice", currentPrice);
					row.Add("StatusText", groupBuyInfo.StatusText);
					row.Add("CanDelete", (groupBuyInfo.Status != GroupBuyStatus.UnderWay || !(groupBuyInfo.StartDate <= DateTime.Now) || !(groupBuyInfo.EndDate >= DateTime.Now)) && groupBuyInfo.Status != GroupBuyStatus.EndUntreated);
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request["GroupBuyIds"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			int[] array = (from d in text.Split(',')
			select int.Parse(d)).ToArray();
			int num = 0;
			int num2 = array.Count();
			int[] array2 = array;
			foreach (int groupBuyId in array2)
			{
				GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(groupBuyId);
				if ((groupBuy.Status == GroupBuyStatus.UnderWay && groupBuy.StartDate <= DateTime.Now && groupBuy.EndDate >= DateTime.Now) || groupBuy.Status == GroupBuyStatus.EndUntreated)
				{
					if (num2 == 1)
					{
						throw new HidistroAshxException("团购活动正在进行中或结束未处理，不允许删除!");
					}
				}
				else
				{
					num++;
					PromoteHelper.DeleteGroupBuy(groupBuyId);
				}
			}
			if (num > 0)
			{
				base.ReturnSuccessResult(context, $"成功删除{num}条团购活动!", 0, true);
				return;
			}
			throw new HidistroAshxException("选择的团购活动暂不可删除!");
		}

		private void Sequence(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "GroupBuyId", true);
			int? intParam2 = base.GetIntParam(context, "sequence", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (!intParam2.HasValue)
			{
				throw new HidistroAshxException("错误的参数：排序值");
			}
			PromoteHelper.SwapGroupBuySequence(intParam.Value, intParam2.Value);
			base.ReturnSuccessResult(context, "更新排序成功", 0, true);
		}
	}
}
