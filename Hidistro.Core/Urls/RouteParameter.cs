using System.Configuration;

namespace Hidistro.Core.Urls
{
	public class RouteParameter : ConfigurationElement
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

		[ConfigurationProperty("value", IsRequired = false)]
		public string Value
		{
			get
			{
				return (string)base["value"];
			}
			set
			{
				base["value"] = value;
			}
		}

		[ConfigurationProperty("constraint", IsRequired = true)]
		public string Constraint
		{
			get
			{
				return (string)base["constraint"];
			}
			set
			{
				base["constraint"] = value;
			}
		}
	}
}
