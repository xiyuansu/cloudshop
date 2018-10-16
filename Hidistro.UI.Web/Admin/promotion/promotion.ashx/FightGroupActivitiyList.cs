using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class FightGroupActivitiyList : AdminBaseHandler
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

		private void Sequence(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "FightGroupActivityId", true);
			int? intParam2 = base.GetIntParam(context, "sequence", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (!intParam2.HasValue)
			{
				throw new HidistroAshxException("错误的参数：排序值");
			}
			VShopHelper.SwapFightGroupActivitySequence(intParam.Value, intParam2.Value);
			base.ReturnSuccessResult(context, "更新排序成功", 0, true);
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
			FightGroupActivitiyQuery fightGroupActivitiyQuery = new FightGroupActivitiyQuery();
			fightGroupActivitiyQuery.ProductName = context.Request["ProductName"];
			fightGroupActivitiyQuery.Status = (EnumFightGroupActivitiyStatus)base.GetIntParam(context, "Status", false).Value;
			fightGroupActivitiyQuery.PageIndex = num;
			fightGroupActivitiyQuery.PageSize = num2;
			fightGroupActivitiyQuery.SortBy = "DisplaySequence DESC,FightGroupActivityId";
			fightGroupActivitiyQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(fightGroupActivitiyQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(FightGroupActivitiyQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<FightGroupActivitiyModel> fightGroupActivities = VShopHelper.GetFightGroupActivities(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = fightGroupActivities.Total;
				foreach (FightGroupActivitiyModel model in fightGroupActivities.Models)
				{
					Dictionary<string, object> item = model.ToDictionary();
					dataGridViewModel.rows.Add(item);
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			DateTime now = DateTime.Now;
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(value);
			if (fightGroupActivitieInfo.StartDate > now)
			{
				VShopHelper.DeleteFightGroupActivitie(value);
				base.ReturnSuccessResult(context, "拼团活动删除成功", 0, true);
			}
			else
			{
				if (fightGroupActivitieInfo.StartDate <= now && fightGroupActivitieInfo.EndDate >= now)
				{
					throw new HidistroAshxException("拼团活动正在进行中，不能删除");
				}
				if (fightGroupActivitieInfo.EndDate < now)
				{
					if (VShopHelper.isEndFightCannotDel(value))
					{
						throw new HidistroAshxException("此活动已经开团，不能删除");
					}
					try
					{
						VShopHelper.DeleteFightGroupActivitie(value);
						base.ReturnSuccessResult(context, "拼团活动删除成功", 0, true);
					}
					catch (Exception)
					{
						throw new HidistroAshxException("活动已产生了订单（含待付款）不能删除");
					}
				}
			}
		}
	}
}
