using HiShop.API.Setting.Entities;
using System;

namespace HiShop.API.Setting.Exceptions
{
	public class ErrorJsonResultException : WeixinException
	{
		public WxJsonResult JsonResult
		{
			get;
			set;
		}

		public ErrorJsonResultException(string message, Exception inner, WxJsonResult jsonResult)
			: base(message, inner)
		{
			this.JsonResult = jsonResult;
		}
	}
}
