using HiShop.API.HiPOS.Entities;
using System;

namespace Hishop.API.HiPOS.CommonAPIs
{
	internal class AccessTokenBag
	{
		public object Lock = new object();

		public string AppId
		{
			get;
			set;
		}

		public string AppSecret
		{
			get;
			set;
		}

		public DateTime ExpireTime
		{
			get;
			set;
		}

		public AccessTokenResult AccessTokenResult
		{
			get;
			set;
		}
	}
}
