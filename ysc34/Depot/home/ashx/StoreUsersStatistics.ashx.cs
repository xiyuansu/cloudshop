using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Hidistro.UI.Web.ashxBase;
using Hidistro.SaleSystem.Store;
using Hidistro.Entities.Store;
using Hidistro.Entities;
using Hidistro.SaleSystem.Depot;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Context;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using System.Data;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.Core.Entities;

namespace Hidistro.UI.Web.Depot.home.ashx
{
    /// <summary>
    /// StoreUsersStatistics 的摘要说明
    /// </summary>
    public class StoreUsersStatistics : StoreAdminBaseHandler
    {
        public override void OnLoad(HttpContext context)
        {
            base.OnLoad(context);
            if (context.Request["flag"] == "GetStoreUsersStatistics")
            {
                Pagination page = new Pagination();
                int shoppingGuiderId = 0;
                int pageIndex = context.Request["PageIndex"].ToInt();
                if (pageIndex < 1) { pageIndex = 1; }
                int pageSize = context.Request["PageSize"].ToInt();
                if (pageSize < 1) { pageSize = 10; }
                int GroupId = context.Request["GroupId"].ToInt();
                if (GroupId != 1 && GroupId != 2 && GroupId != 3 && GroupId != 0)
                {
                    GroupId = 0;
                }
                int TimeScope = context.Request["TimeScope"].ToInt();
                if (TimeScope != 1 && TimeScope != 3 && TimeScope != 6 && TimeScope != 9 && TimeScope != 12)
                {
                    TimeScope = 1;
                }
                if (GroupId == 2 && TimeScope != 1 && TimeScope != 3 && TimeScope != 6)
                {
                    TimeScope = 1;
                }
                page.PageIndex = pageIndex;
                page.PageSize = pageSize;
                page.SortBy = "PayDate";
                page.SortOrder = SortAction.Desc;
                Int32 storeId = CurrentManager.StoreId;
                string keyword = Globals.StripAllTags(context.Request["Keyword"].ToNullString());

                StoreMemberStatisticsQuery query = new StoreMemberStatisticsQuery();
                query.Keyword = keyword;
                query.GroupId = GroupId;
                query.PageIndex = pageIndex;
                query.PageSize = pageSize;
                query.ShoppingGuiderId = shoppingGuiderId;
                query.StoreId = storeId;
                query.TimeScope = TimeScope;
                query.SortBy = "UserId";
                query.SortOrder = SortAction.Desc;
                SiteSettings setting = HiContext.Current.SiteSettings;
                PageModel<StoreMemberStatisticsModel> data = MemberProcessor.GetStoreMemberStatisticsList(query, setting.ConsumeTimesInOneMonth, setting.ConsumeTimesInThreeMonth, setting.ConsumeTimesInSixMonth);
                var json = JsonConvert.SerializeObject(new
                {
                    Result = new
                    {
                        RecordCount = data.Total,
                        List = data.Models.Select(u => new
                        {
                            u.UserId,
                            NickName = string.IsNullOrEmpty(u.NickName) ? (string.IsNullOrEmpty(u.RealName) ? u.UserName : u.RealName) : u.NickName,
                            u.UserName,
                            LastConsumeDate = u.LastConsumeDate.HasValue ? u.LastConsumeDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                            u.ConsumeTimes,
                            ConsumeTotal = u.ConsumeTotal.F2ToString("f2").ToDecimal(),
                            HeadImage = string.IsNullOrEmpty(u.HeadImage.ToNullString()) ? Globals.FullPath("/templates/common/images/headerimg.png") : Globals.FullPath(u.HeadImage.ToNullString()),

                        })
                    }
                });
                context.Response.Write(json);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取错误代码
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        string GetErrorJosn(Int32 errorCode, string errorMsg)
        {
            var message = JsonConvert.SerializeObject(new
            {
                ErrorResponse = new
                {
                    ErrorCode = errorCode,
                    ErrorMsg = errorMsg
                }
            });
            return message;
        }
    }
}