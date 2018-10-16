using System;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.Entities.Depot
{
	public class StoresModel
	{
		private IList<StoreTagInfo> _Tages;

		private IList<int> _TagIds;

		public int StoreId
		{
			get;
			set;
		}

		public int ManagerId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public int RoleId
		{
			get;
			set;
		}

		public DateTime CreateDate
		{
			get;
			set;
		}

		public string HeadImage
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public int TopRegionId
		{
			get;
			set;
		}

		public int RegionId
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public string ContactMan
		{
			get;
			set;
		}

		public string Tel
		{
			get;
			set;
		}

		public int State
		{
			get;
			set;
		}

		public bool CloseStatus
		{
			get;
			set;
		}

		public double? Longitude
		{
			get;
			set;
		}

		public double? Latitude
		{
			get;
			set;
		}

		public string StoreImages
		{
			get;
			set;
		}

		public string WXStateName
		{
			get;
			set;
		}

		public long? WXPoiId
		{
			get;
			set;
		}

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

		public string FullRegionPath
		{
			get;
			set;
		}

		public string StoreOpenTime
		{
			get;
			set;
		}

		public DateTime OpenStartDate
		{
			get;
			set;
		}

		public DateTime OpenEndDate
		{
			get;
			set;
		}

		public bool IsSupportExpress
		{
			get;
			set;
		}

		public bool IsAboveSelf
		{
			get;
			set;
		}

		public bool IsStoreDelive
		{
			get;
			set;
		}

		public decimal? StoreFreight
		{
			get;
			set;
		}

		public decimal? MinOrderPrice
		{
			get;
			set;
		}

		public decimal CommissionRate
		{
			get;
			set;
		}

		public bool IsShelvesProduct
		{
			get;
			set;
		}

		public bool IsModifyPrice
		{
			get;
			set;
		}

		public decimal? MinPriceRate
		{
			get;
			set;
		}

		public decimal? MaxPriceRate
		{
			get;
			set;
		}

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

		public string AlipayAccount
		{
			get;
			set;
		}

		public string AlipayRealName
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

		public string BankCardNo
		{
			get;
			set;
		}

		public DateTime? CloseBeginTime
		{
			get;
			set;
		}

		public DateTime? CloseEndTime
		{
			get;
			set;
		}

		public decimal Balance
		{
			get;
			set;
		}

		public bool IsOnlinePay
		{
			get;
			set;
		}

		public bool IsOfflinePay
		{
			get;
			set;
		}

		public IList<StoreTagInfo> Tags
		{
			get
			{
				if (this._Tages == null)
				{
					this._Tages = new List<StoreTagInfo>();
				}
				return this._Tages;
			}
		}

		public IList<int> TagIds
		{
			get
			{
				if (this._TagIds == null)
				{
					this._TagIds = new List<int>();
				}
				return this._TagIds;
			}
		}

		public string DeliveryTypes
		{
			get
			{
				string text = "";
				if (this.IsAboveSelf)
				{
					text += "上门自提,";
				}
				if (this.IsStoreDelive)
				{
					text += "门店配送,";
				}
				if (this.IsSupportExpress)
				{
					text += "快递配送,";
				}
				return text.TrimEnd(',');
			}
		}

		public string TagsName
		{
			get
			{
				if (this.Tags != null && this.Tags.Count > 0)
				{
					return string.Join(",", (from t in this.Tags
					select t.TagName).ToArray());
				}
				return "";
			}
		}
	}
}
