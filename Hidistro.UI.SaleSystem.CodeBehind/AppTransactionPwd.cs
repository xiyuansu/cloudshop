using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppTransactionPwd : AppshopMemberTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vTransactionPwd.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("交易密码");
			HtmlInputHidden htmlInputHidden = (HtmlInputHidden)this.FindControl("hidkey");
			Literal literal = (Literal)this.FindControl("OrderId");
			Literal literal2 = (Literal)this.FindControl("litOrderTotal");
			string orderId = literal.Text = this.Page.Request.QueryString["orderId"].ToNullString();
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
			string str = "";
			if (orderInfo == null || !TradeHelper.CheckOrderStockBeforePay(orderInfo, out str))
			{
				base.GotoResourceNotFound(str + ",库存不足，不能进行支付");
			}
			else
			{
				HtmlInputHidden htmlInputHidden2 = (HtmlInputHidden)this.FindControl("txtIsFightGroup");
				if (htmlInputHidden2 != null)
				{
					htmlInputHidden2.Value = ((orderInfo.FightGroupId > 0) ? "true" : "false");
				}
				decimal num = default(decimal);
				if (decimal.TryParse(this.Page.Request.QueryString["totalAmount"], out num))
				{
					literal2.Text = num.F2ToString("f2");
				}
				else
				{
					base.GotoResourceNotFound("传入值有误");
				}
			}
		}
	}
}
