using System.Collections.Generic;

namespace Hishop.Alipay.OpenHome.Model
{
	public class Button
	{
		public string name
		{
			get;
			set;
		}

		public string actionParam
		{
			get;
			set;
		}

		public string actionType
		{
			get;
			set;
		}

		public string authType
		{
			get
			{
				return "loginAuth";
			}
		}

		public IEnumerable<Button> subButton
		{
			get;
			set;
		}
	}
}
