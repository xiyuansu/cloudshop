using System.Text;

namespace Hidistro.UI.Web.OpenAPI
{
	public class ProductQuantityParam
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

		public string sku_id
		{
			get;
			set;
		}

		public int quantity
		{
			get;
			set;
		}

		public int type
		{
			get;
			set;
		}

		public string SignStr(string secret)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append($"app_key{this.app_key}");
			stringBuilder.Append($"num_iid{this.num_iid}");
			stringBuilder.Append($"quantity{this.quantity}");
			if (!string.IsNullOrWhiteSpace(this.sku_id))
			{
				stringBuilder.Append($"sku_id{this.sku_id}");
			}
			stringBuilder.Append($"timestamp{this.timestamp}");
			if (this.type > 0)
			{
				stringBuilder.Append($"type{this.type}");
			}
			stringBuilder.Append(secret);
			return stringBuilder.ToString();
		}
	}
}
