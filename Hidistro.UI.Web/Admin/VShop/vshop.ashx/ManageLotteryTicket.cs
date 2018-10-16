using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.vshop.ashx
{
	public class ManageLotteryTicket : AdminBaseHandler
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
			case "setstatus":
				this.SetStatus(context);
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
			LotteryActivityQuery lotteryActivityQuery = new LotteryActivityQuery();
			lotteryActivityQuery.ActivityType = LotteryActivityType.Ticket;
			lotteryActivityQuery.PageIndex = num;
			lotteryActivityQuery.PageSize = num2;
			lotteryActivityQuery.SortBy = "ActivityId";
			lotteryActivityQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(lotteryActivityQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(LotteryActivityQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult lotteryTicketList = VShopHelper.GetLotteryTicketList(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(lotteryTicketList.Data);
				dataGridViewModel.total = lotteryTicketList.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					DateTime t = Convert.ToDateTime(row["StartTime"]);
					DateTime t2 = Convert.ToDateTime(row["EndTime"]);
					DateTime t3 = Convert.ToDateTime(row["OpenTime"]);
					row.Add("CanStart", t < DateTime.Now && t3 > DateTime.Now && t2 > DateTime.Now);
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (VShopHelper.DelteLotteryTicket(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}

		private void SetStatus(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(intParam.Value);
			if (lotteryTicket.OpenTime > DateTime.Now)
			{
				lotteryTicket.OpenTime = DateTime.Now;
				if (VShopHelper.UpdateLotteryTicket(lotteryTicket))
				{
					base.ReturnSuccessResult(context, "操作成功", 0, true);
					goto IL_0083;
				}
				throw new HidistroAshxException("操作失败");
			}
			goto IL_0083;
			IL_0083:
			throw new HidistroAshxException("状态属于不可改变状态");
		}
	}
}
