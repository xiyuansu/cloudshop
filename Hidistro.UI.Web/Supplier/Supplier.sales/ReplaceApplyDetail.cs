using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Messages;
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
	public class ReplaceApplyDetail : SupplierAdminPage
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

		protected HtmlGenericControl divCredentials;

		protected Literal litImageList;

		protected Literal litUserAddress;

		protected ExpressDropDownList expressDropDownList;

		protected TextBox txtShipOrderNumber;

		protected Literal litAdminRemark;

		protected Button btnGetAndSendGoods;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnGetAndSendGoods.Click += this.btnGetAndSendGoods_Click;
			if (!this.Page.IsPostBack)
			{
				this.expressDropDownList.DataBind();
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
				else if (orderInfo.SupplierId != HiContext.Current.Manager.StoreId)
				{
					this.ShowMsg("订单不是当前供应商订单，请勿非法访问。", false);
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
					this.listPrducts.DataBind();
					this.litAdminRemark.Text = replaceInfo.AdminRemark;
					this.litOrderId.Text = orderInfo.OrderId;
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
					if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
					{
						this.btnGetAndSendGoods.Visible = true;
					}
					if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
					{
						this.btnViewUserLogistic.Visible = true;
						AttributeCollection attributes = this.btnViewUserLogistic.Attributes;
						num = replaceInfo.ReplaceId;
						attributes.Add("replaceid", num.ToString());
						this.btnViewUserLogistic.Attributes.Add("expresscompanyname", replaceInfo.UserExpressCompanyName.ToString());
						this.btnViewUserLogistic.Attributes.Add("shipordernumber", replaceInfo.UserShipOrderNumber.ToString());
					}
					if (replaceInfo.HandleStatus == ReplaceStatus.MerchantsDelivery || replaceInfo.HandleStatus == ReplaceStatus.Replaced)
					{
						this.btnViewMallLogistic.Visible = true;
						AttributeCollection attributes2 = this.btnViewMallLogistic.Attributes;
						num = replaceInfo.ReplaceId;
						attributes2.Add("replaceid", num.ToString());
						this.btnViewMallLogistic.Attributes.Add("expresscompanyname", replaceInfo.ExpressCompanyName.ToString());
						this.btnViewMallLogistic.Attributes.Add("shipordernumber", replaceInfo.ShipOrderNumber.ToString());
					}
					string str2 = string.IsNullOrEmpty(orderInfo.RealName) ? "" : (orderInfo.RealName.Replace("\n\r", "").Replace("\n", "").Replace("\r", "") + " (" + (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone) + ")");
					str2 = str2 + orderInfo.ShippingRegion + " " + orderInfo.Address;
					this.litUserAddress.Text = str2;
					this.txtStatus.Text = EnumDescription.GetEnumDescription((Enum)(object)replaceInfo.HandleStatus, 0);
					Literal literal2 = this.txtAfterSaleId;
					num = replaceInfo.ReplaceId;
					literal2.Text = num.ToString();
				}
			}
		}

		private void btnGetAndSendGoods_Click(object sender, EventArgs e)
		{
			int replaceId = this.Page.Request["replaceId"].ToInt(0);
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo == null)
			{
				this.ShowMsg("错误的换货信息", false);
			}
			else
			{
				string arg = "自己门店";
				if (this.UserStoreId == 0)
				{
					arg = "平台";
				}
				if (replaceInfo.StoreId != this.UserStoreId && replaceInfo.StoreId >= 0)
				{
					this.ShowMsg(string.Format("只能由发货的店铺或者平台处理", arg), false);
				}
				else
				{
					string text = "";
					string text2 = "";
					ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(this.expressDropDownList.SelectedValue);
					if (expressCompanyInfo != null)
					{
						text = expressCompanyInfo.Kuaidi100Code;
						text2 = expressCompanyInfo.Name;
						string text3 = this.txtShipOrderNumber.Text;
						if (string.IsNullOrEmpty(text3) || text3.Length > 20)
						{
							this.ShowMsg("请输入运单编号，长度在1-20位之间", false);
						}
						else if (TradeHelper.ReplaceShopSendGoods(replaceId, text, text2, text3, replaceInfo.OrderId, replaceInfo.SkuId))
						{
							if (text.ToUpper() == "HTKY")
							{
								ExpressHelper.GetDataByKuaidi100(text, text3);
							}
							OrderInfo orderInfo = OrderHelper.GetOrderInfo(replaceInfo.OrderId);
							MemberInfo user = Users.GetUser(orderInfo.UserId);
							replaceInfo.HandleStatus = ReplaceStatus.MerchantsDelivery;
							Messenger.AfterSaleDeal(user, orderInfo, null, replaceInfo);
							this.ShowMsg("换货发货成功！", true, HttpContext.Current.Request.Url.ToString());
						}
						else
						{
							this.ShowMsg("换货发货失败，原因未知", false);
						}
					}
					else
					{
						this.ShowMsg("请选择快递公司", false);
					}
				}
			}
		}
	}
}
