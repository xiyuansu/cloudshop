using Hidistro.Entities.Commodities;
using System.Collections.Generic;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	internal class ProductDetail
	{
		public ProductInfo pi;

		public Dictionary<int, IList<int>> attrs;

		public IList<int> tagIds;
	}
}
