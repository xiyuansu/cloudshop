using System.Text;

namespace Hidistro.UI.Web.OpenAPI
{
	public class UpdateTradeMemoParam
	{
		public string app_key
		{
			get;
			set;
		}

		public string timestamp
		{
			get;
			set;
		}

		public string sign
		{
			get;
			set;
		}

		public int flag
		{
			get;
			set;
		}

		public string tid
		{
			get;
			set;
		}

		public string memo
		{
			get;
			set;
		}

		public string SignStr(string secret)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"app_key{this.app_key}");
			if (this.flag > 0)
			{
				stringBuilder.Append($"flag{this.flag}");
			}
			if (!string.IsNullOrWhiteSpace(this.memo))
			{
				stringBuilder.Append($"memo{this.memo}");
			}
			stringBuilder.Append($"tid{this.tid}");
			stringBuilder.Append($"timestamp{this.timestamp}");
			stringBuilder.Append(secret);
			return stringBuilder.ToString();
		}
	}
}
