using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Core.Urls;
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
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.wdgj_api
{
	public class API : IHttpHandler
	{
		private string uCode;

		private string mType;

		private string timestamp;

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
			this.timestamp = context.Request["TimeStamp"];
			this.uCode = context.Request["uCode"];
			this.mType = context.Request["mType"];
			string text = context.Request["Sign"];
			if (string.IsNullOrWhiteSpace(this.timestamp) || !this.checkTimeStamp(this.timestamp))
			{
				context.Response.Write("时间戳错误");
				context.Response.End();
			}
			else if (!text.Equals(this.sign(this.uCode, this.mType, HiContext.Current.SiteSettings.CheckCode, this.timestamp)))
			{
				context.Response.Write("签名错误");
				context.Response.End();
			}
			else
			{
				string text2 = this.mType;
				switch (text2)
				{
				default:
					if (text2 == "mSysGoods")
					{
						context.Response.Write(this.adjustQuantity(context));
					}
					break;
				case "mGetGoods":
					context.Response.Write(this.findProduct(context));
					break;
				case "mOrderSearch":
					context.Response.Write(this.getOrderList(context));
					break;
				case "mGetOrder":
					context.Response.Write(this.getOrderDetail(context));
					break;
				case "mSndGoods":
					context.Response.Write(this.sendGoods(context));
					break;
				}
			}
		}

		private string adjustQuantity(HttpContext context)
		{
			string text = context.Request["ItemID"].ToNullString().Trim();
			string text2 = context.Request["SkuID"].ToNullString().Trim();
			string text3 = context.Request["Quantity"].ToNullString().Trim();
			string text4 = "";
			if (!string.IsNullOrWhiteSpace(text2))
			{
				text4 = text2;
			}
			else
			{
				text4 = $"{text}_0";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			stringBuilder.Append("<Rsp>");
			try
			{
				if (string.IsNullOrEmpty(text2) || string.IsNullOrWhiteSpace(text2))
				{
					if (ProductHelper.UpdateSkuStock(text, text3.Split(',')[0].ToInt(0)))
					{
						ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(text.ToInt(0));
						stringBuilder.Append("<Result>1</Result>");
						if (productSimpleInfo != null && productSimpleInfo.SaleStatus == ProductSaleStatus.OnSale)
						{
							stringBuilder.Append("<GoodsType>Onsale</GoodsType>");
						}
						else
						{
							stringBuilder.Append("<GoodsType>InStock</GoodsType>");
						}
						stringBuilder.Append("<Cause></Cause>");
					}
					else
					{
						stringBuilder.Append("<Result>0</Result>");
						stringBuilder.Append("<GoodsType></GoodsType>");
						stringBuilder.Append("<Cause><![CDATA[{修改库存失败}]]></Cause>");
					}
				}
				else
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>();
					string[] array = text3.Split(',');
					string[] array2 = text2.Split(',');
					for (int i = 0; i < array2.Length; i++)
					{
						int num = 0;
						num = ((array.Length > i) ? array[i].ToInt(0) : array[0].ToInt(0));
						dictionary.Add(Globals.StripAllTags(array2[i]), num);
					}
					if (ProductHelper.UpdateSkuStock(text, dictionary))
					{
						ProductInfo productSimpleInfo2 = ProductBrowser.GetProductSimpleInfo(text.ToInt(0));
						stringBuilder.Append("<Result>1</Result>");
						if (productSimpleInfo2 != null && productSimpleInfo2.SaleStatus == ProductSaleStatus.OnSale)
						{
							stringBuilder.Append("<GoodsType>Onsale</GoodsType>");
						}
						else
						{
							stringBuilder.Append("<GoodsType>InStock</GoodsType>");
						}
						stringBuilder.Append("<Cause></Cause>");
					}
					else
					{
						stringBuilder.Append("<Result>0</Result>");
						stringBuilder.Append("<GoodsType></GoodsType>");
						stringBuilder.Append("<Cause><![CDATA[{修改库存失败}]]></Cause>");
					}
				}
			}
			catch (Exception ex)
			{
				NameValueCollection param = new NameValueCollection
				{
					context.Request.QueryString,
					context.Request.Form
				};
				Globals.WriteExceptionLog_Page(ex, param, "adjustQuantity");
			}
			stringBuilder.Append("</Rsp>");
			return stringBuilder.ToString();
		}

		private string sendGoods(HttpContext context)
		{
			string text = context.Request["OrderNO"].Trim();
			string text2 = context.Request["SndStyle"].Trim();
			string text3 = context.Request["BillID"].Trim();
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("OrderId", text);
			dictionary.Add("SndStyle", text2);
			dictionary.Add("BillID", text3);
			dictionary.Add("ErrorMsg", "");
			if (text.IndexOf(',') > 0)
			{
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{不支持合并发货，请选择单个订单}]]></Cause></Rsp>";
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{未找到此订单}]]></Cause></Rsp>";
			}
			if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && OrderHelper.NeedUpdateStockWhenSendGoods(orderInfo) && !OrderHelper.CheckStock(orderInfo))
			{
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{订单有商品库存不足,请补充库存后发货！}]]></Cause></Rsp>";
			}
			if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
			{
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{当前订单为团购订单，团购活动还未成功结束，所以不能发货！}]]></Cause></Rsp>";
			}
			if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{当前订单状态没有付款或不是等待发货的订单，所以不能发货！}]]></Cause></Rsp>";
			}
			if (string.IsNullOrEmpty(text3.Trim()) || text3.Trim().Length > 20)
			{
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{运单号码不能为空，在1至20个字符之间！}]]></Cause></Rsp>";
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
			orderInfo.ShipOrderNumber = text3;
			if (!string.IsNullOrEmpty(orderInfo.OuterOrderId) && orderInfo.OuterOrderId.StartsWith("jd_") && (expressCompanyInfo == null || string.IsNullOrWhiteSpace(expressCompanyInfo.JDCode)))
			{
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{此订单是京东订单，所选物流公司不被京东支持！}]]></Cause></Rsp>";
			}
			if (OrderHelper.SendAPIGoods(orderInfo, true))
			{
				string text4 = "";
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
							string text5 = orderInfo.OuterOrderId.Replace("tb_", "");
							try
							{
								string requestUriString = $"http://order2.kuaidiangtong.com/UpdateShipping.ashx?tid={text5}&companycode={expressCompanyInfo.TaobaoCode}&outsid={orderInfo.ShipOrderNumber}&Host={HiContext.Current.SiteUrl}";
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
							string text5 = orderInfo.OuterOrderId.Replace("jd_", "");
							try
							{
								SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
								JDHelper.JDOrderOutStorage(masterSettings2.JDAppKey, masterSettings2.JDAppSecret, masterSettings2.JDAccessToken, expressCompanyInfo.JDCode, orderInfo.ShipOrderNumber, text5);
							}
							catch (Exception ex4)
							{
								dictionary["ErrrorMsg"] = "同步京东发货失败";
								Globals.WriteExceptionLog(ex4, dictionary, "APISendGoods");
								text4 = $"同步京东发货失败，京东订单号：{text5}，{ex4.Message}\r\n";
							}
						}
					}
				}
				MemberInfo user = Users.GetUser(orderInfo.UserId);
				Messenger.OrderShipping(orderInfo, user);
				orderInfo.OnDeliver();
				return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>1</Result><Cause></Cause></Rsp>";
			}
			return "<?xml version='1.0' encoding='utf-8'?><Rsp><Result>0</Result><Cause><![CDATA[{发货失败,可能是商品库存不足,订单中有商品正在退货、换货状态！}]]></Cause></Rsp>";
		}

		private string getOrderList(HttpContext context)
		{
			int num = 0;
			int num2 = 0;
			num = Convert.ToInt32(context.Request["PageSize"]);
			num2 = Convert.ToInt32(context.Request["Page"]);
			int num3 = Convert.ToInt32(context.Request["OrderStatus"]);
			string text = null;
			string text2 = null;
			if (!string.IsNullOrWhiteSpace(context.Request["Start_Modified"]))
			{
				text = context.Request["Start_Modified"];
			}
			if (!string.IsNullOrWhiteSpace(context.Request["End_Modified"]))
			{
				text2 = context.Request["End_Modified"];
			}
			OrderQuery orderQuery = new OrderQuery();
			switch (num3)
			{
			case 0:
				orderQuery.Status = OrderStatus.WaitBuyerPay;
				goto default;
			case 1:
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
				goto default;
			case -1:
				return "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Order><Result>1</Result><OrderCount>0</OrderCount><OrderList></OrderList></Order>";
			default:
			{
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					orderQuery.StartDate = DateTime.Parse(text);
					orderQuery.EndDate = DateTime.Parse(text2);
				}
				orderQuery.PageIndex = num2;
				orderQuery.PageSize = num;
				orderQuery.SupplierId = 0;
				DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				stringBuilder.Append("<Order>");
				stringBuilder.Append($"<OrderCount>{orders.TotalRecords}</OrderCount>");
				stringBuilder.Append("<Result>1</Result>");
				stringBuilder.Append("<Cause></Cause>");
				stringBuilder.Append($"<Page>{num2}</Page>");
				stringBuilder.Append("<OrderList>");
				DataTable data = orders.Data;
				for (int i = 0; i < data.Rows.Count; i++)
				{
					stringBuilder.Append(string.Format("<OrderNO>{0}</OrderNO>", data.Rows[i]["OrderId"]));
				}
				stringBuilder.Append("</OrderList>");
				stringBuilder.Append("</Order>");
				return stringBuilder.ToString();
			}
			}
		}

		private string getOrderDetail(HttpContext context)
		{
			string text = context.Request["OrderNO"].Trim();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			stringBuilder.Append("<Order>");
			stringBuilder.Append($"<OrderNO>{text}</OrderNO>");
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
			if (orderInfo == null)
			{
				stringBuilder.Append("<Result>0</Result>");
				stringBuilder.Append("<Cause><![CDATA[{订单不存在}]]></Cause>");
			}
			else
			{
				stringBuilder.Append("<Result>1</Result>");
				stringBuilder.Append("<Cause></Cause>");
				switch (orderInfo.OrderStatus)
				{
				case OrderStatus.WaitBuyerPay:
					stringBuilder.Append("<OrderStatus>WAIT_BUYER_PAY</OrderStatus>");
					break;
				case OrderStatus.BuyerAlreadyPaid:
					stringBuilder.Append("<OrderStatus>WAIT_SELLER_SEND_GOODS</OrderStatus>");
					break;
				case OrderStatus.SellerAlreadySent:
					stringBuilder.Append("<OrderStatus>WAIT_BUYER_CONFIRM_GOODS</OrderStatus>");
					break;
				case OrderStatus.Finished:
					stringBuilder.Append("<OrderStatus>TRADE_FINISHED</OrderStatus>");
					break;
				case OrderStatus.Closed:
					stringBuilder.Append("<OrderStatus>TRADE_CLOSED</OrderStatus>");
					break;
				}
				stringBuilder.Append($"<DateTime>{orderInfo.OrderDate}</DateTime>");
				stringBuilder.Append(string.Format("<BuyerID><![CDATA[{0}]]></BuyerID>", string.IsNullOrEmpty(orderInfo.Username) ? "匿名" : orderInfo.Username));
				stringBuilder.Append(string.Format("<BuyerName><![CDATA[{0}]]></BuyerName>", string.IsNullOrEmpty(orderInfo.ShipTo) ? "匿名" : orderInfo.ShipTo));
				stringBuilder.Append("<CardType></CardType>");
				stringBuilder.Append("<IDCard></IDCard>");
				stringBuilder.Append("<Country><![CDATA[{中国}]]></Country>");
				string[] array = orderInfo.ShippingRegion.Replace("，", ",").Split(',');
				if (array.Length >= 3)
				{
					stringBuilder.Append($"<Province><![CDATA[{array[0]}]]></Province>");
					stringBuilder.Append($"<City><![CDATA[{array[1]}]]></City>");
					stringBuilder.Append($"<Town><![CDATA[{array[2]}]]></Town>");
				}
				stringBuilder.Append(string.Format("<Adr><![CDATA[{0}]]></Adr>", orderInfo.ShippingRegion.Replace("，", ",").Replace(",", "") + orderInfo.Address));
				stringBuilder.Append($"<Zip><![CDATA[{orderInfo.ZipCode}]]></Zip>");
				stringBuilder.Append($"<Email><![CDATA[{orderInfo.EmailAddress}]]></Email>");
				stringBuilder.Append($"<Phone><![CDATA[{(string.IsNullOrWhiteSpace(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone)}]]></Phone>");
				stringBuilder.Append($"<Total>{orderInfo.GetTotal(false)}</Total>");
				stringBuilder.Append("<Currency>CNY</Currency>");
				stringBuilder.Append($"<Postage>{orderInfo.Freight}</Postage>");
				stringBuilder.Append($"<PayAccount>{orderInfo.PaymentType}</PayAccount>");
				stringBuilder.Append($"<PayID>{orderInfo.GatewayOrderId}</PayID>");
				stringBuilder.Append($"<LogisticsName><![CDATA[{orderInfo.ExpressCompanyName}]]></LogisticsName>");
				string gateway = orderInfo.Gateway;
				if (gateway.IndexOf("podrequest") > 0)
				{
					stringBuilder.Append("<Chargetype>货到付款</Chargetype>");
				}
				else if (gateway.IndexOf("advancerequest") > 0)
				{
					stringBuilder.Append("<Chargetype>客户预存款</Chargetype>");
				}
				else if (gateway.IndexOf("bank") > 0)
				{
					stringBuilder.Append("<Chargetype>银行收款</Chargetype>");
				}
				else
				{
					stringBuilder.Append("<Chargetype>担保交易</Chargetype>");
				}
				stringBuilder.Append($"<CustomerRemark><![CDATA[{orderInfo.Remark}]]></CustomerRemark>");
				stringBuilder.Append($"<InvoiceTitle><![CDATA[{orderInfo.InvoiceTitle}]]></InvoiceTitle>");
				stringBuilder.Append($"<Remark><![CDATA[{orderInfo.ManagerRemark}]]></Remark>");
				stringBuilder.AppendFormat("<orders_discount_fee>{0}</orders_discount_fee>", ((orderInfo.IsReduced ? orderInfo.ReducedPromotionAmount : decimal.Zero) + orderInfo.CouponValue + (orderInfo.DeductionMoney.HasValue ? orderInfo.DeductionMoney.Value : decimal.Zero)).F2ToString("f2"));
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					stringBuilder.Append("<Item>");
					stringBuilder.Append($"<GoodsID>{value.SKU}</GoodsID>");
					stringBuilder.Append($"<GoodsName><![CDATA[{value.ItemDescription}]]></GoodsName>");
					stringBuilder.Append($"<GoodsSpec><![CDATA[{value.SKUContent}]]></GoodsSpec>");
					if (value.Status == LineItemStatus.RefundApplied || orderInfo.OrderStatus == OrderStatus.ApplyForRefund)
					{
						stringBuilder.Append("<GoodsStatus>WAIT_SELLER_AGREE</GoodsStatus>");
					}
					else if (value.Status == LineItemStatus.MerchantsAgreedForReturn)
					{
						stringBuilder.Append("<GoodsStatus>WAIT_BUYER_RETURN_GOODS</GoodsStatus>");
					}
					else if (value.Status == LineItemStatus.DeliveryForReturn)
					{
						stringBuilder.Append("<GoodsStatus>WAIT_SELLER_CONFIRM_GOODS</GoodsStatus>");
					}
					else if (value.Status == LineItemStatus.RefundRefused)
					{
						stringBuilder.Append("<GoodsStatus>SELLER_REFUSE_BUYER</GoodsStatus>");
					}
					else if (value.Status == LineItemStatus.Refunded || orderInfo.OrderStatus == OrderStatus.Refunded)
					{
						stringBuilder.Append("<GoodsStatus>SUCCESS</GoodsStatus>");
					}
					else
					{
						stringBuilder.Append("<GoodsStatus></GoodsStatus>");
					}
					stringBuilder.Append($"<Count>{value.ShipmentQuantity}</Count>");
					stringBuilder.Append($"<Price>{value.ItemAdjustedPrice}</Price>");
					stringBuilder.Append("<Tax>0</Tax>");
					stringBuilder.Append("</Item>");
				}
			}
			stringBuilder.Append("</Order>");
			return stringBuilder.ToString();
		}

		private string findProduct(HttpContext context)
		{
			int num = 0;
			int num2 = 0;
			num = Convert.ToInt32(context.Request["PageSize"]);
			num2 = Convert.ToInt32(context.Request["Page"]);
			string text = null;
			string productCode = null;
			string keywords = null;
			ProductSaleStatus saleStatus = ProductSaleStatus.All;
			if (!string.IsNullOrWhiteSpace(context.Request["GoodsType"]))
			{
				text = context.Request["GoodsType"];
				if (text.Equals("Onsale"))
				{
					saleStatus = ProductSaleStatus.OnSale;
				}
				else if (text.Equals("InStock"))
				{
					saleStatus = ProductSaleStatus.OnStock;
				}
			}
			if (!string.IsNullOrWhiteSpace(context.Request["OuterID"]))
			{
				productCode = context.Request["OuterID"];
			}
			else if (!string.IsNullOrWhiteSpace(context.Request["GoodsName"]))
			{
				keywords = context.Request["GoodsName"];
			}
			ProductQuery productQuery = new ProductQuery
			{
				Keywords = keywords,
				ProductCode = productCode,
				PageSize = num,
				PageIndex = num2,
				SortOrder = SortAction.Desc,
				SortBy = "DisplaySequence",
				SaleStatus = saleStatus,
				SupplierId = 0
			};
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = ProductHelper.GetProducts(productQuery);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			stringBuilder.Append("<Goods>");
			stringBuilder.Append($"<TotalCount>{products.TotalRecords}</TotalCount>");
			stringBuilder.Append("<Result>1</Result>");
			stringBuilder.Append("<Cause></Cause>");
			DataTable data = products.Data;
			for (int i = 0; i < data.Rows.Count; i++)
			{
				stringBuilder.Append("<Ware>");
				int num3 = Convert.ToInt32(data.Rows[i]["ProductId"]);
				int num4 = Convert.ToInt32(data.Rows[i]["HasSKU"]);
				stringBuilder.Append($"<ItemID>{num3}</ItemID>");
				stringBuilder.Append(string.Format("<ItemName><![CDATA[{0}]]></ItemName>", data.Rows[i]["ProductName"]));
				stringBuilder.Append(string.Format("<OuterID><![CDATA[{0}]]></OuterID>", data.Rows[i]["ProductCode"]));
				stringBuilder.Append(string.Format("<Price>{0}</Price>", data.Rows[i]["SalePrice"]));
				stringBuilder.Append(string.Format("<Num>{0}</Num>", data.Rows[i]["Stock"]));
				stringBuilder.Append($"<IsSku>{num4}</IsSku>");
				stringBuilder.Append("<Items>");
				if (num4 > 0)
				{
					DataTable dataTable = ProductBrowser.ApiGetSkusByProductId(num3);
					for (int j = 0; j < dataTable.Rows.Count; j++)
					{
						stringBuilder.Append("<Item>");
						stringBuilder.Append(string.Format("<Unit>{0}</Unit>", dataTable.Rows[j]["valuestr"]));
						stringBuilder.Append(string.Format("<SkuID>{0}</SkuID>", dataTable.Rows[j]["skuId"]));
						stringBuilder.Append(string.Format("<Num>{0}</Num>", dataTable.Rows[j]["Stock"]));
						stringBuilder.Append(string.Format("<SkuOuterID>{0}</SkuOuterID>", dataTable.Rows[j]["SKU"]));
						stringBuilder.Append(string.Format("<SkuPrice>{0}</SkuPrice>", dataTable.Rows[j]["SalePrice"]));
						stringBuilder.Append("</Item>");
					}
				}
				stringBuilder.Append("</Items>");
				stringBuilder.Append("</Ware>");
			}
			stringBuilder.Append("</Goods>");
			return stringBuilder.ToString();
		}

		private bool checkTimeStamp(string timeStamp)
		{
			long num = Convert.ToInt64(this.timestamp);
			DateTime utcNow = DateTime.UtcNow;
			return (utcNow - utcNow.UnixTimestampToDateTime(num)).TotalMinutes < 15.0;
		}

		private string sign(string uCode, string mType, string Secret, string TimeStamp)
		{
			string s = string.Format("{0}mType{1}TimeStamp{2}uCode{3}{0}", Secret, mType, TimeStamp, uCode);
			StringBuilder stringBuilder = new StringBuilder(32);
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString().ToUpper();
		}
	}
}
