using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class WAPSetInvoice : WAPMemberTemplatedWebControl
	{
		private HtmlInputHidden hidEnableTax;

		private HtmlInputHidden hidEnableETax;

		private HtmlInputHidden hidEnableVATTax;

		private HtmlInputHidden hidTaxRate;

		private HtmlInputHidden hidVATTaxRate;

		private HtmlInputHidden hidVATInvoiceDays;

		private Literal litAfterSaleDays;

		private Literal litInvoiceSendDays;

		private HtmlInputHidden hidInvoiceId;

		private HtmlInputHidden hidIsPersonal;

		private HtmlInputHidden hidInvoiceType;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private HtmlInputHidden hidInvoiceJson;

		private int shipAddressId = 0;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SetInvoice.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidEnableTax = (HtmlInputHidden)this.FindControl("hidEnableTax");
			this.hidEnableETax = (HtmlInputHidden)this.FindControl("hidEnableETax");
			this.hidEnableVATTax = (HtmlInputHidden)this.FindControl("hidEnableVATTax");
			this.hidVATTaxRate = (HtmlInputHidden)this.FindControl("hidVATTaxRate");
			this.hidTaxRate = (HtmlInputHidden)this.FindControl("hidTaxRate");
			this.hidVATInvoiceDays = (HtmlInputHidden)this.FindControl("hidVATInvoiceDays");
			this.litAfterSaleDays = (Literal)this.FindControl("litAfterSaleDays");
			this.litInvoiceSendDays = (Literal)this.FindControl("litInvoiceSendDays");
			this.hidInvoiceId = (HtmlInputHidden)this.FindControl("hidInvoiceId");
			this.hidIsPersonal = (HtmlInputHidden)this.FindControl("hidIsPersonal");
			this.hidInvoiceType = (HtmlInputHidden)this.FindControl("hidInvoiceType");
			this.hidInvoiceJson = (HtmlInputHidden)this.FindControl("hidInvoiceJson");
			HtmlInputHidden htmlInputHidden = this.hidEnableETax;
			bool flag = this.siteSettings.EnableTax;
			htmlInputHidden.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden2 = this.hidEnableTax;
			flag = this.siteSettings.EnableE_Invoice;
			htmlInputHidden2.Value = flag.ToString().ToLower();
			HtmlInputHidden htmlInputHidden3 = this.hidEnableVATTax;
			flag = this.siteSettings.EnableVATInvoice;
			htmlInputHidden3.Value = flag.ToString().ToLower();
			this.hidVATTaxRate.Value = this.siteSettings.VATTaxRate.ToNullString();
			this.hidTaxRate.Value = this.siteSettings.TaxRate.ToNullString();
			this.hidVATInvoiceDays.Value = this.siteSettings.VATInvoiceDays.ToNullString();
			this.litAfterSaleDays.SetWhenIsNotNull(this.siteSettings.EndOrderDays.ToNullString());
			this.litInvoiceSendDays.SetWhenIsNotNull((this.siteSettings.EndOrderDays + this.siteSettings.VATInvoiceDays).ToNullString());
			this.shipAddressId = this.Page.Request["ShipAddressId"].ToInt(0);
			ShippingAddressInfo shippingAddressInfo = null;
			if (this.shipAddressId > 0)
			{
				shippingAddressInfo = MemberProcessor.GetShippingAddress(this.shipAddressId);
			}
			if (shippingAddressInfo == null)
			{
				IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses(false);
				if (shippingAddresses.Count > 0)
				{
					shippingAddressInfo = shippingAddresses[0];
				}
			}
			IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(HiContext.Current.UserId, shippingAddressInfo);
			string value = JsonConvert.SerializeObject(new
			{
				List = from i in userInvoiceDataList
				select new
				{
					i.Id,
					i.InvoiceType,
					i.InvoiceTitle,
					i.InvoiceTaxpayerNumber,
					i.OpenBank,
					i.BankAccount,
					i.ReceiveAddress,
					i.ReceiveEmail,
					i.ReceiveName,
					i.ReceivePhone,
					i.ReceiveRegionId,
					i.ReceiveRegionName,
					i.RegisterAddress,
					i.RegisterTel
				}
			});
			this.hidInvoiceJson.Value = value;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
