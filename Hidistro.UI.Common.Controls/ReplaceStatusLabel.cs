using Hidistro.Entities;
using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ReplaceStatusLabel : Label
	{
		public int Status
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			foreach (ReplaceStatus value in Enum.GetValues(typeof(ReplaceStatus)))
			{
				if (value == (ReplaceStatus)this.Status)
				{
					base.Text = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					break;
				}
			}
			base.Render(writer);
		}
	}
}
