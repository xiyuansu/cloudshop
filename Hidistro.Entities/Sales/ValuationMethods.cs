using System.ComponentModel;

namespace Hidistro.Entities.Sales
{
	public enum ValuationMethods
	{
		[Description("按件数")]
		Number = 1,
		[Description("按重量")]
		Weight,
		[Description("按体积")]
		Volume
	}
}
