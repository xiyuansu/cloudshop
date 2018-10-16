using System;

namespace Hidistro.Entities.Members
{
	[TableName("aspnet_Referrals")]
	public class ReferralInfo
	{
		[FieldType(FieldType.KeyField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ReferralStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RequetReason
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? RequetDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RefusalReason
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? AuditDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsRepeled
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? RepelTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string RepelReason
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string BannerUrl
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ShopName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int GradeId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string CellPhone
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Email
		{
			get;
			set;
		}

		public string GradeName
		{
			get;
			set;
		}

		public int SubNumber
		{
			get;
			set;
		}

		public decimal LowerSaleTotal
		{
			get;
			set;
		}

		public decimal UserAllSplittin
		{
			get;
			set;
		}

		public string NickName
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}
	}
}
