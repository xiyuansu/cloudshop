namespace Hishop.Alipay.OpenHome.Request
{
	public class GetMenuRequest : IRequest
	{
		public string GetMethodName()
		{
			return "alipay.mobile.public.menu.get";
		}

		public string GetBizContent()
		{
			return null;
		}
	}
}
