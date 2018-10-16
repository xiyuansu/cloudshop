using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.depot.Models
{
	public class HiPOSDetailsListListModel : DataGridViewModel<Dictionary<string, object>>
	{
		public decimal sum_amount
		{
			get;
			set;
		}

		public int recordcount
		{
			get;
			set;
		}
	}
}
