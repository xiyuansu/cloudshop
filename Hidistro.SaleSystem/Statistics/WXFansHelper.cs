using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Statistics;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Statistics;
using Newtonsoft.Json;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.SaleSystem.Statistics
{
	public static class WXFansHelper
	{
		private const string getUserSummaryUrl = "https://api.weixin.qq.com/datacube/getusersummary?access_token={0}";

		private const string getUserCumulateUrl = "https://api.weixin.qq.com/datacube/getusercumulate?access_token={0}";

		private const string getUpstreamMsgUrl = "https://api.weixin.qq.com/datacube/getupstreammsg?access_token={0}";

		public static void ClearFansData()
		{
			new WXFansInteractStatisticsDao().ClearAllData();
			new WXFansStatisticsDao().ClearAllData();
		}

		public static void ClearFansInteractData()
		{
			new WXFansInteractStatisticsDao().ClearAllData();
		}

		public static void ClearFansStatisticsData()
		{
			new WXFansStatisticsDao().ClearAllData();
		}

		public static bool UpdateClickStatistical(string fromUserName, int menuId)
		{
			List<WXMenuClickInfo> list = (List<WXMenuClickInfo>)HiCache.Get("DataCache-WXMenuClickRecords");
			WXMenuClickInfo wXMenuClickInfo = new WXMenuClickInfo();
			if (list == null)
			{
				list = new List<WXMenuClickInfo>();
			}
			wXMenuClickInfo.WXOpenId = fromUserName;
			wXMenuClickInfo.ClickDate = DateTime.Now;
			wXMenuClickInfo.MenuId = menuId;
			list.Add(wXMenuClickInfo);
			HiCache.Remove("DataCache-WXMenuClickRecords");
			HiCache.Insert("DataCache-WXMenuClickRecords", list);
			return true;
		}

		public static PageModel<WXFansStatisticsInfo> GetWxFansList(WXFansStatisticsQuery query)
		{
			return new WXFansStatisticsDao().GetWxFansList(query);
		}

		public static IList<WXFansStatisticsInfo> GetWxFansListNoPage(WXFansStatisticsQuery query)
		{
			return new WXFansStatisticsDao().GetWxFansListNoPage(query);
		}

		public static PageModel<WXFansInteractStatisticsInfo> GetWxFansInteractList(WXFansStatisticsQuery query)
		{
			return new WXFansInteractStatisticsDao().GetWxFansInteractList(query);
		}

		public static IList<WXFansInteractStatisticsInfo> GetWxFansInteractListNoPage(WXFansStatisticsQuery query)
		{
			return new WXFansInteractStatisticsDao().GetWxFansInteractListNoPage(query);
		}

		public static int GetMemberCount()
		{
			return new MemberDao().GetMemberCount();
		}

		public static int GetMemberFansCount()
		{
			return new MemberDao().GetMemberFansCount();
		}

		public static int GetWxFansCount()
		{
			string token = WXFansHelper.GetToken();
			if (string.IsNullOrEmpty(token))
			{
				return 0;
			}
			IList<WXUserCumulate> list = new List<WXUserCumulate>();
			string url = $"https://api.weixin.qq.com/datacube/getusercumulate?access_token={token}";
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			DateTime date = dateTime.Date;
			dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			string wXPostResult = WXFansHelper.GetWXPostResult(url, WXFansHelper.GetPostData(date, dateTime.Date));
			if (!string.IsNullOrEmpty(wXPostResult))
			{
				WXUserCumulateResult wXUserCumulateResult = JsonHelper.ParseFormJson<WXUserCumulateResult>(wXPostResult);
				if (wXUserCumulateResult != null && wXUserCumulateResult.list != null && wXUserCumulateResult.list.Count > 0)
				{
					list = list.Concat(wXUserCumulateResult.list).ToList();
				}
			}
			if (list.Count > 0)
			{
				return list.Sum((WXUserCumulate u) => u.cumulate_user);
			}
			return 0;
		}

		public static int SynchroWXFansData(DateTime startDate, DateTime endDate, out bool isSettingErr)
		{
			isSettingErr = false;
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("startDate", startDate.ToString());
			dictionary.Add("endDate", endDate.ToString());
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			dictionary.Add("AppId", masterSettings.WeixinAppId);
			dictionary.Add("AppSecret", masterSettings.WeixinAppSecret);
			try
			{
				string token = WXFansHelper.GetToken();
				if (string.IsNullOrEmpty(token))
				{
					isSettingErr = true;
					Globals.AppendLog(dictionary, "token为空", "", "", "SynchroWXFansData");
					return 0;
				}
				DateTime dateTime = startDate;
				DateTime t = dateTime;
				int num = (endDate - startDate).Days + 1;
				int num2 = (num % 7 == 0) ? (num / 7).ToInt(0) : ((num / 7).ToInt(0) + 1);
				IList<WXUserSummary> list = new List<WXUserSummary>();
				IList<WXUserCumulate> list2 = new List<WXUserCumulate>();
				DateTime dateTime2;
				for (int i = 0; i < num2; i++)
				{
					dateTime2 = t.AddDays((double)((i != 0) ? 1 : 0));
					dateTime = dateTime2.Date;
					dateTime2 = dateTime.AddDays(6.0);
					t = dateTime2.Date;
					if (t > endDate)
					{
						t = endDate;
					}
					string wXPostResult = WXFansHelper.GetWXPostResult($"https://api.weixin.qq.com/datacube/getusersummary?access_token={token}", WXFansHelper.GetPostData(dateTime.Date, t.Date));
					if (!string.IsNullOrEmpty(wXPostResult))
					{
						WXUserSummaryResult wXUserSummaryResult = JsonHelper.ParseFormJson<WXUserSummaryResult>(wXPostResult);
						if (wXUserSummaryResult != null && wXUserSummaryResult.list != null)
						{
							list = list.Concat(wXUserSummaryResult.list).ToList();
						}
					}
					else
					{
						Globals.AppendLog(dictionary, "获取粉丝增减数据为空", "", "", "SynchroWXFansData");
					}
					wXPostResult = WXFansHelper.GetWXPostResult($"https://api.weixin.qq.com/datacube/getusercumulate?access_token={token}", WXFansHelper.GetPostData(dateTime.Date, t.Date));
					if (string.IsNullOrEmpty(wXPostResult))
					{
						return 0;
					}
					WXUserCumulateResult wXUserCumulateResult = JsonHelper.ParseFormJson<WXUserCumulateResult>(wXPostResult);
					if (wXUserCumulateResult == null || wXUserCumulateResult.list == null || wXUserCumulateResult.list.Count <= 0)
					{
						return 0;
					}
					list2 = list2.Concat(wXUserCumulateResult.list).ToList();
				}
				if (num != 1 || !new WXFansStatisticsDao().IsExistData(startDate))
				{
					for (int j = 0; j < num; j++)
					{
						WXFansStatisticsInfo fansItem = new WXFansStatisticsInfo();
						WXFansStatisticsInfo wXFansStatisticsInfo = fansItem;
						dateTime2 = startDate.AddDays((double)j);
						wXFansStatisticsInfo.StatisticalDate = dateTime2.Date;
						fansItem.NewUser = (from u in list
						where u.ref_date == fansItem.StatisticalDate.ToString("yyyy-MM-dd")
						select u).Sum((WXUserSummary u) => u.new_user);
						fansItem.CancelUser = (from u in list
						where u.ref_date == fansItem.StatisticalDate.ToString("yyyy-MM-dd")
						select u).Sum((WXUserSummary u) => u.cancel_user);
						fansItem.NetGrowthUser = fansItem.NewUser - fansItem.CancelUser;
						fansItem.CumulateUser = (from u in list2
						where u.ref_date == fansItem.StatisticalDate.ToString("yyyy-MM-dd")
						select u).Sum((WXUserCumulate u) => u.cumulate_user);
						new WXFansStatisticsDao().Add(fansItem, null);
					}
				}
				return num;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "SynchroWXFansDataErr");
				return 0;
			}
		}

		public static int SynchroWXFansInteractData(DateTime startDate, DateTime endDate, out bool isSettingErr)
		{
			isSettingErr = false;
			int num = (endDate - startDate).Days + 1;
			try
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("startDate", startDate.ToString());
				dictionary.Add("endDate", endDate.ToString());
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				dictionary.Add("AppId", masterSettings.WeixinAppId);
				dictionary.Add("AppSecret", masterSettings.WeixinAppSecret);
				string token = WXFansHelper.GetToken();
				if (string.IsNullOrEmpty(token))
				{
					isSettingErr = true;
					Globals.AppendLog(dictionary, "token为空", "", "", "SynchroWXFansInteractData");
					return 0;
				}
				DateTime dateTime = startDate;
				DateTime dateTime2 = dateTime;
				int num2 = (num % 7 == 0) ? (num / 7).ToInt(0) : ((num / 7).ToInt(0) + 1);
				IList<WXUpstreamMsg> list = new List<WXUpstreamMsg>();
				for (int i = 0; i < num2; i++)
				{
					dateTime = dateTime2.AddDays((double)((i != 0) ? 1 : 0));
					dateTime2 = dateTime.AddDays(6.0);
					if (dateTime2 > endDate)
					{
						dateTime2 = endDate;
					}
					string wXPostResult = WXFansHelper.GetWXPostResult($"https://api.weixin.qq.com/datacube/getupstreammsg?access_token={token}", WXFansHelper.GetPostData(dateTime, dateTime2));
					if (string.IsNullOrEmpty(wXPostResult))
					{
						return 0;
					}
					WXUpstreamMsgResult wXUpstreamMsgResult = JsonHelper.ParseFormJson<WXUpstreamMsgResult>(wXPostResult);
					if (wXUpstreamMsgResult != null && wXUpstreamMsgResult.list != null && wXUpstreamMsgResult.list.Count > 0)
					{
						list = list.Concat(wXUpstreamMsgResult.list).ToList();
					}
				}
				WxMenuClickData wxMenuClickData = new WxMenuClickData();
				bool flag = false;
				if (num == 1)
				{
					flag = new WXFansInteractStatisticsDao().IsExistData(startDate);
					if (!flag)
					{
						wxMenuClickData = WXFansHelper.GetWxMenuClickData(startDate);
						new WXMenuClickRecordDao().DeleteAllMenuClickRecords(startDate);
					}
				}
				if (!flag)
				{
					for (int j = 0; j < num; j++)
					{
						WXFansInteractStatisticsInfo fansItem = new WXFansInteractStatisticsInfo();
						fansItem.StatisticalDate = startDate.AddDays((double)j).Date;
						fansItem.MsgSendNumbers = (from u in list
						where u.ref_date == fansItem.StatisticalDate.ToString("yyyy-MM-dd")
						select u).Sum((WXUpstreamMsg u) => u.msg_user);
						fansItem.MsgSendTimes = (from u in list
						where u.ref_date == fansItem.StatisticalDate.ToString("yyyy-MM-dd")
						select u).Sum((WXUpstreamMsg u) => u.msg_count);
						fansItem.MenuClickNumbers = ((num == 1) ? wxMenuClickData.ClickNumbers : 0);
						fansItem.MenuClickTimes = ((num == 1) ? wxMenuClickData.ClickTimes : 0);
						fansItem.InteractNumbers = fansItem.MenuClickNumbers + fansItem.MsgSendNumbers;
						fansItem.InteractTimes = fansItem.MenuClickTimes + fansItem.MsgSendTimes;
						new WXFansInteractStatisticsDao().Add(fansItem, null);
					}
				}
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "SynchroWXFansInteractDataErr");
				return 0;
			}
			return num;
		}

		public static WxMenuClickData GetWxMenuClickData(DateTime date)
		{
			IList<WXMenuClickInfo> dayList = new WXMenuClickRecordDao().GetDayList(date);
			WxMenuClickData wxMenuClickData = new WxMenuClickData
			{
				ClickNumbers = 0,
				ClickTimes = 0
			};
			if (dayList != null && dayList.Count > 0)
			{
				wxMenuClickData.ClickNumbers = (from m in dayList
				group m by m.WXOpenId).Count();
				wxMenuClickData.ClickTimes = dayList.Count;
			}
			return wxMenuClickData;
		}

		public static WXPostData GetPostData(DateTime startDate, DateTime endData)
		{
			return new WXPostData
			{
				begin_date = startDate.ToString("yyyy-MM-dd"),
				end_date = endData.ToString("yyyy-MM-dd")
			};
		}

		public static string GetWXPostResult(string url, WXPostData data)
		{
			return Globals.GetPostResult(url, JsonConvert.SerializeObject(data));
		}

		public static string GetToken()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				return "";
			}
			try
			{
				return AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, false);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("WeixinAppId", masterSettings.WeixinAppId);
				dictionary.Add("WeixinAppSecret", masterSettings.WeixinAppSecret);
				Globals.WriteExceptionLog(ex, dictionary, "WxFansGetAccessToken");
				return "";
			}
		}

		public static DateTime? GetWxFansLastStatisticalDate()
		{
			return new WXFansStatisticsDao().GetLastStatisticalDate();
		}

		public static DateTime? GetWxFansInteractLastStatisticalDate()
		{
			return new WXFansInteractStatisticsDao().GetLastStatisticalDate();
		}
	}
}
