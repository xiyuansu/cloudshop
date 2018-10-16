using Hishop.Components.Validation.Validators;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	[TableName("Hishop_Attributes")]
	public class AttributeInfo
	{
		private IList<AttributeValueInfo> attributeValues;

		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int AttributeId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 30, Ruleset = "ValAttribute", MessageTemplate = "扩展属性的名称，长度在1至30个字符之间")]
		[FieldType(FieldType.CommonField)]
		public string AttributeName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DisplaySequence
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int TypeId
		{
			get;
			set;
		}

		public string TypeName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public AttributeUseageMode UsageMode
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool UseAttributeImage
		{
			get;
			set;
		}

		public bool IsMultiView
		{
			get
			{
				return this.UsageMode == AttributeUseageMode.MultiView;
			}
		}

		public IList<AttributeValueInfo> AttributeValues
		{
			get
			{
				if (this.attributeValues == null)
				{
					this.attributeValues = new List<AttributeValueInfo>();
				}
				return this.attributeValues;
			}
			set
			{
				this.attributeValues = value;
			}
		}

		public string ValuesString
		{
			get
			{
				string text = string.Empty;
				foreach (AttributeValueInfo attributeValue in this.AttributeValues)
				{
					text = text + attributeValue.ValueStr + ",";
				}
				if (text.Length > 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
		}
	}
}
