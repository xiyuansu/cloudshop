using System;

namespace Hidistro.Entities.Store
{
	[TableName("aspnet_Managers")]
	public class ManagerInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ManagerId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int RoleId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string UserName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Password
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string PasswordSalt
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime CreateDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string SessionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int Status
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string HeadImage
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ContactInfo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ClientId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Token
		{
			get;
			set;
		}
	}
}
