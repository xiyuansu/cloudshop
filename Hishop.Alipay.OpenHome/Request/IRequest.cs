namespace Hishop.Alipay.OpenHome.Request
{
	public interface IRequest
	{
		string GetMethodName();

		string GetBizContent();
	}
}
