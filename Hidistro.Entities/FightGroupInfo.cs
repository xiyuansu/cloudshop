using Hidistro.Entities.VShop;
using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_FightGroups")]
	public class FightGroupInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int FightGroupId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime StartTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int FightGroupActivityId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EndTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int JoinNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public FightGroupStatus Status
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? CreateTime
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
		public string ProductName
		{
			get;
			set;
		}
	}
}
