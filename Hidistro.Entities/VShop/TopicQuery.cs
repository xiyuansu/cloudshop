using Hidistro.Core.Entities;

namespace Hidistro.Entities.VShop
{
	public class TopicQuery : Pagination
	{
		public string Title
		{
			get;
			set;
		}

		public int TopicType
		{
			get;
			set;
		}
	}
}
