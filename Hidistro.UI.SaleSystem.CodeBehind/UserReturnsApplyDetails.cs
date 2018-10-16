using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserReturnsApplyDetails : MemberTemplatedWebControl
	{
		private int returnsId;

		private string orderId;

		private Literal txtOrderId;

		private Common_OrderItems_AfterSales products;

		private HtmlGenericControl divCredentials;

		private Literal txtAfterSaleId;

		private Literal litRemark;

		private Literal litAdminRemark;

		private Literal litType;

		private Literal litStep;

		private Literal litProcess;

		private Literal litTime;

		private string stepTemplate = "<span>买家申请退货</span><span>商家同意申请</span><span>买家退货</span><span>商家确认收货</span><span>退货成功</span>";

		private string processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";

		private string timeTemplate = "<span>{0}<br>{1}</span>";

		private FormatedMoneyLabel litRefundTotal;

		private FormatedMoneyLabel litTotalPrice;

		private FormatedMoneyLabel litOrderTotal;

		private HtmlInputHidden txtAfterSaleType;

		private Literal litUserRemark;

		private Literal litCredentialsImg;

		private Literal litReturnAmount;

		private string credentialsImgHtml = "<img src=\"{0}\" style=\"max-height:60px;\" />";

		private ReturnInfo returns = null;

		private OrderInfo order = null;

		private HiddenField hidExpressCompanyName;

		private HiddenField hidShipOrderNumber;

		private ExpressDropDownList expresslist1;

		private TextBox txtShipOrderNumber1;

		private HtmlInputHidden txtIsRefused;

		private IButton btnSendGoodsReturns;

		private HtmlAnchor lnkToSendGoods;

		private HtmlAnchor lnkReApply;

		private HtmlGenericControl bankRow1;

		private HtmlGenericControl bankRow2;

		private HtmlGenericControl bankRow3;

		private HtmlGenericControl AdminRemarkRow;

		private Literal litBankName;

		private Literal litBankAccountName;

		private Literal litBankAccountNo;

		private HtmlAnchor btnViewLogistic;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReturnsApplyDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.btnSendGoodsReturns = ButtonManager.Create(this.FindControl("btnSendGoodsReturns"));
			this.divCredentials = (HtmlGenericControl)this.FindControl("divCredentials");
			this.returnsId = base.GetParameter("ReturnsId", false).ToInt(0);
			this.products = (Common_OrderItems_AfterSales)this.FindControl("Common_OrderItems_AfterSales");
			this.txtOrderId = (Literal)this.FindControl("txtOrderId");
			this.litRemark = (Literal)this.FindControl("litRemark");
			this.litAdminRemark = (Literal)this.FindControl("litAdminRemark");
			this.litType = (Literal)this.FindControl("litType");
			this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
			this.litRefundTotal = (FormatedMoneyLabel)this.FindControl("litRefundTotal");
			this.litOrderTotal = (FormatedMoneyLabel)this.FindControl("litOrderTotal");
			this.expresslist1 = (ExpressDropDownList)this.FindControl("expressDropDownList");
			this.hidExpressCompanyName = (HiddenField)this.FindControl("hidExpressCompanyName");
			this.hidShipOrderNumber = (HiddenField)this.FindControl("hidShipOrderNumber");
			this.txtAfterSaleType = (HtmlInputHidden)this.FindControl("txtAfterSaleType");
			this.litStep = (Literal)this.FindControl("litStep");
			this.litTime = (Literal)this.FindControl("litTime");
			this.litProcess = (Literal)this.FindControl("litProcess");
			this.litUserRemark = (Literal)this.FindControl("litUserRemark");
			this.litCredentialsImg = (Literal)this.FindControl("litCredentialsImg");
			this.txtAfterSaleId = (Literal)this.FindControl("txtAfterSaleId");
			this.txtIsRefused = (HtmlInputHidden)this.FindControl("txtIsRefused");
			this.lnkToSendGoods = (HtmlAnchor)this.FindControl("lnkToSendGoods");
			this.lnkReApply = (HtmlAnchor)this.FindControl("lnkReApply");
			this.litReturnAmount = (Literal)this.FindControl("litReturnAmount");
			this.litBankName = (Literal)this.FindControl("litBankName");
			this.litBankAccountName = (Literal)this.FindControl("litBankAccountName");
			this.litBankAccountNo = (Literal)this.FindControl("litBankAccountNo");
			this.bankRow1 = (HtmlGenericControl)this.FindControl("bankRow1");
			this.bankRow2 = (HtmlGenericControl)this.FindControl("bankRow2");
			this.bankRow3 = (HtmlGenericControl)this.FindControl("bankRow3");
			this.AdminRemarkRow = (HtmlGenericControl)this.FindControl("AdminRemarkRow");
			this.btnViewLogistic = (HtmlAnchor)this.FindControl("btnViewLogistic");
			this.btnSendGoodsReturns.Click += this.btnSendGoodsReturns_Click;
			this.returns = TradeHelper.GetReturnInfo(this.returnsId);
			this.txtAfterSaleId.Text = this.returnsId.ToString();
			if (this.returns == null)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("退货信息不存在或者不属于当前用户"));
			}
			else
			{
				this.order = TradeHelper.GetOrderInfo(this.returns.OrderId);
				if (this.order == null || this.order.UserId != HiContext.Current.UserId)
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
				}
				else if (!this.Page.IsPostBack)
				{
					if (this.expresslist1 != null)
					{
						this.expresslist1.ShowAllExpress = true;
						this.expresslist1.DataBind();
					}
					this.BindReturnsTable(this.returnsId);
					this.BindOrderItems(this.order);
					this.BindProducts(this.order, this.returns.SkuId);
				}
			}
		}

		private void BindProducts(OrderInfo order, string SkuId = "")
		{
			if (string.IsNullOrEmpty(SkuId))
			{
				this.products.DataSource = order.LineItems.Values;
			}
			else
			{
				Dictionary<string, LineItemInfo> dictionary = new Dictionary<string, LineItemInfo>();
				foreach (LineItemInfo value in order.LineItems.Values)
				{
					if (value.SkuId == SkuId)
					{
						dictionary.Add(value.SkuId, value);
					}
				}
				this.products.DataSource = dictionary.Values;
			}
			this.products.DataBind();
		}

		private void BindOrderItems(OrderInfo order)
		{
			this.litTotalPrice.Money = order.GetTotal(false);
			this.litOrderTotal.Money = order.GetTotal(false);
			this.txtOrderId.Text = order.OrderId;
		}

		private void BindReturnsTable(int returnsId)
		{
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnsId);
			int num;
			DateTime dateTime;
			DateTime value;
			DateTime dateTime2;
			string text;
			if (returnInfo != null)
			{
				if (returnInfo.HandleStatus == ReturnStatus.Refused && (this.order.OrderStatus == OrderStatus.SellerAlreadySent || (this.order.OrderStatus == OrderStatus.Finished && !this.order.IsServiceOver)))
				{
					this.lnkReApply.Visible = true;
					this.lnkReApply.HRef = "AfterSalesApply?OrderId=" + returnInfo.OrderId + "&SkuId=" + returnInfo.SkuId;
				}
				if (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed || returnInfo.HandleStatus == ReturnStatus.Deliverying)
				{
					this.lnkToSendGoods.Visible = true;
					AttributeCollection attributes = this.lnkToSendGoods.Attributes;
					num = returnInfo.ReturnId;
					attributes.Add("ReturnId", num.ToString());
					if (returnInfo.HandleStatus == ReturnStatus.Deliverying)
					{
						this.lnkToSendGoods.InnerText = "修改发货信息";
					}
					else
					{
						this.lnkToSendGoods.InnerText = "发货";
					}
				}
				HtmlInputHidden htmlInputHidden = this.txtAfterSaleType;
				num = (int)returnInfo.AfterSaleType;
				htmlInputHidden.Value = num.ToString();
				text = "";
				dateTime = (returnInfo.AgreedOrRefusedTime.HasValue ? returnInfo.AgreedOrRefusedTime.Value : this.returns.ApplyForTime);
				if (returnInfo.FinishTime.HasValue)
				{
					DateTime? finishTime = returnInfo.FinishTime;
					value = DateTime.MinValue;
					if (!(finishTime == (DateTime?)value))
					{
						dateTime2 = returnInfo.FinishTime.Value;
						goto IL_01c9;
					}
				}
				dateTime2 = this.returns.ApplyForTime;
				goto IL_01c9;
			}
			return;
			IL_01c9:
			DateTime dateTime3 = dateTime2;
			DateTime dateTime4 = returnInfo.UserSendGoodsTime.HasValue ? returnInfo.UserSendGoodsTime.Value : dateTime;
			DateTime dateTime5 = returnInfo.ConfirmGoodsTime.HasValue ? returnInfo.ConfirmGoodsTime.Value : dateTime4;
			Literal literal = this.litReturnAmount;
			num = returnInfo.Quantity;
			literal.Text = num.ToString();
			this.litRefundTotal.Text = this.returns.RefundAmount.F2ToString("f2");
			if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
			{
				this.stepTemplate = "<span>买家申请退款</span><span>商家同意申请</span><span>退款完成</span>";
				if (returnInfo.HandleStatus == ReturnStatus.Applied)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
					string format = this.timeTemplate;
					value = returnInfo.ApplyForTime;
					string arg = value.ToString("yyyy-MM-dd");
					value = returnInfo.ApplyForTime;
					text = string.Format(format, arg, value.ToString("HH:mm:ss"));
				}
				else if (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
					string format2 = this.timeTemplate;
					value = returnInfo.ApplyForTime;
					string arg2 = value.ToString("yyyy-MM-dd");
					value = returnInfo.ApplyForTime;
					text = string.Format(format2, arg2, value.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				}
				else if (returnInfo.HandleStatus == ReturnStatus.Returned)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
					string format3 = this.timeTemplate;
					value = returnInfo.ApplyForTime;
					string arg3 = value.ToString("yyyy-MM-dd");
					value = returnInfo.ApplyForTime;
					text = string.Format(format3, arg3, value.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd"), dateTime3.ToString("HH:mm:ss"));
				}
				else if (returnInfo.HandleStatus == ReturnStatus.Refused)
				{
					this.txtIsRefused.Value = "1";
					this.stepTemplate = "<span>买家申请退款</span><span>商家拒绝申请</span><span>退款失败</span>";
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
					string format4 = this.timeTemplate;
					value = returnInfo.ApplyForTime;
					string arg4 = value.ToString("yyyy-MM-dd");
					value = returnInfo.ApplyForTime;
					text = string.Format(format4, arg4, value.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd"), dateTime3.ToString("HH:mm:ss"));
				}
			}
			else if (returnInfo.HandleStatus == ReturnStatus.Applied)
			{
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
				string format5 = this.timeTemplate;
				value = returnInfo.ApplyForTime;
				string arg5 = value.ToString("yyyy-MM-dd");
				value = returnInfo.ApplyForTime;
				text = string.Format(format5, arg5, value.ToString("HH:mm:ss"));
			}
			else if (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed)
			{
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
				string format6 = this.timeTemplate;
				value = returnInfo.ApplyForTime;
				string arg6 = value.ToString("yyyy-MM-dd");
				value = returnInfo.ApplyForTime;
				text = string.Format(format6, arg6, value.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
			}
			else if (returnInfo.HandleStatus == ReturnStatus.Deliverying)
			{
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
				string format7 = this.timeTemplate;
				value = returnInfo.ApplyForTime;
				string arg7 = value.ToString("yyyy-MM-dd");
				value = returnInfo.ApplyForTime;
				text = string.Format(format7, arg7, value.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime4.ToString("yyyy-MM-dd"), dateTime4.ToString("HH:mm:ss"));
			}
			else if (returnInfo.HandleStatus == ReturnStatus.GetGoods)
			{
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
				string str = text;
				string format8 = this.timeTemplate;
				value = returnInfo.ApplyForTime;
				string arg8 = value.ToString("yyyy-MM-dd");
				value = returnInfo.ApplyForTime;
				text = str + string.Format(format8, arg8, value.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime4.ToString("yyyy-MM-dd"), dateTime4.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime5.ToString("yyyy-MM-dd"), dateTime5.ToString("HH:mm:ss"));
			}
			else if (returnInfo.HandleStatus == ReturnStatus.Returned)
			{
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
				string str2 = text;
				string format9 = this.timeTemplate;
				value = returnInfo.ApplyForTime;
				string arg9 = value.ToString("yyyy-MM-dd");
				value = returnInfo.ApplyForTime;
				text = str2 + string.Format(format9, arg9, value.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime4.ToString("yyyy-MM-dd"), dateTime4.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime5.ToString("yyyy-MM-dd"), dateTime5.ToString("HH:mm:ss"));
				text += string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd"), dateTime3.ToString("HH:mm:ss"));
			}
			else if (returnInfo.HandleStatus == ReturnStatus.Refused)
			{
				this.txtIsRefused.Value = "1";
				this.stepTemplate = "<span>买家申请退货</span><span>商家拒绝申请</span><span>退货失败</span>";
				this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
			}
			this.litStep.Text = this.stepTemplate;
			this.litTime.Text = text;
			this.litProcess.Text = this.processTemplate;
			this.litUserRemark.Text = returnInfo.UserRemark;
			string userCredentials = returnInfo.UserCredentials;
			if (string.IsNullOrEmpty(userCredentials))
			{
				this.divCredentials.Visible = false;
			}
			else
			{
				string[] array = userCredentials.Split('|');
				userCredentials = "";
				string[] array2 = array;
				foreach (string str3 in array2)
				{
					userCredentials += string.Format(this.credentialsImgHtml, Globals.GetImageServerUrl() + str3);
				}
				this.litCredentialsImg.Text = userCredentials;
			}
			if (!string.IsNullOrEmpty(returnInfo.AdminRemark))
			{
				this.litAdminRemark.Text = returnInfo.AdminRemark;
				if (this.AdminRemarkRow != null)
				{
					this.AdminRemarkRow.Visible = true;
				}
			}
			else if (this.AdminRemarkRow != null)
			{
				this.AdminRemarkRow.Visible = false;
			}
			this.litAdminRemark.Text = returnInfo.AdminRemark;
			this.litRemark.Text = returnInfo.ReturnReason;
			this.litType.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.RefundType, 0);
			this.orderId = returnInfo.OrderId;
			if (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed || returnInfo.HandleStatus == ReturnStatus.Deliverying)
			{
				this.btnSendGoodsReturns.Visible = true;
			}
			if (returnInfo.RefundType == RefundTypes.InBankCard)
			{
				this.bankRow1.Visible = true;
				this.bankRow2.Visible = true;
				this.bankRow3.Visible = true;
				this.litBankName.Text = returnInfo.BankName;
				this.litBankAccountName.Text = returnInfo.BankAccountName;
				this.litBankAccountNo.Text = returnInfo.BankAccountNo;
			}
			if (this.returns.AfterSaleType == AfterSaleTypes.ReturnAndRefund && (this.returns.HandleStatus == ReturnStatus.Deliverying || this.returns.HandleStatus == ReturnStatus.GetGoods || this.returns.HandleStatus == ReturnStatus.Returned))
			{
				this.btnViewLogistic.Visible = true;
				AttributeCollection attributes2 = this.btnViewLogistic.Attributes;
				num = this.returns.ReturnId;
				attributes2.Add("returnsid", num.ToString());
			}
		}

		private void btnSendGoodsReturns_Click(object sender, EventArgs e)
		{
			string text = base.GetParameter("ReturnUrl", false).ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				text = this.Page.Request.UrlReferrer.ToNullString();
				if (text == this.Page.Request.Url.ToString())
				{
					text = "/User/UserOrders";
				}
			}
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(this.returnsId);
			if (returnInfo == null)
			{
				this.ShowMessage("错误的退货信息", false, "", 1);
			}
			else
			{
				string skuId = returnInfo.SkuId;
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMessage("错误的订单信息", false, "", 1);
				}
				else if (orderInfo.LineItems.ContainsKey(skuId))
				{
					if (orderInfo.LineItems[skuId].Status != LineItemStatus.MerchantsAgreedForReturn && orderInfo.LineItems[skuId].Status != LineItemStatus.DeliveryForReturn)
					{
						this.ShowMessage("商品退货状态不正确", false, "", 1);
					}
					else
					{
						string value = this.hidExpressCompanyName.Value;
						string value2 = this.hidShipOrderNumber.Value;
						if (string.IsNullOrEmpty(value))
						{
							this.ShowMessage("请选择一个快递公司！", false, "", 1);
						}
						else
						{
							string text2 = "";
							string text3 = "";
							ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(value);
							if (value != null)
							{
								text2 = expressCompanyInfo.Kuaidi100Code;
								text3 = expressCompanyInfo.Name;
								if (value2.Trim() == "" || value2.Length > 20)
								{
									this.ShowMessage("请输入快递编号，长度为1-20位！", false, "", 1);
								}
								else if (TradeHelper.UserSendGoodsForReturn(returnInfo.ReturnId, text2, text3, value2, orderInfo.OrderId, skuId))
								{
									if (text2.ToUpper() == "HTKY")
									{
										ExpressHelper.GetDataByKuaidi100(text2, value2);
									}
									this.ShowMessage("发货成功", true, text, 2);
									this.BindOrderItems(orderInfo);
								}
								else
								{
									this.ShowMessage("发货失败！", false, "", 1);
								}
							}
							else
							{
								this.ShowMessage("请选择快递公司", false, "", 1);
							}
						}
					}
				}
				else
				{
					this.ShowMessage("订单中不包含商品信息", false, "", 1);
				}
			}
		}
	}
}
