using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Comments
{
	[TableName("Hishop_Helps")]
	public class HelpInfo
	{
		[FieldType(FieldType.CommonField)]
		public int CategoryId
		{
			get;
			set;
		}

		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int HelpId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValHelpInfo", MessageTemplate = "帮助主题不能为空，长度限制在60个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Title
		{
			get;
			set;
		}

		[StringLengthValidator(0, 260, Ruleset = "ValHelpInfo", MessageTemplate = "告诉搜索引擎此帮助页面的主要内容，长度限制在260个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Description
		{
			get;
			set;
		}

		[StringLengthValidator(0, 160, Ruleset = "ValHelpInfo", MessageTemplate = "让用户可以通过搜索引擎搜索到此帮助的浏览页面，长度限制在160个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Keywords
		{
			get;
			set;
		}

		[StringLengthValidator(0, 300, Ruleset = "ValHelpInfo", MessageTemplate = "摘要的长度限制在300个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		[StringLengthValidator(1, 999999999, Ruleset = "ValHelpInfo", MessageTemplate = "帮助内容不能为空")]
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
		public bool IsShowFooter
		{
			get;
			set;
		}
	}
}
