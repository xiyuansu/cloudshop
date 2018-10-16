using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin;
using Hishop.Plugins;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class SendGoods : StoreAdminPage
	{
		private string orderId;

		protected Label lblOrderId;

		protected FormatedTimeLabel lblOrderTime;

		protected HiddenField txtSendGoodType;

		protected HtmlGenericControl labSameCity;

		protected ExpressRadioButtonList expressRadioButtonList;

		protected TextBox txtShipOrderNumber;

		protected HtmlGenericControl txtShipOrderNumberTip;

		protected HiddenField txtDeliveryNo;

		protected Button btnSendGoods;

		protected Order_ItemsList itemsList;

		protected Literal litShippingModeName;

		protected Literal litReceivingInfo;

		protected Label litShipToDate;

		protected Label litRemark;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.orderId = this.Page.Request.QueryString["OrderId"];
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
				this.BindOrderItems(orderInfo);
				this.btnSendGoods.Click += this.btnSendGoods_Click;
				if (!this.Page.IsPostBack)
				{
					if (orderInfo == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.labSameCity.Visible = HiContext.Current.SiteSettings.OpenDadaLogistics;
						this.BindExpressCompany(orderInfo.ShippingModeId);
						this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyName;
						this.BindShippingAddress(orderInfo);
						this.litShippingModeName.Text = orderInfo.ModeName;
						this.litShipToDate.Text = orderInfo.ShipToDate;
						this.litRemark.Text = orderInfo.Remark;
						this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
					}
				}
			}
		}

		private void BindOrderItems(OrderInfo order)
		{
			this.lblOrderId.Text = order.PayOrderId;
			this.lblOrderTime.Time = order.OrderDate;
			this.itemsList.Order = order;
			this.itemsList.ShowAllItem = false;
		}

		private void BindShippingAddress(OrderInfo order)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(order.ShippingRegion))
			{
				text = order.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(order.Address))
			{
				text += order.Address;
			}
			if (!string.IsNullOrEmpty(order.ShipTo))
			{
				text = text + "  " + order.ShipTo;
			}
			if (!string.IsNullOrEmpty(order.TelPhone))
			{
				text = text + "  " + order.TelPhone;
			}
			if (!string.IsNullOrEmpty(order.CellPhone))
			{
				text = text + "  " + order.CellPhone;
			}
			this.litReceivingInfo.Text = text;
		}

		private void BindExpressCompany(int modeId)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!masterSettings.OpenMultStore)
			{
				this.expressRadioButtonList.ExpressCompanies = ExpressHelper.GetAllExpressName(false);
				this.expressRadioButtonList.DataBind();
			}
			else if (modeId != -1 && modeId != -2)
			{
				this.expressRadioButtonList.ExpressCompanies = ExpressHelper.GetAllExpressName(false);
				this.expressRadioButtonList.DataBind();
			}
			else
			{
				IList<string> allExpressName = ExpressHelper.GetAllExpressName(false);
				this.expressRadioButtonList.ExpressCompanies = allExpressName;
				this.expressRadioButtonList.DataBind();
			}
		}

		private void btnSendGoods_Click(object sender, EventArgs e)
		{
			string text = this.txtDeliveryNo.Value.ToNullString();
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			int num = this.txtSendGoodType.Value.ToInt(0);
			if (orderInfo != null)
			{
				if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && !OrderHelper.CheckStock(orderInfo))
				{
					this.ShowMsg("订单有商品库存不足,请补充库存后发货！", false);
				}
				else if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
				{
					this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
				}
				else if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS) || orderInfo.ItemStatus != 0)
				{
					this.ShowMsg("当前订单状态没有付款、不是等待发货的订单，或者订单中有商品正在进行退款操作，所以不能发货", false);
				}
				else if (num == 1 && (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || this.txtShipOrderNumber.Text.Trim().Length > 20))
				{
					this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
				}
				else if (num == 1 && string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
				{
					this.ShowMsg("请选择物流公司", false);
				}
				else if (num == 2 && text == "")
				{
					this.ShowMsg("使用同城物流发货需要正确的物流编号", false);
				}
				else
				{
					string text2 = "";
					ExpressCompanyInfo expressCompanyInfo = null;
					switch (num)
					{
					case 1:
						if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
						{
							break;
						}
						expressCompanyInfo = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
						if (expressCompanyInfo != null)
						{
							orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
							orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
						}
						orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
						if (string.IsNullOrEmpty(orderInfo.OuterOrderId))
						{
							break;
						}
						if (!orderInfo.OuterOrderId.StartsWith("jd_") || !string.IsNullOrWhiteSpace(expressCompanyInfo.JDCode))
						{
							break;
						}
						this.ShowMsg("此订单是京东订单，所选物流公司不被京东支持", false);
						return;
					case 0:
						orderInfo.ExpressCompanyName = "";
						orderInfo.ExpressCompanyAbb = "";
						orderInfo.ShipOrderNumber = "";
						break;
					default:
						orderInfo.ExpressCompanyName = "同城物流配送";
						orderInfo.ExpressCompanyAbb = "";
						orderInfo.ShipOrderNumber = "";
						orderInfo.DadaStatus = DadaStatus.WaitOrder;
						break;
					}
					OrderStatus orderStatus = orderInfo.OrderStatus;
					if (OrderHelper.SendGoods(orderInfo))
					{
						if (!string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb) && orderInfo.ExpressCompanyAbb.ToUpper() == "HTKY")
						{
							ExpressHelper.GetDataByKuaidi100(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
						}
						if (orderInfo.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.podrequest")
						{
							ProductStatisticsHelper.UpdateOrderSaleStatistics(orderInfo);
							TransactionAnalysisHelper.AnalysisOrderTranData(orderInfo);
						}
						if (orderStatus == OrderStatus.WaitBuyerPay)
						{
							OrderHelper.ChangeStoreStockAndWriteLog(orderInfo);
						}
						if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							PayClient payClient = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, masterSettings.WeixinPaySignKey, "", "", "");
							DeliverInfo deliverInfo = new DeliverInfo();
							deliverInfo.TransId = orderInfo.GatewayOrderId;
							deliverInfo.OutTradeNo = orderInfo.PayOrderId;
							deliverInfo.OpenId = Users.GetUser(orderInfo.UserId).MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin").OpenId;
							payClient.DeliverNotify(deliverInfo);
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
										PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.TryDecypt(paymentMode.Settings), orderInfo.PayOrderId, orderInfo.GetTotal(false), "订单发货", "订单号-" + orderInfo.PayOrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(""), Globals.FullPath(base.GetRouteUrl("PaymentReturn_url", new
										{
											HIGW = hIGW
										})), Globals.FullPath(base.GetRouteUrl("PaymentNotify_url", new
										{
											HIGW = hIGW
										})), "");
										paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
									}
								}
								catch (Exception)
								{
								}
							}
							if (!string.IsNullOrEmpty(orderInfo.OuterOrderId) && expressCompanyInfo != null)
							{
								if (orderInfo.OuterOrderId.StartsWith("tb_"))
								{
									string text3 = orderInfo.OuterOrderId.Replace("tb_", "");
									try
									{
										string requestUriString = $"http://order2.kuaidiangtong.com/UpdateShipping.ashx?tid={text3}&companycode={expressCompanyInfo.TaobaoCode}&outsid={orderInfo.ShipOrderNumber}&Host={HiContext.Current.SiteUrl}";
										WebRequest webRequest = WebRequest.Create(requestUriString);
										webRequest.GetResponse();
									}
									catch
									{
									}
								}
								else if (orderInfo.OuterOrderId.StartsWith("jd_") && expressCompanyInfo != null)
								{
									string text3 = orderInfo.OuterOrderId.Replace("jd_", "");
									try
									{
										SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
										JDHelper.JDOrderOutStorage(masterSettings2.JDAppKey, masterSettings2.JDAppSecret, masterSettings2.JDAccessToken, expressCompanyInfo.JDCode, orderInfo.ShipOrderNumber, text3);
									}
									catch (Exception ex2)
									{
										text2 = $"\r\n同步京东发货失败，京东订单号：{text3}，{ex2.Message}\r\n";
									}
								}
							}
						}
						if (orderInfo.ExpressCompanyName == "同城物流配送" && !string.IsNullOrEmpty(text))
						{
							SiteSettings masterSettings3 = SettingsManager.GetMasterSettings();
							DadaHelper.addAfterQuery(masterSettings3.DadaSourceID, text);
						}
						int userId = orderInfo.UserId;
						MemberInfo user = Users.GetUser(orderInfo.UserId);
						Messenger.OrderShipping(orderInfo, user);
						orderInfo.OnDeliver();
						if (orderInfo.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
						{
							OrderHelper.SetOrderIsStoreCollect(orderInfo.OrderId);
						}
						if (string.IsNullOrWhiteSpace(text2))
						{
							this.ShowMsg("发货成功", true);
						}
						else
						{
							this.ShowMsg($"发货成功{text2}", true);
						}
					}
					else
					{
						this.ShowMsg("发货失败,可能是商品库存不足,订单中有商品正在退货、换货状态", false);
					}
				}
			}
		}
	}
}
