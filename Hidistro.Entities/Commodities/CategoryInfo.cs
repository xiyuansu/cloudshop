using Hidistro.Core;
using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System.Text.RegularExpressions;

namespace Hidistro.Entities.Commodities
{
	[HasSelfValidation]
	[TableName("Hishop_Categories")]
	public class CategoryInfo
	{
		private string iconUrl;

		[FieldType(FieldType.KeyField)]
		public int CategoryId
		{
			get;
			set;
		}

		public string IconUrl
		{
			get
			{
				return this.iconUrl;
			}
			set
			{
				this.iconUrl = value;
			}
		}

		[FieldType(FieldType.CommonField)]
		public string BigImageUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ParentCategoryId
		{
			get;
			set;
		}

		public int TopCategoryId
		{
			get
			{
				if (this.Depth == 1)
				{
					return this.CategoryId;
				}
				string s = this.Path.Substring(0, this.Path.IndexOf("|"));
				return int.Parse(s);
			}
		}

		[StringLengthValidator(1, 60, Ruleset = "ValCategory", MessageTemplate = "分类名称不能为空，长度限制在60个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Name
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string SKUPrefix
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

		[StringLengthValidator(0, 100, Ruleset = "ValCategory", MessageTemplate = "告诉搜索引擎此分类浏览页面的主要内容，长度限制在100个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Description
		{
			get;
			set;
		}

		[StringLengthValidator(0, 50, Ruleset = "ValCategory", MessageTemplate = "告诉搜索引擎此分类浏览页面的标题，长度限制在50个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Title
		{
			get;
			set;
		}

		[StringLengthValidator(0, 100, Ruleset = "ValCategory", MessageTemplate = "让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Meta_Keywords
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Notes1
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Notes2
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Notes3
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Notes4
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Notes5
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Depth
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Path
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Icon
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

		[FieldType(FieldType.CommonField)]
		public int? AssociatedProductType
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

		[FieldType(FieldType.CommonField)]
		public bool HasChildren
		{
			get;
			set;
		}

		[SelfValidation(Ruleset = "ValCategory")]
		public void CheckCategory(ValidationResults results)
		{
			if (!string.IsNullOrEmpty(this.SKUPrefix) && (this.SKUPrefix.Length > 5 || !Regex.IsMatch(this.SKUPrefix, "(?!_)(?!-)[a-zA-Z0-9_-]+")))
			{
				results.AddResult(new ValidationResult("商家编码前缀长度限制在5个字符以内,只能以字母或数字开头", this, "", "", null));
			}
			if (!string.IsNullOrEmpty(this.RewriteName) && (this.RewriteName.Length > 60 || !Regex.IsMatch(this.RewriteName, "(^[-_a-zA-Z0-9]+$)")))
			{
				results.AddResult(new ValidationResult("使用URL重写长度限制在60个字符以内，必须为字母数字-和_", this, "", "", null));
			}
		}
	}
}
