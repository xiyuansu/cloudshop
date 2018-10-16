using System;

namespace Hishop.Alipay.OpenHome.Model
{
	[Serializable]
	public class ErrorResponse
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

		public string sub_code
		{
			get;
			set;
		}

		public string sub_msg
		{
			get;
			set;
		}

		public bool IsError
		{
			get
			{
				return !string.IsNullOrEmpty(this.code);
			}
		}
	}
}
