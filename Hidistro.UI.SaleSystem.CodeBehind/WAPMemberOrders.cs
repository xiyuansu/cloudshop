using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPMemberOrders : WAPMemberTemplatedWebControl
	{
		private Common_MemberCenterOrders rptOrders;

		private HyperLink hyAllOrder;

		private HyperLink hyWaitpayOrder;

		private HyperLink hyAlreadyPayOrder;

		private HyperLink hyWaitReceiveOrder;

		private HyperLink hyFinishedOrder;

		private HyperLink hyWaitTakeOnStore;

		private HtmlGenericControl liWaiteTakeOnStore;

		private HtmlGenericControl liWaiteSend;

		private HtmlGenericControl divToDetail;

		private Common_WAPPaymentTypeSelect paymenttypeselect;

		private HtmlGenericControl spandemo;

		private HtmlInputHidden hidHasProductCommentPoint;

		private HtmlInputHidden hidHasTradePassword;

		private bool isVShop = false;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMemberOrders.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text = HttpContext.Current.Request.Url.ToString().ToLower();
			string userAgent = HttpContext.Current.Request.UserAgent;
			if (text.IndexOf("/vshop/") != -1 || userAgent.ToLower().IndexOf("micromessenger") > 0)
			{
				this.isVShop = true;
			}
			this.hidHasProductCommentPoint = (HtmlInputHidden)this.FindControl("hidHasProductCommentPoint");
			this.hidHasProductCommentPoint.Value = (this.GetProductCommentPoint() ? "1" : "0");
			PageTitle.AddSiteNameTitle("会员订单");
			this.hyAllOrder = (this.FindControl("hyAllOrder") as HyperLink);
			this.hyWaitpayOrder = (this.FindControl("hyWaitpayOrder") as HyperLink);
			this.hyAlreadyPayOrder = (this.FindControl("hyAlreadyPayOrder") as HyperLink);
			this.hyWaitReceiveOrder = (this.FindControl("hyWaitReceiveOrder") as HyperLink);
			this.hyFinishedOrder = (this.FindControl("hyFinishedOrder") as HyperLink);
			this.hyWaitTakeOnStore = (this.FindControl("hyWaitTakeOnStore") as HyperLink);
			this.liWaiteTakeOnStore = (this.FindControl("liWaiteTakeOnStore") as HtmlGenericControl);
			this.liWaiteSend = (this.FindControl("liWaiteSend") as HtmlGenericControl);
			this.hidHasTradePassword = (HtmlInputHidden)this.FindControl("hidHasTradePassword");
			if (masterSettings.OpenMultStore)
			{
				this.liWaiteSend.Visible = false;
				this.liWaiteTakeOnStore.Visible = true;
			}
			else
			{
				this.liWaiteSend.Visible = true;
				this.liWaiteTakeOnStore.Visible = false;
			}
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			this.hidHasTradePassword.Value = (string.IsNullOrWhiteSpace(user.TradePassword) ? "0" : "1");
			this.spandemo = (HtmlGenericControl)this.FindControl("spandemo");
			this.spandemo.Visible = masterSettings.IsDemoSite;
			int num = 0;
			int.TryParse(HttpContext.Current.Request.QueryString.Get("status"), out num);
			OrderQuery orderQuery = new OrderQuery();
			this.ClearCss();
			switch (num)
			{
			case 0:
				this.hyAllOrder.CssClass = "tab_active";
				break;
			case 1:
				orderQuery.Status = OrderStatus.WaitBuyerPay;
				this.hyWaitpayOrder.CssClass = "tab_active";
				break;
			case 3:
				orderQuery.Status = OrderStatus.SellerAlreadySent;
				this.hyWaitReceiveOrder.CssClass = "tab_active";
				break;
			case 2:
				orderQuery.Status = OrderStatus.BuyerAlreadyPaid;
				this.hyAlreadyPayOrder.CssClass = "tab_active";
				break;
			case 21:
				orderQuery.Status = OrderStatus.WaitReview;
				this.hyFinishedOrder.CssClass = "tab_active";
				break;
			case 999:
				orderQuery.TakeOnStore = true;
				this.hyWaitTakeOnStore.CssClass = "tab_active";
				break;
			case -1:
				orderQuery.ItemStatus = 1;
				break;
			}
			orderQuery.ShowGiftOrder = true;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["itemStatus"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["itemStatus"], out value))
				{
					orderQuery.ItemStatus = value;
				}
			}
			this.rptOrders = (Common_MemberCenterOrders)this.FindControl("Common_MemberCenterOrder");
			this.rptOrders.ItemDataBound += this.rptOrders_ItemDataBound;
			this.rptOrders.DataSource = MemberProcessor.GetUserOrder(HiContext.Current.UserId, orderQuery);
			this.rptOrders.DataBind();
		}

		private bool GetProductCommentPoint()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings != null && masterSettings.ProductCommentPoint > 0)
			{
				return true;
			}
			return false;
		}

		private void ClearCss()
		{
			HyperLink hyperLink = this.hyWaitTakeOnStore;
			HyperLink hyperLink2 = this.hyAllOrder;
			HyperLink hyperLink3 = this.hyWaitpayOrder;
			HyperLink hyperLink4 = this.hyAlreadyPayOrder;
			HyperLink hyperLink5 = this.hyWaitReceiveOrder;
			HyperLink hyperLink6 = this.hyFinishedOrder;
			string text2 = hyperLink6.CssClass = "";
			string text4 = hyperLink5.CssClass = text2;
			string text6 = hyperLink4.CssClass = text4;
			string text8 = hyperLink3.CssClass = text6;
			string text11 = hyperLink.CssClass = (hyperLink2.CssClass = text8);
		}

		private void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnCouponCode");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnApplyForRefund");
				HtmlAnchor htmlAnchor3 = (HtmlAnchor)e.Item.FindControl("lnkClose");
				HtmlAnchor htmlAnchor4 = (HtmlAnchor)e.Item.FindControl("lkbtnViewMessage");
				HtmlAnchor htmlAnchor5 = (HtmlAnchor)e.Item.FindControl("lkbtnProductReview");
				Literal literal = (Literal)e.Item.FindControl("ltlOrderItems");
				Literal literal2 = (Literal)e.Item.FindControl("ltlOrderGifts");
				HtmlGenericControl htmlGenericControl = e.Item.FindControl("panelOperaters") as HtmlGenericControl;
				HtmlGenericControl htmlGenericControl2 = e.Item.FindControl("divToDetail") as HtmlGenericControl;
				HtmlGenericControl htmlGenericControl3 = e.Item.FindControl("divOrderStatus") as HtmlGenericControl;
				HtmlGenericControl htmlGenericControl4 = e.Item.FindControl("divOrderError") as HtmlGenericControl;
				HtmlGenericControl htmlGenericControl5 = e.Item.FindControl("divOrderGifts") as HtmlGenericControl;
				HtmlGenericControl htmlGenericControl6 = e.Item.FindControl("divOrderItems") as HtmlGenericControl;
				HtmlGenericControl htmlGenericControl7 = (HtmlGenericControl)e.Item.FindControl("OrderIdSpan");
				HtmlGenericControl htmlGenericControl8 = (HtmlGenericControl)e.Item.FindControl("PayMoneySpan");
				HtmlGenericControl htmlGenericControl9 = (HtmlGenericControl)e.Item.FindControl("TakeCodeDIV");
				HtmlAnchor htmlAnchor6 = (HtmlAnchor)e.Item.FindControl("lnkViewLogistics");
				HtmlAnchor htmlAnchor7 = (HtmlAnchor)e.Item.FindControl("lnkToPay");
				HtmlAnchor htmlAnchor8 = (HtmlAnchor)e.Item.FindControl("lnkHelpLink");
				HtmlAnchor htmlAnchor9 = (HtmlAnchor)e.Item.FindControl("lnkFinishOrder");
				HtmlAnchor htmlAnchor10 = (HtmlAnchor)e.Item.FindControl("lnkViewTakeCodeQRCode");
				HtmlAnchor htmlAnchor11 = (HtmlAnchor)e.Item.FindControl("lnkCertification");
				HtmlGenericControl htmlGenericControl10 = (HtmlGenericControl)e.Item.FindControl("divSendRedEnvelope");
				OrderStatus orderStatus = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				Repeater repeater = (Repeater)e.Item.FindControl("Repeater1");
				Repeater repeater2 = (Repeater)e.Item.FindControl("rptPointGifts");
				this.paymenttypeselect = (Common_WAPPaymentTypeSelect)this.FindControl("paymenttypeselect");
				Literal literal3 = (Literal)e.Item.FindControl("litGiftTitle");
				string text = DataBinder.Eval(e.Item.DataItem, "OrderId").ToString();
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo != null)
				{
					if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid || orderInfo.OrderStatus == OrderStatus.Finished || orderInfo.OrderStatus == OrderStatus.WaitReview || orderInfo.OrderStatus == OrderStatus.History)
					{
						WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
						bool visible = false;
						if (openedWeiXinRedEnvelope != null && openedWeiXinRedEnvelope.EnableIssueMinAmount <= orderInfo.GetPayTotal() && orderInfo.OrderDate >= openedWeiXinRedEnvelope.ActiveStartTime && orderInfo.OrderDate <= openedWeiXinRedEnvelope.ActiveEndTime)
						{
							visible = true;
						}
						if (htmlGenericControl10 != null)
						{
							htmlGenericControl10.Visible = visible;
							if (this.isVShop)
							{
								htmlGenericControl10.InnerHtml = "<a href=\"/vshop/SendRedEnvelope.aspx?OrderId=" + orderInfo.OrderId + "\"></a>";
							}
							else
							{
								htmlGenericControl10.InnerHtml = "";
								string text2 = Globals.HttpsFullPath("/vshop/SendRedEnvelope.aspx?OrderId=" + orderInfo.OrderId);
								htmlGenericControl10.Attributes.Add("onclick", string.Format("ShowMsg('{0}','{1}')", "代金红包请前往微信端领取!", "false"));
							}
						}
					}
					this.paymenttypeselect.ClientType = base.ClientType;
					htmlGenericControl4.Visible = (orderInfo.IsError && orderInfo.CloseReason != "订单已退款完成");
					htmlGenericControl3.Visible = !orderInfo.IsError;
					htmlGenericControl5.Visible = (orderInfo.LineItems.Count() == 0);
					htmlGenericControl6.Visible = (orderInfo.LineItems.Count() > 0);
					htmlAnchor2.HRef = "ApplyRefund.aspx?OrderId=" + text;
					htmlAnchor.Visible = false;
					htmlAnchor.HRef = "MemberOrdersVCode?OrderId=" + text;
					HtmlGenericControl htmlGenericControl11 = (HtmlGenericControl)e.Item.FindControl("OrderSupplierH3");
					string text3 = string.Empty;
					if (htmlGenericControl11 != null)
					{
						text3 = htmlGenericControl11.Attributes["class"];
						text3 = ((!string.IsNullOrEmpty(text3)) ? text3.Replace(" ztitle", "").Replace("stitle", "") : "");
					}
					if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || !(orderInfo.ParentOrderId == "-1") || !orderInfo.OrderId.Contains("P"))
					{
						if (HiContext.Current.SiteSettings.OpenMultStore && orderInfo.StoreId > 0 && !string.IsNullOrWhiteSpace(orderInfo.StoreName))
						{
							htmlGenericControl7.InnerText = orderInfo.StoreName;
							text3 += " mtitle";
						}
						else if (orderInfo.StoreId == 0 && HiContext.Current.SiteSettings.OpenSupplier && orderInfo.SupplierId > 0)
						{
							htmlGenericControl7.InnerText = orderInfo.ShipperName;
							text3 += " stitle";
						}
						else
						{
							htmlGenericControl7.InnerText = "平台";
							text3 += " ztitle";
						}
						htmlGenericControl11.Attributes["class"] = text3;
						if (orderInfo.LineItems.Count <= 0)
						{
							literal3.Text = "（礼）";
						}
					}
					else
					{
						htmlGenericControl7.InnerText = orderInfo.OrderId;
						htmlGenericControl11.Attributes["class"] = text3;
					}
					if (orderInfo.PreSaleId > 0)
					{
						htmlGenericControl8.InnerText = (orderInfo.Deposit + orderInfo.FinalPayment).F2ToString("f2");
					}
					else
					{
						htmlGenericControl8.InnerText = Convert.ToDecimal(DataBinder.Eval(e.Item.DataItem, "OrderTotal")).F2ToString("f2");
					}
					if (htmlGenericControl2 != null)
					{
						if (orderInfo.OrderType == OrderType.ServiceOrder)
						{
							htmlGenericControl2.Attributes.Add("onclick", "window.location.href='ServiceMemberOrderDetails.aspx?orderId=" + orderInfo.OrderId + "'");
						}
						else
						{
							htmlGenericControl2.Attributes.Add("onclick", "window.location.href='MemberOrderDetails.aspx?orderId=" + orderInfo.OrderId + "'");
						}
					}
					if (htmlAnchor6 != null)
					{
						if (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished)
						{
							if (!string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb) && !string.IsNullOrEmpty(orderInfo.ShipOrderNumber))
							{
								htmlAnchor6.HRef = "MyLogistics.aspx?OrderId=" + text;
							}
							else if (orderInfo.ExpressCompanyName == "同城物流配送")
							{
								htmlAnchor6.HRef = "MyLogistics.aspx?OrderId=" + text;
							}
							else
							{
								htmlAnchor6.Visible = false;
							}
						}
						else
						{
							htmlAnchor6.Visible = false;
						}
					}
					if (htmlAnchor10 != null)
					{
						htmlAnchor10.HRef = "ViewQRCode.aspx?orderId=" + orderInfo.OrderId;
					}
					int num4;
					if (htmlAnchor5 != null && ((orderStatus == OrderStatus.Finished && orderInfo.LineItems.Count > 0) || (orderStatus == OrderStatus.Closed && orderInfo.OnlyReturnedCount == orderInfo.LineItems.Count && orderInfo.LineItems.Count > 0)))
					{
						htmlAnchor5.Visible = true;
						htmlAnchor5.HRef = "MemberSubmitProductReview.aspx?orderId=" + text;
						DataTable productReviewAll = ProductBrowser.GetProductReviewAll(text);
						LineItemInfo lineItemInfo = new LineItemInfo();
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						bool flag = false;
						foreach (KeyValuePair<string, LineItemInfo> lineItem in orderInfo.LineItems)
						{
							flag = false;
							lineItemInfo = lineItem.Value;
							for (int i = 0; i < productReviewAll.Rows.Count; i++)
							{
								num4 = lineItemInfo.ProductId;
								if (num4.ToString() == productReviewAll.Rows[i][0].ToString() && lineItemInfo.SkuId.ToString().Trim() == productReviewAll.Rows[i][1].ToString().Trim())
								{
									flag = true;
								}
							}
							if (!flag)
							{
								num2++;
							}
							else
							{
								num3++;
							}
						}
						if (num + num2 == orderInfo.LineItems.Count)
						{
							htmlAnchor5.InnerText = "查看评论";
						}
						else
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							if (masterSettings != null)
							{
								if (masterSettings.ProductCommentPoint <= 0)
								{
									htmlAnchor5.InnerText = "评价";
								}
								else
								{
									htmlAnchor5.InnerText = $"评价得{num3 * masterSettings.ProductCommentPoint}积分";
								}
							}
						}
					}
					if (htmlAnchor3 != null && orderStatus == OrderStatus.WaitBuyerPay)
					{
						if (orderInfo.PreSaleId == 0 || (orderInfo.PreSaleId > 0 && !orderInfo.DepositDate.HasValue))
						{
							htmlAnchor3.Visible = true;
						}
						htmlAnchor3.Attributes.Add("onclick", $"closeOrder('{text}')");
					}
					DateTime dateTime;
					if (htmlAnchor7 != null)
					{
						if (orderStatus == OrderStatus.WaitBuyerPay && orderInfo.ItemStatus == OrderItemStatus.Nomarl && orderInfo.PaymentTypeId != -3 && orderInfo.Gateway != "hishop.plugins.payment.bankrequest" && orderInfo.Gateway != "hishop.plugins.payment.podrequest")
						{
							htmlAnchor7.Attributes.Add("IsServiceOrder", (orderInfo.OrderType == OrderType.ServiceOrder).ToString().ToLower());
							if (orderInfo.PreSaleId > 0)
							{
								ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
								if (!orderInfo.DepositDate.HasValue)
								{
									htmlGenericControl8.InnerText = orderInfo.Deposit.F2ToString("f2");
									(e.Item.FindControl("sptotal") as HtmlGenericControl).InnerText = "定金：￥";
									if (productPreSaleInfo.PreSaleEndDate > DateTime.Now)
									{
										AttributeCollection attributes = htmlAnchor7.Attributes;
										num4 = orderInfo.PaymentTypeId;
										attributes.Add("PaymentTypeId", num4.ToString());
										htmlAnchor7.Attributes.Add("OrderId", orderInfo.OrderId);
										htmlAnchor7.Attributes.Add("OrderTotal", orderInfo.Deposit.F2ToString("f2"));
										AttributeCollection attributes2 = htmlAnchor7.Attributes;
										num4 = orderInfo.FightGroupId;
										attributes2.Add("FightGroupId", num4.ToString());
									}
									else
									{
										htmlAnchor7.Visible = false;
										(e.Item.FindControl("sptotal") as HtmlGenericControl).InnerText = "定金：￥";
									}
								}
								else if (productPreSaleInfo.PaymentStartDate > DateTime.Now || productPreSaleInfo.PaymentEndDate < DateTime.Now)
								{
									(e.Item.FindControl("sptotal") as HtmlGenericControl).InnerText = "尾款：￥";
									htmlGenericControl8.InnerText = orderInfo.FinalPayment.F2ToString("f2");
									htmlAnchor7.Visible = false;
									htmlAnchor3.Visible = false;
									if (productPreSaleInfo.PaymentEndDate < DateTime.Now)
									{
										(e.Item.FindControl("sptotal") as HtmlGenericControl).InnerText = "尾款支付结束";
										htmlGenericControl8.Visible = false;
									}
								}
								else
								{
									AttributeCollection attributes3 = htmlAnchor7.Attributes;
									num4 = orderInfo.PaymentTypeId;
									attributes3.Add("PaymentTypeId", num4.ToString());
									htmlAnchor7.Attributes.Add("OrderId", orderInfo.OrderId);
									htmlAnchor7.Attributes.Add("OrderTotal", orderInfo.FinalPayment.F2ToString("f2"));
									AttributeCollection attributes4 = htmlAnchor7.Attributes;
									num4 = orderInfo.FightGroupId;
									attributes4.Add("FightGroupId", num4.ToString());
									htmlGenericControl8.InnerText = orderInfo.FinalPayment.F2ToString("f2");
									(e.Item.FindControl("sptotal") as HtmlGenericControl).InnerText = "尾款：￥";
									htmlAnchor3.Visible = false;
								}
							}
							else
							{
								AttributeCollection attributes5 = htmlAnchor7.Attributes;
								num4 = orderInfo.PaymentTypeId;
								attributes5.Add("PaymentTypeId", num4.ToString());
								htmlAnchor7.Attributes.Add("OrderId", orderInfo.OrderId);
								htmlAnchor7.Attributes.Add("OrderTotal", orderInfo.GetTotal(false).F2ToString("f2"));
								AttributeCollection attributes6 = htmlAnchor7.Attributes;
								num4 = orderInfo.FightGroupId;
								attributes6.Add("FightGroupId", num4.ToString());
								if (HiContext.Current.SiteSettings.OpenMultStore && orderInfo.StoreId > 0 && !SettingsManager.GetMasterSettings().Store_IsOrderInClosingTime)
								{
									StoresInfo storeById = StoresHelper.GetStoreById(orderInfo.StoreId);
									dateTime = DateTime.Now;
									string str = dateTime.ToString("yyyy-MM-dd");
									dateTime = storeById.OpenStartDate;
									DateTime value = (str + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
									dateTime = DateTime.Now;
									string str2 = dateTime.ToString("yyyy-MM-dd");
									dateTime = storeById.OpenEndDate;
									DateTime dateTime2 = (str2 + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
									if (dateTime2 <= value)
									{
										dateTime2 = dateTime2.AddDays(1.0);
									}
									if (DateTime.Now < value || DateTime.Now > dateTime2)
									{
										htmlAnchor7.Attributes.Add("NeedNotInTimeTip", "1");
									}
								}
							}
						}
						else
						{
							htmlAnchor7.Visible = false;
						}
					}
					if (htmlAnchor8 != null)
					{
						if (orderInfo.Gateway == "hishop.plugins.payment.bankrequest" && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
						{
							htmlAnchor8.HRef = "FinishOrder.aspx?OrderId=" + text + "&onlyHelp=true";
						}
						else
						{
							htmlAnchor8.Visible = false;
						}
					}
					if (htmlAnchor9 != null)
					{
						if (orderInfo.OrderStatus == OrderStatus.SellerAlreadySent && orderInfo.ItemStatus == OrderItemStatus.Nomarl)
						{
							htmlAnchor9.Attributes.Add("onclick", $"FinishOrder('{text}','{orderInfo.PaymentType}',{orderInfo.LineItems.Count})");
						}
						else
						{
							htmlAnchor9.Visible = false;
						}
					}
					if (htmlAnchor11 != null)
					{
						if (HiContext.Current.SiteSettings.IsOpenCertification && orderInfo.IDStatus == 0 && orderInfo.IsincludeCrossBorderGoods)
						{
							htmlAnchor11.Attributes.Add("orderId", orderInfo.OrderId);
							htmlAnchor11.Attributes.Add("onclick", "Certification(this)");
							htmlAnchor11.Visible = true;
						}
						else
						{
							htmlAnchor11.Visible = false;
						}
					}
					if (literal != null)
					{
						Literal literal4 = literal;
						num4 = this.GetGoodsNum(orderInfo);
						literal4.Text = num4.ToString();
					}
					if (literal2 != null)
					{
						Literal literal5 = literal2;
						num4 = this.GetGiftsNum(orderInfo);
						literal5.Text = num4.ToString();
					}
					if (orderInfo.OrderType == OrderType.ServiceOrder)
					{
						Label label = (Label)e.Item.FindControl("OrderStatusLabel2");
						IList<OrderVerificationItemInfo> orderVerificationItems = TradeHelper.GetOrderVerificationItems(orderInfo.OrderId);
						ServiceOrderStatus orderStatus2 = this.GetOrderStatus(orderInfo, orderVerificationItems);
						label.Text = ((Enum)(object)orderStatus2).ToDescription();
						label.Visible = true;
					}
					else
					{
						OrderStatusLabel orderStatusLabel = (OrderStatusLabel)e.Item.FindControl("OrderStatusLabel1");
						orderStatusLabel.OrderItemStatus = ((orderInfo.ItemStatus != 0) ? OrderItemStatus.HasReturnOrReplace : OrderItemStatus.Nomarl);
						orderStatusLabel.Gateway = orderInfo.Gateway;
						orderStatusLabel.OrderStatusCode = orderInfo.OrderStatus;
						orderStatusLabel.ShipmentModelId = orderInfo.ShippingModeId;
						orderStatusLabel.IsConfirm = orderInfo.IsConfirm;
						orderStatusLabel.ShipmentModelId = orderInfo.ShippingModeId;
						orderStatusLabel.PaymentTypeId = orderInfo.PaymentTypeId;
						orderStatusLabel.PreSaleId = orderInfo.PreSaleId;
						orderStatusLabel.DepositDate = orderInfo.DepositDate;
						orderStatusLabel.OrderType = orderInfo.OrderType;
						orderStatusLabel.ExpressCompanyName = orderInfo.ExpressCompanyName;
						orderStatusLabel.DadaStatus = orderInfo.DadaStatus;
						orderStatusLabel.Visible = true;
					}
					Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
					foreach (string key in lineItems.Keys)
					{
						lineItems[key].IsValid = (orderInfo.OrderType == OrderType.ServiceOrder);
					}
					repeater.DataSource = lineItems.Values;
					repeater.ItemDataBound += this.Repeater1_ItemDataBound;
					repeater.DataBind();
					if (orderInfo.LineItems.Count == 0)
					{
						IEnumerable<OrderGiftInfo> enumerable = from a in orderInfo.Gifts
						where a.PromoteType == 0 || a.PromoteType == 15
						select a;
						foreach (OrderGiftInfo item in enumerable)
						{
							item.NeedPoint = ((orderInfo.OrderType == OrderType.ServiceOrder) ? 1 : 0);
						}
						repeater2.DataSource = enumerable;
						repeater2.ItemDataBound += this.rptPointGifts_ItemDataBound;
						repeater2.DataBind();
					}
					OrderItemStatus itemStatus = orderInfo.ItemStatus;
					DateTime obj;
					if (DataBinder.Eval(e.Item.DataItem, "FinishDate") != DBNull.Value)
					{
						obj = (DateTime)DataBinder.Eval(e.Item.DataItem, "FinishDate");
					}
					else
					{
						dateTime = DateTime.Now;
						obj = dateTime.AddYears(-1);
					}
					DateTime dateTime3 = obj;
					string text4 = "";
					if (DataBinder.Eval(e.Item.DataItem, "Gateway") != null && !(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
					{
						text4 = (string)DataBinder.Eval(e.Item.DataItem, "Gateway");
					}
					RefundInfo refundInfo = TradeHelper.GetRefundInfo(text);
					if (htmlAnchor2 != null)
					{
						if (orderInfo.OrderType == OrderType.ServiceOrder)
						{
							htmlAnchor2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid && orderInfo.ItemStatus == OrderItemStatus.Nomarl && orderInfo.LineItems.Count != 0);
							if (htmlAnchor2.Visible)
							{
								LineItemInfo value2 = orderInfo.LineItems.FirstOrDefault().Value;
								if (value2.IsRefund)
								{
									if (value2.IsOverRefund)
									{
										htmlAnchor2.Visible = true;
									}
									else if (value2.IsValid)
									{
										htmlAnchor2.Visible = true;
									}
									else if (DateTime.Now >= value2.ValidStartDate.Value && DateTime.Now <= value2.ValidEndDate.Value)
									{
										htmlAnchor2.Visible = true;
									}
									else
									{
										htmlAnchor2.Visible = false;
									}
								}
								else
								{
									htmlAnchor2.Visible = false;
								}
							}
						}
						else
						{
							htmlAnchor2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid && orderInfo.ItemStatus == OrderItemStatus.Nomarl && orderInfo.LineItems.Count != 0 && orderInfo.GetPayTotal() > decimal.Zero);
						}
					}
					if (htmlAnchor != null)
					{
						htmlAnchor.Visible = (orderInfo.OrderType == OrderType.ServiceOrder && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid);
					}
					SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
					if (!string.IsNullOrEmpty(orderInfo.TakeCode) && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid || orderInfo.OrderStatus == OrderStatus.WaitBuyerPay))
					{
						htmlAnchor10.Visible = true;
					}
					if (!htmlAnchor2.Visible && !htmlAnchor4.Visible && !htmlAnchor10.Visible && !htmlAnchor6.Visible && !htmlAnchor3.Visible && !htmlAnchor7.Visible && !htmlAnchor8.Visible && !htmlAnchor9.Visible && !htmlAnchor5.Visible && !htmlAnchor.Visible)
					{
						htmlGenericControl.Visible = false;
					}
					if (orderInfo.FightGroupId > 0)
					{
						FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
						if (fightGroup != null)
						{
							htmlAnchor2.Visible = (fightGroup.Status != 0 && orderInfo.GetPayTotal() > decimal.Zero && (refundInfo == null || refundInfo.HandleStatus == RefundStatus.Refused) && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid);
						}
					}
				}
			}
		}

		private ServiceOrderStatus GetOrderStatus(OrderInfo order, IList<OrderVerificationItemInfo> orderVerificationItems = null)
		{
			ServiceOrderStatus result = ServiceOrderStatus.Finished;
			if (order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				result = ServiceOrderStatus.WaitBuyerPay;
			}
			else if (order.OrderStatus == OrderStatus.Closed)
			{
				result = ServiceOrderStatus.Closed;
			}
			else if (order.OrderStatus == OrderStatus.Finished)
			{
				result = ServiceOrderStatus.Finished;
			}
			else
			{
				IList<OrderVerificationItemInfo> source = orderVerificationItems;
				if (orderVerificationItems == null)
				{
					source = TradeHelper.GetOrderVerificationItems(order.OrderId);
				}
				if (source.Any((OrderVerificationItemInfo d) => d.VerificationStatus == 0.GetHashCode()))
				{
					result = ServiceOrderStatus.WaitConsumption;
				}
				else if (source.Any((OrderVerificationItemInfo d) => d.VerificationStatus == 3.GetHashCode()))
				{
					result = ServiceOrderStatus.Expired;
				}
				else if (source.Count() > 0 && source.Count(delegate(OrderVerificationItemInfo d)
				{
					int verificationStatus = d.VerificationStatus;
					VerificationStatus verificationStatus2 = VerificationStatus.Refunded;
					int result2;
					if (verificationStatus != verificationStatus2.GetHashCode())
					{
						int verificationStatus3 = d.VerificationStatus;
						verificationStatus2 = VerificationStatus.ApplyRefund;
						result2 = ((verificationStatus3 != verificationStatus2.GetHashCode()) ? 1 : 0);
					}
					else
					{
						result2 = 0;
					}
					return (byte)result2 != 0;
				}) == 0)
				{
					result = ServiceOrderStatus.Refunding;
				}
			}
			return result;
		}

		public int GetGoodsNum(OrderInfo OrderInfo)
		{
			int num = 0;
			if (OrderInfo.LineItems.Count > 0)
			{
				foreach (string key in OrderInfo.LineItems.Keys)
				{
					LineItemInfo lineItemInfo = OrderInfo.LineItems[key];
					num += lineItemInfo.ShipmentQuantity;
				}
			}
			return num;
		}

		public int GetGiftsNum(OrderInfo OrderInfo)
		{
			int num = 0;
			if (OrderInfo.Gifts.Count > 0)
			{
				foreach (OrderGiftInfo gift in OrderInfo.Gifts)
				{
					num += gift.Quantity;
				}
			}
			return num;
		}

		private void rptPointGifts_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				OrderGiftInfo orderGiftInfo = e.Item.DataItem as OrderGiftInfo;
				HtmlAnchor htmlAnchor = e.Item.FindControl("hylinkGiftName") as HtmlAnchor;
				Literal literal = e.Item.FindControl("ltlPoints") as Literal;
				Literal literal2 = e.Item.FindControl("ltlGiftCount") as Literal;
				HtmlAnchor htmlAnchor2 = e.Item.FindControl("hyDetailLink") as HtmlAnchor;
				string hRef = (orderGiftInfo.NeedPoint == 1) ? ("ServiceMemberOrderDetails.aspx?OrderId=" + orderGiftInfo.OrderId) : ("MemberOrderDetails.aspx?OrderId=" + orderGiftInfo.OrderId);
				htmlAnchor.InnerText = orderGiftInfo.GiftName;
				htmlAnchor.HRef = hRef;
				literal2.Text = orderGiftInfo.Quantity.ToNullString();
				htmlAnchor2.HRef = hRef;
			}
		}

		private void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LineItemInfo lineItemInfo = e.Item.DataItem as LineItemInfo;
				HtmlAnchor htmlAnchor = e.Item.FindControl("hylinkProductName") as HtmlAnchor;
				Literal literal = e.Item.FindControl("ltlSKUContent") as Literal;
				Literal literal2 = e.Item.FindControl("ltlPrice") as Literal;
				Literal literal3 = e.Item.FindControl("ltlProductCount") as Literal;
				HtmlAnchor htmlAnchor2 = e.Item.FindControl("hyDetailLink") as HtmlAnchor;
				Label label = e.Item.FindControl("litSendCount") as Label;
				string hRef = lineItemInfo.IsValid ? ("ServiceMemberOrderDetails.aspx?OrderId=" + lineItemInfo.OrderId.ToNullString()) : ("MemberOrderDetails.aspx?OrderId=" + lineItemInfo.OrderId.ToNullString());
				htmlAnchor.InnerText = lineItemInfo.ItemDescription.ToNullString();
				htmlAnchor.HRef = hRef;
				literal.Text = lineItemInfo.SKUContent.ToNullString();
				literal2.Text = lineItemInfo.ItemAdjustedPrice.ToDecimal(0).F2ToString("f2");
				literal3.Text = lineItemInfo.Quantity.ToNullString();
				if (lineItemInfo.ShipmentQuantity > lineItemInfo.Quantity)
				{
					label.Text = "赠" + (lineItemInfo.ShipmentQuantity - lineItemInfo.Quantity).ToString();
				}
				htmlAnchor2.HRef = hRef;
				Literal literal4 = (Literal)e.Item.FindControl("litStatus");
				if (literal4 != null)
				{
					LineItemStatus status = lineItemInfo.Status;
					string text = "";
					ReturnInfo returnInfo = TradeHelper.GetReturnInfo(lineItemInfo.OrderId, lineItemInfo.SkuId);
					ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(lineItemInfo.OrderId, lineItemInfo.SkuId);
					if (status == LineItemStatus.Normal)
					{
						text = "";
					}
					else if (returnInfo != null)
					{
						if (returnInfo.HandleStatus != ReturnStatus.Refused)
						{
							text = ((returnInfo.AfterSaleType != AfterSaleTypes.OnlyRefund) ? EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 1) : EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 3));
						}
					}
					else if (replaceInfo != null && replaceInfo.HandleStatus != ReplaceStatus.Refused)
					{
						text = EnumDescription.GetEnumDescription((Enum)(object)replaceInfo.HandleStatus, 1);
					}
					literal4.Text = text;
				}
			}
		}
	}
}
