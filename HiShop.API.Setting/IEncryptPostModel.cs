namespace HiShop.API.Setting
{
	public interface IEncryptPostModel
	{
		string Signature
		{
			get;
			set;
		}

		string Msg_Signature
		{
			get;
			set;
		}

		string Timestamp
		{
			get;
			set;
		}

		string Nonce
		{
			get;
			set;
		}

		string Token
		{
			get;
			set;
		}

		string EncodingAESKey
		{
			get;
			set;
		}
	}
}
