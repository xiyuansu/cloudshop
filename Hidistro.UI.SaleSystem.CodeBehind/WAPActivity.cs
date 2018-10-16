using Hidistro.Context;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPActivity : WAPMemberTemplatedWebControl
	{
		private HiImage img;

		private Literal litDescription;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vActivity.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			int num = default(int);
			int.TryParse(HttpContext.Current.Request.QueryString.Get("id"), out num);
			MemberInfo user = HiContext.Current.User;
			if (user.UserId == 0)
			{
				HttpContext.Current.Response.Redirect("login.aspx?ReturnUrl=/Vshop/Activity.aspx?id=" + num);
			}
			else
			{
				VActivityInfo activity = VshopBrowser.GetActivity(num);
				if (activity == null)
				{
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				}
				else if (activity.MaxValue <= VshopBrowser.GetUserPrizeCount(num))
				{
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>$(function(){alert_h(\"报名人数已达到限制人数！\",function(){window.location.href=\"/vshop/default.aspx\";});});</script>");
				}
				else
				{
					this.img = (HiImage)this.FindControl("img");
					this.litDescription = (Literal)this.FindControl("litDescription");
					this.img.ImageUrl = activity.PicUrl;
					this.litDescription.Text = activity.Description;
					PageTitle.AddSiteNameTitle("微报名");
				}
			}
		}
	}
}
