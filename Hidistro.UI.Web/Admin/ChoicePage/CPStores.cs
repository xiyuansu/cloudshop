using Hidistro.Context;
using Hidistro.SaleSystem;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.ChoicePage
{
	public class CPStores : AdminPage
	{
		protected RegionSelector dropRegion;

		protected DropDownList ddlTags;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack && SettingsManager.GetMasterSettings().OpenMultStore && string.IsNullOrEmpty(base.Request.QueryString["isWholesale"]))
			{
				this.ddlTags.DataSource = StoreTagHelper.GetTagList();
				this.ddlTags.DataBind();
				this.ddlTags.Items.Insert(0, new ListItem
				{
					Text = "请选择",
					Value = "0"
				});
			}
		}
	}
}
