using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Gifts)]
	public class Gifts : AdminPage
	{
		protected HtmlInputCheckBox chkPromotion;

		protected PageSizeDropDownList PageSizeDropDownList;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(base.Request.QueryString["flag"]) && base.Request.QueryString["flag"] == "1")
				{
					this.ShowMsg("成功的添加了一件礼品", true);
				}
				if (!string.IsNullOrEmpty(base.Request.QueryString["flag"]) && base.Request.QueryString["flag"] == "2")
				{
					this.ShowMsg("成功的编辑了一件礼品", true);
				}
			}
		}

		private void LoadParameters()
		{
		}
	}
}
