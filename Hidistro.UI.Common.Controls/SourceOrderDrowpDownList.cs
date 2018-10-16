using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SourceOrderDrowpDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

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

		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
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
				return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			base.Items.Add(new ListItem("pc端订单", "1"));
			base.Items.Add(new ListItem("淘宝订单", "2"));
			base.Items.Add(new ListItem("微信订单", "3"));
			base.Items.Add(new ListItem("触屏版订单", "4"));
			base.Items.Add(new ListItem("生活号（原支付宝服务窗）", "5"));
			base.Items.Add(new ListItem("App订单", "6"));
			base.Items.Add(new ListItem("京东订单", "7"));
			base.Items.Add(new ListItem("微信小程序", "8"));
		}
	}
}
