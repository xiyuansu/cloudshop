using System.Text;

namespace Hidistro.UI.Web.OpenAPI
{
	public class StoreInfoParam
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

		public int storeId
		{
			get;
			set;
		}

		public string SignStr(string secret)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"app_key{this.app_key}");
			stringBuilder.Append($"storeid{this.storeId}");
			stringBuilder.Append($"timestamp{this.timestamp}");
			stringBuilder.Append(secret);
			return stringBuilder.ToString();
		}
	}
}
