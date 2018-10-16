using System;

namespace Hishop.Alipay.OpenHome.Model
{
	[Serializable]
	public class AliResponseMessage
	{
		public string code
		{
			get;
			set;
		}

		public string msg
		{
			get;
			set;
		}
	}
}
