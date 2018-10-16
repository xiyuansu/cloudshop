using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Member;
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

namespace Hidistro.UI.Web.Supplier.sales
{
	[AdministerCheck(true)]
	public class ManageOrder : SupplierAdminPage
	{
		protected int Page_CurrentPageIndex = 1;

		protected int Page_CurrentPageSize = 10;

		public int UserStoreId = 0;

		public string StoreShowStyle = "";

		public int OrderStatusID = 0;

		protected HtmlInputText txtOrderId;

		protected HtmlInputHidden ordStatus;

		protected HtmlInputText txtProductName;

		protected HtmlInputText orderStartDate;

		protected HtmlInputText orderEndDate;

		protected HtmlInputText txtShopTo;

		protected RegionSelector dropRegion;

		protected HtmlInputHidden hidOrderId;

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
			this.OrderStatusID = this.Page.Request.QueryString["OrderStatus"].ToInt(0);
			if (this.OrderStatusID < 0)
			{
				this.OrderStatusID = 0;
			}
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			if (!HiContext.Current.SiteSettings.OpenMultStore)
			{
				this.StoreShowStyle = "display:none;";
			}
			this.LoadParameters();
			this.btnOrderGoods.Click += this.btnOrderGoods_Click;
			this.btnProductGoods.Click += this.btnProductGoods_Click;
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
			this.ordStatus.Value = this.OrderStatusID.ToString();
			this.orderStartDate.Value = this.Page.Request["StartDate"];
			this.orderEndDate.Value = this.Page.Request["EndDate"];
			this.txtProductName.Value = Globals.UrlDecode(this.Page.Request["ProductName"]);
			this.txtShopTo.Value = Globals.UrlDecode(this.Page.Request["ShipTo"]);
			int value = this.Page.Request["region"].ToInt(0);
			this.dropRegion.SetSelectedRegionId(value);
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
				DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				if (productGoods.Tables.Count < 2 || productGoods.Tables[1] == null || productGoods.Tables[1].Rows.Count <= 0)
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
				foreach (DataRow row in productGoods.Tables[0].Rows)
				{
					stringBuilder.AppendLine("<tr>");
					stringBuilder.AppendLine("<td>" + row["ProductName"] + "</td>");
					stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["SKUContent"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
					stringBuilder.AppendLine("<td>" + row["Stock"] + "</td>");
					stringBuilder.AppendLine("</tr>");
				}
				if (productGoods.Tables.Count >= 2 && productGoods.Tables[1] != null && productGoods.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow row2 in productGoods.Tables[1].Rows)
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
				DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
				stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
				stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
				stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
				stringBuilder.AppendLine("<td>订单单号</td>");
				if (orderGoods.Tables.Count < 2 || orderGoods.Tables[1] == null || orderGoods.Tables[1].Rows.Count <= 0)
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
				foreach (DataRow row in orderGoods.Tables[0].Rows)
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
				if (orderGoods.Tables.Count >= 2 && orderGoods.Tables[1] != null && orderGoods.Tables[1].Rows.Count > 0)
				{
					foreach (DataRow row2 in orderGoods.Tables[1].Rows)
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

		private void dlstOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string text = "";
				if (!(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
				{
					text = (string)DataBinder.Eval(e.Item.DataItem, "Gateway");
				}
				string orderId = DataBinder.Eval(e.Item.DataItem, "OrderId").ToString();
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
				OrderStatus orderStatus = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				int num = DataBinder.Eval(e.Item.DataItem, "ShippingModeId").ToInt(0);
				int num2 = (int)((DataBinder.Eval(e.Item.DataItem, "GroupBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "GroupBuyId"));
				int num3 = (int)((DataBinder.Eval(e.Item.DataItem, "CountDownBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "CountDownBuyId"));
				int num4 = (int)((DataBinder.Eval(e.Item.DataItem, "BundlingId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "BundlingId"));
				int num5 = DataBinder.Eval(e.Item.DataItem, "ShippingModeId").ToInt(0);
				Label label = (Label)e.Item.FindControl("lkbtnSendGoods");
				Literal literal = (Literal)e.Item.FindControl("isGiftOrder");
				LinkButton linkButton = (LinkButton)e.Item.FindControl("lbtnFightGroup");
				Image image = (Image)e.Item.FindControl("imgError");
				HtmlInputHidden htmlInputHidden = (HtmlInputHidden)e.Item.FindControl("hidFightGroup");
				HtmlAnchor htmlAnchor = (HtmlAnchor)e.Item.FindControl("aftersaleImg");
				image.Visible = orderInfo.IsError;
				if (orderInfo.IsError)
				{
					image.Attributes.Add("title", orderInfo.ErrorMessage);
					image.ImageUrl = "\\Supplier\\images\\orderError.png";
				}
				if (orderInfo.ItemStatus != 0 || orderInfo.OrderStatus == OrderStatus.ApplyForRefund)
				{
					if (orderInfo.OrderStatus == OrderStatus.ApplyForRefund)
					{
						RefundInfo refundInfo = TradeHelper.GetRefundInfo(orderInfo.OrderId);
						if (refundInfo != null)
						{
							htmlAnchor.Visible = true;
							htmlAnchor.Title = "订单已申请退款";
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
							htmlAnchor.Title = "订单中有商品正在退货/换货中";
						}
						else if (orderInfo.ItemStatus == OrderItemStatus.HasReplace)
						{
							htmlAnchor.Title = "订单中有商品正在进行换货";
						}
						else if (orderInfo.ItemStatus == OrderItemStatus.HasReturn)
						{
							htmlAnchor.Title = "订单中有商品在进行退货/退款操作";
						}
						else if (orderInfo.ReturnedCount > 0)
						{
							htmlAnchor.Title = "订单中有商品已退货完成";
						}
						if (num6 > 0)
						{
							htmlAnchor.Visible = true;
							if (afterSaleTypes == AfterSaleTypes.ReturnAndRefund)
							{
								htmlAnchor.HRef = "ReturnApplyDetail?ReturnId=" + num7;
							}
							else
							{
								htmlAnchor.HRef = "ReplaceApplyDetail?ReplaceId=" + num7;
							}
						}
					}
				}
				if (orderInfo.FightGroupId > 0)
				{
					FightGroupInfo fightGroup = VShopHelper.GetFightGroup(orderInfo.FightGroupId);
					if (fightGroup != null)
					{
						linkButton.PostBackUrl = "/Supplier/vshop/FightGroupDetails.aspx?fightGroupActivityId=" + fightGroup.FightGroupActivityId;
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
				HtmlAnchor htmlAnchor2 = (HtmlAnchor)e.Item.FindControl("lkbtnToDetail");
				int num8 = (int)((DataBinder.Eval(e.Item.DataItem, "StoreId") == DBNull.Value) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "StoreId"));
				bool flag = (bool)((DataBinder.Eval(e.Item.DataItem, "IsStoreCollect") == DBNull.Value) ? ((object)false) : DataBinder.Eval(e.Item.DataItem, "IsStoreCollect"));
				if (orderInfo.LineItems.Count <= 0)
				{
					literal.Text = "(礼)";
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
				if (num2 > 0)
				{
					string[] source = new string[1]
					{
						"hishop.plugins.payment.podrequest"
					};
					GroupBuyStatus groupBuyStatus = (GroupBuyStatus)DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
					Label label2 = label;
					int visible;
					if (orderInfo.ItemStatus == OrderItemStatus.Nomarl)
					{
						switch (orderStatus)
						{
						case OrderStatus.WaitBuyerPay:
							if (source.Contains(text))
							{
								goto case OrderStatus.BuyerAlreadyPaid;
							}
							goto default;
						case OrderStatus.BuyerAlreadyPaid:
							visible = ((groupBuyStatus == GroupBuyStatus.Success) ? 1 : 0);
							break;
						default:
							visible = 0;
							break;
						}
					}
					else
					{
						visible = 0;
					}
					label2.Visible = ((byte)visible != 0);
				}
				else if (num3 > 0 || num4 > 0)
				{
					Label label3 = label;
					int visible2;
					if (orderInfo.ItemStatus == OrderItemStatus.Nomarl)
					{
						switch (orderStatus)
						{
						case OrderStatus.WaitBuyerPay:
							visible2 = ((text == "hishop.plugins.payment.podrequest") ? 1 : 0);
							break;
						default:
							visible2 = 0;
							break;
						case OrderStatus.BuyerAlreadyPaid:
							visible2 = 1;
							break;
						}
					}
					else
					{
						visible2 = 0;
					}
					label3.Visible = ((byte)visible2 != 0);
				}
				else if (masterSettings.OpenMultStore)
				{
					Label label4 = label;
					int visible3;
					switch (orderStatus)
					{
					case OrderStatus.WaitBuyerPay:
						if (text == "hishop.plugins.payment.podrequest")
						{
							goto case OrderStatus.BuyerAlreadyPaid;
						}
						goto default;
					case OrderStatus.BuyerAlreadyPaid:
						if (num5 != -2)
						{
							visible3 = ((orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
							break;
						}
						goto default;
					default:
						visible3 = 0;
						break;
					}
					label4.Visible = ((byte)visible3 != 0);
				}
				else
				{
					Label label5 = label;
					int visible4;
					switch (orderStatus)
					{
					case OrderStatus.WaitBuyerPay:
						if (text == "hishop.plugins.payment.podrequest")
						{
							goto case OrderStatus.BuyerAlreadyPaid;
						}
						goto default;
					case OrderStatus.BuyerAlreadyPaid:
						visible4 = ((orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
						break;
					default:
						visible4 = 0;
						break;
					}
					label5.Visible = ((byte)visible4 != 0);
				}
			}
		}
	}
}
