using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.pay
{
	public class AppStore_wxPay : Page
	{
		protected OrderInfo Order;

		protected string OrderId;

		private StoreCollectionInfo offlineOrder = null;

		private bool isOfflineOrder = false;

		private NameValueCollection param = null;

		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.param = new NameValueCollection
			{
				base.Request.QueryString,
				base.Request.Form
			};
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			NotifyClient notifyClient = null;
			notifyClient = ((string.IsNullOrEmpty(masterSettings.Main_Mch_ID) || string.IsNullOrEmpty(masterSettings.Main_AppId)) ? new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, "", "") : new NotifyClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID));
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				base.Request.QueryString,
				base.Request.Form
			};
			string sign = "";
			PayNotify payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
			if (payNotify == null)
			{
				Globals.AppendLog(this.param, "通知信息为空", sign, "", "AppStore_wxPay");
			}
			else
			{
				this.OrderId = payNotify.PayInfo.OutTradeNo;
				this.Order = ShoppingProcessor.GetOrderInfo(this.OrderId);
				this.offlineOrder = StoresHelper.GetStoreCollectionInfo(this.OrderId);
				if (this.Order == null)
				{
					this.Order = ShoppingProcessor.GetOrderInfo(payNotify.PayInfo.OutTradeNo);
				}
				if (this.offlineOrder == null)
				{
					this.offlineOrder = StoresHelper.GetStoreCollectionInfo(payNotify.PayInfo.OutTradeNo);
				}
				if (this.Order == null && this.offlineOrder == null)
				{
					Globals.AppendLog(this.param, "订单信息为空", sign, "", "AppStore_wxPay");
					base.Response.Write("success");
				}
				else
				{
					EnumPaymentType enumPaymentType = EnumPaymentType.WXPay;
					if (this.Order == null)
					{
						this.isOfflineOrder = true;
					}
					else
					{
						this.Order.PaymentTypeId = (int)enumPaymentType;
						this.Order.PaymentType = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 0);
						this.Order.Gateway = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 1);
						this.Order.GatewayOrderId = payNotify.PayInfo.TransactionId;
						if (this.offlineOrder == null)
						{
							this.offlineOrder = StoresHelper.GetStoreCollectionInfoOfOrderId(this.Order.OrderId);
						}
					}
					if (this.offlineOrder != null)
					{
						this.offlineOrder.GateWayOrderId = payNotify.PayInfo.TransactionId;
						this.offlineOrder.PaymentTypeId = (int)enumPaymentType;
						this.offlineOrder.PaymentTypeName = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 0);
						this.offlineOrder.GateWay = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 1);
					}
					this.UserPayOrder();
					base.Response.Write("success");
				}
			}
		}

		private void UserPayOrder()
		{
			if (this.Order != null && this.Order.OrderStatus == OrderStatus.Closed)
			{
				OrderHelper.SetExceptionOrder(this.Order.OrderId, "支付异常，请联系买家退款");
				Messenger.OrderException(Users.GetUser(this.Order.UserId), this.Order, "订单支付异常,请联系卖家退款.订单号:" + this.Order.OrderId);
				this.ResponseReturn(true, "");
			}
			else if ((this.Order != null && this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid) || (this.offlineOrder != null && this.offlineOrder.Status == 1))
			{
				this.ResponseReturn(true, "");
			}
			else
			{
				try
				{
					int maxCount = 0;
					int yetOrderNum = 0;
					int currentOrderNum = 0;
					if (this.Order != null)
					{
						if (this.Order.GroupBuyId > 0)
						{
							GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(this.Order.GroupBuyId);
							if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
							{
								return;
							}
							yetOrderNum = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
							currentOrderNum = this.Order.GetGroupBuyOerderNumber();
							maxCount = groupBuy.MaxCount;
							if (maxCount < yetOrderNum + currentOrderNum)
							{
								return;
							}
						}
						if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UpdateOrderStatus(this.Order))
						{
							Task.Factory.StartNew(delegate
							{
								TradeHelper.UserPayOrder(this.Order, false, true);
								try
								{
									if (this.offlineOrder != null)
									{
										OrderHelper.UpdateOrderPaymentTypeOfAPI(this.Order);
										OrderHelper.ConfirmTakeGoods(this.Order, true);
									}
									if (this.Order.GroupBuyId > 0 && maxCount == yetOrderNum + currentOrderNum)
									{
										TradeHelper.SetGroupBuyEndUntreated(this.Order.GroupBuyId);
									}
									if (this.Order.UserId != 0 && this.Order.UserId != 1100)
									{
										string verificationPasswords = "";
										if (this.Order.OrderType == OrderType.ServiceOrder)
										{
											verificationPasswords = OrderHelper.GetVerificationPasswordsOfOrderId(this.Order.OrderId);
										}
										Hidistro.Entities.Members.MemberInfo user = Users.GetUser(this.Order.UserId);
										if (user != null)
										{
											Messenger.OrderPayment(user, this.Order, this.Order.GetTotal(true), verificationPasswords);
										}
									}
									StoresInfo storesInfo2 = null;
									if (this.Order.StoreId > 0)
									{
										storesInfo2 = DepotHelper.GetStoreById(this.Order.StoreId);
									}
									if (storesInfo2 != null)
									{
										VShopHelper.AppPsuhRecordForStore(storesInfo2.StoreId, this.Order.OrderId, "", EnumPushStoreAction.StoreOrderPayed);
										if (this.offlineOrder == null)
										{
											if (this.Order.ShippingModeId == -2)
											{
												VShopHelper.AppPsuhRecordForStore(storesInfo2.StoreId, this.Order.OrderId, "", EnumPushStoreAction.TakeOnStoreOrderWaitConfirm);
											}
											else
											{
												VShopHelper.AppPsuhRecordForStore(storesInfo2.StoreId, this.Order.OrderId, "", EnumPushStoreAction.StoreOrderWaitSendGoods);
											}
										}
									}
									if (this.offlineOrder == null)
									{
										ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
										Messenger.OrderPaymentToShipper(defaultOrFirstShipper, storesInfo2, null, this.Order, this.Order.GetTotal(true));
									}
									this.Order.OnPayment();
								}
								catch (Exception ex2)
								{
									IDictionary<string, string> dictionary2 = new Dictionary<string, string>();
									dictionary2.Add("ErrorMessage", ex2.Message);
									dictionary2.Add("StackTrace", ex2.StackTrace);
									if (ex2.InnerException != null)
									{
										dictionary2.Add("InnerException", ex2.InnerException.ToString());
									}
									if (ex2.GetBaseException() != null)
									{
										dictionary2.Add("BaseException", ex2.GetBaseException().Message);
									}
									if (ex2.TargetSite != (MethodBase)null)
									{
										dictionary2.Add("TargetSite", ex2.TargetSite.ToString());
									}
									dictionary2.Add("ExSource", ex2.Source);
									Globals.AppendLog(dictionary2, "支付更新订单收款记录或者消息通知时出错：" + ex2.Message, "", "", "UserPay");
								}
								this.Order.OnPayment();
								this.ResponseReturn(true, "");
							});
						}
						if (this.Order.FightGroupId > 0)
						{
							VShopHelper.SetFightGroupSuccess(this.Order.FightGroupId);
						}
					}
					if (this.offlineOrder != null && this.offlineOrder.Status == 0)
					{
						this.offlineOrder.Status = 1;
						this.offlineOrder.PayTime = DateTime.Now;
						if (StoresHelper.UpdateStoreCollectionInfo(this.offlineOrder) && this.isOfflineOrder)
						{
							string text = "";
							StoresInfo storeById = DepotHelper.GetStoreById(this.offlineOrder.StoreId);
							if (storeById != null)
							{
								text = storeById.StoreName;
								StoreBalanceDetailInfo storeBalanceDetailInfo = new StoreBalanceDetailInfo();
								storeBalanceDetailInfo.StoreId = this.offlineOrder.StoreId;
								storeBalanceDetailInfo.TradeDate = DateTime.Now;
								storeBalanceDetailInfo.TradeType = StoreTradeTypes.OfflineCashier;
								storeBalanceDetailInfo.Expenses = default(decimal);
								storeBalanceDetailInfo.Income = this.offlineOrder.PayAmount;
								storeBalanceDetailInfo.Balance = storeById.Balance + this.offlineOrder.PayAmount;
								storeBalanceDetailInfo.Remark = "线下收银(" + this.offlineOrder.OrderId + ")";
								storeBalanceDetailInfo.ManagerUserName = "";
								storeBalanceDetailInfo.TradeNo = this.offlineOrder.OrderId;
								storeBalanceDetailInfo.CreateTime = DateTime.Now;
								storeBalanceDetailInfo.PlatCommission = decimal.Zero;
								if (StoreBalanceHelper.AddBalanceDetailInfo(storeBalanceDetailInfo))
								{
									StoresInfo storesInfo = storeById;
									storesInfo.Balance += this.offlineOrder.PayAmount;
									StoresHelper.UpdateStore(storeById);
								}
							}
							if (storeById != null)
							{
								VShopHelper.AppPsuhRecordForStore(storeById.StoreId, this.offlineOrder.OrderId, "", EnumPushStoreAction.StoreOrderPayed);
							}
						}
					}
				}
				catch (Exception ex)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("ErrorMessage", ex.Message);
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
					Globals.AppendLog(dictionary, "支付更新订单收款记录或者消息通知时出错：" + ex.Message, "", "", "UserPay1");
				}
				this.ResponseReturn(true, "");
			}
		}

		public void ResponseReturn(bool isSuccess, string msg = "")
		{
			base.Response.ContentType = "text/xml";
			base.Response.Write("<xml>");
			base.Response.Write(string.Format("<return_code><![CDATA[{0}]]></return_code>", isSuccess ? "SUCCESS" : "FAIL"));
			base.Response.Write(string.Format("<return_msg><![CDATA[OK]]></return_msg>", isSuccess ? "OK" : msg));
			base.Response.Write("<xml>");
			base.Response.End();
		}
	}
}
