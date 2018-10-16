using System;

namespace Hidistro.Entities
{
	[TableName("Hishop_UserSignIn")]
	public class UserSignInInfo
	{
		[FieldType(FieldType.KeyField)]
		public int UserId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime LastSignDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ContinuousDays
		{
			get;
			set;
		}
	}
}
