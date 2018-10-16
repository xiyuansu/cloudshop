using System;

namespace Hishop.Open.Api
{
	public interface IUser
	{
		string GetUsers(DateTime? start_time, DateTime? end_time, int page_no, int page_size);

		string GetUser(string user_name);

		string AddUser(string user_name, string password, DateTime created, string real_name, string mobile, string email, string sex, DateTime? birthday, string state, string city, string district, string town, string address);
	}
}
