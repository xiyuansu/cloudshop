using System.Collections.Generic;

namespace Hishop.Alipay.OpenHome.Model
{
	public class Menu : ModelBase
	{
		public IEnumerable<Button> button
		{
			get;
			set;
		}
	}
}
