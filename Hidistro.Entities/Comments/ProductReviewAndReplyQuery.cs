using Hidistro.Core.Entities;

namespace Hidistro.Entities.Comments
{
	public class ProductReviewAndReplyQuery : Pagination
	{
		public long ReviewId
		{
			get;
			set;
		}

		public virtual int ProductId
		{
			get;
			set;
		}
	}
}
