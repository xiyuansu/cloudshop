namespace Hishop.Weixin.Pay
{
	public class PayAccount
	{
		public string Sub_OpenId
		{
			get;
			set;
		}

		public string Sub_AppId
		{
			get;
			set;
		}

		public string AppId
		{
			get;
			set;
		}

		public string AppSecret
		{
			get;
			set;
		}

		public string PartnerId
		{
			get;
			set;
		}

		public string PartnerKey
		{
			get;
			set;
		}

		public string PaySignKey
		{
			get;
			set;
		}

		public string sub_mch_id
		{
			get;
			set;
		}

		public PayAccount()
		{
		}

		public PayAccount(string appId, string appSecret, string partnerId, string partnerKey, string paySignKey, string sub_mch_id = "", string sub_AppId = "", string sub_OpenId = "")
		{
			this.AppId = appId;
			this.AppSecret = appSecret;
			this.PartnerId = partnerId;
			this.PartnerKey = partnerKey;
			this.PaySignKey = paySignKey;
			this.sub_mch_id = sub_mch_id;
			this.Sub_AppId = sub_AppId;
			this.Sub_OpenId = sub_OpenId;
		}
	}
}
