using Hidistro.Context;
using Hidistro.Entities.Members;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class TradeTypeDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay;

		private bool isDistributor = false;

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

		public bool IsDistributor
		{
			get
			{
				return this.isDistributor;
			}
			set
			{
				this.isDistributor = value;
			}
		}

		public new TradeTypes SelectedValue
		{
			get
			{
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					return TradeTypes.NotSet;
				}
				return (TradeTypes)int.Parse(base.SelectedValue);
			}
			set
			{
				ListItemCollection items = base.Items;
				ListItemCollection items2 = base.Items;
				int num = (int)value;
				base.SelectedIndex = items.IndexOf(items2.FindByValue(num.ToString()));
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			this.Items.Add(new ListItem("自助充值", "1"));
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsOpenRechargeGift)
			{
				this.Items.Add(new ListItem("充值赠送", "9"));
			}
			this.Items.Add(new ListItem("后台加款", "2"));
			this.Items.Add(new ListItem("消费", "3"));
			this.Items.Add(new ListItem("提现", "4"));
			if (this.IsDistributor)
			{
				this.Items.Add(new ListItem("采购单退款", "5"));
				this.Items.Add(new ListItem("采购单退货", "7"));
			}
			else
			{
				this.Items.Add(new ListItem("订单退款", "5"));
				this.Items.Add(new ListItem("订单退货", "7"));
			}
			this.Items.Add(new ListItem("分销奖励", "8"));
		}
	}
}
