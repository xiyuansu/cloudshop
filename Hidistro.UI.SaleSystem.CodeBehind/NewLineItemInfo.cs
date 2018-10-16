using Hidistro.Entities.Orders;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class NewLineItemInfo : LineItemInfo
	{
		public new string OrderId
		{
			get;
			set;
		}

		public new LineItemStatus Status
		{
			get;
			set;
		}

		public new string StatusText
		{
			get;
			set;
		}
	}
}
