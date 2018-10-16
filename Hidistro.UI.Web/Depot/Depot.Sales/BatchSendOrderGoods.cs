using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot.sales
{
	public class BatchSendOrderGoods : StoreAdminPage
	{
		private string strIds;

		protected HiddenField txtSendGoodType;

		protected HtmlGenericControl labSameCity;

		protected DropDownList dropExpressComputerpe;

		protected TextBox txtStartShipOrderNumber;

		protected Button btnSetShipOrderNumber;

		protected Repeater grdOrderGoods;

		protected Button btnBatchSendGoods;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.strIds = base.Request.QueryString["OrderIds"];
			this.btnSetShipOrderNumber.Click += this.btnSetShipOrderNumber_Click;
			this.grdOrderGoods.ItemDataBound += this.grdOrderGoods_RowDataBound;
			this.btnBatchSendGoods.Click += this.btnSendGoods_Click;
			if (!this.Page.IsPostBack)
			{
				this.labSameCity.Visible = HiContext.Current.SiteSettings.OpenDadaLogistics;
				this.dropExpressComputerpe.DataSource = ExpressHelper.GetAllExpress(false);
				this.dropExpressComputerpe.DataTextField = "Name";
				this.dropExpressComputerpe.DataValueField = "Name";
				this.dropExpressComputerpe.DataBind();
				this.dropExpressComputerpe.Items.Insert(0, new ListItem("", ""));
				this.BindData();
			}
		}

		private void grdOrderGoods_RowDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				HiddenField hiddenField = (HiddenField)e.Item.FindControl("hidorderId");
				string value = hiddenField.Value;
				ExpressDropDownList expressDropDownList = e.Item.FindControl("expressList1") as ExpressDropDownList;
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(value);
				if (orderInfo != null && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid || (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && orderInfo.Gateway == "hishop.plugins.payment.podrequest")) && orderInfo.ItemStatus == OrderItemStatus.Nomarl)
				{
					expressDropDownList.DataBind();
					expressDropDownList.SelectedValue = orderInfo.ExpressCompanyName;
				}
			}
		}

		protected string GetOrderIds()
		{
			string text = string.Empty;
			for (int i = 0; i < this.grdOrderGoods.Items.Count; i++)
			{
				HiddenField hiddenField = (HiddenField)this.grdOrderGoods.Items[i].FindControl("hidorderId");
				text = text + hiddenField.Value + ",";
			}
			return text.TrimEnd(',');
		}

		private void btnSetShipOrderNumber_Click(object sender, EventArgs e)
		{
			string orderIds = this.GetOrderIds();
			string[] orderIds2 = orderIds.Split(',');
			long num = 0L;
			string orderIds3 = "'" + orderIds.Replace(",", "','") + "'";
			if (!string.IsNullOrEmpty(this.dropExpressComputerpe.SelectedValue))
			{
				OrderHelper.SetOrderExpressComputerpe(orderIds3, this.dropExpressComputerpe.SelectedItem.Text, this.dropExpressComputerpe.SelectedValue);
			}
			if (!string.IsNullOrEmpty(this.txtStartShipOrderNumber.Text.Trim()) && long.TryParse(this.txtStartShipOrderNumber.Text.Trim(), out num))
			{
				try
				{
					OrderHelper.SetOrderShipNumber(orderIds2, this.txtStartShipOrderNumber.Text.Trim(), this.dropExpressComputerpe.SelectedValue);
					this.BindData();
				}
				catch (Exception)
				{
					this.ShowMsg("你输入的起始单号不正确", false);
				}
			}
			else
			{
				this.ShowMsg("起始发货单号不允许为空且必须为正整数", false);
			}
		}

		private void btnSendGoods_Click(object sender, EventArgs e)
		{
			int num = this.txtSendGoodType.Value.ToInt(0);
			if (this.grdOrderGoods.Items.Count <= 0)
			{
				this.ShowMsg("没有要进行发货的订单。", false);
			}
			else
			{
				int num2 = 0;
				for (int i = 0; i < this.grdOrderGoods.Items.Count; i++)
				{
					HiddenField hiddenField = (HiddenField)this.grdOrderGoods.Items[i].FindControl("txtDeliveryNo");
					string text = hiddenField.Value.ToNullString();
					if (num != 2 || !(text == ""))
					{
						HiddenField hiddenField2 = (HiddenField)this.grdOrderGoods.Items[i].FindControl("hidorderId");
						string value = hiddenField2.Value;
						TextBox textBox = (TextBox)this.grdOrderGoods.Items[i].FindControl("txtShippOrderNumber");
						ExpressDropDownList expressDropDownList = this.grdOrderGoods.Items[i].FindControl("expressList1") as ExpressDropDownList;
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(value);
						if ((orderInfo.GroupBuyId <= 0 || orderInfo.GroupBuyStatus == GroupBuyStatus.Success) && ((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && orderInfo.Gateway == "hishop.plugins.payment.podrequest") || orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))
						{
							ExpressCompanyInfo expressCompanyInfo = null;
							switch (num)
							{
							case 1:
								if (!string.IsNullOrEmpty(expressDropDownList.SelectedValue))
								{
									expressCompanyInfo = ExpressHelper.FindNode(expressDropDownList.SelectedValue);
								}
								if (expressCompanyInfo != null)
								{
									orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
									orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
									orderInfo.ShipOrderNumber = textBox.Text;
								}
								break;
							case 2:
								orderInfo.ExpressCompanyName = "同城物流配送";
								orderInfo.ExpressCompanyAbb = "";
								orderInfo.ShipOrderNumber = "";
								orderInfo.DadaStatus = DadaStatus.WaitOrder;
								break;
							default:
								orderInfo.ExpressCompanyName = "";
								orderInfo.ExpressCompanyAbb = "";
								orderInfo.ShipOrderNumber = "";
								break;
							}
							OrderStatus orderStatus = orderInfo.OrderStatus;
							if (OrderHelper.SendGoods(orderInfo))
							{
								if (expressCompanyInfo != null && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb) && orderInfo.ExpressCompanyAbb.ToUpper() == "HTKY")
								{
									ExpressHelper.GetDataByKuaidi100(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
								}
								if (orderStatus == OrderStatus.WaitBuyerPay)
								{
									OrderHelper.ChangeStoreStockAndWriteLog(orderInfo);
								}
								if (orderInfo.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
								{
									OrderHelper.SetOrderIsStoreCollect(orderInfo.OrderId);
									ProductStatisticsHelper.UpdateOrderSaleStatistics(orderInfo);
									TransactionAnalysisHelper.AnalysisOrderTranData(orderInfo);
								}
								if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
								{
									PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
									if (paymentMode != null)
									{
										string hIGW = paymentMode.Gateway.Replace(".", "_");
										PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.TryDecypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(false), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(""), Globals.FullPath(base.GetRouteUrl("PaymentReturn_url", new
										{
											HIGW = hIGW
										})), Globals.FullPath(base.GetRouteUrl("PaymentNotify_url", new
										{
											HIGW = hIGW
										})), "");
										paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
									}
								}
								if (orderInfo.ExpressCompanyName == "同城物流配送" && !string.IsNullOrEmpty(text))
								{
									SiteSettings masterSettings = SettingsManager.GetMasterSettings();
									DadaHelper.addAfterQuery(masterSettings.DadaSourceID, text);
								}
								if (!string.IsNullOrEmpty(orderInfo.OuterOrderId) && expressCompanyInfo != null)
								{
									if (orderInfo.OuterOrderId.StartsWith("tb_"))
									{
										string text2 = orderInfo.OuterOrderId.Replace("tb_", "");
										try
										{
											string requestUriString = $"http://order2.kuaidiangtong.com/UpdateShipping.ashx?tid={text2}&companycode={expressCompanyInfo.TaobaoCode}&outsid={orderInfo.ShipOrderNumber}&Host={HiContext.Current.SiteUrl}";
											WebRequest webRequest = WebRequest.Create(requestUriString);
											webRequest.GetResponse();
										}
										catch
										{
										}
									}
									else if (orderInfo.OuterOrderId.StartsWith("jd_"))
									{
										string text2 = orderInfo.OuterOrderId.Replace("jd_", "");
									}
								}
								MemberInfo user = Users.GetUser(orderInfo.UserId);
								Messenger.OrderShipping(orderInfo, user);
								orderInfo.OnDeliver();
								num2++;
							}
						}
					}
				}
				if (num2 == 0)
				{
					this.ShowMsg("批量发货失败,商品库存不足或者有商品正在退货,换货中的订单不能发货！", false);
				}
				else if (num2 > 0)
				{
					this.BindData();
					this.ShowMsg($"批量发货成功！发货数量{num2}个,商品库存不足或者有商品正在退货,换货中的订单不能发货！", true);
				}
			}
		}

		private void BindData()
		{
			string orderIds = "'" + this.strIds.Replace(",", "','") + "'";
			this.grdOrderGoods.DataSource = OrderHelper.GetSendGoodsOrders(orderIds, true, true);
			this.grdOrderGoods.DataBind();
		}
	}
}
