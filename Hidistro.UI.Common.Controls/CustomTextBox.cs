using System;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class CustomTextBox : TextBox
	{
		public string RegexEnter
		{
			get;
			set;
		}

		public bool UsePageValidatorJS
		{
			get;
			set;
		}

		public bool UseMoneyRangeValidator
		{
			get;
			set;
		}

		public decimal MoneyRangeMinValue
		{
			get;
			set;
		}

		public decimal MoneyRangeMaxValue
		{
			get;
			set;
		}

		public string MessageMoneyRangeError
		{
			get;
			set;
		}

		public int Min
		{
			get;
			set;
		}

		public int Max
		{
			get;
			set;
		}

		public bool AllowEmpty
		{
			get;
			set;
		}

		public string RegexWhenLoseFocus
		{
			get;
			set;
		}

		public string MessageWhenLoseFocus
		{
			get;
			set;
		}

		public CustomTextBox()
		{
			this.CssClass = "forminput form-control";
			this.UsePageValidatorJS = true;
			this.Min = 1;
			this.Max = 100;
			this.MessageWhenLoseFocus = string.Empty;
			this.RegexWhenLoseFocus = string.Empty;
			this.RegexEnter = string.Empty;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.MaxLength = this.Max;
			StringBuilder stringBuilder = new StringBuilder();
			if (this.UsePageValidatorJS)
			{
				stringBuilder.AppendFormat(" initValid(new InputValidator('{0}', {1}, {2}, {3}, '{4}', '{5}'));", this.ClientID, this.Min, this.Max, this.AllowEmpty.ToString().ToLower(), this.RegexWhenLoseFocus, this.MessageWhenLoseFocus);
			}
			if (this.UseMoneyRangeValidator)
			{
				stringBuilder.AppendFormat(" appendValid(new MoneyRangeValidator('{0}', {1}, {2}, '{3}'));", this.ClientID, this.MoneyRangeMinValue, this.MoneyRangeMaxValue, this.MessageMoneyRangeError);
			}
			if (!string.IsNullOrEmpty(this.RegexEnter))
			{
				string value = $"value=value.replace({this.RegexEnter},'')";
				base.Attributes.Add("onkeyup", value);
			}
			string script = "$(function () { " + stringBuilder.ToString() + " });";
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), Guid.NewGuid().ToString(), script, true);
		}
	}
}
