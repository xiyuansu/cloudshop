using Hidistro.Core.Entities;

namespace Hidistro.Entities.Comments
{
	public class MessageBoxQuery : Pagination
	{
		public string Sernder
		{
			get;
			set;
		}

		public string Accepter
		{
			get;
			set;
		}

		public MessageStatus MessageStatus
		{
			get;
			set;
		}
	}
}
