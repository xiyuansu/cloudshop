using Hidistro.Core;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	[HiPOSCheck(true)]
	public class HiPOSDetailsList : AdminPage
	{
		protected string deviceId;

		protected string storeId;

		protected string orderId;

		protected int systemStoreId;

		protected bool isSystemOrder;

		protected DateTime? startDate;

		protected DateTime? endDate;

		protected TextBox txtOrderId;

		protected DropDownList ddlPOS;

		protected CheckBox cbxHishopOnly;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindPOS();
			}
		}

		private void BindPOS()
		{
			DataTable storeHiPOSChild = StoresHelper.GetStoreHiPOSChild(this.systemStoreId);
			DataRow dataRow = storeHiPOSChild.NewRow();
			dataRow["Alias"] = "请选择pos机";
			dataRow["HiPOSDeviceId"] = "0";
			storeHiPOSChild.Rows.InsertAt(dataRow, 0);
			this.ddlPOS.DataTextField = "Alias";
			this.ddlPOS.DataValueField = "HiPOSDeviceId";
			this.ddlPOS.DataSource = storeHiPOSChild;
			this.ddlPOS.DataBind();
			this.ddlPOS.SelectedValue = this.deviceId;
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["systemStoreId"]))
				{
					this.systemStoreId = this.Page.Request.QueryString["systemStoreId"].ToInt(0);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["storeId"]))
				{
					this.storeId = base.Server.UrlDecode(this.Page.Request.QueryString["storeId"].ToNullString());
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["deviceId"]))
				{
					this.deviceId = base.Server.UrlDecode(this.Page.Request.QueryString["deviceId"].ToNullString());
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isSystemOrder"]))
				{
					this.isSystemOrder = this.Page.Request.QueryString["isSystemOrder"].ToBool();
					this.cbxHishopOnly.Checked = this.isSystemOrder;
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
				{
					this.startDate = this.Page.Request.QueryString["startDate"].ToDateTime();
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
				{
					this.endDate = this.Page.Request.QueryString["endDate"].ToDateTime();
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
				{
					this.orderId = this.Page.Request.QueryString["orderId"].ToNullString();
					this.txtOrderId.Text = this.orderId;
				}
			}
			else
			{
				this.deviceId = this.Page.Request.QueryString["deviceId"].ToNullString();
				this.storeId = this.Page.Request.QueryString["storeId"].ToNullString();
				this.startDate = this.Page.Request.QueryString["startDate"].ToDateTime();
				this.endDate = this.Page.Request.QueryString["endDate"].ToDateTime();
				this.isSystemOrder = this.Page.Request.QueryString["isSystemOrder"].ToBool();
				this.systemStoreId = this.Page.Request.QueryString["systemStoreId"].ToInt(0);
				this.orderId = this.Page.Request.QueryString["orderId"].ToNullString();
			}
		}
	}
}
