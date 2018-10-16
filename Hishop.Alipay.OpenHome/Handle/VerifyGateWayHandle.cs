using Aop.Api.Util;
using Hishop.Alipay.OpenHome.Utility;

namespace Hishop.Alipay.OpenHome.Handle
{
	internal class VerifyGateWayHandle : IHandle
	{
		public string LocalRsaPriKey
		{
			get;
			set;
		}

		public string LocalRsaPubKey
		{
			get;
			set;
		}

		public string AliRsaPubKey
		{
			get;
			set;
		}

		public AlipayOHClient client
		{
			get;
			set;
		}

		public string Handle(string requestContent)
		{
			string text = $"<success>true</success><biz_content>{RsaFileHelper.GetRSAKeyContent(this.LocalRsaPubKey, true)}</biz_content>";
			return AlipaySignature.encryptAndSign(text, this.AliRsaPubKey, this.LocalRsaPriKey, "UTF-8", false, true, true);
		}
	}
}
