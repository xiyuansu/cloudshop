using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ContextMenu : WAPTemplatedWebControl
	{
		private Literal litHome;

		private Literal litCategory;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/tags/skin-Common_ContextMenu.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litHome = (Literal)this.FindControl("litHome");
			this.litCategory = (Literal)this.FindControl("litCategory");
			int num = this.Page.Request.QueryString["storeId"].ToInt(0);
			if (num > 0)
			{
				this.litHome.Text = "<a href=\"StoreHome.aspx?storeId=" + num + "&storeSource=4\" class=\"home\"><i class=\"icon-icon_home\"></i>门店</a>";
			}
			else
			{
				this.litHome.Text = "<a href=\"Default.aspx\" class=\"home\"><i class=\"icon-icon_home\"></i>首页</a>";
			}
			this.litCategory.Text = "<a href=\"ProductSearch.aspx?storeId=" + num + "\" class=\"classify\"><i class=\"icon-icon_category-03\"></i>分类</a>";
		}
	}
}
