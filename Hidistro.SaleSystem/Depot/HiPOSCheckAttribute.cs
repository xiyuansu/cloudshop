using System;

namespace Hidistro.SaleSystem.Depot
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public class HiPOSCheckAttribute : Attribute
	{
		public bool boolCheck;

		public bool BoolCheck
		{
			get
			{
				return this.boolCheck;
			}
		}

		public HiPOSCheckAttribute(bool boolCheck)
		{
			this.boolCheck = boolCheck;
		}
	}
}
