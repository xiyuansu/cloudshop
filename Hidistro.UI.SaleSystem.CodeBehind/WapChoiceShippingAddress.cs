using Hidistro.Context;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WapChoiceShippingAddress : WAPMemberTemplatedWebControl
	{
		private WapTemplatedRepeater rptvShipping;

		private HtmlAnchor aLinkToAdd;

		private HtmlButton AddAddress;

		private HtmlInputHidden hidIsMultiStore;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-ChoiceShippingAddress.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.rptvShipping = (WapTemplatedRepeater)this.FindControl("rptvShipping");
			this.hidIsMultiStore = (HtmlInputHidden)this.FindControl("hidIsMultiStore");
			IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
			if (shippingAddresses != null)
			{
				this.rptvShipping.DataSource = shippingAddresses;
				this.rptvShipping.DataBind();
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidIsMultiStore.Value = (masterSettings.OpenMultStore ? "1" : "0");
			PageTitle.AddSiteNameTitle("选择收货地址");
		}
	}
}
