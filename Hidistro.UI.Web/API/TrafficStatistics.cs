using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class TrafficStatistics : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			this.WriteTrafficRecords(context);
		}

		public void WriteTrafficRecords(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			string text;
			DateTime date;
			if (context.Request.HttpMethod.ToLower() == "post")
			{
				num = context.Request["PageType"].ToInt(0);
				num2 = context.Request["SourceId"].ToInt(0);
				num5 = context.Request["StoreId"].ToInt(0);
				if (num >= 0 && num2 > 0 && num2 <= 99 && (num2 <= 3 || num2 >= 99) && num <= 99 && (num <= 3 || num >= 99))
				{
					text = "";
					Guid guid;
					DateTime now;
					if (num2 == 3)
					{
						string text2 = context.Request["SessionId"];
						if (!string.IsNullOrWhiteSpace(text2))
						{
							MemberInfo memberInfo = MemberProcessor.FindMemberBySessionId(text2);
							if (memberInfo == null)
							{
								guid = Guid.NewGuid();
								text = guid.ToString().Replace("-", "");
								HttpCookie httpCookie = new HttpCookie("t_sessionId");
								httpCookie.HttpOnly = true;
								httpCookie.Value = text;
								HttpCookie httpCookie2 = httpCookie;
								now = DateTime.Now;
								httpCookie2.Expires = now.AddYears(1);
								HttpContext.Current.Response.Cookies.Add(httpCookie);
								context.Response.Write("error");
								return;
							}
							text = memberInfo.UserId.ToString();
						}
						else
						{
							try
							{
								int userId = HiContext.Current.UserId;
								if (userId > 0)
								{
									text = userId.ToString();
								}
							}
							catch (Exception)
							{
							}
						}
						if (string.IsNullOrWhiteSpace(text))
						{
							if (context.Request.Cookies.Get("t_sessionId") != null)
							{
								text = context.Request.Cookies.Get("t_sessionId").Value;
							}
							if (string.IsNullOrWhiteSpace(text))
							{
								guid = Guid.NewGuid();
								text = guid.ToString().Replace("-", "");
								HttpCookie httpCookie3 = new HttpCookie("t_sessionId");
								httpCookie3.HttpOnly = true;
								httpCookie3.Value = text;
								HttpCookie httpCookie4 = httpCookie3;
								now = DateTime.Now;
								httpCookie4.Expires = now.AddYears(1);
								HttpContext.Current.Response.Cookies.Add(httpCookie3);
								context.Response.Write("2");
								return;
							}
						}
					}
					else if (HiContext.Current.UserId == 0)
					{
						if (context.Request.Cookies.Get("t_sessionId") != null)
						{
							text = context.Request.Cookies.Get("t_sessionId").Value;
						}
						if (string.IsNullOrWhiteSpace(text))
						{
							guid = Guid.NewGuid();
							text = guid.ToString().Replace("-", "");
							HttpCookie httpCookie5 = new HttpCookie("t_sessionId");
							httpCookie5.HttpOnly = true;
							httpCookie5.Value = text;
							HttpCookie httpCookie6 = httpCookie5;
							now = DateTime.Now;
							httpCookie6.Expires = now.AddYears(1);
							HttpContext.Current.Response.Cookies.Add(httpCookie5);
							context.Response.Write("2");
							return;
						}
					}
					else
					{
						text = HiContext.Current.UserId.ToString();
					}
					now = DateTime.Now;
					date = now.Date;
					int year = date.Year;
					int month = date.Month;
					int day = date.Day;
					if (3 == num)
					{
						num3 = context.Request["ProductId"].ToInt(0);
						num4 = context.Request["ActivityType"].ToInt(0);
						if (num4 > 0 && num4 <= 5 && (num4 != 4 || (num2 != 1 && num2 != 99)) && num3 > 0)
						{
							goto IL_04a0;
						}
						return;
					}
					goto IL_04a0;
				}
				return;
			}
			goto IL_0539;
			IL_0539:
			context.Response.Write("0");
			return;
			IL_04a0:
			AccessRecordModel accessRecordModel = new AccessRecordModel();
			accessRecordModel.AccessDate = date;
			accessRecordModel.IpAddress = text;
			accessRecordModel.PageType = num;
			accessRecordModel.SourceId = num2;
			accessRecordModel.ProductId = num3;
			accessRecordModel.ActivityType = num4;
			accessRecordModel.StoreId = num5;
			List<AccessRecordModel> list = (List<AccessRecordModel>)HiCache.Get("DataCache-AccessRecords");
			if (list == null)
			{
				list = new List<AccessRecordModel>();
				list.Add(accessRecordModel);
				HiCache.Insert("DataCache-AccessRecords", list, 3600);
			}
			else
			{
				list.Add(accessRecordModel);
			}
			goto IL_0539;
		}

		public string GuidTo16String()
		{
			long num = 1L;
			byte[] array = Guid.NewGuid().ToByteArray();
			foreach (byte b in array)
			{
				num *= b + 1;
			}
			return $"{num - DateTime.Now.Ticks:x}";
		}
	}
}
