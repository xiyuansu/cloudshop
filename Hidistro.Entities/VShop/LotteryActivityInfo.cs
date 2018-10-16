using System;
using System.Collections.Generic;

namespace Hidistro.Entities.VShop
{
	[TableName("Vshop_LotteryActivity")]
	public class LotteryActivityInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int ActivityId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ActivityName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ActivityType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ActivityKey
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
		public DateTime EndTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ActivityDesc
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ActivityPic
		{
			get;
			set;
		}

		public List<PrizeSetting> PrizeSettingList
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PrizeSetting
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MaxNum
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int UsePoints
		{
			get;
			set;
		}
	}
}
