using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppUpdateTranPassword : AppshopMemberTemplatedWebControl
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
			if (user != null)
			{
				return;
			}
		}
	}
}
