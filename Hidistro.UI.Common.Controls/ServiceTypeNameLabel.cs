using Hidistro.Entities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ServiceTypeNameLabel : Label
	{
		public object ServiceType
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			int num = 0;
			if (this.ServiceType != DBNull.Value && this.ServiceType != null && this.ServiceType.ToString() != "")
			{
				int.TryParse(this.ServiceType.ToString(), out num);
			}
			base.Text = "";
			foreach (OnlineServiceTypes value in Enum.GetValues(typeof(OnlineServiceTypes)))
			{
				if (value == (OnlineServiceTypes)num)
				{
					base.Text = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					break;
				}
			}
			base.Render(writer);
		}
	}
}
