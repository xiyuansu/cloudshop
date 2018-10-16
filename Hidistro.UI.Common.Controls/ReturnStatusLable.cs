using Hidistro.Entities;
using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ReturnStatusLable : Label
	{
		private bool _ShowInAdmin = false;

		public int Status
		{
			get;
			set;
		}

		public bool ShowInAdmin
		{
			get
			{
				return this._ShowInAdmin;
			}
			set
			{
				this._ShowInAdmin = value;
			}
		}

		public int AfterSaleType
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			foreach (ReturnStatus value in Enum.GetValues(typeof(ReturnStatus)))
			{
				if (value == (ReturnStatus)this.Status)
				{
					if (!this.ShowInAdmin && value == ReturnStatus.GetGoods)
					{
						base.Text = EnumDescription.GetEnumDescription((Enum)(object)ReturnStatus.Deliverying, 0);
					}
					else if (this.AfterSaleType == 3)
					{
						base.Text = EnumDescription.GetEnumDescription((Enum)(object)value, 3);
					}
					else
					{
						base.Text = EnumDescription.GetEnumDescription((Enum)(object)value, 0);
					}
					break;
				}
			}
			base.Render(writer);
		}
	}
}
