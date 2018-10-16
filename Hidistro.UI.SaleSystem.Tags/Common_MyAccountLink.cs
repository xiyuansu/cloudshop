using Hidistro.Context;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_MyAccountLink : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.UserId != 0)
			{
				base.Text = "退出";
				base.NavigateUrl = "/logout";
			}
			else
			{
				base.Text = "注册";
				base.NavigateUrl = "/Register";
			}
			base.Render(writer);
		}
	}
}
