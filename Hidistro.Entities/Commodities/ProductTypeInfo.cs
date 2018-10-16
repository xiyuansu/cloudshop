using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	[TableName("Hishop_ProductTypes")]
	public class ProductTypeInfo
	{
		private IList<int> brands;

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int TypeId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 30, Ruleset = "ValProductType", MessageTemplate = "商品类型名称不能为空，长度限制在1-30个字符之间")]
		[FieldType(FieldType.CommonField)]
		public string TypeName
		{
			get;
			set;
		}

		public IList<int> Brands
		{
			get
			{
				if (this.brands == null)
				{
					this.brands = new List<int>();
				}
				return this.brands;
			}
			set
			{
				this.brands = value;
			}
		}

		[StringLengthValidator(0, 100, Ruleset = "ValProductType", MessageTemplate = "备注的长度限制在0-100个字符之间")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Remark
		{
			get;
			set;
		}
	}
}
