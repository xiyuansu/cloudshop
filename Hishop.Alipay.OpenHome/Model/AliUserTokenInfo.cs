using System;

namespace Hishop.Alipay.OpenHome.Model
{
	[Serializable]
	public class AliUserTokenInfo
	{
		public string alipay_user_id
		{
			get;
			set;
		}

		public string access_token
		{
			get;
			set;
		}
	}
}
