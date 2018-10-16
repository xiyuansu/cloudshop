using Hidistro.Entities.Members;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class TradeTypeNameLabel : Label
	{
		private TradeTypes type;

		private string tradeType;

		private bool isDistributor = false;

		public string TradeType
		{
			get
			{
				return this.tradeType;
			}
			set
			{
				this.tradeType = value;
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

		protected override void OnDataBinding(EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.tradeType))
			{
				this.type = (TradeTypes)Enum.Parse(typeof(TradeTypes), DataBinder.Eval(this.Page.GetDataItem(), this.TradeType).ToString());
			}
			else
			{
				base.OnDataBinding(e);
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			switch (this.type)
			{
			case TradeTypes.SelfhelpInpour:
				base.Text = "自助充值";
				break;
			case TradeTypes.RechargeGift:
				base.Text = "充值赠送";
				break;
			case TradeTypes.BackgroundAddmoney:
				base.Text = "后台加款";
				break;
			case TradeTypes.Consume:
				base.Text = "消费";
				break;
			case TradeTypes.DrawRequest:
				base.Text = "提现";
				break;
			case TradeTypes.RefundOrder:
				if (this.isDistributor)
				{
					base.Text = "采购单退款";
				}
				else
				{
					base.Text = "订单退款";
				}
				break;
			case TradeTypes.ReturnOrder:
				base.Text = (this.isDistributor ? "采购单退货" : "订单退货");
				break;
			case TradeTypes.Commission:
				base.Text = "分销奖励";
				break;
			default:
				base.Text = "其他";
				break;
			}
			base.Render(writer);
		}
	}
}
