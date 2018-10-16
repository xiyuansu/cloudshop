using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AfterSalesApply : MemberTemplatedWebControl
	{
		private string OrderId = "";

		private string SkuId = "";

		private IButton btnSubmit;

		private HtmlInputHidden hdorderId;

		private HtmlInputHidden hidOneMaxRefundAmount;

		private HtmlInputHidden hidMaxRefundAmount;

		private TextBox txtRemark;

		private TextBox txtRefundAmount;

		private HtmlInputText txtQuantity;

		private Literal litMaxAmount;

		private Literal litMaxQuantity;

		private RefundTypeRadioList dropRefundType;

		private HtmlTableRow divQuantity;

		private HtmlTableRow divQuantityTag;

		private Common_OrderItems_AfterSales products;

		private HtmlInputHidden hidMaxQuantity;

		private AfterSalesReasonDropDownList DropReturnsReason;

		private OrderInfo order = null;

		private LineItemInfo ReturnsItem = null;

		private HtmlGenericControl groupbuyPanel;

		private HiddenField hidePicture;

		private HtmlInputText txtBankName;

		private HtmlInputText txtBankAccountName;

		private HtmlInputText txtBankAccountNo;

		private HiddenField hidRefundType;

		private HiddenField hidAfterSaleType;

		private int iRefundType;

		private GroupBuyInfo groupBuy = null;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-AfterSalesApply.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hdorderId = (HtmlInputHidden)this.FindControl("hdorderId");
			this.txtRemark = (TextBox)this.FindControl("txtRemark");
			this.txtRefundAmount = (TextBox)this.FindControl("txtRefundMoney");
			this.dropRefundType = (RefundTypeRadioList)this.FindControl("dropRefundType");
			this.DropReturnsReason = (AfterSalesReasonDropDownList)this.FindControl("RefundReasonDropDownList");
			this.btnSubmit = ButtonManager.Create(this.FindControl("btnSubmit"));
			this.txtQuantity = (HtmlInputText)this.FindControl("txtQuantity");
			this.divQuantity = (HtmlTableRow)this.FindControl("divQuantity");
			this.divQuantityTag = (HtmlTableRow)this.FindControl("divQuantityTag");
			this.litMaxAmount = (Literal)this.FindControl("litMaxAmount");
			this.litMaxQuantity = (Literal)this.FindControl("litMaxQuantity");
			this.groupbuyPanel = (HtmlGenericControl)this.FindControl("groupbuyPanel");
			this.hidRefundType = (HiddenField)this.FindControl("hidRefundType");
			this.hidAfterSaleType = (HiddenField)this.FindControl("hidAfterSaleType");
			this.hidePicture = (HiddenField)this.FindControl("fmSrc");
			this.txtBankName = (HtmlInputText)this.FindControl("txtBankName");
			this.txtBankAccountName = (HtmlInputText)this.FindControl("txtBankAccountName");
			this.txtBankAccountNo = (HtmlInputText)this.FindControl("txtBankAccountNo");
			this.hidMaxQuantity = (HtmlInputHidden)this.FindControl("hidMaxQuantity");
			this.products = (Common_OrderItems_AfterSales)this.FindControl("Common_OrderItems_AfterSales");
			this.hidOneMaxRefundAmount = (HtmlInputHidden)this.FindControl("hidOneMaxRefundAmount");
			this.hidMaxRefundAmount = (HtmlInputHidden)this.FindControl("hidMaxRefundAmount");
			PageTitle.AddSiteNameTitle("我的订单");
			this.OrderId = HttpContext.Current.Request.QueryString["OrderId"].ToNullString();
			this.SkuId = HttpContext.Current.Request.QueryString["SkuId"].ToNullString();
			this.order = TradeHelper.GetOrderInfo(this.OrderId);
			if (this.order == null || this.order.UserId != HiContext.Current.UserId)
			{
				this.ShowMessage("错误的订单信息!", false, "", 1);
			}
			else if (string.IsNullOrEmpty(this.SkuId) || !this.order.LineItems.ContainsKey(this.SkuId))
			{
				this.ShowMessage("错误的商品信息!", false, "", 1);
			}
			else
			{
				if (this.products != null)
				{
					if (string.IsNullOrEmpty(this.SkuId))
					{
						this.products.DataSource = this.order.LineItems.Values;
					}
					else
					{
						Dictionary<string, LineItemInfo> dictionary = new Dictionary<string, LineItemInfo>();
						foreach (LineItemInfo value in this.order.LineItems.Values)
						{
							if (value.SkuId == this.SkuId)
							{
								dictionary.Add(value.SkuId, value);
							}
						}
						this.products.DataSource = dictionary.Values;
					}
					this.products.DataBind();
				}
				decimal canRefundAmount = this.order.GetCanRefundAmount(this.SkuId, this.groupBuy, 0);
				if (canRefundAmount < decimal.Zero)
				{
					this.ShowMessage("订单中已申请的退款金额超过了订单总额!", false, "", 1);
				}
				else
				{
					decimal canRefundAmount2 = this.order.GetCanRefundAmount(this.SkuId, this.groupBuy, 1);
					if (this.order.LineItems.ContainsKey(this.SkuId))
					{
						this.ReturnsItem = this.order.LineItems[this.SkuId];
					}
					this.btnSubmit.Click += this.btnReturns_Click;
					if (!this.Page.IsPostBack)
					{
						this.DropReturnsReason.DataBind();
						this.hidOneMaxRefundAmount.Value = canRefundAmount2.F2ToString("f2");
						this.litMaxAmount.Text = canRefundAmount.F2ToString("f2");
						this.litMaxQuantity.Text = TradeHelper.GetMaxQuantity(this.order, this.SkuId).ToString();
						this.hidMaxQuantity.Value = this.litMaxQuantity.Text;
						this.txtQuantity.Value = this.litMaxQuantity.Text;
						this.txtRefundAmount.Text = this.litMaxAmount.Text;
						this.hidMaxRefundAmount.Value = this.litMaxAmount.Text;
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
						if (this.order.PreSaleId <= 0 || this.order.Gateway.ToLower() == enumDescription || this.order.DepositGatewayOrderId.ToNullString() == enumDescription)
						{
							this.dropRefundType.OrderGateWay = ((this.order.PreSaleId > 0) ? enumDescription : this.order.Gateway);
						}
						this.dropRefundType.BalanceAmount = this.order.BalanceAmount;
						this.dropRefundType.DataBind();
					}
				}
			}
		}

		private void btnReturns_Click(object sender, EventArgs e)
		{
			string text = this.Page.Request.QueryString["returnUrl"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				text = this.Page.Request.UrlReferrer.ToNullString();
				if (text == this.Page.Request.Url.ToString())
				{
					text = "/User/UserOrders";
				}
			}
			int num = 3;
			string text2 = "";
			string text3 = "";
			string text4 = "";
			this.iRefundType = this.hidRefundType.Value.ToInt(0);
			num = this.hidAfterSaleType.Value.ToInt(0);
			if (string.IsNullOrEmpty(this.SkuId) || !this.order.LineItems.ContainsKey(this.SkuId))
			{
				this.ShowMessage("请选择要进行售后的商品", false, "", 1);
			}
			if (!TradeHelper.CanReturn(this.order, this.SkuId))
			{
				this.ShowMessage("该商品正在售后中！", false, "", 1);
			}
			else if (num != 2 && !Enum.IsDefined(typeof(RefundTypes), this.iRefundType))
			{
				this.ShowMessage("错误的退款方式", false, "", 1);
			}
			else
			{
				string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1);
				if ((this.order.Gateway.ToLower() == enumDescription || this.order.DepositGatewayOrderId.ToNullString().ToLower() == enumDescription) && this.iRefundType != 1)
				{
					this.ShowMessage("预付款支付的订单只能退回到预付款帐号", false, "", 1);
				}
				else if (!Enum.IsDefined(typeof(AfterSaleTypes), num))
				{
					this.ShowMessage("错误的售后类型", false, "", 1);
				}
				else
				{
					string userRemark = Globals.StripAllTags(this.txtRemark.Text.Trim());
					if (!this.CanRefundBalance())
					{
						this.ShowMessage("请先开通预付款账户", false, "", 1);
					}
					else
					{
						text2 = Globals.StripAllTags(this.txtBankName.Value);
						text3 = Globals.StripAllTags(this.txtBankAccountName.Value);
						text4 = Globals.StripAllTags(this.txtBankAccountNo.Value);
						string text5 = "";
						string text6 = this.hidePicture.Value.Trim();
						string imageServerUrl = Globals.GetImageServerUrl();
						if (text6.Length > 0)
						{
							string[] array = text6.Split(',');
							for (int i = 0; i < array.Length; i++)
							{
								text5 += (string.IsNullOrEmpty(imageServerUrl) ? (Globals.SaveFile("user\\Credentials", array[i], "/Storage/master/", true, false, "") + "|") : (array[i] + "|"));
							}
						}
						text5 = text5.TrimEnd('|');
						string selectedValue = this.DropReturnsReason.SelectedValue;
						if (string.IsNullOrEmpty(selectedValue))
						{
							this.ShowMessage("请选择售后原因", false, "", 1);
						}
						string refundGateWay = string.IsNullOrEmpty(this.order.Gateway) ? "" : this.order.Gateway.ToLower().Replace(".payment.", ".refund.");
						int num2 = this.ReturnsItem.ShipmentQuantity;
						if (num != 3)
						{
							int.TryParse(this.txtQuantity.Value, out num2);
							if (num2 == 0)
							{
								num2 = TradeHelper.GetMaxQuantity(this.order, this.SkuId);
							}
							else if (this.ReturnsItem != null)
							{
								if (num2 > this.ReturnsItem.ShipmentQuantity)
								{
									this.ShowMessage("数量不能大于购买商品的数量", false, "", 1);
									return;
								}
							}
							else if (num2 > this.order.GetAllQuantity(true))
							{
								this.ShowMessage("数量不能大于订单中商品的数量", false, "", 1);
								return;
							}
						}
						decimal num3 = default(decimal);
						decimal.TryParse(this.txtRefundAmount.Text, out num3);
						decimal canRefundAmount = this.order.GetCanRefundAmount(this.SkuId, this.groupBuy, 1);
						if (num == 3 && canRefundAmount <= decimal.Zero)
						{
							this.ShowMessage("订单支付金额为0时不能进行仅退款操作。", false, "", 1);
						}
						else
						{
							decimal num4 = this.litMaxAmount.Text.ToDecimal(0);
							if (num2 < this.litMaxQuantity.Text.ToInt(0))
							{
								num4 = canRefundAmount * (decimal)num2;
							}
							if (num3 < decimal.Zero && num != 2)
							{
								this.ShowMessage("退款金额必须大于0", false, "", 1);
							}
							else if (num3 > num4)
							{
								this.ShowMessage(string.Format("退款金额不能大于最大可退款金额({0})", num4.F2ToString("f2")), true, "", 1);
							}
							else
							{
								string text7 = (num == 3) ? "退款" : "退货";
								if (num == 3 || num == 1)
								{
									string generateId = Globals.GetGenerateId();
									ReturnInfo returnInfo = new ReturnInfo
									{
										UserRemark = userRemark,
										ReturnReason = selectedValue,
										RefundType = (RefundTypes)this.iRefundType,
										RefundGateWay = refundGateWay,
										RefundOrderId = generateId,
										RefundAmount = num3,
										StoreId = this.order.StoreId,
										ApplyForTime = DateTime.Now,
										BankName = text2,
										BankAccountName = text3,
										BankAccountNo = text4,
										HandleStatus = ReturnStatus.Applied,
										OrderId = this.order.OrderId,
										SkuId = this.SkuId,
										Quantity = num2,
										UserCredentials = text5,
										AfterSaleType = (AfterSaleTypes)num
									};
									if (TradeHelper.ApplyForReturn(returnInfo))
									{
										if (this.order.StoreId > 0)
										{
											VShopHelper.AppPsuhRecordForStore(this.order.StoreId, this.OrderId, this.SkuId, EnumPushStoreAction.StoreOrderReturnApply);
										}
										this.ShowMessage("成功的申请了退货", true, text, 2);
									}
									else
									{
										this.ShowMessage("申请退货失败", false, text, 2);
									}
								}
								else
								{
									ReplaceInfo replace = new ReplaceInfo
									{
										ApplyForTime = DateTime.Now,
										HandleStatus = ReplaceStatus.Applied,
										OrderId = this.order.OrderId,
										Quantity = num2,
										ReplaceReason = selectedValue,
										SkuId = this.SkuId,
										StoreId = this.order.StoreId,
										UserCredentials = text5,
										UserRemark = userRemark
									};
									if (TradeHelper.ApplyForReplace(replace))
									{
										if (this.order.StoreId > 0)
										{
											VShopHelper.AppPsuhRecordForStore(this.order.StoreId, this.OrderId, this.SkuId, EnumPushStoreAction.StoreOrderReplaceApply);
										}
										this.ShowMessage("成功的申请了换货", true, text, 2);
									}
									else
									{
										this.ShowMessage("申请换货失败", false, "", 1);
									}
								}
							}
						}
					}
				}
			}
		}

		private bool CanRefundBalance()
		{
			if (this.iRefundType != 1)
			{
				return true;
			}
			return HiContext.Current.User.IsOpenBalance;
		}
	}
}
