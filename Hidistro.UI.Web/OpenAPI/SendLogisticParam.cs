using System.Text;

namespace Hidistro.UI.Web.OpenAPI
{
	public class SendLogisticParam
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

		public string tid
		{
			get;
			set;
		}

		public string company_name
		{
			get;
			set;
		}

		public string out_sid
		{
			get;
			set;
		}

		public string SignStr(string secret)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"app_key{this.app_key}");
			stringBuilder.Append($"company_name{this.company_name}");
			stringBuilder.Append($"out_sid{this.out_sid}");
			stringBuilder.Append($"tid{this.tid}");
			stringBuilder.Append($"timestamp{this.timestamp}");
			stringBuilder.Append(secret);
			return stringBuilder.ToString();
		}
	}
}
