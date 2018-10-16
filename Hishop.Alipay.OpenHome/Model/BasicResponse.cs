using System.Xml.Serialization;

namespace Hishop.Alipay.OpenHome.Model
{
	public class BasicResponse
	{
		[XmlElement("code")]
		public string ErrCode
		{
			get;
			set;
		}

		[XmlElement("msg")]
		public string ErrMsg
		{
			get;
			set;
		}

		[XmlElement("sub_code")]
		public string SubErrCode
		{
			get;
			set;
		}

		[XmlElement("sub_msg")]
		public string SubErrMsg
		{
			get;
			set;
		}

		public string Body
		{
			get;
			set;
		}

		public bool IsError
		{
			get
			{
				return !string.IsNullOrEmpty(this.SubErrCode);
			}
		}
	}
}
