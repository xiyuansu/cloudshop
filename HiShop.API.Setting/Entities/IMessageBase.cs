using System;

namespace HiShop.API.Setting.Entities
{
	public interface IMessageBase
	{
		string ToUserName
		{
			get;
			set;
		}

		string FromUserName
		{
			get;
			set;
		}

		DateTime CreateTime
		{
			get;
			set;
		}
	}
}
