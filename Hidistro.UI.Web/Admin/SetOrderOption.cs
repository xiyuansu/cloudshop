using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SetOrderOption)]
	public class SetOrderOption : AdminPage
	{
		protected TextBox txtCountDownTime;

		protected HtmlGenericControl P2;

		protected TextBox txtCloseOrderDays;

		protected HtmlGenericControl txtCloseOrderDaysTip;

		protected TextBox txtFinishOrderDays;

		protected HtmlGenericControl txtFinishOrderDaysTip;

		protected TextBox txtEndOrderDays;

		protected HtmlGenericControl txtEndOrderDaysTip;

		protected TextBox txtEndOrderDaysEvaluate;

		protected HtmlGenericControl txtEndOrderDaysEvaluateTip;

		protected OnOff OnOffAutoDealRefund;

		protected OnOff OnOffOrderPayToShipper;

		protected OnOff OnOffCertification;

		protected RadioButtonList radCertificationModel;

		protected OnOff OnOffTax;

		protected OnOff OnOffE_Tax;

		protected TextBox txtTaxRate;

		protected HtmlGenericControl txtTaxRateTip;

		protected OnOff OnOffVATTax;

		protected TextBox txtVATTax;

		protected HtmlGenericControl txtVATTaxTip;

		protected TextBox txtVATInvoiceDays;

		protected HtmlGenericControl txtVATInvoiceDaysTip;

		protected OnOff OnOffIsOpenPickeupInStore;

		protected TextBox tbxPickeupInStoreRemark;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.OnOffTax.Parameter.Add("onSwitchChange", "funCheckEnableTax");
			this.OnOffCertification.Parameter.Add("onSwitchChange", "fuCheckCertification");
			this.OnOffE_Tax.Parameter.Add("onSwitchChange", "funCheckEnableTax");
			this.OnOffVATTax.Parameter.Add("onSwitchChange", "funCheckEnableVATTax");
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				TextBox textBox = this.txtCountDownTime;
				int num = masterSettings.CountDownTime;
				textBox.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox2 = this.txtCloseOrderDays;
				num = masterSettings.CloseOrderDays;
				textBox2.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox3 = this.txtFinishOrderDays;
				num = masterSettings.FinishOrderDays;
				textBox3.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox4 = this.txtEndOrderDays;
				num = masterSettings.EndOrderDays;
				textBox4.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox5 = this.txtTaxRate;
				decimal num2 = masterSettings.TaxRate;
				textBox5.Text = num2.ToString(CultureInfo.InvariantCulture);
				this.OnOffOrderPayToShipper.SelectedValue = masterSettings.OrderPayToShipper;
				this.OnOffTax.SelectedValue = masterSettings.EnableTax;
				TextBox textBox6 = this.txtEndOrderDaysEvaluate;
				num = masterSettings.EndOrderDaysEvaluate;
				textBox6.Text = num.ToString(CultureInfo.InvariantCulture);
				this.OnOffCertification.SelectedValue = masterSettings.IsOpenCertification;
				RadioButtonList radioButtonList = this.radCertificationModel;
				num = masterSettings.CertificationModel;
				radioButtonList.SelectedValue = num.ToString();
				this.tbxPickeupInStoreRemark.Text = masterSettings.PickeupInStoreRemark;
				this.OnOffIsOpenPickeupInStore.SelectedValue = masterSettings.IsOpenPickeupInStore;
				this.OnOffAutoDealRefund.SelectedValue = masterSettings.IsAutoDealRefund;
				this.OnOffVATTax.SelectedValue = masterSettings.EnableVATInvoice;
				this.OnOffE_Tax.SelectedValue = masterSettings.EnableE_Invoice;
				TextBox textBox7 = this.txtVATTax;
				num2 = masterSettings.VATTaxRate;
				textBox7.Text = num2.ToString(CultureInfo.InvariantCulture);
				TextBox textBox8 = this.txtVATInvoiceDays;
				num = masterSettings.VATInvoiceDays;
				textBox8.Text = num.ToString();
			}
			this.btnOK.Click += this.btnOK_Click;
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			int closeOrderDays = default(int);
			int finishOrderDays = default(int);
			int endOrderDays = default(int);
			decimal taxRate = default(decimal);
			int countDownTime = default(int);
			int endOrderDaysEvaluate = default(int);
			if (this.ValidateValues(out closeOrderDays, out finishOrderDays, out endOrderDays, out taxRate, out countDownTime, out endOrderDaysEvaluate))
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.CountDownTime = countDownTime;
				masterSettings.CloseOrderDays = closeOrderDays;
				masterSettings.FinishOrderDays = finishOrderDays;
				masterSettings.EndOrderDays = endOrderDays;
				masterSettings.TaxRate = taxRate;
				masterSettings.OrderPayToShipper = this.OnOffOrderPayToShipper.SelectedValue;
				masterSettings.EnableTax = this.OnOffTax.SelectedValue;
				masterSettings.EndOrderDaysEvaluate = endOrderDaysEvaluate;
				masterSettings.IsOpenCertification = this.OnOffCertification.SelectedValue;
				masterSettings.CertificationModel = this.radCertificationModel.SelectedValue.ToInt(0);
				masterSettings.PickeupInStoreRemark = this.tbxPickeupInStoreRemark.Text;
				masterSettings.IsOpenPickeupInStore = this.OnOffIsOpenPickeupInStore.SelectedValue;
				masterSettings.IsAutoDealRefund = this.OnOffAutoDealRefund.SelectedValue;
				masterSettings.EnableVATInvoice = this.OnOffVATTax.SelectedValue;
				masterSettings.EnableE_Invoice = this.OnOffE_Tax.SelectedValue;
				masterSettings.VATTaxRate = this.txtVATTax.Text.ToDecimal(0);
				masterSettings.VATInvoiceDays = this.txtVATInvoiceDays.Text.ToInt(0);
				if (this.ValidationSettings(masterSettings))
				{
					Globals.EntityCoding(masterSettings, true);
					SettingsManager.Save(masterSettings);
					this.ShowMsg("保存成功", true);
				}
			}
		}

		private bool ValidationSettings(SiteSettings setting)
		{
			ValidationResults validationResults = Validation.Validate(setting, "ValMasterSettings");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}

		private bool ValidateValues(out int closeOrderDays, out int finishOrderDays, out int endOrderDays, out decimal taxRate, out int countDownTime, out int EndOrderDaysEvaluate)
		{
			string text = string.Empty;
			if (!int.TryParse(this.txtCloseOrderDays.Text, out closeOrderDays))
			{
				text += Formatter.FormatErrorMessage("过期几天自动关闭订单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtFinishOrderDays.Text, out finishOrderDays))
			{
				text += Formatter.FormatErrorMessage("发货几天自动完成订单不能为空,必须为正整数,范围在1-90之间");
			}
			if (!int.TryParse(this.txtEndOrderDays.Text, out endOrderDays))
			{
				text += Formatter.FormatErrorMessage("订单完成几天后，系统自动结束交易，不得再申请退换货服务,必须为正整数,范围在1-90之间");
			}
			taxRate = default(decimal);
			if ((this.OnOffTax.SelectedValue || this.OnOffE_Tax.SelectedValue) && !decimal.TryParse(this.txtTaxRate.Text, out taxRate))
			{
				text += Formatter.FormatErrorMessage("发票税率不能为空,为非负数字,范围在0-100之间");
			}
			if (this.OnOffVATTax.SelectedValue)
			{
				decimal num = default(decimal);
				if (!decimal.TryParse(this.txtVATTax.Text, out num))
				{
					text += Formatter.FormatErrorMessage("专票发票税率不能为空,为非负数字,范围在0-100之间");
				}
				decimal num2 = default(decimal);
				if (!decimal.TryParse(this.txtVATInvoiceDays.Text, out num2))
				{
					text += Formatter.FormatErrorMessage("可以开具发票的天数不能为空,为非负数字,范围在0-100之间");
				}
			}
			if (!int.TryParse(this.txtCountDownTime.Text, out countDownTime))
			{
				text += Formatter.FormatErrorMessage("限时抢购订单有效时间不能为空,为非负数字,范围在1-60之间");
			}
			if (!int.TryParse(this.txtEndOrderDaysEvaluate.Text, out EndOrderDaysEvaluate))
			{
				text += Formatter.FormatErrorMessage("订单完成几天后，系统自动好评时间不能为空,为非负数字,范围在1-60之间");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
