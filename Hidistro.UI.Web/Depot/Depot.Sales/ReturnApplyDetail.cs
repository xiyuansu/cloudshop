using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.App_Code;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.Sales
{
	public class ReturnApplyDetail : StoreAdminPage
	{
		private string credentialsImgHtml = "<a href=\"{0}\" target=\"_blank\"><img src=\"{0}\" style=\"max-height:60px;\" /></a>";

		private int UserStoreId = 0;

		public string AfterSaleType = "退货";

		protected HiddenField hidRefundMaxAmount;

		protected HiddenField hidReturnStatus;

		protected HiddenField hidAfterSaleType;

		protected Literal txtAfterSaleId;

		protected Literal txtStatus;

		protected HtmlInputButton btnViewLogistic;

		protected Repeater listPrducts;

		protected Literal litOrderId;

		protected Literal litRefundReason;

		protected FormatedMoneyLabel litRefundTotal;

		protected Literal txtPayMoney;

		protected Literal litType;

		protected Literal litReturnQuantity;

		protected Literal litRemark;

		protected FormatedMoneyLabel litOrderTotal;

		protected HtmlGenericControl divCredentials;

		protected Literal litImageList;

		protected HtmlGenericControl showPanel;

		protected Literal litRefundMoney;

		protected HtmlGenericControl inputPanel;

		protected TextBox txtRefundMoney;

		protected Literal litAdminShipAddrss;

		protected TextBox txtAdminShipAddress;

		protected Literal litAdminShipTo;

		protected TextBox txtAdminShipTo;

		protected Literal litAdminCellPhone;

		protected TextBox txtAdminCellPhone;

		protected TextBox txtAdminRemark;

		protected Button btnAcceptReturn;

		protected Button btnRefuseReturn;

		protected Button btnGetGoods;

		protected Button btnFinishReturn;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			if (!this.Page.IsPostBack)
			{
				this.bindReturnInfo();
			}
			this.btnAcceptReturn.Click += this.btnAcceptReturn_Click;
			this.btnRefuseReturn.Click += this.btnRefuseReturn_Click;
			this.btnGetGoods.Click += this.btnGetGoods_Click;
			this.btnFinishReturn.Click += this.btnFinishReturn_Click;
		}

		public void bindReturnInfo()
		{
			int returnId = this.Page.Request["ReturnId"].ToInt(0);
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
			if (returnInfo == null)
			{
				this.ShowMsg("退货信息错误!", false);
				return;
			}
			HiddenField hiddenField = this.hidReturnStatus;
			int num = (int)returnInfo.HandleStatus;
			hiddenField.Value = num.ToString();
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
			if (orderInfo == null)
			{
				this.ShowMsg("错误的订单信息!", false);
				return;
			}
			if (orderInfo.StoreId != this.UserStoreId)
			{
				this.ShowMsg("不是门店的订单不能进行处理", false);
				return;
			}
			if (string.IsNullOrEmpty(returnInfo.SkuId))
			{
				this.listPrducts.DataSource = orderInfo.LineItems.Values;
			}
			else
			{
				Dictionary<string, LineItemInfo> dictionary = new Dictionary<string, LineItemInfo>();
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					if (value.SkuId == returnInfo.SkuId)
					{
						dictionary.Add(value.SkuId, value);
					}
				}
				this.listPrducts.DataSource = dictionary.Values;
			}
			this.listPrducts.DataBind();
			this.litOrderId.Text = orderInfo.PayOrderId;
			this.litOrderTotal.Text = orderInfo.GetTotal(false).F2ToString("f2");
			this.litRefundReason.Text = returnInfo.ReturnReason;
			this.litRefundTotal.Text = returnInfo.RefundAmount.F2ToString("f2");
			this.litRemark.Text = returnInfo.UserRemark;
			Literal literal = this.litReturnQuantity;
			num = returnInfo.Quantity;
			literal.Text = num.ToString();
			if (returnInfo.RefundType == RefundTypes.InBankCard)
			{
				this.litType.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.RefundType, 0) + "(" + returnInfo.BankName + "  " + returnInfo.BankAccountName + "  " + returnInfo.BankAccountNo + ")";
			}
			else
			{
				this.litType.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.RefundType, 0);
			}
			string userCredentials = returnInfo.UserCredentials;
			if (!string.IsNullOrEmpty(userCredentials))
			{
				string[] array = userCredentials.Split('|');
				userCredentials = "";
				string[] array2 = array;
				foreach (string str in array2)
				{
					userCredentials += string.Format(this.credentialsImgHtml, Globals.GetImageServerUrl() + str);
				}
				this.litImageList.Text = userCredentials;
			}
			else
			{
				this.divCredentials.Visible = false;
			}
			if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
			{
				this.btnAcceptReturn.Text = "确认退款";
				this.btnRefuseReturn.Text = "拒绝退款";
				this.AfterSaleType = "退款";
			}
			if (returnInfo.HandleStatus == ReturnStatus.Applied && orderInfo.StoreId == this.UserStoreId && (returnInfo.AfterSaleType != AfterSaleTypes.OnlyRefund || (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund && orderInfo.IsStoreCollect)))
			{
				this.btnAcceptReturn.Visible = true;
				this.btnRefuseReturn.Visible = true;
			}
			if (returnInfo.HandleStatus == ReturnStatus.Deliverying && this.UserStoreId == orderInfo.StoreId && !orderInfo.IsStoreCollect && returnInfo.RefundType != RefundTypes.InBalance)
			{
				this.btnGetGoods.Visible = true;
			}
			int num2;
			if ((orderInfo.IsStoreCollect || returnInfo.RefundType == RefundTypes.InBalance) && orderInfo.StoreId == this.UserStoreId)
			{
				num2 = ((returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Deliverying) ? 1 : 0);
				goto IL_0439;
			}
			num2 = 0;
			goto IL_0439;
			IL_0439:
			if (num2 != 0)
			{
				if (returnInfo.HandleStatus == ReturnStatus.Deliverying)
				{
					this.btnFinishReturn.Text = "确认收货并完成退款";
				}
				this.btnFinishReturn.Visible = true;
			}
			if (returnInfo.HandleStatus != ReturnStatus.Refused && returnInfo.HandleStatus != ReturnStatus.Returned)
			{
				this.inputPanel.Visible = true;
				this.showPanel.Visible = false;
			}
			else
			{
				this.inputPanel.Visible = false;
				this.showPanel.Visible = true;
			}
			if (returnInfo.HandleStatus != 0)
			{
				this.txtAdminCellPhone.Visible = false;
				this.txtAdminShipAddress.Visible = false;
				this.txtAdminShipTo.Visible = false;
				this.litAdminCellPhone.Visible = true;
				this.litAdminShipAddrss.Visible = true;
				this.litAdminShipTo.Visible = true;
				this.litAdminCellPhone.Text = returnInfo.AdminCellPhone;
				this.litAdminShipTo.Text = returnInfo.AdminShipTo;
				this.litAdminShipAddrss.Text = returnInfo.AdminShipAddress;
			}
			else
			{
				StoresInfo storeById = StoresHelper.GetStoreById(HiContext.Current.Manager.StoreId);
				if (storeById != null)
				{
					Literal literal2 = this.litAdminShipAddrss;
					TextBox textBox = this.txtAdminShipAddress;
					string text3 = literal2.Text = (textBox.Text = RegionHelper.GetFullRegion(storeById.RegionId, " ", true, 0) + " " + storeById.Address);
					Literal literal3 = this.litAdminShipTo;
					TextBox textBox2 = this.txtAdminShipTo;
					text3 = (literal3.Text = (textBox2.Text = storeById.ContactMan));
					Literal literal4 = this.litAdminCellPhone;
					TextBox textBox3 = this.txtAdminCellPhone;
					text3 = (literal4.Text = (textBox3.Text = storeById.Tel));
				}
			}
			this.txtAdminRemark.Text = returnInfo.AdminRemark;
			if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
			{
				this.txtStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 3);
			}
			else
			{
				this.txtStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 0);
			}
			this.litRefundMoney.Text = returnInfo.RefundAmount.F2ToString("f2") + "元";
			Literal literal5 = this.txtAfterSaleId;
			num = returnInfo.ReturnId;
			literal5.Text = num.ToString();
			this.txtPayMoney.Text = orderInfo.GetTotal(false).F2ToString("f2");
			this.txtRefundMoney.Text = returnInfo.RefundAmount.F2ToString("f2");
			HiddenField hiddenField2 = this.hidAfterSaleType;
			num = (int)returnInfo.AfterSaleType;
			hiddenField2.Value = num.ToString();
			this.hidRefundMaxAmount.Value = orderInfo.GetCanRefundAmount(returnInfo.SkuId, null, 0).F2ToString("f2");
			if (returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund && (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Returned))
			{
				this.btnViewLogistic.Visible = true;
				AttributeCollection attributes = this.btnViewLogistic.Attributes;
				num = returnInfo.ReturnId;
				attributes.Add("returnsid", num.ToString());
				this.btnViewLogistic.Attributes.Add("expresscompanyname", returnInfo.ExpressCompanyName.ToString());
				this.btnViewLogistic.Attributes.Add("shipordernumber", returnInfo.ShipOrderNumber.ToString());
			}
		}

		private void btnRefuseReturn_Click(object sender, EventArgs e)
		{
			int returnId = this.Page.Request["ReturnId"].ToInt(0);
			string text = Globals.StripAllTags(this.txtAdminRemark.Text);
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请填写拒绝售后原因!", false);
			}
			else
			{
				ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
				if (returnInfo == null)
				{
					this.ShowMsg("售后信息错误!", false);
				}
				else
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(returnInfo.OrderId);
					string skuId = returnInfo.SkuId;
					if (orderInfo == null || orderInfo.StoreId != this.UserStoreId)
					{
						this.ShowMsg("订单不存在,或者订单不属于该门店！", false);
					}
					else if (!orderInfo.LineItems.ContainsKey(skuId))
					{
						this.ShowMsg("订单中不存在要售后的商品！", false);
					}
					else
					{
						LineItemInfo lineItemInfo = orderInfo.LineItems[skuId];
						if (lineItemInfo.Status != LineItemStatus.ReturnApplied)
						{
							this.ShowMsg("售后状态不正确.", false);
						}
						else
						{
							OrderHelper.CheckReturn(returnInfo, orderInfo, HiContext.Current.Manager.UserName, returnInfo.RefundAmount, text, false, false);
							this.ShowMsg("成功的拒绝了订单售后", true, HttpContext.Current.Request.Url.ToString());
						}
					}
				}
			}
		}

		private void btnAcceptReturn_Click(object sender, EventArgs e)
		{
			int num = this.Page.Request["ReturnId"].ToInt(0);
			string text = Globals.StripAllTags(this.txtAdminShipAddress.Text);
			string adminShipTo = Globals.StripAllTags(this.txtAdminShipTo.Text);
			string adminCellPhone = Globals.StripAllTags(this.txtAdminCellPhone.Text);
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(num);
			string adminRemark = Globals.StripAllTags(this.txtAdminRemark.Text);
			string text2 = "退货";
			if (returnInfo == null)
			{
				this.ShowMsg("售后信息错误!", false);
			}
			else
			{
				bool flag = false;
				if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
				{
					text2 = "退款";
					flag = true;
				}
				string skuId = returnInfo.SkuId;
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMsg("订单不存在！", false);
				}
				else if (!orderInfo.LineItems.ContainsKey(skuId))
				{
					this.ShowMsg("订单中不存在要退货的商品！", false);
				}
				else
				{
					LineItemInfo lineItemInfo = orderInfo.LineItems[skuId];
					decimal num2 = default(decimal);
					if (lineItemInfo.Status != LineItemStatus.ReturnApplied)
					{
						this.ShowMsg(text2 + "状态不正确.", false);
					}
					else if (!decimal.TryParse(this.txtRefundMoney.Text, out num2))
					{
						this.ShowMsg("退款金额需为数字格式！", false);
					}
					else if (num2 < decimal.Zero)
					{
						this.ShowMsg("退款金额必须大于等于0", false);
					}
					else
					{
						if (this.UserStoreId != returnInfo.StoreId)
						{
							if (!flag)
							{
								this.ShowMsg("同意" + text2 + "只能由发货的店铺或者平台进行处理!", false);
								return;
							}
							if (orderInfo.IsStoreCollect)
							{
								this.ShowMsg("门店收的款，只能由门店进行退款确认处理!", false);
								return;
							}
						}
						if (!flag && string.IsNullOrEmpty(text))
						{
							this.ShowMsg("请输入平台收货地址，告之用户发货的地址和联系方式", false);
						}
						else
						{
							GroupBuyInfo groupbuy = null;
							if (orderInfo.GroupBuyId > 0)
							{
								groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
							}
							decimal canRefundAmount = orderInfo.GetCanRefundAmount(skuId, groupbuy, 0);
							if (num2 > canRefundAmount)
							{
								this.ShowMsg("退款金额不能大于退货订单或者商品的金额！", false);
							}
							else if (flag)
							{
								RefundTypes refundType = returnInfo.RefundType;
								string userRemark = returnInfo.UserRemark;
								MemberInfo user = Users.GetUser(orderInfo.UserId);
								string text3 = "";
								if (RefundHelper.IsBackReturn(orderInfo.Gateway) && returnInfo.RefundType == RefundTypes.BackReturn)
								{
									text3 = RefundHelper.SendRefundRequest(orderInfo, num2, returnInfo.RefundOrderId, true);
									if (text3 == "")
									{
										if (OrderHelper.AgreedReturns(num, num2, adminRemark, orderInfo, returnInfo.SkuId, text, adminShipTo, adminCellPhone, flag, false))
										{
											VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, returnInfo.SkuId, EnumPushOrderAction.OrderReturnConfirm);
											Messenger.OrderRefund(user, orderInfo, returnInfo.SkuId);
											this.ShowMsg("成功的完成退款并且已成功原路退回退款金额!", true, HttpContext.Current.Request.Url.ToString());
										}
									}
									else
									{
										this.ShowMsg("退款原路返回错误,错误信息" + text3 + ",请重新尝试!", false);
									}
								}
								else if (OrderHelper.AgreedReturns(num, num2, adminRemark, orderInfo, returnInfo.SkuId, text, adminShipTo, adminCellPhone, flag, returnInfo.RefundType == RefundTypes.InBalance))
								{
									Messenger.OrderRefund(user, orderInfo, returnInfo.SkuId);
									VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, returnInfo.SkuId, EnumPushOrderAction.OrderReturnConfirm);
									if (returnInfo.RefundType == RefundTypes.InBalance)
									{
										this.ShowMsg("成功确定了退款,退款金额已退回用户预付款帐号！", true, HttpContext.Current.Request.Url.ToString());
									}
									else
									{
										this.ShowMsg("成功的完成了退款，请即时给用户退款", true, HttpContext.Current.Request.Url.ToString());
									}
								}
							}
							else if (OrderHelper.AgreedReturns(num, num2, adminRemark, orderInfo, returnInfo.SkuId, text, adminShipTo, adminCellPhone, flag, false))
							{
								VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, returnInfo.SkuId, EnumPushOrderAction.OrderReturnConfirm);
								this.ShowMsg("成功的确认了售后", true, HttpContext.Current.Request.Url.ToString());
							}
							else
							{
								this.ShowMsg("确认售后失败！", false);
							}
						}
					}
				}
			}
		}

		private void btnGetGoods_Click(object sender, EventArgs e)
		{
			int num = this.Page.Request["ReturnId"].ToInt(0);
			string text = "";
			decimal num2 = this.txtRefundMoney.Text.ToDecimal(0);
			text = Globals.StripAllTags(this.txtAdminRemark.Text);
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(num);
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
			if (orderInfo == null)
			{
				this.ShowMsg("订单不存在！", false);
			}
			else if (returnInfo == null)
			{
				this.ShowMsg("退货信息错误!", false);
			}
			else if (returnInfo.HandleStatus != ReturnStatus.Deliverying)
			{
				this.ShowMsg("当前状态不允许进行收货!", false);
			}
			else if (this.UserStoreId != returnInfo.StoreId)
			{
				this.ShowMsg("收货处理只能由发货的店铺或者平台进行处理!", false);
			}
			else
			{
				GroupBuyInfo groupbuy = null;
				if (orderInfo.GroupBuyId > 0)
				{
					groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
				}
				decimal canRefundAmount = orderInfo.GetCanRefundAmount(returnInfo.SkuId, groupbuy, 0);
				if (num2 < decimal.Zero)
				{
					this.ShowMsg("退款金额必须大于等于0", false);
				}
				else if (num2 > canRefundAmount)
				{
					this.ShowMsg("退款金额不能大于退货订单或者商品的金额！", false);
				}
				else
				{
					if (string.IsNullOrEmpty(text))
					{
						text = returnInfo.AdminRemark;
					}
					if (TradeHelper.FinishGetGoodsForReturn(num, text, returnInfo.OrderId, returnInfo.SkuId, num2))
					{
						MemberInfo user = Users.GetUser(orderInfo.UserId);
						returnInfo.HandleStatus = ReturnStatus.GetGoods;
						Messenger.AfterSaleDeal(user, orderInfo, returnInfo, null);
						this.ShowMsg("收货成功，由平台进行退款处理！", true, HttpContext.Current.Request.Url.ToString());
					}
					else
					{
						this.ShowMsg("收货失败！", false);
					}
				}
			}
		}

		private void btnFinishReturn_Click(object sender, EventArgs e)
		{
			int returnId = this.Page.Request["ReturnId"].ToInt(0);
			string text = "";
			text = this.txtAdminRemark.Text;
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
			decimal num = this.txtRefundMoney.Text.ToDecimal(0);
			if (orderInfo == null)
			{
				this.ShowMsg("订单不存在！", false);
			}
			else if (orderInfo == null)
			{
				this.ShowMsg("订单不存在！", false);
			}
			else if (returnInfo == null)
			{
				this.ShowMsg("退货信息错误!", false);
			}
			else if ((!orderInfo.IsStoreCollect || this.UserStoreId != orderInfo.StoreId) && returnInfo.RefundType != RefundTypes.InBalance)
			{
				this.ShowMsg("该订单不是门店收款，不能由门店进行完成退货操作", false);
			}
			else if (returnInfo.HandleStatus != ReturnStatus.GetGoods && returnInfo.HandleStatus != ReturnStatus.Deliverying)
			{
				this.ShowMsg("当前状态不允许完成退货!", false);
			}
			else
			{
				if (string.IsNullOrEmpty(text))
				{
					text = returnInfo.AdminRemark;
				}
				string skuId = returnInfo.SkuId;
				if (!orderInfo.LineItems.ContainsKey(skuId))
				{
					this.ShowMsg("订单中不存在要退货的商品！", false);
				}
				else
				{
					LineItemInfo lineItemInfo = orderInfo.LineItems[skuId];
					if (this.UserStoreId != 0 && lineItemInfo.Status != LineItemStatus.DeliveryForReturn && lineItemInfo.Status != LineItemStatus.GetGoodsForReturn)
					{
						this.ShowMsg("当前状态不允许完成退货.", false);
					}
					else
					{
						GroupBuyInfo groupbuy = null;
						if (orderInfo.GroupBuyId > 0)
						{
							groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
						}
						decimal canRefundAmount = orderInfo.GetCanRefundAmount(returnInfo.SkuId, groupbuy, 0);
						if (num < decimal.Zero)
						{
							this.ShowMsg("退款金额必须大于等于0", false);
						}
						else if (num > canRefundAmount)
						{
							this.ShowMsg("退款金额不能大于退货订单或者商品的金额！", false);
						}
						else
						{
							MemberInfo user = Users.GetUser(orderInfo.UserId);
							string text2 = "";
							if (RefundHelper.IsBackReturn(orderInfo.Gateway) && returnInfo.RefundType == RefundTypes.BackReturn)
							{
								text2 = RefundHelper.SendRefundRequest(orderInfo, num, returnInfo.RefundOrderId, false);
								if (text2 == "")
								{
									if (OrderHelper.CheckReturn(returnInfo, orderInfo, HiContext.Current.Manager.UserName, num, text, true, false))
									{
										this.ShowMsg("成功的完成退货并且已成功原路退回退款金额", true, HttpContext.Current.Request.Url.ToString());
									}
								}
								else
								{
									this.ShowMsg("退货退款原路返回请求失败,错误信息:" + text2 + ",请重试!", false);
								}
							}
							else if (OrderHelper.CheckReturn(returnInfo, orderInfo, HiContext.Current.Manager.UserName, num, text, true, false))
							{
								if (returnInfo.RefundType == RefundTypes.InBalance)
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
	}
}
