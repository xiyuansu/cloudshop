using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
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
	public class UserReplaceApplyDetails : MemberTemplatedWebControl
	{
		private int replaceId;

		private string orderId;

		private Literal txtOrderId;

		private Common_OrderItems_AfterSales products;

		private Literal litRemark;

		private Literal litWeight;

		private Literal litAdminRemark;

		private Literal litShipToDate;

		private Literal litUserRemark;

		private Literal litCredentialsImg;

		private Literal txtAfterSaleId;

		private Literal litReplaceAmount;

		private string credentialsImgHtml = "<img src=\"{0}\" style=\"max-height:60px;\" />";

		private Literal litStep;

		private Literal litProcess;

		private Literal litTime;

		private string stepTemplate = "<span>买家申请换货</span><span>商家同意申请</span><span>买家发货</span><span>商家发货</span><span>换货成功</span>";

		private string processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";

		private string timeTemplate = "<span>{0}<br>{1}</span>";

		private HtmlInputHidden txtIsRefused;

		private ReplaceInfo replace = null;

		private OrderInfo order = null;

		private FormatedMoneyLabel litTotalPrice;

		private HiddenField hidExpressCompanyName;

		private HiddenField hidShipOrderNumber;

		private HtmlGenericControl divCredentials;

		private ExpressDropDownList expresslist1;

		private TextBox txtShipOrderNumber1;

		private HtmlAnchor lnkToSendGoods;

		private HtmlAnchor lnkFinishReplace;

		private HiddenField hidReplaceId;

		private IButton btnSendGoodsReplace;

		private IButton btnFinishReplace;

		private HtmlAnchor lnkReApply;

		private HtmlGenericControl AdminRemarkRow;

		private HtmlAnchor btnViewUserLogistic;

		private HtmlAnchor btnViewMallLogistic;

		private string returnUrl = "";

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReplaceApplyDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.returnUrl = base.GetParameter("ReturnUrl", false).ToNullString();
			if (string.IsNullOrEmpty(this.returnUrl))
			{
				this.returnUrl = this.Page.Request.UrlReferrer.ToNullString();
				if (this.returnUrl == this.Page.Request.Url.ToString())
				{
					this.returnUrl = "/User/UserOrders";
				}
			}
			this.divCredentials = (HtmlGenericControl)this.FindControl("divCredentials");
			this.replaceId = base.GetParameter("ReplaceId", false).ToInt(0);
			this.products = (Common_OrderItems_AfterSales)this.FindControl("Common_OrderItems_AfterSales");
			this.txtOrderId = (Literal)this.FindControl("txtOrderId");
			this.txtAfterSaleId = (Literal)this.FindControl("txtAfterSaleId");
			this.litRemark = (Literal)this.FindControl("litRemark");
			this.litAdminRemark = (Literal)this.FindControl("litAdminRemark");
			this.litWeight = (Literal)this.FindControl("litWeight");
			this.litReplaceAmount = (Literal)this.FindControl("litReplaceAmount");
			this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
			this.lnkToSendGoods = (HtmlAnchor)this.FindControl("lnkToSendGoods");
			this.lnkFinishReplace = (HtmlAnchor)this.FindControl("lnkFinishReplace");
			this.litShipToDate = (Literal)this.FindControl("litShipToDate");
			this.litWeight = (Literal)this.FindControl("litWeight");
			this.litUserRemark = (Literal)this.FindControl("litUserRemark");
			this.hidExpressCompanyName = (HiddenField)this.FindControl("hidExpressCompanyName");
			this.hidShipOrderNumber = (HiddenField)this.FindControl("hidShipOrderNumber");
			this.expresslist1 = (ExpressDropDownList)this.FindControl("expressDropDownList1");
			this.txtShipOrderNumber1 = (TextBox)this.FindControl("txtShipOrderNumber1");
			this.litCredentialsImg = (Literal)this.FindControl("litCredentialsImg");
			this.litStep = (Literal)this.FindControl("litStep");
			this.litTime = (Literal)this.FindControl("litTime");
			this.litProcess = (Literal)this.FindControl("litProcess");
			this.txtIsRefused = (HtmlInputHidden)this.FindControl("txtIsRefused");
			this.lnkReApply = (HtmlAnchor)this.FindControl("lnkReApply");
			this.AdminRemarkRow = (HtmlGenericControl)this.FindControl("AdminRemarkRow");
			this.btnFinishReplace = ButtonManager.Create(this.FindControl("btnFinishReplace"));
			this.btnSendGoodsReplace = ButtonManager.Create(this.FindControl("btnSendGoodsReplace"));
			this.btnViewUserLogistic = (HtmlAnchor)this.FindControl("btnViewUserLogistic");
			this.btnViewMallLogistic = (HtmlAnchor)this.FindControl("btnViewMallLogistic");
			this.btnSendGoodsReplace.Click += this.btnSendGoodsReplace_Click;
			this.replace = TradeHelper.GetReplaceInfo(this.replaceId);
			if (this.replace == null)
			{
				this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("换货信息不存在或者不属于当前用户"));
			}
			else
			{
				this.order = TradeHelper.GetOrderInfo(this.replace.OrderId);
				if (this.order == null || this.order.UserId != HiContext.Current.UserId)
				{
					this.Page.Response.Redirect("/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
				}
				else if (!this.Page.IsPostBack)
				{
					string a = this.Page.Request.QueryString["action"].ToNullString().ToLower();
					if (a == "finishreplace")
					{
						this.FinishReplace();
					}
					this.BindOrderReplace(this.replaceId);
					if (this.expresslist1 != null)
					{
						this.expresslist1.ShowAllExpress = true;
						this.expresslist1.DataBind();
					}
					this.BindProducts(this.order, this.replace.SkuId);
					this.BindOrderItems(this.order);
				}
			}
		}

		private void btnSendGoodsReplace_Click(object sender, EventArgs e)
		{
			this.replaceId = Convert.ToInt32(base.GetParameter("ReplaceId", false));
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(this.replaceId);
			if (replaceInfo == null)
			{
				this.ShowMessage("错误的换货信息", false, "", 1);
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(replaceInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMessage("错误的订单信息", false, "", 1);
				}
				else if (orderInfo.LineItems.ContainsKey(replaceInfo.SkuId))
				{
					if (orderInfo.LineItems[replaceInfo.SkuId].Status != LineItemStatus.MerchantsAgreedForReplace && orderInfo.LineItems[replaceInfo.SkuId].Status != LineItemStatus.UserDeliveryForReplace)
					{
						this.ShowMessage("商品换货状态不正确", false, "", 1);
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
							string text = "";
							string text2 = "";
							ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(value);
							if (value != null)
							{
								text = expressCompanyInfo.Kuaidi100Code;
								text2 = expressCompanyInfo.Name;
								if (value2.Trim() == "" || value2.Length > 20)
								{
									this.ShowMessage("请输入快递编号，长度为1-20位！", false, "", 1);
								}
								else if (TradeHelper.ReplaceUserSendGoods(replaceInfo.ReplaceId, text, text2, value2, orderInfo.OrderId, replaceInfo.SkuId))
								{
									if (text.ToUpper() == "HTKY")
									{
										ExpressHelper.GetDataByKuaidi100(text, value2);
									}
									this.ShowMessage("发货成功", true, this.returnUrl, 2);
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

		private void FinishReplace()
		{
			int num = 0;
			if (HttpContext.Current.Request.QueryString["replaceId"] != null)
			{
				int.TryParse(HttpContext.Current.Request.QueryString["replaceId"], out num);
			}
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(num);
			if (replaceInfo == null)
			{
				this.ShowMessage("错误的换货信息", false, "", 1);
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(replaceInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMessage("错误的订单信息", false, "", 1);
				}
				else if (orderInfo.LineItems.ContainsKey(replaceInfo.SkuId))
				{
					if (orderInfo.LineItems[replaceInfo.SkuId].Status != LineItemStatus.MerchantsDeliveryForRepalce)
					{
						this.ShowMessage("商品换货状态不正确", false, "", 1);
					}
					else if (TradeHelper.FinishReplace(replaceInfo.ReplaceId, replaceInfo.AdminRemark, replaceInfo.OrderId, replaceInfo.SkuId))
					{
						this.ShowMessage("确认收货完成换货成功", true, this.returnUrl, 2);
					}
					else
					{
						this.ShowMessage("确认收货完成换货失败", false, "", 1);
					}
				}
				else
				{
					this.ShowMessage("订单中不包含商品信息", false, "", 1);
				}
			}
		}

		private void BindProducts(OrderInfo order, string SkuId)
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
			this.txtOrderId.Text = order.OrderId;
		}

		private void BindOrderReplace(int replaceId)
		{
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo != null)
			{
				if (replaceInfo.HandleStatus == ReplaceStatus.Refused && (this.order.OrderStatus == OrderStatus.SellerAlreadySent || (this.order.OrderStatus == OrderStatus.Finished && !this.order.IsServiceOver)))
				{
					this.lnkReApply.Visible = true;
					this.lnkReApply.HRef = "/User/AfterSalesApply?OrderId=" + replaceInfo.OrderId + "&SkuId=" + replaceInfo.SkuId;
				}
				int quantity;
				if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsAgreed || replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
				{
					AttributeCollection attributes = this.lnkToSendGoods.Attributes;
					quantity = replaceInfo.ReplaceId;
					attributes.Add("ReplaceId", quantity.ToString());
					this.lnkToSendGoods.Visible = true;
				}
				if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery)
				{
					this.lnkFinishReplace.Visible = true;
					this.lnkFinishReplace.HRef = "UserReplaceApplyDetails?ReplaceId=" + replaceInfo.ReplaceId + "&action=FinishReplace";
				}
				Literal literal = this.litReplaceAmount;
				quantity = replaceInfo.Quantity;
				literal.Text = quantity.ToString();
				this.txtAfterSaleId.Text = replaceId.ToString();
				this.litRemark.Text = replaceInfo.ReplaceReason;
				if (!string.IsNullOrEmpty(replaceInfo.AdminRemark))
				{
					this.litAdminRemark.Text = replaceInfo.AdminRemark;
					if (this.AdminRemarkRow != null)
					{
						this.AdminRemarkRow.Visible = true;
					}
				}
				else if (this.AdminRemarkRow != null)
				{
					this.AdminRemarkRow.Visible = false;
				}
				this.litUserRemark.Text = replaceInfo.UserRemark;
				this.orderId = replaceInfo.OrderId;
				string userCredentials = replaceInfo.UserCredentials;
				if (string.IsNullOrEmpty(userCredentials))
				{
					this.divCredentials.Visible = false;
				}
				else
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
				string text = "";
				DateTime dateTime = replaceInfo.AgreedOrRefusedTime.HasValue ? replaceInfo.AgreedOrRefusedTime.Value : replaceInfo.ApplyForTime;
				DateTime dateTime2 = replaceInfo.UserSendGoodsTime.HasValue ? replaceInfo.UserSendGoodsTime.Value : dateTime;
				DateTime dateTime3 = replaceInfo.MerchantsConfirmGoodsTime.HasValue ? replaceInfo.MerchantsConfirmGoodsTime.Value : dateTime2;
				DateTime dateTime4 = replaceInfo.UserConfirmGoodsTime.HasValue ? replaceInfo.UserConfirmGoodsTime.Value : dateTime3;
				DateTime applyForTime;
				if (replaceInfo.HandleStatus == ReplaceStatus.Applied)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
					string format = this.timeTemplate;
					applyForTime = replaceInfo.ApplyForTime;
					string arg = applyForTime.ToString("yyyy-MM-dd");
					applyForTime = replaceInfo.ApplyForTime;
					text = string.Format(format, arg, applyForTime.ToString("HH:mm:ss"));
				}
				else if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsAgreed)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
					string format2 = this.timeTemplate;
					applyForTime = replaceInfo.ApplyForTime;
					string arg2 = applyForTime.ToString("yyyy-MM-dd");
					applyForTime = replaceInfo.ApplyForTime;
					text = string.Format(format2, arg2, applyForTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				}
				else if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
					string format3 = this.timeTemplate;
					applyForTime = replaceInfo.ApplyForTime;
					string arg3 = applyForTime.ToString("yyyy-MM-dd");
					applyForTime = replaceInfo.ApplyForTime;
					text = string.Format(format3, arg3, applyForTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd"), dateTime2.ToString("HH:mm:ss"));
				}
				else if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div><div class=\"proce\"></div><div class=\"node\"></div>";
					string format4 = this.timeTemplate;
					applyForTime = replaceInfo.ApplyForTime;
					string arg4 = applyForTime.ToString("yyyy-MM-dd");
					applyForTime = replaceInfo.ApplyForTime;
					text = string.Format(format4, arg4, applyForTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd"), dateTime2.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd"), dateTime3.ToString("HH:mm:ss"));
				}
				else if (replaceInfo.HandleStatus == ReplaceStatus.Replaced)
				{
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
					string format5 = this.timeTemplate;
					applyForTime = replaceInfo.ApplyForTime;
					string arg5 = applyForTime.ToString("yyyy-MM-dd");
					applyForTime = replaceInfo.ApplyForTime;
					text = string.Format(format5, arg5, applyForTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime2.ToString("yyyy-MM-dd"), dateTime4.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime3.ToString("yyyy-MM-dd"), dateTime3.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime4.ToString("yyyy-MM-dd"), dateTime4.ToString("HH:mm:ss"));
				}
				else if (replaceInfo.HandleStatus == ReplaceStatus.Refused)
				{
					this.txtIsRefused.Value = "1";
					this.stepTemplate = "<span>买家申请换货</span><span>商家拒绝申请</span><span>换货失败</span>";
					this.processTemplate = "<div class=\"node ready\"></div><div class=\"proce done\"></div><div class=\"node done\"></div><div class=\"proce done\"></div><div class=\"node now\"></div>";
					string format6 = this.timeTemplate;
					applyForTime = replaceInfo.ApplyForTime;
					string arg6 = applyForTime.ToString("yyyy-MM-dd");
					applyForTime = replaceInfo.ApplyForTime;
					text = string.Format(format6, arg6, applyForTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
					text += string.Format(this.timeTemplate, dateTime.ToString("yyyy-MM-dd"), dateTime.ToString("HH:mm:ss"));
				}
				this.litProcess.Text = this.processTemplate;
				this.litTime.Text = text;
				this.litStep.Text = this.stepTemplate;
				if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
				{
					this.btnViewUserLogistic.Visible = true;
					AttributeCollection attributes2 = this.btnViewUserLogistic.Attributes;
					quantity = replaceInfo.ReplaceId;
					attributes2.Add("replaceid", quantity.ToString());
				}
				if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery || replaceInfo.HandleStatus == ReplaceStatus.Replaced)
				{
					this.btnViewMallLogistic.Visible = true;
					AttributeCollection attributes3 = this.btnViewMallLogistic.Attributes;
					quantity = replaceInfo.ReplaceId;
					attributes3.Add("replaceid", quantity.ToString());
				}
			}
		}

		private ReplaceApplyQuery GetReplaceQuery()
		{
			ReplaceApplyQuery replaceApplyQuery = new ReplaceApplyQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				replaceApplyQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			replaceApplyQuery.SortBy = "ApplyForTime";
			return replaceApplyQuery;
		}
	}
}
