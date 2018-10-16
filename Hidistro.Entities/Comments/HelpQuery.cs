using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Comments
{
	public class HelpQuery : Pagination
	{
		public int? CategoryId
		{
			get;
			set;
		}

		public string Keywords
		{
			get;
			set;
		}

		public DateTime? StartArticleTime
		{
			get;
			set;
		}

		public DateTime? EndArticleTime
		{
			get;
			set;
		}
	}
}
