using System;
using System.Linq.Expressions;

namespace Hidistro.Core
{
	public class QueryBase
	{
		public int PageNo
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}

		public string Sort
		{
			get;
			set;
		}

		public bool IsAsc
		{
			get;
			set;
		}

		public string Key
		{
			get;
			set;
		}

		public QueryBase()
		{
			this.PageNo = 1;
			this.PageSize = 10;
		}
	}
	public class QueryBase<T, Tout> where T : BaseModel
	{
		public int PageNo
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}

		public Expression<Func<T, Tout>> Sort
		{
			get;
			set;
		}

		public bool IsAsc
		{
			get;
			set;
		}
	}
}
