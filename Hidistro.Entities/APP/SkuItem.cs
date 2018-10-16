using System.Collections.Generic;

namespace Hidistro.Entities.APP
{
	public class SkuItem
	{
		public string AttributeName
		{
			get;
			set;
		}

		public string AttributeId
		{
			get;
			set;
		}

		public List<AttributeValue> AttributeValue
		{
			get;
			set;
		}
	}
}
