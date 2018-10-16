using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPImageTextDetails : WAPTemplatedWebControl
	{
		private int messageId;

		private HiImage imgUrl;

		private Literal litContent;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VImageTextDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			if (!int.TryParse(this.Page.Request.QueryString["messageId"], out this.messageId))
			{
				base.GotoResourceNotFound("");
			}
			this.imgUrl = (HiImage)this.FindControl("imgUrl");
			this.litContent = (Literal)this.FindControl("litContent");
			MessageInfo message = VshopBrowser.GetMessage(this.messageId);
			if (message == null)
			{
				base.GotoResourceNotFound("");
			}
			this.imgUrl.ImageUrl = message.ImageUrl;
			this.litContent.Text = message.Content;
			PageTitle.AddSiteNameTitle("内容详情");
		}
	}
}
