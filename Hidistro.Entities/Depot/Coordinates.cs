namespace Hidistro.Entities.Depot
{
	public class Coordinates
	{
		public string lat
		{
			get;
			set;
		}

		public string lng
		{
			get;
			set;
		}

		public string LatLng
		{
			get
			{
				return this.lat + "," + this.lng;
			}
		}

		public Location location
		{
			get;
			set;
		}
	}
}
