using Hidistro.Core;
using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	[TableName("Hishop_BrandCategories")]
	public class BrandCategoryInfo
	{
		private IList<int> productTypes;

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int BrandId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 30, Ruleset = "ValBrandCategory", MessageTemplate = "品牌名称不能为空，长度限制在30个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string BrandName
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValBrandCategory")]
		[ValidatorComposition(CompositionType.Or, Ruleset = "ValBrandCategory", MessageTemplate = "品牌官方网站的网址必须以http://开头，长度限制在100个字符以内")]
		[RegexValidator("^(http)://.*", Ruleset = "ValBrandCategory")]
		[FieldType(FieldType.CommonField)]
		public string CompanyUrl
		{
			get;
			set;
		}

		[StringLengthValidator(0, 300, Ruleset = "ValCategory", MessageTemplate = "品牌介绍的长度限制在300个字符以内")]
		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RewriteName
		{
			get;
			set;
		}

		[StringLengthValidator(0, 160, Ruleset = "ValCategory", MessageTemplate = "让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在160个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string MetaKeywords
		{
			get;
			set;
		}

		[StringLengthValidator(0, 260, Ruleset = "ValCategory", MessageTemplate = "让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在260个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string MetaDescription
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
		public string Logo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Theme
		{
			get;
			set;
		}

		public IList<int> ProductTypes
		{
			get
			{
				if (this.productTypes == null)
				{
					this.productTypes = new List<int>();
				}
				return this.productTypes;
			}
			set
			{
				this.productTypes = value;
			}
		}
	}
}
