using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Pay : MemberTemplatedWebControl
	{
		private Label lblOrderId;

		private FormatedMoneyLabel lblOrderAmount;

		private FormatedMoneyLabel lblDeposit;

		private FormatedMoneyLabel lblFinalPayment;

		private FormatedMoneyLabel litUseableBalance;

		private TextBox txtPassword;

		private IButton btnPay;

		private HiddenField hidPreSaleId;

		private HiddenField hidIsPayDeposit;

		private string orderId;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-Pay.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.lblOrderId = (Label)this.FindControl("lblOrderId");
			this.lblOrderAmount = (FormatedMoneyLabel)this.FindControl("lblOrderAmount");
			this.txtPassword = (TextBox)this.FindControl("txtPassword");
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			this.btnPay = ButtonManager.Create(this.FindControl("btnPay"));
			this.lblDeposit = (FormatedMoneyLabel)this.FindControl("lblDeposit");
			this.lblFinalPayment = (FormatedMoneyLabel)this.FindControl("lblFinalPayment");
			this.hidPreSaleId = (HiddenField)this.FindControl("hidPreSaleId");
			this.hidIsPayDeposit = (HiddenField)this.FindControl("hidIsPayDeposit");
			this.orderId = base.GetParameter("orderId", false);
			PageTitle.AddSiteNameTitle("订单支付");
			this.btnPay.Click += this.btnPay_Click;
			if (string.IsNullOrEmpty(this.orderId))
			{
				base.GotoResourceNotFound();
			}
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (!user.IsOpenBalance)
				{
					this.Page.Response.Redirect($"/user/OpenBalance.aspx?ReturnUrl={HttpContext.Current.Request.Url}");
				}
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				if (!orderInfo.CheckAction(OrderActions.BUYER_PAY))
				{
					this.ShowMessage("当前的订单订单状态不是等待付款，所以不能支付", false, "", 1);
					this.btnPay.Visible = false;
				}
				this.lblOrderId.Text = orderInfo.OrderId;
				this.lblOrderAmount.Money = orderInfo.GetTotal(true);
				this.litUseableBalance.Money = user.Balance - user.RequestBalance;
				if (orderInfo.PreSaleId > 0)
				{
					this.lblDeposit.Money = orderInfo.Deposit;
					this.lblFinalPayment.Money = orderInfo.FinalPayment;
					this.hidPreSaleId.Value = "1";
					if (orderInfo.DepositDate.HasValue)
					{
						this.hidIsPayDeposit.Value = "1";
					}
				}
			}
		}

		protected void btnPay_Click(object sender, EventArgs e)
		{
			MemberInfo user = HiContext.Current.User;
			if (string.IsNullOrEmpty(user.TradePassword))
			{
				this.Page.Response.Redirect("/user/OpenBalance.aspx");
			}
			string empty = string.Empty;
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (orderInfo.CountDownBuyId > 0)
			{
				foreach (KeyValuePair<string, LineItemInfo> lineItem in orderInfo.LineItems)
				{
					CountDownInfo countDownInfo = TradeHelper.CheckUserCountDown(lineItem.Value.ProductId, orderInfo.CountDownBuyId, lineItem.Value.SkuId, HiContext.Current.UserId, orderInfo.GetAllQuantity(true), orderInfo.OrderId, out empty, orderInfo.StoreId);
					if (countDownInfo == null)
					{
						this.ShowMessage(empty, false, "", 1);
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
						this.ShowMessage(empty, false, "", 1);
						return;
					}
				}
			}
			if (orderInfo.GroupBuyId > 0)
			{
				GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(orderInfo.GroupBuyId);
				if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
				{
					this.ShowMessage("当前的订单为团购订单，此团购活动已结束，所以不能支付", false, "", 1);
					return;
				}
				num2 = TradeHelper.GetOrderCount(orderInfo.GroupBuyId);
				num3 = orderInfo.GetGroupBuyOerderNumber();
				num = groupBuy.MaxCount;
				if (num < num2 + num3)
				{
					this.ShowMessage("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付", false, "", 1);
					return;
				}
			}
			if (orderInfo.PreSaleId > 0)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
				if (productPreSaleInfo == null)
				{
					this.ShowMessage("预售活动不存在不能支付", false, "", 1);
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
			if (!orderInfo.CheckAction(OrderActions.BUYER_PAY))
			{
				this.ShowMessage("当前的订单订单状态不是等待付款，所以不能支付", false, "", 1);
			}
			else if (HiContext.Current.UserId != orderInfo.UserId)
			{
				this.ShowMessage("预付款只能为自己下的订单付款,查一查该订单是不是你的", false, "", 1);
			}
			else if ((decimal)this.litUseableBalance.Money < orderInfo.GetTotal(false))
			{
				this.ShowMessage("预付款余额不足,支付失败", false, "", 1);
			}
			else if (MemberProcessor.ValidTradePassword(this.txtPassword.Text))
			{
				string str = "";
				if (!TradeHelper.CheckOrderStock(orderInfo, out str))
				{
					this.ShowMessage("订单中有商品(" + str + ")库存不足", false, "", 1);
				}
				else
				{
					Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
					foreach (LineItemInfo value in lineItems.Values)
					{
						int skuStock = ShoppingCartProcessor.GetSkuStock(value.SkuId, 0);
						if (skuStock < value.ShipmentQuantity)
						{
							this.ShowMessage("订单中商品库存不足，禁止支付！", false, "", 1);
							return;
						}
					}
					if (TradeHelper.UserPayOrder(orderInfo, true, false))
					{
						if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
						{
							TradeHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
						}
						if (orderInfo.ParentOrderId == "-1")
						{
							OrderQuery orderQuery = new OrderQuery();
							orderQuery.ParentOrderId = orderInfo.OrderId;
							IList<OrderInfo> listUserOrder = MemberProcessor.GetListUserOrder(orderInfo.UserId, orderQuery);
							foreach (OrderInfo item in listUserOrder)
							{
								OrderHelper.OrderConfirmPaySendMessage(item);
							}
						}
						else
						{
							OrderHelper.OrderConfirmPaySendMessage(orderInfo);
						}
						this.Page.Response.Redirect("/user/PaySucceed.aspx?orderId=" + this.orderId);
					}
					else
					{
						this.ShowMessage($"对订单{orderInfo.OrderId} 支付失败", false, "", 1);
					}
				}
			}
			else
			{
				this.ShowMessage("交易密码有误，请重试", false, "", 1);
			}
		}
	}
}
