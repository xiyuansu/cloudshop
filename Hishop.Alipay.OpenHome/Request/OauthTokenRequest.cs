using System;

namespace Hishop.Alipay.OpenHome.Request
{
	public class OauthTokenRequest : IRequest
	{
		private string token;

		public string GetMethodName()
		{
			return "alipay.system.oauth.token";
		}

		public void SetToken(string token)
		{
			this.token = token;
		}

		public string GetBizContent()
		{
			throw new NotImplementedException();
		}
	}
}
