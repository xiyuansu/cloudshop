using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.depot.Models
{
	public class MemberDetailsModel<T>
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
