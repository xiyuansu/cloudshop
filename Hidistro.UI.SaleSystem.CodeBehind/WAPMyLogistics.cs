using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPMyLogistics : WAPMemberTemplatedWebControl
	{
		private HtmlGenericControl LogisticsInfo;

		private HyperLink hylExpress100Search;

		private HtmlGenericControl ulExpress;

		private const string DADA_EXPRESS_COMPANY_NAME = "同城物流配送";

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyLogistics.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string orderId = this.Page.Request.QueryString["orderId"];
			this.LogisticsInfo = (HtmlGenericControl)this.FindControl("LogisticsInfoPanel");
			Literal literal = this.FindControl("litLogisiticType") as Literal;
			this.hylExpress100Search = (HyperLink)this.FindControl("hylExpress100Search");
			this.ulExpress = (HtmlGenericControl)this.FindControl("ulExpress");
			int num = 0;
			int.TryParse(this.Page.Request.QueryString["returnsId"], out num);
			int num2 = 0;
			int.TryParse(this.Page.Request.QueryString["replaceId"], out num2);
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			bool flag = false;
			HtmlInputHidden htmlInputHidden = this.FindControl("hidReplaceType") as HtmlInputHidden;
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			if (orderInfo == null || orderInfo.UserId != HiContext.Current.UserId)
			{
				if (num > 0)
				{
					ReturnInfo returnInfo = TradeHelper.GetReturnInfo(num);
					if (returnInfo != null && (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Returned))
					{
						text = returnInfo.ExpressCompanyName;
						text2 = returnInfo.ShipOrderNumber;
						text3 = returnInfo.AdminShipTo;
						text4 = returnInfo.AdminCellPhone;
						text5 = returnInfo.AdminShipAddress;
						text6 = returnInfo.ExpressCompanyAbb;
						flag = true;
					}
				}
				if (num2 > 0)
				{
					ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(num2);
					orderInfo = ShoppingProcessor.GetOrderInfo(replaceInfo.OrderId);
					if (orderInfo == null)
					{
						this.LogisticsInfo.InnerHtml = "订单不存在或已被删除";
						return;
					}
					if (replaceInfo != null)
					{
						if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
						{
							htmlInputHidden.Value = "User";
							text = replaceInfo.UserExpressCompanyName;
							text2 = replaceInfo.UserShipOrderNumber;
							text3 = replaceInfo.AdminShipTo;
							text4 = replaceInfo.AdminCellPhone;
							text5 = replaceInfo.AdminShipAddress;
							text6 = replaceInfo.ExpressCompanyAbb;
							flag = true;
						}
						if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery || replaceInfo.HandleStatus == ReplaceStatus.Replaced)
						{
							htmlInputHidden.Value = "Mall";
							text = replaceInfo.ExpressCompanyName;
							text2 = replaceInfo.ShipOrderNumber;
							text3 = orderInfo.ShipTo;
							text4 = (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone);
							text5 = orderInfo.ShippingRegion + "   " + orderInfo.Address;
							text6 = orderInfo.ExpressCompanyAbb;
						}
						else
						{
							this.LogisticsInfo.InnerHtml = "商家还未发货";
						}
					}
				}
			}
			else
			{
				flag = true;
				text = orderInfo.ExpressCompanyName;
				text2 = orderInfo.ShipOrderNumber;
				text3 = orderInfo.ShipTo;
				text4 = (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone);
				text5 = orderInfo.ShippingRegion + "   " + orderInfo.Address;
				text6 = orderInfo.ExpressCompanyAbb;
			}
			if (!flag)
			{
				base.GotoResourceNotFound("错误的物流数据");
			}
			Literal literal2 = this.FindControl("ltlExpressCompanyName") as Literal;
			Literal literal3 = this.FindControl("ltlShipOrderNumber") as Literal;
			Literal literal4 = this.FindControl("ltlReceiveName") as Literal;
			Literal literal5 = this.FindControl("ltlTel") as Literal;
			Literal literal6 = this.FindControl("ltlShipAddress") as Literal;
			Literal literal7 = this.FindControl("Repeater") as Literal;
			HtmlInputHidden htmlInputHidden2 = this.FindControl("hidOrderId") as HtmlInputHidden;
			htmlInputHidden2.Value = ((orderInfo != null) ? orderInfo.OrderId : string.Empty);
			HtmlInputHidden htmlInputHidden3 = this.FindControl("hidIsShowDadaGIS") as HtmlInputHidden;
			HtmlInputHidden htmlInputHidden4 = this.FindControl("hidUserLatlng") as HtmlInputHidden;
			HtmlInputHidden htmlInputHidden5 = this.FindControl("hidStoreLatlng") as HtmlInputHidden;
			HtmlInputHidden htmlInputHidden6 = this.FindControl("hidBaseURL") as HtmlInputHidden;
			htmlInputHidden6.Value = Globals.HostPath(this.Page.Request.Url);
			if (orderInfo != null)
			{
				if (orderInfo.ExpressCompanyName.ToNullString() == "同城物流配送")
				{
					htmlInputHidden3.Value = "1";
				}
				if (orderInfo.ShippingId > 0)
				{
					ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(orderInfo.ShippingId);
					if (shippingAddress != null && !string.IsNullOrWhiteSpace(shippingAddress.LatLng))
					{
						htmlInputHidden4.Value = shippingAddress.LatLng;
					}
				}
				if (orderInfo.StoreId > 0)
				{
					StoresInfo storeById = StoresHelper.GetStoreById(orderInfo.StoreId);
					if (storeById != null && !string.IsNullOrWhiteSpace(storeById.LatLng))
					{
						htmlInputHidden5.Value = storeById.LatLng;
					}
				}
				else
				{
					ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
					if (defaultOrFirstShipper != null)
					{
						htmlInputHidden5.Value = defaultOrFirstShipper.Latitude + "," + defaultOrFirstShipper.Longitude;
					}
				}
			}
			literal2.Text = text;
			literal3.Text = text2;
			if (!string.IsNullOrEmpty(text2))
			{
				string text7 = HttpContext.Current.Request.Url.ToString().ToLower();
				if (text7.IndexOf("/wapshop/") != -1)
				{
					if (text6.ToLower() == "sf")
					{
						this.hylExpress100Search.NavigateUrl = $"http://www.sf-express.com/mobile/cn/sc/dynamic_function/waybill/waybill_query_info.html?billno={text2}";
						this.hylExpress100Search.Text = "顺丰官网查询>";
					}
					else
					{
						this.hylExpress100Search.NavigateUrl = $"https://m.kuaidi100.com/result.jsp?nu={text2}";
					}
				}
				else if (text6.ToLower() == "sf")
				{
					this.hylExpress100Search.NavigateUrl = $"http://www.sf-express.com/mobile/cn/sc/dynamic_function/waybill/waybill_query_info.html?billno={text2}";
					this.hylExpress100Search.Text = "顺丰官网查询>";
				}
				else
				{
					this.hylExpress100Search.NavigateUrl = $"https://www.kuaidi100.com/chaxun?nu={text2}";
				}
			}
			else
			{
				this.ulExpress.Visible = false;
			}
			literal4.Text = text3;
			literal5.Text = text4;
			literal6.Text = text5;
			PageTitle.AddSiteNameTitle("查看物流");
		}
	}
}
