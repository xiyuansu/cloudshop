using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin.promotion
{
	[PrivilegeCheck(Privilege.CombinationBuy)]
	public class CombinationBuy : AdminPage
	{
		protected string productName = string.Empty;

		protected int status = -1;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			int num = 0;
			if (int.TryParse(this.Page.Request.QueryString["status"], out num))
			{
				this.status = num;
			}
		}
	}
}
