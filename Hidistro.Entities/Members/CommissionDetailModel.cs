using System;

namespace Hidistro.Entities.Members
{
	public class CommissionDetailModel
	{
		public string ReferalUserName
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string FromUserName
		{
			get;
			set;
		}

		public string TradeDateStr
		{
			get
			{
				if (this.SplittingType == SplittingTypes.RegReferralDeduct)
				{
					return "";
				}
				DateTime tradeDate = this.TradeDate;
				return (this.TradeDate == DateTime.MinValue) ? "" : this.TradeDate.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}

		public DateTime TradeDate
		{
			get;
			set;
		}

		public string FinishDateStr
		{
			get
			{
				DateTime dateTime;
				if (this.SplittingType == SplittingTypes.RegReferralDeduct)
				{
					DateTime tradeDate = this.TradeDate;
					object result;
					if (!(this.TradeDate == DateTime.MinValue))
					{
						dateTime = this.TradeDate;
						result = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					else
					{
						result = "";
					}
					return (string)result;
				}
				DateTime finishDate = this.FinishDate;
				object result2;
				if (!(this.FinishDate == DateTime.MinValue))
				{
					dateTime = this.FinishDate;
					result2 = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				}
				else
				{
					result2 = "";
				}
				return (string)result2;
			}
		}

		public DateTime FinishDate
		{
			get;
			set;
		}

		public decimal OrderTotal
		{
			get;
			set;
		}

		public decimal Commission
		{
			get
			{
				if (this.SplittingType == SplittingTypes.DrawRequest)
				{
					return this.Expenses;
				}
				return this.Income;
			}
		}

		public SplittingTypes SplittingType
		{
			get;
			set;
		}

		public string SplittingTypeText
		{
			get
			{
				return EnumDescription.GetEnumDescription((Enum)(object)this.SplittingType, 0);
			}
		}

		public decimal Income
		{
			get;
			set;
		}

		public decimal Expenses
		{
			get;
			set;
		}
	}
}
