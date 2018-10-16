using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SplitDrawStatusLable : Label
	{
		public object Status
		{
			get;
			set;
		}

		public object RequestState
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			foreach (SplitDrawStatus value in Enum.GetValues(typeof(SplitDrawStatus)))
			{
				if (value == (SplitDrawStatus)this.Status.ToInt(0))
				{
					base.Text = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					base.Attributes.Add("class", value.ToString().ToLower());
					break;
				}
			}
			if (this.Status.ToInt(0) == 1 && this.RequestState.ToInt(0) > 1)
			{
				base.Text = "处理中";
			}
			base.Render(writer);
		}
	}
}
