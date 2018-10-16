using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderRemarkImage : Literal
	{
		private string imageFormat = "<img border=\"0\" src=\"{0}\"  />";

		private string dataField;

		public string DataField
		{
			get
			{
				return this.dataField;
			}
			set
			{
				this.dataField = value;
			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
			if (obj != null && obj != DBNull.Value)
			{
				base.Text = string.Format(this.imageFormat, this.GetImageSrc(obj));
			}
			else
			{
				base.Text = "&#xe603;";
			}
			base.OnDataBinding(e);
		}

		protected string GetImageSrc(object managerMark)
		{
			string str = "/Admin/images/";
			switch ((OrderMark)managerMark)
			{
			case OrderMark.Red:
				return str + "iconc.png";
			case OrderMark.Green:
				return str + "icona.png";
			case OrderMark.Gray:
				return str + "iconae.png";
			case OrderMark.Draw:
				return str + "iconaf.png";
			case OrderMark.Yellow:
				return str + "iconad.png";
			case OrderMark.ExclamationMark:
				return str + "iconb.png";
			default:
				return string.Format(this.imageFormat, "/Admin/images/xi.gif");
			}
		}
	}
}
