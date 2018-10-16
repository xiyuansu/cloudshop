using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.sales
{
	public class ManageOrder : StoreAdminPage
	{
		protected int Page_CurrentPageIndex = 1;

		protected int Page_CurrentPageSize = 10;

		public int UserStoreId = 0;

		public int OrderStatusID = 0;

		protected int? isTickit = null;

		protected HtmlInputText txtOrderId;

		protected HtmlInputText txtShopTo;

		protected HtmlInputText orderStartDate;

		protected HtmlInputText orderEndDate;

		protected OrderTypeDrowpDownList ddlOrderType;

		protected HtmlInputText txtCode;

		protected DropDownList dropInvoiceType;

		protected CloseTranReasonDropDownList ddlCloseReason;

		protected FormatedMoneyLabel lblOrderTotalForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected TextBox txtRemark;

		protected HtmlInputHidden hidOrderTotal;

		protected HtmlInputHidden hidExpressCompanyName;

		protected HtmlInputHidden hidShipOrderNumber;

		protected Button btnOrderGoods;

		protected Button btnProductGoods;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.UserStoreId = HiContext.Current.Manager.StoreId;
			this.ddlOrderType.DataBind();
			this.LoadParameters();
			this.btnOrderGoods.Click += this.btnOrderGoods_Click;
			this.btnProductGoods.Click += this.btnProductGoods_Click;
			this.OrderStatusID = this.Page.Request.QueryString["OrderStatus"].ToInt(0);
			if (this.OrderStatusID < 0)
			{
				this.OrderStatusID = 0;
			}
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
			this.txtOrderId.Value = this.Page.Request["OrderId"];
			this.orderStartDate.Value = this.Page.Request["StartDate"];
			this.orderEndDate.Value = this.Page.Request["EndDate"];
			this.txtCode.Value = this.Page.Request["takecode"];
			this.txtShopTo.Value = Globals.UrlDecode(this.Page.Request["ShipTo"]);
			if (!string.IsNullOrWhiteSpace(this.Page.Request["isTickit"]))
			{
				this.isTickit = this.Page.Request["isTickit"].ToInt(0);
			}
		}

		private void dlstOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
			{
				return;
			}
			string orderId = DataBinder.Eval(e.Item.DataItem, "OrderId").ToString();
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
			OrderStatus orderStatus = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "OrderStatus");
			HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("lkbtnCheckTake");
			Literal literal = (Literal)e.Item.FindControl("litCloseOrder");
			int num = DataBinder.Eval(e.Item.DataItem, "ShippingModeId").ToInt(0);
			string text = "";
			if (!(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
			{
				text = (string)DataBinder.Eval(e.Item.DataItem, "Gateway");
			}
			int num2 = (int)((DataBinder.Eval(e.Item.DataItem, "GroupBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "GroupBuyId"));
			int num3 = (int)((DataBinder.Eval(e.Item.DataItem, "CountDownBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "CountDownBuyId"));
			Label label = (Label)e.Item.FindControl("lkbtnSendGoods");
			ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
			ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
			HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
			ImageLinkButton imageLinkButton3 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirm");
			int num4 = (int)((DataBinder.Eval(e.Item.DataItem, "StoreId") == DBNull.Value) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "StoreId"));
			bool flag = (bool)((DataBinder.Eval(e.Item.DataItem, "IsStoreCollect") == DBNull.Value) ? ((object)false) : DataBinder.Eval(e.Item.DataItem, "IsStoreCollect"));
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
			bool flag2 = false;
			RefundInfo refundInfo = TradeHelper.GetRefundInfo(orderId);
			if (refundInfo != null && orderInfo.StoreId == this.UserStoreId)
			{
				htmlAnchor2.Attributes.Add("refundid", refundInfo.RefundId.ToString());
				flag2 = true;
			}
			Literal literal2 = (Literal)e.Item.FindControl("group");
			if (literal2 != null)
			{
				if (num2 > 0)
				{
					literal2.Text = "(团)";
				}
				if (num3 > 0)
				{
					literal2.Text = "(抢)";
				}
				if (orderInfo.PreSaleId > 0)
				{
					literal2.Text = "(预)";
				}
			}
			if (orderStatus == OrderStatus.WaitBuyerPay)
			{
				if (orderInfo.PreSaleId > 0)
				{
					if (!orderInfo.DepositDate.HasValue)
					{
						literal.Visible = true;
						if (text != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0)
						{
							imageLinkButton.Visible = true;
						}
					}
					else
					{
						ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
						if (productPreSaleInfo.PaymentStartDate <= DateTime.Now && productPreSaleInfo.PaymentEndDate >= DateTime.Now && text != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0)
						{
							imageLinkButton.Visible = true;
						}
					}
				}
				else
				{
					literal.Visible = true;
					if (text != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0)
					{
						imageLinkButton.Visible = true;
					}
				}
			}
			if (text == "hishop.plugins.payment.podrequest" && (orderStatus == OrderStatus.WaitBuyerPay || orderStatus == OrderStatus.SellerAlreadySent))
			{
				literal.Visible = true;
			}
			if ((orderStatus == OrderStatus.ApplyForRefund && this.UserStoreId == num4) & flag)
			{
				htmlAnchor2.Visible = true;
			}
			if (num2 > 0)
			{
				string[] source = new string[1]
				{
					"hishop.plugins.payment.podrequest"
				};
				GroupBuyStatus groupBuyStatus = (GroupBuyStatus)DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
				Label label2 = label;
				int visible;
				switch (orderStatus)
				{
				case OrderStatus.WaitBuyerPay:
					if (source.Contains(text))
					{
						goto case OrderStatus.BuyerAlreadyPaid;
					}
					goto default;
				case OrderStatus.BuyerAlreadyPaid:
					if (groupBuyStatus == GroupBuyStatus.Success)
					{
						visible = ((orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
						break;
					}
					goto default;
				default:
					visible = 0;
					break;
				}
				label2.Visible = ((byte)visible != 0);
			}
			else
			{
				Label label3 = label;
				int visible2;
				switch (orderStatus)
				{
				case OrderStatus.WaitBuyerPay:
					if (text == "hishop.plugins.payment.podrequest")
					{
						goto case OrderStatus.BuyerAlreadyPaid;
					}
					goto default;
				case OrderStatus.BuyerAlreadyPaid:
					visible2 = ((orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
					break;
				default:
					visible2 = 0;
					break;
				}
				label3.Visible = ((byte)visible2 != 0);
			}
			imageLinkButton2.Visible = (orderStatus == OrderStatus.SellerAlreadySent && orderInfo.ItemStatus == OrderItemStatus.Nomarl);
			HtmlAnchor htmlAnchor3;
			int visible3;
			if (num == -2)
			{
				htmlAnchor3 = htmlAnchor;
				if ((orderStatus == OrderStatus.BuyerAlreadyPaid || orderStatus == OrderStatus.WaitBuyerPay) && orderInfo.ItemStatus == OrderItemStatus.Nomarl)
				{
					visible3 = (orderInfo.IsConfirm ? 1 : 0);
					goto IL_05d6;
				}
				visible3 = 0;
				goto IL_05d6;
			}
			goto IL_0644;
			IL_0631:
			int visible4 = (orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0;
			goto IL_063d;
			IL_0644:
			LinkButton linkButton = (LinkButton)e.Item.FindControl("lbtnFightGroup");
			Image image = (Image)e.Item.FindControl("imgError");
			HtmlInputHidden htmlInputHidden = (HtmlInputHidden)e.Item.FindControl("hidFightGroup");
			image.Visible = orderInfo.IsError;
			if (orderInfo.IsError)
			{
				image.Attributes.Add("title", orderInfo.ErrorMessage);
				image.ImageUrl = "\\Admin\\images\\orderError.png";
			}
			if (orderInfo.FightGroupId > 0)
			{
				FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
				if (fightGroup != null)
				{
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
			return;
			IL_05d6:
			htmlAnchor3.Visible = ((byte)visible3 != 0);
			literal.Visible = (orderStatus == OrderStatus.WaitBuyerPay && orderInfo.ItemStatus == OrderItemStatus.Nomarl);
			label.Visible = false;
			ImageLinkButton imageLinkButton4 = imageLinkButton3;
			if (!orderInfo.IsConfirm)
			{
				if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && (text == "hishop.plugins.payment.podrequest" || orderInfo.PaymentTypeId == -3))
				{
					goto IL_0631;
				}
				if (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					goto IL_0631;
				}
			}
			visible4 = 0;
			goto IL_063d;
			IL_063d:
			imageLinkButton4.Visible = ((byte)visible4 != 0);
			goto IL_0644;
		}

		private void btnProductGoods_Click(object sender, EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的订单", false);
			}
			else
			{
				List<string> list = new List<string>();
				string[] array = text.Split(',');
				foreach (string str in array)
				{
					list.Add("'" + str + "'");
				}
				DataSet storeProductGoods = OrderHelper.GetStoreProductGoods(string.Join(",", list.ToArray()), this.UserStoreId);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				if (storeProductGoods.Tables.Count < 2 || storeProductGoods.Tables[1] == null || storeProductGoods.Tables[1].Rows.Count <= 0)
				{
					stringBuilder.AppendLine("<td>商品名称</td>");
				}
				else
				{
					stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
				}
				stringBuilder.AppendLine("<td>货号</td>");
				stringBuilder.AppendLine("<td>规格</td>");
				stringBuilder.AppendLine("<td>拣货数量</td>");
				stringBuilder.AppendLine("<td>现库存数</td>");
				stringBuilder.AppendLine("</tr>");
				foreach (DataRow row in storeProductGoods.Tables[0].Rows)
				{
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td>" + row["ProductName"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["SKUContent"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["Stock"] + "</td>");
					stringBuilder.AppendLine("</tr>");
				}
				if (storeProductGoods.Tables.Count >= 2 && storeProductGoods.Tables[1] != null && storeProductGoods.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow row2 in storeProductGoods.Tables[1].Rows)
					{
						stringBuilder.AppendLine("<tr>");
						stringBuilder.AppendLine("<td>" + row2["GiftName"] + "[礼品]</td>");
						stringBuilder.AppendLine("<td></td>");
						stringBuilder.AppendLine("<td></td>");
						stringBuilder.AppendLine("<td>" + row2["Quantity"] + "</td>");
						stringBuilder.AppendLine("<td></td>");
						stringBuilder.AppendLine("</tr>");
					}
				}
				stringBuilder.AppendLine("</table>");
				stringBuilder.AppendLine("</body></html>");
				base.Response.Clear();
				base.Response.Buffer = false;
				base.Response.Charset = "GB2312";
				base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
				base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
				base.Response.ContentType = "application/ms-excel";
				this.EnableViewState = false;
				base.Response.Write(stringBuilder.ToString());
				base.Response.End();
			}
		}

		private void btnOrderGoods_Click(object sender, EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的订单", false);
			}
			else
			{
				List<string> list = new List<string>();
				string[] array = text.Split(',');
				foreach (string str in array)
				{
					list.Add("'" + str + "'");
				}
				DataSet storeOrderGoods = OrderHelper.GetStoreOrderGoods(string.Join(",", list.ToArray()), this.UserStoreId);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				stringBuilder.AppendLine("<td>订单单号</td>");
				if (storeOrderGoods.Tables.Count < 2 || storeOrderGoods.Tables[1] == null || storeOrderGoods.Tables[1].Rows.Count <= 0)
				{
					stringBuilder.AppendLine("<td>商品名称</td>");
				}
				else
				{
					stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
				}
				stringBuilder.AppendLine("<td>货号</td>");
				stringBuilder.AppendLine("<td>规格</td>");
				stringBuilder.AppendLine("<td>拣货数量</td>");
				stringBuilder.AppendLine("<td>现库存数</td>");
				stringBuilder.AppendLine("<td>备注</td>");
				stringBuilder.AppendLine("</tr>");
				foreach (DataRow row in storeOrderGoods.Tables[0].Rows)
				{
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["ProductName"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["SKUContent"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["Stock"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["Remark"] + "</td>");
					stringBuilder.AppendLine("</tr>");
				}
				if (storeOrderGoods.Tables.Count >= 2 && storeOrderGoods.Tables[1] != null && storeOrderGoods.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow row2 in storeOrderGoods.Tables[1].Rows)
					{
						stringBuilder.AppendLine("<tr>");
						stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row2["GiftOrderId"] + "</td>");
						stringBuilder.AppendLine("<td>" + row2["GiftName"] + "[礼品]</td>");
						stringBuilder.AppendLine("<td></td>");
						stringBuilder.AppendLine("<td></td>");
						stringBuilder.AppendLine("<td>" + row2["Quantity"] + "</td>");
						stringBuilder.AppendLine("<td></td>");
						stringBuilder.AppendLine("<td></td>");
						stringBuilder.AppendLine("</tr>");
					}
				}
				stringBuilder.AppendLine("</table>");
				stringBuilder.AppendLine("</body></html>");
				base.Response.Clear();
				base.Response.Buffer = false;
				base.Response.Charset = "GB2312";
				base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
				base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
				base.Response.ContentType = "application/ms-excel";
				this.EnableViewState = false;
				base.Response.Write(stringBuilder.ToString());
				base.Response.End();
			}
		}
	}
}
