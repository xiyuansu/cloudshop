using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.store
{
	[PrivilegeCheck(Privilege.DefineTopics)]
	public class WapTopicList : AdminPage
	{
		protected HtmlGenericControl li1;

		protected HtmlGenericControl liH5;

		protected HtmlGenericControl liApp;

		protected Label lblStatus;

		protected DropDownList ddlTopicType;

		protected HtmlGenericControl alink;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.BindTopicType();
		}

		private void BindTopicType()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.ddlTopicType.Items.Add(new ListItem("请选择类型", "0"));
			if (masterSettings.OpenAliho == 1 || masterSettings.OpenWap == 1 || masterSettings.OpenAliho == 1 || masterSettings.OpenVstore == 1)
			{
				this.liH5.Visible = true;
				this.ddlTopicType.Items.Add(new ListItem("微信", "1"));
			}
			else
			{
				this.liH5.Visible = false;
			}
			if (masterSettings.OpenMobbile == 1)
			{
				this.liApp.Visible = true;
				this.ddlTopicType.Items.Add(new ListItem("APP", "2"));
			}
			else
			{
				this.liApp.Visible = false;
			}
			if (masterSettings.OpenPcShop)
			{
				this.ddlTopicType.Items.Add(new ListItem("PC", "3"));
			}
			else
			{
				this.li1.Visible = false;
			}
		}
	}
}
