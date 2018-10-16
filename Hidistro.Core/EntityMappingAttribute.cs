using System;

namespace Hidistro.Core
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class EntityMappingAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}
	}
}
