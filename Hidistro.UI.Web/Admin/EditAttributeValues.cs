using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class EditAttributeValues : AdminPage
	{
		private int attributeId;

		private int typeId;

		protected TextBox txtValue;

		protected TextBox txtOldValue;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["AttributeId"], out this.attributeId))
			{
				base.GotoResourceNotFound();
			}
			else if (!int.TryParse(this.Page.Request.QueryString["TypeId"], out this.typeId))
			{
				base.GotoResourceNotFound();
			}
		}
	}
}
