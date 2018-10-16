using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.push;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using com.igetui.api.openservice.payload;
using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SqlDal.App;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.VShop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Hidistro.SaleSystem.Store
{
	public static class VShopHelper
	{
		public static readonly char SEPARATORCONTEXT = '§';

		public static readonly char SEPARATOREVERY = '∽';

		public static object APPPUSHLOCK = new object();

		private static string HOST = "http://sdk.open.api.igexin.com/apiex.htm";

		public static IEnumerable<AppPushRecordInfo> AppPushListByUserForIOS(int userId)
		{
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
			MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user.GradeId);
			string gradeId = memberGrade.GradeId.ToString();
			return new AppPushMsgDao().AppPushListByUserForIOS(userId, gradeId);
		}

		public static void AppPushSetReadForIOS(int pushRecordId, int userId)
		{
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			lock (VShopHelper.APPPUSHLOCK)
			{
				if (!appPushMsgDao.IsPushRecordRead(pushRecordId, userId))
				{
					AppPushRecordUserReadInfo model = new AppPushRecordUserReadInfo
					{
						PushRecordId = pushRecordId,
						UserId = userId
					};
					appPushMsgDao.Add(model, null);
				}
			}
		}

		public static int AppPushRecordForIOS(int userId)
		{
			Hidistro.Entities.Members.MemberInfo user = Users.GetUser(userId);
			MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user.GradeId);
			string gradeId = memberGrade.GradeId.ToString();
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			return appPushMsgDao.AppPushRecordCountForIOS(userId, gradeId);
		}

		public static int GetPushRecordCountOfMsgType(int userId, int msgType)
		{
			return new AppPushMsgDao().GetPushRecordNotReadListOfMsgType(userId, msgType).Count();
		}

		public static IList<AppPushRecordInfo> GetPushRecordsOfMsgType(int userId, int msgType)
		{
			return new AppPushMsgDao().GetPushRecordNotReadListOfMsgType(userId, msgType);
		}

		public static void AppPsuhRecordForStore(int storeId, string orderId, string skuId, EnumPushStoreAction pushStoreAction)
		{
			StoresInfo storeById = StoresHelper.GetStoreById(storeId);
			if (storeById != null)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenMultStore)
				{
					AppPushRecordInfo appPushRecordInfo = new AppPushRecordInfo();
					DateTime dateTime = DateTime.Now;
					dateTime = dateTime.Date;
					appPushRecordInfo.PushSendDate = dateTime.AddHours((double)DateTime.Now.Hour);
					appPushRecordInfo.PushSendType = 1;
					appPushRecordInfo.PushStatus = 1;
					appPushRecordInfo.PushType = 1;
					AppPushRecordInfo appPushRecordInfo2 = appPushRecordInfo;
					try
					{
						OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
						string pushTitle = masterSettings.SiteName;
						string pushContent = string.Empty;
						object[] obj = new object[7]
						{
							"PushMsgType",
							null,
							null,
							null,
							null,
							null,
							null
						};
						char c = VShopHelper.SEPARATORCONTEXT;
						obj[1] = c.ToString();
						obj[2] = (int)pushStoreAction;
						c = VShopHelper.SEPARATOREVERY;
						obj[3] = c.ToString();
						obj[4] = "PushRecordId";
						c = VShopHelper.SEPARATORCONTEXT;
						obj[5] = c.ToString();
						obj[6] = "key";
						string text = string.Concat(obj);
						int num;
						switch (pushStoreAction)
						{
						case EnumPushStoreAction.StoreOrderPayed:
							pushTitle = "订单已支付";
							pushContent = $"您好，门店有订单{orderId}已付款，请及时备货发货";
							text = text.Replace("key", orderId);
							break;
						case EnumPushStoreAction.StoreOrderRefundApply:
						{
							RefundInfo refundInfo = TradeHelper.GetRefundInfo(orderInfo);
							if (refundInfo != null)
							{
								pushTitle = "退款待处理";
								pushContent = string.Format("您好，门店有订单{0}已申请退款，退款金额{1}元,请即时处理", orderId, refundInfo.RefundAmount.F2ToString("f2"));
								string text3 = text;
								num = refundInfo.RefundId;
								text = text3.Replace("key", num.ToString());
								break;
							}
							return;
						}
						case EnumPushStoreAction.StoreOrderReturnApply:
						{
							ReturnInfo returnInfo = TradeHelper.GetReturnInfo(orderId, skuId);
							if (returnInfo != null)
							{
								pushTitle = "退货待处理";
								pushContent = string.Format("您好，门店有订单{0}已申请退货，退款金额{1}元,请即时处理", orderId, returnInfo.RefundAmount.F2ToString("f2"));
								string text4 = text;
								num = returnInfo.ReturnId;
								text = text4.Replace("key", num.ToString());
								break;
							}
							return;
						}
						case EnumPushStoreAction.StoreOrderReplaceApply:
						{
							ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(orderId, skuId);
							if (replaceInfo != null)
							{
								pushTitle = "换货待处理";
								pushContent = $"您好，门店有订单{orderId}已申请换,请即时处理";
								string text2 = text;
								num = replaceInfo.ReplaceId;
								text = text2.Replace("key", num.ToString());
								break;
							}
							return;
						}
						case EnumPushStoreAction.StoreOrderWaitSendGoods:
							pushTitle = "门店订单待发货";
							pushContent = $"您好，门店有订单{orderId}等待发货，请及时处理";
							text = text.Replace("key", orderId);
							break;
						case EnumPushStoreAction.TakeOnStoreOrderWaitConfirm:
							pushTitle = "上门自提订单待确认";
							pushContent = $"您好，门店有上门自提的订单{orderId}待确认,请即时处理";
							text = text.Replace("key", orderId);
							break;
						case EnumPushStoreAction.StoreStockWarning:
							pushTitle = "门店库存警告";
							pushContent = "您好，门店有商品库存不足,请即时补充库存";
							text = text.Replace("key", "");
							break;
						}
						appPushRecordInfo2.Extras = text.Replace("key", "");
						appPushRecordInfo2.PushContent = pushContent;
						appPushRecordInfo2.PushTitle = pushTitle;
						appPushRecordInfo2.PushContent = pushContent;
						appPushRecordInfo2.UserId = storeById.StoreId;
						appPushRecordInfo2.PushTagText = $"推送门店【{storeById.StoreName}】";
						appPushRecordInfo2.PushMsgType = (int)pushStoreAction;
						VShopHelper.SendMessage(appPushRecordInfo2, false, true);
					}
					catch (Exception ex)
					{
						IDictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("Extras", appPushRecordInfo2.Extras);
						dictionary.Add("PushContent", appPushRecordInfo2.PushContent);
						dictionary.Add("PushMsgType", appPushRecordInfo2.PushMsgType.Value.ToNullString());
						dictionary.Add("PushRecordId", appPushRecordInfo2.PushRecordId.ToNullString());
						dictionary.Add("PushRemark", appPushRecordInfo2.PushRemark.ToNullString());
						dictionary.Add("PushSendDate", appPushRecordInfo2.PushSendDate.ToNullString());
						dictionary.Add("PushSendTime", appPushRecordInfo2.PushSendTime.ToNullString());
						dictionary.Add("PushSendType", appPushRecordInfo2.PushSendType.ToNullString());
						dictionary.Add("PushStatus", appPushRecordInfo2.PushStatus.ToNullString());
						dictionary.Add("PushTag", appPushRecordInfo2.PushTag.ToNullString());
						dictionary.Add("PushTagText", appPushRecordInfo2.PushTagText.ToNullString());
						dictionary.Add("PushTitle", appPushRecordInfo2.PushTitle.ToNullString());
						dictionary.Add("PushType", appPushRecordInfo2.PushType.ToNullString());
						dictionary.Add("ToAll", appPushRecordInfo2.ToAll.ToNullString());
						dictionary.Add("UserId", appPushRecordInfo2.UserId.ToNullString());
						Globals.WriteExceptionLog(ex, dictionary, "AppStorePush");
					}
				}
			}
		}

		public static void AppPushRecordForOrder(string orderId, string skuId, EnumPushOrderAction pushOrderAction)
		{
			bool flag = false;
			try
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenMobbile > 0)
				{
					string text = masterSettings.SiteUrl.ToNullString().ToLower();
					if (!text.StartsWith("http://"))
					{
						text = "http://" + text;
					}
					AppPushRecordInfo appPushRecordInfo = new AppPushRecordInfo();
					DateTime dateTime = DateTime.Now;
					dateTime = dateTime.Date;
					appPushRecordInfo.PushSendDate = dateTime.AddHours((double)DateTime.Now.Hour);
					appPushRecordInfo.PushSendType = 1;
					appPushRecordInfo.PushStatus = 1;
					appPushRecordInfo.PushType = 1;
					AppPushRecordInfo appPushRecordInfo2 = appPushRecordInfo;
					OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
					string pushTitle = masterSettings.SiteName;
					string pushContent = string.Empty;
					DataTable dataTable = new DataTable();
					int num = 0;
					switch (pushOrderAction)
					{
					case EnumPushOrderAction.OrderSended:
						flag = masterSettings.EnableAppPushSetOrderSend.ToBool();
						pushTitle = "订单已发货";
						pushContent = $"您好，您的订单{orderId}已发货，请您注意查收";
						appPushRecordInfo2.Extras = string.Format("url{1}{0}{2}", text + "/appshop/MemberOrderDetails?orderId=" + orderId, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						break;
					case EnumPushOrderAction.OrderRefund:
					{
						flag = masterSettings.EnableAppPushSetOrderRefund.ToBool();
						pushTitle = "退款通知";
						pushContent = $"您好，您的订单{orderId}退款申请已处理";
						RefundInfo refundInfo = TradeHelper.GetRefundInfo(orderId);
						if (refundInfo != null)
						{
							num = refundInfo.RefundId;
						}
						appPushRecordInfo2.Extras = string.Format("url{1}{0}{2}", text + "/appshop/UserRefundDetail?RefundId=" + num, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						break;
					}
					case EnumPushOrderAction.OrderReturnConfirm:
					{
						flag = masterSettings.EnableAppPushSetOrderReturn.ToBool();
						pushTitle = "退货通知";
						pushContent = $"您好，您的订单{orderId}退货申请已通过，请您尽快寄回商品";
						ReturnInfo returnInfo2 = TradeHelper.GetReturnInfo(orderId, skuId);
						if (returnInfo2 != null)
						{
							num = returnInfo2.ReturnId;
						}
						appPushRecordInfo2.Extras = string.Format("url{1}{0}{2}", text + "/appshop/UserReturnDetail?ReturnId=" + num, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						break;
					}
					case EnumPushOrderAction.OrderReturnFinish:
					{
						flag = masterSettings.EnableAppPushSetOrderReturn.ToBool();
						pushTitle = "退货通知";
						pushContent = $"您好，您的订单{orderId}退货申请已处理";
						ReturnInfo returnInfo = TradeHelper.GetReturnInfo(orderId, skuId);
						if (returnInfo != null)
						{
							num = returnInfo.ReturnId;
						}
						appPushRecordInfo2.Extras = string.Format("url{1}{0}{2}", text + "/appshop/UserReturnDetail?ReturnId=" + num, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
						break;
					}
					}
					appPushRecordInfo2.PushContent = pushContent;
					appPushRecordInfo2.PushTitle = pushTitle;
					appPushRecordInfo2.PushContent = pushContent;
					appPushRecordInfo2.UserId = orderInfo.UserId;
					appPushRecordInfo2.PushTagText = $"推送会员【{orderInfo.Username}】";
					appPushRecordInfo2.PushMsgType = (int)pushOrderAction;
					if (flag)
					{
						VShopHelper.SendMessage(appPushRecordInfo2, false, false);
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("checkSend", flag.ToString());
				dictionary.Add("orderId", orderId);
				dictionary.Add("skuId", skuId);
				IDictionary<string, string> dictionary2 = dictionary;
				int num2 = (int)pushOrderAction;
				dictionary2.Add("pushOrderAction", num2.ToString());
				Globals.WriteExceptionLog(ex, dictionary, "AppPushRecordForOrder");
			}
		}

		public static void AppPushRecordForPreSaleOrder(string orderId, int UserId, string Username, decimal Amount, DateTime dtStart, DateTime dtEnd)
		{
			try
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (masterSettings.OpenMobbile > 0)
				{
					string text = masterSettings.SiteUrl.ToNullString().ToLower();
					if (!text.StartsWith("http://"))
					{
						text = "http://" + text;
					}
					AppPushRecordInfo appPushRecordInfo = new AppPushRecordInfo();
					DateTime dateTime = DateTime.Now;
					dateTime = dateTime.Date;
					appPushRecordInfo.PushSendDate = dateTime.AddHours((double)DateTime.Now.Hour);
					appPushRecordInfo.PushSendType = 1;
					appPushRecordInfo.PushStatus = 1;
					appPushRecordInfo.PushType = 1;
					AppPushRecordInfo appPushRecordInfo2 = appPushRecordInfo;
					string siteName = masterSettings.SiteName;
					string empty = string.Empty;
					DataTable dataTable = new DataTable();
					siteName = "预售订单提醒";
					empty = string.Format("您好，您的订单{0}已完成定金支付！请您在{1}至{2}时间内完成尾款的支付,过期定金将不予以退还，请您谅解！", orderId, dtStart.ToString("yyyy/MM/dd"), dtEnd.ToString("yyyy/MM/dd"));
					appPushRecordInfo2.Extras = string.Format("url{1}{0}{2}", text + "/appshop/MemberOrderDetails?orderId=" + orderId, VShopHelper.SEPARATORCONTEXT, VShopHelper.SEPARATOREVERY);
					appPushRecordInfo2.PushContent = empty;
					appPushRecordInfo2.PushTitle = siteName;
					appPushRecordInfo2.PushContent = empty;
					appPushRecordInfo2.UserId = UserId;
					appPushRecordInfo2.PushTagText = $"推送会员【{Username}】";
					appPushRecordInfo2.PushMsgType = 5;
					VShopHelper.SendMessage(appPushRecordInfo2, false, false);
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("orderId", orderId);
				dictionary.Add("Amount", Amount.ToString());
				dictionary.Add("pushOrderAction", 5.ToString());
				Globals.WriteExceptionLog(ex, dictionary, "AppPushRecordForOrder");
			}
		}

		public static void AppPushRecordForJob()
		{
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			IList<AppPushRecordInfo> needPushSendRecords = appPushMsgDao.GetNeedPushSendRecords();
			needPushSendRecords.ForEach(delegate(AppPushRecordInfo c)
			{
				VShopHelper.SendMessage(c, true, false);
			});
		}

		public static void AppPushStoreStockForJob()
		{
			IList<StoresInfo> allStores = StoresHelper.GetAllStores();
			foreach (StoresInfo item in allStores)
			{
				if (!StoresHelper.StoreStockIsEnough(item.StoreId))
				{
					VShopHelper.AppPsuhRecordForStore(item.StoreId, "", "", EnumPushStoreAction.StoreStockWarning);
				}
			}
		}

		public static AppPushRecordInfo GetAppPushRecordInfo(int pushRecordId)
		{
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			return appPushMsgDao.Get<AppPushRecordInfo>(pushRecordId);
		}

		public static void AppPushRecordSendAboutAtOnce(AppPushRecordInfo appPushRecordInfo)
		{
			if (appPushRecordInfo.PushSendType == 1)
			{
				VShopHelper.SendMessage(appPushRecordInfo, true, false);
			}
		}

		private static void SetPushAndorid(PushPayload pushPayload, AppPushRecordInfo appPushRecordInfo, bool pushMessage = false)
		{
			AndroidNotification androidNotification = new AndroidNotification();
			pushPayload.notification.AndroidNotification = androidNotification;
			androidNotification.setTitle(appPushRecordInfo.PushTitle);
			androidNotification.setAlert(appPushRecordInfo.PushContent);
			if (!string.IsNullOrEmpty(appPushRecordInfo.Extras))
			{
				string[] array = appPushRecordInfo.Extras.Split(VShopHelper.SEPARATOREVERY);
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array3 = text.Split(VShopHelper.SEPARATORCONTEXT);
						if (array3.Length != 0 && array3.Length > 1)
						{
							androidNotification.AddExtra(array3[0], array3[1]);
						}
					}
				}
			}
			if (pushMessage)
			{
				pushPayload.message = cn.jpush.api.push.mode.Message.content(appPushRecordInfo.PushContent).setTitle(appPushRecordInfo.PushTitle);
				if (!string.IsNullOrEmpty(appPushRecordInfo.Extras))
				{
					string[] array4 = appPushRecordInfo.Extras.Split(VShopHelper.SEPARATOREVERY);
					string[] array5 = array4;
					foreach (string text2 in array5)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string[] array6 = text2.Split(VShopHelper.SEPARATORCONTEXT);
							if (array6.Length != 0 && array6.Length > 1)
							{
								pushPayload.message = cn.jpush.api.push.mode.Message.content(appPushRecordInfo.PushContent).setTitle(appPushRecordInfo.PushTitle).AddExtras(array6[0], array6[1]);
							}
						}
					}
				}
			}
			VShopHelper.SetPuahPayload(pushPayload, appPushRecordInfo);
		}

		private static void SetPushIOS(PushPayload pushPayload, AppPushRecordInfo appPushRecordInfo, bool pushMessage = false)
		{
			pushPayload.options.apns_production = true;
			IosNotification iosNotification = new IosNotification();
			pushPayload.notification.IosNotification = iosNotification;
			iosNotification.setAlert(appPushRecordInfo.PushContent);
			if (!string.IsNullOrEmpty(appPushRecordInfo.Extras))
			{
				string[] array = appPushRecordInfo.Extras.Split(VShopHelper.SEPARATOREVERY);
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array3 = text.Split(VShopHelper.SEPARATORCONTEXT);
						if (array3.Length != 0 && array3.Length > 1)
						{
							iosNotification.AddExtra(array3[0], array3[1]);
						}
					}
				}
				iosNotification.AddExtra("subtitle", appPushRecordInfo.PushTitle);
			}
			if (pushMessage)
			{
				pushPayload.message = cn.jpush.api.push.mode.Message.content(appPushRecordInfo.PushContent).setTitle(appPushRecordInfo.PushTitle);
				if (!string.IsNullOrEmpty(appPushRecordInfo.Extras))
				{
					string[] array4 = appPushRecordInfo.Extras.Split(VShopHelper.SEPARATOREVERY);
					string[] array5 = array4;
					foreach (string text2 in array5)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string[] array6 = text2.Split(VShopHelper.SEPARATORCONTEXT);
							if (array6.Length != 0 && array6.Length > 1)
							{
								pushPayload.message = cn.jpush.api.push.mode.Message.content(appPushRecordInfo.PushContent).setTitle(appPushRecordInfo.PushTitle).AddExtras(array6[0], array6[1]);
							}
						}
					}
				}
			}
			VShopHelper.SetPuahPayload(pushPayload, appPushRecordInfo);
		}

		private static void SetPushWinPhone(PushPayload pushPayload, AppPushRecordInfo appPushRecordInfo, bool pushMessage = false)
		{
			WinphoneNotification winphoneNotification = new WinphoneNotification();
			pushPayload.notification.WinphoneNotification = winphoneNotification;
			winphoneNotification.setTitle(appPushRecordInfo.PushTitle);
			winphoneNotification.setAlert(appPushRecordInfo.PushContent);
			if (!string.IsNullOrEmpty(appPushRecordInfo.Extras))
			{
				string[] array = appPushRecordInfo.Extras.Split(VShopHelper.SEPARATOREVERY);
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array3 = text.Split(VShopHelper.SEPARATORCONTEXT);
						if (array3.Length != 0 && array3.Length > 1)
						{
							winphoneNotification.AddExtra(array3[0], array3[1]);
						}
					}
				}
			}
			if (pushMessage)
			{
				pushPayload.message = cn.jpush.api.push.mode.Message.content(appPushRecordInfo.PushContent).setTitle(appPushRecordInfo.PushTitle);
				if (!string.IsNullOrEmpty(appPushRecordInfo.Extras))
				{
					string[] array4 = appPushRecordInfo.Extras.Split(VShopHelper.SEPARATOREVERY);
					string[] array5 = array4;
					foreach (string text2 in array5)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							string[] array6 = text2.Split(VShopHelper.SEPARATORCONTEXT);
							if (array6.Length != 0 && array6.Length > 1)
							{
								pushPayload.message = cn.jpush.api.push.mode.Message.content(appPushRecordInfo.PushContent).setTitle(appPushRecordInfo.PushTitle).AddExtras(array6[0], array6[1]);
							}
						}
					}
				}
			}
			VShopHelper.SetPuahPayload(pushPayload, appPushRecordInfo);
		}

		private static void SetPuahPayload(PushPayload pushPayload, AppPushRecordInfo appPushRecordInfo)
		{
			if (appPushRecordInfo.ToAll)
			{
				pushPayload.audience = Audience.all();
			}
			else
			{
				int? userId = appPushRecordInfo.UserId;
				if (userId.HasValue)
				{
					List<string> list = new List<string>();
					List<string> list2 = list;
					userId = appPushRecordInfo.UserId;
					list2.Add(userId.ToString());
					pushPayload.audience = Audience.s_alias(list.ToArray());
				}
				else if (!string.IsNullOrEmpty(appPushRecordInfo.SendUserIds))
				{
					List<string> list3 = new List<string>();
					string[] array = appPushRecordInfo.SendUserIds.Split(',');
					foreach (string item in array)
					{
						list3.Add(item);
					}
					pushPayload.audience = Audience.s_alias(list3.ToArray());
				}
				else if (!string.IsNullOrEmpty(appPushRecordInfo.PushTag))
				{
					List<string> list4 = new List<string>();
					list4.Add(appPushRecordInfo.PushTag);
					pushPayload.audience = Audience.s_tag_and(list4.ToArray());
				}
			}
		}

		public static AppPushRecordInfo SendAgainAppPushRecord(int pushRecordId)
		{
			AppPushRecordInfo appPushRecordInfo = VShopHelper.GetAppPushRecordInfo(pushRecordId);
			if (appPushRecordInfo != null && !appPushRecordInfo.PushStatus.Equals(EnumPushStatus.PushSucceed))
			{
				appPushRecordInfo.PushSendType = 1;
				appPushRecordInfo.PushSendDate = DateTime.Now;
				VShopHelper.SendMessage(appPushRecordInfo, true, false);
				VShopHelper.UpdateAppPushRecord(appPushRecordInfo);
			}
			return appPushRecordInfo;
		}

		public static void StoreAppPushSend(AppPushRecordInfo appPushRecordInfo)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string storeAppPushAppKey = masterSettings.StoreAppPushAppKey;
			string storeAppPushMasterSecret = masterSettings.StoreAppPushMasterSecret;
			if (!string.IsNullOrEmpty(storeAppPushAppKey) && !string.IsNullOrEmpty(storeAppPushMasterSecret))
			{
				JPushClient jPushClient = new JPushClient(storeAppPushAppKey, storeAppPushMasterSecret);
				PushPayload pushPayload = new PushPayload();
				pushPayload.platform = Platform.all();
				pushPayload.notification = new Notification();
				VShopHelper.SetPushAndorid(pushPayload, appPushRecordInfo, true);
				VShopHelper.SetPushIOS(pushPayload, appPushRecordInfo, true);
				VShopHelper.SetPushWinPhone(pushPayload, appPushRecordInfo, true);
				string value = "";
				try
				{
					MessageResult obj = jPushClient.SendPush(pushPayload);
					value = obj.ToNullString();
					appPushRecordInfo.PushSendTime = DateTime.Now;
					appPushRecordInfo.PushStatus = 3;
				}
				catch (APIRequestException ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("Extras", appPushRecordInfo.Extras.ToNullString());
					dictionary.Add("PushContent", appPushRecordInfo.PushContent.ToNullString());
					dictionary.Add("PushMsgType", appPushRecordInfo.PushMsgType.ToNullString());
					dictionary.Add("PushRecordId", appPushRecordInfo.PushRecordId.ToNullString());
					dictionary.Add("PushRemark", appPushRecordInfo.PushRemark.ToNullString());
					dictionary.Add("PushSendDate", appPushRecordInfo.PushSendDate.ToNullString());
					dictionary.Add("PushSendTime", appPushRecordInfo.PushSendTime.ToNullString());
					dictionary.Add("PushSendType", appPushRecordInfo.PushSendType.ToNullString());
					dictionary.Add("PushStatus", appPushRecordInfo.PushStatus.ToNullString());
					dictionary.Add("PushTag", appPushRecordInfo.PushTag.ToNullString());
					dictionary.Add("PushTagText", appPushRecordInfo.PushTagText.ToNullString());
					dictionary.Add("PushTitle", appPushRecordInfo.PushTitle.ToNullString());
					dictionary.Add("PushType", appPushRecordInfo.PushType.ToNullString());
					dictionary.Add("ToAll", appPushRecordInfo.ToAll.ToNullString());
					dictionary.Add("UserId", appPushRecordInfo.UserId.ToNullString());
					if (pushPayload.audience != null)
					{
						dictionary.Add("audience", pushPayload.audience.allAudience);
					}
					dictionary.Add("StackTrace", ex.StackTrace);
					if (ex.InnerException != null)
					{
						dictionary.Add("InnerException", ex.InnerException.ToString());
					}
					if (ex.GetBaseException() != null)
					{
						dictionary.Add("BaseException", ex.GetBaseException().Message);
					}
					if (ex.TargetSite != (MethodBase)null)
					{
						dictionary.Add("TargetSite", ex.TargetSite.ToString());
					}
					dictionary.Add("ExSource", ex.Source);
					dictionary.Add("ErrorMessage", ex.ErrorMessage);
					dictionary.Add("resutStr", value);
					Globals.AppendLog(dictionary, "门店APP消息推送失败", "", "", "AppStorePush");
					appPushRecordInfo.PushStatus = 2;
					appPushRecordInfo.PushRemark = ex.ErrorMessage;
				}
			}
		}

		public static void PresalePushSend(AppPushRecordInfo appPushRecordInfo)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string appPushAppKey = masterSettings.AppPushAppKey;
			string appPushMasterSecret = masterSettings.AppPushMasterSecret;
			if (!string.IsNullOrEmpty(masterSettings.AppPushAppKey))
			{
				JPushClient jPushClient = new JPushClient(appPushAppKey, appPushMasterSecret);
				PushPayload pushPayload = new PushPayload();
				pushPayload.platform = Platform.all();
				pushPayload.notification = new Notification();
				VShopHelper.SetPushAndorid(pushPayload, appPushRecordInfo, false);
				VShopHelper.SetPushIOS(pushPayload, appPushRecordInfo, false);
				VShopHelper.SetPushWinPhone(pushPayload, appPushRecordInfo, false);
				try
				{
					MessageResult messageResult = jPushClient.SendPush(pushPayload);
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("Extras", appPushRecordInfo.Extras.ToNullString());
					dictionary.Add("PushContent", appPushRecordInfo.PushContent.ToNullString());
					dictionary.Add("PushMsgType", appPushRecordInfo.PushMsgType.ToNullString());
					dictionary.Add("PushRecordId", appPushRecordInfo.PushRecordId.ToNullString());
					dictionary.Add("PushRemark", appPushRecordInfo.PushRemark.ToNullString());
					dictionary.Add("PushSendDate", appPushRecordInfo.PushSendDate.ToNullString());
					dictionary.Add("PushSendTime", appPushRecordInfo.PushSendTime.ToNullString());
					dictionary.Add("PushSendType", appPushRecordInfo.PushSendType.ToNullString());
					dictionary.Add("PushStatus", appPushRecordInfo.PushStatus.ToNullString());
					dictionary.Add("PushTag", appPushRecordInfo.PushTag.ToNullString());
					dictionary.Add("PushTagText", appPushRecordInfo.PushTagText.ToNullString());
					dictionary.Add("PushTitle", appPushRecordInfo.PushTitle.ToNullString());
					dictionary.Add("PushType", appPushRecordInfo.PushType.ToNullString());
					dictionary.Add("ToAll", appPushRecordInfo.ToAll.ToNullString());
					dictionary.Add("UserId", appPushRecordInfo.UserId.ToNullString());
					dictionary.Add("msg_id", messageResult.msg_id.ToNullString());
					dictionary.Add("sendno", messageResult.sendno.ToNullString());
					dictionary.Add("isResultOK", messageResult.isResultOK().ToNullString());
					Globals.AppendLog(dictionary, "用户APP消息推送成功", "", "", "AppPushsucess");
				}
				catch (APIRequestException ex)
				{
					IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
					dictionary2.Add("Extras", appPushRecordInfo.Extras.ToNullString());
					dictionary2.Add("PushContent", appPushRecordInfo.PushContent.ToNullString());
					dictionary2.Add("PushMsgType", appPushRecordInfo.PushMsgType.ToNullString());
					dictionary2.Add("PushRecordId", appPushRecordInfo.PushRecordId.ToNullString());
					dictionary2.Add("PushRemark", appPushRecordInfo.PushRemark.ToNullString());
					dictionary2.Add("PushSendDate", appPushRecordInfo.PushSendDate.ToNullString());
					dictionary2.Add("PushSendTime", appPushRecordInfo.PushSendTime.ToNullString());
					dictionary2.Add("PushSendType", appPushRecordInfo.PushSendType.ToNullString());
					dictionary2.Add("PushStatus", appPushRecordInfo.PushStatus.ToNullString());
					dictionary2.Add("PushTag", appPushRecordInfo.PushTag.ToNullString());
					dictionary2.Add("PushTagText", appPushRecordInfo.PushTagText.ToNullString());
					dictionary2.Add("PushTitle", appPushRecordInfo.PushTitle.ToNullString());
					dictionary2.Add("PushType", appPushRecordInfo.PushType.ToNullString());
					dictionary2.Add("ToAll", appPushRecordInfo.ToAll.ToNullString());
					dictionary2.Add("UserId", appPushRecordInfo.UserId.ToNullString());
					if (pushPayload.audience != null)
					{
						dictionary2.Add("audience", pushPayload.audience.allAudience);
					}
					dictionary2.Add("StackTrace", ex.StackTrace);
					if (ex.InnerException != null)
					{
						dictionary2.Add("InnerException", ex.InnerException.ToString());
					}
					if (ex.GetBaseException() != null)
					{
						dictionary2.Add("BaseException", ex.GetBaseException().Message);
					}
					if (ex.TargetSite != (MethodBase)null)
					{
						dictionary2.Add("TargetSite", ex.TargetSite.ToString());
					}
					dictionary2.Add("ExSource", ex.Source);
					dictionary2.Add("ErrorMessage", ex.ErrorMessage);
					Globals.AppendLog(dictionary2, "用户APP消息推送失败", "", "", "AppError");
				}
			}
		}

		public static void AppPushSend(AppPushRecordInfo appPushRecordInfo)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string appPushAppKey = masterSettings.AppPushAppKey;
			string appPushMasterSecret = masterSettings.AppPushMasterSecret;
			JPushClient jPushClient = new JPushClient(appPushAppKey, appPushMasterSecret);
			PushPayload pushPayload = new PushPayload();
			pushPayload.platform = Platform.all();
			pushPayload.notification = new Notification();
			VShopHelper.SetPushAndorid(pushPayload, appPushRecordInfo, false);
			VShopHelper.SetPushIOS(pushPayload, appPushRecordInfo, false);
			VShopHelper.SetPushWinPhone(pushPayload, appPushRecordInfo, false);
			try
			{
				MessageResult messageResult = jPushClient.SendPush(pushPayload);
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Extras", appPushRecordInfo.Extras.ToNullString());
				dictionary.Add("PushContent", appPushRecordInfo.PushContent.ToNullString());
				dictionary.Add("PushMsgType", appPushRecordInfo.PushMsgType.ToNullString());
				dictionary.Add("PushRecordId", appPushRecordInfo.PushRecordId.ToNullString());
				dictionary.Add("PushRemark", appPushRecordInfo.PushRemark.ToNullString());
				dictionary.Add("PushSendDate", appPushRecordInfo.PushSendDate.ToNullString());
				dictionary.Add("PushSendTime", appPushRecordInfo.PushSendTime.ToNullString());
				dictionary.Add("PushSendType", appPushRecordInfo.PushSendType.ToNullString());
				dictionary.Add("PushStatus", appPushRecordInfo.PushStatus.ToNullString());
				dictionary.Add("PushTag", appPushRecordInfo.PushTag.ToNullString());
				dictionary.Add("PushTagText", appPushRecordInfo.PushTagText.ToNullString());
				dictionary.Add("PushTitle", appPushRecordInfo.PushTitle.ToNullString());
				dictionary.Add("PushType", appPushRecordInfo.PushType.ToNullString());
				dictionary.Add("ToAll", appPushRecordInfo.ToAll.ToNullString());
				dictionary.Add("UserId", appPushRecordInfo.UserId.ToNullString());
				dictionary.Add("msg_id", messageResult.msg_id.ToNullString());
				dictionary.Add("sendno", messageResult.sendno.ToNullString());
				dictionary.Add("isResultOK", messageResult.isResultOK().ToNullString());
				appPushRecordInfo.PushSendTime = DateTime.Now;
				appPushRecordInfo.PushStatus = 3;
			}
			catch (APIRequestException ex)
			{
				IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
				dictionary2.Add("Extras", appPushRecordInfo.Extras.ToNullString());
				dictionary2.Add("PushContent", appPushRecordInfo.PushContent.ToNullString());
				dictionary2.Add("PushMsgType", appPushRecordInfo.PushMsgType.ToNullString());
				dictionary2.Add("PushRecordId", appPushRecordInfo.PushRecordId.ToNullString());
				dictionary2.Add("PushRemark", appPushRecordInfo.PushRemark.ToNullString());
				dictionary2.Add("PushSendDate", appPushRecordInfo.PushSendDate.ToNullString());
				dictionary2.Add("PushSendTime", appPushRecordInfo.PushSendTime.ToNullString());
				dictionary2.Add("PushSendType", appPushRecordInfo.PushSendType.ToNullString());
				dictionary2.Add("PushStatus", appPushRecordInfo.PushStatus.ToNullString());
				dictionary2.Add("PushTag", appPushRecordInfo.PushTag.ToNullString());
				dictionary2.Add("PushTagText", appPushRecordInfo.PushTagText.ToNullString());
				dictionary2.Add("PushTitle", appPushRecordInfo.PushTitle.ToNullString());
				dictionary2.Add("PushType", appPushRecordInfo.PushType.ToNullString());
				dictionary2.Add("ToAll", appPushRecordInfo.ToAll.ToNullString());
				dictionary2.Add("UserId", appPushRecordInfo.UserId.ToNullString());
				if (pushPayload.audience != null)
				{
					dictionary2.Add("audience", pushPayload.audience.allAudience);
				}
				dictionary2.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					dictionary2.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					dictionary2.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					dictionary2.Add("TargetSite", ex.TargetSite.ToString());
				}
				dictionary2.Add("ExSource", ex.Source);
				dictionary2.Add("ErrorMessage", ex.ErrorMessage);
				Globals.AppendLog(dictionary2, "用户APP消息推送失败", "", "", "AppPush");
				appPushRecordInfo.PushStatus = 2;
				appPushRecordInfo.PushRemark = ex.ErrorMessage;
				appPushRecordInfo.PushStatus = 2;
				appPushRecordInfo.PushRemark = ex.ErrorMessage;
			}
			new AppPushMsgDao().Update(appPushRecordInfo, null);
		}

		public static void SendMessage(AppPushRecordInfo appPushRecordInfo, bool isUpdate = true, bool isStoreApp = false)
		{
			string pushTitle = appPushRecordInfo.PushTitle;
			string pushContent = appPushRecordInfo.PushContent;
			string extras = appPushRecordInfo.Extras;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = "";
			string text2 = "";
			string text3 = "";
			if (isStoreApp)
			{
				text = masterSettings.StoreAppPushAppId;
				text2 = masterSettings.StoreAppPushAppKey;
				text3 = masterSettings.StoreAppPushMasterSecret;
			}
			else
			{
				text = masterSettings.AppPushAppId;
				text2 = masterSettings.AppPushAppKey;
				text3 = masterSettings.AppPushMasterSecret;
			}
			if (appPushRecordInfo.ToAll)
			{
				VShopHelper.PushToAll(text, text2, text3, appPushRecordInfo);
			}
			else
			{
				IList<MemberClientTokenInfo> list = null;
				list = ((!isStoreApp) ? MemberHelper.GetClientIdAndTokenByUserId(appPushRecordInfo.UserId.HasValue ? appPushRecordInfo.UserId.Value : 0, appPushRecordInfo.SendUserIds, appPushRecordInfo.PushTag) : DepotHelper.GetClientIdAndTokenByStoreId(appPushRecordInfo.UserId.HasValue ? appPushRecordInfo.UserId.Value : 0, appPushRecordInfo.SendUserIds));
				List<string> list2 = (from i in list
				where (string.IsNullOrWhiteSpace(i.Token) || (!string.IsNullOrWhiteSpace(i.Token) && i.Token.Length < 64)) && !string.IsNullOrWhiteSpace(i.ClientId)
				select i.ClientId).Distinct().ToList();
				List<string> list3 = (from i in list
				where !string.IsNullOrWhiteSpace(i.Token) && i.Token.Length >= 64
				select i.Token).Distinct().ToList();
				Globals.WriteLog("APPIGetUI.txt", string.Join(",", list2) + string.Join(",", list3) + ";title:" + appPushRecordInfo.PushTitle + ";text:" + appPushRecordInfo.PushContent + ";transmissionContent:" + appPushRecordInfo.Extras + ";\r\t");
				VShopHelper.PushMessageToList(text, text2, text3, list2, appPushRecordInfo);
				if (list3.Count > 0)
				{
					VShopHelper.ApnPush(text, text2, text3, list3, appPushRecordInfo);
				}
			}
			if (isUpdate)
			{
				new AppPushMsgDao().Update(appPushRecordInfo, null);
			}
		}

		private static void PushToAll(string appId, string appKey, string masterSecret, AppPushRecordInfo appPushRecordInfo)
		{
			DateTime now;
			try
			{
				IGtPush gtPush = new IGtPush(VShopHelper.HOST, appKey, masterSecret);
				AppMessage appMessage = new AppMessage();
				TransmissionTemplate data = VShopHelper.TransmissionTemplateToAll(appId, appKey, appPushRecordInfo.PushTitle, appPushRecordInfo.PushContent, appPushRecordInfo.Extras);
				appMessage.IsOffline = true;
				appMessage.OfflineExpireTime = 43200000L;
				appMessage.Data = data;
				List<string> list = new List<string>();
				list.Add(appId);
				appMessage.AppIdList = list;
				string str = gtPush.pushMessageToApp(appMessage);
				now = DateTime.Now;
				Globals.WriteLog("apnPush.txt", now.ToString() + ":" + str + "\r\t");
				appPushRecordInfo.PushSendTime = DateTime.Now;
				appPushRecordInfo.PushStatus = 3;
			}
			catch (Exception ex)
			{
				appPushRecordInfo.PushStatus = 2;
				appPushRecordInfo.PushRemark = "推送失败";
				string[] obj = new string[5];
				now = DateTime.Now;
				obj[0] = now.ToString();
				obj[1] = ":";
				obj[2] = ex.Message;
				obj[3] = ex.StackTrace;
				obj[4] = "\r\t";
				Globals.WriteLog("apnPush.txt", string.Concat(obj));
			}
		}

		public static TransmissionTemplate TransmissionTemplateToAll(string appId, string appKey, string title, string text, string transmissionContent)
		{
			TransmissionTemplate transmissionTemplate = new TransmissionTemplate();
			transmissionTemplate.AppId = appId;
			transmissionTemplate.AppKey = appKey;
			transmissionTemplate.TransmissionType = "2";
			transmissionTemplate.TransmissionContent = VShopHelper.TransmissionContentJson(transmissionContent, title, text);
			APNPayload aPNPayload = new APNPayload();
			DictionaryAlertMsg dictionaryAlertMsg = new DictionaryAlertMsg();
			dictionaryAlertMsg.Body = text;
			dictionaryAlertMsg.Title = title;
			aPNPayload.AlertMsg = dictionaryAlertMsg;
			aPNPayload.ContentAvailable = 1;
			aPNPayload.addCustomMsg("TransmissionContent", VShopHelper.TransmissionContentJson(transmissionContent, title, text));
			transmissionTemplate.setAPNInfo(aPNPayload);
			return transmissionTemplate;
		}

		public static TransmissionTemplate TransmissionTemplateDemo(string appId, string appKey, string transmissionContent, string title, string text)
		{
			TransmissionTemplate transmissionTemplate = new TransmissionTemplate();
			transmissionTemplate.AppId = appId;
			transmissionTemplate.AppKey = appKey;
			transmissionTemplate.TransmissionType = "2";
			transmissionTemplate.TransmissionContent = VShopHelper.TransmissionContentJson(transmissionContent, title, text);
			return transmissionTemplate;
		}

		public static string TransmissionContentJson(string content, string pushTitle, string pushContent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("\"PushTitle\":\"{0}\",", pushTitle);
			stringBuilder.AppendFormat("\"PushContent\":\"{0}\",", pushContent);
			if (!string.IsNullOrEmpty(content))
			{
				string[] array = content.Split(VShopHelper.SEPARATOREVERY);
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array3 = text.Split(VShopHelper.SEPARATORCONTEXT);
						if (array3.Length != 0 && array3.Length > 1)
						{
							stringBuilder.AppendFormat("\"{0}\":\"{1}\",", array3[0], array3[1]);
						}
					}
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
			}
			return "{" + stringBuilder.ToString() + "}";
		}

		private static void ApnPush(string appId, string appKey, string MASTERSECRET, List<string> devicetokenlist, AppPushRecordInfo appPushRecordInfo)
		{
			DateTime now;
			try
			{
				IGtPush gtPush = new IGtPush(VShopHelper.HOST, appKey, MASTERSECRET);
				APNTemplate aPNTemplate = new APNTemplate();
				APNPayload aPNPayload = new APNPayload();
				DictionaryAlertMsg dictionaryAlertMsg = new DictionaryAlertMsg();
				dictionaryAlertMsg.Body = appPushRecordInfo.PushContent;
				dictionaryAlertMsg.Title = appPushRecordInfo.PushTitle;
				aPNPayload.AlertMsg = dictionaryAlertMsg;
				aPNPayload.ContentAvailable = 1;
				aPNPayload.addCustomMsg("TransmissionContent", VShopHelper.TransmissionContentJson(appPushRecordInfo.Extras, appPushRecordInfo.PushTitle, appPushRecordInfo.PushContent));
				aPNTemplate.setAPNInfo(aPNPayload);
				ListMessage listMessage = new ListMessage();
				listMessage.Data = aPNTemplate;
				string aPNContentId = gtPush.getAPNContentId(appId, listMessage);
				string text = gtPush.pushAPNMessageToList(appId, aPNContentId, devicetokenlist);
				appPushRecordInfo.PushSendTime = DateTime.Now;
				appPushRecordInfo.PushStatus = 3;
				Console.Out.WriteLine(text);
				now = DateTime.Now;
				Globals.WriteLog("apnPush.txt", now.ToString() + ":" + text + "\r\t");
			}
			catch (Exception ex)
			{
				appPushRecordInfo.PushStatus = 2;
				appPushRecordInfo.PushRemark = "推送失败";
				string[] obj = new string[5];
				now = DateTime.Now;
				obj[0] = now.ToString();
				obj[1] = ":";
				obj[2] = ex.Message;
				obj[3] = ex.StackTrace;
				obj[4] = "\r\t";
				Globals.WriteLog("apnPush.txt", string.Concat(obj));
			}
		}

		private static void PushMessageToList(string appId, string appKey, string MASTERSECRET, List<string> clientList, AppPushRecordInfo appPushRecordInfo)
		{
			DateTime now;
			try
			{
				IGtPush gtPush = new IGtPush(VShopHelper.HOST, appKey, MASTERSECRET);
				ListMessage listMessage = new ListMessage();
				TransmissionTemplate data = VShopHelper.TransmissionTemplateDemo(appId, appKey, appPushRecordInfo.Extras, appPushRecordInfo.PushTitle, appPushRecordInfo.PushContent);
				listMessage.IsOffline = true;
				listMessage.OfflineExpireTime = 43200000L;
				listMessage.Data = data;
				listMessage.PushNetWorkType = 0;
				List<Target> list = new List<Target>();
				Target target = new Target();
				foreach (string client in clientList)
				{
					if (!string.IsNullOrEmpty(client))
					{
						target = new Target();
						target.appId = appId;
						target.clientId = client;
						list.Add(target);
					}
				}
				string contentId = gtPush.getContentId(listMessage);
				string str = gtPush.pushMessageToList(contentId, list);
				appPushRecordInfo.PushSendTime = DateTime.Now;
				appPushRecordInfo.PushStatus = 3;
				now = DateTime.Now;
				Globals.WriteLog("APPIGetUI.txt", now.ToString() + ":" + str + "\r\t");
			}
			catch (Exception ex)
			{
				appPushRecordInfo.PushStatus = 2;
				appPushRecordInfo.PushRemark = "推送失败";
				string[] obj = new string[5];
				now = DateTime.Now;
				obj[0] = now.ToString();
				obj[1] = ":";
				obj[2] = ex.Message;
				obj[3] = ex.StackTrace;
				obj[4] = "\r\t";
				Globals.WriteLog("APPIGetUI.txt", string.Concat(obj));
			}
		}

		private static string FullImageUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return "";
			}
			return url.StartsWith("http://") ? url : Globals.FullPath(url);
		}

		public static bool CheckAppPushRecordDuplicate(AppPushRecordInfo appPushRecordInfo)
		{
			return new AppPushMsgDao().IsAppPushRecordDuplicate(appPushRecordInfo);
		}

		public static void AddAppPushRecord(AppPushRecordInfo appPushRecordInfo)
		{
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			appPushMsgDao.Add(appPushRecordInfo, null);
		}

		public static void UpdateAppPushRecord(AppPushRecordInfo appPushRecordInfo)
		{
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			AppPushRecordInfo appPushRecordInfo2 = appPushMsgDao.Get<AppPushRecordInfo>(appPushRecordInfo.PushRecordId);
			if (appPushRecordInfo2 != null)
			{
				appPushRecordInfo2.PushContent = appPushRecordInfo.PushContent;
				appPushRecordInfo2.PushSendDate = appPushRecordInfo.PushSendDate;
				appPushRecordInfo2.PushSendTime = appPushRecordInfo.PushSendTime;
				appPushRecordInfo2.PushSendType = appPushRecordInfo.PushSendType;
				appPushRecordInfo2.PushStatus = appPushRecordInfo.PushStatus;
				appPushRecordInfo2.PushTag = appPushRecordInfo.PushTag;
				appPushRecordInfo2.PushTitle = appPushRecordInfo.PushTitle;
				appPushRecordInfo2.PushType = appPushRecordInfo.PushType;
				appPushRecordInfo2.PushRemark = appPushRecordInfo.PushRemark;
				appPushRecordInfo2.Extras = appPushRecordInfo.Extras;
				appPushRecordInfo2.ToAll = appPushRecordInfo.ToAll;
				appPushMsgDao.Update(appPushRecordInfo2, null);
			}
		}

		public static PageModel<AppPushRecordInfo> GetAppPushRecords(AppPushRecordQuery query)
		{
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			return appPushMsgDao.GetAppPushRecords(query);
		}

		public static void DeleteAppPushRecord(int pushRecordId)
		{
			AppPushMsgDao appPushMsgDao = new AppPushMsgDao();
			appPushMsgDao.DeleteAppPushRecord(pushRecordId);
		}

		public static DataTable GetHomeProducts()
		{
			return new HomeProductDao().GetHomeProducts(0);
		}

		public static DbQueryResult GetHomeProducts(ProductQuery query)
		{
			return new HomeProductDao().GetHomeProducts(query);
		}

		public static bool AddHomeProdcut(HomeProductInfo info)
		{
			return new HomeProductDao().AddHomeProdcut(info);
		}

		public static bool RemoveHomeProduct(int productId)
		{
			return new HomeProductDao().RemoveHomeProduct(productId);
		}

		public static bool RemoveAllHomeProduct(ClientType client)
		{
			return new HomeProductDao().RemoveAllHomeProduct(client);
		}

		public static bool UpdateHomeProductSequence(HomeProductInfo info)
		{
			return new HomeProductDao().Update(info, null);
		}

		public static bool SaveActivity(VActivityInfo activity)
		{
			int activityId = new ActivityDao().SaveActivity(activity);
			ReplyInfo replyInfo = new TextReplyInfo();
			replyInfo.Keys = activity.Keys;
			replyInfo.MatchType = MatchType.Equal;
			replyInfo.MessageType = MessageType.Text;
			replyInfo.ReplyType = ReplyType.SignUp;
			replyInfo.ActivityId = activityId;
			return new ReplyDao().SaveReply(replyInfo);
		}

		public static bool UpdateActivity(VActivityInfo activity)
		{
			return new ActivityDao().UpdateActivity(activity);
		}

		public static bool DeleteActivity(int activityId)
		{
			return new ActivityDao().DeleteActivity(activityId);
		}

		public static VActivityInfo GetActivity(int activityId)
		{
			return new ActivityDao().GetActivity(activityId);
		}

		public static IList<VActivityInfo> GetAllActivity()
		{
			return new ActivityDao().GetAllActivity();
		}

		public static DbQueryResult GetSignUpActivityList()
		{
			return new ActivityDao().GetActivitys();
		}

		public static IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
		{
			return new ActivitySignUpDao().GetActivitySignUpById(activityId);
		}

		public static int SaveLotteryTicket(LotteryTicketInfo info)
		{
			string text2 = info.PrizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
			return (int)new LotteryActivityDao().Add(info, null);
		}

		public static bool UpdateLotteryTicket(LotteryTicketInfo info)
		{
			string text2 = info.PrizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
			return new LotteryActivityDao().UpdateLotteryTicket(info);
		}

		public static bool DelteLotteryTicket(int activityId)
		{
			return new LotteryActivityDao().DelteLotteryTicket(activityId);
		}

		public static LotteryTicketInfo GetLotteryTicket(int activityid)
		{
			LotteryTicketInfo lotteryTicketInfo = new LotteryActivityDao().Get<LotteryTicketInfo>(activityid);
			lotteryTicketInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryTicketInfo.PrizeSetting);
			return lotteryTicketInfo;
		}

		public static DbQueryResult GetLotteryTicketList(LotteryActivityQuery page)
		{
			return new LotteryActivityDao().GetLotteryTicketList(page);
		}

		public static TopicInfo Gettopic(int topicId)
		{
			return new TopicDao().GetTopic(topicId);
		}

		public static DbQueryResult GettopicList(TopicQuery page)
		{
			return new TopicDao().GetTopicList(page);
		}

		public static bool SetHomePage(int topicId)
		{
			return new TopicDao().SetHomePage(topicId);
		}

		public static bool CancelHomePage(int topicId)
		{
			return new TopicDao().CancelHomePage(topicId);
		}

		public static IList<TopicInfo> GetAppTopics()
		{
			return new TopicDao().GetAppTopics();
		}

		public static IList<TopicInfo> Gettopics()
		{
			return new TopicDao().Gettopics();
		}

		public static IList<TopicInfo> GetPcTopics()
		{
			return new TopicDao().GetPcTopics();
		}

		public static int Deletetopics(IList<int> topics)
		{
			if (topics == null || topics.Count == 0)
			{
				return 0;
			}
			int num = 0;
			TopicDao topicDao = new TopicDao();
			foreach (int topic in topics)
			{
				if (topicDao.Delete<TopicInfo>(topic))
				{
					num++;
				}
			}
			return num;
		}

		public static bool Createtopic(TopicInfo topic, out int id)
		{
			id = 0;
			if (topic == null)
			{
				return false;
			}
			Globals.EntityCoding(topic, true);
			id = (int)new TopicDao().Add(topic, null);
			return id > 0;
		}

		public static bool Updatetopic(TopicInfo topic)
		{
			if (topic == null)
			{
				return false;
			}
			Globals.EntityCoding(topic, true);
			return new TopicDao().UpdateTopic(topic);
		}

		public static bool Deletetopic(int topicId)
		{
			return new TopicDao().DeleteTopic(topicId);
		}

		public static bool SwapTopicSequence(int topicid, int displaysequence)
		{
			return new TopicDao().SaveSequence<TopicInfo>(topicid, displaysequence, null);
		}

		public static IList<MenuInfo> GetMenus(ClientType clientType)
		{
			IList<MenuInfo> list = new List<MenuInfo>();
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			if (topMenus == null)
			{
				return list;
			}
			foreach (MenuInfo item in topMenus)
			{
				list.Add(item);
				IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(item.MenuId, clientType);
				if (menusByParentId != null)
				{
					foreach (MenuInfo item2 in menusByParentId)
					{
						list.Add(item2);
					}
				}
			}
			return list;
		}

		public static IList<MenuInfo> GetMenusByParentId(int parentId, ClientType clientType)
		{
			return new MenuDao().GetMenusByParentId(parentId, clientType);
		}

		public static MenuInfo GetMenu(int menuId)
		{
			return new MenuDao().Get<MenuInfo>(menuId);
		}

		public static IList<MenuInfo> GetTopMenus(ClientType clientType)
		{
			return new MenuDao().GetTopMenus(clientType);
		}

		public static bool CanAddMenu(int parentId, ClientType clientType)
		{
			int num = 3;
			int num2 = 5;
			if (clientType == ClientType.AliOH)
			{
				num = 3;
				num2 = 5;
			}
			IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId, clientType);
			if (menusByParentId == null || menusByParentId.Count == 0)
			{
				return true;
			}
			if (parentId == 0)
			{
				return menusByParentId.Count < num;
			}
			return menusByParentId.Count < num2;
		}

		public static bool UpdateMenu(MenuInfo menu)
		{
			return new MenuDao().Update(menu, null);
		}

		public static bool SaveMenu(MenuInfo menu)
		{
			MenuDao menuDao = new MenuDao();
			menu.DisplaySequence = menuDao.GetMaxDisplaySequence<MenuInfo>();
			return menuDao.Add(menu, null) > 0;
		}

		public static bool DeleteMenu(int menuId)
		{
			return new MenuDao().Delete<MenuInfo>(menuId);
		}

		public static void SwapMenuSequence(int menuId, int displaySequence)
		{
			new MenuDao().SaveSequence<MenuInfo>(menuId, displaySequence, null);
		}

		public static IList<MenuInfo> GetInitMenus(ClientType clientType)
		{
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(clientType);
			foreach (MenuInfo item in topMenus)
			{
				item.Chilren = menuDao.GetMenusByParentId(item.MenuId, clientType);
				if (item.Chilren == null)
				{
					item.Chilren = new List<MenuInfo>();
				}
			}
			return topMenus;
		}

		public static bool AddWXReferral(string openId, int referralUserId)
		{
			MemberWXReferralInfo memberWXReferralInfo = new MemberWXReferralInfo();
			memberWXReferralInfo.OpenId = openId;
			memberWXReferralInfo.ReferralUserId = referralUserId;
			return new ReferralDao().Add(memberWXReferralInfo, null) > 0;
		}

		public static bool UpdateWXReferral(string openId, int referralUserId)
		{
			ReferralDao referralDao = new ReferralDao();
			MemberWXReferralInfo memberWXReferralInfoByOpenId = referralDao.GetMemberWXReferralInfoByOpenId(openId);
			if (memberWXReferralInfoByOpenId == null)
			{
				return false;
			}
			memberWXReferralInfoByOpenId.ReferralUserId = referralUserId;
			return referralDao.Update(memberWXReferralInfoByOpenId, null);
		}

		public static bool DeleteWXReferral(string openId)
		{
			return new ReferralDao().MemberWXReferralInfoByOpenId(openId);
		}

		public static MemberWXReferralInfo GetWXReferral(string openId)
		{
			return new ReferralDao().GetMemberWXReferralInfoByOpenId(openId);
		}

		public static string UploadVipBGImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/Vipcard/vipbg" + Path.GetExtension(postedFile.FileName);
			postedFile.SaveAs(HttpContext.Current.Request.MapPath(text));
			return text;
		}

		public static string UploadDefautBg(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetCommonSkinPath() + "/images/ad/DefautPageBg" + Path.GetExtension(postedFile.FileName);
			postedFile.SaveAs(HttpContext.Current.Request.MapPath(text));
			return text;
		}

		public static string UploadWeiXinCodeImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/WeiXinCodeImageUrl" + Path.GetExtension(postedFile.FileName);
			postedFile.SaveAs(HttpContext.Current.Request.MapPath(text));
			return text;
		}

		public static string UploadVipQRImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/Vipcard/vipqr" + Path.GetExtension(postedFile.FileName);
			postedFile.SaveAs(HttpContext.Current.Request.MapPath(text));
			return text;
		}

		public static string UploadTopicImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/topic/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HttpContext.Current.Request.MapPath(text));
			return text;
		}

		public static int InsertLotteryActivity(LotteryActivityInfo info)
		{
			string text2 = info.PrizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
			return (int)new LotteryActivityDao().Add(info, null);
		}

		public static IList<LotteryActivityInfo> GetLotteryActivityByType(LotteryActivityType type)
		{
			return new LotteryActivityDao().GetLotteryActivityByType(type);
		}

		public static bool UpdateLotteryActivity(LotteryActivityInfo info)
		{
			string text2 = info.PrizeSetting = JsonConvert.SerializeObject(info.PrizeSettingList);
			return new LotteryActivityDao().UpdateLotteryActivity(info);
		}

		public static bool DeleteLotteryActivity(int activityid, string type = "")
		{
			return new LotteryActivityDao().DelteLotteryActivity(activityid, type);
		}

		public static LotteryActivityInfo GetLotteryActivityInfo(int activityid)
		{
			LotteryActivityInfo lotteryActivityInfo = new LotteryActivityDao().Get<LotteryActivityInfo>(activityid);
			lotteryActivityInfo.PrizeSettingList = JsonConvert.DeserializeObject<List<PrizeSetting>>(lotteryActivityInfo.PrizeSetting);
			return lotteryActivityInfo;
		}

		public static DbQueryResult GetLotteryActivityList(LotteryActivityQuery page)
		{
			return new LotteryActivityDao().GetLotteryActivityList(page);
		}

		public static List<PrizeRecordInfo> GetPrizeList(PrizeQuery page)
		{
			return new LotteryActivityDao().GetPrizeList(page);
		}

		public static void SetFightGroupSuccess(int fightGroupId)
		{
			DateTime now = DateTime.Now;
			int fightGroupActiveNumber = VShopHelper.GetFightGroupActiveNumber(fightGroupId);
			FightGroupDao fightGroupDao = new FightGroupDao();
			if (fightGroupDao.UpdateFightOrderSuccess(fightGroupId, now, fightGroupActiveNumber))
			{
				FightGroupInfo info = fightGroupDao.Get<FightGroupInfo>(fightGroupId);
				List<OrderInfo> list = (from o in VShopHelper.GetFightGroupOrdersJustShowPay(fightGroupId)
				orderby o.IsFightGroupHead descending
				select o).ToList();
				Hidistro.Entities.Members.MemberInfo headuser = null;
				for (int i = 0; i < list.Count; i++)
				{
					OrderInfo orderInfo = list[i];
					if (i == 0)
					{
						headuser = Users.GetUser(orderInfo.UserId);
					}
					Hidistro.Entities.Members.MemberInfo user = Users.GetUser(orderInfo.UserId);
					Messenger.FightGroupOrderSuccess(user, orderInfo, headuser, info);
				}
			}
		}

		public static void DealFightGroupFail(int fightGroupId)
		{
			int num = VShopHelper.SetFightGroupFail(fightGroupId);
			IList<OrderInfo> fightGroupOrders = VShopHelper.GetFightGroupOrders(fightGroupId);
			foreach (OrderInfo item in fightGroupOrders)
			{
				int refundType = 3;
				string gateway = item.Gateway;
				string orderId = item.OrderId;
				string gatewayOrderId = item.GatewayOrderId;
				decimal total = item.GetTotal(false);
				int storeId = item.StoreId;
				int allQuantity = item.GetAllQuantity(true);
				string productInfo = "";
				using (Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator2 = item.LineItems.Values.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						LineItemInfo current2 = enumerator2.Current;
						productInfo = current2.ItemDescription;
					}
				}
				switch (item.OrderStatus)
				{
				case OrderStatus.WaitBuyerPay:
					TradeHelper.CloseOrder(orderId, "火拼团订单在成团时间内未付款");
					break;
				case OrderStatus.BuyerAlreadyPaid:
				{
					if (gateway == "hishop.plugins.payment.advancerequest" || item.BalanceAmount > decimal.Zero)
					{
						refundType = 1;
					}
					RefundInfo refundInfo = new RefundInfo();
					refundInfo.OrderId = orderId;
					refundInfo.RefundReason = "拼团失败，自动退款";
					refundInfo.RefundType = (RefundTypes)refundType;
					refundInfo.RefundOrderId = gatewayOrderId;
					refundInfo.RefundGateWay = (string.IsNullOrEmpty(gateway) ? "" : gateway.ToLower().Replace(".payment.", ".refund."));
					refundInfo.StoreId = storeId;
					refundInfo.AdminRemark = "拼团失败，自动退款";
					refundInfo.ApplyForTime = DateTime.Now;
					refundInfo.UserRemark = "";
					refundInfo.RefundAmount = item.GetTotal(false);
					TradeHelper.ApplyForRefund(refundInfo);
					Hidistro.Entities.Members.MemberInfo user = Users.GetUser(item.UserId);
					string fightGroupInfo = string.Format("{0}人团{1}元", num, total.F2ToString("f2"));
					Messenger.FightGroupOrderFail(user, productInfo, fightGroupInfo, item.OrderId);
					break;
				}
				}
			}
		}

		public static int SetFightGroupFail(int fightGroupId)
		{
			return new FightGroupDao().UpdateFightOrderFaile(fightGroupId);
		}

		public static void CloseOrderToReduceFightGroup(int fightGroupId, string skuId, int quantity)
		{
			new FightGroupDao().CloseOrderToReduceFightGroup(fightGroupId, skuId, quantity);
		}

		public static DataTable GetCountDownSkus(int countDownId)
		{
			return new CountDownDao().GetCountDownSkus(countDownId, 0, false);
		}

		public static FightGroupSkuInfo GetFightGroupSku(int fightGroupActivityId, string skuId)
		{
			return new FightGroupDao().GetFightGroupSku(fightGroupActivityId, skuId);
		}

		public static FightGroupActivityInfo CheckUserFightGroup(int productId, int fightGroupActivityId, int fightGroupId, string skuId, int userId, int buyAmount, string orderId, int quantity, out string msg)
		{
			List<int> list = new List<int>();
			list.Add(4);
			msg = "";
			bool flag = false;
			bool flag2 = false;
			FightGroupDao fightGroupDao = new FightGroupDao();
			FightGroupActivityInfo fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(fightGroupActivityId);
			FightGroupInfo fightGroup = VShopHelper.GetFightGroup(fightGroupId);
			if (fightGroupActivitieInfo == null && fightGroup != null)
			{
				fightGroupActivitieInfo = VShopHelper.GetFightGroupActivitieInfo(fightGroup.FightGroupActivityId);
				fightGroupActivityId = fightGroup.FightGroupActivityId;
			}
			FightGroupSkuInfo fightGroupSku = VShopHelper.GetFightGroupSku(fightGroupActivityId, skuId);
			SKUItem skuItem = new SkuDao().GetSkuItem(skuId, 0);
			flag2 = fightGroupDao.IsOverMaxCountFightGroup(fightGroupActivityId, userId, orderId, list, quantity);
			if (fightGroup != null)
			{
				flag = fightGroupDao.IsDuplicateBuyGroup(fightGroup.FightGroupId, userId, orderId, list);
			}
			bool flag3 = new OrderDao().ExistsOrder(orderId);
			ProductInfo simpleProductDetail = new ProductDao().GetSimpleProductDetail(productId);
			if (simpleProductDetail == null)
			{
				msg = "拼团商品不存在，请选择其他拼团活动！";
				return null;
			}
			if (!simpleProductDetail.SaleStatus.Equals(ProductSaleStatus.OnSale))
			{
				msg = "拼团商品已下架，请选择其他拼团活动！";
				return null;
			}
			if (fightGroupActivitieInfo == null)
			{
				msg = "拼团活动不存在，请选择其他拼团活动！";
				return null;
			}
			if (fightGroup == null)
			{
				if (fightGroupActivitieInfo.StartDate > DateTime.Now)
				{
					msg = "拼团活动还未开始，请选择其他拼团活动！";
					return null;
				}
				if (fightGroupActivitieInfo.EndDate < DateTime.Now)
				{
					msg = "拼团活动已经结束，请选择其他拼团活动！";
					return null;
				}
			}
			else
			{
				if (fightGroup.StartTime > DateTime.Now)
				{
					msg = "拼团活动还未开始，请选择其他拼团活动！";
					return null;
				}
				if (fightGroup.EndTime < DateTime.Now)
				{
					msg = "拼团活动已经结束，请选择其他拼团活动！";
					return null;
				}
				if (flag)
				{
					msg = "您已经参加了该拼团活动！";
					return null;
				}
			}
			if (fightGroupActivitieInfo.MaxCount < buyAmount)
			{
				msg = "超过每单限购数量，请正确填写数量";
				return null;
			}
			if (fightGroupSku.BoughtCount > fightGroupSku.TotalCount & flag3)
			{
				msg = "拼团活动商品已抢完，请选择其他拼团活动";
				return null;
			}
			if (!flag3 && fightGroupSku.BoughtCount + buyAmount > fightGroupSku.TotalCount)
			{
				msg = "购买数量已超过抢购拼团活动剩余数量，请选择其他拼团活动或者减少购买数量";
				return null;
			}
			if (fightGroupSku != null)
			{
				if (fightGroupSku.BoughtCount > fightGroupSku.TotalCount & flag3)
				{
					msg = "活动规格库存已经抢完，请选择其他规格或者其他拼团活动";
					return null;
				}
				if (fightGroupActivitieInfo.MaxCount < buyAmount)
				{
					msg = "超过每单限购数量，请正确填写数量";
					return null;
				}
				if (!flag3 && fightGroupSku.BoughtCount + buyAmount > fightGroupSku.TotalCount)
				{
					msg = "购买数量已超过拼团活动剩余数量，请选择其他拼团活动或者减少购买数量";
					return null;
				}
			}
			if (skuItem == null)
			{
				msg = "拼团活动不存在，请选择其他拼团活动！";
				return null;
			}
			if (skuItem.Stock < buyAmount)
			{
				msg = "拼团商品库存不足，请选择其他拼团活动";
				return null;
			}
			if (flag2)
			{
				msg = "每个用户限购" + fightGroupActivitieInfo.MaxCount + "件，您已达到单个用户的最大购买数";
				return null;
			}
			return fightGroupActivitieInfo;
		}

		public static DataTable GetFightGroups(int fightGroupActivityId)
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			return new FightGroupDao().GetFightGroups(fightGroupActivityId, list);
		}

		public static IList<FightGroupModel> GetAllFightGroups(int fightGroupActivityId)
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			return new FightGroupDao().GetAllFightGroups(fightGroupActivityId, list);
		}

		public static IList<FightGroupUserModel> GetFightGroupUsers(int fightGroupId)
		{
			return new FightGroupDao().GetFightGroupUsers(fightGroupId);
		}

		public static bool CheckHasActiveFightGroupActivities()
		{
			return new FightGroupDao().CheckHasActiveFightGroupActivities();
		}

		public static DataTable GetFightGroupUsersWithSuccess(int fightGroupId)
		{
			return new FightGroupDao().GetFightGroupUsersWithSuccess(fightGroupId);
		}

		public static PageModel<UserFightGroupActivitiyModel> GetMyFightGroups(FightGroupQuery query)
		{
			return new FightGroupDao().GetMyFightGroups(query);
		}

		private static void ApplyRefund(OrderInfo order)
		{
			string userRemark = "拼团失败，自动退款";
			string refundReason = "拼团失败，自动退款";
			int refundType = 3;
			if (order.Gateway == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1))
			{
				refundType = 1;
			}
			if (TradeHelper.CanRefund(order, ""))
			{
				string gateway = order.Gateway;
				string generateId = Globals.GetGenerateId();
				decimal payTotal = order.GetPayTotal();
				RefundInfo refund = new RefundInfo
				{
					UserRemark = userRemark,
					RefundReason = refundReason,
					RefundType = (RefundTypes)refundType,
					RefundGateWay = gateway,
					RefundOrderId = generateId,
					RefundAmount = payTotal,
					StoreId = order.StoreId,
					ApplyForTime = DateTime.Now
				};
				TradeHelper.ApplyForRefund(refund);
			}
		}

		public static int GetMyFightGroupActiveNumber(int userId = 0)
		{
			if (userId == 0)
			{
				userId = HiContext.Current.UserId;
			}
			if (userId == 0)
			{
				return 0;
			}
			List<int> list = new List<int>();
			list.Add(0);
			List<int> list2 = new List<int>();
			list2.Add(1);
			list2.Add(4);
			return new FightGroupDao().GetFightGroupActiveNumber(null, userId, list, list2);
		}

		public static int GetFightGroupActiveNumber(int fightGroupId)
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(4);
			return new FightGroupDao().GetFightGroupActiveNumber(fightGroupId, null, null, list);
		}

		public static FightGroupInfo GetFightGroup(int fightGroupId)
		{
			return new FightGroupDao().Get<FightGroupInfo>(fightGroupId);
		}

		public static FightGroupModel GetFightGroupInfo(int fightGroupId)
		{
			return new FightGroupDao().GetFightGroupInfo(fightGroupId);
		}

		public static void EditFightGroupActivitie(FightGroupActivityInfo fightGroupActivitie)
		{
			new FightGroupDao().EditFightGroupActivitie(fightGroupActivitie);
		}

		public static void DeleteFightGroupActivitie(int fightGroupActivityId)
		{
			new FightGroupDao().DeleteFightGroupActivitie(fightGroupActivityId);
		}

		public static DataTable GetFightGroupSkus(int fightGroupActivityId, int productId)
		{
			DataTable skusByProductIdNew = ProductHelper.GetSkusByProductIdNew(productId);
			IList<FightGroupSkuInfo> fightGroupSkus = VShopHelper.GetFightGroupSkus(fightGroupActivityId);
			if (skusByProductIdNew.Rows.Count > 0)
			{
				skusByProductIdNew.Columns.Add("FightGroupActivityId", typeof(int));
				skusByProductIdNew.Columns.Add("FightGroupSkuId", typeof(int));
				skusByProductIdNew.Columns.Add("FightGroupSalePrice", typeof(decimal));
				skusByProductIdNew.Columns.Add("FightGroupTotalCount", typeof(int));
				skusByProductIdNew.Columns.Add("FightGroupBoughtCount", typeof(int));
			}
			for (int i = 0; i < skusByProductIdNew.Rows.Count; i++)
			{
				DataRow dataRow = skusByProductIdNew.Rows[i];
				string skuId = dataRow["SkuId"].ToString();
				FightGroupSkuInfo fightGroupSkuInfo = (from c in fightGroupSkus
				where c.SkuId == skuId && c.FightGroupActivityId == fightGroupActivityId
				select c).FirstOrDefault();
				if (fightGroupSkuInfo != null)
				{
					dataRow["FightGroupActivityId"] = fightGroupActivityId;
					dataRow["FightGroupSkuId"] = fightGroupSkuInfo.FightGroupSkuId.ToString();
					dataRow["FightGroupSalePrice"] = fightGroupSkuInfo.SalePrice;
					dataRow["FightGroupTotalCount"] = fightGroupSkuInfo.TotalCount;
					dataRow["FightGroupBoughtCount"] = fightGroupSkuInfo.BoughtCount;
				}
			}
			return skusByProductIdNew;
		}

		public static FightGroupActivityInfo GetFightGroupActivitieInfo(int fightGroupActivityId)
		{
			return new FightGroupDao().Get<FightGroupActivityInfo>(fightGroupActivityId);
		}

		public static IList<FightGroupSkuInfo> GetFightGroupSkus(int fightGroupActivityId)
		{
			return new FightGroupDao().GetFightGroupSkus(fightGroupActivityId);
		}

		public static bool CanAddFightGroupActivitiy(int prodcutId, string productName, int fightGroupActivityId = 0)
		{
			CountDownInfo countDownInfo = new CountDownDao().ActiveCountDownByProductId(prodcutId);
			if (countDownInfo != null)
			{
				return false;
			}
			if (new GroupBuyDao().IsActiveGroupByProductId(prodcutId))
			{
				return false;
			}
			if (new PromotionDao().GetActiveIdByProduct(prodcutId).HasValue)
			{
				return false;
			}
			if (fightGroupActivityId > 0)
			{
				IEnumerable<FightGroupActivitiyModel> models = VShopHelper.GetFightGroupActivities(new FightGroupActivitiyQuery
				{
					PageSize = 2147483647,
					PageIndex = 1,
					ProductId = prodcutId
				}).Models;
				DateTime now = DateTime.Now;
				if ((from c in models
				where c.FightGroupActivityId != fightGroupActivityId && c.EndDate >= now
				select c).Count() > 0)
				{
					return false;
				}
			}
			else
			{
				IEnumerable<FightGroupActivitiyModel> models2 = VShopHelper.GetFightGroupActivities(new FightGroupActivitiyQuery
				{
					PageSize = 2147483647,
					PageIndex = 1,
					ProductId = prodcutId
				}).Models;
				DateTime now2 = DateTime.Now;
				if ((from c in models2
				where c.EndDate >= now2
				select c).Count() > 0)
				{
					return false;
				}
			}
			return true;
		}

		public static string GetFightGroupActivitiyActiveProducts()
		{
			return new FightGroupDao().GetFightGroupActivitiyActiveProducts();
		}

		public static void AddFightGroupActivitie(FightGroupActivityInfo fightGroupActivitie, List<FightGroupSkuInfo> fightGroupSkus)
		{
			new FightGroupDao().AddFightGroupActivitie(fightGroupActivitie, fightGroupSkus);
		}

		public static void AddFightGroup(FightGroupInfo fightGroup)
		{
			new FightGroupDao().Add(fightGroup, null);
		}

		public static void EditFightGroupActivitie(FightGroupActivityInfo fightGroupActivitie, IList<FightGroupSkuInfo> fightGroupSkus)
		{
			FightGroupDao fightGroupDao = new FightGroupDao();
			FightGroupActivityInfo fightGroupActivityInfo = fightGroupDao.Get<FightGroupActivityInfo>(fightGroupActivitie.FightGroupActivityId);
			if (fightGroupActivityInfo != null)
			{
				fightGroupActivityInfo.EndDate = fightGroupActivitie.EndDate;
				fightGroupActivityInfo.Icon = fightGroupActivitie.Icon;
				fightGroupActivityInfo.JoinNumber = fightGroupActivitie.JoinNumber;
				fightGroupActivityInfo.LimitedHour = fightGroupActivitie.LimitedHour;
				fightGroupActivityInfo.MaxCount = fightGroupActivitie.MaxCount;
				fightGroupActivityInfo.ProductId = fightGroupActivitie.ProductId;
				fightGroupActivityInfo.StartDate = fightGroupActivitie.StartDate;
				fightGroupActivityInfo.ProductName = fightGroupActivitie.ProductName;
				fightGroupActivityInfo.ShareTitle = fightGroupActivitie.ShareTitle;
				fightGroupActivityInfo.ShareContent = fightGroupActivitie.ShareContent;
				fightGroupDao.Update(fightGroupActivityInfo, null);
				if (fightGroupActivityInfo.StartDate > DateTime.Now)
				{
					fightGroupDao.DeleteFightGroupSkuByActivityId(fightGroupActivitie.FightGroupActivityId);
				}
			}
			foreach (FightGroupSkuInfo fightGroupSku in fightGroupSkus)
			{
				FightGroupSkuInfo groupSkuInfoByActivityIdSkuId = fightGroupDao.GetGroupSkuInfoByActivityIdSkuId(fightGroupActivitie.FightGroupActivityId, fightGroupSku.SkuId);
				if (groupSkuInfoByActivityIdSkuId != null)
				{
					groupSkuInfoByActivityIdSkuId.SalePrice = fightGroupSku.SalePrice;
					groupSkuInfoByActivityIdSkuId.SkuId = fightGroupSku.SkuId;
					groupSkuInfoByActivityIdSkuId.TotalCount = fightGroupSku.TotalCount;
					fightGroupDao.Update(groupSkuInfoByActivityIdSkuId, null);
				}
				else
				{
					fightGroupDao.Add(fightGroupSku, null);
				}
			}
		}

		public static bool ProductFightGroupActivitiyExist(int productId)
		{
			return new FightGroupDao().ProductFightGroupActivitiyExist(productId);
		}

		public static bool ProductFightGroupActivitiyExist(int productId, int fightGroupActivityId, DateTime endDate)
		{
			return new FightGroupDao().ProductFightGroupActivitiyExist(productId, fightGroupActivityId, endDate);
		}

		public static IList<OrderInfo> GetFightGroupOrders(int fightGroupId)
		{
			return new OrderDao().GetFightGroupOrders(fightGroupId, false);
		}

		public static IList<OrderInfo> GetFightGroupOrdersJustShowPay(int fightGroupId)
		{
			return new OrderDao().GetFightGroupOrders(fightGroupId, true);
		}

		public static PageModel<FightGroupModel> GetFightGroups(FightGroupActivitiyQuery query)
		{
			return new FightGroupDao().GetFightGroups(query);
		}

		public static PageModel<FightGroupModel> GetFightGroupList(FightGroupActivitiyQuery query)
		{
			return new FightGroupDao().GetFightGroupList(query);
		}

		public static PageModel<FightGroupActivitiyModel> GetFightGroupActivities(FightGroupActivitiyQuery query)
		{
			PageModel<FightGroupActivityInfo> fightGroupActivities = new FightGroupDao().GetFightGroupActivities(query);
			List<FightGroupActivitiyModel> fightGroupActivitiyModelList = new List<FightGroupActivitiyModel>();
			fightGroupActivities.Models.ForEach(delegate(FightGroupActivityInfo c)
			{
				FightGroupActivitiyModel fightGroupActivitiyModel = new FightGroupActivitiyModel();
				fightGroupActivitiyModel.ProductName = new ProductDao().GetProductDetails(c.ProductId).ProductName;
				fightGroupActivitiyModel.StartDate = c.StartDate;
				fightGroupActivitiyModel.EndDate = c.EndDate;
				fightGroupActivitiyModel.FightGroupActivityId = c.FightGroupActivityId;
				fightGroupActivitiyModel.CreateGroupCount = VShopHelper.GetFightGroupActivityCreateGroupCount(c.FightGroupActivityId);
				fightGroupActivitiyModel.CreateGroupSuccessCount = new FightGroupDao().GetFightGroupActivityCreateGroupSuccessCount(c.FightGroupActivityId);
				fightGroupActivitiyModel.ProductId = c.ProductId;
				fightGroupActivitiyModel.DisplaySequence = c.DisplaySequence;
				fightGroupActivitiyModelList.Add(fightGroupActivitiyModel);
			});
			return new PageModel<FightGroupActivitiyModel>
			{
				Models = fightGroupActivitiyModelList,
				Total = fightGroupActivities.Total
			};
		}

		public static PageModel<FightGroupActivitiyModel> GetFightGroupActivitieLists(FightGroupActivityQuery query)
		{
			return new FightGroupDao().GetFightGroupActivitieLists(query);
		}

		public static int GetFightGroupActivityCreateGroupCount(int fightGroupActivityId)
		{
			return new FightGroupDao().GetFightGroupActivityCreateGroupCount(fightGroupActivityId);
		}

		public static bool ExistEffectiveFightGroupInfo(int ProductId)
		{
			return new FightGroupDao().ExistEffectiveFightGroupInfo(ProductId);
		}

		public static bool UserIsFightGroupHead(int fightGroupId, int userId)
		{
			if (userId <= 0)
			{
				return false;
			}
			return new FightGroupDao().UserIsFightGroupHead(fightGroupId, userId);
		}

		public static decimal GetUserFightPrice(int fightGroupId, int userId)
		{
			return new FightGroupDao().GetUserFightPrice(fightGroupId, userId);
		}

		public static bool isEndFightCannotDel(int fightGroupActivityId)
		{
			return new FightGroupDao().isEndFightCannotDel(fightGroupActivityId);
		}

		public static bool IsFightGroupCanRefund(int fightgroupId)
		{
			FightGroupInfo fightGroup = VShopHelper.GetFightGroup(fightgroupId);
			if (fightGroup != null)
			{
				return fightGroup.Status == FightGroupStatus.FightGroupSuccess;
			}
			return true;
		}

		public static void SwapFightGroupActivitySequence(int fightgroupActivityId, int displaySequence)
		{
			new FightGroupDao().SaveSequence<FightGroupActivityInfo>(fightgroupActivityId, displaySequence, null);
		}
	}
}
