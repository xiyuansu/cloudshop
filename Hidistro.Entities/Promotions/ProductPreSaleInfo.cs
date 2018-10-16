using System;

namespace Hidistro.Entities.Promotions
{
	[TableName("Hishop_ProductPreSale")]
	public class ProductPreSaleInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int PreSaleId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ProductId
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public decimal SalePrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DepositPercent
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Deposit
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime PreSaleEndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime PaymentStartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime PaymentEndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int DeliveryDays
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? DeliveryDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int ExecutMark
		{
			get;
			set;
		}

		public ProductPreSaleInfo()
		{
		}

		public ProductPreSaleInfo(int preSaleId, int productId, int depositPercent, decimal deposit, DateTime preSaleEndDate, DateTime paymentStartDate, DateTime paymentEndDate, int deliveryDays, DateTime? deliveryDate)
		{
			this.PreSaleId = preSaleId;
			this.ProductId = productId;
			this.DepositPercent = depositPercent;
			this.Deposit = deposit;
			this.PreSaleEndDate = preSaleEndDate;
			this.PaymentStartDate = paymentStartDate;
			this.PaymentEndDate = paymentEndDate;
			this.DeliveryDays = deliveryDays;
			this.DeliveryDate = deliveryDate;
		}
	}
}
