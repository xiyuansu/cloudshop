using Hidistro.Context;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppProductDetails : AppshopTemplatedWebControl
	{
		private int productId;

		private Literal litShortDescription;

		private Literal litDescription;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VProductDetails.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litShortDescription = (Literal)this.FindControl("litShortDescription");
			this.litDescription = (Literal)this.FindControl("litDescription");
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound("");
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ProductInfo productDescription = ProductBrowser.GetProductDescription(this.productId);
			if (this.litDescription != null)
			{
				string text = "";
				Regex regex = new Regex("<script[^>]*?>.*?</script>", RegexOptions.IgnoreCase);
				if (!string.IsNullOrWhiteSpace(productDescription.MobbileDescription))
				{
					text = regex.Replace(productDescription.MobbileDescription, "");
				}
				else if (!string.IsNullOrWhiteSpace(productDescription.Description))
				{
					text = regex.Replace(productDescription.Description, "");
				}
				text = text.Replace("src", "data-url");
				text = text.Replace("vurl", "src");
				this.litDescription.Text = text;
			}
		}
	}
}
