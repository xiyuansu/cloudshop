using System;
using System.Xml.Serialization;

namespace Hishop.Alipay.OpenHome.Model
{
	[Serializable]
	[XmlRoot("XML")]
	internal class AliRequstWhenFollow : AliRequest
	{
		[XmlElement("UserInfo")]
		public UserInfo UserInfo
		{
			get;
			set;
		}
	}
}
