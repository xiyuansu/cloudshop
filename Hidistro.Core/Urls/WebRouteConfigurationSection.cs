using System.Configuration;

namespace Hidistro.Core.Urls
{
	public class WebRouteConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("map", IsRequired = false)]
		public RoutingCollection Map
		{
			get
			{
				return (RoutingCollection)base["map"];
			}
		}
	}
}
