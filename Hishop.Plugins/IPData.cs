namespace Hishop.Plugins
{
	public class IPData
	{
		public string Country
		{
			get;
			set;
		}

		public string Province
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string Area
		{
			get;
			set;
		}

		public string CityCountry
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public string FullAddress
		{
			get
			{
				return this.Country + (string.IsNullOrEmpty(this.Country) ? "" : " ") + this.Province + (string.IsNullOrEmpty(this.Province) ? "" : " ") + this.City + (string.IsNullOrEmpty(this.City) ? "" : " ") + this.CityCountry + (string.IsNullOrEmpty(this.CityCountry) ? "" : " ") + this.Address;
			}
		}

		public string IPAddress
		{
			get;
			set;
		}

		public string ISP
		{
			get;
			set;
		}
	}
}
