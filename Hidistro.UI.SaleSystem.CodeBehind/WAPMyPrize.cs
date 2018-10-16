using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPMyPrize : WAPMemberTemplatedWebControl
	{
		private HtmlInputHidden hidstaut;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-MyPrize.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string value = this.Page.Request.QueryString["staut"];
			if (string.IsNullOrWhiteSpace(value))
			{
				value = "1";
			}
			this.hidstaut = (HtmlInputHidden)this.FindControl("hidstaut");
			this.hidstaut.Value = value;
			PageTitle.AddSiteNameTitle("我的奖品");
		}
	}
}
