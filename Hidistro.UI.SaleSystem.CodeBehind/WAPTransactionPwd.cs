using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPTransactionPwd : WAPTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VTransactionPwd.html";
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
			if (orderInfo == null)
			{
				base.GotoResourceNotFound("数据错误！");
			}
			else
			{
				if (orderInfo.PreSaleId > 0)
				{
					ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(orderInfo.PreSaleId);
					if (productPreSaleInfo == null)
					{
						base.GotoResourceNotFound("预售活动不存在不能支付");
						return;
					}
					if (!orderInfo.DepositDate.HasValue && productPreSaleInfo.PreSaleEndDate < DateTime.Now)
					{
						base.GotoResourceNotFound("您支付晚了，预售活动已经结束");
						return;
					}
					if (orderInfo.DepositDate.HasValue && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						if (productPreSaleInfo.PaymentStartDate > DateTime.Now)
						{
							base.GotoResourceNotFound("尾款支付尚未开始");
							return;
						}
						DateTime t = productPreSaleInfo.PaymentEndDate.AddDays(1.0);
						if (t <= DateTime.Now)
						{
							base.GotoResourceNotFound("尾款支付已结束");
							return;
						}
					}
				}
				else if (!TradeHelper.CheckOrderStockBeforePay(orderInfo, out str))
				{
					base.GotoResourceNotFound(str + ",库存不足，不能进行支付");
					return;
				}
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
