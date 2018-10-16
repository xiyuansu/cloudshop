using Hidistro.Context;
using Hidistro.SaleSystem;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.ChoicePage
{
	public class CPCountDownStores : AdminPage
	{
		public SiteSettings site = SettingsManager.GetMasterSettings();

		protected RegionSelector dropRegion;

		protected DropDownList ddlTags;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack && this.site.OpenMultStore)
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
