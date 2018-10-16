using System;
using System.ComponentModel;

namespace Hidistro.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class GlobalCodeAttribute : DescriptionAttribute
	{
		public bool OnlyAttribute
		{
			get;
			set;
		}

		public int DisplayOrder
		{
			get;
			set;
		}

		public string ExtName
		{
			get;
			set;
		}

		public bool IsHtmlCode
		{
			get;
			set;
		}

		public bool IsEncryption
		{
			get;
			set;
		}

		public bool IsUrlEncode
		{
			get;
			set;
		}

		public GlobalCodeAttribute(string description)
			: base(description)
		{
		}
	}
}
