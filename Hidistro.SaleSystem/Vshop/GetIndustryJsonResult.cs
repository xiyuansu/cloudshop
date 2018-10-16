using Senparc.Weixin.Entities;

namespace Hidistro.SaleSystem.Vshop
{
	public class GetIndustryJsonResult : WxJsonResult
	{
		public GetIndustry_Item primary_industry
		{
			get;
			set;
		}

		public GetIndustry_Item secondary_industry
		{
			get;
			set;
		}
	}
}
