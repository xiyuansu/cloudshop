using Hidistro.SaleSystem.Shopping;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ShoppingCart_Info : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write("<span>购物车<em id='spanCartNum'>" + ShoppingCartProcessor.GetCartQuantity() + "</em>件</span>");
		}
	}
}
