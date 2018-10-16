namespace HiShop.API.Setting.Entities
{
	public interface IJsonResult
	{
		string errmsg
		{
			get;
			set;
		}

		object P2PData
		{
			get;
			set;
		}
	}
}
