using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Order
{
	[PrivilegeCheck(Privilege.SupplierOrderList)]
	public class ManageOrder : AdminPage
	{
		protected int Page_CurrentPageIndex = 1;

		protected int Page_CurrentPageSize = 10;

		public int UserStoreId = 0;

		public int OrderStatusID = 0;

		protected HtmlInputText txtOrderId;

		protected HtmlInputText txtProductName;

		protected CalendarPanel cldStartDate;

		protected CalendarPanel cldEndDate;

		protected HtmlInputText txtShopTo;

		protected SuplierDropDownList ddlSuppliers;

		protected DropDownList dropInvoiceType;

		protected HtmlInputHidden hidOrderId;

		protected CloseTranReasonDropDownList ddlCloseReason;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected TextBox txtRemark;

		protected HtmlInputHidden hidOrderTotal;

		protected HtmlInputHidden hidExpressCompanyName;

		protected HtmlInputHidden hidShipOrderNumber;

		protected Button btnRemark;

		protected Button btnOrderGoods;

		protected Button btnProductGoods;

		protected HiddenField hidGroupId;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.ddlSuppliers.DataBind();
			}
			this.LoadParameters();
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			this.btnRemark.Click += this.btnRemark_Click;
		}

		private void LoadParameters()
		{
			this.Page_CurrentPageIndex = this.Page.Request["pageindex"].ToInt(0);
			if (!string.IsNullOrWhiteSpace(this.Page.Request["page"]))
			{
				this.Page_CurrentPageIndex = this.Page.Request["page"].ToInt(0);
			}
			if (this.Page_CurrentPageIndex < 1)
			{
				this.Page_CurrentPageIndex = 1;
			}
			this.Page_CurrentPageSize = this.Page.Request["pagesize"].ToInt(0);
			if (!string.IsNullOrWhiteSpace(this.Page.Request["rows"]))
			{
				this.Page_CurrentPageSize = this.Page.Request["rows"].ToInt(0);
			}
			if (this.Page_CurrentPageSize < 1)
			{
				this.Page_CurrentPageSize = 10;
			}
			this.OrderStatusID = this.Page.Request.QueryString["OrderStatus"].ToInt(0);
			if (this.OrderStatusID < 0)
			{
				this.OrderStatusID = 0;
			}
			this.hidGroupId.Value = this.Page.Request["GroupBuyId"];
			this.Page_CurrentPageIndex = this.Page.Request["pageindex"].ToInt(0);
			if (this.Page_CurrentPageIndex < 1)
			{
				this.Page_CurrentPageIndex = 1;
			}
			this.txtOrderId.Value = this.Page.Request["OrderId"];
			this.cldStartDate.Text = this.Page.Request["StartDate"];
			this.cldEndDate.Text = this.Page.Request["EndDate"];
			this.txtProductName.Value = Globals.UrlDecode(this.Page.Request["ProductName"]);
			this.txtShopTo.Value = Globals.UrlDecode(this.Page.Request["ShipTo"]);
			string b = this.Page.Request["SupplierId"];
			foreach (ListItem item in this.ddlSuppliers.Items)
			{
				if (item.Value == b)
				{
					item.Selected = true;
				}
			}
			this.dropInvoiceType.SelectedValue = this.Page.Request["InvoiceType"].ToNullString();
		}

		private void dlstOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string a = "";
				if (!(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
				{
					a = (string)DataBinder.Eval(e.Item.DataItem, "Gateway");
				}
				string text = DataBinder.Eval(e.Item.DataItem, "OrderId").ToString();
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				OrderStatus orderStatus = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				int num = DataBinder.Eval(e.Item.DataItem, "ShippingModeId").ToInt(0);
				ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
				Literal literal = (Literal)e.Item.FindControl("litCloseOrder");
				HyperLink hyperLink = (HyperLink)e.Item.FindControl("lkbtnEditPrice");
				int num2 = (int)((DataBinder.Eval(e.Item.DataItem, "GroupBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "GroupBuyId"));
				int num3 = (int)((DataBinder.Eval(e.Item.DataItem, "CountDownBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "CountDownBuyId"));
				int num4 = (int)((DataBinder.Eval(e.Item.DataItem, "BundlingId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "BundlingId"));
				int num5 = DataBinder.Eval(e.Item.DataItem, "ShippingModeId").ToInt(0);
				ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
				Literal literal2 = (Literal)e.Item.FindControl("litDivideStore");
				Literal literal3 = (Literal)e.Item.FindControl("isGiftOrder");
				LinkButton linkButton = (LinkButton)e.Item.FindControl("lbtnFightGroup");
				Image image = (Image)e.Item.FindControl("imgError");
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				HtmlInputHidden htmlInputHidden = (HtmlInputHidden)e.Item.FindControl("hidFightGroup");
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("aftersaleImg");
				image.Visible = orderInfo.IsError;
				if (orderInfo.IsError)
				{
					image.Attributes.Add("title", orderInfo.ErrorMessage);
					image.ImageUrl = "\\Admin\\images\\orderError.png";
				}
				ProductPreSaleInfo productPreSaleInfo = null;
				if (orderInfo.PreSaleId > 0)
				{
					Literal literal4 = (Literal)e.Item.FindControl("litPreSale");
					Literal literal5 = (Literal)e.Item.FindControl("litSendGoods");
					FormatedMoneyLabel formatedMoneyLabel = (FormatedMoneyLabel)e.Item.FindControl("lblOrderTotals");
					e.Item.FindControl("lblAmount").Visible = true;
					productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
					literal4.Text = "<font>定金：" + orderInfo.Deposit.F2ToString("f2") + "</font>";
					Literal literal6 = literal4;
					literal6.Text = literal6.Text + "<font>尾款：" + orderInfo.FinalPayment.F2ToString("f2") + "</font>";
					literal4.Visible = true;
					formatedMoneyLabel.Money = orderInfo.Deposit + orderInfo.FinalPayment;
					formatedMoneyLabel.Text = (orderInfo.Deposit + orderInfo.FinalPayment).ToString();
					if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						literal5.Visible = true;
						DateTime dateTime;
						if (productPreSaleInfo.DeliveryDate.HasValue)
						{
							Literal literal7 = literal5;
							dateTime = productPreSaleInfo.DeliveryDate.Value;
							literal7.Text = "<span>预计发货时间：" + dateTime.ToString("yyyy-MM-dd") + "</span>";
						}
						else
						{
							DateTime payDate = orderInfo.PayDate;
							if (orderInfo.PayDate != DateTime.MinValue)
							{
								Literal literal8 = literal5;
								dateTime = orderInfo.PayDate;
								dateTime = dateTime.AddDays((double)productPreSaleInfo.DeliveryDays);
								literal8.Text = "<span>预计发货时间：" + dateTime.ToString("yyyy-MM-dd") + "</span>";
							}
						}
					}
				}
				RefundInfo refundInfo = TradeHelper.GetRefundInfo(orderInfo.OrderId);
				if (orderInfo.ItemStatus != 0 || orderInfo.OrderStatus == OrderStatus.ApplyForRefund)
				{
					if (orderInfo.OrderStatus == OrderStatus.ApplyForRefund)
					{
						if (refundInfo != null)
						{
							htmlAnchor2.Visible = true;
							htmlAnchor2.Title = "订单已申请退款";
							htmlAnchor2.HRef = "/Admin/sales/RefundApplyDetail?RefundId=" + refundInfo.RefundId;
						}
					}
					else
					{
						int num6 = 0;
						AfterSaleTypes afterSaleTypes = AfterSaleTypes.ReturnAndRefund;
						int num7 = 0;
						foreach (LineItemInfo value in orderInfo.LineItems.Values)
						{
							if (value.ReturnInfo != null || value.ReplaceInfo != null)
							{
								ReturnInfo returnInfo = value.ReturnInfo;
								ReplaceInfo replaceInfo = value.ReplaceInfo;
								if (num6 == 0 || (returnInfo != null && returnInfo.HandleStatus != ReturnStatus.Refused && returnInfo.HandleStatus != ReturnStatus.Returned) || (replaceInfo != null && (replaceInfo.HandleStatus != ReplaceStatus.Refused || replaceInfo.HandleStatus != ReplaceStatus.Replaced)))
								{
									if (value.ReturnInfo != null)
									{
										afterSaleTypes = AfterSaleTypes.ReturnAndRefund;
										num7 = value.ReturnInfo.ReturnId;
									}
									else
									{
										afterSaleTypes = AfterSaleTypes.Replace;
										num7 = value.ReplaceInfo.ReplaceId;
									}
								}
								num6++;
							}
						}
						if (orderInfo.ItemStatus == OrderItemStatus.HasReturnOrReplace)
						{
							htmlAnchor2.Title = "订单中有商品正在退货/换货中";
						}
						else if (orderInfo.ReturnedCount > 0)
						{
							htmlAnchor2.Title = "订单中有商品已退货完成";
						}
						else if (orderInfo.ItemStatus == OrderItemStatus.HasReplace)
						{
							htmlAnchor2.Title = "订单中有商品正在进行换货操作";
						}
						else if (orderInfo.ItemStatus == OrderItemStatus.HasReturn)
						{
							htmlAnchor2.Title = "订单中有商品正在进行退货操作";
						}
						if (num6 > 0)
						{
							htmlAnchor2.Visible = true;
							if (afterSaleTypes == AfterSaleTypes.ReturnAndRefund)
							{
								htmlAnchor2.HRef = "ReturnApplyDetail?ReturnId=" + num7;
							}
							else
							{
								htmlAnchor2.HRef = "ReplaceApplyDetail?ReplaceId=" + num7;
							}
						}
					}
				}
				if (orderInfo.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
					if (fightGroup != null)
					{
						linkButton.PostBackUrl = "/Admin/promotion/FightGroupDetails.aspx?fightGroupActivityId=" + fightGroup.FightGroupActivityId;
						if (fightGroup.Status == FightGroupStatus.FightGroupIn && orderInfo.OrderStatus != OrderStatus.WaitBuyerPay && orderInfo.OrderStatus != OrderStatus.Closed)
						{
							htmlInputHidden.Value = "1";
						}
						else
						{
							htmlInputHidden.Value = "0";
						}
					}
				}
				else
				{
					linkButton.Visible = false;
				}
				OrderStatusLabel orderStatusLabel = (OrderStatusLabel)e.Item.FindControl("lblOrderStatus");
				if (orderStatusLabel != null)
				{
					orderStatusLabel.OrderItemStatus = orderInfo.ItemStatus;
					if (orderInfo.PreSaleId > 0)
					{
						orderStatusLabel.PreSaleId = orderInfo.PreSaleId;
						orderStatusLabel.DepositDate = orderInfo.DepositDate;
					}
				}
				HtmlAnchor htmlAnchor3 = (HtmlAnchor)e.Item.FindControl("lkbtnToDetail");
				int num8 = (int)((DataBinder.Eval(e.Item.DataItem, "StoreId") == DBNull.Value) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "StoreId"));
				bool flag = (bool)((DataBinder.Eval(e.Item.DataItem, "IsStoreCollect") == DBNull.Value) ? ((object)false) : DataBinder.Eval(e.Item.DataItem, "IsStoreCollect"));
				bool flag2 = false;
				if (refundInfo != null)
				{
					htmlAnchor.HRef = "/Admin/sales/RefundApplyDetail?RefundId=" + refundInfo.RefundId.ToString();
					flag2 = true;
				}
				if (orderInfo.LineItems.Count <= 0)
				{
					literal3.Text = "(礼)";
				}
				Literal literal9 = (Literal)e.Item.FindControl("group");
				if (literal9 != null)
				{
					if (num2 > 0)
					{
						literal9.Text = "(团)";
					}
					if (num3 > 0)
					{
						literal9.Text = "(抢)";
					}
					if (orderInfo.PreSaleId > 0)
					{
						literal9.Text = "(预)";
					}
				}
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					if (orderInfo.PreSaleId > 0)
					{
						if (!orderInfo.DepositDate.HasValue)
						{
							hyperLink.NavigateUrl = "javascript:DialogFrame('/Admin/sales/EditOrder.aspx?OrderId=" + text + "','修改订单价格',null,null,function(e){location.reload();})";
							hyperLink.Visible = true;
							literal.Visible = true;
							if (a != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0)
							{
								imageLinkButton.Visible = true;
							}
						}
						else if (productPreSaleInfo.PaymentStartDate <= DateTime.Now && productPreSaleInfo.PaymentEndDate >= DateTime.Now && a != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0)
						{
							imageLinkButton.Visible = true;
						}
					}
					else
					{
						hyperLink.NavigateUrl = "javascript:DialogFrame('/Admin/sales/EditOrder.aspx?OrderId=" + text + "','修改订单价格',null,null,function(e){location.reload();})";
						hyperLink.Visible = true;
						if (a != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0 && (orderInfo.ParentOrderId == "0" || orderInfo.ParentOrderId == "-1"))
						{
							imageLinkButton.Visible = true;
						}
					}
				}
				if (a == "hishop.plugins.payment.podrequest")
				{
					int num9;
					switch (orderStatus)
					{
					case OrderStatus.SellerAlreadySent:
						num9 = ((orderInfo.ParentOrderId == "0" || orderInfo.ParentOrderId == "-1") ? 1 : 0);
						break;
					default:
						num9 = 0;
						break;
					case OrderStatus.WaitBuyerPay:
						num9 = 1;
						break;
					}
					if (num9 != 0)
					{
						literal.Visible = true;
					}
				}
				if (orderStatus == OrderStatus.SellerAlreadySent && orderInfo.StoreId > 0 && masterSettings.OpenMultStore)
				{
					literal.Visible = false;
				}
				if (orderStatus == OrderStatus.ApplyForRefund && !orderInfo.IsStoreCollect)
				{
					htmlAnchor.Visible = true;
				}
				imageLinkButton2.Visible = (orderStatus == OrderStatus.SellerAlreadySent && orderInfo.ItemStatus == OrderItemStatus.Nomarl);
				if (masterSettings.OpenMultStore && num == -2 && orderStatus == OrderStatus.WaitBuyerPay)
				{
					literal.Visible = !orderInfo.IsConfirm;
					hyperLink.Visible = true;
					if (a != "hishop.plugins.payment.podrequest" && orderInfo.PaymentTypeId != -3 && orderInfo.FightGroupId == 0)
					{
						imageLinkButton.Visible = true;
					}
				}
			}
		}

		private void btnRemark_Click(object sender, EventArgs e)
		{
			if (this.txtRemark.Text.Length > 300)
			{
				this.ShowMsg("备忘录长度限制在300个字符以内", false);
			}
			else
			{
				Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9._一-龥-]+$");
				if (!regex.IsMatch(this.txtRemark.Text))
				{
					this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
				}
				else
				{
					OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
					if (this.orderRemarkImageForRemark.SelectedItem != null)
					{
						orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
					}
					orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
					if (OrderHelper.SaveRemark(orderInfo))
					{
						this.ShowMsg("保存备忘录成功", true);
					}
					else
					{
						this.ShowMsg("保存失败", false);
					}
				}
			}
		}
	}
}
