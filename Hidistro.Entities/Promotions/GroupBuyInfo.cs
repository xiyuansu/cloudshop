using Hidistro.Core;
using System;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_GroupBuy")]
	public class GroupBuyInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int GroupBuyId
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
		public decimal NeedPrice
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

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Content
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public GroupBuyStatus Status
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Count
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Price
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

		public string StatusText
		{
			get
			{
				string result = "-";
				switch (this.Status)
				{
				case GroupBuyStatus.UnderWay:
					result = ((DateTime.Compare(this.StartDate, DateTime.Now) > 0) ? "还未开始" : "正在进行中");
					break;
				case GroupBuyStatus.Failed:
					result = "失败结束";
					break;
				case GroupBuyStatus.Success:
					result = "成功结束";
					break;
				case GroupBuyStatus.EndUntreated:
					result = "结束未处理";
					break;
				}
				return result;
			}
		}
	}
}
