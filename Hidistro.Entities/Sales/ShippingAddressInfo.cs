using Hidistro.Core;

namespace Hidistro.Entities.Sales
{
	[TableName("Hishop_UserShippingAddresses")]
	public class ShippingAddressInfo
	{
		[FieldType(FieldType.IncrementField)]
		[FieldType(FieldType.KeyField)]
		public int ShippingId
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

		[FieldType(FieldType.KeyField)]
		public int UserId
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string ShipTo
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string Address
		{
			get;
			set;
		}

		public string FullAddress
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string Zipcode
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string TelPhone
		{
			get;
			set;
		}

		[HtmlCoding]
		[FieldType(FieldType.CommonField)]
		public string CellPhone
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public bool IsDefault
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string LatLng
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
		public string RegionLocation
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string IDNumber
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string IDImage1
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string IDImage2
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public int IDStatus
		{
			get;
			set;
		}

		[FieldType(FieldType.CommonField)]
		public string BuildingNumber
		{
			get;
			set;
		}
	}
}
