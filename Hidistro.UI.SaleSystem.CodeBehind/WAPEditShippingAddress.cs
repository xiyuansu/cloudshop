using Hidistro.Context;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPEditShippingAddress : WAPMemberTemplatedWebControl
	{
		private RegionSelector dropRegions;

		private HtmlInputText shipTo;

		private HtmlTextArea address;

		private HtmlInputText cellphone;

		private HtmlInputHidden Hiddenshipid;

		private HtmlInputHidden regionText;

		private HtmlInputHidden region;

		private HtmlInputCheckBox chkIsDefault;

		private Common_WAPLocateAddress WapLocateAddress;

		private Common_WAPLocateAddressUpgrade WAPLocateAddressUpgrade;

		private HtmlInputHidden hidIsOpenCertification;

		private int shippingid;

		protected override void OnInit(EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ShippingId"], out this.shippingid))
			{
				this.Page.Response.Redirect("./ShippingAddresses.aspx", true);
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Veditshippingaddress.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.shipTo = (HtmlInputText)this.FindControl("shipTo");
			this.cellphone = (HtmlInputText)this.FindControl("cellphone");
			this.Hiddenshipid = (HtmlInputHidden)this.FindControl("shipId");
			this.WapLocateAddress = (Common_WAPLocateAddress)this.FindControl(Common_WAPLocateAddress.TagId);
			this.WAPLocateAddressUpgrade = (Common_WAPLocateAddressUpgrade)this.FindControl(Common_WAPLocateAddressUpgrade.TagId);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.chkIsDefault = (HtmlInputCheckBox)this.FindControl("chkIsDefault");
			this.hidIsOpenCertification = (HtmlInputHidden)this.FindControl("hidIsOpenCertification");
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(this.shippingid);
			string fullRegion = RegionHelper.GetFullRegion(shippingAddress.RegionId, " ", true, 0);
			if (masterSettings.OpenMultStore)
			{
				this.WapLocateAddress.Visible = false;
				this.WAPLocateAddressUpgrade.Visible = true;
				if (this.WAPLocateAddressUpgrade != null)
				{
					this.WAPLocateAddressUpgrade.RegionId = shippingAddress.RegionId;
					this.WAPLocateAddressUpgrade.Address = shippingAddress.RegionLocation + shippingAddress.Address;
					this.WAPLocateAddressUpgrade.RegionLocation = fullRegion;
					this.WAPLocateAddressUpgrade.BuildingNumber = shippingAddress.BuildingNumber;
					this.WAPLocateAddressUpgrade.LatLng = shippingAddress.LatLng;
				}
			}
			else
			{
				this.WapLocateAddress.Visible = true;
				this.WAPLocateAddressUpgrade.Visible = false;
				if (this.WapLocateAddress != null)
				{
					this.WapLocateAddress.RegionId = shippingAddress.RegionId;
					this.WapLocateAddress.Address = shippingAddress.Address;
					this.WapLocateAddress.BuildingNumber = shippingAddress.BuildingNumber;
				}
			}
			if (this.shipTo != null)
			{
				this.shipTo.Value = shippingAddress.ShipTo;
			}
			if (this.cellphone != null)
			{
				this.cellphone.Value = shippingAddress.CellPhone;
			}
			if (this.Hiddenshipid != null)
			{
				this.Hiddenshipid.Value = this.shippingid.ToString();
			}
			this.chkIsDefault.Checked = shippingAddress.IsDefault;
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["shipTo"]))
			{
				this.shipTo.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["shipTo"]);
			}
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["cellphone"]))
			{
				this.cellphone.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["cellphone"]);
			}
			this.hidIsOpenCertification.Value = (masterSettings.IsOpenCertification ? "下一步" : "保存收货地址");
			PageTitle.AddSiteNameTitle("编辑收货地址");
		}
	}
}
