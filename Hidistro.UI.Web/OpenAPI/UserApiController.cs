using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hishop.Open.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Hidistro.UI.Web.OpenAPI
{
	public class UserApiController : ApiController
	{
		public HttpResponseMessage GetUsers()
		{
			string content = "";
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				sortedDictionary.Add(text, nameValueCollection.Get(text));
			}
			DateTime? start_time = null;
			DateTime? end_time = null;
			int page_no = 0;
			int page_size = 0;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (this.CheckUsersParameters(sortedDictionary, out start_time, out end_time, out page_no, out page_size, out content) && OpenApiSign.CheckSign(sortedDictionary, siteSettings.CheckCode, ref content))
			{
				content = this.lastGetUsers(start_time, end_time, page_no, page_size);
			}
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		public HttpResponseMessage GetUser()
		{
			string content = "";
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string text in allKeys)
			{
				sortedDictionary.Add(text, nameValueCollection.Get(text));
			}
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (this.CheckUserParameters(sortedDictionary, out content) && OpenApiSign.CheckSign(sortedDictionary, siteSettings.CheckCode, ref content))
			{
				content = this.lastGetUser(sortedDictionary["user_name"]);
			}
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpGet]
		public HttpResponseMessage AddUser()
		{
			NameValueCollection nameValueCollection = base.Request.RequestUri.ParseQueryString();
			UserParam userParam = new UserParam();
			if (nameValueCollection.AllKeys.Contains("app_key"))
			{
				userParam.app_key = nameValueCollection["app_key"];
			}
			if (nameValueCollection.AllKeys.Contains("timestamp"))
			{
				userParam.timestamp = nameValueCollection["timestamp"];
			}
			if (nameValueCollection.AllKeys.Contains("sign"))
			{
				userParam.sign = nameValueCollection["sign"];
			}
			if (nameValueCollection.AllKeys.Contains("user_name"))
			{
				userParam.user_name = nameValueCollection["user_name"];
			}
			if (nameValueCollection.AllKeys.Contains("password"))
			{
				userParam.password = nameValueCollection["password"];
			}
			if (nameValueCollection.AllKeys.Contains("created") && !string.IsNullOrEmpty(nameValueCollection["created"]))
			{
				userParam.created = Convert.ToDateTime(nameValueCollection["created"]);
			}
			else
			{
				userParam.created = DateTime.Now;
			}
			if (nameValueCollection.AllKeys.Contains("real_name"))
			{
				userParam.real_name = nameValueCollection["real_name"];
			}
			if (nameValueCollection.AllKeys.Contains("mobile"))
			{
				userParam.mobile = nameValueCollection["mobile"];
			}
			if (nameValueCollection.AllKeys.Contains("email"))
			{
				userParam.email = nameValueCollection["email"];
			}
			if (nameValueCollection.AllKeys.Contains("sex"))
			{
				userParam.sex = nameValueCollection["sex"];
			}
			if (nameValueCollection.AllKeys.Contains("birthday") && !string.IsNullOrEmpty(nameValueCollection["birthday"]))
			{
				userParam.birthday = Convert.ToDateTime(nameValueCollection["birthday"]);
			}
			if (nameValueCollection.AllKeys.Contains("state"))
			{
				userParam.state = nameValueCollection["state"];
			}
			if (nameValueCollection.AllKeys.Contains("city"))
			{
				userParam.city = nameValueCollection["city"];
			}
			if (nameValueCollection.AllKeys.Contains("district"))
			{
				userParam.district = nameValueCollection["district"];
			}
			if (nameValueCollection.AllKeys.Contains("town"))
			{
				userParam.town = nameValueCollection["town"];
			}
			if (nameValueCollection.AllKeys.Contains("address"))
			{
				userParam.address = nameValueCollection["address"];
			}
			string content = this._addUser(userParam);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		[HttpPost]
		public HttpResponseMessage AddUser(UserParam data)
		{
			string content = this._addUser(data);
			return new HttpResponseMessage
			{
				Content = new StringContent(content, Encoding.UTF8, "application/json")
			};
		}

		private string _addUser(UserParam data)
		{
			string result = default(string);
			if (this.CheckAddUserParameters(data, out result))
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string text = OpenApiSign.Sign(data.SignStr(siteSettings.CheckCode), "MD5", "utf-8");
				if (text.Equals(data.sign))
				{
					result = this.lastAddUser(data);
					return result;
				}
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Signature, "sign");
				return result;
			}
			return result;
		}

		private bool CheckUsersParameters(SortedDictionary<string, string> parameters, out DateTime? start_time, out DateTime? end_time, out int page_no, out int page_size, out string result)
		{
			start_time = null;
			end_time = null;
			page_no = 1;
			page_size = 40;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!OpenApiHelper.CheckSystemParameters(parameters, siteSettings.AppKey, out result))
			{
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["start_time"]) && !OpenApiHelper.IsDate(parameters["start_time"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "start_time");
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["end_time"]) && !OpenApiHelper.IsDate(parameters["end_time"]))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Invalid_Timestamp, "end_time");
				return false;
			}
			if (!string.IsNullOrEmpty(parameters["start_time"]))
			{
				DateTime dateTime = default(DateTime);
				DateTime.TryParse(parameters["start_time"], out dateTime);
				start_time = dateTime;
				if (dateTime > DateTime.Now)
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_Now, "start_time and currenttime");
					return false;
				}
				if (!string.IsNullOrEmpty(parameters["end_time"]))
				{
					DateTime dateTime2 = default(DateTime);
					DateTime.TryParse(parameters["end_time"], out dateTime2);
					end_time = dateTime2;
					if (dateTime > dateTime2)
					{
						result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Time_Start_End, "start_time and end_created");
						return false;
					}
				}
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && !int.TryParse(parameters["page_size"].ToString(), out page_size))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_size");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_size"])) && (page_size <= 0 || page_size > 100))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_size");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && !int.TryParse(parameters["page_no"].ToString(), out page_no))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "page_no");
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["page_no"])) && page_no <= 0)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Page_Size_Too_Long, "page_no");
				return false;
			}
			return true;
		}

		private string lastGetUsers(DateTime? start_time, DateTime? end_time, int page_no, int page_size)
		{
			string format = "{{\"users_get_response\":{{\"total_results\":\"{0}\",\"users\":{1}}}}}";
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.PageIndex = page_no;
			memberQuery.PageSize = page_size;
			if (start_time.HasValue)
			{
				memberQuery.StartTime = start_time;
			}
			if (end_time.HasValue)
			{
				memberQuery.EndTime = end_time;
			}
			DbQueryResult members = MemberHelper.GetMembers(memberQuery);
			return string.Format(format, members.TotalRecords, this.ConvertUsers(members.Data));
		}

		private string ConvertUsers(DataTable dt)
		{
			List<user_list_model> list = new List<user_list_model>();
			foreach (DataRow row in dt.Rows)
			{
				user_list_model user_list_model = new user_list_model();
				user_list_model.uid = (int)row["UserId"];
				if (row["UserName"] != DBNull.Value)
				{
					user_list_model.user_name = (string)row["UserName"];
				}
				if (row["RealName"] != DBNull.Value)
				{
					user_list_model.real_name = (string)row["RealName"];
				}
				if (row["Picture"] != DBNull.Value)
				{
					user_list_model.avatar = (string)row["Picture"];
				}
				if (row["BirthDate"] != DBNull.Value)
				{
					user_list_model.birthday = (DateTime)row["BirthDate"];
				}
				if (row["CreateDate"] != DBNull.Value)
				{
					user_list_model.created = (DateTime)row["CreateDate"];
				}
				if (row["Email"] != DBNull.Value)
				{
					user_list_model.email = (string)row["Email"];
				}
				if (row["CellPhone"] != DBNull.Value)
				{
					user_list_model.mobile = (string)row["CellPhone"];
				}
				if (row["Points"] != DBNull.Value)
				{
					user_list_model.points = (int)row["Points"];
				}
				if (row["gender"] != DBNull.Value)
				{
					user_list_model.sex = (((int)row["gender"] == 0) ? "男" : "女");
				}
				if (row["regionId"] != DBNull.Value && (int)row["regionId"] > 0)
				{
					int currentRegionId = (int)row["regionId"];
					string[] array = RegionHelper.GetFullRegion(currentRegionId, "|", true, 0).Split('|');
					if (array.Length == 4)
					{
						user_list_model.state = array[0];
						user_list_model.city = array[1];
						user_list_model.district = array[2];
						user_list_model.town = array[3];
					}
				}
				if (row["Address"] != DBNull.Value)
				{
					user_list_model.address = (string)row["Address"];
				}
				if (row["Balance"] != DBNull.Value)
				{
					user_list_model.trade_amount = (decimal)row["Balance"];
				}
				if (row["OrderNumber"] != DBNull.Value)
				{
					user_list_model.trade_count = (int)row["OrderNumber"];
				}
				list.Add(user_list_model);
			}
			return JsonConvert.SerializeObject(list);
		}

		private bool CheckUserParameters(SortedDictionary<string, string> parameters, out string result)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!OpenApiHelper.CheckSystemParameters(parameters, siteSettings.AppKey, out result))
			{
				return false;
			}
			if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(parameters["user_name"])))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "user_name");
				return false;
			}
			return true;
		}

		private string lastGetUser(string user_name)
		{
			string format = "{{\"user_get_response\":{{\"user\":{0}}}}}";
			MemberInfo member = MemberProcessor.FindMemberByUsername(user_name);
			return string.Format(format, this.ConvertUser(member));
		}

		private string ConvertUser(MemberInfo member)
		{
			string result = "{}";
			if (member != null)
			{
				user_list_model user_list_model = new user_list_model();
				user_list_model.uid = member.UserId;
				user_list_model.user_name = member.UserName;
				user_list_model.real_name = member.RealName;
				user_list_model.avatar = member.Picture;
				user_list_model.birthday = member.BirthDate;
				user_list_model.created = member.CreateDate;
				user_list_model.email = member.Email;
				user_list_model.mobile = member.CellPhone;
				user_list_model.points = member.Points;
				if (member.Gender > Gender.NotSet)
				{
					user_list_model.sex = ((member.Gender == Gender.Male) ? "男" : "女");
				}
				if (member.RegionId > 0)
				{
					string[] array = RegionHelper.GetFullRegion(member.RegionId, "|", true, 0).Split('|');
					if (array.Length == 4)
					{
						user_list_model.state = array[0];
						user_list_model.city = array[1];
						user_list_model.district = array[2];
						user_list_model.town = array[3];
					}
				}
				user_list_model.address = member.Address;
				user_list_model.trade_amount = member.Balance;
				user_list_model.trade_count = member.OrderNumber;
				result = JsonConvert.SerializeObject(user_list_model);
			}
			return result;
		}

		private bool CheckAddUserParameters(UserParam parameter, out string result)
		{
			if (!OpenApiHelper.CheckSystemParameters(parameter.app_key, parameter.timestamp, parameter.sign, out result))
			{
				return false;
			}
			parameter.user_name = DataHelper.CleanSearchString(parameter.user_name);
			if (string.IsNullOrWhiteSpace(parameter.user_name))
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "user_name");
				return false;
			}
			if (MemberProcessor.FindMemberByUsername(parameter.user_name) != null)
			{
				result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "user_name");
				return false;
			}
			Regex regex = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", RegexOptions.Compiled);
			Regex regex2 = new Regex("^0?(13|15|18|14|17)[0-9]{9}$", RegexOptions.Compiled);
			if (!string.IsNullOrWhiteSpace(parameter.email))
			{
				if (!regex.IsMatch(parameter.email))
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "email");
					return false;
				}
				if (MemberProcessor.FindMemberByEmail(parameter.email) != null)
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "email");
					return false;
				}
			}
			if (!string.IsNullOrWhiteSpace(parameter.mobile))
			{
				if (!regex2.IsMatch(parameter.mobile))
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "mobile");
					return false;
				}
				if (MemberProcessor.FindMemberByCellphone(parameter.mobile) != null)
				{
					result = OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.Parameters_Format_Error, "mobile");
					return false;
				}
			}
			return true;
		}

		private string lastAddUser(UserParam parameter)
		{
			string format = "{{\"user_add_response\":{{\"user\":{{ \"uid\":\"{0}\",\"password\":\"{1}\",\"created\":\"{2}\" }} }} }}";
			MemberInfo memberInfo = new MemberInfo();
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			memberInfo.UserName = parameter.user_name;
			memberInfo.RealName = parameter.real_name;
			memberInfo.Email = parameter.email;
			memberInfo.CellPhone = parameter.mobile;
			string text = Globals.RndStr(128, true);
			if (string.IsNullOrWhiteSpace(parameter.password))
			{
				parameter.password = Globals.RndStr(6, true);
			}
			string password = parameter.password;
			password = (memberInfo.Password = Users.EncodePassword(password, text));
			memberInfo.PasswordSalt = text;
			if (parameter.sex.IndexOf("男") >= 0)
			{
				memberInfo.Gender = Gender.Female;
			}
			else if (parameter.sex.IndexOf("女") >= 0)
			{
				memberInfo.Gender = Gender.Female;
			}
			else
			{
				memberInfo.Gender = Gender.NotSet;
			}
			memberInfo.BirthDate = parameter.birthday;
			memberInfo.RegionId = RegionHelper.GetRegionId(parameter.town, parameter.district, parameter.city, parameter.state);
			memberInfo.Address = parameter.address;
			memberInfo.CreateDate = DateTime.Now;
			int num = MemberProcessor.CreateMember(memberInfo);
			if (num > 0)
			{
				return string.Format(format, num, parameter.password, memberInfo.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			return OpenApiErrorMessage.ShowErrorMsg((Enum)(object)OpenApiErrorCode.System_Error, "create user");
		}
	}
}
