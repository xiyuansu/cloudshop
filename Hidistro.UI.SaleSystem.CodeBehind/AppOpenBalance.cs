using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class AppOpenBalance : AppshopMemberTemplatedWebControl
	{
		private HiddenField isOpen;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-OpenBalance.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("开启预付款帐号");
			MemberInfo user = HiContext.Current.User;
			if (user != null)
			{
				HiddenField hiddenField = this.FindControl("IsOpen") as HiddenField;
				if (!this.Page.IsPostBack)
				{
					if (hiddenField != null)
					{
						hiddenField.Value = user.IsOpenBalance.ToString();
					}
					if (!string.IsNullOrWhiteSpace(user.TradePassword))
					{
						this.Page.Response.Redirect(string.Format("/{0}/MyAccountSummary.aspx", "AppShop"));
					}
				}
			}
		}
	}
}
