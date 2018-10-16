using Hishop.Alipay.OpenHome.Utility;
using System.Xml.Serialization;

namespace Hishop.Alipay.OpenHome.Model
{
	[XmlRoot("Item")]
	public class Item
	{
		private string title;

		private string description;

		private string imageUrl;

		private string url;

		[XmlElement("Title", typeof(CData))]
		public CData Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
			}
		}

		[XmlElement("Desc", typeof(CData))]
		public CData Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		[XmlElement("ImageUrl", typeof(CData))]
		public CData ImageUrl
		{
			get
			{
				return this.imageUrl;
			}
			set
			{
				this.imageUrl = value;
			}
		}

		[XmlElement("Url", typeof(CData))]
		public CData Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}
	}
}
