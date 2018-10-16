using Hishop.Alipay.OpenHome.Model;
using System;

namespace Hishop.Alipay.OpenHome.Response
{
	[Serializable]
	public class MenuUpdateResponse : AliResponse, IAliResponseStatus
	{
		public AliResponseMessage alipay_mobile_public_menu_update_response
		{
			get;
			set;
		}

		public string Code
		{
			get
			{
				return this.alipay_mobile_public_menu_update_response.code;
			}
		}

		public string Message
		{
			get
			{
				return this.alipay_mobile_public_menu_update_response.msg;
			}
		}
	}
}
