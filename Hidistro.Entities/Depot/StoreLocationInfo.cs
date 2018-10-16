namespace Hidistro.Entities.Depot
{
	public class StoreLocationInfo
	{
		public int StoreId
		{
			get;
			set;
		}

		public string StoreName
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

		public string Longitude
		{
			get;
			set;
		}

		public string Latitude
		{
			get;
			set;
		}

		public double Distances
		{
			get;
			set;
		}

		public string LatLng
		{
			get
			{
				return (this.Latitude != null && this.Longitude != null) ? (this.Latitude + "," + this.Longitude) : "";
			}
		}
	}
}
