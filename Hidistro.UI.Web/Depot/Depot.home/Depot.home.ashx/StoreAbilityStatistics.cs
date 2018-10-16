using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class StoreAbilityStatistics : StoreAdminBaseHandler
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
			if (context.Request["flag"] == "StoreAbilityStatistics")
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
					now = dateTime.Date;
					break;
				case 2:
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-6.0);
					now = dateTime.Date;
					break;
				default:
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-89.0);
					now = dateTime.Date;
					break;
				}
				StoreAbilityStatisticsInfo abilityStatisticsInfo = StoresHelper.GetAbilityStatisticsInfo(storeId, 0, now, now2);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						SaleQuantity = abilityStatisticsInfo.SaleQuantity,
						PayOrderCount = abilityStatisticsInfo.PayNoRefundOrderCount,
						JointRate = abilityStatisticsInfo.JointRate.F2ToString("f2").ToDecimal(0),
						UnitPrice = abilityStatisticsInfo.UnitPrice.F2ToString("f2").ToDecimal(0),
						GuestUnitPrice = abilityStatisticsInfo.GuestUnitPrice.F2ToString("f2").ToDecimal(0),
						MemberCount = abilityStatisticsInfo.MemberCount
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}
	}
}
