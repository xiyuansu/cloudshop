using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Order
{
	[PrivilegeCheck(Privilege.SupplierReplace)]
	public class ReplaceApplyDetail : AdminPage
	{
		private string credentialsImgHtml = "<img src=\"{0}\" style=\"max-height:60px;\" />";

		private int UserStoreId = 0;

		protected HiddenField hidReplaceStatus;

		protected Literal txtAfterSaleId;

		protected Literal txtStatus;

		protected HtmlInputButton btnViewUserLogistic;

		protected HtmlInputButton btnViewMallLogistic;

		protected Repeater listPrducts;

		protected Literal litOrderId;

		protected Literal litRefundReason;

		protected Literal litReturnQuantity;

		protected Literal litRemark;

		protected FormatedMoneyLabel litOrderTotal;

		protected HtmlGenericControl divCredentials;

		protected Literal litImageList;

		protected Literal litUserAddress;

		protected Literal litAdminShipAddrss;

		protected TextBox txtAdminShipAddress;

		protected Literal litAdminShipTo;

		protected TextBox txtAdminShipTo;

		protected Literal litAdminCellPhone;

		protected TextBox txtAdminCellPhone;

		protected TextBox txtAdminRemark;

		protected Button btnAcceptReplace;

		protected Button btnRefuseReplace;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAcceptReplace.Click += this.btnAcceptReplace_Click;
			this.btnRefuseReplace.Click += this.btnRefuseReplace_Click;
			if (!this.Page.IsPostBack)
			{
				this.bindReplaceInfo();
			}
		}

		public void bindReplaceInfo()
		{
			int replaceId = this.Page.Request["replaceId"].ToInt(0);
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo == null)
			{
				this.ShowMsg("换货信息错误!", false);
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(replaceInfo.OrderId);
				HiddenField hiddenField = this.hidReplaceStatus;
				int num = (int)replaceInfo.HandleStatus;
				hiddenField.Value = num.ToString();
				if (orderInfo == null)
				{
					this.ShowMsg("错误的订单信息!", false);
				}
				else
				{
					if (string.IsNullOrEmpty(replaceInfo.SkuId))
					{
						this.listPrducts.DataSource = orderInfo.LineItems.Values;
					}
					else
					{
						Dictionary<string, LineItemInfo> dictionary = new Dictionary<string, LineItemInfo>();
						foreach (LineItemInfo value in orderInfo.LineItems.Values)
						{
							if (value.SkuId == replaceInfo.SkuId)
							{
								dictionary.Add(value.SkuId, value);
							}
						}
						this.listPrducts.DataSource = dictionary.Values;
					}
					this.txtAdminRemark.Text = replaceInfo.AdminRemark;
					this.listPrducts.DataBind();
					this.litOrderId.Text = orderInfo.PayOrderId;
					this.litOrderTotal.Text = orderInfo.GetTotal(false).F2ToString("f2");
					this.litRefundReason.Text = replaceInfo.ReplaceReason;
					this.litRemark.Text = replaceInfo.UserRemark;
					Literal literal = this.litReturnQuantity;
					num = replaceInfo.Quantity;
					literal.Text = num.ToString();
					string userCredentials = replaceInfo.UserCredentials;
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
					if (replaceInfo.HandleStatus == ReplaceStatus.Applied)
					{
						this.btnAcceptReplace.Visible = true;
						this.btnRefuseReplace.Visible = true;
					}
					if (replaceInfo.HandleStatus != 0)
					{
						this.txtAdminCellPhone.Visible = false;
						this.txtAdminShipAddress.Visible = false;
						this.txtAdminShipTo.Visible = false;
						this.litAdminCellPhone.Visible = true;
						this.litAdminShipAddrss.Visible = true;
						this.litAdminShipTo.Visible = true;
						this.litAdminCellPhone.Text = replaceInfo.AdminCellPhone;
						this.litAdminShipTo.Text = replaceInfo.AdminShipTo;
						this.litAdminShipAddrss.Text = replaceInfo.AdminShipAddress;
					}
					else
					{
						ShippersInfo defaultGetGoodsShipperBysupplierId = SalesHelper.GetDefaultGetGoodsShipperBysupplierId(orderInfo.SupplierId);
						if (defaultGetGoodsShipperBysupplierId != null)
						{
							Literal literal2 = this.litAdminShipAddrss;
							TextBox textBox = this.txtAdminShipAddress;
							string text3 = literal2.Text = (textBox.Text = RegionHelper.GetFullRegion(defaultGetGoodsShipperBysupplierId.RegionId, " ", true, 0) + " " + defaultGetGoodsShipperBysupplierId.Address);
							Literal literal3 = this.litAdminShipTo;
							TextBox textBox2 = this.txtAdminShipTo;
							text3 = (literal3.Text = (textBox2.Text = defaultGetGoodsShipperBysupplierId.ShipperName));
							Literal literal4 = this.litAdminCellPhone;
							TextBox textBox3 = this.txtAdminCellPhone;
							text3 = (literal4.Text = (textBox3.Text = defaultGetGoodsShipperBysupplierId.CellPhone));
						}
					}
					if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
					{
						this.btnViewUserLogistic.Visible = true;
						AttributeCollection attributes = this.btnViewUserLogistic.Attributes;
						num = replaceInfo.ReplaceId;
						attributes.Add("replaceid", num.ToString());
					}
					if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery || replaceInfo.HandleStatus == ReplaceStatus.Replaced)
					{
						this.btnViewMallLogistic.Visible = true;
						AttributeCollection attributes2 = this.btnViewMallLogistic.Attributes;
						num = replaceInfo.ReplaceId;
						attributes2.Add("replaceid", num.ToString());
					}
					string str2 = string.IsNullOrEmpty(orderInfo.RealName) ? "" : (orderInfo.RealName.Replace("\n\r", "").Replace("\n", "").Replace("\r", "") + " (" + (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone) + ")");
					str2 = str2 + orderInfo.ShippingRegion + " " + orderInfo.Address;
					this.litUserAddress.Text = str2;
					this.txtStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)replaceInfo.HandleStatus, 0);
					Literal literal5 = this.txtAfterSaleId;
					num = replaceInfo.ReplaceId;
					literal5.Text = num.ToString();
				}
			}
		}

		private void btnRefuseReplace_Click(object sender, EventArgs e)
		{
			int replaceId = this.Page.Request["replaceId"].ToInt(0);
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo == null)
			{
				this.ShowMsg("换货信息错误!", false);
			}
			else
			{
				string text = Globals.StripAllTags(this.txtAdminRemark.Text);
				if (string.IsNullOrEmpty(text))
				{
					this.ShowMsg("请填写换货拒绝原因。", false);
				}
				else
				{
					OrderHelper.CheckReplace(replaceInfo.OrderId, text, false, replaceInfo.SkuId, "", "", "", false);
					this.ShowMsg("成功的拒绝了订单换货", true, HttpContext.Current.Request.Url.ToString());
				}
			}
		}

		private void btnAcceptReplace_Click(object sender, EventArgs e)
		{
			string text = Globals.StripAllTags(this.txtAdminRemark.Text);
			string text2 = Globals.StripAllTags(this.txtAdminShipAddress.Text);
			string adminShipTo = Globals.StripAllTags(this.txtAdminShipTo.Text);
			string adminCellPhone = Globals.StripAllTags(this.txtAdminCellPhone.Text);
			int replaceId = this.Page.Request["replaceId"].ToInt(0);
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo == null)
			{
				this.ShowMsg("换货信息错误!", false);
			}
			else if (this.UserStoreId != replaceInfo.StoreId && replaceInfo.StoreId >= 0)
			{
				this.ShowMsg("换货只能由发货的店铺或者平台进行处理!", false);
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(replaceInfo.OrderId);
				if (orderInfo == null)
				{
					this.ShowMsg("绑定的订单信息不存在!", false);
				}
				else
				{
					if (string.IsNullOrEmpty(text))
					{
						text = replaceInfo.AdminRemark;
					}
					if (string.IsNullOrEmpty(text2))
					{
						this.ShowMsg("请输入收货地址，告之用户发货的地址和联系方式", false);
					}
					else if (OrderHelper.AgreedReplace(replaceInfo.ReplaceId, replaceInfo.OrderId, replaceInfo.SkuId, text, text2, adminShipTo, adminCellPhone, false))
					{
						this.ShowMsg("成功的确认了订单换货", true, HttpContext.Current.Request.Url.ToString());
					}
					else
					{
						this.ShowMsg("订单或者订单项状态错误!", false);
					}
				}
			}
		}
	}
}
