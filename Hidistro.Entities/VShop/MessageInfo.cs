using System;

namespace Hidistro.Entities.VShop
{
	[Serializable]
	[TableName("vshop_Message")]
	public class MessageInfo
	{
		[FieldType(FieldType.CommonField)]
		public int? ReplyId
		{
			get;
			set;
		}

		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int MsgId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Title
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Url
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Description
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ImageUrl
		{
			get;
			set;
		}

		public int UrlType
		{
			get;
			set;
		}
	}
}
