using Hidistro.Context;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class UserShippingAddresses : MemberTemplatedWebControl
	{
		private TextBox txtShipTo;

		private TextBox txtAddress;

		private TextBox txtZipcode;

		private TextBox txtTelPhone;

		private TextBox txtCellPhone;

		private TextBox txtBuilderNumber;

		private RegionSelector dropRegionsSelect;

		private IButton btnAddAddress;

		private HtmlInputCheckBox chkIsDefault;

		private HtmlInputHidden hidAddressId;

		private Literal lblAddressCount;

		private Common_Address_AddressList dtlstRegionsSelect;

		private static int shippingId = 0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserShippingAddresses.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.txtShipTo = (TextBox)this.FindControl("txtShipTo");
			this.txtAddress = (TextBox)this.FindControl("txtAddress");
			this.txtZipcode = (TextBox)this.FindControl("txtZipcode");
			this.txtTelPhone = (TextBox)this.FindControl("txtTelPhone");
			this.txtCellPhone = (TextBox)this.FindControl("txtCellPhone");
			this.dropRegionsSelect = (RegionSelector)this.FindControl("dropRegions");
			this.btnAddAddress = ButtonManager.Create(this.FindControl("btnAddAddress"));
			this.hidAddressId = (HtmlInputHidden)this.FindControl("hidAddressId");
			this.dtlstRegionsSelect = (Common_Address_AddressList)this.FindControl("list_Common_Consignee_ConsigneeList");
			this.lblAddressCount = (Literal)this.FindControl("lblAddressCount");
			this.chkIsDefault = (HtmlInputCheckBox)this.FindControl("chkIsDefault");
			this.txtBuilderNumber = (TextBox)this.FindControl("txtBuilderNumber");
			this.dtlstRegionsSelect.ItemCommand += this.dtlstRegionsSelect_ItemCommand;
			PageTitle.AddSiteNameTitle("我的收货地址");
			if (!this.Page.IsPostBack)
			{
				this.lblAddressCount.Text = HiContext.Current.SiteSettings.UserAddressMaxCount.ToString();
				this.dropRegionsSelect.DataBind();
				this.Reset();
				this.BindList();
			}
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			this.Reset();
		}

		protected void dtlstRegionsSelect_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			int num = Convert.ToInt32(e.CommandArgument);
			this.ViewState["shippingId"] = num;
			if (e.CommandName == "Edit")
			{
				ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(num);
				if (shippingAddress != null)
				{
					this.hidAddressId.Value = num.ToString();
					this.txtShipTo.Text = shippingAddress.ShipTo;
					this.dropRegionsSelect.SetSelectedRegionId(shippingAddress.RegionId);
					this.txtAddress.Text = shippingAddress.RegionLocation + shippingAddress.Address;
					this.txtTelPhone.Text = shippingAddress.TelPhone;
					this.txtCellPhone.Text = shippingAddress.CellPhone;
					this.chkIsDefault.Checked = shippingAddress.IsDefault;
					this.txtBuilderNumber.Text = shippingAddress.BuildingNumber;
					this.BindList();
				}
				else
				{
					this.hidAddressId.Value = "0";
				}
			}
			if (e.CommandName == "Delete")
			{
				if (MemberProcessor.DelShippingAddress(num, HiContext.Current.UserId))
				{
					this.ShowMessage("成功的删除了你要删除的记录", true, "", 1);
					this.txtShipTo.Text = string.Empty;
					this.txtAddress.Text = string.Empty;
					this.txtTelPhone.Text = string.Empty;
					this.txtCellPhone.Text = string.Empty;
					this.txtBuilderNumber.Text = string.Empty;
					this.BindList();
					this.hidAddressId.Value = "0";
				}
				else
				{
					this.ShowMessage("删除失败", false, "", 1);
				}
				num = 0;
			}
			if (e.CommandName == "SetDefault")
			{
				if (MemberProcessor.SetDefaultShippingAddress(num, HiContext.Current.UserId))
				{
					this.ShowMessage("设置默认收货地址成功", true, "", 1);
					this.txtShipTo.Text = string.Empty;
					this.txtAddress.Text = string.Empty;
					this.txtTelPhone.Text = string.Empty;
					this.txtCellPhone.Text = string.Empty;
					this.txtBuilderNumber.Text = string.Empty;
					this.hidAddressId.Value = "0";
					this.BindList();
				}
				else
				{
					this.ShowMessage("删除失败", false, "", 1);
				}
				num = 0;
			}
		}

		private void BindList()
		{
			IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
			foreach (ShippingAddressInfo item in shippingAddresses)
			{
				item.IDStatus = 0;
				if (HiContext.Current.SiteSettings.IsOpenCertification)
				{
					if (HiContext.Current.SiteSettings.CertificationModel == 1 && string.IsNullOrWhiteSpace(item.IDNumber))
					{
						item.IDStatus = 1;
					}
					if (HiContext.Current.SiteSettings.CertificationModel == 2 && (string.IsNullOrWhiteSpace(item.IDImage1) || string.IsNullOrWhiteSpace(item.IDImage2)))
					{
						item.IDStatus = 1;
					}
				}
			}
			this.dtlstRegionsSelect.DataSource = shippingAddresses;
			this.dtlstRegionsSelect.DataBind();
		}

		private void Reset()
		{
			this.txtShipTo.Text = string.Empty;
			this.dropRegionsSelect.SetSelectedRegionId(null);
			this.txtAddress.Text = string.Empty;
			this.txtTelPhone.Text = string.Empty;
			this.txtCellPhone.Text = string.Empty;
			this.txtBuilderNumber.Text = string.Empty;
			UserShippingAddresses.shippingId = 0;
			this.btnAddAddress.Visible = true;
			this.btnAddAddress.Text = (HiContext.Current.SiteSettings.IsOpenCertification ? "下一步" : "保存");
		}
	}
}
