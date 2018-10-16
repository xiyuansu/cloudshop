using System;

namespace Hidistro.Entities
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public sealed class FieldTypeAttribute : Attribute
	{
		private FieldType fieldType;

		public FieldType FieldType
		{
			get
			{
				return this.fieldType;
			}
		}

		public FieldTypeAttribute(FieldType fieldType)
		{
			this.fieldType = fieldType;
		}
	}
}
