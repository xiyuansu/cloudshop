using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.SqlDal.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class BalanceDetail : AdminBaseHandler
	{
		private string sError = string.Empty;

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
				if (action == "exportexcel")
				{
					this.ExportExcel(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void GetList(HttpContext context)
		{
			StoreBalanceDetailQuery dataQuery = this.GetDataQuery(context);
			PageModel<StoreBalanceDetailInfo> balanceDetails = StoreBalanceHelper.GetBalanceDetails(dataQuery);
			string s = base.SerializeObjectToJson(new
			{
				Result = new
				{
					total = balanceDetails.Total,
					rows = from b in balanceDetails.Models
					select new
					{
						Income = (b.Income.HasValue ? b.Income.Value.F2ToString("f2").ToDecimal(0) : decimal.Zero),
						Expenses = (b.Expenses.HasValue ? b.Expenses.Value.F2ToString("f2").ToDecimal(0) : decimal.Zero),
						TradeType = b.TradeType,
						TradeTypeText = b.TradeTypeText,
						TradeDate = b.TradeDate.ToString("yyyy-MM-dd HH:mm"),
						Balance = b.Balance,
						TradeNo = b.TradeNo,
						UserName = ((new ManagerDao().FindManagerByStoreId(b.StoreId, SystemRoles.StoreAdmin) != null) ? new ManagerDao().FindManagerByStoreId(b.StoreId, SystemRoles.StoreAdmin).UserName : "")
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private StoreBalanceDetailQuery GetDataQuery(HttpContext context)
		{
			StoreBalanceDetailQuery storeBalanceDetailQuery = new StoreBalanceDetailQuery();
			storeBalanceDetailQuery.StoreId = base.GetIntParam(context, "StoreId", false);
			storeBalanceDetailQuery.FromDate = base.GetDateTimeParam(context, "FromDate");
			storeBalanceDetailQuery.ToDate = base.GetDateTimeParam(context, "ToDate");
			storeBalanceDetailQuery.TradeType = (StoreTradeTypes)base.GetIntParam(context, "TradeType", false).Value;
			storeBalanceDetailQuery.PageIndex = base.CurrentPageIndex;
			storeBalanceDetailQuery.PageSize = base.CurrentPageSize;
			return storeBalanceDetailQuery;
		}

		public void ExportExcel(HttpContext context)
		{
			StoreBalanceDetailQuery dataQuery = this.GetDataQuery(context);
			StoresInfo storeById = DepotHelper.GetStoreById(dataQuery.StoreId.Value);
			IList<StoreBalanceDetailInfo> balanceDetails4Report = StoreBalanceHelper.GetBalanceDetails4Report(dataQuery);
			StringBuilder stringBuilder = new StringBuilder(300);
			stringBuilder.Append("门店");
			stringBuilder.Append(",时间");
			stringBuilder.Append(",类型");
			stringBuilder.Append(",订单号");
			stringBuilder.Append(",收入");
			stringBuilder.Append(",支出");
			stringBuilder.Append(",账户余额");
			stringBuilder.Append(",备注\r\n");
			foreach (StoreBalanceDetailInfo item in balanceDetails4Report)
			{
				stringBuilder.Append(storeById.StoreName);
				stringBuilder.Append("," + item.TradeDate);
				stringBuilder.Append("," + item.TradeTypeText);
				stringBuilder.Append("," + item.TradeNo);
				stringBuilder.Append("," + item.Income);
				stringBuilder.Append("," + item.Expenses);
				stringBuilder.Append("," + item.Balance);
				stringBuilder.Append("," + item.Remark + "\r\n");
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			context.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetail.csv");
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.ContentType = "application/octet-stream";
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
		}
	}
}
