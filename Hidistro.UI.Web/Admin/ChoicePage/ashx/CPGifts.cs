using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.ChoicePage.ashx
{
	public class CPGifts : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			string action = base.action;
			if (action == "getlist")
			{
				this.GetList(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}

		private GiftQuery GetDataQuery(HttpContext context)
		{
			GiftQuery giftQuery = new GiftQuery();
			giftQuery.Name = base.GetParameter(context, "Name", true);
			giftQuery.FilterGiftIds = base.GetParameter(context, "FilterGiftIds", true);
			giftQuery.IsPromotion = true;
			giftQuery.Page.PageSize = base.CurrentPageSize;
			giftQuery.Page.PageIndex = base.CurrentPageIndex;
			giftQuery.Page.SortBy = "GiftId";
			giftQuery.Page.SortOrder = SortAction.Desc;
			return giftQuery;
		}

		private void GetList(HttpContext context)
		{
			GiftQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(GiftQuery query)
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
					if (row["CostPrice"] == null || Convert.IsDBNull(row["CostPrice"]))
					{
						row["CostPrice"] = 0;
					}
				}
			}
			return dataGridViewModel;
		}
	}
}
