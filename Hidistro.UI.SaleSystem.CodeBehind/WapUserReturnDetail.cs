using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapUserReturnDetail : WAPMemberTemplatedWebControl
	{
		private Literal txtOrderId;

		private Literal txtRefundMoney;

		private Literal txtRefundType;

		private Literal txtMemo;

		private Literal txtStatus;

		private Literal txtAdminRemark;

		private Literal txtQuantity;

		private Literal txtExpress;

		private Literal txtExpressNo;

		private Literal txtReturnReason;

		private HtmlInputHidden hidUploadImages;

		private HtmlInputHidden hidErrorMsg;

		private HtmlInputHidden hidOldImages;

		private HtmlInputHidden txtReturnsId;

		private HtmlInputHidden txtIsRefund;

		private HtmlInputButton btnReferral;

		private HtmlInputButton btnSendGoods;

		private HtmlInputButton btnLogistic;

		private HtmlGenericControl ExpressRow;

		private HtmlGenericControl ExpressNumberRow;

		private HtmlGenericControl AdminRemarkRow;

		private Literal litCredentialsImg;

		private Common_OrderItem_AfterSales products;

		private Literal txtAfterSaleId;

		private Literal litStep;

		private Literal litStatus;

		private string activityStyle = "class=\"returns_step_active\"";

		private string stepTemplate = "<li {style}><div class=\"logistics_info\"><span>{StatusText}</span>{time}</li>";

		private string timeTemplate = "<span class=\"color_6\">{0}</span>";

		private string remarkTemplate = "<span class=\"color_6\">{0}</span>";

		private string credentialsImgHtml = "<a href=\"{0}\" class=\"preview\" target=\"_blank\"><img src=\"/Admin/PicRar.aspx?P={0}&W=50&H=50\"/></a>";

		private HtmlGenericControl bankRow1;

		private HtmlGenericControl bankRow2;

		private HtmlGenericControl bankRow3;

		private Literal litBankName;

		private Literal litBankAccountName;

		private Literal litBankAccountNo;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-ReturnsDetail.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.btnReferral = (HtmlInputButton)this.FindControl("btnReferral");
			this.btnSendGoods = (HtmlInputButton)this.FindControl("btnSendGoods");
			this.btnLogistic = (HtmlInputButton)this.FindControl("btnLogistic");
			this.hidUploadImages = (HtmlInputHidden)this.FindControl("hidUploadImages");
			this.hidErrorMsg = (HtmlInputHidden)this.FindControl("hidErrorMsg");
			this.hidOldImages = (HtmlInputHidden)this.FindControl("hidOldImages");
			this.txtReturnsId = (HtmlInputHidden)this.FindControl("txtReturnsId");
			this.txtOrderId = (Literal)this.FindControl("txtOrderId");
			this.txtRefundMoney = (Literal)this.FindControl("txtRefundMoney");
			this.txtRefundType = (Literal)this.FindControl("txtRefundType");
			this.txtMemo = (Literal)this.FindControl("txtMemo");
			this.txtStatus = (Literal)this.FindControl("txtStatus");
			this.txtAdminRemark = (Literal)this.FindControl("txtAdminRemark");
			this.txtQuantity = (Literal)this.FindControl("txtQuantity");
			this.ExpressRow = (HtmlGenericControl)this.FindControl("ExpressRow");
			this.ExpressNumberRow = (HtmlGenericControl)this.FindControl("ExpressNumberRow");
			this.AdminRemarkRow = (HtmlGenericControl)this.FindControl("AdminRemarkRow");
			this.txtExpress = (Literal)this.FindControl("txtExpress");
			this.txtExpressNo = (Literal)this.FindControl("txtExpressNo");
			this.txtReturnReason = (Literal)this.FindControl("txtReturnReason");
			this.litStep = (Literal)this.FindControl("litStep");
			this.litStatus = (Literal)this.FindControl("litStatus");
			this.txtIsRefund = (HtmlInputHidden)this.FindControl("txtIsRefund");
			this.products = (Common_OrderItem_AfterSales)this.FindControl("Common_OrderItemAfterSales");
			this.txtAfterSaleId = (Literal)this.FindControl("txtAfterSaleId");
			this.litCredentialsImg = (Literal)this.FindControl("litCredentialsImg");
			this.litBankName = (Literal)this.FindControl("litBankName");
			this.litBankAccountName = (Literal)this.FindControl("litBankAccountName");
			this.litBankAccountNo = (Literal)this.FindControl("litBankAccountNo");
			this.bankRow1 = (HtmlGenericControl)this.FindControl("bankRow1");
			this.bankRow2 = (HtmlGenericControl)this.FindControl("bankRow2");
			this.bankRow3 = (HtmlGenericControl)this.FindControl("bankRow3");
			int num = HttpContext.Current.Request.QueryString["ReturnId"].ToInt(0);
			if (num <= 0)
			{
				HttpContext.Current.Request.QueryString["ReturnsId"].ToInt(0);
			}
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(num);
			if (returnInfo == null)
			{
				this.ShowError("错误的退货信息");
			}
			else
			{
				if (returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund)
				{
					this.litStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 0);
				}
				else
				{
					this.litStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 3);
				}
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo == null || orderInfo.UserId != HiContext.Current.UserId)
				{
					this.ShowMessage("退货订单不存在或者不属于当前用户的订单", false, "", 1);
				}
				else
				{
					if (!string.IsNullOrEmpty(returnInfo.UserCredentials))
					{
						this.hidOldImages.Value = returnInfo.UserCredentials;
					}
					Literal literal = this.txtAfterSaleId;
					int num2 = returnInfo.ReturnId;
					literal.Text = num2.ToString();
					Literal literal2 = this.txtQuantity;
					num2 = returnInfo.Quantity;
					literal2.Text = num2.ToString();
					HtmlInputHidden htmlInputHidden = this.txtReturnsId;
					num2 = returnInfo.ReturnId;
					htmlInputHidden.Value = num2.ToString();
					this.txtOrderId.Text = returnInfo.OrderId;
					this.txtRefundMoney.Text = returnInfo.RefundAmount.F2ToString("f2");
					this.txtRefundType.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.RefundType, 0);
					this.txtMemo.Text = returnInfo.UserRemark;
					this.txtReturnReason.Text = returnInfo.ReturnReason;
					if (returnInfo.RefundType == RefundTypes.InBankCard)
					{
						this.bankRow1.Visible = true;
						this.bankRow2.Visible = true;
						this.bankRow3.Visible = true;
						this.litBankName.Text = returnInfo.BankName;
						this.litBankAccountName.Text = returnInfo.BankAccountName;
						this.litBankAccountNo.Text = returnInfo.BankAccountNo;
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
						this.litCredentialsImg.Text = userCredentials;
					}
					if (returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund && (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Returned))
					{
						this.txtExpress.Text = returnInfo.ExpressCompanyName;
						this.txtExpressNo.Text = returnInfo.ShipOrderNumber;
					}
					else
					{
						this.ExpressRow.Visible = false;
						this.ExpressNumberRow.Visible = false;
						this.btnLogistic.Visible = false;
					}
					if (returnInfo.HandleStatus != ReturnStatus.MerchantsAgreed && returnInfo.HandleStatus != ReturnStatus.Deliverying)
					{
						this.btnSendGoods.Visible = false;
					}
					else if (returnInfo.HandleStatus == ReturnStatus.Deliverying)
					{
						this.btnSendGoods.Value = "修改发货信息";
					}
					if (!string.IsNullOrEmpty(returnInfo.AdminRemark))
					{
						this.txtAdminRemark.Text = returnInfo.AdminRemark;
						if (this.AdminRemarkRow != null)
						{
							this.AdminRemarkRow.Visible = true;
						}
					}
					else if (this.AdminRemarkRow != null)
					{
						this.AdminRemarkRow.Visible = false;
					}
					if (orderInfo != null)
					{
						this.products.order = orderInfo;
						if (string.IsNullOrEmpty(returnInfo.SkuId))
						{
							this.products.DataSource = orderInfo.LineItems.Values;
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
							this.products.DataSource = dictionary.Values;
						}
						this.products.DataBind();
					}
					DateTime dateTime = returnInfo.AgreedOrRefusedTime.HasValue ? returnInfo.AgreedOrRefusedTime.Value : returnInfo.ApplyForTime;
					DateTime dateTime2 = (!returnInfo.FinishTime.HasValue || returnInfo.FinishTime.Value == DateTime.MinValue) ? returnInfo.ApplyForTime : returnInfo.FinishTime.Value;
					DateTime dateTime3 = returnInfo.UserSendGoodsTime.HasValue ? returnInfo.UserSendGoodsTime.Value : dateTime;
					DateTime dateTime4 = returnInfo.ConfirmGoodsTime.HasValue ? returnInfo.ConfirmGoodsTime.Value : dateTime3;
					string text = "<ul>";
					returnInfo.AdminRemark = (string.IsNullOrEmpty(returnInfo.AdminRemark) ? "" : ("备注:" + returnInfo.AdminRemark));
					DateTime applyForTime;
					if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
					{
						this.txtIsRefund.Value = "1";
						if (returnInfo.HandleStatus == ReturnStatus.Applied)
						{
							string str2 = text;
							string text2 = this.stepTemplate.Replace("{style}", this.activityStyle);
							string format = this.timeTemplate;
							applyForTime = returnInfo.ApplyForTime;
							text = str2 + text2.Replace("{time}", string.Format(format, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退款中");
							text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
								.Replace("{StatusText}", "商家同意退款");
							text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
								.Replace("{StatusText}", "退款完成");
						}
						else if (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed)
						{
							string str3 = text;
							string text3 = this.stepTemplate.Replace("{style}", this.activityStyle);
							string format2 = this.timeTemplate;
							applyForTime = returnInfo.ApplyForTime;
							text = str3 + text3.Replace("{time}", string.Format(format2, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退款中");
							text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
								.Replace("{StatusText}", "商家同意申请");
							text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
								.Replace("{StatusText}", "退款完成");
						}
						else if (returnInfo.HandleStatus == ReturnStatus.Returned)
						{
							string str4 = text;
							string text4 = this.stepTemplate.Replace("{style}", this.activityStyle);
							string format3 = this.timeTemplate;
							applyForTime = returnInfo.ApplyForTime;
							text = str4 + text4.Replace("{time}", string.Format(format3, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退款中");
							text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
								.Replace("{StatusText}", "商家同意申请");
							text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
								.Replace("{StatusText}", "退款完成");
						}
						else if (returnInfo.HandleStatus == ReturnStatus.Refused)
						{
							string str5 = text;
							string text5 = this.stepTemplate.Replace("{style}", this.activityStyle);
							string format4 = this.timeTemplate;
							applyForTime = returnInfo.ApplyForTime;
							text = str5 + text5.Replace("{time}", string.Format(format4, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退款中");
							text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
								.Replace("{StatusText}", "商家拒绝申请");
							text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, ""))
								.Replace("{StatusText}", "退款失败");
						}
					}
					else if (returnInfo.HandleStatus == ReturnStatus.Applied)
					{
						string str6 = text;
						string text6 = this.stepTemplate.Replace("{style}", this.activityStyle);
						string format5 = this.timeTemplate;
						applyForTime = returnInfo.ApplyForTime;
						text = str6 + text6.Replace("{time}", string.Format(format5, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退货中");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "商家同意申请");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "买家退货");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "商家确认收货");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "退货完成");
					}
					else if (returnInfo.HandleStatus == ReturnStatus.MerchantsAgreed)
					{
						string str7 = text;
						string text7 = this.stepTemplate.Replace("{style}", this.activityStyle);
						string format6 = this.timeTemplate;
						applyForTime = returnInfo.ApplyForTime;
						text = str7 + text7.Replace("{time}", string.Format(format6, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退货中");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
							.Replace("{StatusText}", "商家同意申请");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "买家退货");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "商家确认收货");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "退货完成");
					}
					else if (returnInfo.HandleStatus == ReturnStatus.Deliverying)
					{
						string str8 = text;
						string text8 = this.stepTemplate.Replace("{style}", this.activityStyle);
						string format7 = this.timeTemplate;
						applyForTime = returnInfo.ApplyForTime;
						text = str8 + text8.Replace("{time}", string.Format(format7, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退货中");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
							.Replace("{StatusText}", "商家同意申请");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
							.Replace("{StatusText}", "买家退货");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "商家确认收货");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "退货完成");
					}
					else if (returnInfo.HandleStatus == ReturnStatus.GetGoods)
					{
						string str9 = text;
						string text9 = this.stepTemplate.Replace("{style}", this.activityStyle);
						string format8 = this.timeTemplate;
						applyForTime = returnInfo.ApplyForTime;
						text = str9 + text9.Replace("{time}", string.Format(format8, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退货中");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
							.Replace("{StatusText}", "商家同意申请");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
							.Replace("{StatusText}", "买家退货");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime4.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
							.Replace("{StatusText}", "商家确认收货");
						text += this.stepTemplate.Replace("{style}", "").Replace("{time}", "").Replace("{remark}", "")
							.Replace("{StatusText}", "退货完成");
					}
					else if (returnInfo.HandleStatus == ReturnStatus.Returned)
					{
						string str10 = text;
						string text10 = this.stepTemplate.Replace("{style}", this.activityStyle);
						string format9 = this.timeTemplate;
						applyForTime = returnInfo.ApplyForTime;
						text = str10 + text10.Replace("{time}", string.Format(format9, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退货中");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
							.Replace("{StatusText}", "商家同意申请");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
							.Replace("{StatusText}", "买家退货");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime4.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
							.Replace("{StatusText}", "商家确认收货");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
							.Replace("{StatusText}", "退货完成");
					}
					else if (returnInfo.HandleStatus == ReturnStatus.Refused)
					{
						string str11 = text;
						string text11 = this.stepTemplate.Replace("{style}", this.activityStyle);
						string format10 = this.timeTemplate;
						applyForTime = returnInfo.ApplyForTime;
						text = str11 + text11.Replace("{time}", string.Format(format10, applyForTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, "")).Replace("{StatusText}", "申请退货中");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", string.Format(this.remarkTemplate, returnInfo.AdminRemark))
							.Replace("{StatusText}", "商家拒绝退货");
						text += this.stepTemplate.Replace("{style}", this.activityStyle).Replace("{time}", string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd HH:mm:ss"))).Replace("{remark}", "")
							.Replace("{StatusText}", "退货失败");
					}
					text += "</ul>";
					this.litStep.Text = text;
					if (returnInfo != null && returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
					{
						PageTitle.AddSiteNameTitle("退款详情");
					}
					else
					{
						PageTitle.AddSiteNameTitle("退货详情");
					}
				}
			}
		}

		public void ShowError(string errorMsg)
		{
			this.hidErrorMsg.Value = errorMsg;
		}
	}
}
