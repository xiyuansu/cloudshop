namespace HiShop.API.Setting.Entities
{
	public interface IWxJsonResult : IJsonResult
	{
		ReturnCode errcode
		{
			get;
			set;
		}
	}
}
