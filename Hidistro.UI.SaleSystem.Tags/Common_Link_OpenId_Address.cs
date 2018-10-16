using Hidistro.Context;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Link_OpenId_Address : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			int num = 0;
			if (int.TryParse(this.Page.Request.QueryString["buyAmount"], out num) && !string.IsNullOrEmpty(this.Page.Request.QueryString["from"]))
			{
				return;
			}
			if (!string.IsNullOrEmpty(base.ImageUrl))
			{
				if (base.ImageUrl.StartsWith("~"))
				{
					base.ImageUrl = base.ResolveUrl(base.ImageUrl);
				}
				else if (base.ImageUrl.StartsWith("/"))
				{
					base.ImageUrl = HiContext.Current.GetSkinPath() + base.ImageUrl;
				}
				else
				{
					base.ImageUrl = HiContext.Current.GetSkinPath() + "/" + base.ImageUrl;
				}
			}
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.UserId.ToString()];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				base.NavigateUrl = "/OpenID/LogisticsAddress.aspx?alipaytoken=" + httpCookie.Value;
				base.Render(writer);
			}
		}
	}
}
