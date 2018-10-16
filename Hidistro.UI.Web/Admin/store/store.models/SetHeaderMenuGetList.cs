using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.store.models
{
	public class SetHeaderMenuGetList : DataGridViewModel<Dictionary<string, object>>
	{
		public int CategoryNum
		{
			get;
			set;
		}
	}
}
