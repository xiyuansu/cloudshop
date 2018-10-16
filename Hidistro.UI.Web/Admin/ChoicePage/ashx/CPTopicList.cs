using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.ChoicePage.ashx
{
	public class CPTopicList : AdminBaseHandler
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

		private TopicQuery GetDataQuery(HttpContext context)
		{
			TopicQuery topicQuery = new TopicQuery();
			topicQuery.TopicType = 2;
			topicQuery.PageSize = base.CurrentPageSize;
			topicQuery.PageIndex = base.CurrentPageIndex;
			topicQuery.SortBy = "topicId";
			topicQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(topicQuery, true);
			return topicQuery;
		}

		private void GetList(HttpContext context)
		{
			TopicQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(TopicQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult dbQueryResult = VShopHelper.GettopicList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(dbQueryResult.Data);
				dataGridViewModel.total = dbQueryResult.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}
	}
}
