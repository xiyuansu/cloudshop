using System;

namespace HiShop.API.Setting.Entities
{
	public class MessageBase : IMessageBase
	{
		public string ToUserName
		{
			get;
			set;
		}

		public string FromUserName
		{
			get;
			set;
		}

		public DateTime CreateTime
		{
			get;
			set;
		}
	}
}
