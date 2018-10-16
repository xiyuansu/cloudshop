using Hidistro.Core;
using Hishop.Components.Validation.Validators;

namespace Hidistro.Entities.Comments
{
	[TableName("Hishop_ArticleCategories")]
	public class ArticleCategoryInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int CategoryId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValArticleCategoryInfo", MessageTemplate = "分类名称不能为空，长度限制在60个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Name
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
		public string IconUrl
		{
			get;
			set;
		}

		[StringLengthValidator(0, 300, Ruleset = "ValArticleCategoryInfo", MessageTemplate = "分类介绍最多只能输入300个字符")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}
	}
}
