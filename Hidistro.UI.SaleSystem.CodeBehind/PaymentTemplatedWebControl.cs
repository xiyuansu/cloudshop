using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class PaymentTemplatedWebControl : HtmlTemplatedWebControl
	{
		private readonly bool isBackRequest;

		protected PaymentNotify Notify;

		protected OrderInfo Order;

		protected string OrderId;

		protected decimal Amount;

		protected string Gateway;

		protected bool IsQRCodePay = false;

		private object lockobj = new object();

		private NameValueCollection pageParam = null;

		private bool hasNotify = false;

		public PaymentTemplatedWebControl(bool _isBackRequest)
		{
			this.isBackRequest = _isBackRequest;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			this.pageParam = new NameValueCollection
			{
				HttpContext.Current.Request.Form,
				HttpContext.Current.Request.QueryString
			};
			if (!this.isBackRequest)
			{
				if (!base.LoadHtmlThemedControl())
				{
					throw new SkinNotFoundException(this.SkinPath);
				}
				this.AttachChildControls();
			}
			this.DoValidate();
		}

		public static string BuildQuery(IDictionary<string, string> dict, bool encode)
		{
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>(dict);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			IEnumerator<KeyValuePair<string, string>> enumerator = (IEnumerator<KeyValuePair<string, string>>)(object)sortedDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				string value = current.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
					if (flag)
					{
						stringBuilder.Append("&");
					}
					stringBuilder.Append(key);
					stringBuilder.Append("=");
					if (encode && key.ToLower() != "service" && key.ToLower() != "_input_charset")
					{
						stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
					}
					else
					{
						stringBuilder.Append(value);
					}
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}

		public Dictionary<string, string> getParamDict(NameValueCollection parameters)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			int num = 0;
			decimal num2 = default(decimal);
			text = this.Page.Request["sign"];
			text2 = this.Page.Request["user_id"];
			text3 = this.Page.Request["qrcode"];
			text4 = this.Page.Request["goods_id"];
			text5 = this.Page.Request["goods_name"];
			int.TryParse(this.Page.Request["quantity"], out num);
			decimal.TryParse(this.Page.Request["price"], out num2);
			text6 = this.Page.Request["sku_id"];
			text7 = this.Page.Request["sku_name"];
			text8 = this.Page.Request["context_data"];
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("user_id", text2);
			dictionary.Add("qrcode", text3);
			dictionary.Add("goods_id", text4);
			dictionary.Add("goods_name", text5);
			dictionary.Add("quantity", num.ToString());
			dictionary.Add("price", num2.ToString());
			dictionary.Add("sku_id", text6);
			dictionary.Add("sku_name", text7);
			dictionary.Add("context_data", text8);
			return dictionary;
		}

		private string GetWXQRCodePayResult()
		{
			Stream inputStream = this.Page.Request.InputStream;
			int num = 0;
			byte[] array = new byte[1024];
			StringBuilder stringBuilder = new StringBuilder();
			while ((num = inputStream.Read(array, 0, 1024)) > 0)
			{
				stringBuilder.Append(Encoding.UTF8.GetString(array, 0, num));
			}
			inputStream.Flush();
			inputStream.Close();
			inputStream.Dispose();
			return stringBuilder.ToString();
		}

		public void ValidateQRCode(NameValueCollection parameters)
		{
			string str = "";
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
			string text = "";
			string text2 = "";
			string text3 = "";
			int num = 0;
			decimal d = default(decimal);
			text = this.Page.Request["sign"];
			text2 = this.Page.Request["goods_id"];
			int.TryParse(this.Page.Request["quantity"], out num);
			decimal.TryParse(this.Page.Request["price"], out d);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary = this.getParamDict(parameters);
			string str2 = PaymentTemplatedWebControl.BuildQuery(dictionary, false);
			string orderId = text2;
			this.Order = TradeHelper.GetOrderInfo(orderId);
			if (this.Order == null)
			{
				Globals.WriteLog(parameters, "订单信息为空" + this.Gateway, "", "", "PayNotify");
				text3 = "PARAM_ILLEGAL";
			}
			else
			{
				PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.Order.PaymentTypeId);
				if (paymentMode == null)
				{
					Globals.WriteLog(parameters, "支付方式为空" + this.Gateway, "", "", "PayNotify");
					text3 = "PARAM_ILLEGAL";
				}
				else
				{
					Globals.EntityCoding(paymentMode, false);
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(paymentMode.Settings));
					string text4 = configData.SettingsXml.ToLower();
					if (text4.IndexOf("<key>") > -1 && text4.IndexOf("</key>") > -1)
					{
						str = text4.Substring(text4.IndexOf("<key>") + 5, text4.IndexOf("</key>") - text4.IndexOf("<key>") - 5);
					}
					string b = FormsAuthentication.HashPasswordForStoringInConfigFile(str2 + str, "MD5").ToLower();
					if (this.Order.GetTotal(true) != d)
					{
						Globals.WriteLog(parameters, "支付金额与订单金额不匹配" + this.Gateway, "", "", "PayNotify");
						text3 = "PRICE_NOT_MATCH";
					}
					if (text != b)
					{
						Globals.WriteLog(parameters, "签名不匹配" + this.Gateway, "", "", "PayNotify");
						text3 = "PARAM_ILLEGAL";
					}
				}
			}
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.ContentType = "application/json";
			StringBuilder stringBuilder = new StringBuilder("{");
			if (text3 != "")
			{
				stringBuilder.AppendFormat("is_success:\"F\",error_code:\"{0}\"", text3);
			}
			else
			{
				stringBuilder.AppendFormat("is_success:\"T\",out_trade_no:\"{0}\"", this.Order.OrderId);
			}
			stringBuilder.Append("}");
			HttpContext.Current.Response.Write(stringBuilder.ToString());
			HttpContext.Current.Response.End();
		}

		private void DoValidate()
		{
			string text = "";
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.Gateway = base.GetParameter("HIGW", false);
			if (!string.IsNullOrEmpty(this.Gateway))
			{
				this.Gateway = this.Gateway.ToLower().Replace("_", ".");
				if (this.Gateway.ToLower().IndexOf("cmpay.d") > -1)
				{
					this.Gateway = this.Gateway.ToLower().Replace("cmpay.d", "cmpay_d");
				}
			}
			else
			{
				this.Gateway = "";
			}
			if (this.Gateway == "hishop.plugins.payment.alipayqrcode.qrcoderequest" && nameValueCollection["goods_id"] != null && nameValueCollection["goods_id"] != "")
			{
				this.ValidateQRCode(nameValueCollection);
			}
			else
			{
				if (this.Gateway == "hishop.plugins.payment.wxqrcode.wxqrcoderequest")
				{
					string wXQRCodePayResult = this.GetWXQRCodePayResult();
					nameValueCollection.Add("notify_data", wXQRCodePayResult);
				}
				if (this.Gateway == "hishop.plugins.payment.alipayforextrade")
				{
					nameValueCollection.Add("IsReturn", this.isBackRequest ? "false" : "true");
				}
				if (this.Gateway == "hishop.plugins.payment.alipaydirect.directrequest")
				{
					if (this.isBackRequest)
					{
						nameValueCollection = new NameValueCollection
						{
							this.Page.Request.Form
						};
					}
					else
					{
						nameValueCollection = new NameValueCollection
						{
							this.Page.Request.QueryString
						};
						nameValueCollection.Add("IsReturn", this.isBackRequest ? "false" : "true");
					}
				}
				this.Notify = PaymentNotify.CreateInstance(this.Gateway, nameValueCollection);
				if (this.isBackRequest)
				{
					string hIGW = this.Gateway.Replace(".", "_");
					this.Notify.ReturnUrl = Globals.FullPath(base.GetRouteUrl("PaymentReturn_url", new
					{
						HIGW = hIGW
					})) + "?" + this.Page.Request.Url.Query;
				}
				this.OrderId = this.Notify.GetOrderId();
				this.Order = TradeHelper.GetOrderInfo(this.OrderId);
				if (this.Order == null)
				{
					Globals.AppendLog(this.pageParam, "订单信息为空", "", "", "PayNotify");
					this.ResponseStatus(true, "ordernotfound");
				}
				else
				{
					text = "OrderId:" + this.OrderId;
					try
					{
						this.Amount = this.Notify.GetOrderAmount();
					}
					catch
					{
						this.Amount = default(decimal);
					}
					if (this.Amount <= decimal.Zero)
					{
						this.Amount = this.Order.GetTotal(true);
					}
					text = text + "--Amount:" + this.Amount;
					this.hasNotify = !string.IsNullOrEmpty(this.Order.GatewayOrderId);
					if (this.Order.PreSaleId > 0 && this.Order.DepositGatewayOrderId.ToNullString() == this.Notify.GetGatewayOrderId())
					{
						this.ResponseStatus(true, "预售订单订金已支付成功");
					}
					else
					{
						if (this.Order.PreSaleId > 0 && !this.Order.DepositDate.HasValue)
						{
							this.Order.DepositGatewayOrderId = this.Notify.GetGatewayOrderId();
						}
						else
						{
							this.Order.GatewayOrderId = this.Notify.GetGatewayOrderId();
						}
						text = text + "--GateWayOrderId:" + this.Notify.GetGatewayOrderId();
						PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.Order.PaymentTypeId);
						if (paymentMode == null)
						{
							Globals.WriteLog(nameValueCollection, "支付方式没找到" + this.Gateway, "", "", "PayNotify");
							this.ResponseStatus(true, "gatewaynotfound");
						}
						else
						{
							this.Notify.Finished += this.Notify_Finished;
							this.Notify.NotifyVerifyFaild += this.Notify_NotifyVerifyFaild;
							this.Notify.Payment += this.Notify_Payment;
							try
							{
								this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(paymentMode.Settings));
							}
							catch (Exception ex)
							{
								if (!(ex is ThreadAbortException))
								{
									Globals.WriteLog(nameValueCollection, ex.Message + "---程序错误1---" + this.Gateway, "", "", "PayNotify");
								}
							}
						}
					}
				}
			}
		}

		private void Notify_Payment(object sender, EventArgs e)
		{
			this.UserPayOrder();
		}

		private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
		{
			this.ResponseStatus(false, "verifyfaild");
		}

		private void Notify_Finished(object sender, FinishedEventArgs e)
		{
			if (e.IsMedTrade)
			{
				this.FinishOrder();
			}
			else
			{
				this.UserPayOrder();
			}
		}

		protected abstract void DisplayMessage(string status);

		private void ResponseStatus(bool success, string status)
		{
			if (this.isBackRequest)
			{
				this.Notify.WriteBack(HiContext.Current.Context, success);
			}
			else
			{
				this.DisplayMessage(status);
			}
		}

		private void UserPayOrder()
		{
			lock (this.lockobj)
			{
				if (this.Order.OrderStatus == OrderStatus.Closed)
				{
					Globals.WriteLog("订单已关闭" + this.Gateway + "-" + this.Order.OrderId, "", "", "PayNotify");
					if (!this.hasNotify)
					{
						OrderHelper.SetExceptionOrder(this.Order.OrderId, "支付异常，请联系买家退款");
						Messenger.OrderException(Users.GetUser(this.Order.UserId), this.Order, "订单支付异常,请联系卖家退款.订单号:" + this.Order.OrderId);
						TradeHelper.UpdateOrderGatewayOrderId(this.Order.OrderId, this.Order.GatewayOrderId);
					}
					this.ResponseStatus(true, "orderclosed");
				}
				else if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					Globals.WriteLog("订单已付款" + this.Gateway + "-" + this.Order.OrderId, "", "", "PayNotify");
					this.ResponseStatus(true, "success");
				}
				else
				{
					int maxCount = 0;
					int yetOrderNum = 0;
					int currentOrderNum = 0;
					if (this.Order.GroupBuyId > 0)
					{
						GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(this.Order.GroupBuyId);
						if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
						{
							this.ResponseStatus(false, "groupbuyalreadyfinished");
							goto end_IL_000a;
						}
						yetOrderNum = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
						currentOrderNum = this.Order.GetGroupBuyOerderNumber();
						maxCount = groupBuy.MaxCount;
						if (maxCount < yetOrderNum + currentOrderNum)
						{
							this.ResponseStatus(false, "exceedordermax");
							goto end_IL_000a;
						}
					}
					if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UpdateOrderStatus(this.Order))
					{
						Task.Factory.StartNew(delegate
						{
							TradeHelper.UserPayOrder(this.Order, false, true);
							try
							{
								if (this.Order.FightGroupId > 0)
								{
									VShopHelper.SetFightGroupSuccess(this.Order.FightGroupId);
								}
								if (this.Order.GroupBuyId > 0 && maxCount == yetOrderNum + currentOrderNum)
								{
									TradeHelper.SetGroupBuyEndUntreated(this.Order.GroupBuyId);
								}
								if (this.Order.ParentOrderId == "-1")
								{
									OrderQuery orderQuery = new OrderQuery();
									orderQuery.ParentOrderId = this.Order.OrderId;
									IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(this.Order.UserId, orderQuery);
									foreach (OrderInfo item in listUserOrder)
									{
										OrderHelper.OrderConfirmPaySendMessage(item);
									}
								}
								else
								{
									OrderHelper.OrderConfirmPaySendMessage(this.Order);
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
								Globals.AppendLog(dictionary, "支付更新订单收款记录或者消息通知时出错：" + ex.Message, "", "", "UserPay");
							}
							this.Order.OnPayment();
						});
						this.ResponseStatus(true, "success");
					}
					else
					{
						this.ResponseStatus(false, "fail");
					}
				}
				end_IL_000a:;
			}
		}

		private void FinishOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.Finished)
			{
				this.ResponseStatus(true, "success");
			}
			else if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
			{
				this.ResponseStatus(true, "success");
			}
			else
			{
				this.ResponseStatus(false, "fail");
			}
		}
	}
}
