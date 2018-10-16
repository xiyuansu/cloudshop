namespace HiShop.API.Setting.Entities
{
	public class QyJsonResult : IJsonResult
	{
		public ReturnCode_QY errcode
		{
			get;
			set;
		}

		public string errmsg
		{
			get;
			set;
		}

		public virtual object P2PData
		{
			get;
			set;
		}
	}
}
