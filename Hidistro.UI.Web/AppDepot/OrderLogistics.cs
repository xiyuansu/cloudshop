using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.AppDepot
{
	public class OrderLogistics : Page
	{
		private const string DADA_EXPRESS_COMPANY_NAME = "同城物流配送";

		protected HtmlGenericControl LogisticsInfoPanel;

		protected HtmlInputHidden hidOrderId;

		protected HtmlInputHidden hidReplaceType;

		protected Literal ltlExpressCompanyName;

		protected Literal ltlShipOrderNumber;

		protected Literal ltlReceiveName;

		protected Literal ltlTel;

		protected Literal ltlShipAddress;

		protected HtmlGenericControl ulExpress;

		protected HyperLink hylExpress100Search;

		protected HtmlGenericControl divlogisticsInfo;

		protected HtmlInputHidden hidIsShowDadaGIS;

		protected HtmlInputHidden hidUserLatlng;

		protected HtmlInputHidden hidStoreLatlng;

		protected HtmlInputHidden hidBaseURL;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.hidBaseURL.Value = Globals.HostPath(base.Request.Url);
			string text = this.Page.Request.QueryString["orderId"];
			Literal literal = this.FindControl("litLogisiticType") as Literal;
			int num = 0;
			int.TryParse(this.Page.Request.QueryString["returnsId"], out num);
			int num2 = 0;
			int.TryParse(this.Page.Request.QueryString["replaceId"], out num2);
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			bool flag = false;
			ReturnInfo returnInfo = null;
			ReplaceInfo replaceInfo = null;
			if (num > 0)
			{
				returnInfo = TradeHelper.GetReturnInfo(num);
			}
			if (num2 > 0)
			{
				replaceInfo = TradeHelper.GetReplaceInfo(num2);
			}
			OrderInfo orderInfo = null;
			if (!string.IsNullOrEmpty(text))
			{
				orderInfo = ShoppingProcessor.GetOrderInfo(text);
				if (orderInfo != null && orderInfo.ExpressCompanyName == "同城物流配送")
				{
					this.hidIsShowDadaGIS.Value = "1";
				}
				if (orderInfo != null && !string.IsNullOrWhiteSpace(orderInfo.LatLng))
				{
					this.hidUserLatlng.Value = orderInfo.LatLng;
				}
				else if (orderInfo != null && orderInfo.ShippingId > 0)
				{
					ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(orderInfo.ShippingId);
					if (shippingAddress != null && !string.IsNullOrWhiteSpace(shippingAddress.LatLng))
					{
						this.hidUserLatlng.Value = shippingAddress.LatLng;
					}
				}
				if (orderInfo != null)
				{
					if (orderInfo.StoreId > 0)
					{
						StoresInfo storeById = StoresHelper.GetStoreById(orderInfo.StoreId);
						if (storeById != null && !string.IsNullOrWhiteSpace(storeById.LatLng))
						{
							this.hidStoreLatlng.Value = storeById.LatLng;
						}
					}
					else
					{
						ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
						if (defaultOrFirstShipper != null)
						{
							this.hidStoreLatlng.Value = defaultOrFirstShipper.Latitude + "," + defaultOrFirstShipper.Longitude;
						}
					}
				}
			}
			int num3;
			if (returnInfo != null)
			{
				if (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Returned)
				{
					text2 = returnInfo.ExpressCompanyName;
					text3 = returnInfo.ShipOrderNumber;
					text4 = returnInfo.AdminShipTo;
					text5 = returnInfo.AdminCellPhone;
					text6 = returnInfo.AdminShipAddress;
					text7 = returnInfo.ExpressCompanyAbb;
					flag = true;
				}
			}
			else if (replaceInfo != null)
			{
				if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
				{
					this.hidReplaceType.Value = "User";
					text2 = replaceInfo.UserExpressCompanyName;
					text3 = replaceInfo.UserShipOrderNumber;
					text4 = replaceInfo.AdminShipTo;
					text5 = replaceInfo.AdminCellPhone;
					text6 = replaceInfo.AdminShipAddress;
					text7 = replaceInfo.ExpressCompanyAbb;
					flag = true;
				}
				else if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery || replaceInfo.HandleStatus == ReplaceStatus.Replaced)
				{
					this.hidReplaceType.Value = "Mall";
					text2 = replaceInfo.ExpressCompanyName;
					text3 = replaceInfo.ShipOrderNumber;
					text4 = orderInfo.ShipTo;
					text5 = (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone);
					text6 = orderInfo.ShippingRegion + "   " + orderInfo.Address;
					text7 = orderInfo.ExpressCompanyAbb;
				}
				else
				{
					this.LogisticsInfoPanel.InnerHtml = "商家还未发货";
				}
			}
			else if (orderInfo != null)
			{
				if ((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent || orderInfo.OrderStatus == OrderStatus.Finished || (!string.IsNullOrEmpty(orderInfo.ExpressCompanyName) && !string.IsNullOrEmpty(orderInfo.ShipOrderNumber))) && orderInfo.ItemStatus != OrderItemStatus.HasReturnOrReplace)
				{
					num3 = ((orderInfo.ShippingModeId != -2) ? 1 : 0);
					goto IL_0456;
				}
				num3 = 0;
				goto IL_0456;
			}
			goto IL_0778;
			IL_0778:
			if (flag)
			{
				this.hidOrderId.Value = ((orderInfo != null) ? orderInfo.OrderId : string.Empty);
				this.ltlExpressCompanyName.Text = (text2.Equals("店员配送") ? "店员配送" : (text2 + "："));
				this.ltlShipOrderNumber.Text = text3;
				this.divlogisticsInfo.Visible = !text2.Equals("店员配送");
				this.ltlReceiveName.Text = text4;
				this.ltlTel.Text = text5;
				this.ltlShipAddress.Text = text6;
				if (!string.IsNullOrEmpty(text3))
				{
					string text8 = HttpContext.Current.Request.Url.ToString().ToLower();
					if (text8.IndexOf("/wapshop/") != -1)
					{
						if (text7.ToLower() == "sf")
						{
							this.hylExpress100Search.NavigateUrl = $"http://www.sf-express.com/mobile/cn/sc/dynamic_function/waybill/waybill_query_info.html?billno={text3}";
							this.hylExpress100Search.Text = "顺丰官网查询>";
						}
						else
						{
							this.hylExpress100Search.NavigateUrl = $"https://m.kuaidi100.com/result.jsp?nu={text3}";
						}
					}
					else if (text7.ToLower() == "sf")
					{
						this.hylExpress100Search.NavigateUrl = $"http://www.sf-express.com/mobile/cn/sc/dynamic_function/waybill/waybill_query_info.html?billno={text3}";
						this.hylExpress100Search.Text = "顺丰官网查询>";
					}
					else
					{
						this.hylExpress100Search.NavigateUrl = $"https://www.kuaidi100.com/chaxun?nu={text3}";
					}
				}
				else
				{
					this.ulExpress.Visible = false;
				}
			}
			return;
			IL_0456:
			if (num3 != 0)
			{
				flag = true;
				text2 = ((orderInfo.ExpressCompanyName != null) ? orderInfo.ExpressCompanyName : "");
				text3 = orderInfo.ShipOrderNumber;
				text4 = orderInfo.ShipTo;
				text5 = (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone);
				text6 = orderInfo.ShippingRegion + "   " + orderInfo.Address;
				text7 = orderInfo.ExpressCompanyAbb;
			}
			else if (orderInfo.ItemStatus == OrderItemStatus.HasReturn)
			{
				returnInfo = TradeHelper.GetReturnInfo(orderInfo.OrderId, "");
				if (returnInfo == null)
				{
					foreach (LineItemInfo value in orderInfo.LineItems.Values)
					{
						if (value.ReturnInfo != null)
						{
							returnInfo = value.ReturnInfo;
							break;
						}
					}
				}
				if (returnInfo != null && (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Returned))
				{
					text2 = returnInfo.ExpressCompanyName;
					text3 = returnInfo.ShipOrderNumber;
					text4 = returnInfo.AdminShipTo;
					text5 = returnInfo.AdminCellPhone;
					text6 = returnInfo.AdminShipAddress;
					text7 = returnInfo.ExpressCompanyAbb;
					flag = true;
				}
			}
			else if (orderInfo.ItemStatus == OrderItemStatus.HasReplace)
			{
				replaceInfo = TradeHelper.GetReplaceInfo(orderInfo.OrderId, "");
				orderInfo = ShoppingProcessor.GetOrderInfo(replaceInfo.OrderId);
				if (orderInfo == null)
				{
					foreach (LineItemInfo value2 in orderInfo.LineItems.Values)
					{
						if (value2.ReplaceInfo != null)
						{
							replaceInfo = value2.ReplaceInfo;
							break;
						}
					}
				}
				if (replaceInfo != null)
				{
					if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
					{
						this.hidReplaceType.Value = "User";
						text2 = replaceInfo.UserExpressCompanyName;
						text3 = replaceInfo.UserShipOrderNumber;
						text4 = replaceInfo.AdminShipTo;
						text5 = replaceInfo.AdminCellPhone;
						text6 = replaceInfo.AdminShipAddress;
						text7 = replaceInfo.ExpressCompanyAbb;
						flag = true;
					}
					if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery || replaceInfo.HandleStatus == ReplaceStatus.Replaced)
					{
						this.hidReplaceType.Value = "Mall";
						text2 = replaceInfo.ExpressCompanyName;
						text3 = replaceInfo.ShipOrderNumber;
						text4 = orderInfo.ShipTo;
						text5 = (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone);
						text6 = orderInfo.ShippingRegion + "   " + orderInfo.Address;
						text7 = orderInfo.ExpressCompanyAbb;
					}
					else
					{
						this.LogisticsInfoPanel.InnerHtml = "商家还未发货";
					}
				}
			}
			goto IL_0778;
		}
	}
}
