using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ProductImageAlbum : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			int num = 0;
			if (!int.TryParse(this.Page.Request.QueryString["productId"].ToNullString(), out num))
			{
				int num2 = 0;
				int.TryParse(this.Page.Request.QueryString["countDownId"].ToNullString(), out num2);
				if (num2 > 0)
				{
					CountDownInfo countDownInfo = PromoteHelper.GetCountDownInfo(num2, 0);
					if (countDownInfo != null)
					{
						num = countDownInfo.ProductId;
					}
				}
				if (num <= 0)
				{
					int num3 = 0;
					int.TryParse(this.Page.Request.QueryString["groupBuyId"].ToNullString(), out num3);
					if (num3 > 0)
					{
						GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(num3);
						if (groupBuy != null)
						{
							num = groupBuy.ProductId;
						}
					}
				}
			}
			if (num > 0)
			{
				base.NavigateUrl = base.GetRouteUrl("ProductImages", new
				{
					ProductId = num
				});
			}
			else
			{
				this.Visible = false;
			}
			base.Render(writer);
		}
	}
}
