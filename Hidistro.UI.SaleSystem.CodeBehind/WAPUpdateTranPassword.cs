using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPUpdateTranPassword : WAPMemberTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-UpdateTranPassword.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("修改交易密码");
			MemberInfo user = Users.GetUser(HiContext.Current.UserId);
			if (user == null)
			{
				if (base.ClientType == ClientType.VShop)
				{
					HttpContext.Current.Response.Redirect("/VShop/MemberCenter.aspx");
				}
				else
				{
					HttpContext.Current.Response.Redirect("Login.aspx");
				}
			}
		}
	}
}
