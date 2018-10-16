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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Orders)]
	public class ManageOrder : AdminPage
	{
		public int UserStoreId = 0;

		public int OrderStatusID = 0;

		public bool showAnchors999 = true;

		protected int? Page_OrderStatus;

		protected int Page_CurrentPageIndex = 1;

		protected int Page_CurrentPageSize = 10;

		protected bool Page_IsShowStore = false;

		protected int? IsAllotStore = null;

		protected int? IsPrinted = null;

		protected HtmlInputText txtOrderId;

		protected HiddenField hidStatus;

		protected HtmlInputText txtUserName;

		protected CalendarPanel cldStartDate;

		protected CalendarPanel cldEndDate;

		protected OrderTypeDrowpDownList ddlOrderType;

		protected HtmlGenericControl listore;

		protected StoreDropDownList ddlSearchStore;

		protected SourceOrderDrowpDownList dropsourceorder;

		protected HtmlInputText so_more_input;

		protected HtmlGenericControl so_more_none;

		protected HtmlInputText txtProductName;

		protected HtmlInputText txtShopTo;

		protected HtmlGenericControl liStoreFilter;

		protected RegionSelector dropRegion;

		protected DropDownList dropInvoiceType;

		protected HyperLink hplinkprint;

		protected HtmlInputHidden hidOrderId;

		protected CloseTranReasonDropDownList ddlCloseReason;

		protected FormatedMoneyLabel lblOrderTotalForRemark;

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
			if (HiContext.Current.Manager != null)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			this.ddlSearchStore.DataBind();
			this.dropsourceorder.DataBind();
			this.ddlOrderType.DataBind();
			this.LoadParameters();
			if (HiContext.Current.SiteSettings.OpenMultStore)
			{
				this.Page_IsShowStore = true;
			}
			if (base.Request.QueryString["IsMoreSearch"].ToBool())
			{
				this.so_more_none.Style.Add("display", "block");
			}
			else
			{
				this.so_more_none.Style.Add("display", "none");
			}
			this.btnRemark.Click += this.btnRemark_Click;
			this.btnOrderGoods.Click += this.btnOrderGoods_Click;
			this.btnProductGoods.Click += this.btnProductGoods_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				if (!masterSettings.OpenMultStore)
				{
					this.showAnchors999 = false;
					this.listore.Visible = false;
					this.liStoreFilter.Visible = false;
				}
				this.BindErpLink();
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
			this.Page_OrderStatus = this.Page.Request.QueryString["OrderStatus"].ToInt(0);
			if (this.Page_OrderStatus < 0)
			{
				this.Page_OrderStatus = 0;
			}
			this.hidStatus.Value = this.Page_OrderStatus.ToString();
			this.hidGroupId.Value = this.Page.Request["GroupBuyId"];
			this.txtUserName.Value = Globals.UrlDecode(this.Page.Request["UserName"]);
			this.cldStartDate.Text = this.Page.Request["StartDate"];
			this.cldEndDate.Text = this.Page.Request["EndDate"];
			this.txtProductName.Value = Globals.UrlDecode(this.Page.Request["ProductName"]);
			this.txtShopTo.Value = Globals.UrlDecode(this.Page.Request["ShipTo"]);
			this.IsAllotStore = this.Page.Request["IsAllotStore"].ToInt(0);
			if (!string.IsNullOrWhiteSpace(this.Page.Request["IsPrinted"]))
			{
				this.IsPrinted = this.Page.Request["IsPrinted"].ToInt(0);
			}
			string b = this.Page.Request["StoreId"];
			foreach (ListItem item in this.ddlSearchStore.Items)
			{
				if (item.Value == b)
				{
					item.Selected = true;
				}
			}
			this.dropsourceorder.SelectedValue = this.Page.Request["SourceOrder"].ToInt(0);
			this.dropRegion.SetSelectedRegionId(this.Page.Request["RegionId"].ToInt(0));
			this.dropInvoiceType.SelectedValue = this.Page.Request["InvoiceType"].ToNullString();
		}

		private void BindErpLink()
		{
			string text = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
			string checkCode = HiContext.Current.SiteSettings.CheckCode;
			string text2 = HiContext.Current.SiteSettings.AppKey;
			if (string.IsNullOrEmpty(text2))
			{
				text2 = this.CreateAppKey();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.AppKey = text2;
				SettingsManager.Save(masterSettings);
				this.RegisterERP(text2, checkCode);
			}
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			sortedDictionary.Add("app_key", text2);
			sortedDictionary.Add("timestamp", text);
			string text3 = HiCryptographer.SignTopRequest(sortedDictionary, checkCode);
			this.hplinkprint.NavigateUrl = "http://hierp.huz.cn/ExpressBill/Allot?app_key=" + text2 + "&timestamp=" + text + "&sign=" + text3;
		}

		private string CreateAppKey()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(ushort)(48 + (ushort)(num % 7))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMddHHmmss") + text;
		}

		private void RegisterERP(string appkey, string appsecret)
		{
			string url = "http://hierp.huz.cn/api/commercialtenantregister";
			string text = Globals.HostPath(HiContext.Current.Context.Request.Url) + "/OpenAPI/";
			string postResult = Globals.GetPostResult(url, "appKey=" + appkey + "&appSecret=" + appsecret + "&getSoldTrades=" + text + "Hishop.Open.Api.ITrade.GetSoldTrades&getIncrementSoldTrades=" + text + "Hishop.Open.Api.ITrade.GetIncrementSoldTrades&sendLogistic=" + text + "Hishop.Open.Api.ITrade.SendLogistic");
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
				string text2 = DataBinder.Eval(e.Item.DataItem, "OrderId").ToString();
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text2);
				OrderStatus orderStatus = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				int num = DataBinder.Eval(e.Item.DataItem, "ShippingModeId").ToInt(0);
				ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
				Literal literal = (Literal)e.Item.FindControl("litCloseOrder");
				HyperLink hyperLink = (HyperLink)e.Item.FindControl("lkbtnEditPrice");
				int num2 = (int)((DataBinder.Eval(e.Item.DataItem, "GroupBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "GroupBuyId"));
				int num3 = (int)((DataBinder.Eval(e.Item.DataItem, "CountDownBuyId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "CountDownBuyId"));
				int num4 = (int)((DataBinder.Eval(e.Item.DataItem, "BundlingId") is DBNull) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "BundlingId"));
				int num5 = (DataBinder.Eval(e.Item.DataItem, "StoreId") != DBNull.Value) ? DataBinder.Eval(e.Item.DataItem, "StoreId").ToInt(0) : 0;
				int num6 = DataBinder.Eval(e.Item.DataItem, "ShippingModeId").ToInt(0);
				ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
				Label label = (Label)e.Item.FindControl("lkbtnSendGoods");
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
							htmlAnchor2.HRef = "RefundApplyDetail?RefundId=" + refundInfo.RefundId;
						}
					}
					else
					{
						int num7 = 0;
						AfterSaleTypes afterSaleTypes = AfterSaleTypes.ReturnAndRefund;
						int num8 = 0;
						foreach (LineItemInfo value in orderInfo.LineItems.Values)
						{
							if (value.ReturnInfo != null || value.ReplaceInfo != null)
							{
								ReturnInfo returnInfo = value.ReturnInfo;
								ReplaceInfo replaceInfo = value.ReplaceInfo;
								if (num7 == 0 || (returnInfo != null && returnInfo.HandleStatus != ReturnStatus.Refused && returnInfo.HandleStatus != ReturnStatus.Returned) || (replaceInfo != null && (replaceInfo.HandleStatus != ReplaceStatus.Refused || replaceInfo.HandleStatus != ReplaceStatus.Replaced)))
								{
									if (value.ReturnInfo != null)
									{
										afterSaleTypes = AfterSaleTypes.ReturnAndRefund;
										num8 = value.ReturnInfo.ReturnId;
									}
									else
									{
										afterSaleTypes = AfterSaleTypes.Replace;
										num8 = value.ReplaceInfo.ReplaceId;
									}
								}
								num7++;
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
						if (num7 > 0)
						{
							htmlAnchor2.Visible = true;
							if (afterSaleTypes == AfterSaleTypes.ReturnAndRefund)
							{
								htmlAnchor2.HRef = "ReturnApplyDetail?ReturnId=" + num8;
							}
							else
							{
								htmlAnchor2.HRef = "ReplaceApplyDetail?ReplaceId=" + num8;
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
				int num9 = (int)((DataBinder.Eval(e.Item.DataItem, "StoreId") == DBNull.Value) ? ((object)0) : DataBinder.Eval(e.Item.DataItem, "StoreId"));
				bool flag = (bool)((DataBinder.Eval(e.Item.DataItem, "IsStoreCollect") == DBNull.Value) ? ((object)false) : DataBinder.Eval(e.Item.DataItem, "IsStoreCollect"));
				bool flag2 = false;
				if (refundInfo != null)
				{
					htmlAnchor.HRef = "RefundApplyDetail?RefundId=" + refundInfo.RefundId.ToString();
					flag2 = true;
				}
				if (orderInfo.LineItems.Count <= 0)
				{
					if (orderInfo.UserAwardRecordsId > 0)
					{
						literal3.Text = "(奖)";
					}
					else
					{
						literal3.Text = "(礼)";
					}
				}
				HtmlGenericControl htmlGenericControl = (HtmlGenericControl)e.Item.FindControl("spanstoretitle");
				if (!masterSettings.OpenMultStore)
				{
					literal2.Visible = false;
					htmlGenericControl.Visible = false;
				}
				if (orderInfo.GroupBuyId > 0 || orderInfo.CountDownBuyId > 0)
				{
					literal2.Visible = false;
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
							hyperLink.NavigateUrl = "javascript:DialogFrame('/Admin/sales/EditOrder.aspx?OrderId=" + text2 + "','修改订单价格',null,null,function(e){location.reload();})";
							hyperLink.Visible = true;
							literal.Visible = true;
							if (text != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0)
							{
								imageLinkButton.Visible = true;
							}
						}
						else if (productPreSaleInfo.PaymentStartDate <= DateTime.Now && productPreSaleInfo.PaymentEndDate >= DateTime.Now && text != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0)
						{
							imageLinkButton.Visible = true;
						}
					}
					else
					{
						hyperLink.NavigateUrl = "javascript:DialogFrame('/Admin/sales/EditOrder.aspx?OrderId=" + text2 + "','修改订单价格',null,null,function(e){location.reload();})";
						hyperLink.Visible = true;
						if (text != "hishop.plugins.payment.podrequest" && num != -2 && orderInfo.FightGroupId == 0 && (orderInfo.ParentOrderId == "0" || orderInfo.ParentOrderId == "-1"))
						{
							imageLinkButton.Visible = true;
							literal.Visible = true;
						}
					}
				}
				if (text == "hishop.plugins.payment.podrequest" && (orderStatus == OrderStatus.WaitBuyerPay || orderStatus == OrderStatus.SellerAlreadySent) && (orderInfo.ParentOrderId == "0" || orderInfo.ParentOrderId == "-1"))
				{
					literal.Visible = true;
				}
				if (orderStatus == OrderStatus.SellerAlreadySent && orderInfo.StoreId > 0 && masterSettings.OpenMultStore)
				{
					literal.Visible = false;
				}
				if (orderStatus == OrderStatus.ApplyForRefund && !orderInfo.IsStoreCollect)
				{
					htmlAnchor.Visible = true;
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
					literal2.Visible = false;
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
					if (num5 == -1)
					{
						literal2.Text = "分配门店";
						Literal literal10 = literal2;
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
							if (orderInfo.ItemStatus == OrderItemStatus.Nomarl)
							{
								visible3 = ((orderInfo.LineItems.Count > 0) ? 1 : 0);
								break;
							}
							goto default;
						default:
							visible3 = 0;
							break;
						}
						literal10.Visible = ((byte)visible3 != 0);
					}
					else if (num5 > 0)
					{
						literal2.Text = "更改门店";
						Literal literal11 = literal2;
						int visible4;
						switch (orderStatus)
						{
						case OrderStatus.WaitBuyerPay:
							if (!(text == "hishop.plugins.payment.podrequest") && orderInfo.PaymentTypeId != -3)
							{
								goto default;
							}
							goto case OrderStatus.BuyerAlreadyPaid;
						case OrderStatus.BuyerAlreadyPaid:
							if (!orderInfo.IsConfirm)
							{
								visible4 = ((orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
								break;
							}
							goto default;
						default:
							visible4 = 0;
							break;
						}
						literal11.Visible = ((byte)visible4 != 0);
					}
					else if (num5 == 0)
					{
						Literal literal12 = literal2;
						int visible5;
						switch (orderStatus)
						{
						case OrderStatus.WaitBuyerPay:
							if (text == "hishop.plugins.payment.podrequest")
							{
								goto case OrderStatus.BuyerAlreadyPaid;
							}
							goto default;
						case OrderStatus.BuyerAlreadyPaid:
							if (num6 != -2 && orderInfo.ItemStatus == OrderItemStatus.Nomarl)
							{
								visible5 = ((orderInfo.LineItems.Count > 0) ? 1 : 0);
								break;
							}
							goto default;
						default:
							visible5 = 0;
							break;
						}
						literal12.Visible = ((byte)visible5 != 0);
						Label label4 = label;
						int visible6;
						switch (orderStatus)
						{
						case OrderStatus.WaitBuyerPay:
							if (text == "hishop.plugins.payment.podrequest")
							{
								goto case OrderStatus.BuyerAlreadyPaid;
							}
							goto default;
						case OrderStatus.BuyerAlreadyPaid:
							if (num6 != -2)
							{
								visible6 = ((orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
								break;
							}
							goto default;
						default:
							visible6 = 0;
							break;
						}
						label4.Visible = ((byte)visible6 != 0);
					}
				}
				else
				{
					Label label5 = label;
					int visible7;
					switch (orderStatus)
					{
					case OrderStatus.WaitBuyerPay:
						if (text == "hishop.plugins.payment.podrequest")
						{
							goto case OrderStatus.BuyerAlreadyPaid;
						}
						goto default;
					case OrderStatus.BuyerAlreadyPaid:
						visible7 = ((orderInfo.ItemStatus == OrderItemStatus.Nomarl) ? 1 : 0);
						break;
					default:
						visible7 = 0;
						break;
					}
					label5.Visible = ((byte)visible7 != 0);
				}
				imageLinkButton2.Visible = (orderStatus == OrderStatus.SellerAlreadySent && orderInfo.ItemStatus == OrderItemStatus.Nomarl);
				if (masterSettings.OpenMultStore && num == -2 && orderStatus == OrderStatus.WaitBuyerPay)
				{
					literal.Visible = !orderInfo.IsConfirm;
					hyperLink.Visible = true;
					if (text != "hishop.plugins.payment.podrequest" && orderInfo.PaymentTypeId != -3 && orderInfo.FightGroupId == 0)
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
