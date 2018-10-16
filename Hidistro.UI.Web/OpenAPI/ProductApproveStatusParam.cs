using System.Text;

namespace Hidistro.UI.Web.OpenAPI
{
	public class ProductApproveStatusParam
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

		public int num_iid
		{
			get;
			set;
		}

		public string approve_status
		{
			get;
			set;
		}

		public string SignStr(string secret)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"app_key{this.app_key}");
			stringBuilder.Append($"approve_status{this.approve_status}");
			stringBuilder.Append($"num_iid{this.num_iid}");
			stringBuilder.Append($"timestamp{this.timestamp}");
			stringBuilder.Append(secret);
			return stringBuilder.ToString();
		}
	}
}
