using System.ComponentModel;

namespace Hidistro.Entities
{
	public enum EnumStore_PositionNoMatchTo : byte
	{
		[Description("推荐至平台")]
		Platform = 1,
		[Description("不推荐至平台")]
		Nothing
	}
}
