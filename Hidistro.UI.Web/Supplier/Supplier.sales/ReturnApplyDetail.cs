using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier.sales
{
	public class ReturnApplyDetail : SupplierAdminPage
	{
		private string credentialsImgHtml = "<img src=\"{0}\" style=\"max-height:60px;\" />";

		public string AfterSaleType = "退货";

		protected HiddenField hidRefundMaxAmount;

		protected HiddenField hidReturnStatus;

		protected HiddenField hidAfterSaleType;

		protected Literal txtAfterSaleId;

		protected Literal txtStatus;

		protected HtmlInputButton btnViewLogistic;

		protected Repeater listPrducts;

		protected Literal litOrderId;

		protected Literal litRefundReason;

		protected Literal litReturnQuantity;

		protected Literal litRemark;

		protected HtmlGenericControl divCredentials;

		protected Literal litImageList;

		protected Literal litAdminShipAddrss;

		protected Literal litAdminShipTo;

		protected Literal litAdminCellPhone;

		protected Literal liAdminRemark;

		protected Button btnGetGood2;

		protected Button btnGetGoods;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.bindReturnInfo();
			}
			this.btnGetGoods.Click += this.btnGetGoods_Click;
		}

		public void bindReturnInfo()
		{
			int returnId = this.Page.Request["ReturnId"].ToInt(0);
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
			if (returnInfo == null)
			{
				this.ShowMsg("退货信息错误!", false);
			}
			else
			{
				HiddenField hiddenField = this.hidReturnStatus;
				int num = (int)returnInfo.HandleStatus;
				hiddenField.Value = num.ToString();
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMsg("错误的订单信息!", false);
				}
				else if (orderInfo.SupplierId != HiContext.Current.Manager.StoreId)
				{
					this.ShowMsg("订单不是当前供应商订单，请勿非法操作。", false);
				}
				else
				{
					if (string.IsNullOrEmpty(returnInfo.SkuId))
					{
						this.listPrducts.DataSource = orderInfo.LineItems.Values;
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
						this.listPrducts.DataSource = dictionary.Values;
					}
					this.listPrducts.DataBind();
					if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
					{
						this.AfterSaleType = "退款";
					}
					this.litOrderId.Text = orderInfo.OrderId;
					this.litRefundReason.Text = returnInfo.ReturnReason;
					this.litRemark.Text = returnInfo.UserRemark;
					Literal literal = this.litReturnQuantity;
					num = returnInfo.Quantity;
					literal.Text = num.ToString();
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
						this.litImageList.Text = userCredentials;
					}
					else
					{
						this.divCredentials.Visible = false;
					}
					if (returnInfo.HandleStatus == ReturnStatus.Deliverying)
					{
						Button button = this.btnGetGoods;
						Button button2 = this.btnGetGood2;
						bool visible = button2.Visible = true;
						button.Visible = visible;
					}
					if (returnInfo.HandleStatus != 0)
					{
						this.litAdminCellPhone.Text = returnInfo.AdminCellPhone;
						this.litAdminShipTo.Text = returnInfo.AdminShipTo;
						this.litAdminShipAddrss.Text = returnInfo.AdminShipAddress;
					}
					else
					{
						ShippersInfo defaultGetGoodsShipperBysupplierId = SalesHelper.GetDefaultGetGoodsShipperBysupplierId(HiContext.Current.Manager.StoreId);
						if (defaultGetGoodsShipperBysupplierId != null)
						{
							this.litAdminShipAddrss.Text = RegionHelper.GetFullRegion(defaultGetGoodsShipperBysupplierId.RegionId, " ", true, 0) + " " + defaultGetGoodsShipperBysupplierId.Address;
							this.litAdminShipTo.Text = defaultGetGoodsShipperBysupplierId.ShipperName;
							this.litAdminCellPhone.Text = defaultGetGoodsShipperBysupplierId.CellPhone;
						}
					}
					this.liAdminRemark.Text = returnInfo.AdminRemark;
					if (returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund)
					{
						this.txtStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 3);
					}
					else
					{
						this.txtStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)returnInfo.HandleStatus, 0);
					}
					Literal literal2 = this.txtAfterSaleId;
					num = returnInfo.ReturnId;
					literal2.Text = num.ToString();
					HiddenField hiddenField2 = this.hidAfterSaleType;
					num = (int)returnInfo.AfterSaleType;
					hiddenField2.Value = num.ToString();
					this.hidRefundMaxAmount.Value = orderInfo.GetCanRefundAmount(returnInfo.SkuId, null, 0).F2ToString("f2");
					if (returnInfo.AfterSaleType == AfterSaleTypes.ReturnAndRefund && (returnInfo.HandleStatus == ReturnStatus.Deliverying || returnInfo.HandleStatus == ReturnStatus.GetGoods || returnInfo.HandleStatus == ReturnStatus.Returned))
					{
						this.btnViewLogistic.Visible = true;
						AttributeCollection attributes = this.btnViewLogistic.Attributes;
						num = returnInfo.ReturnId;
						attributes.Add("returnsid", num.ToString());
						this.btnViewLogistic.Attributes.Add("expresscompanyname", returnInfo.ExpressCompanyName.ToString());
						this.btnViewLogistic.Attributes.Add("shipordernumber", returnInfo.ShipOrderNumber.ToString());
					}
				}
			}
		}

		private void btnGetGoods_Click(object sender, EventArgs e)
		{
			int returnId = this.Page.Request["ReturnId"].ToInt(0);
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
			if (orderInfo == null)
			{
				this.ShowMsg("订单不存在！", false);
			}
			else if (orderInfo.SupplierId != HiContext.Current.Manager.StoreId)
			{
				this.ShowMsg("订单不是当前供应商订单，请勿非法操作。", false);
			}
			else if (returnInfo == null)
			{
				this.ShowMsg("退货信息错误!", false);
			}
			else if (returnInfo.HandleStatus != ReturnStatus.Deliverying)
			{
				this.ShowMsg("当前状态不允许进行收货!", false);
			}
			else if (TradeHelper.FinishGetGoodsForReturn_Supplier(returnInfo.ReturnId, returnInfo.OrderId, returnInfo.SkuId))
			{
				this.ShowMsg("收货成功，由平台进行退款处理！", true, HttpContext.Current.Request.Url.ToString());
			}
			else
			{
				this.ShowMsg("收货失败！", false);
			}
		}
	}
}
