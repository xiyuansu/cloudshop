using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class WAPMemberTemplatedWebControl : WAPTemplatedWebControl
	{
		private static string autoSetTags = "UserDefault-AutoSetTags";

		private object lockCopyRedEnvelope = new object();

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			string text = this.Page.Request.QueryString["action"].ToNullString();
			MemberInfo user = HiContext.Current.User;
			string text2 = Globals.UrlDecode(HttpContext.Current.Request.QueryString["ReturnUrl"]);
			if (string.IsNullOrEmpty(text2))
			{
				text2 = HttpContext.Current.Request.Url.ToString();
			}
			DateTime now;
			if (user != null && user.UserId > 0)
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Shop-Member"];
				if (httpCookie != null)
				{
					HttpCookie httpCookie2 = httpCookie;
					now = DateTime.Now;
					httpCookie2.Expires = now.AddDays(30.0);
					HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
				if (!user.CellPhoneVerification)
				{
					string text3 = HttpContext.Current.Request.Url.ToString().ToLower();
					if (text3.IndexOf("/bindphone") == -1)
					{
						if (MemberProcessor.IsTrustLoginUser(user) && HiContext.Current.SiteSettings.QuickLoginIsForceBindingMobbile)
						{
							HttpContext.Current.Response.Redirect("BindPhone");
						}
						else if (HiContext.Current.SiteSettings.UserLoginIsForceBindingMobbile)
						{
							HttpContext.Current.Response.Redirect("BindPhone");
						}
					}
				}
			}
			if (user.UserId == 0 || (user.UserId != 0 && !user.IsLogined))
			{
				string query = HttpContext.Current.Request.Url.Query;
				query = ((string.IsNullOrEmpty(query) || !query.StartsWith("?")) ? "" : ("&" + query.Substring(1)));
				if (base.ClientType == ClientType.VShop)
				{
					OAuthUserInfo oAuthUserInfo = base.GetOAuthUserInfo(true);
					if (!this.HasLogin(oAuthUserInfo.OpenId, oAuthUserInfo.unionId, oAuthUserInfo.IsAttention))
					{
						if (!string.IsNullOrEmpty(oAuthUserInfo.OpenId))
						{
							HttpCookie httpCookie3 = new HttpCookie("openId");
							httpCookie3.HttpOnly = true;
							httpCookie3.Value = oAuthUserInfo.OpenId;
							httpCookie3.Expires = DateTime.MaxValue;
							HttpContext.Current.Response.Cookies.Add(httpCookie3);
						}
						string text4 = Globals.UrlEncode(oAuthUserInfo.HeadImageUrl.ToNullString());
						bool isAttention;
						if (!string.IsNullOrEmpty(oAuthUserInfo.NickName))
						{
							if (string.IsNullOrEmpty(text))
							{
								HttpResponse response = this.Page.Response;
								string[] obj = new string[12]
								{
									"/Vshop/Login?openId=",
									oAuthUserInfo.OpenId,
									"&headimage=",
									text4,
									"&nickname=",
									oAuthUserInfo.NickName,
									"&IsSubscribe=",
									null,
									null,
									null,
									null,
									null
								};
								isAttention = oAuthUserInfo.IsAttention;
								obj[7] = isAttention.ToString();
								obj[8] = "&unionId=";
								obj[9] = oAuthUserInfo.unionId;
								obj[10] = query;
								obj[11] = ((query.IndexOf("returnUrl") > -1) ? "" : ("&returnUrl=" + Globals.UrlEncode(text2)));
								response.Redirect(string.Concat(obj));
							}
							else
							{
								HttpResponse response2 = this.Page.Response;
								string[] obj2 = new string[14]
								{
									"/Vshop/Login.aspx?action=",
									text,
									"&openId=",
									oAuthUserInfo.OpenId,
									"&headimage=",
									text4,
									"&nickname=",
									oAuthUserInfo.NickName,
									"&IsSubscribe=",
									null,
									null,
									null,
									null,
									null
								};
								isAttention = oAuthUserInfo.IsAttention;
								obj2[9] = isAttention.ToString();
								obj2[10] = "&unionId=";
								obj2[11] = oAuthUserInfo.unionId;
								obj2[12] = query;
								obj2[13] = ((query.IndexOf("returnUrl") > -1) ? "" : ("&returnUrl=" + Globals.UrlEncode(text2)));
								response2.Redirect(string.Concat(obj2));
							}
						}
						else if (string.IsNullOrEmpty(text))
						{
							HttpResponse response3 = this.Page.Response;
							string[] obj3 = new string[10]
							{
								"/Vshop/Login?openId=",
								oAuthUserInfo.OpenId,
								"&IsSubscribe=",
								null,
								null,
								null,
								null,
								null,
								null,
								null
							};
							isAttention = oAuthUserInfo.IsAttention;
							obj3[3] = isAttention.ToString();
							obj3[4] = "&headimage=";
							obj3[5] = text4;
							obj3[6] = "&unionId=";
							obj3[7] = oAuthUserInfo.unionId;
							obj3[8] = query;
							obj3[9] = ((query.IndexOf("returnUrl") > -1) ? "" : ("&returnUrl=" + Globals.UrlEncode(text2)));
							response3.Redirect(string.Concat(obj3));
						}
						else
						{
							HttpResponse response4 = this.Page.Response;
							string[] obj4 = new string[12]
							{
								"/Vshop/Login?action=",
								text,
								"&openId=",
								oAuthUserInfo.OpenId,
								"&headimage=",
								text4,
								"&IsSubscribe=",
								null,
								null,
								null,
								null,
								null
							};
							isAttention = oAuthUserInfo.IsAttention;
							obj4[7] = isAttention.ToString();
							obj4[8] = "&unionId=";
							obj4[9] = oAuthUserInfo.unionId;
							obj4[10] = query;
							obj4[11] = ((query.IndexOf("returnUrl") > -1) ? "" : ("&returnUrl=" + Globals.UrlEncode(text2)));
							response4.Redirect(string.Concat(obj4));
						}
					}
				}
				else if (string.IsNullOrEmpty(text))
				{
					this.Page.Response.Redirect("Login?returnUrl=" + HttpUtility.UrlEncode(text2), true);
				}
				else
				{
					this.Page.Response.Redirect("Login?action=" + text + "&returnUrl=" + HttpUtility.UrlEncode(text2), true);
				}
			}
			else
			{
				HttpCookie httpCookie4 = HiContext.Current.Context.Request.Cookies[WAPMemberTemplatedWebControl.autoSetTags + "_" + user.UserId];
				if (httpCookie4 == null)
				{
					IList<MemberTagInfo> list = MemberTagHelper.AutoTagsByMember(user.UserId, user.OrderNumber, user.Expenditure);
					if (list.Count > 0)
					{
						string text5 = user.TagIds;
						foreach (MemberTagInfo item in list)
						{
							if (string.IsNullOrEmpty(text5))
							{
								text5 = text5 + "," + item.TagId + ",";
							}
							if (!("," + text5 + ",").Contains("," + item.TagId + ","))
							{
								text5 = ((text5.LastIndexOf(",") != text5.Length - 1) ? (text5 + "," + item.TagId + ",") : (text5 + item.TagId + ","));
							}
						}
						if (MemberTagHelper.UpdateSingleMemberTags(user.UserId, text5) > 0)
						{
							httpCookie4 = new HttpCookie(WAPMemberTemplatedWebControl.autoSetTags + "_" + user.UserId);
							httpCookie4.HttpOnly = true;
							HttpCookie httpCookie5 = httpCookie4;
							now = DateTime.Now;
							httpCookie5.Expires = now.AddDays(1.0);
							httpCookie4.Value = Globals.UrlEncode(user.UserId.ToString());
							HttpContext.Current.Response.Cookies.Add(httpCookie4);
						}
					}
				}
				HttpCookie httpCookie6 = HiContext.Current.Context.Request.Cookies["openId"];
				if (httpCookie6 != null && !string.IsNullOrEmpty(httpCookie6.Value))
				{
					lock (this.lockCopyRedEnvelope)
					{
						this.CopyRedEnvelope(httpCookie6.Value, user);
					}
				}
			}
		}

		public void WriteError(string msg, string OpenId)
		{
			DataTable dataTable = new DataTable();
			dataTable.TableName = "wxlogin";
			dataTable.Columns.Add("OperTime");
			dataTable.Columns.Add("ErrorMsg");
			dataTable.Columns.Add("OpenId");
			dataTable.Columns.Add("PageUrl");
			DataRow dataRow = dataTable.NewRow();
			dataRow["OperTime"] = DateTime.Now;
			dataRow["ErrorMsg"] = msg;
			dataRow["OpenId"] = OpenId;
			dataRow["PageUrl"] = HttpContext.Current.Request.Url;
			dataTable.Rows.Add(dataRow);
			dataTable.WriteXml(HttpContext.Current.Request.MapPath("/wxlogin" + DateTime.Now.ToString("yyMMddHHmmssfff") + ".xml"));
		}

		public bool HasLogin(string OpenId, string unionId, bool isSubscirbe)
		{
			if (Users.UserIsLogout())
			{
				return false;
			}
			MemberInfo memberInfo = null;
			bool flag = false;
			memberInfo = MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.weixin", OpenId);
			if (memberInfo == null && !string.IsNullOrEmpty(unionId))
			{
				memberInfo = MemberProcessor.GetMemberByUnionId(unionId);
				if (memberInfo != null)
				{
					flag = true;
					if (!memberInfo.IsLogined)
					{
						return false;
					}
				}
			}
			if (memberInfo != null)
			{
				if (memberInfo.IsQuickLogin && !memberInfo.IsLogined)
				{
					return false;
				}
				memberInfo.IsSubscribe = isSubscirbe;
				if (!flag && !string.IsNullOrEmpty(unionId))
				{
					memberInfo.UnionId = unionId;
					MemberProcessor.UpdateMember(memberInfo);
				}
				if (flag)
				{
					memberInfo.IsLogined = true;
					MemberProcessor.UpdateMember(memberInfo);
					MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdDao().GetMemberOpenIdInfo(memberInfo.UserId, "hishop.plugins.openid.weixin");
					if (memberOpenIdInfo == null)
					{
						MemberOpenIdInfo memberOpenIdInfo2 = new MemberOpenIdInfo();
						memberOpenIdInfo2.UserId = memberInfo.UserId;
						memberOpenIdInfo2.OpenIdType = "hishop.plugins.openid.weixin";
						memberOpenIdInfo2.OpenId = OpenId;
						MemberProcessor.AddMemberOpenId(memberOpenIdInfo2);
					}
					else if (memberOpenIdInfo.OpenId != OpenId && !memberInfo.IsQuickLogin)
					{
						memberOpenIdInfo.OpenId = OpenId;
						MemberProcessor.UpdateMemberOpenId(memberOpenIdInfo);
					}
				}
				Users.SetCurrentUser(memberInfo.UserId, 30, true, false);
				HiContext.Current.User = memberInfo;
				HiContext.Current.UserId = memberInfo.UserId;
				lock (this.lockCopyRedEnvelope)
				{
					this.CopyRedEnvelope(OpenId, memberInfo);
				}
				return true;
			}
			return false;
		}

		public void CopyRedEnvelope(string openId, MemberInfo memberInfo)
		{
			IList<RedEnvelopeGetRecordInfo> list = WeiXinRedEnvelopeProcessor.GettWaitToUserRedEnvelopeGetRecord(openId);
			foreach (RedEnvelopeGetRecordInfo item in list)
			{
				WeiXinRedEnvelopeInfo weiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetWeiXinRedEnvelope(item.RedEnvelopeId);
				if (weiXinRedEnvelope != null)
				{
					CouponItemInfo couponItemInfo = new CouponItemInfo();
					couponItemInfo.UserId = memberInfo.UserId;
					couponItemInfo.UserName = memberInfo.UserName;
					couponItemInfo.CanUseProducts = "";
					couponItemInfo.ClosingTime = weiXinRedEnvelope.EffectivePeriodEndTime;
					couponItemInfo.CouponId = 0;
					couponItemInfo.RedEnvelopeId = weiXinRedEnvelope.Id;
					couponItemInfo.CouponName = weiXinRedEnvelope.Name;
					couponItemInfo.OrderUseLimit = weiXinRedEnvelope.EnableUseMinAmount;
					couponItemInfo.Price = item.Amount;
					couponItemInfo.StartTime = weiXinRedEnvelope.EffectivePeriodStartTime;
					couponItemInfo.UseWithGroup = false;
					couponItemInfo.UseWithPanicBuying = false;
					couponItemInfo.GetDate = DateTime.Now;
					if (WeiXinRedEnvelopeProcessor.SetRedEnvelopeGetRecordToMember(item.Id, memberInfo.UserName))
					{
						CouponActionStatus couponActionStatus = CouponHelper.AddRedEnvelopeItemInfo(couponItemInfo);
					}
				}
			}
		}

		public new bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		private new string GetResponseResult(string url)
		{
			ServicePointManager.ServerCertificateValidationCallback = this.CheckValidationResult;
			WebRequest webRequest = WebRequest.Create(url);
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (Stream stream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
					{
						return streamReader.ReadToEnd();
					}
				}
			}
		}

		private void SkipWinxinOpenId(string userName, string openId)
		{
			string generateId = Globals.GetGenerateId();
			MemberInfo memberInfo = new MemberInfo();
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			memberInfo.UserName = userName;
			memberInfo.SessionId = generateId;
			memberInfo.Password = generateId;
			string text2 = memberInfo.PasswordSalt = "Open";
			memberInfo.RealName = string.Empty;
			memberInfo.Address = string.Empty;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num == 0)
			{
				memberInfo.UserName = "weixin" + generateId;
				memberInfo.Password = generateId;
				MemberProcessor.CreateMember(memberInfo);
			}
			MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo();
			memberOpenIdInfo.UserId = num;
			memberOpenIdInfo.OpenIdType = "hishop.plugins.openid.weixin";
			memberOpenIdInfo.OpenId = openId;
			if (MemberProcessor.GetMemberByOpenId(memberOpenIdInfo.OpenIdType, memberOpenIdInfo.OpenId) == null)
			{
				MemberProcessor.AddMemberOpenId(memberOpenIdInfo);
			}
			Users.SetCurrentUser(num, 30, true, false);
			HiContext.Current.User = memberInfo;
		}
	}
}
