using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserOrders : MemberTemplatedWebControl
	{
		private CalendarPanel calendarStartDate;

		private CalendarPanel calendarEndDate;

		private TextBox txtOrderId;

		private TextBox txtProductName;

		private TextBox txtShipId;

		private TextBox txtShipTo;

		private OrderStautsDropDownList dropOrderStatus;

		private DropDownList dropPayType;

		private Button imgbtnSearch;

		private IButton btnPay;

		private Literal litOrderTotal;

		private HtmlInputHidden hdorderId;

		private TextBox txtRemark;

		private TextBox txtReturnRemark;

		private TextBox txtReplaceRemark;

		private Common_OrderManage_OrderList listOrders;

		private HtmlGenericControl demodiv;

		private Pager pager;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserOrders.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.calendarStartDate = (CalendarPanel)this.FindControl("calendarStartDate");
			this.calendarEndDate = (CalendarPanel)this.FindControl("calendarEndDate");
			this.hdorderId = (HtmlInputHidden)this.FindControl("hdorderId");
			this.txtOrderId = (TextBox)this.FindControl("txtOrderId");
			this.txtProductName = (TextBox)this.FindControl("txtProductName");
			this.txtShipId = (TextBox)this.FindControl("txtShipId");
			this.txtShipTo = (TextBox)this.FindControl("txtShipTo");
			this.txtRemark = (TextBox)this.FindControl("txtRemark");
			this.txtReturnRemark = (TextBox)this.FindControl("txtReturnRemark");
			this.txtReplaceRemark = (TextBox)this.FindControl("txtReplaceRemark");
			this.dropOrderStatus = (OrderStautsDropDownList)this.FindControl("dropOrderStatus");
			this.dropPayType = (DropDownList)this.FindControl("dropPayType");
			this.btnPay = ButtonManager.Create(this.FindControl("btnPay"));
			this.imgbtnSearch = (Button)this.FindControl("imgbtnSearch");
			this.litOrderTotal = (Literal)this.FindControl("litOrderTotal");
			this.listOrders = (Common_OrderManage_OrderList)this.FindControl("Common_OrderManage_OrderList");
			this.pager = (Pager)this.FindControl("pager");
			this.demodiv = (HtmlGenericControl)this.FindControl("demodiv");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.demodiv.Visible = masterSettings.IsDemoSite;
			this.imgbtnSearch.Click += this.imgbtnSearch_Click;
			this.btnPay.Click += this.btnPay_Click;
			this.listOrders.ItemDataBound += this.listOrders_ItemDataBound;
			this.listOrders.ItemCommand += this.listOrders_ItemCommand;
			PageTitle.AddSiteNameTitle("我的订单");
			if (!this.Page.IsPostBack)
			{
				IList<PaymentModeInfo> paymentModes = TradeHelper.GetPaymentModes(PayApplicationType.payOnPC);
				PaymentModeInfo paymentModeInfo = (from p in paymentModes
				where p.Gateway.ToLower() == "hishop.plugins.payment.podrequest"
				select p).FirstOrDefault();
				PaymentModeInfo paymentModeInfo2 = (from p in paymentModes
				where p.Gateway.ToLower() == "hishop.plugins.payment.bankrequest"
				select p).FirstOrDefault();
				if (paymentModeInfo != null)
				{
					paymentModes.Remove(paymentModeInfo);
				}
				if (paymentModeInfo2 != null)
				{
					paymentModes.Remove(paymentModeInfo2);
				}
				this.dropPayType.DataSource = paymentModes;
				this.dropPayType.DataTextField = "Name";
				this.dropPayType.DataValueField = "ModeId";
				this.dropPayType.DataBind();
				string text = "";
				for (int i = 0; i < this.dropPayType.Items.Count; i++)
				{
					text = paymentModes[i].Gateway;
					this.dropPayType.Items[i].Attributes["Gateway"] = text;
					AttributeCollection attributes = this.dropPayType.Items[i].Attributes;
					bool flag = TradeHelper.GatewayIsCanBackReturn(text) || text == "hishop.plugins.payment.advancerequest";
					attributes["IsBackReturn"] = flag.ToString().ToLower();
				}
				this.BindOrders();
			}
		}

		public bool IsOnlyOneShop(OrderInfo order)
		{
			if (order != null)
			{
				return order.LineItems.Count > 1;
			}
			return false;
		}

		private void btnPay_Click(object sender, EventArgs e)
		{
			string value = this.hdorderId.Value;
			int modeId = 0;
			int.TryParse(this.dropPayType.SelectedValue, out modeId);
			PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(modeId);
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(value);
			string msg = "";
			if (!TradeHelper.CheckOrderStock(orderInfo, out msg))
			{
				this.ShowMessage(msg, false, "", 1);
			}
			else
			{
				IList<string> list = new List<string>();
				list.Add("hishop.plugins.payment.podrequest");
				list.Add("hishop.plugins.payment.bankrequest");
				if (orderInfo.CountDownBuyId > 0)
				{
					if (list.Contains(paymentMode.Gateway.ToLower().Trim()))
					{
						msg = "限购的订单不支持货到付款和线下支付";
						this.ShowMessage(msg, false, "", 1);
						return;
					}
					foreach (LineItemInfo value2 in orderInfo.LineItems.Values)
					{
						CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(value2.ProductId, orderInfo.CountDownBuyId, value2.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, out msg, orderInfo.StoreId);
						if (countDownInfo == null)
						{
							this.ShowMessage(msg, false, "", 1);
							return;
						}
					}
				}
				if (orderInfo.FightGroupId > 0)
				{
					if (paymentMode == null || (!TradeHelper.GatewayIsCanBackReturn(paymentMode.Gateway) && paymentMode.Gateway != EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1)))
					{
						this.ShowMessage("火拼团订单不支付该支付方式进行支付", false, "", 1);
						return;
					}
					foreach (KeyValuePair<string, LineItemInfo> lineItem in orderInfo.LineItems)
					{
						FightGroupActivityInfo fightGroupActivityInfo = VShopHelper.CheckUserFightGroup(lineItem.Value.ProductId, orderInfo.FightGroupActivityId, orderInfo.FightGroupId, lineItem.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, lineItem.Value.Quantity, out msg);
						if (fightGroupActivityInfo == null)
						{
							this.ShowMessage(msg, false, "", 1);
							return;
						}
					}
				}
				if (orderInfo.GroupBuyId > 0 && list.Contains(paymentMode.Gateway.ToLower().Trim()))
				{
					msg = "团购的订单不支持货到付款和线下支付";
					this.ShowMessage(msg, false, "", 1);
				}
				else
				{
					if (orderInfo.PreSaleId > 0)
					{
						if (paymentMode.Gateway.ToLower().Trim() == "hishop.plugins.payment.podrequest")
						{
							this.ShowMessage("预售订单不支持货到付款", false, "", 1);
							return;
						}
						ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
						if (productPreSaleInfo == null)
						{
							this.ShowMessage("预售活动不存在,不能支付", false, "", 1);
							return;
						}
						if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && productPreSaleInfo.PreSaleEndDate < DateTime.Now)
						{
							this.ShowMessage("您支付晚了，预售活动已经结束", false, "", 1);
							return;
						}
						if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
						{
							if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
							{
								this.ShowMessage("尾款支付尚未开始", false, "", 1);
								return;
							}
							DateTime dateTime = productPreSaleInfo.PaymentEndDate;
							DateTime date = dateTime.Date;
							dateTime = DateTime.Now;
							if (date < dateTime.Date)
							{
								this.ShowMessage("尾款支付已结束", false, "", 1);
								return;
							}
						}
					}
					if (paymentMode != null)
					{
						orderInfo.PaymentTypeId = paymentMode.ModeId;
						orderInfo.PaymentType = paymentMode.Name;
						orderInfo.Gateway = paymentMode.Gateway;
						TradeHelper.UpdateOrderPaymentType(orderInfo);
					}
					this.Page.Response.Redirect(base.GetRouteUrl("sendPayment", new
					{
						orderId = value
					}));
				}
			}
		}

		protected void listProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LineItemStatus lineItemStatus = (LineItemStatus)DataBinder.Eval(e.Item.DataItem, "Status");
				string text = (string)DataBinder.Eval(e.Item.DataItem, "StatusText");
				string orderId = (string)DataBinder.Eval(e.Item.DataItem, "OrderId");
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
				string text2 = DataBinder.Eval(e.Item.DataItem, "SkuId").ToString();
				LineItemInfo lineItemInfo = orderInfo.LineItems[text2];
				if (lineItemStatus == LineItemStatus.Normal)
				{
					text = TradeHelper.GetOrderItemSatusText(lineItemInfo.Status);
				}
				OrderStatus orderStatus = orderInfo.OrderStatus;
				DateTime finishDate = orderInfo.FinishDate;
				string gateway = orderInfo.Gateway;
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnAfterSalesApply");
				Label label = (Label)e.Item.FindControl("ItemLogistics");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnViewMessage");
				htmlAnchor.Attributes.Add("OrderId", orderInfo.OrderId);
				htmlAnchor.Attributes.Add("SkuId", text2);
				htmlAnchor.Attributes.Add("GateWay", gateway);
				ReplaceInfo replaceInfo = lineItemInfo.ReplaceInfo;
				ReturnInfo returnInfo = lineItemInfo.ReturnInfo;
				Literal literal = (Literal)e.Item.FindControl("litStatusText");
				if (literal != null && (replaceInfo != null || returnInfo != null))
				{
					if (returnInfo != null)
					{
						if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
						{
							literal.Text = "<a href=\"UserReturnsApplyDetails?ReturnsId=" + returnInfo.ReturnId + "\" class=\"aslink\">" + EnumDescription.GetEnumDescription((Enum)(object)lineItemStatus, 3) + "</a>";
						}
						else
						{
							literal.Text = "<a href=\"UserReturnsApplyDetails?ReturnsId=" + returnInfo.ReturnId + "\" class=\"aslink\">" + EnumDescription.GetEnumDescription((Enum)(object)lineItemStatus, 2) + "</a>";
						}
					}
					else
					{
						literal.Text = "<a href=\"UserReplaceApplyDetails?ReplaceId=" + replaceInfo.ReplaceId + "\" class=\"aslink\">" + EnumDescription.GetEnumDescription((Enum)(object)lineItemStatus, 2) + "</a>";
					}
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				HtmlAnchor htmlAnchor3 = htmlAnchor;
				int visible;
				if (orderInfo.OrderType != OrderType.ServiceOrder)
				{
					switch (orderStatus)
					{
					case OrderStatus.Finished:
						visible = ((!orderInfo.IsServiceOver) ? 1 : 0);
						break;
					default:
						visible = 0;
						break;
					case OrderStatus.SellerAlreadySent:
						visible = 1;
						break;
					}
				}
				else
				{
					visible = 0;
				}
				htmlAnchor3.Visible = ((byte)visible != 0);
				if (htmlAnchor.Visible)
				{
					htmlAnchor.Visible = ((returnInfo == null || returnInfo.HandleStatus == ReturnStatus.Refused) && (replaceInfo == null || replaceInfo.HandleStatus == ReplaceStatus.Refused || replaceInfo.HandleStatus == ReplaceStatus.Replaced));
				}
			}
		}

		protected void listOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				OrderStatus orderStatus = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				string text = DataBinder.Eval(e.Item.DataItem, "OrderId").ToString();
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo != null)
				{
					if (orderInfo.PreSaleId > 0)
					{
						Literal literal = (Literal)e.Item.FindControl("litPresale");
						literal.Text = "（预售）";
						literal.Visible = true;
					}
					OrderItemStatus itemStatus = orderInfo.ItemStatus;
					DateTime dateTime = (DataBinder.Eval(e.Item.DataItem, "FinishDate") == DBNull.Value) ? DateTime.Now.AddYears(-1) : ((DateTime)DataBinder.Eval(e.Item.DataItem, "FinishDate"));
					string text2 = "";
					if (DataBinder.Eval(e.Item.DataItem, "Gateway") != null && !(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
					{
						text2 = (string)DataBinder.Eval(e.Item.DataItem, "Gateway");
					}
					HyperLink hyperLink = (HyperLink)e.Item.FindControl("hplinkorderreview");
					HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("hlinkPay");
					ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
					ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnCloseOrder");
					HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnApplyForRefund");
					HtmlAnchor htmlAnchor3 = (HtmlAnchor)e.Item.FindControl("lkbtnUserRealNameVerify");
					HyperLink hyperLink2 = (HyperLink)e.Item.FindControl("hlinkOrderDetails");
					Repeater repeater = (Repeater)e.Item.FindControl("rpProduct");
					Repeater repeater2 = (Repeater)e.Item.FindControl("rpGift");
					Label label = (Label)e.Item.FindControl("Logistics");
					HtmlAnchor htmlAnchor4 = (HtmlAnchor)e.Item.FindControl("lkbtnViewMessage");
					HtmlAnchor htmlAnchor5 = (HtmlAnchor)e.Item.FindControl("lkbtnRefundDetail");
					htmlAnchor2.Attributes.Add("OrderId", text);
					htmlAnchor2.Attributes.Add("SkuId", "");
					htmlAnchor2.Attributes.Add("GateWay", text2);
					OrderStatusLabel orderStatusLabel = (OrderStatusLabel)e.Item.FindControl("lblOrderStatus");
					Literal literal2 = (Literal)e.Item.FindControl("lblGiftTitle");
					orderStatusLabel.order = orderInfo;
					if (orderInfo.LineItems.Count <= 0)
					{
						Literal literal3 = literal2;
						literal3.Text += "（礼）";
					}
					if (hyperLink != null)
					{
						if (orderInfo.GetGiftQuantity() > 0 && orderInfo.LineItems.Count() == 0)
						{
							hyperLink.Visible = false;
						}
						else
						{
							HyperLink hyperLink3 = hyperLink;
							int visible;
							switch (orderStatus)
							{
							case OrderStatus.Closed:
								visible = ((orderInfo.OnlyReturnedCount == orderInfo.LineItems.Count) ? 1 : 0);
								break;
							default:
								visible = 0;
								break;
							case OrderStatus.Finished:
								visible = 1;
								break;
							}
							hyperLink3.Visible = ((byte)visible != 0);
							if (hyperLink.Visible)
							{
								DataTable productReviewAll = ProductBrowser.GetProductReviewAll(orderInfo.OrderId);
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
										if (lineItemInfo.ProductId.ToString() == productReviewAll.Rows[i][0].ToString() && lineItemInfo.SkuId.ToString().Trim() == productReviewAll.Rows[i][1].ToString().Trim())
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
									hyperLink.Text = "查看评论";
								}
								else
								{
									SiteSettings masterSettings = SettingsManager.GetMasterSettings();
									if (masterSettings != null)
									{
										if (masterSettings.ProductCommentPoint <= 0)
										{
											hyperLink.Text = "评价";
										}
										else
										{
											hyperLink.Text = $"评价得{num3 * masterSettings.ProductCommentPoint}积分";
										}
									}
								}
							}
						}
					}
					if (orderInfo.PreSaleId > 0)
					{
						FormatedMoneyLabel formatedMoneyLabel = (FormatedMoneyLabel)e.Item.FindControl("FormatedMoneyLabel2");
						formatedMoneyLabel.Money = orderInfo.Deposit + orderInfo.FinalPayment;
						formatedMoneyLabel.Text = (orderInfo.Deposit + orderInfo.FinalPayment).F2ToString("f2");
						if (orderStatus == OrderStatus.WaitBuyerPay && text2 != "hishop.plugins.payment.podrequest" && text2 != "hishop.plugins.payment.bankrequest" && orderInfo.PaymentTypeId != -3)
						{
							if (!orderInfo.DepositDate.HasValue)
							{
								htmlAnchor.Visible = true;
							}
							else if (orderInfo.DepositDate.HasValue)
							{
								ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
								if (productPreSaleInfo.PaymentStartDate <= DateTime.Now && DateTime.Now <= productPreSaleInfo.PaymentEndDate)
								{
									htmlAnchor.Visible = true;
								}
								else
								{
									htmlAnchor.Visible = false;
								}
							}
							else
							{
								htmlAnchor.Visible = false;
							}
						}
						else
						{
							htmlAnchor.Visible = false;
						}
					}
					else
					{
						htmlAnchor.Visible = (orderStatus == OrderStatus.WaitBuyerPay && text2 != "hishop.plugins.payment.podrequest" && text2 != "hishop.plugins.payment.bankrequest" && orderInfo.PaymentTypeId != -3);
					}
					imageLinkButton.Visible = (orderStatus == OrderStatus.SellerAlreadySent && itemStatus == OrderItemStatus.Nomarl);
					if (orderInfo.PreSaleId > 0)
					{
						imageLinkButton2.Visible = (orderStatus == OrderStatus.WaitBuyerPay && itemStatus == OrderItemStatus.Nomarl && !orderInfo.DepositDate.HasValue);
					}
					else
					{
						imageLinkButton2.Visible = (orderStatus == OrderStatus.WaitBuyerPay && itemStatus == OrderItemStatus.Nomarl);
					}
					RefundInfo refundInfo = TradeHelper.GetRefundInfo(text);
					htmlAnchor2.Visible = ((orderInfo.FightGroupId > 0 && VShopHelper.IsFightGroupCanRefund(orderInfo.FightGroupId) && orderInfo.IsCanRefund) || (orderInfo.FightGroupId <= 0 && orderInfo.IsCanRefund));
					htmlAnchor3.Visible = (HiContext.Current.SiteSettings.IsOpenCertification && orderInfo.IDStatus == 0 && orderInfo.IsincludeCrossBorderGoods);
					if (htmlAnchor3.Visible)
					{
						htmlAnchor3.Attributes.Add("OrderId", text);
					}
					if (repeater != null && repeater2 != null)
					{
						repeater.ItemDataBound += this.listProduct_ItemDataBound;
						IList<NewLineItemInfo> list = new List<NewLineItemInfo>();
						foreach (LineItemInfo value in orderInfo.LineItems.Values)
						{
							NewLineItemInfo newLineItemInfo = this.GetNewLineItemInfo(value, orderInfo.OrderId);
							list.Add(newLineItemInfo);
						}
						if (list == null || list.Count == 0)
						{
							repeater.Visible = false;
							DataTable dataTable = (DataTable)(repeater2.DataSource = TradeHelper.GetOrderGiftsThumbnailsUrl(((DataRowView)e.Item.DataItem).Row["OrderId"].ToString()));
							repeater2.DataBind();
							repeater2.Visible = true;
						}
						else
						{
							repeater.DataSource = list;
							repeater.DataBind();
							repeater2.Visible = false;
							repeater.Visible = true;
						}
					}
					if (refundInfo != null && orderInfo.ItemStatus == OrderItemStatus.Nomarl && (orderInfo.OrderStatus == OrderStatus.ApplyForRefund || orderInfo.OrderStatus == OrderStatus.RefundRefused || orderInfo.OrderStatus == OrderStatus.Closed))
					{
						htmlAnchor5.HRef = "/user/UserRefundApplyDetails/" + refundInfo.RefundId;
						htmlAnchor5.Visible = true;
					}
					hyperLink2.NavigateUrl = "/user/OrderDetails/" + orderInfo.OrderId;
					if ((orderStatus == OrderStatus.SellerAlreadySent || orderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb) && !string.IsNullOrEmpty(orderInfo.ShipOrderNumber) && orderInfo.ShippingModeId != -2)
					{
						label.Attributes.Add("action", "order");
						label.Attributes.Add("orderId", text);
						label.Visible = true;
					}
					if (orderInfo.FightGroupId > 0)
					{
						FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
						htmlAnchor2.Visible = (fightGroup.Status != 0 && orderInfo.GetPayTotal() > decimal.Zero && (refundInfo == null || refundInfo.HandleStatus == RefundStatus.Refused) && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid);
					}
					if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid || orderInfo.OrderStatus == OrderStatus.Finished || orderInfo.OrderStatus == OrderStatus.WaitReview || orderInfo.OrderStatus == OrderStatus.History)
					{
						WeiXinRedEnvelopeInfo openedWeiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetOpenedWeiXinRedEnvelope();
						bool flag2 = false;
						if (openedWeiXinRedEnvelope != null && openedWeiXinRedEnvelope.EnableIssueMinAmount <= orderInfo.GetPayTotal() && orderInfo.OrderDate >= openedWeiXinRedEnvelope.ActiveStartTime && orderInfo.OrderDate <= openedWeiXinRedEnvelope.ActiveEndTime)
						{
							flag2 = true;
						}
						if (flag2)
						{
							Image image = (Image)e.Item.FindControl("imgRedEnvelope");
							image.ImageUrl = "../../../../common/images/SendRedEnvelope.png";
							image.Attributes.Add("class", "ztitle_RedEnvelope");
							image.Attributes.Add("onclick", "GetRedEnvelope(" + orderInfo.OrderId + ")");
							image.Visible = true;
						}
					}
					if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || !(orderInfo.ParentOrderId == "-1") || !orderInfo.OrderId.Contains("P"))
					{
						Label label2 = (Label)e.Item.FindControl("lblsupplier");
						if (label2 != null)
						{
							string empty = string.Empty;
							if (HiContext.Current.SiteSettings.OpenMultStore && orderInfo.StoreId > 0 && !string.IsNullOrWhiteSpace(orderInfo.StoreName))
							{
								label2.Text = orderInfo.StoreName;
								empty = "mtitle_1";
							}
							else if (orderInfo.StoreId == 0 && HiContext.Current.SiteSettings.OpenSupplier && orderInfo.SupplierId > 0)
							{
								label2.Text = orderInfo.ShipperName;
								empty = "stitle_1";
							}
							else
							{
								label2.Text = "平台";
								empty = "ztitle_1_new";
							}
							label2.Attributes.Add("style", string.IsNullOrWhiteSpace(label2.Text) ? "display:none" : "display:inline");
							label2.Attributes.Add("class", empty);
							label2.Visible = true;
						}
					}
				}
			}
		}

		protected void listOrders_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(e.CommandArgument.ToString());
			if (orderInfo != null)
			{
				if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
				{
					if (TradeHelper.ConfirmOrderFinish(orderInfo))
					{
						this.BindOrders();
						this.ShowMessage("成功的完成了该订单", true, "", 1);
						if (orderInfo.LineItems.Count > 0)
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							if (masterSettings != null && masterSettings.ProductCommentPoint > 0)
							{
								Panel panel = (Panel)this.FindControl("panl_productcommentTip");
								if (panel != null)
								{
									panel.Visible = true;
									HiddenField hiddenField = (HiddenField)this.FindControl("goCommentLink");
									if (hiddenField != null)
									{
										hiddenField.Value = $"/user/OrderReviews/{orderInfo.OrderId}";
									}
								}
							}
						}
					}
					else
					{
						this.ShowMessage("完成订单失败，订单状态错误或者订单商品有退款、退货或者换货正在进行中!", false, "", 1);
					}
				}
				if (e.CommandName == "CLOSE_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_CLOSE))
				{
					if (TradeHelper.CloseOrder(orderInfo.OrderId, "会员主动关闭") && orderInfo.ItemStatus == OrderItemStatus.Nomarl)
					{
						if (orderInfo.ShippingModeId == -2 && orderInfo.IsConfirm)
						{
							OrderHelper.CloseDeportOrderReturnStock(orderInfo, "会员" + HiContext.Current.User.UserName + "关闭订单");
						}
						Messenger.OrderClosed(HiContext.Current.User, orderInfo, "用户自己关闭订单");
						this.BindOrders();
						this.ShowMessage("成功的关闭了该订单", true, "", 1);
					}
					else
					{
						this.ShowMessage("关闭订单失败。", false, "", 1);
					}
				}
			}
		}

		protected void listOrders_ReBindData(object sender)
		{
			this.ReloadUserOrders(false);
		}

		protected void imgbtnSearch_Click(object sender, EventArgs e)
		{
			this.ReloadUserOrders(true);
		}

		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult userOrder = TradeHelper.GetUserOrder(HiContext.Current.UserId, orderQuery);
			this.listOrders.DataSource = userOrder.Data;
			this.listOrders.DataBind();
			this.txtOrderId.Text = (string.IsNullOrEmpty(orderQuery.OrderId) ? orderQuery.ProductName : orderQuery.OrderId);
			this.txtProductName.Text = orderQuery.ProductName;
			this.txtShipId.Text = orderQuery.ShipId;
			this.txtShipTo.Text = orderQuery.ShipTo;
			this.dropOrderStatus.SelectedValue = orderQuery.Status;
			this.calendarStartDate.SelectedDate = orderQuery.StartDate;
			this.calendarEndDate.SelectedDate = orderQuery.EndDate;
			this.pager.TotalRecords = userOrder.TotalRecords;
		}

		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipId"]))
			{
				orderQuery.ShipId = this.Page.Request.QueryString["shipId"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
			{
				orderQuery.ShipTo = this.Page.Request.QueryString["shipTo"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ParentOrderId"]))
			{
				orderQuery.ParentOrderId = this.Page.Request.QueryString["ParentOrderId"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				orderQuery.StartDate = Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				orderQuery.EndDate = Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["endDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderStatus"]))
			{
				int status = 0;
				if (int.TryParse(this.Page.Request.QueryString["orderStatus"], out status))
				{
					orderQuery.Status = (OrderStatus)status;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["itemStatus"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["itemStatus"], out value))
				{
					orderQuery.ItemStatus = value;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
			{
				orderQuery.SortBy = this.Page.Request.QueryString["sortBy"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
			{
				int sortOrder = 0;
				if (int.TryParse(this.Page.Request.QueryString["sortOrder"], out sortOrder))
				{
					orderQuery.SortOrder = (SortAction)sortOrder;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				orderQuery.ProductName = Globals.UrlDecode(Globals.StripAllTags(this.Page.Request.QueryString["productName"]));
			}
			orderQuery.IsServiceOrder = false;
			orderQuery.PageIndex = this.pager.PageIndex;
			orderQuery.PageSize = this.pager.PageSize;
			return orderQuery;
		}

		private void ReloadUserOrders(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			string text = Globals.StripAllTags(this.txtOrderId.Text.Trim());
			if (TradeHelper.IsOrderId(text))
			{
				nameValueCollection.Add("orderId", text);
			}
			else
			{
				nameValueCollection.Add("productName", text);
			}
			nameValueCollection.Add("shipId", this.txtShipId.Text.Trim());
			NameValueCollection nameValueCollection2 = nameValueCollection;
			DateTime? selectedDate = this.calendarStartDate.SelectedDate;
			nameValueCollection2.Add("startDate", selectedDate.ToString());
			NameValueCollection nameValueCollection3 = nameValueCollection;
			selectedDate = this.calendarEndDate.SelectedDate;
			nameValueCollection3.Add("endDate", selectedDate.ToString());
			NameValueCollection nameValueCollection4 = nameValueCollection;
			int num = (int)this.dropOrderStatus.SelectedValue;
			nameValueCollection4.Add("orderStatus", num.ToString());
			NameValueCollection nameValueCollection5 = nameValueCollection;
			num = (int)this.listOrders.SortOrder;
			nameValueCollection5.Add("sortOrder", num.ToString());
			if (!isSearch)
			{
				NameValueCollection nameValueCollection6 = nameValueCollection;
				num = this.pager.PageIndex;
				nameValueCollection6.Add("pageIndex", num.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}

		public NewLineItemInfo GetNewLineItemInfo(LineItemInfo item, string OrderId)
		{
			NewLineItemInfo newLineItemInfo = new NewLineItemInfo();
			newLineItemInfo.OrderId = OrderId;
			newLineItemInfo.ThumbnailsUrl = (string.IsNullOrEmpty(item.ThumbnailsUrl) ? HiContext.Current.SiteSettings.DefaultProductThumbnail2 : item.ThumbnailsUrl.Replace("thumbs40/40", "thumbs60/60"));
			newLineItemInfo.ReplaceInfo = item.ReplaceInfo;
			newLineItemInfo.ReturnInfo = item.ReturnInfo;
			newLineItemInfo.ItemDescription = item.ItemDescription;
			newLineItemInfo.RealTotalPrice = decimal.Parse(item.RealTotalPrice.F2ToString("f2"));
			newLineItemInfo.ItemAdjustedPrice = item.ItemAdjustedPrice;
			newLineItemInfo.ShipmentQuantity = item.ShipmentQuantity;
			newLineItemInfo.ItemCostPrice = item.ItemCostPrice;
			newLineItemInfo.ItemDescription = item.ItemDescription;
			newLineItemInfo.ItemListPrice = item.ItemListPrice;
			newLineItemInfo.ItemWeight = item.ItemWeight;
			newLineItemInfo.ProductId = item.ProductId;
			newLineItemInfo.PromoteType = item.PromoteType;
			newLineItemInfo.PromotionId = item.PromotionId;
			newLineItemInfo.PromotionName = item.PromotionName;
			newLineItemInfo.RefundAmount = item.RefundAmount;
			newLineItemInfo.SKU = item.SKU;
			newLineItemInfo.SKUContent = item.SKUContent;
			newLineItemInfo.SkuId = item.SkuId;
			newLineItemInfo.ProductId = item.ProductId;
			newLineItemInfo.Quantity = item.Quantity;
			newLineItemInfo.OrderId = OrderId;
			newLineItemInfo.SkuId = item.SkuId;
			newLineItemInfo.Status = item.Status;
			newLineItemInfo.StatusText = item.StatusText;
			return newLineItemInfo;
		}
	}
}
