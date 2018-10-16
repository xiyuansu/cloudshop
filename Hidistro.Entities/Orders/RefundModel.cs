using System;

namespace Hidistro.Entities.Orders
{
	public class RefundModel
	{
		public int RefundId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public DateTime ApplyForTime
		{
			get;
			set;
		}

		public RefundTypes RefundType
		{
			get;
			set;
		}

		public decimal RefundAmount
		{
			get;
			set;
		}

		public string UserRemark
		{
			get;
			set;
		}

		public DateTime? FinishTime
		{
			get;
			set;
		}

		public string AdminRemark
		{
			get;
			set;
		}

		public string Operator
		{
			get;
			set;
		}

		public RefundStatus HandleStatus
		{
			get;
			set;
		}

		public string RefundOrderId
		{
			get;
			set;
		}

		public string RefundGateWay
		{
			get;
			set;
		}

		public int? StoreId
		{
			get;
			set;
		}

		public string RefundReason
		{
			get;
			set;
		}

		public string BankName
		{
			get;
			set;
		}

		public string BankAccountName
		{
			get;
			set;
		}

		public string BankAccountNo
		{
			get;
			set;
		}

		public DateTime? AgreedOrRefusedTime
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string OrderTotal
		{
			get;
			set;
		}

		public OrderStatus OrderStatus
		{
			get;
			set;
		}

		public int SupplierId
		{
			get;
			set;
		}

		public string ShipperName
		{
			get;
			set;
		}

		public string ExceptionInfo
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public int Quantity
		{
			get;
			set;
		}

		public bool IsServiceProduct
		{
			get;
			set;
		}

		public string ValidCodes
		{
			get;
			set;
		}

		public string Gateway
		{
			get;
			set;
		}

		public string GetDealTime
		{
			get
			{
				if (this.HandleStatus == RefundStatus.Applied)
				{
					return "";
				}
				DateTime value;
				if (this.HandleStatus == RefundStatus.Refunded)
				{
					object result;
					if (!this.FinishTime.HasValue)
					{
						if (!this.AgreedOrRefusedTime.HasValue)
						{
							result = "";
						}
						else
						{
							value = this.AgreedOrRefusedTime.Value;
							result = value.ToString("yyyy-MM-dd HH:mm");
						}
					}
					else
					{
						value = this.FinishTime.Value;
						result = value.ToString("yyyy-MM-dd HH:mm");
					}
					return (string)result;
				}
				object result2;
				if (!this.AgreedOrRefusedTime.HasValue)
				{
					result2 = "";
				}
				else
				{
					value = this.AgreedOrRefusedTime.Value;
					result2 = value.ToString("yyyy-MM-dd HH:mm");
				}
				return (string)result2;
			}
		}

		public string PayOrderId
		{
			get;
			set;
		}
	}
}
