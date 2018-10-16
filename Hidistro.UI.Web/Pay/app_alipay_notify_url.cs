using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Messages;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.pay
{
	public class app_alipay_notify_url : Page
	{
		private bool hasNotify = false;

		protected void Page_Load(object sender, EventArgs e)
		{
			SortedDictionary<string, string> requestPost = this.GetRequestPost();
			if (requestPost.Count > 0)
			{
				Notify notify = new Notify();
				if (notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]))
				{
					string text = base.Request.Form["out_trade_no"];
					string text2 = base.Request.Form["trade_no"];
					string text3 = base.Request.Form["subject"];
					string a = base.Request.Form["trade_status"];
					if (!(a == "TRADE_FINISHED") && a == "TRADE_SUCCESS")
					{
						if (text3.Trim().Equals("预付款充值"))
						{
							InpourRequestInfo inpourBlance = MemberProcessor.GetInpourBlance(text);
							if (inpourBlance == null)
							{
								app_alipay_notify_url.writeLog("没有找到充值申请记录");
								base.Response.Write("success");
								return;
							}
							MemberProcessor.AddBalanceDetailInfo(inpourBlance, "支付宝app支付");
						}
						else if (text3.Trim().Equals("订单支付"))
						{
							if (!string.IsNullOrEmpty(text) && text.Length > 5)
							{
								text = text.Substring(0, text.Length - 5);
							}
							OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(text);
							if (orderInfo == null)
							{
								base.Response.Write("success");
								base.Response.End();
								return;
							}
							this.hasNotify = !string.IsNullOrEmpty(orderInfo.GatewayOrderId);
							if (orderInfo.PreSaleId > 0 && orderInfo.DepositGatewayOrderId.ToNullString() == text2)
							{
								base.Response.Write("success");
								base.Response.End();
								return;
							}
							if (orderInfo.PreSaleId > 0 && !orderInfo.DepositDate.HasValue)
							{
								orderInfo.DepositGatewayOrderId = text2;
							}
							else
							{
								orderInfo.GatewayOrderId = text2;
							}
							this.UserPayOrder(orderInfo);
						}
					}
					base.Response.Write("success");
					base.Response.End();
				}
				else
				{
					base.Response.Write("fail");
					base.Response.End();
				}
			}
			else
			{
				base.Response.Write("无通知参数");
				base.Response.End();
			}
		}

		internal static void writeLog(string msg)
		{
			DataTable dataTable = new DataTable();
			dataTable.TableName = "log";
			dataTable.Columns.Add(new DataColumn("HishopOperTime"));
			dataTable.Columns.Add(new DataColumn("Msg"));
			DataRow dataRow = dataTable.NewRow();
			dataRow["HishopOperTime"] = DateTime.Now;
			dataRow["Msg"] = msg;
			dataTable.Rows.Add(dataRow);
			dataTable.WriteXml(HttpContext.Current.Server.MapPath("/alinotifylog.xml"));
		}

		private void UserPayOrder(OrderInfo order)
		{
			if (order.OrderStatus == OrderStatus.Closed)
			{
				if (!this.hasNotify)
				{
					OrderHelper.SetExceptionOrder(order.OrderId, "支付异常，请联系买家退款");
					Messenger.OrderException(Users.GetUser(order.UserId), order, "订单支付异常,请联系卖家退款.订单号:" + order.OrderId);
					TradeHelper.UpdateOrderGatewayOrderId(order.OrderId, order.GatewayOrderId);
				}
			}
			else if (order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
			{
				int maxCount = 0;
				int yetOrderNum = 0;
				int currentOrderNum = 0;
				if (order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UpdateOrderStatus(order))
				{
					Task.Factory.StartNew(delegate
					{
						TradeHelper.UserPayOrder(order, false, true);
						try
						{
							if (order.FightGroupId > 0)
							{
								VShopHelper.SetFightGroupSuccess(order.FightGroupId);
							}
							if (order.GroupBuyId > 0 && maxCount == yetOrderNum + currentOrderNum)
							{
								TradeHelper.SetGroupBuyEndUntreated(order.GroupBuyId);
							}
							if (order.ParentOrderId == "-1")
							{
								OrderQuery orderQuery = new OrderQuery();
								orderQuery.ParentOrderId = order.OrderId;
								IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(order.UserId, orderQuery);
								foreach (OrderInfo item in listUserOrder)
								{
									OrderHelper.OrderConfirmPaySendMessage(item);
								}
							}
							else
							{
								OrderHelper.OrderConfirmPaySendMessage(order);
							}
							order.OnPayment();
							base.Response.Write("success");
							base.Response.End();
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
					});
				}
			}
		}

		private SortedDictionary<string, string> GetRequestPost()
		{
			int num = 0;
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			NameValueCollection form = base.Request.Form;
			string[] allKeys = form.AllKeys;
			for (num = 0; num < allKeys.Length; num++)
			{
				sortedDictionary.Add(allKeys[num], base.Request.Form[allKeys[num]]);
			}
			return sortedDictionary;
		}
	}
}
