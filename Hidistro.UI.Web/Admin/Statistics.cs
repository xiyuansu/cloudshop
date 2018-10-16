using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Statistics;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Statistics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
	public class Statistics : IHttpHandler
	{
		private string msg = "";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null)
			{
				this.ResponseError(context, "管理员未登录！");
			}
			else
			{
				string text = context.Request["action"];
				switch (text)
				{
				case "MemberAdded":
					this.GetUserAdd(context);
					break;
				case "GetUserRegisteredSource":
					this.GetUserRegisteredSource(context);
					break;
				case "GetMemberScopeCount":
					this.GetMemberScopeCount(context);
					break;
				case "GetPageview":
					this.GetPageview(context);
					break;
				case "GetPageviewSource":
					this.GetPageviewSource(context);
					break;
				case "GetProductCategoryStatistics":
					this.GetProductCategoryStatistics(context);
					break;
				case "GetProductStatistics":
					this.GetProductStatisticsData(context);
					break;
				case "TranscctionData":
					this.GetTranscctionData(context);
					break;
				case "CustomerTrading":
					this.GetCustomerTrading(context);
					break;
				case "OrderAmountDistribution":
					this.GetOrderAmountDistribution(context);
					break;
				case "OrderSourceDistribution":
					this.GetOrderSourceDistribution(context);
					break;
				case "GetWxFansStatistics":
					this.GetWxFansStatistics(context);
					break;
				case "GetWxFansData":
					this.GetWxFansList(context);
					break;
				case "GetWxFansInteractStatistics":
					this.GetWxFansInteractStatistics(context);
					break;
				case "GetWxFansInteractList":
					this.GetWxFansInteractList(context);
					break;
				case "GetWxFansInteractPersonalData":
					this.GetWxFansInteractPersonalData(context);
					break;
				case "SynchroWXFansData":
					this.SynchroWXFansData(context);
					break;
				case "SynchroWXFansInteractData":
					this.SynchroWXFansInteractData(context);
					break;
				case "GetMemberFansStatistics":
					this.GetMemberFansStatistics(context);
					break;
				case "GetYesterdayWxFansInteractData":
					this.GetYesterdayWxFansInteractData(context);
					break;
				}
			}
		}

		private void GetWxFansInteractPersonalData(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				WXFansStatisticsQuery wXFansStatisticsQuery = new WXFansStatisticsQuery();
				int num = context.Request["LastConsumeTime"].ToInt(0);
				if (!Enum.IsDefined(typeof(EnumConsumeTime), num))
				{
					this.ResponseError(context, "错误的统计时间范围");
				}
				else
				{
					DateTime? nullable = context.Request["CustomConsumeStartTime"].ToDateTime();
					DateTime? nullable2 = context.Request["CustomConsumeEndTime"].ToDateTime();
					if (num == 8 && (!nullable.HasValue || !nullable2.HasValue))
					{
						this.ResponseError(context, "开始时间或者结束时间错误");
					}
					else
					{
						wXFansStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num;
						wXFansStatisticsQuery.CustomConsumeStartTime = nullable;
						wXFansStatisticsQuery.CustomConsumeEndTime = nullable2;
						DateTime startDate;
						DateTime dateTime = default(DateTime);
						int dateScopeDays = this.GetDateScopeDays(wXFansStatisticsQuery.LastConsumeTime, nullable, nullable2, out startDate, out dateTime);
						IList<WXFansInteractStatisticsInfo> list = WXFansHelper.GetWxFansInteractListNoPage(wXFansStatisticsQuery);
						IList<WXFansStatisticsInfo> fansData = WXFansHelper.GetWxFansListNoPage(wXFansStatisticsQuery);
						if (list == null || list.Count == 0 || list.Count < dateScopeDays)
						{
							if (list == null)
							{
								list = new List<WXFansInteractStatisticsInfo>();
							}
							if (fansData == null)
							{
								fansData = new List<WXFansStatisticsInfo>();
							}
							int i;
							 
							for (i = 0; i < dateScopeDays; i++)
							{
								DateTime dateTime2;
								if ((from d in list
								select d.StatisticalDate == startDate.AddDays((double)i).Date).Count() <= 0)
								{
									WXFansInteractStatisticsInfo wXFansInteractStatisticsInfo = new WXFansInteractStatisticsInfo();
									WXFansInteractStatisticsInfo wXFansInteractStatisticsInfo2 = wXFansInteractStatisticsInfo;
									dateTime2 = startDate.AddDays((double)i);
									wXFansInteractStatisticsInfo2.StatisticalDate = dateTime2.Date;
									list.Add(wXFansInteractStatisticsInfo);
								}
								if ((from fd in fansData
								select fd.StatisticalDate == startDate.AddDays((double)i).Date).Count() <= 0)
								{
									WXFansStatisticsInfo wXFansStatisticsInfo = new WXFansStatisticsInfo();
									WXFansStatisticsInfo wXFansStatisticsInfo2 = wXFansStatisticsInfo;
									dateTime2 = startDate.AddDays((double)i);
									wXFansStatisticsInfo2.StatisticalDate = dateTime2.Date;
									fansData.Add(wXFansStatisticsInfo);
								}
							}
						}
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "Success",
								DaysCount = list.Count,
								Data = from c in list
								select new
								{
									InteractNumbers = c.InteractNumbers,
									Rate = this.GetInteractNumberRate(c.InteractNumbers, c.StatisticalDate, fansData),
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd")
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetWxFansInteractStatistics");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		private decimal GetInteractNumberRate(int interactNumbers, DateTime statisticalDate, IList<WXFansStatisticsInfo> fansData)
		{
			if (interactNumbers <= 0 || fansData == null || (from fd in fansData
			where fd.StatisticalDate == statisticalDate
			select fd).Count() == 0)
			{
				return decimal.Zero;
			}
			WXFansStatisticsInfo wXFansStatisticsInfo = (from fd in fansData
			where fd.StatisticalDate == statisticalDate
			select fd).First();
			if (wXFansStatisticsInfo.CumulateUser > 0)
			{
				return (decimal)interactNumbers / ((decimal)wXFansStatisticsInfo.CumulateUser * 1.0m) * 100m;
			}
			return decimal.Zero;
		}

		private void GetMemberFansStatistics(HttpContext context)
		{
			int wxFansCount = WXFansHelper.GetWxFansCount();
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "Success",
					MemberCount = WXFansHelper.GetMemberCount(),
					FansCount = wxFansCount,
					MemberFansCount = ((wxFansCount != 0) ? WXFansHelper.GetMemberFansCount() : 0)
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void SynchroWXFansData(HttpContext context)
		{
			bool flag = false;
			WXFansHelper.ClearFansStatisticsData();
			DateTime startDate = new DateTime(2015, 1, 1);
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			int recordCount = WXFansHelper.SynchroWXFansData(startDate, dateTime.Date, out flag);
			if (!flag)
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				siteSettings.IsInitWXFansData = true;
				SettingsManager.Save(siteSettings);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "Success",
						RecordCount = recordCount
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				this.ResponseError(context, "同步失败,未获取到数据或者微信AppId和AppSecret配置错误。");
			}
		}

		private void SynchroWXFansInteractData(HttpContext context)
		{
			bool flag = false;
			WXFansHelper.ClearFansInteractData();
			DateTime startDate = new DateTime(2015, 1, 1);
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			int recordCount = WXFansHelper.SynchroWXFansInteractData(startDate, dateTime.Date, out flag);
			if (!flag)
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				siteSettings.IsInitWXFansInteractData = true;
				SettingsManager.Save(siteSettings);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "Success",
						RecordCount = recordCount
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				this.ResponseError(context, "同步失败,未获取到数据或者微信AppId和AppSecret配置错误。");
			}
		}

		private void GetWxFansList(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				int num = context.Request["PageIndex"].ToInt(0);
				if (num <= 0)
				{
					num = 1;
				}
				int num2 = context.Request["PageSize"].ToInt(0);
				if (num2 <= 0)
				{
					num2 = 20;
				}
				WXFansStatisticsQuery wXFansStatisticsQuery = new WXFansStatisticsQuery();
				int num3 = context.Request["LastConsumeTime"].ToInt(0);
				if (!Enum.IsDefined(typeof(EnumConsumeTime), num3))
				{
					this.ResponseError(context, "错误的统计时间范围");
				}
				else
				{
					wXFansStatisticsQuery.PageSize = num2;
					wXFansStatisticsQuery.PageIndex = num;
					DateTime? customConsumeStartTime = context.Request["CustomConsumeStartTime"].ToDateTime();
					DateTime? customConsumeEndTime = context.Request["CustomConsumeEndTime"].ToDateTime();
					if (num3 == 8 && (!customConsumeStartTime.HasValue || !customConsumeEndTime.HasValue))
					{
						this.ResponseError(context, "开始时间或者结束时间错误");
					}
					else
					{
						wXFansStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num3;
						wXFansStatisticsQuery.CustomConsumeEndTime = customConsumeEndTime;
						wXFansStatisticsQuery.CustomConsumeStartTime = customConsumeStartTime;
						wXFansStatisticsQuery.SortOrder = SortAction.Desc;
						wXFansStatisticsQuery.SortBy = "StatisticalDate";
						PageModel<WXFansStatisticsInfo> wxFansList = WXFansHelper.GetWxFansList(wXFansStatisticsQuery);
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "Success",
								RecordCount = wxFansList.Total,
								Data = from c in wxFansList.Models
								select new
								{
									CancelUser = c.CancelUser,
									CumulateUser = c.CumulateUser,
									NewUser = c.NewUser,
									NetGrowthUser = c.NetGrowthUser,
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd")
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetWxFansData");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		private int GetDateScopeDays(EnumConsumeTime timescop, DateTime? customStartDate, DateTime? customEndDate, out DateTime startTime, out DateTime endTime)
		{
			DateTime dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			startTime = dateTime.Date;
			dateTime = DateTime.Now;
			dateTime = dateTime.AddDays(-1.0);
			endTime = dateTime.Date;
			switch (timescop)
			{
			case EnumConsumeTime.yesterday:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				startTime = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				endTime = dateTime.Date;
				break;
			case EnumConsumeTime.inOneWeek:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-7.0);
				startTime = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				endTime = dateTime.Date;
				break;
			case EnumConsumeTime.inTwoWeek:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-14.0);
				startTime = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				endTime = dateTime.Date;
				break;
			case EnumConsumeTime.inOneMonth:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-1);
				startTime = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				endTime = dateTime.Date;
				break;
			case EnumConsumeTime.preSixMonth:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-6);
				startTime = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				endTime = dateTime.Date;
				break;
			case EnumConsumeTime.preThreeMonth:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-3);
				startTime = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				endTime = dateTime.Date;
				break;
			case EnumConsumeTime.preTwoMonth:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddMonths(-2);
				startTime = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-1.0);
				endTime = dateTime.Date;
				break;
			case EnumConsumeTime.custom:
			{
				DateTime dateTime2;
				if (!customStartDate.HasValue)
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-1.0);
					dateTime2 = dateTime.Date;
				}
				else
				{
					dateTime2 = customStartDate.Value;
				}
				startTime = dateTime2;
				DateTime dateTime3;
				if (!customEndDate.HasValue)
				{
					dateTime = DateTime.Now;
					dateTime = dateTime.AddDays(-1.0);
					dateTime3 = dateTime.Date;
				}
				else
				{
					dateTime3 = customEndDate.Value;
				}
				endTime = dateTime3;
				break;
			}
			}
			return (endTime - startTime).Days + 1;
		}

		private void GetWxFansStatistics(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				WXFansStatisticsQuery wXFansStatisticsQuery = new WXFansStatisticsQuery();
				int num = context.Request["LastConsumeTime"].ToInt(0);
				if (!Enum.IsDefined(typeof(EnumConsumeTime), num))
				{
					this.ResponseError(context, "错误的统计时间范围");
				}
				else
				{
					DateTime? nullable = context.Request["CustomConsumeStartTime"].ToDateTime();
					DateTime? nullable2 = context.Request["CustomConsumeEndTime"].ToDateTime();
					if (num == 8 && (!nullable.HasValue || !nullable2.HasValue))
					{
						this.ResponseError(context, "开始时间或者结束时间错误");
					}
					else
					{
						wXFansStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num;
						wXFansStatisticsQuery.CustomConsumeStartTime = nullable;
						wXFansStatisticsQuery.CustomConsumeEndTime = nullable2;
						wXFansStatisticsQuery.SortOrder = SortAction.Desc;
						DateTime startDate;
						DateTime dateTime = default(DateTime);
						int dateScopeDays = this.GetDateScopeDays(wXFansStatisticsQuery.LastConsumeTime, nullable, nullable2, out startDate, out dateTime);
						IList<WXFansStatisticsInfo> list = WXFansHelper.GetWxFansListNoPage(wXFansStatisticsQuery);
						if (list == null || list.Count == 0 || list.Count < dateScopeDays)
						{
							if (list == null)
							{
								list = new List<WXFansStatisticsInfo>();
							}
							int i;
							for (i = 0; i < dateScopeDays; i++)
							{
								if ((from d in list
								where d.StatisticalDate == startDate.AddDays((double)i).Date
								select d).Count() <= 0)
								{
									WXFansStatisticsInfo wXFansStatisticsInfo = new WXFansStatisticsInfo();
									wXFansStatisticsInfo.StatisticalDate = startDate.AddDays((double)i).Date;
									list.Add(wXFansStatisticsInfo);
								}
							}
						}
						list = (from d in list
						orderby d.StatisticalDate descending
						select d).ToList();
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "Success",
								DaysCount = list.Count,
								NewUser = list.Sum((WXFansStatisticsInfo d) => d.NewUser),
								CancelUser = list.Sum((WXFansStatisticsInfo d) => d.CancelUser),
								NetGrowthUser = list.Sum((WXFansStatisticsInfo d) => d.NetGrowthUser),
								CumulateUser = list.FirstOrDefault().CumulateUser,
								NewUserData = from c in list
								select new
								{
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd"),
									NewUser = c.NewUser
								},
								CancelUserData = from c in list
								select new
								{
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd"),
									CancelUser = c.CancelUser
								},
								NetGrowthUserData = from c in list
								select new
								{
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd"),
									NetGrowthUser = c.NetGrowthUser
								},
								CumulateUserData = from c in list
								select new
								{
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd"),
									CumulateUser = c.CumulateUser
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetProductStatisticsData");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		private void GetYesterdayWxFansInteractData(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				WXFansStatisticsQuery wXFansStatisticsQuery = new WXFansStatisticsQuery();
				wXFansStatisticsQuery.LastConsumeTime = EnumConsumeTime.yesterday;
				IList<WXFansInteractStatisticsInfo> wxFansInteractListNoPage = WXFansHelper.GetWxFansInteractListNoPage(wXFansStatisticsQuery);
				if (wxFansInteractListNoPage == null || wxFansInteractListNoPage.Count == 0)
				{
					this.ResponseError(context, "未获取到数据");
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "Success",
							InteractNumbers = wxFansInteractListNoPage[0].InteractNumbers,
							InteractTimes = wxFansInteractListNoPage[0].InteractTimes,
							MenuClickTimes = wxFansInteractListNoPage[0].MenuClickTimes,
							MsgSendTimes = wxFansInteractListNoPage[0].MsgSendTimes
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetYesterdayWxFansInteractData");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		private void GetWxFansInteractStatistics(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				WXFansStatisticsQuery wXFansStatisticsQuery = new WXFansStatisticsQuery();
				int num = context.Request["LastConsumeTime"].ToInt(0);
				if (!Enum.IsDefined(typeof(EnumConsumeTime), num))
				{
					this.ResponseError(context, "错误的统计时间范围");
				}
				else
				{
					DateTime? nullable = context.Request["CustomConsumeStartTime"].ToDateTime();
					DateTime? nullable2 = context.Request["CustomConsumeEndTime"].ToDateTime();
					if (num == 8 && (!nullable.HasValue || !nullable2.HasValue))
					{
						this.ResponseError(context, "开始时间或者结束时间错误");
					}
					else
					{
						wXFansStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num;
						wXFansStatisticsQuery.CustomConsumeStartTime = nullable;
						wXFansStatisticsQuery.CustomConsumeEndTime = nullable2;
						DateTime startDate;
						DateTime dateTime = default(DateTime);
						int dateScopeDays = this.GetDateScopeDays(wXFansStatisticsQuery.LastConsumeTime, nullable, nullable2, out startDate, out dateTime);
						IList<WXFansInteractStatisticsInfo> list = WXFansHelper.GetWxFansInteractListNoPage(wXFansStatisticsQuery);
						if (list == null || list.Count == 0 || list.Count < dateScopeDays)
						{
							if (list == null)
							{
								list = new List<WXFansInteractStatisticsInfo>();
							}
							int i;
							for (i = 0; i < dateScopeDays; i++)
							{
								if ((from d in list
								where d.StatisticalDate == startDate.AddDays((double)i).Date
								select d).Count() <= 0)
								{
									WXFansInteractStatisticsInfo wXFansInteractStatisticsInfo = new WXFansInteractStatisticsInfo();
									wXFansInteractStatisticsInfo.StatisticalDate = startDate.AddDays((double)i).Date;
									list.Add(wXFansInteractStatisticsInfo);
								}
							}
						}
						list = (from d in list
						orderby d.StatisticalDate descending
						select d).ToList();
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "Success",
								Data = from c in list
								select new
								{
									InteractNumbers = c.InteractNumbers,
									InteractTimes = c.InteractTimes,
									MenuClickTimes = c.MenuClickTimes,
									MsgSendTimes = c.MsgSendTimes,
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd")
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetWxFansInteractStatistics");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		private void GetWxFansInteractList(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				int num = context.Request["PageIndex"].ToInt(0);
				if (num <= 0)
				{
					num = 1;
				}
				int num2 = context.Request["PageSize"].ToInt(0);
				if (num2 <= 0)
				{
					num2 = 20;
				}
				WXFansStatisticsQuery wXFansStatisticsQuery = new WXFansStatisticsQuery();
				int num3 = context.Request["LastConsumeTime"].ToInt(0);
				if (!Enum.IsDefined(typeof(EnumConsumeTime), num3))
				{
					this.ResponseError(context, "错误的统计时间范围");
				}
				else
				{
					wXFansStatisticsQuery.PageSize = num2;
					wXFansStatisticsQuery.PageIndex = num;
					DateTime? customConsumeStartTime = context.Request["CustomConsumeStartTime"].ToDateTime();
					DateTime? customConsumeEndTime = context.Request["CustomConsumeEndTime"].ToDateTime();
					wXFansStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num3;
					wXFansStatisticsQuery.CustomConsumeEndTime = customConsumeEndTime;
					wXFansStatisticsQuery.CustomConsumeStartTime = customConsumeStartTime;
					wXFansStatisticsQuery.SortOrder = SortAction.Desc;
					wXFansStatisticsQuery.SortBy = "StatisticalDate";
					if (num3 == 8 && (!customConsumeStartTime.HasValue || !customConsumeEndTime.HasValue))
					{
						this.ResponseError(context, "开始时间或者结束时间错误");
					}
					else
					{
						PageModel<WXFansInteractStatisticsInfo> wxFansInteractList = WXFansHelper.GetWxFansInteractList(wXFansStatisticsQuery);
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "Success",
								RecordCount = wxFansInteractList.Total,
								Data = from c in wxFansInteractList.Models
								select new
								{
									InteractNumbers = c.InteractNumbers,
									InteractTimes = c.InteractTimes,
									MenuClickTimes = c.MenuClickTimes,
									MsgSendTimes = c.MsgSendTimes,
									StatisticalDate = c.StatisticalDate.ToString("yyyy-MM-dd")
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetWxFansInteractList");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		private void GetTranscctionData(HttpContext context)
		{
			int num = context.Request["LastConsumeTime"].ToInt(0);
			DateTime dateTime3;
			DateTime dateTime2;
			EnumConsumeTime timeType;
			DateTime dateTime;
			switch (num)
			{
			case 8:
				dateTime3 = Convert.ToDateTime(context.Request["StartDate"]);
				dateTime2 = Convert.ToDateTime(context.Request["EndDate"]);
				if (dateTime2 < dateTime3)
				{
					this.ResponseError(context, "错误的统计时间范围");
					return;
				}
				timeType = EnumConsumeTime.custom;
				break;
			case 4:
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime3 = dateTime2.AddDays(-30.0);
				timeType = EnumConsumeTime.inOneMonth;
				break;
			case 2:
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime3 = dateTime2.AddDays(-7.0);
				timeType = EnumConsumeTime.inOneWeek;
				break;
			default:
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				dateTime3 = dateTime.AddDays(-1.0);
				timeType = EnumConsumeTime.yesterday;
				break;
			}
			IList<OrderStatisticModel> list = TransactionAnalysisHelper.GetOrderDailyStatisticsList(timeType, dateTime3, dateTime2);
			int orderUserNum = 0;
			int orderNum = 0;
			int orderProductQuantity = 0;
			decimal num2 = default(decimal);
			int paymentUserNum = 0;
			int num3 = 0;
			int paymentProductNum = 0;
			decimal num4 = default(decimal);
			decimal num5 = default(decimal);
			decimal num6 = default(decimal);
			int accessStatisticsModelList = TransactionAnalysisHelper.GetAccessStatisticsModelList(timeType, dateTime3, dateTime2);
			if (list.Count > 0)
			{
				orderUserNum = list.Sum((OrderStatisticModel c) => c.OrderUserNum);
				orderNum = list.Sum((OrderStatisticModel c) => c.OrderNum);
				orderProductQuantity = list.Sum((OrderStatisticModel c) => c.OrderProductQuantity);
				num2 = list.Sum((OrderStatisticModel c) => c.OrderAmount);
				paymentUserNum = list.Sum((OrderStatisticModel c) => c.PaymentUserNum);
				num3 = list.Sum((OrderStatisticModel c) => c.PaymentOrderNum);
				num4 = list.Sum((OrderStatisticModel c) => c.PaymentAmount);
				paymentProductNum = list.Sum((OrderStatisticModel c) => c.PaymentProductNum);
				num5 = ((num3 > 0) ? (num4 / (decimal)num3) : decimal.Zero);
				num6 = list.Sum((OrderStatisticModel c) => c.RefundAmount);
			}
			bool flag = true;
			if (dateTime3 < dateTime2)
			{
				DateTime dtDay = dateTime3;
				while ((dtDay <= dateTime2 && num == 8) || (dtDay < dateTime2 && num != 8))
				{
					if (list.FirstOrDefault((OrderStatisticModel c) => c.StatisticalDate == dtDay) == null)
					{
						OrderStatisticModel orderStatisticModel = new OrderStatisticModel();
						orderStatisticModel.StatisticalDate = dtDay;
						orderStatisticModel.PV = 0;
						orderStatisticModel.UV = 0;
						orderStatisticModel.OrderAmount = decimal.Zero;
						orderStatisticModel.OrderNum = 0;
						orderStatisticModel.OrderProductQuantity = 0;
						orderStatisticModel.OrderUserNum = 0;
						orderStatisticModel.PaymentAmount = decimal.Zero;
						orderStatisticModel.PaymentOrderNum = 0;
						orderStatisticModel.PaymentProductNum = 0;
						orderStatisticModel.PaymentUserNum = 0;
						orderStatisticModel.RefundAmount = decimal.Zero;
						list.Add(orderStatisticModel);
					}
					dtDay = dtDay.AddDays(1.0);
				}
				list = (from c in list
				orderby c.StatisticalDate
				select c).ToList();
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "Success",
					AllPV = accessStatisticsModelList,
					OrderUserNum = orderUserNum,
					OrderNum = orderNum,
					OrderProductQuantity = orderProductQuantity,
					OrderAmount = num2.F2ToString("f2"),
					PaymentUserNum = paymentUserNum,
					PaymentOrderNum = num3,
					PaymentProductNum = paymentProductNum,
					PaymentAmount = num4.F2ToString("f2"),
					GuestUnitPrice = num5.F2ToString("f2"),
					RefundAmount = num6.F2ToString("f2"),
					RecordCount = list.Count,
					List = from p in list
					select new
					{
						PaymentUserNum = p.PaymentUserNum,
						PaymentProductNum = p.PaymentProductNum,
						PaymentAmount = p.PaymentAmount,
						ConversionRate = p.ConversionRate,
						PaymentRate = p.PaymentRate,
						ClinchaDealRate = p.ClinchaDealRate,
						RefundAmount = p.RefundAmount,
						StatisticalDate = p.StatisticalDate.ToString("yyyy-MM-dd")
					}
				}
			});
			context.Response.ContentType = "application/json";
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetCustomerTrading(HttpContext context)
		{
			int num = context.Request["Year"].ToInt(0);
			int num2 = context.Request["Month"].ToInt(0);
			if (num >= 2000 && num <= DateTime.Now.Year && num2 > 0 && num2 <= 12)
			{
				DateTime dateTime = new DateTime(num, num2, 1);
				DateTime dtEnd = dateTime.AddMonths(1);
				CustomerTradingModel customerTrading = TransactionAnalysisHelper.GetCustomerTrading(dateTime, dtEnd);
				DateTime dtStart = dateTime.AddMonths(-1);
				CustomerTradingModel customerTrading2 = TransactionAnalysisHelper.GetCustomerTrading(dtStart, dateTime);
				string newAmountCompareLastMonth = "/";
				if (customerTrading2.NewCustomerAmount > decimal.Zero)
				{
					newAmountCompareLastMonth = ((customerTrading.NewCustomerAmount - customerTrading2.NewCustomerAmount) * 100m / customerTrading2.NewCustomerAmount).F2ToString("f2");
				}
				string oldAmountCompareLastMonth = "/";
				if (customerTrading2.OldCustomerAmount > decimal.Zero)
				{
					oldAmountCompareLastMonth = ((customerTrading.OldCustomerAmount - customerTrading2.OldCustomerAmount) * 100m / customerTrading2.OldCustomerAmount).F2ToString("f2");
				}
				string newUserNumCompareLastMonth = "/";
				if (customerTrading2.NewCustomerPayNum > 0)
				{
					newUserNumCompareLastMonth = ((customerTrading.NewCustomerPayNum - customerTrading2.NewCustomerPayNum) * 100 / customerTrading2.NewCustomerPayNum).F2ToString("f2");
				}
				string oldUserNumCompareLastMonth = "/";
				if (customerTrading2.OldCustomerNum > 0)
				{
					oldUserNumCompareLastMonth = ((customerTrading.OldCustomerNum - customerTrading2.OldCustomerNum) * 100 / customerTrading2.OldCustomerNum).ToString();
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						NewCustomerNum = customerTrading.NewCustomerNum,
						OldCustomerNum = customerTrading.OldCustomerNum,
						OldCustomerPayNum = customerTrading.OldCustomerPayNum,
						NewCustomerPayNum = customerTrading.NewCustomerPayNum,
						NewCustomerAmount = customerTrading.NewCustomerAmount.F2ToString("f2"),
						OldCustomerAmount = customerTrading.OldCustomerAmount.F2ToString("f2"),
						NewAmountCompareLastMonth = newAmountCompareLastMonth,
						OldAmountCompareLastMonth = oldAmountCompareLastMonth,
						NewUserNumCompareLastMonth = newUserNumCompareLastMonth,
						OldUserNumCompareLastMonth = oldUserNumCompareLastMonth
					}
				});
				context.Response.ContentType = "application/json";
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void GetOrderAmountDistribution(HttpContext context)
		{
			DateTime dateTime3;
			DateTime dateTime;
			DateTime dateTime2;
			switch (context.Request["LastConsumeTime"].ToInt(0))
			{
			case 8:
				dateTime3 = Convert.ToDateTime(context.Request["StartDate"]);
				dateTime = Convert.ToDateTime(context.Request["EndDate"]);
				dateTime = dateTime.Date;
				dateTime2 = dateTime.AddDays(1.0);
				if (!(dateTime2 < dateTime3))
				{
					break;
				}
				this.ResponseError(context, "错误的统计时间范围");
				return;
			case 4:
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime3 = dateTime2.AddDays(-30.0);
				break;
			default:
				dateTime = DateTime.Now;
				dateTime2 = dateTime.Date;
				dateTime3 = dateTime2.AddDays(-7.0);
				break;
			}
			OrderAmountDistributionModel orderAmountDistribution = TransactionAnalysisHelper.GetOrderAmountDistribution(dateTime3, dateTime2);
			string s = JsonConvert.SerializeObject(new
			{
				Result = orderAmountDistribution
			});
			context.Response.ContentType = "application/json";
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetOrderSourceDistribution(HttpContext context)
		{
			int num = context.Request["Year"].ToInt(0);
			int num2 = context.Request["Month"].ToInt(0);
			if (num >= 2000 && num <= DateTime.Now.Year && num2 > 0 && num2 <= 12)
			{
				DateTime dateTime = new DateTime(num, num2, 1);
				DateTime endDate = dateTime.AddMonths(1);
				OrderSourceDistributionModel orderSourceDistribution = TransactionAnalysisHelper.GetOrderSourceDistribution(dateTime, endDate);
				DateTime startDate = dateTime.AddMonths(-1);
				OrderSourceDistributionModel orderSourceDistribution2 = TransactionAnalysisHelper.GetOrderSourceDistribution(startDate, dateTime);
				string appOrderCountCompareLastMonth = "/";
				if (orderSourceDistribution2.AppCount > 0)
				{
					appOrderCountCompareLastMonth = ((orderSourceDistribution.AppCount - orderSourceDistribution2.AppCount) * 100 / orderSourceDistribution2.AppCount).F2ToString("f2");
				}
				string appOrderAmountCompareLastMonth = "/";
				if (orderSourceDistribution2.AppAmount > decimal.Zero)
				{
					appOrderAmountCompareLastMonth = ((orderSourceDistribution.AppAmount - orderSourceDistribution2.AppAmount) * 100m / orderSourceDistribution2.AppAmount).F2ToString("f2");
				}
				string weiXinOrderCountCompareLastMonth = "/";
				if (orderSourceDistribution2.WeiXinCount > 0)
				{
					weiXinOrderCountCompareLastMonth = ((orderSourceDistribution.WeiXinCount - orderSourceDistribution2.WeiXinCount) * 100 / orderSourceDistribution2.WeiXinCount).F2ToString("f2");
				}
				string weiXinOrderAmountCompareLastMonth = "/";
				if (orderSourceDistribution2.WeiXinAmount > decimal.Zero)
				{
					weiXinOrderAmountCompareLastMonth = ((orderSourceDistribution.WeiXinAmount - orderSourceDistribution2.WeiXinAmount) * 100m / orderSourceDistribution2.WeiXinAmount).F2ToString("f2");
				}
				string pCOrderCountCompareLastMonth = "/";
				if (orderSourceDistribution2.PCCount > 0)
				{
					pCOrderCountCompareLastMonth = ((orderSourceDistribution.PCCount - orderSourceDistribution2.PCCount) * 100 / orderSourceDistribution2.PCCount).F2ToString("f2");
				}
				string pCOrderAmountCompareLastMonth = "/";
				if (orderSourceDistribution2.PCAmount > decimal.Zero)
				{
					pCOrderAmountCompareLastMonth = ((orderSourceDistribution.PCAmount - orderSourceDistribution2.PCAmount) * 100m / orderSourceDistribution2.PCAmount).F2ToString("f2");
				}
				string appletOrderCountCompareLastMonth = "/";
				if (orderSourceDistribution2.AppletCount > 0)
				{
					appletOrderCountCompareLastMonth = ((orderSourceDistribution.AppletCount - orderSourceDistribution2.AppletCount) * 100 / orderSourceDistribution2.AppletCount).F2ToString("f2");
				}
				string appletOrderAmountCompareLastMonth = "/";
				if (orderSourceDistribution2.AppletAmount > decimal.Zero)
				{
					appletOrderAmountCompareLastMonth = ((orderSourceDistribution.AppletAmount - orderSourceDistribution2.AppletAmount) * 100m / orderSourceDistribution2.AppletAmount).F2ToString("f2");
				}
				string otherOrderCountCompareLastMonth = "/";
				if (orderSourceDistribution2.OtherCount > 0)
				{
					otherOrderCountCompareLastMonth = ((orderSourceDistribution.OtherCount - orderSourceDistribution2.OtherCount) * 100 / orderSourceDistribution2.OtherCount).F2ToString("f2");
				}
				string otherOrderAmountCompareLastMonth = "/";
				if (orderSourceDistribution2.OtherAmount > decimal.Zero)
				{
					otherOrderAmountCompareLastMonth = ((orderSourceDistribution.OtherAmount - orderSourceDistribution2.OtherAmount) * 100m / orderSourceDistribution2.OtherAmount).F2ToString("f2");
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						AppCount = orderSourceDistribution.AppCount,
						WeiXinCount = orderSourceDistribution.WeiXinCount,
						PCCount = orderSourceDistribution.PCCount,
						AppletCount = orderSourceDistribution.AppletCount,
						OtherCount = orderSourceDistribution.OtherCount,
						AppAmount = orderSourceDistribution.AppAmount.F2ToString("f2"),
						WeiXinAmount = orderSourceDistribution.WeiXinAmount.F2ToString("f2"),
						PCAmount = orderSourceDistribution.PCAmount.F2ToString("f2"),
						AppletAmount = orderSourceDistribution.AppletAmount.F2ToString("f2"),
						OtherAmount = orderSourceDistribution.OtherAmount.F2ToString("f2"),
						AppOrderCountCompareLastMonth = appOrderCountCompareLastMonth,
						AppOrderAmountCompareLastMonth = appOrderAmountCompareLastMonth,
						WeiXinOrderCountCompareLastMonth = weiXinOrderCountCompareLastMonth,
						WeiXinOrderAmountCompareLastMonth = weiXinOrderAmountCompareLastMonth,
						PCOrderCountCompareLastMonth = pCOrderCountCompareLastMonth,
						PCOrderAmountCompareLastMonth = pCOrderAmountCompareLastMonth,
						AppletOrderCountCompareLastMonth = appletOrderCountCompareLastMonth,
						AppletOrderAmountCompareLastMonth = appletOrderAmountCompareLastMonth,
						OtherOrderCountCompareLastMonth = otherOrderCountCompareLastMonth,
						OtherOrderAmountCompareLastMonth = otherOrderAmountCompareLastMonth
					}
				});
				context.Response.ContentType = "application/json";
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void GetMemberScopeCount(HttpContext context)
		{
			try
			{
				IDictionary<string, int> memberScopeCount = MemberHelper.GetMemberScopeCount();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						List = memberScopeCount
					}
				}));
				context.Response.Write(stringBuilder);
			}
			catch (Exception ex)
			{
				this.ResponseError(context, ex.Message);
			}
		}

		private void GetUserAdd(HttpContext context)
		{
			try
			{
				int num = context.Request["year"].ToInt(0);
				int num2 = context.Request["month"].ToInt(0);
				IList<UserStatistics> userAdd = MemberHelper.GetUserAdd(num, num2);
				StringBuilder stringBuilder = new StringBuilder();
				DateTime dateTime = default(DateTime);
				int num3 = 0;
				if (num > 0 & num2 <= 0)
				{
					num3 = ((!DateTime.IsLeapYear(num)) ? 365 : 366);
					dateTime = new DateTime(num, 1, 1);
				}
				else if (num > 0 && num2 > 0)
				{
					dateTime = new DateTime(num, num2, 1);
					num3 = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
				}
				DateTime newTime = dateTime;
				for (int i = 1; i <= num3; i++)
				{
					if (newTime < DateTime.Today)
					{
						if (userAdd.FirstOrDefault((UserStatistics c) => c.Time == newTime) == null)
						{
							UserStatistics userStatistics = new UserStatistics();
							userStatistics.Time = newTime;
							userAdd.Add(userStatistics);
						}
						newTime = newTime.AddDays(1.0);
					}
				}
				userAdd = (from c in userAdd
				orderby c.Time
				select c).ToList();
				IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter
				{
					DateTimeFormat = "yyyy-MM-dd"
				};
				stringBuilder.Append(JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						List = userAdd
					}
				}, Formatting.Indented, isoDateTimeConverter));
				context.Response.Write(stringBuilder.ToString());
			}
			catch (Exception ex)
			{
				this.ResponseError(context, ex.Message);
			}
		}

		private void GetUserRegisteredSource(HttpContext context)
		{
			try
			{
				int year = context.Request["year"].ToInt(0);
				int month = context.Request["month"].ToInt(0);
				IList<RegisteredSourceStatistics> userRegisteredSource = MemberHelper.GetUserRegisteredSource(year, month);
				int[] array = new int[7]
				{
					1,
					2,
					3,
					4,
					5,
					6,
					7
				};
				IList<RegisteredSourceStatistics> list = new List<RegisteredSourceStatistics>();
				int[] array2 = array;
				foreach (int num in array2)
				{
					RegisteredSourceStatistics registeredSourceStatistics = new RegisteredSourceStatistics();
					foreach (RegisteredSourceStatistics item in userRegisteredSource)
					{
						if (num == item.RegisteredSource)
						{
							registeredSourceStatistics.CreateDate = item.CreateDate;
							registeredSourceStatistics.RegisteredSource = item.RegisteredSource;
							registeredSourceStatistics.Percentage = item.Percentage;
							break;
						}
						registeredSourceStatistics.CreateDate = item.CreateDate;
						registeredSourceStatistics.RegisteredSource = num;
					}
					list.Add(registeredSourceStatistics);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						List = list
					}
				}));
				context.Response.Write(stringBuilder);
			}
			catch (Exception ex)
			{
				this.ResponseError(context, ex.Message);
			}
		}

		private void GetPageview(HttpContext context)
		{
			try
			{
				TrafficQuery trafficQuery = new TrafficQuery();
				string text = context.Request["LastConsumeTime"].ToNullString();
				int pageType = context.Request["PageType"].ToInt(0);
				DateTime dateTime = Convert.ToDateTime(context.Request["CustomConsumeStartTime"]);
				DateTime dateTime2 = Convert.ToDateTime(context.Request["CustomConsumeEndTime"]);
				EnumConsumeTime lastConsumeTime = default(EnumConsumeTime);
				Enum.TryParse<EnumConsumeTime>(text, false, out lastConsumeTime);
				if (text.ToLower() == 8.ToString() && dateTime > dateTime2)
				{
					this.ResponseError(context, "开始时间或者结束时间错误");
				}
				else
				{
					trafficQuery.LastConsumeTime = lastConsumeTime;
					trafficQuery.PageType = pageType;
					trafficQuery.CustomConsumeStartTime = dateTime;
					trafficQuery.CustomConsumeEndTime = dateTime2;
					IList<Traffics> pageview = TrafficHelper.GetPageview(trafficQuery);
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = true,
							List = pageview
						}
					}));
					context.Response.Write(stringBuilder);
				}
			}
			catch (Exception ex)
			{
				this.ResponseError(context, ex.Message);
			}
		}

		private void GetPageviewSource(HttpContext context)
		{
			try
			{
				TrafficQuery trafficQuery = new TrafficQuery();
				string text = context.Request["LastConsumeTime"].ToNullString();
				string text2 = context.Request["Type"].ToNullString();
				DateTime dateTime = Convert.ToDateTime(context.Request["CustomConsumeStartTime"]);
				DateTime dateTime2 = Convert.ToDateTime(context.Request["CustomConsumeEndTime"]);
				EnumConsumeTime lastConsumeTime = default(EnumConsumeTime);
				Enum.TryParse<EnumConsumeTime>(text, false, out lastConsumeTime);
				trafficQuery.LastConsumeTime = lastConsumeTime;
				trafficQuery.Type = text2.Trim().ToLower();
				if (text.ToLower() == 8.ToString() && dateTime > dateTime2)
				{
					this.ResponseError(context, "开始时间或者结束时间错误");
				}
				else
				{
					trafficQuery.CustomConsumeStartTime = dateTime;
					trafficQuery.CustomConsumeEndTime = dateTime2;
					IList<TrafficSourceScope> pageviewSource = TrafficHelper.GetPageviewSource(trafficQuery);
					IList<TrafficSourceScope> list = new List<TrafficSourceScope>();
					int[] array = new int[4]
					{
						1,
						2,
						3,
						99
					};
					int[] array2 = array;
					foreach (int num in array2)
					{
						TrafficSourceScope trafficSourceScope = new TrafficSourceScope();
						foreach (TrafficSourceScope item in pageviewSource)
						{
							if (num == item.SourceId)
							{
								trafficSourceScope.SourceId = item.SourceId;
								trafficSourceScope.Scope = item.Scope;
								break;
							}
							trafficSourceScope.SourceId = num;
							trafficSourceScope.Scope = decimal.Zero;
						}
						list.Add(trafficSourceScope);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = true,
							List = list
						}
					}));
					context.Response.Write(stringBuilder);
				}
			}
			catch (Exception ex)
			{
				this.ResponseError(context, ex.Message);
			}
		}

		private void GetProductStatisticsData(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				int num = context.Request["PageIndex"].ToInt(0);
				if (num <= 0)
				{
					num = 1;
				}
				int num2 = context.Request["PageSize"].ToInt(0);
				if (num2 <= 0)
				{
					num2 = 20;
				}
				ProductStatisticsQuery productStatisticsQuery = new ProductStatisticsQuery();
				int num3 = context.Request["LastConsumeTime"].ToInt(0);
				if (!Enum.IsDefined(typeof(EnumConsumeTime), num3))
				{
					this.ResponseError(context, "错误的统计时间范围");
				}
				else
				{
					productStatisticsQuery.PageSize = num2;
					productStatisticsQuery.PageIndex = num;
					DateTime? customConsumeStartTime = context.Request["CustomConsumeStartTime"].ToDateTime();
					DateTime? customConsumeEndTime = context.Request["CustomConsumeEndTime"].ToDateTime();
					if (num3 == 8 && (!customConsumeStartTime.HasValue || !customConsumeEndTime.HasValue))
					{
						this.ResponseError(context, "开始时间或者结束时间错误");
					}
					else
					{
						productStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num3;
						productStatisticsQuery.CustomConsumeEndTime = customConsumeEndTime;
						productStatisticsQuery.CustomConsumeStartTime = customConsumeStartTime;
						string a = context.Request["sortorder"].ToNullString().ToLower();
						string text = context.Request["sortby"].ToNullString().ToLower();
						if (text != "pv" && text != "uv" && text != "paymentnum" && text != "salequantity" && text != "saleamount" && text != "productconversionrate")
						{
							text = "pv";
						}
						productStatisticsQuery.SortBy = text;
						productStatisticsQuery.SortOrder = SortAction.Desc;
						if (a == "asc")
						{
							productStatisticsQuery.SortOrder = SortAction.Asc;
						}
						productStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num3;
						productStatisticsQuery.CustomConsumeStartTime = customConsumeStartTime;
						productStatisticsQuery.CustomConsumeEndTime = customConsumeEndTime;
						PageModel<ProductStatisticsInfo> productStatisticsData = ProductStatisticsHelper.GetProductStatisticsData(productStatisticsQuery);
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "Success",
								RecordCount = productStatisticsData.Total,
								List = from p in productStatisticsData.Models
								select new
								{
									p.ProductName,
									p.PV,
									p.UV,
									p.ProductConversionRate,
									p.PaymentNum,
									p.SaleQuantity,
									p.SaleAmount
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetProductStatisticsData");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		private void GetProductCategoryStatistics(HttpContext context)
		{
			NameValueCollection param = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			try
			{
				ProductStatisticsQuery productStatisticsQuery = new ProductStatisticsQuery();
				int num = context.Request["LastConsumeTime"].ToInt(0);
				if (!Enum.IsDefined(typeof(EnumConsumeTime), num))
				{
					this.ResponseError(context, "错误的统计时间范围");
				}
				else
				{
					DateTime? customConsumeStartTime = context.Request["CustomConsumeStartTime"].ToDateTime();
					DateTime? customConsumeEndTime = context.Request["CustomConsumeEndTime"].ToDateTime();
					if (num == 8 && (!customConsumeStartTime.HasValue || !customConsumeEndTime.HasValue))
					{
						this.ResponseError(context, "开始时间或者结束时间错误");
					}
					else
					{
						productStatisticsQuery.LastConsumeTime = (EnumConsumeTime)num;
						productStatisticsQuery.CustomConsumeEndTime = customConsumeEndTime;
						productStatisticsQuery.CustomConsumeStartTime = customConsumeStartTime;
						IList<ProductCategoryStatisticsInfo> productCategoryStatisticsData = ProductStatisticsHelper.GetProductCategoryStatisticsData(productStatisticsQuery);
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "Success",
								QuantityData = from c in productCategoryStatisticsData
								select new
								{
									c.CategoryName,
									c.CategoryId,
									c.SaleCounts
								},
								AmountData = from c in productCategoryStatisticsData
								select new
								{
									c.CategoryName,
									c.CategoryId,
									c.SaleAmounts
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is ThreadAbortException))
				{
					Globals.WriteExceptionLog_Page(ex, param, "GetProductCategoryStatistics");
					this.ResponseError(context, ex.Message);
				}
			}
		}

		public void ResponseError(HttpContext context, string msg)
		{
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "Failure",
					Message = msg
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}
	}
}
