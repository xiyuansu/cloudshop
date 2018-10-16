using Newtonsoft.Json;

namespace Hidistro.Entities.Members
{
	public class PositionInfo
	{
		public double Latitude
		{
			get;
			set;
		}

		public double Longitude
		{
			get;
			set;
		}

		[JsonIgnore]
		public int CityId
		{
			get;
			set;
		}

		public int AreaId
		{
			get;
			set;
		}

		public PositionInfo(double lat, double lon)
		{
			this.Latitude = lat;
			this.Longitude = lon;
		}
	}
}
