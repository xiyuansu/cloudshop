using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class FightGroupDetails : AdminBaseHandler
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
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
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
			int? intParam = base.GetIntParam(context, "GroupStatus", true);
			FightGroupActivitiyQuery fightGroupActivitiyQuery = new FightGroupActivitiyQuery();
			if (intParam > -1)
			{
				fightGroupActivitiyQuery.GroupStatus = (FightGroupStatus?)intParam;
			}
			fightGroupActivitiyQuery.StartDate = base.GetDateTimeParam(context, "StartDate");
			fightGroupActivitiyQuery.EndDate = base.GetDateTimeParam(context, "EndDate");
			fightGroupActivitiyQuery.FightGroupActivityId = base.GetIntParam(context, "FightGroupActivityId", false).Value;
			fightGroupActivitiyQuery.PageIndex = num;
			fightGroupActivitiyQuery.PageSize = num2;
			fightGroupActivitiyQuery.SortBy = "FightGroupActivityId";
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
				PageModel<FightGroupModel> fightGroupList = VShopHelper.GetFightGroupList(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = fightGroupList.Total;
				foreach (FightGroupModel model in fightGroupList.Models)
				{
					Dictionary<string, object> dictionary = model.ToDictionary();
					string value = "";
					switch (model.GroupStatus)
					{
					case FightGroupStatus.FightGroupFail:
						value = "组团失败";
						break;
					case FightGroupStatus.FightGroupIn:
						value = "组团中";
						break;
					case FightGroupStatus.FightGroupSuccess:
						value = "组团成功";
						break;
					}
					dictionary.Add("GroupStatusText", value);
					dictionary.Add("CreateTimeText", model.CreateTime.ToDateTime());
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}
	}
}
