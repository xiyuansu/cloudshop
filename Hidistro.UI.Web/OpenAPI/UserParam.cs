using System;
using System.Text;

namespace Hidistro.UI.Web.OpenAPI
{
	public class UserParam
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

		public string user_name
		{
			get;
			set;
		}

		public string password
		{
			get;
			set;
		}

		public DateTime created
		{
			get;
			set;
		}

		public string real_name
		{
			get;
			set;
		}

		public string mobile
		{
			get;
			set;
		}

		public string email
		{
			get;
			set;
		}

		public string sex
		{
			get;
			set;
		}

		public DateTime? birthday
		{
			get;
			set;
		}

		public string state
		{
			get;
			set;
		}

		public string city
		{
			get;
			set;
		}

		public string district
		{
			get;
			set;
		}

		public string town
		{
			get;
			set;
		}

		public string address
		{
			get;
			set;
		}

		public string SignStr(string secret)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(this.address))
			{
				stringBuilder.Append($"address{this.address}");
			}
			stringBuilder.Append($"app_key{this.app_key}");
			DateTime dateTime;
			if (this.birthday.HasValue)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				dateTime = this.birthday.Value;
				stringBuilder2.Append(string.Format("birthday{0}", dateTime.ToString("yyyy-MM-dd HH:mm:ss")));
			}
			if (!string.IsNullOrWhiteSpace(this.city))
			{
				stringBuilder.Append($"city{this.city}");
			}
			if (this.created > DateTime.MinValue)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				dateTime = this.created;
				stringBuilder3.Append(string.Format("created{0}", dateTime.ToString("yyyy-MM-dd HH:mm:ss")));
			}
			if (!string.IsNullOrWhiteSpace(this.district))
			{
				stringBuilder.Append($"district{this.district}");
			}
			if (!string.IsNullOrWhiteSpace(this.email))
			{
				stringBuilder.Append($"email{this.email}");
			}
			if (!string.IsNullOrWhiteSpace(this.mobile))
			{
				stringBuilder.Append($"mobile{this.mobile}");
			}
			if (!string.IsNullOrWhiteSpace(this.password))
			{
				stringBuilder.Append($"password{this.password}");
			}
			if (!string.IsNullOrWhiteSpace(this.real_name))
			{
				stringBuilder.Append($"real_name{this.real_name}");
			}
			if (!string.IsNullOrWhiteSpace(this.sex))
			{
				stringBuilder.Append($"sex{this.sex}");
			}
			if (!string.IsNullOrWhiteSpace(this.state))
			{
				stringBuilder.Append($"state{this.state}");
			}
			stringBuilder.Append($"timestamp{this.timestamp}");
			if (!string.IsNullOrWhiteSpace(this.town))
			{
				stringBuilder.Append($"town{this.town}");
			}
			stringBuilder.Append($"user_name{this.user_name}");
			stringBuilder.Append(secret);
			return stringBuilder.ToString();
		}
	}
}
