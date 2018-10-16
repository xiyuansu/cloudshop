using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class APPPointMall : AppshopMemberTemplatedWebControl
	{
		private Literal litCurrentPoints;

		private Literal litUserName;

		private HtmlImage imgUser;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-PointMall.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litCurrentPoints = (Literal)this.FindControl("litCurrentPoints");
			this.litUserName = (Literal)this.FindControl("litUserName");
			this.imgUser = (HtmlImage)this.FindControl("imgUser");
			MemberInfo user = HiContext.Current.User;
			if (user == null)
			{
				base.GotoResourceNotFound("请先登录");
			}
			this.litCurrentPoints.Text = user.Points.ToString();
			this.litUserName.Text = user.UserName;
			if (!string.IsNullOrEmpty(user.Picture))
			{
				this.imgUser.Src = user.Picture;
			}
			PageTitle.AddSiteNameTitle("积分商城");
		}
	}
}
