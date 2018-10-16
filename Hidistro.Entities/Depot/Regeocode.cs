namespace Hidistro.Entities.Depot
{
	public class Regeocode
	{
		public string formatted_address
		{
			get;
			set;
		}

		public AddressComponent addressComponent
		{
			get;
			set;
		}
	}
}
