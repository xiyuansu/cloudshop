using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class MemberTemplatedWebControl : HtmlTemplatedWebControl
	{
		private static string autoSetTags = "UserDefault-AutoSetTags";

		protected MemberTemplatedWebControl()
		{
			MemberInfo user = HiContext.Current.User;
			if (HiContext.Current.User.UserId == 0)
			{
				this.Page.Response.Redirect($"/User/login?ReturnUrl={this.Page.Request.RawUrl}", true);
			}
			DateTime now;
			if (user != null && user.UserId > 0)
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["PC-Member"];
				if (httpCookie != null)
				{
					HttpCookie httpCookie2 = httpCookie;
					now = DateTime.Now;
					httpCookie2.Expires = now.AddMinutes(30.0);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				if (!user.CellPhoneVerification)
				{
					string text = HttpContext.Current.Request.Url.ToString().ToLower();
					if (text.IndexOf("/user/usercellphoneverification") == -1)
					{
						if (MemberProcessor.IsTrustLoginUser(user) && HiContext.Current.SiteSettings.QuickLoginIsForceBindingMobbile)
						{
							HttpContext.Current.Response.Redirect("/User/UserCellPhoneVerification");
						}
						else if (HiContext.Current.SiteSettings.UserLoginIsForceBindingMobbile)
						{
							HttpContext.Current.Response.Redirect("/User/UserCellPhoneVerification");
						}
					}
				}
			}
			if (user != null)
			{
				HttpCookie httpCookie3 = HiContext.Current.Context.Request.Cookies[MemberTemplatedWebControl.autoSetTags + "_" + user.UserId];
				if (httpCookie3 == null)
				{
					IList<MemberTagInfo> list = MemberTagHelper.AutoTagsByMember(user.UserId, user.OrderNumber, user.Expenditure);
					if (list.Count > 0)
					{
						string text2 = user.TagIds;
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
						if (MemberTagHelper.UpdateSingleMemberTags(user.UserId, text2) > 0)
						{
							httpCookie3 = new HttpCookie(MemberTemplatedWebControl.autoSetTags + "_" + user.UserId)
							{
								HttpOnly = true
							};
							HttpCookie httpCookie4 = httpCookie3;
							now = DateTime.Now;
							httpCookie4.Expires = now.AddDays(1.0);
							httpCookie3.Value = Globals.UrlEncode(user.UserId.ToString());
							HttpContext.Current.Response.Cookies.Add(httpCookie3);
						}
					}
				}
			}
		}
	}
}
