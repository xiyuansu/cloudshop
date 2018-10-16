using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class MakeTaobao : AdminPage
	{
		protected HyperLink hlinkToTaobao;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.Response.Redirect($"http://order2.kuaidiangtong.com/TaoBaoApi.aspx?Host={HiContext.Current.SiteUrl}&ApplicationPath={string.Empty}");
		}
	}
}
