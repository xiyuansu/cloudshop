namespace Hidistro.Entities.Depot
{
	public class QQMapResult
	{
		public Location location
		{
			get;
			set;
		}

		public string address
		{
			get;
			set;
		}

		public FormattedAddress formatted_address
		{
			get;
			set;
		}

		public address_component address_component
		{
			get;
			set;
		}

		public ad_info ad_info
		{
			get;
			set;
		}
	}
}
