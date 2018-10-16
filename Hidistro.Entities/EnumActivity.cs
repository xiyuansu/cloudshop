using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumActivity
	{
		[Description("团购")]
		GroupBuy = 1,
		[Description("限时抢购")]
		CountDownBuy
	}
}
