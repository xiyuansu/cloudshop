using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class AccountSummaryMoneyColumncs : Literal
	{
		private object income;

		private object expenses;

		public object Income
		{
			get
			{
				return this.income;
			}
			set
			{
				this.income = value;
			}
		}

		public object Expenses
		{
			get
			{
				return this.expenses;
			}
			set
			{
				this.expenses = value;
			}
		}

		public override void DataBind()
		{
			this.Income = DataBinder.Eval(this.Page.GetDataItem(), "Income");
			this.Expenses = DataBinder.Eval(this.Page.GetDataItem(), "Expenses");
		}

		protected override void Render(HtmlTextWriter writer)
		{
			decimal num = (this.Income != null && this.Income != DBNull.Value) ? this.Income.ToDecimal(0) : decimal.MinusOne;
			decimal num2 = (this.Expenses != null && this.Expenses != DBNull.Value) ? this.Expenses.ToDecimal(0) : decimal.MinusOne;
			if (num > decimal.Zero)
			{
				base.Text = "<span style=\"color:green;\">+" + Globals.FormatMoney(num) + "</span>";
			}
			else if (num2 > decimal.Zero)
			{
				base.Text = "<span style=\"color:red;\">-" + Globals.FormatMoney(num2) + "</span>";
			}
			else
			{
				base.Text = "<span>" + Globals.FormatMoney(decimal.Zero) + "</span>";
			}
			base.Render(writer);
		}
	}
}
