using System;

namespace Hidistro.SaleSystem.Vshop
{
	public class GetPrivateTemplate_TemplateItem
	{
		public string template_id
		{
			get;
			set;
		}

		public string title
		{
			get;
			set;
		}

		public string primary_industry
		{
			get;
			set;
		}

		public string deputy_industry
		{
			get;
			set;
		}

		public string content
		{
			get;
			set;
		}

		public string example
		{
			get;
			set;
		}

		public IndustryCode ConvertToIndustryCode()
		{
			string value = string.Format("{0}_{1}", this.primary_industry, this.deputy_industry.Replace("|", "_").Replace("/", "_"));
			IndustryCode result = default(IndustryCode);
			if (!Enum.TryParse<IndustryCode>(value, true, out result))
			{
				return IndustryCode.其它_其它;
			}
			return result;
		}
	}
}
