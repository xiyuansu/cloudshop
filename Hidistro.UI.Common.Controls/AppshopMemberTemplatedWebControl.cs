using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public class AppshopMemberTemplatedWebControl : AppshopTemplatedWebControl
	{
		private static string autoSetTags = "UserDefault-AutoSetTags";

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			MemberInfo memberInfo = HiContext.Current.User;
			if (HiContext.Current.UserId == 0)
			{
				string text = HttpContext.Current.Request["SessionId"].ToNullString();
				if (!string.IsNullOrEmpty(text))
				{
					memberInfo = MemberProcessor.FindMemberBySessionId(text);
					if (memberInfo == null)
					{
						HttpContext.Current.Response.Write("sessionid过期");
						HttpContext.Current.Response.End();
					}
					HiContext.Current.User = memberInfo;
					Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
				}
			}
			DateTime now;
			if (memberInfo != null && memberInfo.UserId > 0)
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Shop-Member"];
				if (httpCookie != null)
				{
					HttpCookie httpCookie2 = httpCookie;
					now = DateTime.Now;
					httpCookie2.Expires = now.AddDays(30.0);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
			}
			if (memberInfo != null)
			{
				HttpCookie httpCookie3 = HiContext.Current.Context.Request.Cookies[AppshopMemberTemplatedWebControl.autoSetTags + "_" + memberInfo.UserId];
				if (httpCookie3 == null)
				{
					IList<MemberTagInfo> list = MemberTagHelper.AutoTagsByMember(memberInfo.UserId, memberInfo.OrderNumber, memberInfo.Expenditure);
					if (list.Count > 0)
					{
						string text2 = memberInfo.TagIds;
						foreach (MemberTagInfo item in list)
						{
							if (string.IsNullOrEmpty(text2))
							{
								text2 = text2 + "," + item.TagId + ",";
							}
							if (!("," + text2 + ",").Contains("," + item.TagId + ","))
							{
								text2 = ((text2.LastIndexOf(",") != text2.Length - 1) ? (text2 + "," + item.TagId + ",") : (text2 + item.TagId + ","));
							}
						}
						if (MemberTagHelper.UpdateSingleMemberTags(memberInfo.UserId, text2) > 0)
						{
							httpCookie3 = new HttpCookie(AppshopMemberTemplatedWebControl.autoSetTags + "_" + memberInfo.UserId);
							httpCookie3.HttpOnly = true;
							HttpCookie httpCookie4 = httpCookie3;
							now = DateTime.Now;
							httpCookie4.Expires = now.AddDays(1.0);
							httpCookie3.Value = Globals.UrlEncode(memberInfo.UserId.ToString());
							HttpContext.Current.Response.Cookies.Add(httpCookie3);
						}
					}
				}
			}
		}

		protected override void AttachChildControls()
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
