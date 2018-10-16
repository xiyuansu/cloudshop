using System;

namespace Hishop.Alipay.OpenHome.Model
{
	[Serializable]
	public abstract class AliResponse
	{
		public string sign
		{
			get;
			set;
		}

		public ErrorResponse error_response
		{
			get;
			set;
		}
	}
}
