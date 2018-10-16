using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web
{
	public class ResourceNotFound : Page
	{
		protected Literal litMsg;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.litMsg.Text = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["errorMsg"]));
		}
	}
}
