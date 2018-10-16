using Hidistro.Context;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class Order_ShippingAddress : UserControl
	{
		private SiteSettings setting;

		protected string edit = "";

		private OrderInfo order;

		protected ExpressDropDownList expressRadioButtonList;

		protected TextBox txtpost;

		protected HtmlInputHidden txt_expressCompanyName;

		protected HtmlInputHidden OrderId;

		protected Button btnupdatepost;

		protected HtmlInputHidden hdtagId;

		protected Panel plExpress;

		public bool IsStoreAdminView
		{
			get;
			set;
		}

		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			this.setting = SettingsManager.GetMasterSettings();
			if (!this.Page.IsPostBack)
			{
				this.LoadControl();
			}
			if ((this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(this.order.ExpressCompanyAbb) && this.plExpress != null)
			{
				this.plExpress.Visible = true;
			}
			this.btnupdatepost.Click += this.btnupdatepost_Click;
		}

		private void btnupdatepost_Click(object sender, EventArgs e)
		{
			string value = this.txt_expressCompanyName.Value;
			this.order.ShipOrderNumber = this.txtpost.Text.Trim();
			this.order.ExpressCompanyName = value;
			string arg = "";
			ExpressCompanyInfo expressCompanyInfo = null;
			if (!"".Equals(value))
			{
				expressCompanyInfo = ExpressHelper.FindNode(value);
				if (expressCompanyInfo != null)
				{
					this.order.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
					this.order.ExpressCompanyName = expressCompanyInfo.Name;
				}
				if (!string.IsNullOrEmpty(this.order.OuterOrderId) && !string.IsNullOrEmpty(this.order.ShipOrderNumber) && this.order.OuterOrderId.StartsWith("jd_") && string.IsNullOrWhiteSpace(expressCompanyInfo.JDCode))
				{
					this.ShowMsg("此订单是京东订单，所选物流公司不被京东支持", false);
					return;
				}
				if (this.order.ExpressCompanyAbb != null && this.order.ExpressCompanyAbb.ToUpper() == "HTKY")
				{
					ExpressHelper.GetDataByKuaidi100(this.order.ExpressCompanyAbb, this.order.ShipOrderNumber);
				}
				if (this.order.Gateway != null && this.order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
				{
					ProductStatisticsHelper.UpdateOrderSaleStatistics(this.order);
					TransactionAnalysisHelper.AnalysisOrderTranData(this.order);
				}
				if (!string.IsNullOrEmpty(this.order.OuterOrderId))
				{
					if (this.order.OuterOrderId.StartsWith("tb_"))
					{
						string text = this.order.OuterOrderId.Replace("tb_", "");
						try
						{
							if (expressCompanyInfo != null)
							{
								string requestUriString = $"http://order2.kuaidiangtong.com/UpdateShipping.ashx?tid={text}&companycode={expressCompanyInfo.TaobaoCode}&outsid={this.order.ShipOrderNumber}&Host={HiContext.Current.SiteUrl}";
								WebRequest webRequest = WebRequest.Create(requestUriString);
								webRequest.GetResponse();
							}
						}
						catch
						{
						}
					}
					else if (this.order.OuterOrderId.StartsWith("jd_"))
					{
						string text = this.order.OuterOrderId.Replace("jd_", "");
						try
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings();
							JDHelper.JDOrderOutStorage(masterSettings.JDAppKey, masterSettings.JDAppSecret, masterSettings.JDAccessToken, expressCompanyInfo.JDCode, this.order.ShipOrderNumber, text);
						}
						catch (Exception ex)
						{
							arg = $"\r\n同步京东发货失败，京东订单号：{text}，{ex.Message}\r\n";
						}
					}
				}
			}
			string expresssCompanyAbb = string.IsNullOrEmpty(this.order.ExpressCompanyAbb) ? "" : this.order.ExpressCompanyAbb;
			OrderHelper.SetOrderShipNumber(this.order.OrderId, expresssCompanyAbb, this.order.ExpressCompanyName, this.order.ShipOrderNumber);
			this.ShowMsg($"修改发货单号成功{arg}", true);
			this.LoadControl();
		}

		protected virtual void ShowMsg(string msg, bool success)
		{
			string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
			}
		}

		public void LoadControl()
		{
			if (this.order.OrderStatus == OrderStatus.Finished || this.order.OrderStatus == OrderStatus.SellerAlreadySent)
			{
				this.txtpost.Text = this.order.ShipOrderNumber;
				this.OrderId.Value = this.order.OrderId;
			}
			this.expressRadioButtonList.DataBind();
			this.expressRadioButtonList.SelectedValue = this.order.ExpressCompanyName;
		}
	}
}
