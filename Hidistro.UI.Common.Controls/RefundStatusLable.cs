using Hidistro.Entities;
using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RefundStatusLable : Label
	{
		public int Status
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			foreach (RefundStatus value in Enum.GetValues(typeof(RefundStatus)))
			{
				if (value == (RefundStatus)this.Status)
				{
					base.Text = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					break;
				}
			}
			base.Render(writer);
		}
	}
}
