using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.App_Code;
using System;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class RefundApplyDetail : AdminPage
	{
		public int UserStoreId = 0;

		private RefundInfo refund = null;

		private OrderInfo order = null;

		protected HiddenField hidRefundMaxAmount;

		protected Literal txtAfterSaleId;

		protected Literal txtStatus;

		protected Repeater listPrducts;

		protected Literal litOrderId;

		protected Literal litRefundReason;

		protected FormatedMoneyLabel litRefundTotal;

		protected Literal txtPayMoney;

		protected Literal litType;

		protected Literal litRemark;

		protected FormatedMoneyLabel litOrderTotal;

		protected HtmlGenericControl showPanel;

		protected Literal litRefundMoney;

		protected HtmlGenericControl inputPanel;

		protected TextBox txtRefundMoney;

		protected TextBox txtAdminRemark;

		protected Button btnAcceptRefund;

		protected Button btnRefuseRefund;

		protected RefundInfo RefundData
		{
			get;
			set;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAcceptRefund.Click += this.btnAcceptRefund_Click;
			this.btnRefuseRefund.Click += this.btnRefuseRefund_Click;
			int refundId = this.Page.Request["RefundId"].ToInt(0);
			this.refund = TradeHelper.GetRefundInfo(refundId);
			this.RefundData = this.refund;
			if (this.refund == null)
			{
				this.ShowMsg("退款信息错误!", false);
			}
			else
			{
				this.order = TradeHelper.GetOrderInfo(this.refund.OrderId);
				if (this.order == null)
				{
					this.ShowMsg("错误的订单信息!", false);
				}
				else if (!this.Page.IsPostBack)
				{
					this.bindRefundInfo();
				}
			}
		}

		public void bindRefundInfo()
		{
			this.listPrducts.DataSource = this.order.LineItems.Values;
			this.litOrderId.Text = this.order.PayOrderId;
			this.litOrderTotal.Text = this.order.GetTotal(false).F2ToString("f2");
			this.litRefundReason.Text = this.refund.RefundReason;
			this.litRefundTotal.Text = this.refund.RefundAmount.F2ToString("f2");
			this.litRemark.Text = this.refund.UserRemark;
			this.txtAdminRemark.Text = this.refund.AdminRemark;
			this.txtAfterSaleId.Text = this.refund.RefundId.ToString();
			this.txtPayMoney.Text = this.order.GetTotal(false).F2ToString("f2");
			this.txtRefundMoney.Text = this.refund.RefundAmount.F2ToString("f2");
			this.txtStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)this.refund.HandleStatus, 0);
			if (this.refund.RefundType == RefundTypes.InBankCard)
			{
				this.litType.Text = EnumDescription.GetEnumDescription((Enum)(object)this.refund.RefundType, 0) + "(" + this.refund.BankName + "  " + this.refund.BankAccountName + "  " + this.refund.BankAccountNo + ")";
			}
			else
			{
				this.litType.Text = EnumDescription.GetEnumDescription((Enum)(object)this.refund.RefundType, 0);
			}
			GroupBuyInfo groupBuyInfo = null;
			if (this.order.GroupBuyId > 0)
			{
				groupBuyInfo = ProductBrowser.GetGroupBuy(this.order.GroupBuyId);
			}
			decimal total = this.order.GetTotal(false);
			this.hidRefundMaxAmount.Value = total.F2ToString("f2");
			this.litRefundMoney.Text = this.refund.RefundAmount.F2ToString("f2");
			if (this.order.FightGroupId > 0 && this.refund.RefundReason.IndexOf("自动退款") > -1)
			{
				this.btnRefuseRefund.Visible = false;
			}
			if (this.refund.HandleStatus != 0)
			{
				this.txtRefundMoney.ReadOnly = true;
				this.btnAcceptRefund.Visible = false;
				this.btnRefuseRefund.Visible = false;
				this.showPanel.Visible = true;
				this.inputPanel.Visible = false;
			}
			else
			{
				this.inputPanel.Visible = true;
				this.showPanel.Visible = false;
			}
		}

		protected void btnAcceptRefund_Click(object sender, EventArgs e)
		{
			bool flag = true;
			string userName = HiContext.Current.Manager.UserName;
			string adminRemark = Globals.StripAllTags(this.txtAdminRemark.Text);
			int refundId = this.Page.Request.QueryString["RefundId"].ToInt(0);
			RefundInfo refundInfo = TradeHelper.GetRefundInfo(refundId);
			if (refundInfo == null)
			{
				this.ShowMsg("错误的退款申请信息", false);
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(refundInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMsg("错误的订单信息", false);
				}
				else
				{
					decimal num = this.txtRefundMoney.Text.ToDecimal(0);
					if (num < decimal.Zero)
					{
						this.ShowMsg("退款金额必须大于等于0", false);
					}
					else
					{
						if (!refundInfo.IsServiceProduct)
						{
							GroupBuyInfo groupbuy = null;
							if (orderInfo.GroupBuyId > 0)
							{
								groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
							}
							if (num > orderInfo.GetCanRefundAmount("", groupbuy, 0))
							{
								this.ShowMsg("退款金额不能大于订单/商品最大金额", false);
								return;
							}
						}
						else if (num > refundInfo.RefundAmount)
						{
							this.ShowMsg("退款金额不能大于可退款总额", false);
							return;
						}
						if (refundInfo.HandleStatus != 0)
						{
							this.ShowMsg("退款状态不正确", false);
						}
						else if (refundInfo.IsServiceProduct || (!refundInfo.IsServiceProduct && OrderHelper.CanFinishRefund(orderInfo, refundInfo, num, false)))
						{
							RefundTypes refundType = refundInfo.RefundType;
							string userRemark = refundInfo.UserRemark;
							MemberInfo user = Users.GetUser(orderInfo.UserId);
							string text = "";
							if (RefundHelper.IsBackReturn(orderInfo.Gateway) && refundInfo.RefundType == RefundTypes.BackReturn)
							{
								text = RefundHelper.SendRefundRequest(orderInfo, num, refundInfo.RefundOrderId, true);
								if (text == "")
								{
									if (OrderHelper.CheckRefund(orderInfo, refundInfo, num, userName, adminRemark, true, false))
									{
										VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
										Messenger.OrderRefund(user, orderInfo, "");
										this.ShowMsg("成功的完成退款并且已成功原路退回退款金额!", true, HttpContext.Current.Request.Url.ToString());
									}
								}
								else
								{
									TradeHelper.SaveRefundErr(refundInfo.RefundId, text, true);
									this.ShowMsg("退款原路返回错误,错误信息" + text + ",请重新尝试!", false);
								}
							}
							else if (OrderHelper.CheckRefund(orderInfo, refundInfo, num, userName, adminRemark, true, false))
							{
								Messenger.OrderRefund(user, orderInfo, "");
								VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
								if (refundInfo.RefundType == RefundTypes.InBalance)
								{
									this.ShowMsg("成功确定了退款,退款金额已退回用户预付款帐号！", true, HttpContext.Current.Request.Url.ToString());
								}
								else
								{
									this.ShowMsg("成功的完成了退款，请即时给用户退款", true, HttpContext.Current.Request.Url.ToString());
								}
							}
						}
					}
				}
			}
		}

		private void btnRefuseRefund_Click(object sender, EventArgs e)
		{
			int refundId = this.Page.Request.QueryString["RefundId"].ToInt(0);
			string text = Globals.StripAllTags(this.txtAdminRemark.Text);
			RefundInfo refundInfo = TradeHelper.GetRefundInfo(refundId);
			if (refundInfo == null)
			{
				this.ShowMsg("退款信息错误!", false);
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(refundInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMsg("错误的订单信息", false);
				}
				else if (string.IsNullOrEmpty(text))
				{
					this.ShowMsg("请填写拒绝退款的原因", false);
				}
				else
				{
					refundInfo.AdminRemark = text;
					decimal num = refundInfo.RefundAmount;
					if (num == decimal.Zero)
					{
						num = orderInfo.GetTotal(false);
					}
					string userName = HiContext.Current.Manager.UserName;
					OrderHelper.CheckRefund(orderInfo, refundInfo, num, userName, text, false, false);
					MemberInfo user = Users.GetUser(orderInfo.UserId);
					Messenger.OrderRefundRefused(user, orderInfo, refundInfo);
					this.ShowMsg("成功的拒绝了订单退款", true, HttpContext.Current.Request.Url.ToString());
				}
			}
		}
	}
}
