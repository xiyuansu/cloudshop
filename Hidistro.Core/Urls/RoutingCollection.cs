using System.Configuration;

namespace Hidistro.Core.Urls
{
	public class RoutingCollection : ConfigurationElementCollection
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
				return "route";
			}
		}

		public RoutingItem this[int index]
		{
			get
			{
				return (RoutingItem)base.BaseGet(index);
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
			return new RoutingItem();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RoutingItem)element).Name;
		}
	}
}
