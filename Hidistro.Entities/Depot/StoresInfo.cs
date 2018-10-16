using System;

namespace Hidistro.Entities.Depot
{
	[TableName("Hishop_Stores")]
	public class StoresInfo
	{
		[FieldType(FieldType.KeyField)]
		[FieldType(FieldType.IncrementField)]
		public int StoreId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string StoreName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int TopRegionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int RegionId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Address
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string ContactMan
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Tel
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int State
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool CloseStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXCategoryName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WxAddress
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public double? Longitude
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public double? Latitude
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXBusinessName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXBranchName
		{
			get;
			set;
		}

		public string CategoryName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string StoreImages
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXTelephone
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? WXAvgPrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXOpenTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXRecommend
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXSpecial
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXIntroduction
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WXSId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int? WXState
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public long? WXPoiId
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string WxOpenId
		{
			get;
			set;
		}

		public string LatLng
		{
			get
			{
				return (this.Latitude.HasValue && this.Longitude.HasValue) ? (this.Latitude + "," + this.Longitude) : "";
			}
		}

		[FieldType(FieldType.CommonField)]
		public double? ServeRadius
		{
			get;
			set;
		}

		public double Distance
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string FullRegionPath
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string StoreOpenTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime OpenStartDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime OpenEndDate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsSupportExpress
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsAboveSelf
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsStoreDelive
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? StoreFreight
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? MinOrderPrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal CommissionRate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsShelvesProduct
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsModifyPrice
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? MinPriceRate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal? MaxPriceRate
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsRequestBlance
		{
			get;
			set;
		}

		public int NearStoreType
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AlipayAccount
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string AlipayRealName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string BankName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string BankAccountName
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string BankCardNo
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TradePassword
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string TradePasswordSalt
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? CloseBeginTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public DateTime? CloseEndTime
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public decimal Balance
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string StoreSlideImages
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsOnlinePay
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsOfflinePay
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsCashOnDelivery
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Introduce
		{
			get;
			set;
		}
	}
}
