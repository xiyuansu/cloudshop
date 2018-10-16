using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class Default : HtmlTemplatedWebControl
	{
		protected override void OnInit(EventArgs e)
		{
			int homePageTopicId = HiContext.Current.SiteSettings.HomePageTopicId;
			if (homePageTopicId > 0)
			{
				HiContext.Current.Context.Response.Redirect("/Topics?TopicId=" + HiContext.Current.SiteSettings.HomePageTopicId);
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Default.html";
			}
			base.OnInit(e);
		}

		protected override void OnLoad(EventArgs e)
		{
			this.Page.ClientScript.RegisterStartupScript(base.GetType(), "myscript", "<script>alert_h(\"活动还未开始或者已经结束！\",function(){window.location.href=\"/vshop/default.aspx\";})</script>");
			base.OnLoad(e);
		}

		protected override void AttachChildControls()
		{
			HiContext current = HiContext.Current;
			PageTitle.AddTitle(current.SiteSettings.SiteName + " - " + current.SiteSettings.SiteDescription, HiContext.Current.Context);
		}
	}
}
