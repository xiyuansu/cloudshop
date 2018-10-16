using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class TodaySaleStatistics : StoreAdminBaseHandler
	{
		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (context.Request["flag"] == "Select")
			{
				StoreSalesStatisticsModel todaySaleStatistics = StoresHelper.GetTodaySaleStatistics(base.CurrentManager.StoreId);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						SaleAmount = todaySaleStatistics.SaleAmount.F2ToString("f2").ToDecimal(0),
						Views = todaySaleStatistics.Views,
						OrderCount = todaySaleStatistics.OrderCount,
						PayOrderCount = todaySaleStatistics.PayOrderCount
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}
	}
}
