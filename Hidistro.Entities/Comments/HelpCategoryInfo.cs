using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System.Collections.Generic;

namespace Hidistro.Entities.Comments
{
	[TableName("Hishop_HelpCategories")]
	public class HelpCategoryInfo
	{
		private IList<HelpInfo> helps;

		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int CategoryId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValHelpCategoryInfo", MessageTemplate = "分类名称不能为空，长度限制在60个字符以内")]
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

		[FieldType(FieldType.CommonField)]
		public string IndexChar
		{
			get;
			set;
		}

		[StringLengthValidator(0, 300, Ruleset = "ValHelpCategoryInfo", MessageTemplate = "分类介绍最多只能输入300个字符")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Description
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

		public IList<HelpInfo> Helps
		{
			get
			{
				if (this.helps == null)
				{
					this.helps = new List<HelpInfo>();
				}
				return this.helps;
			}
			set
			{
				this.helps = value;
			}
		}
	}
}
