using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Comments
{
	[TableName("Hishop_Affiche")]
	public class AfficheInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int AfficheId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValAfficheInfo", MessageTemplate = "公告标题不能为空，长度限制在60个字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Title
		{
			get;
			set;
		}

		[StringLengthValidator(1, 999999999, Ruleset = "ValAfficheInfo", MessageTemplate = "公告内容不能为空")]
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
	}
}
