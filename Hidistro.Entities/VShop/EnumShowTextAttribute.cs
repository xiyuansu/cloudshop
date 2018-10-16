using System;

namespace Hidistro.Entities.VShop
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public sealed class EnumShowTextAttribute : Attribute
	{
		public string ShowText
		{
			get;
			private set;
		}

		public EnumShowTextAttribute(string showTest)
		{
			this.ShowText = showTest;
		}
	}
}
