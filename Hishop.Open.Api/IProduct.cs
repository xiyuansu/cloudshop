using System;

namespace Hishop.Open.Api
{
	public interface IProduct
	{
		string GetSoldProducts(DateTime? start_modified, DateTime? end_modified, string approve_status, string q, string order_by, int page_no, int page_size);

		string GetProduct(int num_iid);

		string UpdateProductQuantity(int num_iid, string sku_id, int quantity, int type);

		string UpdateProductApproveStatus(int num_iid, string approve_status);
	}
}
