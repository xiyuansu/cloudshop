using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.FightGroupManage)]
	public class FightGroupDetails : AdminPage
	{
		public int fightGroupActivityId;

		protected DropDownList ddlStatus;

		protected CalendarPanel CPStartTime;

		protected CalendarPanel CPEndTime;

		public string startDate
		{
			get;
			set;
		}

		public string endDate
		{
			get;
			set;
		}

		public string status
		{
			get;
			set;
		}

		public string orderId
		{
			get;
			set;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.ddlStatus.Items.Add(new ListItem
				{
					Value = "-1",
					Text = "请选择"
				});
				foreach (FightGroupStatus value in Enum.GetValues(typeof(FightGroupStatus)))
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
			this.fightGroupActivityId = this.Page.Request["fightGroupActivityId"].ToInt(0);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["status"]))
			{
				this.status = Globals.UrlDecode(this.Page.Request.QueryString["status"]);
			}
			this.ddlStatus.SelectedValue = this.status;
		}
	}
}
