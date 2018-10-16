using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class PaymentDropDownList : DropDownList
	{
		private bool allowNull = true;

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return null;
				}
				return int.Parse(base.SelectedValue);
			}
			set
			{
				if (!value.HasValue)
				{
					base.SelectedValue = string.Empty;
				}
				else
				{
					base.SelectedValue = value.ToString();
				}
			}
		}

		public override void DataBind()
		{
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(string.Empty, string.Empty));
			}
			IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnAll);
			foreach (PaymentModeInfo item in paymentModes)
			{
				base.Items.Add(new ListItem(Globals.HtmlDecode(item.Name), item.ModeId.ToString()));
			}
		}
	}
}
