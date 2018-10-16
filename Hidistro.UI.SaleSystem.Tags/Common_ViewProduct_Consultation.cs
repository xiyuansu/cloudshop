using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ViewProduct_Consultation : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(base.Text))
			{
				base.Text = "我要咨询";
			}
			int num = RouteConfig.GetParameter(this.Page, "countDownId", false).ToInt(0);
			int num2 = RouteConfig.GetParameter(this.Page, "groupBuyId", false).ToInt(0);
			int num3 = RouteConfig.GetParameter(this.Page, "PreSaleId", false).ToInt(0);
			if (num > 0)
			{
				CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(num, 0);
				if (countDownInfo != null)
				{
					base.NavigateUrl = base.GetRouteUrl("ProductConsultations", new
					{
						countDownInfo.ProductId
					});
				}
			}
			else if (num2 > 0)
			{
				GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(num2);
				if (groupBuy != null)
				{
					base.NavigateUrl = base.GetRouteUrl("ProductConsultations", new
					{
						groupBuy.ProductId
					});
				}
			}
			else if (num3 > 0)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(num3);
				if (productPreSaleInfo != null)
				{
					base.NavigateUrl = base.GetRouteUrl("ProductConsultations", new
					{
						productPreSaleInfo.ProductId
					});
				}
			}
			else
			{
				base.NavigateUrl = base.GetRouteUrl("ProductConsultations", new
				{
					ProductId = RouteConfig.GetParameter(this.Page, "productId", false)
				});
			}
			base.Render(writer);
		}
	}
}
