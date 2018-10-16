using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.depot.Models
{
	public class StoreResponseModel<T>
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

		public List<Dictionary<string, object>> oders
		{
			get;
			set;
		}
	}
}
