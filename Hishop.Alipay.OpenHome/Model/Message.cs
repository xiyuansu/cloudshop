using Hishop.Alipay.OpenHome.Utility;
using System.Xml.Serialization;

namespace Hishop.Alipay.OpenHome.Model
{
	[XmlRoot("xml")]
	public class Message
	{
		private string toUserId;

		private string agreementId;

		private string appId;

		private string msgType;

		[XmlElement("ToUserId", typeof(CData))]
		public CData ToUserId
		{
			get
			{
				return this.toUserId;
			}
			set
			{
				this.toUserId = value;
			}
		}

		[XmlElement("AgreementId", typeof(CData))]
		public CData AgreementId
		{
			get
			{
				return this.agreementId;
			}
			set
			{
				this.agreementId = value;
			}
		}

		[XmlElement("AppId", typeof(CData))]
		public CData AppId
		{
			get
			{
				return this.appId;
			}
			set
			{
				this.appId = value;
			}
		}

		[XmlElement("CreateTime")]
		public string CreateTime
		{
			get;
			set;
		}

		[XmlElement("MsgType", typeof(CData))]
		public CData MsgType
		{
			get
			{
				return this.msgType;
			}
			set
			{
				this.msgType = value;
			}
		}

		[XmlElement("ArticleCount")]
		public int ArticleCount
		{
			get;
			set;
		}

		[XmlElement("Articles")]
		public Articles Articles
		{
			get;
			set;
		}
	}
}
