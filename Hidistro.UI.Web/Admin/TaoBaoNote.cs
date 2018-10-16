using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SyncTaobao)]
	public class TaoBaoNote : AdminPage
	{
		protected HyperLink hlinkToTaobao;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.hlinkToTaobao.NavigateUrl = $"http://order2.kuaidiangtong.com/TaoBaoApi.aspx?Host={HiContext.Current.SiteUrl}&ApplicationPath={string.Empty}";
		}
	}
}
