using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class MemberDetails : AdminPage
	{
		protected DropDownList ManagerList;

		protected CalendarPanel startDate;

		protected CalendarPanel endDate;

		protected HtmlInputHidden butstoreId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.BindingData();
		}

		private void BindingData()
		{
			string s = base.Request.QueryString["storeId"];
			string s2 = base.Request.QueryString["managerId"];
			int num = 0;
			int storeId = 0;
			int.TryParse(s, out storeId);
			int.TryParse(s2, out num);
			IList<ManagerInfo> managerIdAndNameRoleId = ManagerHelper.GetManagerIdAndNameRoleId(storeId);
			managerIdAndNameRoleId.Insert(0, new ManagerInfo
			{
				ManagerId = 0,
				UserName = "全部"
			});
			this.ManagerList.DataSource = managerIdAndNameRoleId;
			this.ManagerList.DataTextField = "UserName";
			this.ManagerList.DataValueField = "ManagerId";
			this.ManagerList.SelectedValue = num.ToString();
			this.ManagerList.DataBind();
		}
	}
}
