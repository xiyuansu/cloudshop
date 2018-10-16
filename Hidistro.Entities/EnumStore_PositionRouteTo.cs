using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumStore_PositionRouteTo : byte
	{
		[Description("匹配至最近门店")]
		NearestStore = 1,
		[Description("多门店首页")]
		StoreList,
		[Description("进入平台首页")]
		Platform
	}
}
