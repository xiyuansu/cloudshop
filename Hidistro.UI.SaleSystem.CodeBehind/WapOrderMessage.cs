using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WapOrderMessage : WAPMemberTemplatedWebControl
	{
		private HtmlGenericControl txtTitle;

		private HtmlGenericControl txtContent;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyLogistics.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string innerText = (HttpContext.Current.Request["Title"] == null) ? "" : Globals.UrlDecode(Globals.StripAllTags(HttpContext.Current.Request["Title"]));
			string innerHtml = (HttpContext.Current.Request["Content"] == null) ? "" : Globals.UrlDecode(Globals.StripAllTags(HttpContext.Current.Request["Content"]));
			this.txtTitle = (HtmlGenericControl)this.FindControl("txtTitle");
			this.txtContent = (HtmlGenericControl)this.FindControl("txtContent");
			if (this.txtTitle != null)
			{
				this.txtTitle.InnerText = innerText;
			}
			if (this.txtContent != null)
			{
				this.txtContent.InnerHtml = innerHtml;
			}
			PageTitle.AddSiteNameTitle("订单信息");
		}
	}
}
