using Hidistro.Entities.Comments;
using System.Collections.Generic;

namespace Hidistro.Entities.APP
{
	public class GetProductConsultationModel
	{
		public List<ProductConsultationInfo> ProductConsultationlst
		{
			get;
			set;
		}

		public int PageIndex
		{
			get;
			set;
		}

		public int PageSize
		{
			get;
			set;
		}

		public int Total
		{
			get;
			set;
		}
	}
}
