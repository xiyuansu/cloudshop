using Senparc.Weixin.Entities;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Vshop
{
	public class PrivateTemplateJsonResult : WxJsonResult
	{
		public List<GetPrivateTemplate_TemplateItem> template_list
		{
			get;
			set;
		}
	}
}
