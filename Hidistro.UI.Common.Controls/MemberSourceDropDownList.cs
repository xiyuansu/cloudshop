using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class MemberSourceDropDownList : DropDownList
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
			base.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			ListItemCollection items = base.Items;
			int num = 1;
			items.Add(new ListItem("PC端", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items2 = base.Items;
			num = 2;
			items2.Add(new ListItem("触屏版", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items3 = base.Items;
			num = 3;
			items3.Add(new ListItem("微信商城", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items4 = base.Items;
			num = 4;
			items4.Add(new ListItem("生活号（原支付宝服务窗）", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items5 = base.Items;
			num = 5;
			items5.Add(new ListItem("APP", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items6 = base.Items;
			num = 6;
			items6.Add(new ListItem("微信小程序", num.ToString(CultureInfo.InvariantCulture)));
			ListItemCollection items7 = base.Items;
			num = 7;
			items7.Add(new ListItem("导入会员", num.ToString(CultureInfo.InvariantCulture)));
		}
	}
}
