namespace Hidistro.Entities.Depot
{
	public class MapGetAddressByLatLngResult
	{
		public int status
		{
			get;
			set;
		}

		public string message
		{
			get;
			set;
		}

		public AddressByLatLng result
		{
			get;
			set;
		}
	}
}
