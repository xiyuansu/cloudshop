namespace Hishop.Plugins.Refund
{
	public class ResponseResult
	{
		public ResponseStatus Status
		{
			get;
			set;
		}

		public string Code
		{
			get;
			set;
		}

		public string Msg
		{
			get;
			set;
		}

		public string SubCode
		{
			get;
			set;
		}

		public string SubMsg
		{
			get;
			set;
		}

		public string TradeNo
		{
			get;
			set;
		}

		public string OriginalResult
		{
			get;
			set;
		}

		public decimal RefundCharge
		{
			get;
			set;
		}

		public string OutTradeNo
		{
			get;
			set;
		}
	}
}
