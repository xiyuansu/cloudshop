using System.Data;

namespace Hidistro.Core.Entities
{
	public class DbQueryResult
	{
		public int TotalRecords
		{
			get;
			set;
		}

		public DataTable Data
		{
			get;
			set;
		}
	}
}
