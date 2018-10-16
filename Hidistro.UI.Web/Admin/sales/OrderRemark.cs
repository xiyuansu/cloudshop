using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public class OrderRemark : AdminPage
	{
		private string orderId;

		private OrderInfo order;

		protected HtmlInputHidden hidRemarkImage;

		protected Literal spanOrderId;

		protected FormatedTimeLabel lblorderDateForRemark;

		protected FormatedMoneyLabel lblorderTotalForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected TextBox txtRemark;

		protected Button btnModifyRemark;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.orderId = this.Page.Request.QueryString["OrderId"].ToNullString();
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			if (this.order == null)
			{
				this.ShowMsg("订单不存在，或者已被删除。", false);
			}
			else if (!this.Page.IsPostBack)
			{
				this.spanOrderId.Text = this.order.PayOrderId;
				this.lblorderDateForRemark.Time = this.order.OrderDate;
				this.lblorderTotalForRemark.Money = this.order.GetTotal(false);
				this.txtRemark.Text = Globals.HtmlDecode(this.order.ManagerRemark);
				this.orderRemarkImageForRemark.SelectedValue = this.order.ManagerMark;
				if (this.order.ManagerMark.HasValue)
				{
					this.hidRemarkImage.Value = this.order.ManagerMark.Value.ToString();
				}
			}
		}

		protected void btnModifyRemark_Click(object sender, EventArgs e)
		{
			if (this.txtRemark.Text.Length > 300)
			{
				this.ShowMsg("备忘录长度限制在300个字符以内", false);
			}
			else
			{
				if (this.orderRemarkImageForRemark.SelectedItem != null)
				{
					this.order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
				}
				this.order.ManagerRemark = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtRemark.Text));
				if (OrderHelper.SaveRemark(this.order))
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
