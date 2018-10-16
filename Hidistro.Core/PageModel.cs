using System.Collections.Generic;

namespace Hidistro.Core
{
	public class PageModel<T>
	{
		public IEnumerable<T> Models
		{
			get;
			set;
		}

		public int Total
		{
			get;
			set;
		}

		public IEnumerable<T> rows
		{
			get
			{
				return this.Models;
			}
		}

		public int total
		{
			get
			{
				return this.Total;
			}
		}
	}
}
