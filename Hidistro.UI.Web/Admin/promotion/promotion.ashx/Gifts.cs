using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class Gifts : AdminBaseHandler
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
				if (action == "delete")
				{
					this.Delete(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
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
			GiftQuery giftQuery = new GiftQuery();
			giftQuery.Name = context.Request["Name"];
			giftQuery.IsPromotion = base.GetBoolParam(context, "IsPromotion", false).Value;
			giftQuery.Page.PageIndex = num;
			giftQuery.Page.PageSize = num2;
			giftQuery.Page.SortBy = "GiftId";
			giftQuery.Page.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(giftQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(GiftQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult gifts = GiftHelper.GetGifts(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(gifts.Data);
				dataGridViewModel.total = gifts.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row["ThumbnailUrl40"] = base.GetImageOrDefaultImage(row["ThumbnailUrl40"], base.CurrentSiteSetting.DefaultProductImage);
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request["ids"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			int[] array = (from d in text.Split(',')
			select int.Parse(d)).ToArray();
			int num = 0;
			int num2 = array.Count();
			int[] array2 = array;
			foreach (int giftId in array2)
			{
				if (GiftHelper.DeleteGift(giftId))
				{
					num++;
				}
			}
			if (num > 0)
			{
				if (num2 == 1)
				{
					base.ReturnSuccessResult(context, "删除成功", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, $"成功删除{num}条记录", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除失败");
		}
	}
}
