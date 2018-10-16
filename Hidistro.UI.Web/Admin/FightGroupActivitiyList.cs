using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.FightGroupManage)]
	public class FightGroupActivitiyList : AdminPage
	{
		public string status;

		protected DropDownList ddlStatus;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				foreach (EnumFightGroupActivitiyStatus value in Enum.GetValues(typeof(EnumFightGroupActivitiyStatus)))
				{
					ListItemCollection items = this.ddlStatus.Items;
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					int num = (int)value;
					items.Add(new ListItem(enumDescription, num.ToString()));
				}
			}
			this.LoadParameters();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["status"]))
			{
				this.status = Globals.UrlDecode(this.Page.Request.QueryString["status"]);
			}
			this.ddlStatus.SelectedValue = this.status;
		}
	}
}
