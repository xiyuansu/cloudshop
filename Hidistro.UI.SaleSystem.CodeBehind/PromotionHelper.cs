using Hidistro.Entities.Promotions;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class PromotionHelper
	{
		public static string GetShortName(PromoteType promotionType)
		{
			string result = "";
			switch (promotionType)
			{
			case PromoteType.QuantityDiscount:
				result = "折";
				break;
			case PromoteType.FullAmountSentFreight:
				result = "包邮";
				break;
			case PromoteType.FullAmountSentTimesPoint:
				result = "积分加倍";
				break;
			case PromoteType.SentProduct:
				result = "送";
				break;
			}
			return result;
		}
	}
}
