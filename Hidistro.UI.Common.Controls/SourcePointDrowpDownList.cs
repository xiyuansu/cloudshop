using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SourcePointDrowpDownList : DropDownList
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
				base.Items.Add(new ListItem("请选择积分来源", string.Empty));
			}
			base.Items.Add(new ListItem("兑换优惠券", "0"));
			base.Items.Add(new ListItem("兑换礼品", "1"));
			base.Items.Add(new ListItem("购物奖励", "2"));
			base.Items.Add(new ListItem("退款或关闭订单", "3"));
			base.Items.Add(new ListItem("抽奖获得积分", "4"));
			base.Items.Add(new ListItem("摇一摇抽奖", "5"));
			base.Items.Add(new ListItem("每日签到", "6"));
			base.Items.Add(new ListItem("管理员修改", "7"));
			base.Items.Add(new ListItem("会员注册", "8"));
			base.Items.Add(new ListItem("连续签到", "9"));
			base.Items.Add(new ListItem("购物抵扣", "11"));
			base.Items.Add(new ListItem("大转盘抽奖", "12"));
			base.Items.Add(new ListItem("刮刮卡抽奖", "13"));
			base.Items.Add(new ListItem("砸金蛋抽奖", "14"));
			base.Items.Add(new ListItem("商品评论", "16"));
		}
	}
}
