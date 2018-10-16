using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Comments
{
	[TableName("Hishop_ProductReviews")]
	public class ProductReviewInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public long ReviewId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ProductId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UserId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 500, Ruleset = "Refer", MessageTemplate = "评论内容为必填项，长度限制在500字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string ReviewText
		{
			get;
			set;
		}

		[StringLengthValidator(1, 30, Ruleset = "Refer", MessageTemplate = "用户昵称为必填项，长度限制在30字符以内")]
		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string UserName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserEmail
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime ReviewDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string OrderId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SkuId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SkuContent
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Score
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl1
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl2
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl3
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl4
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl5
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ReplyText
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? ReplyDate
		{
			get;
			set;
		}
	}
}
