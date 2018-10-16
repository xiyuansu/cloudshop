using System.Configuration;

namespace Hidistro.Core.Urls
{
	public class RoutingItem : ConfigurationElement
	{
		[ConfigurationProperty("name", IsKey = true, IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)base["name"];
			}
			set
			{
				base["name"] = value;
			}
		}

		[ConfigurationProperty("url", IsRequired = true)]
		public string Url
		{
			get
			{
				return (string)base["url"];
			}
			set
			{
				base["url"] = value;
			}
		}

		[ConfigurationProperty("dest", IsRequired = true)]
		public string Dest
		{
			get
			{
				return (string)base["dest"];
			}
			set
			{
				base["dest"] = value;
			}
		}

		[ConfigurationProperty("parameters", IsDefaultCollection = true)]
		public RouteParameterCollection Parameters
		{
			get
			{
				return (RouteParameterCollection)base["parameters"];
			}
		}
	}
}
