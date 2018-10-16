namespace HiShop.API.Setting
{
	public class EncryptPostModel : IEncryptPostModel
	{
		public string Signature
		{
			get;
			set;
		}

		public string Msg_Signature
		{
			get;
			set;
		}

		public string Timestamp
		{
			get;
			set;
		}

		public string Nonce
		{
			get;
			set;
		}

		public string Token
		{
			get;
			set;
		}

		public string EncodingAESKey
		{
			get;
			set;
		}

		public virtual void SetSecretInfo(string token, string encodingAESKey)
		{
			this.Token = token;
			this.EncodingAESKey = encodingAESKey;
		}
	}
}
