using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class Votes : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
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

		private void GetList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataSet votes = StoreHelper.GetVotes();
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(votes.Tables[0]);
			dataGridViewModel.total = votes.Tables[0].Rows.Count;
			foreach (Dictionary<string, object> row in dataGridViewModel.rows)
			{
				int num = row["VoteId"].ToInt(0);
				VoteInfo voteById = StoreHelper.GetVoteById(num);
				IList<VoteItemInfo> voteItems = StoreHelper.GetVoteItems(num);
				List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
				foreach (VoteItemInfo item in voteItems)
				{
					list.Add(item.ToDictionary());
				}
				row.Add("VoteItems", list);
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (StoreHelper.DeleteVote(value))
			{
				base.ReturnSuccessResult(context, "成功删除了选择的投票！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除投票失败");
		}
	}
}
