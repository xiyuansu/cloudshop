using System.Collections.Generic;

namespace Hidistro.UI.Web.ashxBase
{
	public class DataGridViewModel<T>
	{
		public List<T> rows
		{
			get;
			set;
		}

		public int total
		{
			get;
			set;
		}
	}
}
