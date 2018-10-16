using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class GetRedEnvelope : WAPTemplatedWebControl
	{
		private WapTemplatedRepeater rptRedEnvelopeGetRecord;

		private WeiXinRedEnvelopeInfo weiXinRedEnvelope;

		private Literal redEnvelopeAmount;

		private HtmlAnchor toBuy;

		private HtmlButton toLogin;

		private HtmlGenericControl divGetList;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-GetRedEnvelope.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.toBuy = (HtmlAnchor)this.FindControl("toBuy");
			this.toLogin = (HtmlButton)this.FindControl("toLogin");
			this.divGetList = (HtmlGenericControl)this.FindControl("divGetList");
			this.toBuy.Visible = true;
			this.toLogin.Visible = false;
			this.divGetList.Visible = true;
			string text = this.Page.Request["SendCode"];
			string orderId = this.Page.Request["OrderId"];
			OAuthUserInfo oAuthUserInfo = this.Context.Session["oAuthUserInfo"] as OAuthUserInfo;
			this.CheckRedEnvelope(text, orderId);
			if (HiContext.Current.UserId > 0 && oAuthUserInfo == null)
			{
				MemberInfo user = HiContext.Current.User;
				MemberOpenIdInfo memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
				if (memberOpenIdInfo != null)
				{
					oAuthUserInfo = new OAuthUserInfo();
					oAuthUserInfo.HeadImageUrl = user.Picture;
					oAuthUserInfo.IsAttention = user.IsSubscribe;
					oAuthUserInfo.NickName = user.NickName;
					oAuthUserInfo.OpenId = memberOpenIdInfo.OpenId;
					oAuthUserInfo.unionId = user.UnionId;
				}
			}
			if (oAuthUserInfo == null)
			{
				oAuthUserInfo = base.GetOAuthUserInfo(true);
				if (string.IsNullOrEmpty(oAuthUserInfo.OpenId))
				{
					Globals.AppendLog("第一次获取用户OpenId失败,错误原因:" + oAuthUserInfo.ErrMsg, "", "", "OAuthUserInfoError");
					this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=授权信息获取失败,请重试", true);
				}
				this.Context.Session["oAuthUserInfo"] = oAuthUserInfo;
			}
			if (string.IsNullOrEmpty(oAuthUserInfo.OpenId))
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=用户OpenId获取失败,错误原因:" + oAuthUserInfo.ErrMsg, true);
			}
			Guid sendCode = Guid.Parse(text);
			RedEnvelopeGetRecordInfo redEnvelopeGetRecord = this.GetRedEnvelopeGetRecord(oAuthUserInfo, sendCode, orderId);
			int redEnvelopeGetRecordcCount = WeiXinRedEnvelopeProcessor.GetRedEnvelopeGetRecordcCount(this.weiXinRedEnvelope.Id, sendCode, orderId);
			RedEnvelopeGetRecordInfo lastRedEnvelopeGetRecord = WeiXinRedEnvelopeProcessor.GetLastRedEnvelopeGetRecord(oAuthUserInfo.OpenId, sendCode, orderId);
			DateTime dateTime = lastRedEnvelopeGetRecord?.GetTime ?? DateTime.Now.AddDays(-1.0);
			DateTime now = DateTime.Now;
			bool flag = false;
			TimeSpan timeSpan = new TimeSpan(dateTime.Ticks);
			TimeSpan ts = new TimeSpan(now.Ticks);
			if (timeSpan.Subtract(ts).Duration().TotalSeconds <= 4.0)
			{
				flag = true;
			}
			if (!flag)
			{
				if (redEnvelopeGetRecordcCount >= this.weiXinRedEnvelope.MaxNumber)
				{
					if (oAuthUserInfo.IsAttention && WeiXinRedEnvelopeProcessor.IsGetInToday(oAuthUserInfo.OpenId, sendCode, true, orderId))
					{
						this.Page.Response.Redirect("/Vshop/RedEnvelopePrompt", true);
					}
					else
					{
						this.Page.Response.Redirect("/Vshop/RedEnvelopeFinish", true);
					}
				}
				if (WeiXinRedEnvelopeProcessor.IsGetInToday(oAuthUserInfo.OpenId, sendCode, null, orderId))
				{
					this.Page.Response.Redirect("/Vshop/RedEnvelopePrompt", true);
				}
				if (WeiXinRedEnvelopeProcessor.GetInTodayCount(oAuthUserInfo.OpenId, "", null, "") >= 3)
				{
					this.Page.Response.Redirect("/Vshop/RedEnvelopePrompt", true);
				}
				if (!oAuthUserInfo.IsAttention)
				{
					if (!WeiXinRedEnvelopeProcessor.IsGetInToday(oAuthUserInfo.OpenId, sendCode, null, ""))
					{
						WeiXinRedEnvelopeProcessor.AddRedEnvelopeGetRecord(redEnvelopeGetRecord);
						this.AddCouponItemInfo(oAuthUserInfo, redEnvelopeGetRecord);
					}
					RedEnvelopeSendRecord redEnvelopeSendRecord = WeiXinRedEnvelopeProcessor.GetRedEnvelopeSendRecord(Guid.Parse(text), "", "");
					this.Page.Response.Redirect($"AttentionHNYSJY?SendRecordId={redEnvelopeSendRecord.Id}", true);
				}
				else
				{
					string openId = oAuthUserInfo.OpenId;
					if (WeiXinRedEnvelopeProcessor.CheckRedEnvelopeGetRecordNoAttentionIsExist(openId))
					{
						WeiXinRedEnvelopeProcessor.SetRedEnvelopeGetRecordToAttention(oAuthUserInfo.NickName, oAuthUserInfo.HeadImageUrl, openId);
					}
					else
					{
						WeiXinRedEnvelopeProcessor.AddRedEnvelopeGetRecord(redEnvelopeGetRecord);
						this.AddCouponItemInfo(oAuthUserInfo, redEnvelopeGetRecord);
					}
				}
			}
			this.redEnvelopeAmount = (Literal)this.FindControl("RedEnvelopeAmount");
			this.redEnvelopeAmount.Text = (flag ? ((lastRedEnvelopeGetRecord != null) ? lastRedEnvelopeGetRecord.Amount.F2ToString("f2") : "0") : redEnvelopeGetRecord.Amount.F2ToString("f2"));
			this.rptRedEnvelopeGetRecord = (WapTemplatedRepeater)this.FindControl("rptRedEnvelopeGetRecord");
			this.RedEnvelopeGetRecordBind(text);
		}

		private bool CheckRedEnvelope(string sendCode, string orderId)
		{
			if (string.IsNullOrEmpty(sendCode))
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=发送码错误", true);
			}
			if (string.IsNullOrEmpty(orderId))
			{
				NameValueCollection nameValueCollection = new NameValueCollection
				{
					this.Context.Request.QueryString,
					this.Context.Request.Form
				};
				nameValueCollection.Add("OrderId1", orderId);
				nameValueCollection.Add("sendCode1", sendCode);
				Globals.AppendLog(nameValueCollection, "", "", "", "CheckRedEnvelope");
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=订单编号为空", true);
			}
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
			if (orderInfo == null)
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=订单不存在", true);
			}
			RedEnvelopeSendRecord redEnvelopeSendRecord = WeiXinRedEnvelopeProcessor.GetRedEnvelopeSendRecord(Guid.Parse(sendCode), orderId, "");
			if (redEnvelopeSendRecord == null)
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=发送码不存在", true);
			}
			this.weiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetWeiXinRedEnvelope(redEnvelopeSendRecord.RedEnvelopeId.Value);
			if (this.weiXinRedEnvelope == null)
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=红包活动已经被删除", true);
			}
			if (this.weiXinRedEnvelope.State == 0)
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=红包活动已经关闭", true);
			}
			DateTime now = DateTime.Now;
			if (now > this.weiXinRedEnvelope.ActiveStartTime && this.weiXinRedEnvelope.ActiveEndTime < now)
			{
				this.Page.Response.Redirect("/Vshop/RedEnvelopeError?errorInfo=红包活动没有开始或已过期", true);
			}
			return true;
		}

		public void AddCouponItemInfo(OAuthUserInfo oAuthUserInfo, RedEnvelopeGetRecordInfo redEnvelopeGetRecord)
		{
			CouponItemInfo couponItemInfo = new CouponItemInfo();
			MemberInfo memberInfo = null;
			memberInfo = ((HiContext.Current.UserId <= 0) ? MemberProcessor.GetMemberByOpenId("hishop.plugins.openid.weixin", oAuthUserInfo.OpenId) : HiContext.Current.User);
			if (memberInfo == null)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("OpenId", oAuthUserInfo.OpenId);
				dictionary.Add("IsAttention", oAuthUserInfo.IsAttention.ToNullString());
				dictionary.Add("unionId", oAuthUserInfo.unionId);
				dictionary.Add("NickName", oAuthUserInfo.NickName);
				dictionary.Add("ErrMsg", oAuthUserInfo.ErrMsg);
				dictionary.Add("RedEnvelopeId", redEnvelopeGetRecord.RedEnvelopeId.ToNullString());
				dictionary.Add("UserName", redEnvelopeGetRecord.UserName.ToNullString());
				dictionary.Add("OrderId", redEnvelopeGetRecord.OrderId.ToNullString());
				dictionary.Add("RedOpenId", redEnvelopeGetRecord.OpenId.ToNullString());
				dictionary.Add("SendCode", redEnvelopeGetRecord.SendCode.ToNullString());
				Globals.AppendLog(dictionary, "", "", "", "AddCouponItemInfo");
				this.toLogin.Visible = true;
				this.toBuy.Visible = false;
				this.divGetList.Visible = false;
			}
			else
			{
				couponItemInfo.UserId = memberInfo.UserId;
				couponItemInfo.UserName = redEnvelopeGetRecord.UserName;
				couponItemInfo.CanUseProducts = "";
				couponItemInfo.ClosingTime = this.weiXinRedEnvelope.EffectivePeriodEndTime;
				couponItemInfo.CouponId = 0;
				couponItemInfo.RedEnvelopeId = this.weiXinRedEnvelope.Id;
				couponItemInfo.CouponName = this.weiXinRedEnvelope.Name;
				couponItemInfo.OrderUseLimit = this.weiXinRedEnvelope.EnableUseMinAmount;
				couponItemInfo.Price = redEnvelopeGetRecord.Amount;
				couponItemInfo.StartTime = this.weiXinRedEnvelope.EffectivePeriodStartTime;
				couponItemInfo.UseWithGroup = false;
				couponItemInfo.UseWithPanicBuying = false;
				couponItemInfo.UseWithFireGroup = false;
				couponItemInfo.GetDate = DateTime.Now;
				if (WeiXinRedEnvelopeProcessor.SetRedEnvelopeGetRecordToMember(redEnvelopeGetRecord.Id, memberInfo.UserName))
				{
					CouponActionStatus couponActionStatus = CouponHelper.AddRedEnvelopeItemInfo(couponItemInfo);
				}
			}
		}

		private void RedEnvelopeGetRecordBind(string sendCode)
		{
			this.rptRedEnvelopeGetRecord.DataSource = WeiXinRedEnvelopeProcessor.GetRedEnvelopeGetRecord(20, Guid.Parse(sendCode));
			this.rptRedEnvelopeGetRecord.DataBind();
		}

		private RedEnvelopeGetRecordInfo GetRedEnvelopeGetRecord(OAuthUserInfo authUserInfo, Guid sendCode, string orderId)
		{
			decimal amount = this.GetRedEnvelopeAmount(this.weiXinRedEnvelope);
			RedEnvelopeGetRecordInfo redEnvelopeGetRecordInfo = new RedEnvelopeGetRecordInfo();
			redEnvelopeGetRecordInfo.RedEnvelopeId = this.weiXinRedEnvelope.Id;
			redEnvelopeGetRecordInfo.UserName = string.Empty;
			redEnvelopeGetRecordInfo.OpenId = authUserInfo.OpenId;
			redEnvelopeGetRecordInfo.NickName = authUserInfo.NickName;
			redEnvelopeGetRecordInfo.HeadImgUrl = authUserInfo.HeadImageUrl;
			redEnvelopeGetRecordInfo.IsAttention = authUserInfo.IsAttention;
			redEnvelopeGetRecordInfo.GetTime = DateTime.Now;
			redEnvelopeGetRecordInfo.Amount = amount;
			redEnvelopeGetRecordInfo.SendCode = sendCode;
			redEnvelopeGetRecordInfo.OrderId = orderId;
			return redEnvelopeGetRecordInfo;
		}

		public decimal GetRedEnvelopeAmount(WeiXinRedEnvelopeInfo weiXinRedEnvelope)
		{
			if (weiXinRedEnvelope.MinAmount == weiXinRedEnvelope.MaxAmount)
			{
				return weiXinRedEnvelope.MaxAmount;
			}
			return this.GetRandomNumber(weiXinRedEnvelope.MinAmount, weiXinRedEnvelope.MaxAmount, 2);
		}

		public decimal GetRandomNumber(decimal minimum, decimal maximum, int Len)
		{
			Random random = new Random();
			return Math.Round((decimal)random.NextDouble() * (maximum - minimum) + minimum, Len);
		}
	}
}
