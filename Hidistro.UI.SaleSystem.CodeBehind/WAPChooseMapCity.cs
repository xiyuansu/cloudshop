using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPChooseMapCity : WAPTemplatedWebControl
	{
		private HtmlInputHidden hidShippingId;

		private HtmlInputHidden hidShipTo;

		private HtmlInputHidden hidCellphone;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ChooseMapCity.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidShippingId = (HtmlInputHidden)this.FindControl("hidShippingId");
			this.hidShipTo = (HtmlInputHidden)this.FindControl("hidShipTo");
			this.hidCellphone = (HtmlInputHidden)this.FindControl("hidCellphone");
			int num = this.Page.Request.QueryString["ShippingId"].ToInt(0);
			this.hidShippingId.Value = num.ToNullString();
			this.hidShipTo.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["shipTo"]);
			this.hidCellphone.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["cellphone"]);
			PageTitle.AddSiteNameTitle("切换城市");
		}
	}
}
