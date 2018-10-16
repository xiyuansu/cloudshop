using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.BalanceDrawRequest)]
	public class BalanceDrawRequestOnLine : Page
	{
		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			string text = this.Page.Request.QueryString["Ids"].ToNullString();
			string text2 = this.Page.Request.QueryString["Type"].ToNullString().ToLower();
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				base.Response.Redirect("/admin");
			}
			else
			{
				try
				{
					switch (text2)
					{
					case "balance":
						MemberHelper.OnLineBalanceDrawRequest_Alipay(text);
						break;
					case "splittin":
						MemberHelper.OnLineSplittinDrawRequest_Alipay(text);
						break;
					case "balance4supplier":
						BalanceHelper.OnLineBalanceDrawRequest_Alipay(text);
						break;
					case "balance4store":
						StoreBalanceHelper.OnLineBalanceDrawRequest_Alipay(text);
						break;
					default:
						base.Response.Redirect("/admin");
						break;
					}
				}
				catch
				{
					base.Response.Redirect("/admin");
				}
			}
		}
	}
}
