using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.OpenIdServices)]
	public class OpenIdServices : AdminPage
	{
		protected Panel pnlConfigedList;

		protected Repeater grdConfigedItemsNew;

		protected Panel pnlConfigedNote;

		protected Panel pnlEmptyList;

		protected Repeater grdEmptyListNew;

		protected Panel pnlEmptyNote;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			this.BindConfigedList();
			this.BindEmptyList();
		}

		private void BindConfigedList()
		{
			PluginItemCollection configedItems = OpenIdHelper.GetConfigedItems();
			if (configedItems != null && configedItems.Count > 0)
			{
				this.grdConfigedItemsNew.DataSource = configedItems.Items;
				this.grdConfigedItemsNew.DataBind();
				this.pnlConfigedList.Visible = true;
				this.pnlConfigedNote.Visible = false;
			}
			else
			{
				this.pnlConfigedList.Visible = false;
				this.pnlConfigedNote.Visible = true;
			}
		}

		private void BindEmptyList()
		{
			PluginItemCollection emptyItems = OpenIdHelper.GetEmptyItems();
			if (emptyItems != null && emptyItems.Count > 0)
			{
				this.grdEmptyListNew.DataSource = emptyItems.Items;
				this.grdEmptyListNew.DataBind();
				this.pnlEmptyList.Visible = true;
				this.pnlEmptyNote.Visible = false;
			}
			else
			{
				this.pnlEmptyList.Visible = false;
				this.pnlEmptyNote.Visible = true;
			}
		}

		protected void grdConfigedItemsNew_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			HiddenField hiddenField = e.Item.FindControl("hfFullName") as HiddenField;
			string commandName = e.CommandName;
			if (commandName == "Delete")
			{
				string value = hiddenField.Value;
				OpenIdHelper.DeleteSettings(value);
				this.BindData();
			}
		}

		public string GetHelpDoc(object obj)
		{
			return "";
		}

		protected void grdEmptyListNew_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			HiddenField hiddenField = e.Item.FindControl("hfFullName") as HiddenField;
			string commandName = e.CommandName;
			if (commandName == "Delete")
			{
				string value = hiddenField.Value;
				OpenIdHelper.DeleteSettings(value);
				this.BindData();
			}
		}
	}
}
