using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.MP;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Handler;
using Hishop.Weixin.MP.Request;
using Hishop.Weixin.MP.Request.Event;
using Hishop.Weixin.MP.Response;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class CustomMsgHandler : RequestHandler
	{
		public CustomMsgHandler(string xml)
			: base(xml)
		{
		}

		public bool IsOpenManyService()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			return masterSettings.OpenManyService;
		}

		public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
		{
			if (!this.IsOpenManyService())
			{
				return null;
			}
			AbstractResponse abstractResponse = new AbstractResponse();
			abstractResponse.FromUserName = requestMessage.ToUserName;
			abstractResponse.ToUserName = requestMessage.FromUserName;
			abstractResponse.MsgType = ResponseMsgType.transfer_customer_service;
			return abstractResponse;
		}

		public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("CreateTime", requestMessage.CreateTime.ToNullString());
			dictionary.Add("FromUserName", requestMessage.FromUserName.ToNullString());
			dictionary.Add("MsgId", requestMessage.MsgId.ToNullString());
			dictionary.Add("MsgType", requestMessage.MsgType.ToNullString());
			ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply();
			if (mismatchReply == null || this.IsOpenManyService())
			{
				return this.GotoManyCustomerService(requestMessage);
			}
			AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName, null);
			if (response == null)
			{
				return this.GotoManyCustomerService(requestMessage);
			}
			response.ToUserName = requestMessage.FromUserName;
			response.FromUserName = requestMessage.ToUserName;
			return response;
		}

		public new AbstractResponse OnLinkRequest(LinkRequest request)
		{
			return null;
		}

		public override AbstractResponse OnTextRequest(TextRequest textRequest)
		{
			AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
			if (keyResponse != null)
			{
				return keyResponse;
			}
			IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys);
			if (replies == null || (replies.Count == 0 && this.IsOpenManyService()))
			{
				return this.GotoManyCustomerService(textRequest);
			}
			foreach (ReplyInfo item in replies)
			{
				if (item.MatchType == MatchType.Equal && item.Keys == textRequest.Content)
				{
					AbstractResponse response = this.GetResponse(item, textRequest.FromUserName, null);
					response.ToUserName = textRequest.FromUserName;
					response.FromUserName = textRequest.ToUserName;
					return response;
				}
				if (item.MatchType == MatchType.Like && item.Keys.Contains(textRequest.Content))
				{
					AbstractResponse response2 = this.GetResponse(item, textRequest.FromUserName, null);
					response2.ToUserName = textRequest.FromUserName;
					response2.FromUserName = textRequest.ToUserName;
					return response2;
				}
			}
			return this.DefaultResponse(textRequest);
		}

		public override AbstractResponse OnEvent_UnSubscribeRequest(UnSubscribeEventRequest unSubscribeEventRequest)
		{
			string fromUserName = unSubscribeEventRequest.FromUserName;
			try
			{
				MemberProcessor.UpdateWXUserIsSubscribeStatus(fromUserName, false);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("openId", fromUserName);
				Globals.WriteExceptionLog(ex, dictionary, "UpdateWXUserIsSubscribeStatus");
			}
			return base.OnEvent_UnSubscribeRequest(unSubscribeEventRequest);
		}

		public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
		{
			string fromUserName = subscribeEventRequest.FromUserName;
			try
			{
				MemberProcessor.UpdateWXUserIsSubscribeStatus(fromUserName, true);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("openId", fromUserName);
				Globals.WriteExceptionLog(ex, dictionary, "UpdateWXUserIsSubscribeStatus");
			}
			ReplyInfo replyInfo = ReplyHelper.GetSubscribeReply();
			if (replyInfo == null)
			{
				replyInfo = new ReplyInfo();
			}
			replyInfo.Keys = "登录";
			if (WeiXinRedEnvelopeProcessor.CheckRedEnvelopeGetRecordNoAttentionIsExist(subscribeEventRequest.FromUserName))
			{
				replyInfo.Keys = "红包";
			}
			if (subscribeEventRequest != null && !string.IsNullOrEmpty(subscribeEventRequest.EventKey) && subscribeEventRequest.EventKey.Split('_').Length == 2)
			{
				string text = subscribeEventRequest.EventKey.Split('_')[1];
				if (text == "referralregister")
				{
					replyInfo.Keys = "分销注册";
				}
				else if (text.Contains("referraluserid"))
				{
					if (text.Split(':').Length == 2)
					{
						int referralUserId = text.Split(':')[1].ToInt(0);
						MemberWXReferralInfo wXReferral = VShopHelper.GetWXReferral(subscribeEventRequest.FromUserName);
						if (wXReferral != null && wXReferral.Id > 0)
						{
							VShopHelper.UpdateWXReferral(subscribeEventRequest.FromUserName, referralUserId);
						}
						else
						{
							VShopHelper.AddWXReferral(subscribeEventRequest.FromUserName, referralUserId);
						}
					}
				}
				else if (text.Contains("shoppingguiderid"))
				{
					if (text.Split(':').Length == 2)
					{
						int shoppingGuiderId = text.Split(':')[1].ToInt(0);
						MemberWXShoppingGuiderInfo memberWXShoppingGuider = MemberHelper.GetMemberWXShoppingGuider(subscribeEventRequest.FromUserName);
						if (memberWXShoppingGuider != null && memberWXShoppingGuider.Id > 0)
						{
							MemberHelper.UpdateWXShoppingGuider(subscribeEventRequest.FromUserName, shoppingGuiderId);
						}
						else
						{
							MemberHelper.AddWXShoppingGuider(subscribeEventRequest.FromUserName, shoppingGuiderId);
						}
					}
				}
				else
				{
					int num = text.ToInt(0);
					if (num > 0)
					{
						RedEnvelopeSendRecord redEnvelopeSendRecordById = WeiXinRedEnvelopeProcessor.GetRedEnvelopeSendRecordById(num);
						if (redEnvelopeSendRecordById != null && WeiXinRedEnvelopeProcessor.IsGetInToday(subscribeEventRequest.FromUserName, redEnvelopeSendRecordById.SendCode, true, ""))
						{
							replyInfo.Keys = "今日已领红包";
						}
					}
				}
			}
			AbstractResponse response = this.GetResponse(replyInfo, subscribeEventRequest.FromUserName, subscribeEventRequest);
			if (response == null)
			{
				return this.GotoManyCustomerService(subscribeEventRequest);
			}
			response.ToUserName = subscribeEventRequest.FromUserName;
			response.FromUserName = subscribeEventRequest.ToUserName;
			return response;
		}

		public override AbstractResponse OnEvent_ScanRequest(ScanEventRequest scanEventRequest)
		{
			ReplyInfo replyInfo = ReplyHelper.GetSubscribeReply();
			string fromUserName = scanEventRequest.FromUserName;
			try
			{
				MemberProcessor.UpdateWXUserIsSubscribeStatus(fromUserName, true);
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("openId", fromUserName);
				Globals.WriteExceptionLog(ex, dictionary, "UpdateWXUserIsSubscribeStatus");
			}
			if (replyInfo == null)
			{
				replyInfo = new ReplyInfo();
			}
			replyInfo.Keys = "登录";
			if (scanEventRequest != null && !string.IsNullOrEmpty(scanEventRequest.EventKey) && scanEventRequest.EventKey.Split('_').Length >= 2)
			{
				string text = scanEventRequest.EventKey.Split('_')[1];
				if (text == "referralregister")
				{
					replyInfo.Keys = "分销注册";
				}
				else if (text.Contains("referraluserid"))
				{
					if (text.Split(':').Length == 2)
					{
						int referralUserId = text.Split(':')[1].ToInt(0);
						MemberWXReferralInfo wXReferral = VShopHelper.GetWXReferral(scanEventRequest.FromUserName);
						if (wXReferral != null && wXReferral.Id > 0)
						{
							VShopHelper.UpdateWXReferral(scanEventRequest.FromUserName, referralUserId);
						}
						else
						{
							VShopHelper.AddWXReferral(scanEventRequest.FromUserName, referralUserId);
						}
					}
				}
				else if (text.Contains("shoppingguiderid") && text.Split(':').Length == 2)
				{
					int shoppingGuiderId = text.Split(':')[1].ToInt(0);
					MemberWXShoppingGuiderInfo memberWXShoppingGuider = MemberHelper.GetMemberWXShoppingGuider(scanEventRequest.FromUserName);
					if (memberWXShoppingGuider != null && memberWXShoppingGuider.Id > 0)
					{
						MemberHelper.UpdateWXShoppingGuider(scanEventRequest.FromUserName, shoppingGuiderId);
					}
					else
					{
						MemberHelper.AddWXShoppingGuider(scanEventRequest.FromUserName, shoppingGuiderId);
					}
				}
			}
			AbstractResponse response = this.GetResponse(replyInfo, scanEventRequest.FromUserName, scanEventRequest);
			if (response == null)
			{
				return this.GotoManyCustomerService(scanEventRequest);
			}
			response.ToUserName = scanEventRequest.FromUserName;
			response.FromUserName = scanEventRequest.ToUserName;
			return response;
		}

		public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
		{
			int num = clickEventRequest.EventKey.ToInt(0);
			MenuInfo menuInfo = null;
			if (num > 0)
			{
				menuInfo = VShopHelper.GetMenu(num);
			}
			if (menuInfo == null)
			{
				return null;
			}
			ReplyInfo reply = ReplyHelper.GetReply(menuInfo.ReplyId);
			if (reply == null)
			{
				return null;
			}
			AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
			if (keyResponse != null)
			{
				return keyResponse;
			}
			AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName, null);
			if (response == null)
			{
				return this.GotoManyCustomerService(clickEventRequest);
			}
			response.ToUserName = clickEventRequest.FromUserName;
			response.FromUserName = clickEventRequest.ToUserName;
			return response;
		}

		public override AbstractResponse OnLocationRequest(LocationRequest locationRequest)
		{
			return base.OnLocationRequest(locationRequest);
		}

		public override AbstractResponse OnEvent_LocationRequest(LocationEventRequest locationEventRequest)
		{
			return base.OnEvent_LocationRequest(locationEventRequest);
		}

		private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
		{
			IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Vote);
			if (replies != null && replies.Count > 0)
			{
				foreach (ReplyInfo item in replies)
				{
					if (item.Keys == key)
					{
						VoteInfo voteById = StoreHelper.GetVoteById(item.ActivityId);
						if (voteById != null && voteById.IsBackup)
						{
							NewsResponse newsResponse = new NewsResponse();
							newsResponse.CreateTime = DateTime.Now;
							newsResponse.FromUserName = request.ToUserName;
							newsResponse.ToUserName = request.FromUserName;
							newsResponse.Articles = new List<Article>();
							newsResponse.Articles.Add(new Article
							{
								Description = voteById.VoteName,
								PicUrl = $"http://{HttpContext.Current.Request.Url.Host}{voteById.ImageUrl}",
								Title = voteById.VoteName,
								Url = $"http://{HttpContext.Current.Request.Url.Host}/vshop/Vote.aspx?voteId={voteById.VoteId}"
							});
							return newsResponse;
						}
					}
				}
			}
			IList<ReplyInfo> replies2 = ReplyHelper.GetReplies(ReplyType.Wheel);
			if (replies2 != null && replies2.Count > 0)
			{
				foreach (ReplyInfo item2 in replies2)
				{
					if (item2.Keys == key)
					{
						LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(item2.ActivityId);
						if (lotteryActivityInfo != null)
						{
							NewsResponse newsResponse2 = new NewsResponse();
							newsResponse2.CreateTime = DateTime.Now;
							newsResponse2.FromUserName = request.ToUserName;
							newsResponse2.ToUserName = request.FromUserName;
							newsResponse2.Articles = new List<Article>();
							newsResponse2.Articles.Add(new Article
							{
								Description = lotteryActivityInfo.ActivityDesc,
								PicUrl = $"http://{HttpContext.Current.Request.Url.Host}{lotteryActivityInfo.ActivityPic}",
								Title = lotteryActivityInfo.ActivityName,
								Url = $"http://{HttpContext.Current.Request.Url.Host}/vshop/BigWheel.aspx?activityId={lotteryActivityInfo.ActivityId}"
							});
							return newsResponse2;
						}
					}
				}
			}
			IList<ReplyInfo> replies3 = ReplyHelper.GetReplies(ReplyType.Scratch);
			if (replies3 != null && replies3.Count > 0)
			{
				foreach (ReplyInfo item3 in replies3)
				{
					if (item3.Keys == key)
					{
						LotteryActivityInfo lotteryActivityInfo2 = VShopHelper.GetLotteryActivityInfo(item3.ActivityId);
						if (lotteryActivityInfo2 != null)
						{
							NewsResponse newsResponse3 = new NewsResponse();
							newsResponse3.CreateTime = DateTime.Now;
							newsResponse3.FromUserName = request.ToUserName;
							newsResponse3.ToUserName = request.FromUserName;
							newsResponse3.Articles = new List<Article>();
							newsResponse3.Articles.Add(new Article
							{
								Description = lotteryActivityInfo2.ActivityDesc,
								PicUrl = $"http://{HttpContext.Current.Request.Url.Host}{lotteryActivityInfo2.ActivityPic}",
								Title = lotteryActivityInfo2.ActivityName,
								Url = $"http://{HttpContext.Current.Request.Url.Host}/vshop/Scratch.aspx?activityId={lotteryActivityInfo2.ActivityId}"
							});
							return newsResponse3;
						}
					}
				}
			}
			IList<ReplyInfo> replies4 = ReplyHelper.GetReplies(ReplyType.SmashEgg);
			if (replies4 != null && replies4.Count > 0)
			{
				foreach (ReplyInfo item4 in replies4)
				{
					if (item4.Keys == key)
					{
						LotteryActivityInfo lotteryActivityInfo3 = VShopHelper.GetLotteryActivityInfo(item4.ActivityId);
						if (lotteryActivityInfo3 != null)
						{
							NewsResponse newsResponse4 = new NewsResponse();
							newsResponse4.CreateTime = DateTime.Now;
							newsResponse4.FromUserName = request.ToUserName;
							newsResponse4.ToUserName = request.FromUserName;
							newsResponse4.Articles = new List<Article>();
							newsResponse4.Articles.Add(new Article
							{
								Description = lotteryActivityInfo3.ActivityDesc,
								PicUrl = $"http://{HttpContext.Current.Request.Url.Host}{lotteryActivityInfo3.ActivityPic}",
								Title = lotteryActivityInfo3.ActivityName,
								Url = $"http://{HttpContext.Current.Request.Url.Host}/vshop/SmashEgg.aspx?activityId={lotteryActivityInfo3.ActivityId}"
							});
							return newsResponse4;
						}
					}
				}
			}
			IList<ReplyInfo> replies5 = ReplyHelper.GetReplies(ReplyType.SignUp);
			if (replies5 != null && replies5.Count > 0)
			{
				foreach (ReplyInfo item5 in replies5)
				{
					if (item5.Keys == key)
					{
						VActivityInfo activity = VShopHelper.GetActivity(item5.ActivityId);
						if (activity != null)
						{
							NewsResponse newsResponse5 = new NewsResponse();
							newsResponse5.CreateTime = DateTime.Now;
							newsResponse5.FromUserName = request.ToUserName;
							newsResponse5.ToUserName = request.FromUserName;
							newsResponse5.Articles = new List<Article>();
							newsResponse5.Articles.Add(new Article
							{
								Description = activity.Description,
								PicUrl = $"http://{HttpContext.Current.Request.Url.Host}{activity.PicUrl}",
								Title = activity.Name,
								Url = $"http://{HttpContext.Current.Request.Url.Host}/vshop/Activity.aspx?id={activity.ActivityId}"
							});
							return newsResponse5;
						}
					}
				}
			}
			IList<ReplyInfo> replies6 = ReplyHelper.GetReplies(ReplyType.Ticket);
			if (replies6 != null && replies6.Count > 0)
			{
				foreach (ReplyInfo item6 in replies6)
				{
					if (item6.Keys == key)
					{
						LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(item6.ActivityId);
						if (lotteryTicket != null)
						{
							NewsResponse newsResponse6 = new NewsResponse();
							newsResponse6.CreateTime = DateTime.Now;
							newsResponse6.FromUserName = request.ToUserName;
							newsResponse6.ToUserName = request.FromUserName;
							newsResponse6.Articles = new List<Article>();
							newsResponse6.Articles.Add(new Article
							{
								Description = lotteryTicket.ActivityDesc,
								PicUrl = $"http://{HttpContext.Current.Request.Url.Host}{lotteryTicket.ActivityPic}",
								Title = lotteryTicket.ActivityName,
								Url = $"http://{HttpContext.Current.Request.Url.Host}/vshop/SignUp.aspx?id={lotteryTicket.ActivityId}"
							});
							return newsResponse6;
						}
					}
				}
			}
			return null;
		}

		public AbstractResponse GetResponse(ReplyInfo reply, string openId, AbstractRequest bstractRequest = null)
		{
			if (reply.MessageType == MessageType.Text)
			{
				TextReplyInfo textReplyInfo = reply as TextReplyInfo;
				TextResponse textResponse = new TextResponse();
				textResponse.CreateTime = DateTime.Now;
				if (textReplyInfo != null && !string.IsNullOrEmpty(textReplyInfo.Text))
				{
					textResponse.Content = textReplyInfo.Text;
				}
				if (reply.Keys == "登录")
				{
					string arg = $"http://{HttpContext.Current.Request.Url.Host}/Vshop/MemberCenter.aspx?SessionId={openId}";
					textResponse.Content = textResponse.Content.Replace("$login$", $"<a href=\"{arg}\">一键登录</a>");
				}
				if (reply.Keys == "红包")
				{
					string value = ((SubscribeEventRequest)bstractRequest).EventKey.Split('_')[1];
					RedEnvelopeSendRecord redEnvelopeSendRecordById = WeiXinRedEnvelopeProcessor.GetRedEnvelopeSendRecordById(Convert.ToInt32(value));
					if (redEnvelopeSendRecordById != null)
					{
						string arg2 = $"http://{HttpContext.Current.Request.Url.Host}/Vshop/GetRedEnvelope?SendCode={redEnvelopeSendRecordById.SendCode}&OrderId={redEnvelopeSendRecordById.OrderId}";
						textResponse.Content += $"<a href=\"{arg2}\">立即领取红包</a>";
					}
				}
				if (reply.Keys == "今日已领红包")
				{
					string arg3 = $"http://{HttpContext.Current.Request.Url.Host}/Vshop/Default.aspx";
					textResponse.Content += $"<a href=\"{arg3}\">你今日已领取过了，立即购物</a>";
				}
				if (reply.Keys == "分销注册")
				{
					string arg4 = $"http://{HttpContext.Current.Request.Url.Host}/Vshop/ReferralRegister.aspx?again=1";
					textResponse.Content += $"感谢您的关注。点击<a href=\"{arg4}\">注册</a>成为本商城分销员，赚取丰厚奖励！";
				}
				return textResponse;
			}
			HttpContext current = HttpContext.Current;
			NewsResponse newsResponse = new NewsResponse();
			newsResponse.CreateTime = DateTime.Now;
			newsResponse.Articles = new List<Article>();
			foreach (NewsMsgInfo item2 in (reply as NewsReplyInfo).NewsMsg)
			{
				Article item = new Article
				{
					Description = item2.Description,
					PicUrl = $"{Globals.GetProtocal(current)}://{HttpContext.Current.Request.Url.Host}{item2.PicUrl}",
					Title = item2.Title,
					Url = (string.IsNullOrEmpty(item2.Url) ? $"{Globals.GetProtocal(current)}://{HttpContext.Current.Request.Url.Host}/Vshop/ImageTextDetails.aspx?messageId={item2.Id}" : item2.Url)
				};
				newsResponse.Articles.Add(item);
			}
			return newsResponse;
		}
	}
}
