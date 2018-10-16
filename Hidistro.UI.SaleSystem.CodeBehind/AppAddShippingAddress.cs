using Hidistro.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class AppAddShippingAddress : AppshopMemberTemplatedWebControl
	{
		private Common_WAPLocateAddress WAPLocateAddress;

		private Common_WAPLocateAddressUpgrade WAPLocateAddressUpgrade;

		private HtmlInputText shipTo;

		private HtmlInputText cellphone;

		private HtmlInputHidden hidIsOpenCertification;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Vaddshippingaddress.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.WAPLocateAddress = (Common_WAPLocateAddress)this.FindControl(Common_WAPLocateAddress.TagId);
			this.WAPLocateAddressUpgrade = (Common_WAPLocateAddressUpgrade)this.FindControl(Common_WAPLocateAddressUpgrade.TagId);
			this.shipTo = (HtmlInputText)this.FindControl("shipTo");
			this.cellphone = (HtmlInputText)this.FindControl("cellphone");
			this.hidIsOpenCertification = (HtmlInputHidden)this.FindControl("hidIsOpenCertification");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.OpenMultStore)
			{
				this.WAPLocateAddress.Visible = false;
				this.WAPLocateAddressUpgrade.Visible = true;
			}
			else
			{
				this.WAPLocateAddress.Visible = true;
				this.WAPLocateAddressUpgrade.Visible = false;
			}
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["shipTo"]))
			{
				this.shipTo.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["shipTo"]);
			}
			if (!string.IsNullOrWhiteSpace(this.Page.Request.QueryString["cellphone"]))
			{
				this.cellphone.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["cellphone"]);
			}
			this.hidIsOpenCertification.Value = (masterSettings.IsOpenCertification ? "下一步" : "保存收货地址");
			PageTitle.AddSiteNameTitle("添加收货地址");
		}
	}
}
