namespace HiShop.API.Setting.Entities
{
	public class WxJsonResult : IWxJsonResult, IJsonResult
	{
		public ReturnCode errcode
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
