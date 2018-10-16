namespace HiShop.API.Setting.Entities
{
	public interface IPaging
	{
		int page
		{
			get;
			set;
		}

		int page_size
		{
			get;
			set;
		}
	}
}
