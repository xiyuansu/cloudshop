using HiShop.API.Setting;

namespace SHiShop.API.HiPOS.Entities.Request
{
	public class PostModel : EncryptPostModel
	{
		public string AppId
		{
			get;
			set;
		}

		public void SetSecretInfo(string token, string encodingAESKey, string appId)
		{
			base.Token = token;
			base.EncodingAESKey = encodingAESKey;
			this.AppId = appId;
		}
	}
}
