using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class StoreUsersStatistics : StoreAdminBaseHandler
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
			if (context.Request["flag"] == "GetStoreUsersStatistics")
			{
				Pagination pagination = new Pagination();
				int shoppingGuiderId = 0;
				int num = context.Request["PageIndex"].ToInt(0);
				if (num < 1)
				{
					num = 1;
				}
				int num2 = context.Request["PageSize"].ToInt(0);
				if (num2 < 1)
				{
					num2 = 10;
				}
				int num3 = context.Request["GroupId"].ToInt(0);
				if (num3 != 1 && num3 != 2 && num3 != 3 && num3 != 0)
				{
					num3 = 0;
				}
				int num4 = context.Request["TimeScope"].ToInt(0);
				if (num4 != 1 && num4 != 3 && num4 != 6 && num4 != 9 && num4 != 12)
				{
					num4 = 1;
				}
				if (num3 == 2 && num4 != 1 && num4 != 3 && num4 != 6)
				{
					num4 = 1;
				}
				pagination.PageIndex = num;
				pagination.PageSize = num2;
				pagination.SortBy = "PayDate";
				pagination.SortOrder = SortAction.Desc;
				int storeId = base.CurrentManager.StoreId;
				string keyword = Globals.StripAllTags(context.Request["Keyword"].ToNullString());
				StoreMemberStatisticsQuery storeMemberStatisticsQuery = new StoreMemberStatisticsQuery();
				storeMemberStatisticsQuery.Keyword = keyword;
				storeMemberStatisticsQuery.GroupId = num3;
				storeMemberStatisticsQuery.PageIndex = num;
				storeMemberStatisticsQuery.PageSize = num2;
				storeMemberStatisticsQuery.ShoppingGuiderId = shoppingGuiderId;
				storeMemberStatisticsQuery.StoreId = storeId;
				storeMemberStatisticsQuery.TimeScope = num4;
				storeMemberStatisticsQuery.SortBy = "UserId";
				storeMemberStatisticsQuery.SortOrder = SortAction.Desc;
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				PageModel<StoreMemberStatisticsModel> storeMemberStatisticsList = MemberProcessor.GetStoreMemberStatisticsList(storeMemberStatisticsQuery, siteSettings.ConsumeTimesInOneMonth, siteSettings.ConsumeTimesInThreeMonth, siteSettings.ConsumeTimesInSixMonth);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = storeMemberStatisticsList.Total,
						List = from u in storeMemberStatisticsList.Models
						select new
						{
							UserId = u.UserId,
							NickName = (string.IsNullOrEmpty(u.NickName) ? (string.IsNullOrEmpty(u.RealName) ? u.UserName : u.RealName) : u.NickName),
							UserName = u.UserName,
							LastConsumeDate = (u.LastConsumeDate.HasValue ? u.LastConsumeDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""),
							ConsumeTimes = u.ConsumeTimes,
							ConsumeTotal = u.ConsumeTotal.F2ToString("f2").ToDecimal(0),
							HeadImage = (string.IsNullOrEmpty(u.HeadImage.ToNullString()) ? Globals.FullPath("/templates/common/images/headerimg.png") : Globals.FullPath(u.HeadImage.ToNullString()))
						}
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private string GetErrorJosn(int errorCode, string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				ErrorResponse = new
				{
					ErrorCode = errorCode,
					ErrorMsg = errorMsg
				}
			});
		}
	}
}
