using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FinishOrder : HtmlTemplatedWebControl
	{
		private string orderId;

		private Literal litOrderId;

		private FormatedMoneyLabel litOrderPrice;

		private FormatedMoneyLabel litDeposit;

		private HtmlGenericControl promptMsg;

		private HtmlGenericControl onlinePayPanel;

		private HtmlGenericControl offlinePayPanel;

		private Common_PaymentModeList paymentModeList;

		private HtmlInputHidden paymentModeId;

		private HtmlInputHidden AdvanceId;

		private HtmlInputHidden hidOrderid;

		private IButton btnOrderPay;

		private HtmlGenericControl msgTitle;

		private HtmlAnchor userLink;

		private HtmlAnchor orderLink;

		private HiddenField hidIspop;

		private HiddenField hidIsPreSale;

		private HtmlGenericControl divfinish;

		private HtmlGenericControl divOrderPayInfo;

		private HtmlImage imgPayResult;

		private Literal litDeposittxt;

		private Literal litOfflinePayContent;

		private HtmlGenericControl demodiv;

		private HtmlInputHidden hidHasTradePassword;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-FinishOrder.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.orderId = base.GetParameter("orderId", false);
			if (base.GetParameter("isCallback", false).ToBool())
			{
				HiContext.Current.Context.Response.Clear();
				HiContext.Current.Context.Response.ContentType = "application/json";
				string parameter = base.GetParameter("action", false);
				if (parameter.Equals("ToPay"))
				{
					string empty = string.Empty;
					OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
					if (orderInfo != null || orderInfo.UserId != HiContext.Current.UserId)
					{
						if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
						{
							HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"当前订单不是等待付款状态\"}");
							return;
						}
						if (orderInfo.CountDownBuyId > 0)
						{
							foreach (KeyValuePair<string, LineItemInfo> lineItem in orderInfo.LineItems)
							{
								CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(lineItem.Value.ProductId, orderInfo.CountDownBuyId, lineItem.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, out empty, orderInfo.StoreId);
								if (countDownInfo == null)
								{
									HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"" + empty + "\"}");
									return;
								}
							}
						}
						if (orderInfo.FightGroupId > 0)
						{
							foreach (KeyValuePair<string, LineItemInfo> lineItem2 in orderInfo.LineItems)
							{
								FightGroupActivityInfo fightGroupActivityInfo = VShopHelper.CheckUserFightGroup(lineItem2.Value.ProductId, orderInfo.FightGroupActivityId, orderInfo.FightGroupId, lineItem2.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, lineItem2.Value.Quantity, out empty);
								if (fightGroupActivityInfo == null)
								{
									HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"" + empty + "\"}");
									return;
								}
							}
						}
						if (orderInfo.PreSaleId > 0)
						{
							ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
							if (productPreSaleInfo == null)
							{
								HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"预售活动不存在不能支付\"}");
								return;
							}
							if (!orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && productPreSaleInfo.PreSaleEndDate < DateTime.Now)
							{
								HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"您支付晚了，预售活动已经结束\"}");
								return;
							}
							if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
							{
								if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
								{
									HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"尾款支付尚未开始\"}");
									return;
								}
								DateTime dateTime = productPreSaleInfo.PaymentEndDate;
								DateTime date = dateTime.Date;
								dateTime = DateTime.Now;
								if (date < dateTime.Date)
								{
									HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"尾款支付已结束\"}");
									return;
								}
							}
						}
						int modeId = base.GetParameter("paymentModeId", false).ToInt(0);
						PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
						if (paymentMode == null)
						{
							HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"请选择支付方式\"}");
							return;
						}
						orderInfo.PaymentTypeId = paymentMode.ModeId;
						orderInfo.Gateway = paymentMode.Gateway;
						orderInfo.PaymentType = paymentMode.Name;
						if (TradeHelper.UpdateOrderPaymentType(orderInfo))
						{
							string empty2 = string.Empty;
							empty2 = ((!(orderInfo.Gateway.ToLower() != "hishop.plugins.payment.advancerequest")) ? ("{\"Status\":1,\"Msg\":\"" + $"/user/pay.aspx?OrderId={this.orderId}" + "\"}") : ((!(orderInfo.Gateway.ToLower() == "hishop.plugins.payment.bankrequest")) ? ("{\"Status\":1,\"Msg\":\"" + base.GetRouteUrl("sendPayment", new
							{
								this.orderId
							}) + "\"}") : ("{\"Status\":1,\"Msg\":\"" + Globals.FullPath(base.GetRouteUrl("bank_pay", new
							{
								orderInfo.OrderId
							})) + "\"}")));
							HttpContext.Current.Response.Write(empty2);
							HttpContext.Current.Response.End();
							return;
						}
						goto IL_058f;
					}
					HiContext.Current.Context.Response.Write("{\"Status\":0,\"Msg\":\"错误的订单信息\"}");
					HttpContext.Current.Response.End();
					return;
				}
			}
			goto IL_058f;
			IL_058f:
			if (string.IsNullOrEmpty(this.orderId))
			{
				base.GotoResourceNotFound();
			}
			this.litOrderId = (Literal)this.FindControl("litOrderId");
			this.litOrderPrice = (FormatedMoneyLabel)this.FindControl("litOrderPrice");
			this.promptMsg = (HtmlGenericControl)this.FindControl("promptMsg");
			this.onlinePayPanel = (HtmlGenericControl)this.FindControl("onlinePayPanel");
			this.btnOrderPay = ButtonManager.Create(this.FindControl("btnOrderPay"));
			this.paymentModeList = (Common_PaymentModeList)this.FindControl("grd_Common_PaymentModeList");
			this.paymentModeId = (HtmlInputHidden)this.FindControl("paymentModeId");
			this.AdvanceId = (HtmlInputHidden)this.FindControl("AdvanceId");
			this.hidOrderid = (HtmlInputHidden)this.FindControl("hidOrderid");
			this.msgTitle = (HtmlGenericControl)this.FindControl("msgTitle");
			this.hidIspop = (HiddenField)this.FindControl("hidIspop");
			this.userLink = (HtmlAnchor)this.FindControl("userLink");
			this.orderLink = (HtmlAnchor)this.FindControl("orderLink");
			this.divfinish = (HtmlGenericControl)this.FindControl("divfinish");
			this.userLink.HRef = "/User/UserDefault";
			this.divOrderPayInfo = (HtmlGenericControl)this.FindControl("divOrderPayInfo");
			this.imgPayResult = (HtmlImage)this.FindControl("imgPayResult");
			this.hidIsPreSale = (HiddenField)this.FindControl("hidIsPreSale");
			this.litDeposit = (FormatedMoneyLabel)this.FindControl("litDeposit");
			this.litDeposittxt = (Literal)this.FindControl("litDeposittxt");
			this.demodiv = (HtmlGenericControl)this.FindControl("demodiv");
			this.demodiv.Visible = SettingsManager.GetMasterSettings().IsDemoSite;
			this.hidHasTradePassword = (HtmlInputHidden)this.FindControl("hidHasTradePassword");
			this.litOfflinePayContent = (Literal)this.FindControl("litOfflinePayContent");
			this.offlinePayPanel = (HtmlGenericControl)this.FindControl("offlinePayPanel");
			if (!this.Page.IsPostBack)
			{
				this.LoadOrderInfo();
			}
		}

		public void LoadOrderInfo()
		{
			PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(ShoppingProcessor.GetPaymentGateway(EnumPaymentType.OfflinePay));
			if (paymentMode != null)
			{
				this.litOfflinePayContent.SetWhenIsNotNull(paymentMode.Description);
			}
			this.hidOrderid.Value = this.orderId;
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo == null || (orderInfo.UserId != 0 && orderInfo.UserId != HiContext.Current.UserId))
			{
				base.GotoResourceNotFound();
				return;
			}
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			this.hidHasTradePassword.Value = (string.IsNullOrWhiteSpace(user.TradePassword) ? "0" : "1");
			if (orderInfo.ParentOrderId == "-1")
			{
				this.orderLink.HRef = base.GetRouteUrl("user_OrderList", new
				{
					ParentOrderId = this.orderId
				});
				this.litOrderId.Text = OrderHelper.GetOrderIdsByParent(orderInfo.OrderId).Replace(",", "&nbsp;&nbsp;&nbsp;&nbsp;");
			}
			else
			{
				this.orderLink.HRef = base.GetRouteUrl("user_OrderDetails", new
				{
					OrderId = this.orderId
				});
				this.litOrderId.Text = orderInfo.OrderId;
			}
			IList<string> paymentTypes = new List<string>();
			paymentTypes.Add("hishop.plugins.payment.podrequest");
			paymentTypes.Add("hishop.plugins.payment.bankrequest");
			paymentTypes.Add("hishop.plugins.payment.advancerequest");
			if (orderInfo.PreSaleId > 0)
			{
				this.litOrderPrice.Money = orderInfo.Deposit + orderInfo.FinalPayment;
			}
			else
			{
				this.litOrderPrice.Money = orderInfo.GetTotal(false);
			}
			if (orderInfo.PreSaleId > 0)
			{
				this.hidIsPreSale.Value = "1";
				if (!orderInfo.DepositDate.HasValue)
				{
					this.litDeposittxt.Text = "定金：";
					this.litDeposit.Money = orderInfo.Deposit;
				}
				else
				{
					this.litDeposittxt.Text = "尾款：";
					this.litDeposit.Money = orderInfo.FinalPayment;
				}
			}
			this.onlinePayPanel.Visible = false;
			this.offlinePayPanel.Visible = false;
			int num;
			if ((orderInfo.PaymentTypeId != -3 || orderInfo.PaymentTypeId != -2) && orderInfo.Gateway != "hishop.plugins.payment.bankrequest" && orderInfo.PaymentType != "到店支付" && orderInfo.Gateway != "hishop.plugins.payment.podrequest")
			{
				num = ((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay) ? 1 : 0);
				goto IL_02ea;
			}
			num = 0;
			goto IL_02ea;
			IL_02ea:
			if (num != 0)
			{
				this.onlinePayPanel.Visible = true;
			}
			else if (orderInfo.Gateway == "hishop.plugins.payment.bankrequest")
			{
				this.offlinePayPanel.Visible = true;
			}
			string text = "";
			text = ((orderInfo.CountDownBuyId <= 0) ? (HiContext.Current.SiteSettings.CloseOrderDays + "天") : (HiContext.Current.SiteSettings.CountDownTime + "分钟"));
			PaymentModeInfo paymentMode2 = TradeHelper.GetPaymentMode("hishop.plugins.payment.podrequest");
			bool flag = false;
			if ((orderInfo.Gateway == "hishop.plugins.payment.podrequest" || (paymentMode2 != null && paymentMode2.ModeId == orderInfo.PaymentTypeId)) && orderInfo.PreSaleId <= 0)
			{
				flag = true;
			}
			switch (orderInfo.OrderStatus)
			{
			case OrderStatus.WaitBuyerPay:
				if (base.GetParameter("t", false).ToNullString() == "1")
				{
					this.promptMsg.InnerHtml = "银行系统可能会出现延时，如您已经支付，请耐心等待或者联系商家客服！";
					this.msgTitle.InnerHtml = "付款不成功，请重新支付!";
					this.divfinish.Visible = true;
					this.onlinePayPanel.Visible = false;
					this.divOrderPayInfo.Visible = false;
					this.imgPayResult.Src = "/templates/pccommon/images/cart/ordererror.png";
				}
				if (orderInfo.PreSaleId > 0)
				{
					if (orderInfo.DepositDate.HasValue)
					{
						this.promptMsg.InnerHtml = "定金支付成功,请按时支付尾款。";
						this.msgTitle.InnerHtml = "恭喜您，您的定金已支付成功!";
					}
					else
					{
						this.promptMsg.InnerHtml = "请支付定金。";
						this.msgTitle.InnerHtml = "订单提交成功，请您尽快支付定金！";
					}
					this.divfinish.Visible = true;
				}
				if (flag)
				{
					this.msgTitle.InnerHtml = "货到付款订单提交成功,平台将及时给您发货";
				}
				break;
			case OrderStatus.BuyerAlreadyPaid:
				if (orderInfo.PaymentType == "上门自提")
				{
					this.promptMsg.InnerHtml = "您的订单已支付成功,平台将及时给您备货。";
					this.msgTitle.InnerHtml = "恭喜您，您的订单已支付成功!";
				}
				else
				{
					this.promptMsg.InnerHtml = "您的订单已支付成功,平台将及时给您发货。";
					this.msgTitle.InnerHtml = "恭喜您，您的订单已支付成功!";
				}
				this.divfinish.Visible = true;
				break;
			case OrderStatus.Closed:
				this.promptMsg.InnerHtml = "您的订单已关闭，关闭原因：" + orderInfo.CloseReason + "。";
				this.msgTitle.InnerHtml = "您的订单已关闭!";
				this.divfinish.Visible = true;
				break;
			case OrderStatus.Finished:
				this.promptMsg.InnerHtml = "您的订单已完成.";
				this.msgTitle.InnerHtml = "恭喜您，您的订单已完成!";
				this.divfinish.Visible = true;
				break;
			case OrderStatus.SellerAlreadySent:
				this.promptMsg.InnerHtml = "您的订单已发货,请注意收货。";
				this.msgTitle.InnerHtml = "恭喜您，您的订单已发货!";
				this.divfinish.Visible = true;
				break;
			default:
				this.promptMsg.InnerHtml = "您的订单已提交成功。";
				this.divfinish.Visible = true;
				break;
			}
			if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.promptMsg.InnerHtml = "您的订单已支付成功,平台将及时给您发货。";
			}
			else if (orderInfo.PaymentTypeId != -3 && !flag && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				DateTime dateTime;
				if (base.GetParameter("t", false).ToNullString() == "1")
				{
					if (orderInfo.PreSaleId > 0 && orderInfo.DepositDate.HasValue)
					{
						this.promptMsg.InnerHtml = "定金支付成功,请按时支付尾款。";
						ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
						if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
						{
							HtmlGenericControl htmlGenericControl = this.promptMsg;
							string[] obj = new string[5]
							{
								"定金支付成功,请您在",
								null,
								null,
								null,
								null
							};
							dateTime = productPreSaleInfo.PaymentStartDate;
							obj[1] = dateTime.ToString("yyyy-MM-dd");
							obj[2] = "至";
							dateTime = productPreSaleInfo.PreSaleEndDate;
							obj[3] = dateTime.ToString("yyyy-MM-dd");
							obj[4] = "之间支付尾款,过期定金将不予以退还，请您谅解！";
							htmlGenericControl.InnerHtml = string.Concat(obj);
							this.onlinePayPanel.Visible = false;
						}
					}
					else
					{
						this.promptMsg.InnerHtml = "银行系统可能会出现延时，如您已经支付，请耐心等待或者联系商家客服！";
					}
				}
				else if (orderInfo.PreSaleId > 0)
				{
					if (orderInfo.DepositDate.HasValue)
					{
						this.promptMsg.InnerHtml = "定金支付成功,请按时支付尾款。";
						ProductPreSaleInfo productPreSaleInfo2 = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
						if (productPreSaleInfo2.PaymentStartDate > DateTime.Now)
						{
							HtmlGenericControl htmlGenericControl2 = this.promptMsg;
							string[] obj2 = new string[5]
							{
								"定金支付成功,请您在",
								null,
								null,
								null,
								null
							};
							dateTime = productPreSaleInfo2.PaymentStartDate;
							obj2[1] = dateTime.ToString("yyyy-MM-dd");
							obj2[2] = "至";
							dateTime = productPreSaleInfo2.PaymentStartDate;
							obj2[3] = dateTime.ToString("yyyy-MM-dd");
							obj2[4] = "之间支付尾款,过期定金将不予以退还，请您谅解！";
							htmlGenericControl2.InnerHtml = string.Concat(obj2);
							this.onlinePayPanel.Visible = false;
						}
					}
					else
					{
						this.promptMsg.InnerHtml = "请您在订单提交后 <em>" + text + "</em>内完成定金支付，否则订单自动取消。";
					}
				}
				else
				{
					this.promptMsg.InnerHtml = "请您在订单提交后 <em>" + text + "</em>内完成支付，否则订单自动取消。";
				}
			}
			else if (flag)
			{
				this.promptMsg.InnerHtml = "您的订单选择了货到付款,平台将及时给您发货。";
			}
			else if (orderInfo.PaymentTypeId == -3)
			{
				this.promptMsg.InnerHtml = "您的订单选择了上门自提,门店正在备货中。";
			}
			if (this.onlinePayPanel.Visible)
			{
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add(new DataColumn("ModeId"));
				dataTable.Columns.Add(new DataColumn("Name"));
				dataTable.Columns.Add(new DataColumn("GateWay"));
				dataTable.Columns.Add(new DataColumn("Description"));
				dataTable.Columns.Add(new DataColumn("OutHtml"));
				IList<PaymentModeInfo> paymentModes = ShoppingProcessor.GetPaymentModes(PayApplicationType.payOnPC);
				paymentModes = (from item in paymentModes
				where !paymentTypes.Contains(item.Gateway)
				select item).ToList();
				if (HiContext.Current.UserId != 0 && HiContext.Current.User.IsOpenBalance)
				{
					PaymentModeInfo paymentMode3 = ShoppingProcessor.GetPaymentMode("hishop.plugins.payment.advancerequest");
					if (paymentMode3 != null)
					{
						this.AdvanceId.Value = paymentMode3.ModeId.ToString();
						paymentModes.Add(paymentMode3);
					}
				}
				if (paymentModes.Count == 0)
				{
					this.ShowMessage("商城暂未配置支付方式，请稍后支付。", false, "", 1);
					this.onlinePayPanel.Visible = false;
				}
				else
				{
					decimal num2 = default(decimal);
					if (HiContext.Current.User != null && HiContext.Current.User.UserId > 0)
					{
						num2 = HiContext.Current.User.Balance - HiContext.Current.User.RequestBalance;
					}
					string format = "<a class=\"pic\"><b><img src=\"/pay/images/{0}.png\" alt=\"{1}\"></b> <span>{1}</span></a>";
					string text2 = "<a class=\"name\"><em>{0}</em> <b>可用余额 ¥{1}</b> <span><a href=\"/user/RechargeRequest.aspx\" target=\"_blank\" style=\"line-height:4.5\">去充值</a></span></a>";
					string format2 = "<a class=\"name\"><em>{0}</em></a>";
					int num3 = 0;
					foreach (PaymentModeInfo item in paymentModes)
					{
						if (!(item.Gateway.ToLower() == "hishop.plugins.payment.advancerequest") && (orderInfo.FightGroupActivityId <= 0 || TradeHelper.GatewayIsCanBackReturn(item.Gateway) || !(item.Gateway.ToLower() != "hishop.plugins.payment.advancerequest")))
						{
							DataRow dataRow = dataTable.NewRow();
							dataRow["ModeId"] = item.ModeId;
							dataRow["Name"] = item.Name;
							dataRow["Gateway"] = item.Gateway;
							dataRow["Description"] = item.Description;
							if (!(item.Gateway.ToLower() == "hishop.plugins.payment.advancerequest"))
							{
								if (File.Exists(this.Page.Server.MapPath("/pay/images/" + item.Gateway + ".png")))
								{
									dataRow["OutHtml"] = string.Format(format, item.Gateway, item.Name);
								}
								else
								{
									dataRow["OutHtml"] = string.Format(format2, item.Name);
								}
								dataTable.Rows.Add(dataRow);
								num3++;
							}
						}
					}
					this.paymentModeList.DataSource = dataTable;
					this.paymentModeList.DataBind();
				}
			}
		}
	}
}
