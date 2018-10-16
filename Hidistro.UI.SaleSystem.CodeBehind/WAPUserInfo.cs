using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPUserInfo : WAPMemberTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VUserInfo.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			HtmlInputText control = (HtmlInputText)this.FindControl("txtUserName");
			HtmlInputText control2 = (HtmlInputText)this.FindControl("txtRealName");
			HtmlInputText control3 = (HtmlInputText)this.FindControl("txtPhone");
			HtmlInputText control4 = (HtmlInputText)this.FindControl("txtEmail");
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				control.SetWhenIsNotNull(user.UserName);
				control2.SetWhenIsNotNull(user.RealName);
				control3.SetWhenIsNotNull(user.CellPhone);
				control4.SetWhenIsNotNull(user.QQ);
			}
			PageTitle.AddSiteNameTitle("修改用户信息");
		}
	}
}
