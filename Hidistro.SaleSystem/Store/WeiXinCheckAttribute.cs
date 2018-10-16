using System;

namespace Hidistro.SaleSystem.Store
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class WeiXinCheckAttribute : Attribute
	{
		public bool boolCheck;

		public bool BoolCheck
		{
			get
			{
				return this.boolCheck;
			}
		}

		public WeiXinCheckAttribute(bool boolCheck)
		{
			this.boolCheck = boolCheck;
		}
	}
}
