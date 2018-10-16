namespace Hishop.Alipay.OpenHome.Handle
{
	internal interface IHandle
	{
		string LocalRsaPriKey
		{
			get;
			set;
		}

		string LocalRsaPubKey
		{
			get;
			set;
		}

		string AliRsaPubKey
		{
			get;
			set;
		}

		AlipayOHClient client
		{
			get;
			set;
		}

		string Handle(string requestContent);
	}
}
