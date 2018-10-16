using System;

namespace Hidistro.Entities.VShop
{
	public class ReplyInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ReplyId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Keys
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public MatchType MatchType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public ReplyType ReplyType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public MessageType MessageType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDisable
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime LastEditDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string LastEditor
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityId
		{
			get;
			set;
		}

		public string MessageTypeName
		{
			get
			{
				return ((Enum)(object)this.MessageType).ToShowText();
			}
		}

		[FieldType(FieldType.CommonField)]
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Type
		{
			get;
			set;
		}

		public ReplyInfo()
		{
			this.LastEditDate = DateTime.Now;
			this.MatchType = MatchType.Like;
			this.MessageType = MessageType.Text;
		}
	}
}
