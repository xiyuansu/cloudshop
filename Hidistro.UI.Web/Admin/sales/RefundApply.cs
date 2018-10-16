using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	[PrivilegeCheck(Privilege.OrderRefundApply)]
	public class RefundApply : AdminPage
	{
		protected int? HandleStatus;

		protected HiddenField hidStatus;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected HtmlInputHidden hidOrderId;

		protected HtmlInputHidden hidRefundId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			string text = this.Page.Request["HandleStatus"];
			if (!string.IsNullOrWhiteSpace(text))
			{
				this.HandleStatus = text.ToInt(0);
			}
			this.hidStatus.Value = this.HandleStatus.ToString();
		}
	}
}
