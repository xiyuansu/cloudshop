using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_UserLogin : AscxTemplatedWebControl
	{
		private Panel pnlLogin;

		private Panel pnlLogout;

		private Literal litMemberGrade;

		private Literal litPoint;

		private Image userPicture;

		private Literal litNum;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Skin-Common_UserLogin.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.pnlLogin = (Panel)this.FindControl("pnlLogin");
			this.pnlLogout = (Panel)this.FindControl("pnlLogout");
			this.litMemberGrade = (Literal)this.FindControl("litMemberGrade");
			this.litPoint = (Literal)this.FindControl("litPoint");
			this.userPicture = (Image)this.FindControl("userPicture");
			this.pnlLogout.Visible = (HiContext.Current.UserId != 0);
			this.pnlLogin.Visible = (HiContext.Current.UserId == 0);
			if (!this.Page.IsPostBack)
			{
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0)
				{
					this.litPoint.Text = user.Points.ToString();
					string text = default(string);
					int num = default(int);
					MemberProcessor.LoadMemberExpandInfo(user.GradeId, user.UserName, out text, out num);
					if (!string.IsNullOrEmpty(user.Picture.ToNullString()))
					{
						this.userPicture.ImageUrl = user.Picture;
					}
					else
					{
						this.userPicture.ImageUrl = HiContext.Current.GetSkinPath() + "/images/users/hyzx_25.jpg";
					}
					this.litMemberGrade.Text = text;
				}
			}
		}
	}
}
