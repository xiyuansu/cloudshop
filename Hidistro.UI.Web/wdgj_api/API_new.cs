using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace Hidistro.UI.Web.wdgj_api
{
	public class API_new : IHttpHandler
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
			context.Response.ContentType = "text/plain";
			string appkey = context.Request["appkey"];
			string token = context.Request["token"];
			string text = context.Request["method"];
			string bizcontent = context.Request["bizcontent"];
			string sign = context.Request["sign"];
			string checkCode = HiContext.Current.SiteSettings.CheckCode;
			string text2 = this.CheckSign(appkey, token, text, bizcontent, sign, checkCode);
			if (!text2.Contains("成功"))
			{
				context.Response.Write(text2);
				context.Response.End();
			}
			else
			{
				switch (text)
				{
				case "Differ.JH.Business.GetOrder":
					context.Response.Write(this.GetOrder(context));
					break;
				case "Differ.JH.Business.CheckRefundStatus":
					context.Response.Write(this.CheckRefundStatus(context));
					break;
				case "Differ.JH.Business.Send":
					context.Response.Write(this.Send(context));
					break;
				case "Differ.JH.Business.UpdateSellerMemo":
					context.Response.Write(this.UpdateSellerMemo(context));
					break;
				case "Differ.JH.Business.DownloadProduct":
					context.Response.Write(this.DownloadProduct(context));
					break;
				case "Differ.JH.Business.SyncStock":
					context.Response.Write(this.SyncStock(context));
					break;
				case "Differ.JH.Business.GetRefund":
					context.Response.Write(this.GetRefund(context));
					break;
				}
			}
		}

		public string GetOrder(HttpContext context)
		{
			string value = context.Request["bizcontent"];
			JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
			string text = "";
			if (jObject["OrderStatus"] != null)
			{
				text = jObject["OrderStatus"].ToString();
			}
			string text2 = "";
			if (jObject["PlatOrderNo"] != null)
			{
				text2 = jObject["PlatOrderNo"].ToString();
			}
			string text3 = null;
			if (jObject["StartTime"] != null)
			{
				text3 = jObject["StartTime"].ToString();
			}
			string text4 = null;
			if (jObject["EndTime"] != null)
			{
				text4 = jObject["EndTime"].ToString();
			}
			string a = "";
			if (jObject["TimeType"] != null)
			{
				a = jObject["TimeType"].ToString();
			}
			int pageIndex = 1;
			if (jObject["PageIndex"] != null)
			{
				pageIndex = Convert.ToInt32(jObject["PageIndex"].ToString());
			}
			int pageSize = 10;
			if (jObject["PageSize"] != null)
			{
				pageSize = Convert.ToInt32(jObject["PageSize"].ToString());
			}
			OrderQuery orderQuery = new OrderQuery();
			switch (text)
			{
			case "JH_01":
				orderQuery.Status = OrderStatus.WaitBuyerPay;
				break;
			case "JH_02":
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
				break;
			case "JH_03":
				orderQuery.Status = OrderStatus.SellerAlreadySent;
				break;
			case "JH_04":
				orderQuery.Status = OrderStatus.Finished;
				break;
			case "JH_05":
				orderQuery.Status = OrderStatus.Closed;
				break;
			case "JH_99":
				orderQuery.Status = OrderStatus.All;
				break;
			}
			if (a == "JH_02")
			{
				if (!string.IsNullOrEmpty(text3) && !string.IsNullOrEmpty(text4))
				{
					orderQuery.StartDate = DateTime.Parse(text3);
					orderQuery.EndDate = DateTime.Parse(text4);
				}
			}
			else if (!(a == "JH_01"))
			{
				goto IL_0270;
			}
			goto IL_0270;
			IL_0270:
			orderQuery.PageIndex = pageIndex;
			orderQuery.PageSize = pageSize;
			orderQuery.SupplierId = 0;
			if (!string.IsNullOrEmpty(text2) && text2 != "")
			{
				orderQuery.OrderId = text2;
			}
			DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
			DataTable data = orders.Data;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"code\":\"10000\"");
			stringBuilder.Append(",\"message\":\"SUCCESS\"");
			if (data != null)
			{
				stringBuilder.Append(",\"numtotalorder\":\"" + data.Rows.Count + "\"");
				stringBuilder.Append(",\"orders\":");
				stringBuilder.Append("[");
				for (int i = 0; i < data.Rows.Count; i++)
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(data.Rows[i]["OrderId"].ToString());
					stringBuilder.Append("{");
					stringBuilder.Append("\"PlatOrderNo\":\"" + orderInfo.OrderId + "\"");
					string text5 = "";
					string text6 = "";
					OrderStatus orderStatus = orderInfo.OrderStatus;
					if (orderStatus.ToString() == "1")
					{
						text5 = "JH_01";
						text6 = "等待买家付款";
					}
					else
					{
						orderStatus = orderInfo.OrderStatus;
						if (orderStatus.ToString() == "2")
						{
							text5 = "JH_02";
							text6 = "等待卖家发货";
						}
						else
						{
							orderStatus = orderInfo.OrderStatus;
							if (orderStatus.ToString() == "3")
							{
								text5 = "JH_03";
								text6 = "等待买家确认收货";
							}
							else
							{
								orderStatus = orderInfo.OrderStatus;
								if (orderStatus.ToString() == "4")
								{
									text5 = "JH_05";
									text6 = "交易关闭";
								}
								else
								{
									orderStatus = orderInfo.OrderStatus;
									if (orderStatus.ToString() == "5")
									{
										text5 = "JH_04";
										text6 = "交易完成";
									}
									else
									{
										text5 = "JH_99";
										text6 = "其他";
									}
								}
							}
						}
					}
					stringBuilder.Append(",\"tradeStatus\":\"" + text5 + "\"");
					stringBuilder.Append(",\"tradeStatusdescription\":\"" + text6 + "\"");
					StringBuilder stringBuilder2 = stringBuilder;
					DateTime dateTime = orderInfo.OrderDate;
					stringBuilder2.Append(",\"tradetime\":\"" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
					stringBuilder.Append(",\"payorderno\":\"\"");
					stringBuilder.Append(",\"country\":\"CN\"");
					string[] array = orderInfo.ShippingRegion.Replace('，', ',').Split(',');
					string str = "";
					if (array.Length != 0)
					{
						str = array[0];
					}
					string str2 = "";
					if (array.Length > 1)
					{
						str2 = array[1];
					}
					string str3 = "";
					if (array.Length > 2)
					{
						str3 = array[2];
					}
					string str4 = "";
					if (array.Length > 3)
					{
						str4 = array[3];
					}
					stringBuilder.Append(",\"province\":\"" + str + "\"");
					stringBuilder.Append(",\"city\":\"" + str2 + "\"");
					stringBuilder.Append(",\"area\":\"" + str3 + "\"");
					stringBuilder.Append(",\"town\":\"" + str4 + "\"");
					stringBuilder.Append(",\"address\":\"" + orderInfo.Address + "\"");
					stringBuilder.Append(",\"zip\":\"" + orderInfo.ZipCode + "\"");
					stringBuilder.Append(",\"phone\":\"" + orderInfo.TelPhone + "\"");
					stringBuilder.Append(",\"mobile\":\"" + orderInfo.CellPhone + "\"");
					stringBuilder.Append(",\"email\":\"" + orderInfo.EmailAddress + "\"");
					stringBuilder.Append(",\"customerremark\":\"" + orderInfo.Remark + "\"");
					stringBuilder.Append(",\"sellerremark\":\"" + orderInfo.ManagerRemark + "\"");
					stringBuilder.Append(",\"postfee\":\"" + orderInfo.AdjustedFreight + "\"");
					stringBuilder.Append(",\"goodsfee\":\"" + orderInfo.GetTotal(false) + "\"");
					stringBuilder.Append(",\"totalmoney\":\"" + orderInfo.GetTotal(false) + "\"");
					stringBuilder.Append(",\"favourablemoney\":\"" + orderInfo.ReducedPromotionAmount + "\"");
					stringBuilder.Append(",\"commissionvalue\":\"\"");
					stringBuilder.Append(",\"taxamount\":\"\"");
					stringBuilder.Append(",\"tariffamount\":\"\"");
					stringBuilder.Append(",\"addedvalueamount\":\"" + orderInfo.Tax + "\"");
					stringBuilder.Append(",\"consumptiondutyamount\":\"\"");
					stringBuilder.Append(",\"sendstyle\":\"" + orderInfo.ModeName + "\"");
					stringBuilder.Append(",\"qq\":\"" + orderInfo.QQ + "\"");
					StringBuilder stringBuilder3 = stringBuilder;
					dateTime = orderInfo.PayDate;
					stringBuilder3.Append(",\"paytime\":\"" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
					stringBuilder.Append(",\"invoicetitle\":\"" + orderInfo.InvoiceTitle + "\"");
					stringBuilder.Append(",\"taxpayerident\":\"" + orderInfo.InvoiceTaxpayerNumber + "\"");
					stringBuilder.Append(",\"codservicefee\":\"\"");
					stringBuilder.Append(",\"cardtype\":\"JH_01\"");
					stringBuilder.Append(",\"idcard\":\"" + orderInfo.IDNumber + "\"");
					stringBuilder.Append(",\"idcardtruename\":\"\"");
					stringBuilder.Append(",\"receivername\":\"" + orderInfo.ShipTo + "\"");
					stringBuilder.Append(",\"nick\":\"" + orderInfo.Username + "\"");
					stringBuilder.Append(",\"whsecode\":\"\"");
					stringBuilder.Append(",\"IsHwgFlag\":\"0\"");
					stringBuilder.Append(",\"ShouldPayType\":\"" + orderInfo.PaymentType + "\"");
					stringBuilder.Append(",\"goodinfos\":");
					if (orderInfo.LineItems.Values.Count > 0)
					{
						stringBuilder.Append("[");
						int num = 0;
						foreach (LineItemInfo value2 in orderInfo.LineItems.Values)
						{
							num++;
							stringBuilder.Append("{");
							stringBuilder.Append("\"ProductId\":\"" + value2.SkuId + "\"");
							stringBuilder.Append(",\"suborderno\":\"\"");
							stringBuilder.Append(",\"tradegoodsno\":\"" + value2.SKU + "\"");
							stringBuilder.Append(",\"tradegoodsname\":\"" + value2.ItemDescription + "\"");
							stringBuilder.Append(",\"tradegoodsspec\":\"" + value2.SKUContent + "\"");
							stringBuilder.Append(",\"goodscount\":\"" + value2.Quantity + "\"");
							stringBuilder.Append(",\"price\":\"" + value2.ItemListPrice + "\"");
							stringBuilder.Append(",\"discountmoney\":\"\"");
							stringBuilder.Append(",\"taxamount\":\"\"");
							string text7 = "";
							LineItemStatus status = value2.Status;
							if (status.ToString() == "0")
							{
								text7 = "JH_07";
							}
							else
							{
								status = value2.Status;
								if (status.ToString() == "10")
								{
									text7 = "JH_02";
								}
								else
								{
									status = value2.Status;
									if (status.ToString() == "32")
									{
										text7 = "JH_03";
									}
									else
									{
										status = value2.Status;
										if (status.ToString() == "12")
										{
											text7 = "JH_04";
										}
										else
										{
											status = value2.Status;
											text7 = ((!(status.ToString() == "11")) ? "JH_99" : "JH_06");
										}
									}
								}
							}
							stringBuilder.Append(",\"refundStatus\":\"" + text7 + "\"");
							stringBuilder.Append(",\"Status\":\"\"");
							stringBuilder.Append(",\"remark\":\"\"");
							if (num == orderInfo.LineItems.Values.Count)
							{
								stringBuilder.Append("}");
							}
							else
							{
								stringBuilder.Append("},");
							}
						}
						stringBuilder.Append("]");
					}
					if (i + 1 == data.Rows.Count)
					{
						stringBuilder.Append("}");
					}
					else
					{
						stringBuilder.Append("},");
					}
				}
				stringBuilder.Append("]");
			}
			else
			{
				stringBuilder.Append(",\"numtotalorder\":\"0\"");
				stringBuilder.Append(",\"orders\":\"\"");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		public string CheckRefundStatus(HttpContext context)
		{
			try
			{
				string value = context.Request["bizcontent"];
				JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
				RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
				if (jObject["OrderId"] != null)
				{
					refundApplyQuery.OrderId = Globals.UrlDecode(jObject["OrderId"].ToString());
				}
				PageModel<RefundModel> refundApplys = OrderHelper.GetRefundApplys(refundApplyQuery);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{");
				if (refundApplys.Models.Count() > 0)
				{
					foreach (RefundModel model in refundApplys.Models)
					{
						string text = "";
						string text2 = "";
						RefundStatus handleStatus = model.HandleStatus;
						if (handleStatus.ToString() == "1")
						{
							text = "JH_01";
							text2 = "已申请";
						}
						else
						{
							handleStatus = model.HandleStatus;
							if (handleStatus.ToString() == "2")
							{
								text = "JH_06";
								text2 = "已退款";
							}
							else
							{
								handleStatus = model.HandleStatus;
								if (handleStatus.ToString() == "3")
								{
									text = "JH_04";
									text2 = "拒绝申请";
								}
								else
								{
									text = "JH_99";
									text2 = "其他";
								}
							}
						}
						stringBuilder.Append("\"refundStatus\":\"" + text + "\"");
						stringBuilder.Append(",\"refundStatusdescription\":\"" + text2 + "\"");
						stringBuilder.Append(",\"childrenrefundStatus\":\"\"");
						stringBuilder.Append(",\"code\":\"10000\"");
						stringBuilder.Append(",\"message\":\"SUCCESS\"");
						stringBuilder.Append(",\"submessage\":\"退款成功\"");
					}
				}
				else
				{
					stringBuilder.Append("\"refundStatus\":\"JH_99\"");
					stringBuilder.Append(",\"refundStatusdescription\":\"没有退款\"");
					stringBuilder.Append(",\"childrenrefundStatus\":\"\"");
					stringBuilder.Append(",\"code\":\"40000\"");
					stringBuilder.Append(",\"message\":\"System Error\"");
					stringBuilder.Append(",\"submessage\":\"没有退款\"");
				}
				stringBuilder.Append("}");
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append("{");
				stringBuilder2.Append("\"code\":\"40000\"");
				stringBuilder2.Append(",\"message\":\"System Error\"");
				stringBuilder2.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
				stringBuilder2.Append(",\"submessage\":\"" + ex.Message + "\"");
				stringBuilder2.Append("}");
				return stringBuilder2.ToString();
			}
		}

		public string Send(HttpContext context)
		{
			try
			{
				string value = context.Request["bizcontent"];
				JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
				string text = null;
				if (jObject["SendType"] != null)
				{
					text = jObject["SendType"].ToString();
				}
				string text2 = null;
				if (jObject["LogisticName"] != null)
				{
					text2 = jObject["LogisticName"].ToString();
				}
				string text3 = null;
				if (jObject["LogisticType"] != null)
				{
					text3 = jObject["LogisticType"].ToString();
				}
				string text4 = null;
				if (jObject["LogisticNo"] != null)
				{
					text4 = jObject["LogisticNo"].ToString();
				}
				string text5 = null;
				if (jObject["PlatOrderNo"] != null)
				{
					text5 = jObject["PlatOrderNo"].ToString();
				}
				string text6 = null;
				if (jObject["IsSplit"] != null)
				{
					text6 = jObject["IsSplit"].ToString();
				}
				string text7 = null;
				if (jObject["SubPlatOrderNo"] != null)
				{
					text7 = jObject["SubPlatOrderNo"].ToString();
				}
				string text8 = null;
				if (jObject["SenderName"] != null)
				{
					text8 = jObject["SenderName"].ToString();
				}
				string text9 = null;
				if (jObject["SenderTel"] != null)
				{
					text9 = jObject["SenderTel"].ToString();
				}
				string text10 = null;
				if (jObject["SenderAddress"] != null)
				{
					text10 = jObject["SenderAddress"].ToString();
				}
				string text11 = null;
				if (jObject["IsHwgFlag"] != null)
				{
					text11 = jObject["IsHwgFlag"].ToString();
				}
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("OrderId", text5);
				dictionary.Add("SndStyle", text2);
				dictionary.Add("BillID", text4);
				dictionary.Add("ErrorMsg", "");
				if (text5.IndexOf(',') > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{");
					stringBuilder.Append("\"code\":\"40000\"");
					stringBuilder.Append(",\"message\":\"System Error\"");
					stringBuilder.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder.Append(",\"submessage\":\"不支持合并发货，请选择单个订单\"");
					stringBuilder.Append("}");
					return stringBuilder.ToString();
				}
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(text5);
				if (orderInfo == null)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append("{");
					stringBuilder2.Append("\"code\":\"40000\"");
					stringBuilder2.Append(",\"message\":\"System Error\"");
					stringBuilder2.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder2.Append(",\"submessage\":\"未找到此订单\"");
					stringBuilder2.Append("}");
					return stringBuilder2.ToString();
				}
				if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenSendGoods(orderInfo) && !OrderHelper.CheckStock(orderInfo))
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder3.Append("{");
					stringBuilder3.Append("\"code:\"40000\"");
					stringBuilder3.Append(",\"message:\"System Error\"");
					stringBuilder3.Append(",\"subcode:\"GSE.SYSTEM_ERROR\"");
					stringBuilder3.Append(",\"submessage:\"订单有商品库存不足,请补充库存后发货！\"");
					stringBuilder3.Append("}");
					return stringBuilder3.ToString();
				}
				if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
				{
					StringBuilder stringBuilder4 = new StringBuilder();
					stringBuilder4.Append("{");
					stringBuilder4.Append("\"code\":\"40000\"");
					stringBuilder4.Append(",\"message\":\"System Error\"");
					stringBuilder4.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder4.Append(",\"submessage\":\"当前订单为团购订单，团购活动还未成功结束，所以不能发货！\"");
					stringBuilder4.Append("}");
					return stringBuilder4.ToString();
				}
				if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
				{
					StringBuilder stringBuilder5 = new StringBuilder();
					stringBuilder5.Append("{");
					stringBuilder5.Append("\"code\":\"40000\"");
					stringBuilder5.Append(",\"message\":\"System Error\"");
					stringBuilder5.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder5.Append(",\"submessage\":\"当前订单状态没有付款或不是等待发货的订单，所以不能发货！\"");
					stringBuilder5.Append("}");
					return stringBuilder5.ToString();
				}
				if (string.IsNullOrEmpty(text4.Trim()) || text4.Trim().Length > 20)
				{
					StringBuilder stringBuilder6 = new StringBuilder();
					stringBuilder6.Append("{");
					stringBuilder6.Append("\"code\":\"40000\"");
					stringBuilder6.Append(",\"message\":\"System Error\"");
					stringBuilder6.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder6.Append(",\"submessage\":\"运单号码不能为空，在1至20个字符之间！\"");
					stringBuilder6.Append("}");
					return stringBuilder6.ToString();
				}
				ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNodeLikeName(text2);
				if (expressCompanyInfo != null)
				{
					orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
					orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
				}
				else
				{
					orderInfo.ExpressCompanyAbb = "";
					orderInfo.ExpressCompanyName = text2;
				}
				orderInfo.ShipOrderNumber = text4;
				if (!string.IsNullOrEmpty(orderInfo.OuterOrderId) && orderInfo.OuterOrderId.StartsWith("jd_") && (expressCompanyInfo == null || string.IsNullOrWhiteSpace(expressCompanyInfo.JDCode)))
				{
					StringBuilder stringBuilder7 = new StringBuilder();
					stringBuilder7.Append("{");
					stringBuilder7.Append("\"code\":\"40000\"");
					stringBuilder7.Append(",\"message\":\"System Error\"");
					stringBuilder7.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder7.Append(",\"submessage\":\"此订单是京东订单，所选物流公司不被京东支持！\"");
					stringBuilder7.Append("}");
					return stringBuilder7.ToString();
				}
				if (OrderHelper.SendAPIGoods(orderInfo, true))
				{
					string text12 = "";
					if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
					{
						try
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, "", "", "");
							DeliverInfo deliverInfo = new DeliverInfo();
							deliverInfo.TransId = orderInfo.GatewayOrderId;
							deliverInfo.OutTradeNo = orderInfo.OrderId;
							MemberOpenIdInfo memberOpenIdInfo = Users.GetUser(orderInfo.UserId).MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
							if (memberOpenIdInfo != null)
							{
								deliverInfo.OpenId = memberOpenIdInfo.OpenId;
							}
							payClient.DeliverNotify(deliverInfo);
						}
						catch (Exception ex)
						{
							dictionary["ErrrorMsg"] = "同步微信支付发货状态失败";
							Globals.WriteExceptionLog(ex, dictionary, "APISendGoods");
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
						{
							try
							{
								PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
								if (paymentMode != null && !string.IsNullOrEmpty(paymentMode.Settings))
								{
									string hIGW = paymentMode.Gateway.Replace(".", "_");
									PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.PayOrderId, orderInfo.GetTotal(false), "订单发货", "订单号-" + orderInfo.PayOrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(""), Globals.FullPath(RouteConfig.GetRouteUrl(HttpContext.Current, "PaymentReturn_url", new
									{
										HIGW = hIGW
									})), Globals.FullPath(RouteConfig.GetRouteUrl(HttpContext.Current, "PaymentNotify_url", new
									{
										HIGW = hIGW
									})), "");
									paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
								}
							}
							catch (Exception ex2)
							{
								dictionary["ErrrorMsg"] = "同步支付接口发货状态失败";
								Globals.WriteExceptionLog(ex2, dictionary, "APISendGoods");
							}
						}
						if (!string.IsNullOrEmpty(orderInfo.OuterOrderId))
						{
							if (orderInfo.OuterOrderId.StartsWith("tb_"))
							{
								string text13 = orderInfo.OuterOrderId.Replace("tb_", "");
								try
								{
									string requestUriString = $"http://order2.kuaidiangtong.com/UpdateShipping.ashx?tid={text13}&companycode={expressCompanyInfo.TaobaoCode}&outsid={orderInfo.ShipOrderNumber}& Host ={HiContext.Current.SiteUrl}";
									WebRequest webRequest = WebRequest.Create(requestUriString);
									webRequest.GetResponse();
								}
								catch (Exception ex3)
								{
									dictionary["ErrrorMsg"] = "同步淘宝发货状态失败";
									Globals.WriteExceptionLog(ex3, dictionary, "APISendGoods");
								}
							}
							else if (orderInfo.OuterOrderId.StartsWith("jd_"))
							{
								string text13 = orderInfo.OuterOrderId.Replace("jd_", "");
								try
								{
									SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
									JDHelper.JDOrderOutStorage(masterSettings2.JDAppKey, masterSettings2.JDAppSecret, masterSettings2.JDAccessToken, expressCompanyInfo.JDCode, orderInfo.ShipOrderNumber, text13);
								}
								catch (Exception ex4)
								{
									dictionary["ErrrorMsg"] = "同步京东发货失败";
									Globals.WriteExceptionLog(ex4, dictionary, "APISendGoods");
									text12 = $"同步京东发货失败，京东订单号：{text13}，{ex4.Message}\r\n";
								}
							}
						}
					}
					MemberInfo user = Users.GetUser(orderInfo.UserId);
					Messenger.OrderShipping(orderInfo, user);
					orderInfo.OnDeliver();
					StringBuilder stringBuilder8 = new StringBuilder();
					stringBuilder8.Append("{");
					stringBuilder8.Append("\"code\":\"10000\"");
					stringBuilder8.Append(",\"message\":\"SUCCESS\"");
					stringBuilder8.Append("}");
					return stringBuilder8.ToString();
				}
				StringBuilder stringBuilder9 = new StringBuilder();
				stringBuilder9.Append("{");
				stringBuilder9.Append("\"code\":\"40000\"");
				stringBuilder9.Append(",\"message\":\"System Error\"");
				stringBuilder9.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
				stringBuilder9.Append(",\"submessage\":\"发货失败,可能是商品库存不足,订单中有商品正在退货、换货状态！\"");
				stringBuilder9.Append("}");
				return stringBuilder9.ToString();
			}
			catch (Exception ex5)
			{
				StringBuilder stringBuilder10 = new StringBuilder();
				stringBuilder10.Append("{");
				stringBuilder10.Append("\"code\":\"40000\"");
				stringBuilder10.Append(",\"message\":\"System Error\"");
				stringBuilder10.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
				stringBuilder10.Append(",\"submessage\":\"" + ex5.Message + "\"");
				stringBuilder10.Append("}");
				return stringBuilder10.ToString();
			}
		}

		public string UpdateSellerMemo(HttpContext context)
		{
			try
			{
				string value = context.Request["bizcontent"];
				JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
				string text = null;
				if (jObject["PlatOrderNo"] != null)
				{
					text = jObject["PlatOrderNo"].ToString();
				}
				if (text == null || text == "")
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{");
					stringBuilder.Append("\"code\":\"40000\"");
					stringBuilder.Append(",\"message\":\"System Error\"");
					stringBuilder.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder.Append(",\"submessage\":\"没有此订单号!\"");
					stringBuilder.Append("}");
					return stringBuilder.ToString();
				}
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append("{");
					stringBuilder2.Append("\"code\":\"40000\"");
					stringBuilder2.Append(",\"message\":\"System Error\"");
					stringBuilder2.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder2.Append(",\"submessage\":\"没有此订单号!\"");
					stringBuilder2.Append("}");
					return stringBuilder2.ToString();
				}
				orderInfo.OrderId = text;
				if (jObject["SellerMemo"] != null)
				{
					orderInfo.ManagerRemark = jObject["SellerMemo"].ToString();
				}
				string text2 = null;
				if (jObject["SellerFlag"] != null)
				{
					text2 = jObject["SellerFlag"].ToString();
					if (text2 == "JH_Gray")
					{
						orderInfo.ManagerMark = OrderMark.Gray;
					}
					else if (text2 == "JH_Red")
					{
						orderInfo.ManagerMark = OrderMark.Red;
					}
					else if (text2 == "JH_Yellow")
					{
						orderInfo.ManagerMark = OrderMark.Yellow;
					}
					else if (text2 == "JH_Green")
					{
						orderInfo.ManagerMark = OrderMark.Green;
					}
					else
					{
						orderInfo.ManagerMark = OrderMark.Draw;
					}
				}
				StringBuilder stringBuilder3 = new StringBuilder();
				if (OrderHelper.SaveRemark(orderInfo))
				{
					stringBuilder3.Append("{");
					stringBuilder3.Append("\"code\":\"10000\"");
					stringBuilder3.Append(",\"message\":\"SUCCESS\"");
					stringBuilder3.Append("}");
				}
				else
				{
					stringBuilder3.Append("{");
					stringBuilder3.Append("\"code\":\"40000\"");
					stringBuilder3.Append(",\"message\":\"System Error\"");
					stringBuilder3.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder3.Append(",\"submessage\":\"保存失败\"");
					stringBuilder3.Append("}");
				}
				return stringBuilder3.ToString();
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder4 = new StringBuilder();
				stringBuilder4.Append("{");
				stringBuilder4.Append("\"code\":\"40000\"");
				stringBuilder4.Append(",\"message\":\"System Error\"");
				stringBuilder4.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
				stringBuilder4.Append(",\"submessage\":\"" + ex.Message + "\"");
				stringBuilder4.Append("}");
				return stringBuilder4.ToString();
			}
		}

		public string DownloadProduct(HttpContext context)
		{
			string value = context.Request["bizcontent"];
			JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
			int pageSize = 0;
			int pageIndex = 0;
			if (jObject["PageSize"] != null)
			{
				pageSize = Convert.ToInt32(jObject["PageSize"].ToString());
			}
			if (jObject["PageIndex"] != null)
			{
				pageIndex = Convert.ToInt32(jObject["PageIndex"].ToString());
			}
			string productCode = null;
			string keywords = null;
			ProductSaleStatus saleStatus = ProductSaleStatus.All;
			if (jObject["Status"] != null)
			{
				saleStatus = ((jObject["Status"].ToString() == "JH_01") ? ProductSaleStatus.OnSale : ((!(jObject["Status"].ToString() == "JH_02")) ? ProductSaleStatus.All : ProductSaleStatus.UnSale));
			}
			if (jObject["ProductId"] != null)
			{
				productCode = jObject["ProductId"].ToString();
			}
			if (jObject["ProductName"] != null)
			{
				keywords = jObject["ProductName"].ToString();
			}
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = keywords,
				ProductCode = productCode,
				PageSize = pageSize,
				PageIndex = pageIndex,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				SaleStatus = saleStatus,
				SupplierId = 0
			};
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			DataTable data = products.Data;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"code\":\"10000\"");
			stringBuilder.Append(",\"message\":\"SUCCESS\"");
			if (data != null)
			{
				stringBuilder.Append(",\"totalcount\":\"" + data.Rows.Count + "\"");
				stringBuilder.Append(",\"goodslist\":");
				stringBuilder.Append("[");
				for (int i = 0; i < data.Rows.Count; i++)
				{
					stringBuilder.Append("{");
					int num = Convert.ToInt32(data.Rows[i]["ProductId"]);
					stringBuilder.Append("\"PlatProductID\":\"" + num + "\"");
					stringBuilder.Append(",\"name\":\"" + data.Rows[i]["ProductName"] + "\"");
					stringBuilder.Append(",\"OuterID\":\"\"");
					stringBuilder.Append(",\"price\":\"" + data.Rows[i]["SalePrice"] + "\"");
					stringBuilder.Append(",\"num\":\"" + data.Rows[i]["Stock"] + "\"");
					stringBuilder.Append(",\"pictureurl\":\"\"");
					stringBuilder.Append(",\"whsecode\":\"\"");
					int num2 = Convert.ToInt32(data.Rows[i]["HasSKU"]);
					if (num2 > 0)
					{
						stringBuilder.Append(",\"skus\":");
						stringBuilder.Append("[");
						DataTable dataTable = ProductBrowser.ApiGetSkusByProductId(num);
						for (int j = 0; j < dataTable.Rows.Count; j++)
						{
							stringBuilder.Append("{");
							stringBuilder.Append("\"SkuID\":\"" + dataTable.Rows[j]["skuId"] + "\"");
							stringBuilder.Append(",\"skuOuterID\":\"" + dataTable.Rows[j]["SKU"] + "\"");
							stringBuilder.Append(",\"skuprice\":\"" + dataTable.Rows[j]["SalePrice"] + "\"");
							stringBuilder.Append(",\"skuQuantity\":\"" + dataTable.Rows[j]["Stock"] + "\"");
							stringBuilder.Append(",\"skuname\":\"\"");
							stringBuilder.Append(",\"skuproperty\":\"\"");
							stringBuilder.Append(",\"skupictureurl\":\"\"");
							if (j + 1 == dataTable.Rows.Count)
							{
								stringBuilder.Append("}");
							}
							else
							{
								stringBuilder.Append("},");
							}
						}
						stringBuilder.Append("]");
					}
					else
					{
						stringBuilder.Append(",\"skus\":");
						stringBuilder.Append("[");
						stringBuilder.Append("{");
						stringBuilder.Append("\"SkuID\":\"\"");
						stringBuilder.Append(",\"skuOuterID\":\"\"");
						stringBuilder.Append(",\"skuprice\":\"\"");
						stringBuilder.Append(",\"skuQuantity\":\"\"");
						stringBuilder.Append(",\"skuname\":\"\"");
						stringBuilder.Append(",\"skuproperty\":\"\"");
						stringBuilder.Append(",\"skupictureurl\":\"\"");
						stringBuilder.Append("}");
						stringBuilder.Append("]");
					}
					if (i + 1 == data.Rows.Count)
					{
						stringBuilder.Append("}");
					}
					else
					{
						stringBuilder.Append("},");
					}
				}
				stringBuilder.Append("]");
			}
			else
			{
				stringBuilder.Append(",\"totalcount\":\"0\"");
				stringBuilder.Append(",\"goodslist\":\"\"");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		public string SyncStock(HttpContext context)
		{
			try
			{
				string value = context.Request["bizcontent"];
				JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
				string text = "";
				if (jObject["PlatProductID"] != null)
				{
					text = jObject["PlatProductID"].ToNullString().Trim();
				}
				string text2 = "";
				if (jObject["SkuID"] != null)
				{
					text2 = jObject["SkuID"].ToNullString().Trim();
				}
				string text3 = "";
				if (jObject["OuterID"] != null)
				{
					text3 = jObject["OuterID"].ToNullString().Trim();
				}
				string text4 = "";
				if (jObject["Quantity"] != null)
				{
					text4 = jObject["Quantity"].ToNullString().Trim();
				}
				string text5 = "";
				if (jObject["OutSkuID"] != null)
				{
					text5 = jObject["OutSkuID"].ToNullString().Trim();
				}
				string text6 = "";
				if (!string.IsNullOrWhiteSpace(text2))
				{
					text6 = text2;
				}
				else
				{
					text6 = $"{text}_0";
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (string.IsNullOrEmpty(text2) || string.IsNullOrWhiteSpace(text2))
				{
					if (text == null || text == "")
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						stringBuilder2.Append("{");
						stringBuilder2.Append("\"code\":\"40000\"");
						stringBuilder2.Append(",\"message\":\"System Error\"");
						stringBuilder2.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
						stringBuilder2.Append(",\"submessage\":\"商品ID不存在!\"");
						stringBuilder2.Append("}");
						return stringBuilder2.ToString();
					}
					if (text4 == null || text4 == "")
					{
						StringBuilder stringBuilder3 = new StringBuilder();
						stringBuilder3.Append("{");
						stringBuilder3.Append("\"code\":\"40000\"");
						stringBuilder3.Append(",\"message\":\"System Error\"");
						stringBuilder3.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
						stringBuilder3.Append(",\"submessage\":\"数量不能为空!\"");
						stringBuilder3.Append("}");
						return stringBuilder3.ToString();
					}
					if (ProductHelper.UpdateSkuStock(text, text4.Split(',')[0].ToInt(0)))
					{
						ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(text.ToInt(0));
						stringBuilder.Append("{");
						stringBuilder.Append("\"code\":\"10000\"");
						stringBuilder.Append(",\"message\":\"SUCCESS\"");
						stringBuilder.Append(",\"Quantity\":\"" + productSimpleInfo.Stock + "\"");
						stringBuilder.Append("}");
					}
					else
					{
						stringBuilder.Append("{");
						stringBuilder.Append("\"code\":\"40000\"");
						stringBuilder.Append(",\"message\":\"System Error\"");
						stringBuilder.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
						stringBuilder.Append(",\"submessage\":\"更新失败\"");
						stringBuilder.Append("}");
					}
				}
				else
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>();
					string[] array = text4.Split(',');
					string[] array2 = text2.Split(',');
					for (int i = 0; i < array2.Length; i++)
					{
						int num = 0;
						num = ((array.Length > i) ? array[i].ToInt(0) : array[0].ToInt(0));
						dictionary.Add(Globals.StripAllTags(array2[i]), num);
					}
					if (text == null || text == "")
					{
						StringBuilder stringBuilder4 = new StringBuilder();
						stringBuilder4.Append("{");
						stringBuilder4.Append("\"code\":\"40000\"");
						stringBuilder4.Append(",\"message\":\"System Error\"");
						stringBuilder4.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
						stringBuilder4.Append(",\"submessage\":\"商品ID不存在!\"");
						stringBuilder4.Append("}");
						return stringBuilder4.ToString();
					}
					if (ProductHelper.UpdateSkuStock(text, dictionary))
					{
						ProductInfo productSimpleInfo2 = ProductBrowser.GetProductSimpleInfo(text.ToInt(0));
						stringBuilder.Append("{");
						stringBuilder.Append("\"code\":\"10000\"");
						stringBuilder.Append(",\"message\":\"SUCCESS\"");
						stringBuilder.Append(",\"Quantity\":\"" + productSimpleInfo2.Stock + "\"");
						stringBuilder.Append("}");
					}
					else
					{
						stringBuilder.Append("{");
						stringBuilder.Append("\"code\":\"40000\"");
						stringBuilder.Append(",\"message\":\"System Error\"");
						stringBuilder.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
						stringBuilder.Append(",\"submessage\":\"更新失败\"");
						stringBuilder.Append("}");
					}
				}
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder5 = new StringBuilder();
				stringBuilder5.Append("{");
				stringBuilder5.Append("\"code\":\"40000\"");
				stringBuilder5.Append(",\"message\":\"System Error\"");
				stringBuilder5.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
				stringBuilder5.Append(",\"submessage\":\"" + ex.Message + "\"");
				stringBuilder5.Append("}");
				return stringBuilder5.ToString();
			}
		}

		public string GetRefund(HttpContext context)
		{
			string value = context.Request["bizcontent"];
			JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
			int num = 1;
			int num2 = 10;
			if (jObject["PageIndex"] != null)
			{
				num = Convert.ToInt32(jObject["PageIndex"].ToString());
				if (num < 1)
				{
					num = 1;
				}
			}
			if (jObject["PageSize"] != null)
			{
				num2 = Convert.ToInt32(jObject["PageSize"].ToString());
				if (num2 < 1)
				{
					num2 = 10;
				}
			}
			string text = "";
			if (jObject["BeginTime"] != null)
			{
				text = jObject["BeginTime"].ToNullString().Trim();
			}
			string text2 = "";
			if (jObject["EndTime"] != null)
			{
				text2 = jObject["EndTime"].ToNullString().Trim();
			}
			RefundApplyQuery refundApplyQuery = new RefundApplyQuery();
			refundApplyQuery.PageIndex = num;
			refundApplyQuery.PageSize = num2;
			refundApplyQuery.SortBy = "ApplyForTime";
			refundApplyQuery.SortOrder = SortAction.Desc;
			PageModel<RefundModel> refundApplys = OrderHelper.GetRefundApplys(refundApplyQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"code\":\"10000\"");
			stringBuilder.Append(",\"message\":\"SUCCESS\"");
			stringBuilder.Append(",\"IsSuccess\":true");
			stringBuilder.Append(",\"totalcount\":\"" + refundApplys.Total + "\"");
			stringBuilder.Append(",\"refunds\":");
			stringBuilder.Append("[");
			int num3 = 0;
			foreach (RefundModel model in refundApplys.Models)
			{
				num3++;
				stringBuilder.Append("{");
				stringBuilder.Append("\"refundno\":\"" + model.RefundOrderId + "\"");
				stringBuilder.Append(",\"platorderno\":\"" + model.OrderId + "\"");
				stringBuilder.Append(",\"subplatorderno\":\"\"");
				stringBuilder.Append(",\"totalamount\":\"" + model.OrderTotal + "\"");
				stringBuilder.Append(",\"refundamount\":\"" + model.RefundAmount + "\"");
				stringBuilder.Append(",\"payamount\":\"" + model.OrderTotal + "\"");
				stringBuilder.Append(",\"buyernick\":\"" + model.UserName + "\"");
				stringBuilder.Append(",\"sellernick\":\"" + model.Operator + "\"");
				StringBuilder stringBuilder2 = stringBuilder;
				DateTime dateTime = model.ApplyForTime;
				stringBuilder2.Append(",\"createtime\":\"" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = Convert.ToDateTime(model.AgreedOrRefusedTime);
				stringBuilder3.Append(",\"updatetime\":\"" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
				string str = "";
				string str2 = "";
				OrderStatus orderStatus = model.OrderStatus;
				if (orderStatus.ToString() == "1")
				{
					str = "JH_01";
					str2 = "等待买家付款";
				}
				else
				{
					orderStatus = model.OrderStatus;
					if (orderStatus.ToString() == "2")
					{
						str = "JH_02";
						str2 = "等待卖家发货";
					}
					else
					{
						orderStatus = model.OrderStatus;
						if (orderStatus.ToString() == "3")
						{
							str = "JH_03";
							str2 = "等待买家确认收货";
						}
						else
						{
							orderStatus = model.OrderStatus;
							if (orderStatus.ToString() == "4")
							{
								str = "JH_05";
								str2 = "交易关闭";
							}
							else
							{
								orderStatus = model.OrderStatus;
								if (orderStatus.ToString() == "5")
								{
									str = "JH_04";
									str2 = "交易完成";
								}
							}
						}
					}
				}
				stringBuilder.Append(",\"orderstatus\":\"" + str + "\"");
				stringBuilder.Append(",\"orderstatusdesc\":\"" + str2 + "\"");
				string text3 = "";
				string text4 = "";
				RefundStatus handleStatus = model.HandleStatus;
				if (handleStatus.ToString() == "1")
				{
					text3 = "JH_01";
					text4 = "已申请";
				}
				else
				{
					handleStatus = model.HandleStatus;
					if (handleStatus.ToString() == "2")
					{
						text3 = "JH_06";
						text4 = "已退款";
					}
					else
					{
						handleStatus = model.HandleStatus;
						if (handleStatus.ToString() == "3")
						{
							text3 = "JH_04";
							text4 = "拒绝申请";
						}
						else
						{
							text3 = "JH_99";
							text4 = "其他";
						}
					}
				}
				stringBuilder.Append(",\"refundstatus\":\"" + text3 + "\"");
				stringBuilder.Append(",\"refundstatusdesc\":\"" + text4 + "\"");
				stringBuilder.Append(",\"goodsstatus\":\"\"");
				stringBuilder.Append(",\"goodsstatusdesc\":\"\"");
				stringBuilder.Append(",\"hasgoodsreturn\":\"\"");
				stringBuilder.Append(",\"reason\":\"" + model.RefundReason + "\"");
				stringBuilder.Append(",\"desc\":\"" + model.UserRemark + "\"");
				stringBuilder.Append(",\"productname\":\"" + model.ProductName + "\"");
				stringBuilder.Append(",\"productnum\":\"" + model.Quantity + "\"");
				stringBuilder.Append(",\"logisticname\":\"\"");
				stringBuilder.Append(",\"logisticno\":\"\"");
				stringBuilder.Append(",\"sku\":\"\"");
				stringBuilder.Append(",\"outerid\":\"\"");
				if (num3 == refundApplys.Models.Count())
				{
					stringBuilder.Append("}");
				}
				else
				{
					stringBuilder.Append("},");
				}
			}
			stringBuilder.Append("]");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		protected int? GetIntParam(HttpContext context, string name, bool canNull = false)
		{
			string parameter = this.GetParameter(context, name, false);
			int? nullable = canNull ? null : new int?(0);
			int? result = nullable;
			if (!string.IsNullOrWhiteSpace(parameter))
			{
				try
				{
					result = Convert.ToInt32(parameter);
				}
				catch
				{
				}
			}
			return result;
		}

		protected string GetParameter(HttpContext context, string name, bool hasUrlDecode = false)
		{
			string text = "";
			if (context.Request[name] != null)
			{
				text = context.Request[name].ToString();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				RouteData routeData = context.Request.RequestContext.RouteData;
				if (routeData.Values[name] != null)
				{
					text = routeData.Values[name].ToString();
				}
			}
			if (hasUrlDecode && string.IsNullOrWhiteSpace(text))
			{
				text = Globals.UrlDecode(text);
			}
			return text;
		}

		public DateTime? GetHandleTime(RefundModel model)
		{
			DateTime? result = null;
			if (model.HandleStatus == RefundStatus.Refunded || model.HandleStatus == RefundStatus.Refused)
			{
				return model.FinishTime.HasValue ? model.FinishTime : model.AgreedOrRefusedTime;
			}
			return result;
		}

		private string GetStatusText(RefundStatus Status, string exceptionInfo)
		{
			string text = "";
			if (!string.IsNullOrEmpty(exceptionInfo) && Status == RefundStatus.Applied)
			{
				text = "异常";
			}
			return EnumDescription.GetEnumDescription((Enum)(object)Status, 0);
		}

		private string GetOperText(RefundStatus status)
		{
			string result = "处理";
			if (status == RefundStatus.Refused || status == RefundStatus.Refunded)
			{
				result = "详情";
			}
			return result;
		}

		private string CheckSign(string appkey, string token, string method, string bizcontent, string sign, string appSecret)
		{
			try
			{
				SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
				sortedDictionary.Add("method", method);
				sortedDictionary.Add("appkey", appkey);
				sortedDictionary.Add("token", token);
				sortedDictionary.Add("bizcontent", bizcontent);
				string b = this.Sign(sortedDictionary, appSecret);
				if (sign != b)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{");
					stringBuilder.Append("\"code\":\"40000\"");
					stringBuilder.Append(",\"message\":\"签名错误\"");
					stringBuilder.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
					stringBuilder.Append(",\"submessage\":\"签名错误.\"");
					stringBuilder.Append("}");
					return stringBuilder.ToString();
				}
				return "验证成功!";
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append("{");
				stringBuilder2.Append("\"code\":\"40000\"");
				stringBuilder2.Append(",\"message\":\"" + ex.Message + "\"");
				stringBuilder2.Append(",\"subcode\":\"GSE.SYSTEM_ERROR\"");
				stringBuilder2.Append(",\"submessage\":\"" + ex.Message + ".\"");
				stringBuilder2.Append("}");
				return stringBuilder2.ToString();
			}
		}

		private string Sign(SortedDictionary<string, string> dic, string appSecret)
		{
			string text = string.Empty;
			foreach (string key in dic.Keys)
			{
				text = text + key + dic[key];
			}
			return this.MD5((appSecret + text + appSecret).ToLower());
		}

		public string MD5(string data)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder(32);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}
	}
}
