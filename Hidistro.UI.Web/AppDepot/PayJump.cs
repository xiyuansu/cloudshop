using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.AppDepot
{
	public class PayJump : Page
	{
		protected OrderInfo Order;

		protected string OrderId;

		protected decimal Amount;

		protected string Gateway;

		private StoreCollectionInfo offlineOrder = null;

		private bool isOfflineOrder = false;

		private string sessionId = "";

		protected HtmlForm myform;

		protected HiddenField hasWxPayRight;

		protected HiddenField hasAliPayRight;

		protected HiddenField hidErrMsg;

		protected HtmlGenericControl inputPanel;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.isOfflineOrder = (base.Request.QueryString["isOffline"].ToNullString().ToLower() == "true");
			bool flag = false;
			bool flag2 = false;
			string text = HttpContext.Current.Request.UserAgent;
			if (string.IsNullOrEmpty(text))
			{
				text = "";
			}
			bool flag3 = false;
			if (text.ToLower().IndexOf("micromessenger") > -1)
			{
				flag3 = true;
			}
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!string.IsNullOrEmpty(siteSettings.WeixinAppId) && !string.IsNullOrEmpty(siteSettings.WeixinAppSecret) && !string.IsNullOrEmpty(siteSettings.WeixinPartnerID) && !string.IsNullOrEmpty(siteSettings.WeixinPartnerKey))
			{
				this.hasWxPayRight.Value = "1";
				flag = true;
			}
			else
			{
				this.hasWxPayRight.Value = "0";
			}
			PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
			if (paymentMode != null)
			{
				this.hasAliPayRight.Value = "1";
				flag2 = true;
			}
			else
			{
				this.hasAliPayRight.Value = "0";
			}
			this.sessionId = this.Page.Request["SessionId"].ToNullString();
			if (!string.IsNullOrEmpty(this.sessionId))
			{
				this.inputPanel.Visible = true;
			}
			else
			{
				this.inputPanel.Visible = false;
			}
			if (!this.isOfflineOrder)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request.QueryString["OrderId"].ToNullString());
				if (orderInfo == null)
				{
					this.hidErrMsg.Value = "错误的订单ID";
				}
				else if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
				{
					this.hidErrMsg.Value = "错误的订单状态";
				}
				else
				{
					EnumPaymentType enumPaymentType = flag3 ? EnumPaymentType.WXPay : EnumPaymentType.WapAliPay;
					this.offlineOrder = StoresHelper.GetStoreCollectionInfo(this.OrderId);
					if (this.offlineOrder == null)
					{
						StoreCollectionInfo storeCollectionInfo = new StoreCollectionInfo();
						storeCollectionInfo.CreateTime = orderInfo.OrderDate;
						storeCollectionInfo.FinishTime = DateTime.Now;
						storeCollectionInfo.PayTime = DateTime.Now;
						storeCollectionInfo.PaymentTypeId = (int)enumPaymentType;
						storeCollectionInfo.PaymentTypeName = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 0);
						storeCollectionInfo.GateWay = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 1);
						storeCollectionInfo.OrderId = orderInfo.OrderId;
						storeCollectionInfo.OrderType = 1;
						storeCollectionInfo.PayAmount = orderInfo.GetTotal(false);
						storeCollectionInfo.RefundAmount = decimal.Zero;
						storeCollectionInfo.Remark = "上门自提订单确认提货:" + orderInfo.OrderId;
						storeCollectionInfo.SerialNumber = Globals.GetGenerateId();
						storeCollectionInfo.Status = 0;
						storeCollectionInfo.StoreId = orderInfo.StoreId;
						storeCollectionInfo.UserId = orderInfo.UserId;
						StoresHelper.AddStoreCollectionInfo(storeCollectionInfo);
					}
					else
					{
						this.offlineOrder.PaymentTypeId = (int)enumPaymentType;
						this.offlineOrder.PaymentTypeName = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 0);
						this.offlineOrder.GateWay = EnumDescription.GetEnumDescription((Enum)(object)enumPaymentType, 1);
						StoresHelper.UpdateStoreCollectionInfo(this.offlineOrder);
					}
				}
			}
			if (flag3)
			{
				if (flag && !this.inputPanel.Visible)
				{
					base.Response.Redirect("/Vshop/StoreOrderPay?OrderId=" + base.Request.QueryString["OrderId"].ToNullString() + "&IsOffline=" + base.Request.QueryString["isOffline"].ToNullString());
				}
			}
			else if (flag2 && !this.inputPanel.Visible)
			{
				base.Response.Redirect("/WapShop/StoreOrderPay?OrderId=" + base.Request.QueryString["OrderId"].ToNullString() + "&IsOffline=" + base.Request.QueryString["isOffline"].ToNullString());
			}
		}
	}
}
