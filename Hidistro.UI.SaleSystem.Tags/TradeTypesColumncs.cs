using Hidistro.Entities.Members;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class TradeTypesColumncs : Literal
	{
		private TradeTypes tradeTypes;

		public TradeTypes TradeTypes
		{
			get
			{
				return this.tradeTypes;
			}
			set
			{
				this.tradeTypes = value;
			}
		}

		public override void DataBind()
		{
			this.tradeTypes = (TradeTypes)DataBinder.Eval(this.Page.GetDataItem(), "TradeType");
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.tradeTypes == TradeTypes.SelfhelpInpour)
			{
				base.Text = "自助充值";
			}
			else if (this.tradeTypes == TradeTypes.BackgroundAddmoney)
			{
				base.Text = "后台加款";
			}
			else if (this.tradeTypes == TradeTypes.Consume)
			{
				base.Text = "消费";
			}
			else if (this.tradeTypes == TradeTypes.DrawRequest)
			{
				base.Text = "提现";
			}
			else if (this.tradeTypes == TradeTypes.RefundOrder)
			{
				base.Text = "订单退款";
			}
			else if (this.tradeTypes == TradeTypes.ReturnOrder)
			{
				base.Text = "订单退货";
			}
			else if (this.tradeTypes == TradeTypes.Commission)
			{
				base.Text = "分销奖励";
			}
			else if (this.tradeTypes == TradeTypes.RechargeGift)
			{
				base.Text = "充值赠送";
			}
			else
			{
				base.Text = "未知";
			}
			base.Render(writer);
		}
	}
}
