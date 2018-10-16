using System.Xml.Serialization;

namespace Hishop.Alipay.OpenHome.Model
{
	[XmlRoot("XML")]
	public class AliRequest
	{
		[XmlElement("AppId")]
		public string AppId
		{
			get;
			set;
		}

		[XmlElement("FromUserId")]
		public string FromUserId
		{
			get;
			set;
		}

		[XmlElement("CreateTime")]
		public long CreateTime
		{
			get;
			set;
		}

		[XmlElement("MsgType")]
		public string MsgType
		{
			get;
			set;
		}

		[XmlElement("EventType")]
		public string EventType
		{
			get;
			set;
		}

		[XmlElement("ActionParam")]
		public string ActionParam
		{
			get;
			set;
		}

		[XmlElement("AgreementId")]
		public string AgreementId
		{
			get;
			set;
		}

		[XmlElement("AccountNo")]
		public string AccountNo
		{
			get;
			set;
		}
	}
}
