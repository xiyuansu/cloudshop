using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Comments
{
	[TableName("Hishop_Articles")]
	public class ArticleInfo
	{
		[FieldType(FieldType.CommonField)]
		public int CategoryId
		{
			get;
			set;
		}

		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ArticleId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 200, Ruleset = "ValArticleInfo", MessageTemplate = "文章标题不能为空，长度限制在200个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Title
		{
			get;
			set;
		}

		[StringLengthValidator(0, 260, Ruleset = "ValArticleInfo", MessageTemplate = "告诉搜索引擎此文章页面的主要内容，长度限制在260个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Description
		{
			get;
			set;
		}

		[StringLengthValidator(0, 160, Ruleset = "ValArticleInfo", MessageTemplate = "让用户可以通过搜索引擎搜索到此文章的浏览页面，长度限制在160个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Keywords
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

		[StringLengthValidator(0, 300, Ruleset = "ValArticleInfo", MessageTemplate = "文章摘要的长度限制在300个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		[StringLengthValidator(1, 999999999, Ruleset = "ValArticleInfo", MessageTemplate = "文章内容不能为空")]
		[FieldType(FieldType.CommonField)]
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime AddedDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsRelease
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Hits
		{
			get;
			set;
		}
	}
}
