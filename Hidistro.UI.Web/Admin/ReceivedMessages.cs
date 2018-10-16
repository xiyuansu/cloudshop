using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ReceivedMessages)]
	public class ReceivedMessages : AdminPage
	{
		protected int MessageStatus = 1;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(base.Request.QueryString["MessageStatus"]))
			{
				this.MessageStatus = base.Request.QueryString["MessageStatus"].ToInt(0);
			}
		}
	}
}
