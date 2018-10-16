using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.vshop
{
	public class ViewRedEnvelope : AdminPage
	{
		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected TextBox txtName;

		protected TextBox txtMaxNumber;

		protected RadioButton rdbTypeRandom;

		protected RadioButton rdbTypeFixed;

		protected HtmlGenericControl random;

		protected TextBox txtMinAmount;

		protected TextBox txtMaxAmount;

		protected HtmlGenericControl one;

		protected TextBox txtAmountFixed;

		protected RadioButton rdbUnlimited;

		protected RadioButton rdbSatisfy;

		protected HtmlGenericControl divSatisfy;

		protected TextBox txtEnableUseMinAmount;

		protected TextBox txtEnableIssueMinAmount;

		protected CalendarPanel txtActiveStartTime;

		protected CalendarPanel txtActiveEndTime;

		protected CalendarPanel txtEffectivePeriodStartTime;

		protected CalendarPanel txtEffectivePeriodEndTime;

		protected TextBox txtShareTitle;

		protected TextBox txtShareDetails;

		protected void Page_Load(object sender, EventArgs e)
		{
			int id = this.Page.Request["RedEnvelopeId"].ToInt(0);
			WeiXinRedEnvelopeInfo weiXinRedEnvelope = WeiXinRedEnvelopeProcessor.GetWeiXinRedEnvelope(id);
			Dictionary<string, object> calendarParameter = this.txtActiveStartTime.CalendarParameter;
			DateTime dateTime = DateTime.Now;
			calendarParameter.Add("startDate", dateTime.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter2 = this.txtActiveEndTime.CalendarParameter;
			dateTime = DateTime.Now;
			calendarParameter2.Add("startDate", dateTime.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter3 = this.txtEffectivePeriodStartTime.CalendarParameter;
			dateTime = DateTime.Now;
			calendarParameter3.Add("startDate", dateTime.ToString("yyyy-MM-dd"));
			Dictionary<string, object> calendarParameter4 = this.txtEffectivePeriodEndTime.CalendarParameter;
			dateTime = DateTime.Now;
			calendarParameter4.Add("startDate", dateTime.ToString("yyyy-MM-dd"));
			if (weiXinRedEnvelope == null)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				CalendarPanel calendarPanel = this.txtActiveEndTime;
				dateTime = weiXinRedEnvelope.ActiveEndTime;
				calendarPanel.Text = dateTime.ToString("yyyy-MM-dd");
				CalendarPanel calendarPanel2 = this.txtActiveStartTime;
				dateTime = weiXinRedEnvelope.ActiveStartTime;
				calendarPanel2.Text = dateTime.ToString("yyyy-MM-dd");
				CalendarPanel calendarPanel3 = this.txtEffectivePeriodEndTime;
				dateTime = weiXinRedEnvelope.EffectivePeriodEndTime;
				calendarPanel3.Text = dateTime.ToString("yyyy-MM-dd");
				CalendarPanel calendarPanel4 = this.txtEffectivePeriodStartTime;
				dateTime = weiXinRedEnvelope.EffectivePeriodStartTime;
				calendarPanel4.Text = dateTime.ToString("yyyy-MM-dd");
				this.txtEnableIssueMinAmount.Text = weiXinRedEnvelope.EnableIssueMinAmount.F2ToString("f2");
				this.txtEnableUseMinAmount.Text = weiXinRedEnvelope.EnableUseMinAmount.F2ToString("f2");
				this.txtMaxAmount.Text = weiXinRedEnvelope.MaxAmount.F2ToString("f2");
				this.txtMaxNumber.Text = weiXinRedEnvelope.MaxNumber.F2ToString("f2");
				this.txtName.Text = weiXinRedEnvelope.Name;
				this.txtShareDetails.Text = weiXinRedEnvelope.ShareDetails;
				this.txtShareTitle.Text = weiXinRedEnvelope.ShareTitle;
				if (weiXinRedEnvelope.EnableUseMinAmount > decimal.Zero)
				{
					this.rdbSatisfy.Checked = true;
					this.rdbUnlimited.Checked = false;
				}
				else
				{
					this.divSatisfy.Visible = false;
					this.rdbSatisfy.Checked = false;
					this.rdbUnlimited.Checked = true;
				}
				if (weiXinRedEnvelope.Type == 1.GetHashCode())
				{
					this.random.Visible = false;
					this.one.Visible = true;
					this.rdbTypeFixed.Checked = true;
					this.rdbTypeRandom.Checked = false;
					this.txtAmountFixed.Text = weiXinRedEnvelope.MinAmount.F2ToString("f2");
				}
				else
				{
					this.random.Visible = true;
					this.one.Visible = false;
					this.rdbTypeFixed.Checked = false;
					this.rdbTypeRandom.Checked = true;
					this.txtMinAmount.Text = weiXinRedEnvelope.MinAmount.F2ToString("f2");
					this.txtMaxAmount.Text = weiXinRedEnvelope.MaxAmount.F2ToString("f2");
				}
			}
		}
	}
}
