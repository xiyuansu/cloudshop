using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class SaleAchievementData : StoreAdminBaseHandler
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
			if (context.Request["flag"] == "SaleAchievementData")
			{
				int storeId = base.CurrentManager.StoreId;
				int num = context.Request["TimeScope"].ToInt(0);
				if (num != 1 && num != 2 && num != 3)
				{
					num = 1;
				}
				DateTime now = DateTime.Now;
				DateTime now2 = DateTime.Now;
				DateTime dateTime;
				switch (num)
				{
				case 1:
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-6.0);
					now = dateTime.Date;
					break;
				case 2:
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-29.0);
					now = dateTime.Date;
					break;
				default:
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-89.0);
					now = dateTime.Date;
					break;
				}
				IList<StoreDaySaleAmountModel> saleAmountOfDay = StoresHelper.GetSaleAmountOfDay(storeId, now, now2);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						SaleAmount = saleAmountOfDay.Sum((StoreDaySaleAmountModel sa) => sa.SaleAmount).F2ToString("f2").ToDecimal(0),
						TopAmount = saleAmountOfDay.Max((StoreDaySaleAmountModel sa) => sa.SaleAmount).F2ToString("f2").ToDecimal(0),
						DayAverageAmount = saleAmountOfDay.Average((StoreDaySaleAmountModel sa) => sa.SaleAmount).F2ToString("f2").ToDecimal(0),
						AchievementData = from d in saleAmountOfDay
						select new
						{
							SaleAmount = d.SaleAmount.F2ToString("f2").ToDecimal(0),
							Date = d.OrderDate.ToString("yyyy-MM-dd")
						}
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}
	}
}
