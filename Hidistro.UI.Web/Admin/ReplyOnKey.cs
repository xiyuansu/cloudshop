using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.VReplyOnKey)]
	public class ReplyOnKey : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
