namespace Hidistro.Entities.Statistics
{
	public class OrderAmountDistributionModel
	{
		public int OneCount
		{
			get;
			set;
		}

		public string OneName
		{
			get
			{
				return "0-50元";
			}
		}

		public int TwoCount
		{
			get;
			set;
		}

		public string TwoName
		{
			get
			{
				return "51-100元";
			}
		}

		public int ThreeCount
		{
			get;
			set;
		}

		public string ThreeName
		{
			get
			{
				return "101-200元";
			}
		}

		public int FourCount
		{
			get;
			set;
		}

		public string FourName
		{
			get
			{
				return "201-500元";
			}
		}

		public int FiveCount
		{
			get;
			set;
		}

		public string FiveName
		{
			get
			{
				return "501-1000元";
			}
		}

		public int SixCount
		{
			get;
			set;
		}

		public string SixName
		{
			get
			{
				return "1001-5000元";
			}
		}

		public int SevenCount
		{
			get;
			set;
		}

		public string SevenName
		{
			get
			{
				return "5001-10000元";
			}
		}

		public int EightCount
		{
			get;
			set;
		}

		public string EightName
		{
			get
			{
				return "10001元以上";
			}
		}
	}
}
