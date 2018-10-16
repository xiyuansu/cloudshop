using System.Configuration;

namespace Hidistro.Core.Urls
{
	public class RouteParameterCollection : ConfigurationElementCollection
	{
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		protected override string ElementName
		{
			get
			{
				return "add";
			}
		}

		public RouteParameter this[int index]
		{
			get
			{
				return (RouteParameter)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new RouteParameter();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RouteParameter)element).Name;
		}
	}
}
