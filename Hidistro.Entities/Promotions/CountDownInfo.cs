using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_CountDown")]
	public class CountDownInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int CountDownId
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
		public DateTime StartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime EndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int MaxCount
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
		public int DisplaySequence
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareTitle
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareDetails
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShareIcon
		{
			get;
			set;
		}

		public bool IsChangeProduct
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreType
		{
			get;
			set;
		}

		public string StoreIds
		{
			get;
			set;
		}

		public bool IsJoin
		{
			get;
			set;
		}

		public bool IsNomal
		{
			get;
			set;
		}

		public List<CountDownSkuInfo> CountDownSkuInfo
		{
			get;
			set;
		}

		public bool IsRunning
		{
			get
			{
				if (this.StartDate > DateTime.Now || this.EndDate <= DateTime.Now)
				{
					return false;
				}
				return (from t in this.CountDownSkuInfo
				where t.BoughtCount >= t.ActivityTotal
				select t).Count() < this.CountDownSkuInfo.Count;
			}
		}
	}
}
