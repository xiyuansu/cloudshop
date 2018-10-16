using System;

namespace Hidistro.Entities.Members
{
	public class CommissionRequestModel
	{
		public string ReferralUserName
		{
			get;
			set;
		}

		public DateTime RequestDate
		{
			get;
			set;
		}

		public decimal Amount
		{
			get;
			set;
		}

		public string ManagerUserName
		{
			get;
			set;
		}

		public string ManagerRemark
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public string AccountInfo
		{
			get
			{
				string text = "";
				if (this.IsWeixin)
				{
					return "提现到微信";
				}
				if (this.IsAlipay)
				{
					return "提现到支付宝(支付宝帐号:" + this.AlipayCode + "，支付宝姓名:" + this.AlipayRealName + ")";
				}
				if (this.IsWithdrawToAccount)
				{
					return "提现到预付款帐号";
				}
				return $"提现到银行卡(开户银行:{this.BankName}，银行开户名:{this.AccountName}，银行卡帐号:{this.MerchantCode})";
			}
		}

		public bool IsWeixin
		{
			get;
			set;
		}

		public bool IsAlipay
		{
			get;
			set;
		}

		public string AlipayRealName
		{
			get;
			set;
		}

		public string AlipayCode
		{
			get;
			set;
		}

		public bool IsWithdrawToAccount
		{
			get;
			set;
		}

		public string AccountName
		{
			get;
			set;
		}

		public string BankName
		{
			get;
			set;
		}

		public string MerchantCode
		{
			get;
			set;
		}

		public decimal Balance
		{
			get;
			set;
		}
	}
}
