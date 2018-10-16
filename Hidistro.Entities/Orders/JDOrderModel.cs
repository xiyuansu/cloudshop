using System.Collections.Generic;

namespace Hidistro.Entities.Orders
{
	public class JDOrderModel
	{
		public string OrderId
		{
			get;
			set;
		}

		public string CreatedAt
		{
			get;
			set;
		}

		public string OrderPayment
		{
			get;
			set;
		}

		public string PayType
		{
			get;
			set;
		}

		public string OrderReMark
		{
			get;
			set;
		}

		public string ModifyAt
		{
			get;
			set;
		}

		public string OrderManagerReMark
		{
			get;
			set;
		}

		public string Freight
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public bool IsExsit
		{
			get;
			set;
		}

		public JDOrderConsigneeModel Consignee
		{
			get;
			set;
		}

		public List<JDOrderItemModel> Products
		{
			get;
			set;
		}
	}
}
