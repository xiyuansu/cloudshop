using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	public class EditSpecificationValues : AdminPage
	{
		private int attributeId;

		protected HtmlInputHidden currentAttributeId;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["AttributeId"], out this.attributeId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.currentAttributeId.Value = this.attributeId.ToString();
			}
		}
	}
}
