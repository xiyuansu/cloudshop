using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.Core
{
	public static class DataHelper
	{
		private static Regex RegPhone = new Regex("^[0-9]+[-]?[0-9]+[-]?[0-9]$", RegexOptions.IgnoreCase);

		private static Regex RegNumber = new Regex("^[0-9]+$");

		private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");

		private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");

		private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");

		private static Regex RegEmail = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$");

		private static Regex RegCHZN = new Regex("[一-龥]");

		private static Regex RegUrl = new Regex("http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?", RegexOptions.IgnoreCase);

		private static Regex RegZipCode = new Regex("\\d{6}");

		private static Regex RegIP = new Regex("^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");

		private static Regex RegNormalchar = new Regex("[\\w\\d_]+", RegexOptions.IgnoreCase);

		private static Regex RegChinese = new Regex("^[\\u4e00-\\u9fa5]+$", RegexOptions.IgnoreCase);

		private static Regex RegUserName = new Regex("[\\u4e00-\\u9fa5a-zA-Z0-9]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*");

		private static Regex RegMobile = new Regex("^0?(13|15|18|14|17|19|16)[0-9]{9}$");

		public static string GetSafeDateTimeFormat(DateTime date)
		{
			return date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.SortableDateTimePattern, CultureInfo.InvariantCulture);
		}

		public static string DateComparerString(int dateComparer)
		{
			switch (dateComparer)
			{
			case -1:
				return "<";
			case 0:
				return "=";
			case 1:
				return ">";
			default:
				return "=";
			}
		}

		public static string GetParticipleSearchSql(string keyword, string field)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = Regex.Split(keyword.ToNullString().Trim(), "\\s+");
			int num = 0;
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (num > 0)
					{
						stringBuilder.Append(" OR ");
					}
					else
					{
						stringBuilder.Append(" (");
					}
					stringBuilder.AppendFormat("{0} LIKE '%{1}%'", field, DataHelper.CleanSearchString(text));
					num++;
				}
			}
			if (stringBuilder.ToString().Length > 0)
			{
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		public static string CleanSearchString(string searchString)
		{
			if (string.IsNullOrEmpty(searchString))
			{
				return "";
			}
			searchString = searchString.Replace("*", "%");
			searchString = Globals.StripHtmlXmlTags(searchString);
			searchString = Regex.Replace(searchString, "--|;|'|\"", " ", RegexOptions.Multiline | RegexOptions.Compiled);
			searchString = Regex.Replace(searchString, " {1,}", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			return searchString;
		}

		public static DataTable ConverDataReaderToDataTable(IDataReader reader)
		{
			if (reader == null)
			{
				return null;
			}
			DataTable dataTable = new DataTable();
			dataTable.Locale = CultureInfo.InvariantCulture;
			int fieldCount = reader.FieldCount;
			for (int i = 0; i < fieldCount; i++)
			{
				dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
			}
			while (reader.Read())
			{
				DataRow dataRow = dataTable.NewRow();
				for (int j = 0; j < fieldCount; j++)
				{
					if (DBNull.Value != ((IDataRecord)reader)[j])
					{
						dataRow[j] = ((IDataRecord)reader)[j].ToNullString();
					}
				}
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}

		private static string ReplaceImageServerUrl(string content)
		{
			string imageServerUrl = Globals.GetImageServerUrl();
			if (!string.IsNullOrEmpty(imageServerUrl))
			{
				if (content.ToLower().StartsWith("/storage/"))
				{
					content = content.ToLower().Replace("/storage/", imageServerUrl + "/storage/");
				}
				content = content.Replace(" src=\"/storage/", " src=\"" + imageServerUrl + "/storage/").Replace(" src='/storage/", " src='" + imageServerUrl + "/storage/").Replace(" href=\"/storage/", " href=\"" + imageServerUrl + "/storage/")
					.Replace(" href='/storage/", " href='" + imageServerUrl + "/storage/");
			}
			return content;
		}

		public static T ReaderToModel<T>(IDataReader objReader) where T : new()
		{
			if (objReader?.Read() ?? false)
			{
				Type typeFromHandle = typeof(T);
				int fieldCount = objReader.FieldCount;
				T val = new T();
				for (int i = 0; i < fieldCount; i++)
				{
					if (!DataHelper.IsNullOrDBNull(((IDataRecord)objReader)[i]))
					{
						PropertyInfo property = typeFromHandle.GetProperty(objReader.GetName(i).Replace("_", ""), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
						if (property != (PropertyInfo)null)
						{
							Type type = property.PropertyType;
							if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
							{
								NullableConverter nullableConverter = new NullableConverter(type);
								type = nullableConverter.UnderlyingType;
							}
							if (type.IsEnum)
							{
								object value = Enum.ToObject(type, ((IDataRecord)objReader)[i]);
								property.SetValue(val, value, null);
							}
							else
							{
								object obj = DataHelper.CheckType(((IDataRecord)objReader)[i], type);
								if (obj is string)
								{
									obj = obj.ToNullString();
								}
								property.SetValue(val, obj, null);
							}
						}
					}
				}
				return val;
			}
			return default(T);
		}

		public static IList<T> ReaderToList<T>(IDataReader objReader) where T : new()
		{
			if (objReader != null)
			{
				List<T> list = new List<T>();
				Type typeFromHandle = typeof(T);
				while (objReader.Read())
				{
					T val = new T();
					for (int i = 0; i < objReader.FieldCount; i++)
					{
						if (!DataHelper.IsNullOrDBNull(((IDataRecord)objReader)[i]))
						{
							PropertyInfo property = typeFromHandle.GetProperty(objReader.GetName(i).Replace("_", ""), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
							if (property != (PropertyInfo)null)
							{
								Type type = property.PropertyType;
								if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
								{
									NullableConverter nullableConverter = new NullableConverter(type);
									type = nullableConverter.UnderlyingType;
								}
								if (property.PropertyType.IsEnum)
								{
									object value = Enum.ToObject(type, ((IDataRecord)objReader)[i]);
									property.SetValue(val, value, null);
								}
								else
								{
									object obj = DataHelper.CheckType(((IDataRecord)objReader)[i], type);
									if (obj is string)
									{
										obj = obj.ToNullString();
									}
									property.SetValue(val, obj, null);
								}
							}
						}
					}
					list.Add(val);
				}
				return list;
			}
			return null;
		}

		private static object CheckType(object value, Type conversionType)
		{
			if (value == null)
			{
				return null;
			}
			return Convert.ChangeType(value, conversionType);
		}

		private static bool IsNullOrDBNull(object obj)
		{
			return (obj == null || obj is DBNull) && true;
		}

		public static DbQueryResult PagingByRownumber(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
		{
			return DataHelper.PagingByRownumber(pageIndex, pageSize, sortBy, sortOrder, isCount, table, pk, filter, selectFields, 0);
		}

		public static DbQueryResult PagingByRownumber(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields, int partitionSize)
		{
			if (string.IsNullOrEmpty(table))
			{
				return null;
			}
			if (string.IsNullOrEmpty(sortBy) && string.IsNullOrEmpty(pk))
			{
				return null;
			}
			if (string.IsNullOrEmpty(selectFields))
			{
				selectFields = "*";
			}
			string query = DataHelper.BuildRownumberQuery(sortBy, sortOrder, isCount, table, pk, filter, selectFields, partitionSize);
			int num = (pageIndex - 1) * pageSize + 1;
			int num2 = num + pageSize - 1;
			DbQueryResult dbQueryResult = new DbQueryResult();
			try
			{
				Database database = DatabaseFactory.CreateDatabase();
				DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
				database.AddInParameter(sqlStringCommand, "StartNumber", DbType.Int32, num);
				database.AddInParameter(sqlStringCommand, "EndNumber", DbType.Int32, num2);
				using (IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (isCount && partitionSize == 0 && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
			}
			catch (Exception ex)
			{
				HttpContext.Current.Response.Write(selectFields + "<br>" + ex.Message);
			}
			return dbQueryResult;
		}

		private static string BuildRownumberQuery(string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields, int partitionSize)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string arg = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
			if (partitionSize > 0)
			{
				stringBuilder.AppendFormat("SELECT TOP {0} {1}, ROW_NUMBER() OVER (ORDER BY ", partitionSize.ToString(CultureInfo.InvariantCulture), selectFields);
			}
			else
			{
				stringBuilder.AppendFormat("SELECT {0} , ROW_NUMBER() OVER (ORDER BY ", selectFields);
			}
			stringBuilder.AppendFormat("{0} {1}", string.IsNullOrEmpty(sortBy) ? pk : sortBy, sortOrder.ToString());
			stringBuilder.AppendFormat(") AS RowNumber FROM {0} {1}", table, arg);
			stringBuilder.Insert(0, "SELECT * FROM (").Append(") T WHERE T.RowNumber BETWEEN @StartNumber AND @EndNumber order by T.RowNumber");
			if (isCount && partitionSize == 0)
			{
				stringBuilder.AppendFormat(";SELECT COUNT(*) FROM {0} {1}", table, arg);
			}
			return stringBuilder.ToString();
		}

		public static DbQueryResult PagingByTopsort(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
		{
			if (string.IsNullOrEmpty(table))
			{
				return null;
			}
			if (string.IsNullOrEmpty(sortBy) && string.IsNullOrEmpty(pk))
			{
				return null;
			}
			if (string.IsNullOrEmpty(selectFields))
			{
				selectFields = "*";
			}
			string query = DataHelper.BuildTopQuery(pageIndex, pageSize, sortBy, sortOrder, isCount, table, pk, filter, selectFields);
			DbQueryResult dbQueryResult = new DbQueryResult();
			Database database = DatabaseFactory.CreateDatabase();
			DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
			using (IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (isCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		private static string BuildTopQuery(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string pk, string filter, string selectFields)
		{
			string text = string.IsNullOrEmpty(sortBy) ? pk : sortBy;
			string text2 = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
			string text3 = string.IsNullOrEmpty(filter) ? "" : ("AND " + filter);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} {1} FROM {2} ", pageSize.ToString(CultureInfo.InvariantCulture), selectFields, table);
			if (pageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} ORDER BY {1} {2}", text2, text, sortOrder.ToString());
			}
			else
			{
				int num = ((pageIndex - 1 <= 0) ? 1 : (pageIndex - 1)) * pageSize;
				if (sortOrder == SortAction.Asc)
				{
					stringBuilder.AppendFormat("WHERE {0} > (SELECT MAX({0}) FROM (SELECT TOP {1} {0} FROM {2} {3} ORDER BY {0} ASC) AS TMP) {4} ORDER BY {0} ASC", text, num, table, text2, text3);
				}
				else
				{
					stringBuilder.AppendFormat("WHERE {0} < (SELECT MIN({0}) FROM (SELECT TOP {1} {0} FROM {2} {3} ORDER BY {0} DESC) AS TMP) {4} ORDER BY {0} DESC", text, num, table, text2, text3);
				}
			}
			if (isCount)
			{
				stringBuilder.AppendFormat(";SELECT COUNT({0}) FROM {1} {2}", text, table, text2);
			}
			return stringBuilder.ToString();
		}

		public static DbQueryResult PagingByTopnotin(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string key, string filter, string selectFields)
		{
			if (string.IsNullOrEmpty(table))
			{
				return null;
			}
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			if (string.IsNullOrEmpty(selectFields))
			{
				selectFields = "*";
			}
			string query = DataHelper.BuildNotinQuery(pageIndex, pageSize, sortBy, sortOrder, isCount, table, key, filter, selectFields);
			DbQueryResult dbQueryResult = new DbQueryResult();
			Database database = DatabaseFactory.CreateDatabase();
			DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
			using (IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (isCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}

		public static string BuildNotinQuery(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string key, string filter, string selectFields)
		{
			string text = string.IsNullOrEmpty(filter) ? "" : ("WHERE " + filter);
			string text2 = string.IsNullOrEmpty(filter) ? "" : ("AND " + filter);
			string text3 = string.IsNullOrEmpty(sortBy) ? "" : ("ORDER BY " + sortBy + " " + sortOrder.ToString());
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} {1} FROM {2} ", pageSize.ToString(CultureInfo.InvariantCulture), selectFields, table);
			if (pageIndex == 1)
			{
				stringBuilder.AppendFormat("{0} {1}", text, text3);
			}
			else
			{
				int num = ((pageIndex - 1 <= 0) ? 1 : (pageIndex - 1)) * pageSize;
				stringBuilder.AppendFormat("WHERE {0} NOT IN (SELECT TOP {1} {0} FROM {2} {3} {4}) {5} {4}", key, num, table, text, text3, text2);
			}
			if (isCount)
			{
				stringBuilder.AppendFormat(";SELECT COUNT({0}) FROM {1} {2}", key, table, text);
			}
			return stringBuilder.ToString();
		}

		public static bool SwapSequence(string table, string keyField, string sequenceField, int key, int replaceKey, int sequence, int replaceSequence)
		{
			string str = $"UPDATE {table} SET {sequenceField} = {replaceSequence} WHERE {keyField} = {key}";
			str += $" UPDATE {table} SET {sequenceField} = {sequence} WHERE {keyField} = {replaceKey}";
			Database database = DatabaseFactory.CreateDatabase();
			DbCommand sqlStringCommand = database.GetSqlStringCommand(str);
			return database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public static bool CheckLen(string Str, int len, bool IsEmpty)
		{
			if ((Str.Equals(string.Empty) || Str.Length == 0) & IsEmpty)
			{
				return true;
			}
			if (Str.Length < len || Str.Length == 0)
			{
				return false;
			}
			return true;
		}

		public static bool IsNumber(string inputData)
		{
			Match match = DataHelper.RegNumber.Match(inputData);
			return match.Success;
		}

		public static bool IsNumberSign(string inputData)
		{
			Match match = DataHelper.RegNumberSign.Match(inputData);
			return match.Success;
		}

		public static bool IsDecimal(string inputData)
		{
			Match match = DataHelper.RegDecimal.Match(inputData);
			return match.Success;
		}

		public static bool IsDecimalSign(string inputData)
		{
			Match match = DataHelper.RegDecimalSign.Match(inputData);
			return match.Success;
		}

		public static bool IsHasCHZN(string inputData)
		{
			Match match = DataHelper.RegCHZN.Match(inputData);
			return match.Success;
		}

		public static bool IsEmail(string inputData)
		{
			Match match = DataHelper.RegEmail.Match(inputData);
			return match.Success;
		}

		public static bool IsUrl(string inputdata)
		{
			Match match = DataHelper.RegUrl.Match(inputdata);
			return match.Success;
		}

		public static string HtmlEncode(string inputData)
		{
			return HttpUtility.HtmlEncode(inputData);
		}

		public static string HtmlDecode(string inputData)
		{
			return HttpUtility.HtmlDecode(inputData);
		}

		public static string SqlTextClear(string sqlText)
		{
			if (sqlText == null)
			{
				return null;
			}
			if (sqlText == "")
			{
				return "";
			}
			sqlText = sqlText.Replace(",", "");
			sqlText = sqlText.Replace("<", "");
			sqlText = sqlText.Replace(">", "");
			sqlText = sqlText.Replace("--", "");
			sqlText = sqlText.Replace("'", "");
			sqlText = sqlText.Replace("\"", "");
			sqlText = sqlText.Replace("=", "");
			sqlText = sqlText.Replace("%", "");
			sqlText = sqlText.Replace(" ", "");
			return sqlText;
		}

		public static bool IsContainSameChar(string strInput)
		{
			string charInput = string.Empty;
			if (!string.IsNullOrEmpty(strInput))
			{
				charInput = strInput.Substring(0, 1);
			}
			return DataHelper.isContainSameChar(strInput, charInput);
		}

		public static bool isContainSameChar(string strInput, string charInput)
		{
			if (string.IsNullOrEmpty(charInput))
			{
				return false;
			}
			Regex regex = new Regex($"^([{charInput}])+$");
			Match match = regex.Match(strInput);
			return match.Success;
		}

		public static bool isContainSpecChar(string strInput)
		{
			string[] array = new string[2]
			{
				"123456",
				"654321"
			};
			bool result = false;
			int num = 0;
			while (num < array.Length)
			{
				if (!(strInput == array[num]))
				{
					num++;
					continue;
				}
				result = true;
				break;
			}
			return result;
		}

		public static bool IsZipCode(string data)
		{
			return DataHelper.RegZipCode.IsMatch(data);
		}

		public static bool IsIP(string data)
		{
			return DataHelper.RegIP.IsMatch(data);
		}

		public static bool IsNormalChar(string data)
		{
			return DataHelper.RegNormalchar.IsMatch(data);
		}

		public static bool IsZHCN(string data)
		{
			return DataHelper.RegChinese.IsMatch(data);
		}

		public static bool IsIDCard(string Id)
		{
			if (Id.Length == 18)
			{
				return DataHelper.IsIDCard18(Id);
			}
			if (Id.Length == 15)
			{
				return DataHelper.IsIDCard15(Id);
			}
			return false;
		}

		public static bool IsIDCard18(string Id)
		{
			long num = 0L;
			if (!long.TryParse(Id.Remove(17), out num) || (double)num < Math.Pow(10.0, 16.0) || !long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out num))
			{
				return false;
			}
			string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (text.IndexOf(Id.Remove(2)) == -1)
			{
				return false;
			}
			string s = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
			DateTime dateTime = default(DateTime);
			if (!DateTime.TryParse(s, out dateTime))
			{
				return false;
			}
			string[] array = "1,0,x,9,8,7,6,5,4,3,2".Split(',');
			string[] array2 = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(',');
			char[] array3 = Id.Remove(17).ToCharArray();
			int num2 = 0;
			for (int i = 0; i < 17; i++)
			{
				num2 += int.Parse(array2[i]) * int.Parse(array3[i].ToString());
			}
			int num3 = -1;
			Math.DivRem(num2, 11, out num3);
			if (array[num3] != Id.Substring(17, 1).ToLower())
			{
				return false;
			}
			return true;
		}

		public static bool IsIDCard15(string Id)
		{
			long num = 0L;
			if (!long.TryParse(Id, out num) || (double)num < Math.Pow(10.0, 14.0))
			{
				return false;
			}
			string text = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (text.IndexOf(Id.Remove(2)) == -1)
			{
				return false;
			}
			string s = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
			DateTime dateTime = default(DateTime);
			if (!DateTime.TryParse(s, out dateTime))
			{
				return false;
			}
			return true;
		}

		public static bool IsUserName(string data)
		{
			return DataHelper.RegUserName.IsMatch(data);
		}

		public static bool IsMobile(string data)
		{
			return DataHelper.RegMobile.IsMatch(data);
		}

		public static bool IsPhone(string data)
		{
			Match match = DataHelper.RegPhone.Match(data);
			return match.Success;
		}

		public static bool IsTel(string data)
		{
			return DataHelper.IsPhone(data) || DataHelper.IsMobile(data);
		}

		public static object DefaultForType(Type targetType)
		{
			return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
		}

		public static T ConvertDictionaryToEntity<T>(IDictionary<string, object> dict)
		{
			T val = Activator.CreateInstance<T>();
            PropertyInfo[] properties = val.GetType().GetProperties();
			if (properties.Length != 0 && ((ICollection<KeyValuePair<string, object>>)dict).Count > 0)
			{
				PropertyInfo[] array = properties;
				foreach (PropertyInfo propertyInfo in array)
				{
					MethodInfo getMethod = propertyInfo.GetGetMethod();
					if (dict.ContainsKey(propertyInfo.Name))
					{
						string name = propertyInfo.Name;
						string value = dict[propertyInfo.Name].ToNullString();
						try
						{
							if (propertyInfo.CanWrite)
							{
								propertyInfo.SetValue(val, string.IsNullOrEmpty(value) ? ((propertyInfo.PropertyType == typeof(string)) ? "" : DataHelper.DefaultForType(propertyInfo.PropertyType)) : Convert.ChangeType(value, propertyInfo.PropertyType), null);
							}
						}
						catch (Exception ex)
						{
							IDictionary<string, string> dictionary = new Dictionary<string, string>();
							dictionary.Add("key", name);
							dictionary.Add("value", value);
							Globals.WriteExceptionLog(ex, dictionary, "ConvertException");
						}
					}
				}
			}
			return val;
		}

		public static IDictionary<string, object> ConvertEntityToObjDictionary(object obj)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != (MethodInfo)null && getMethod.IsPublic && propertyInfo.CanRead)
				{
					dictionary.Add(propertyInfo.Name, getMethod.Invoke(obj, new object[0]));
				}
			}
			return dictionary;
		}

		public static Dictionary<string, object> ConvertEntityToDictionarys(object obj)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != (MethodInfo)null && getMethod.IsPublic)
				{
					dictionary.Add(propertyInfo.Name, getMethod.Invoke(obj, new object[0]).ToNullString());
				}
			}
			return dictionary;
		}

		public static PageModel<T> PagingByRownumber<T>(int pageIndex, int pageSize, string sortBy, SortAction sortOrder, bool isCount, string table, string key, string filter, string selectFields) where T : new()
		{
			PageModel<T> pageModel = new PageModel<T>();
			IList<T> models = null;
			if (string.IsNullOrEmpty(table))
			{
				return null;
			}
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			if (string.IsNullOrEmpty(selectFields))
			{
				selectFields = "*";
			}
			string query = DataHelper.BuildRownumberQuery(sortBy, sortOrder, isCount, table, key, filter, selectFields, 0);
			int num = (pageIndex - 1) * pageSize + 1;
			int num2 = num + pageSize - 1;
			Database database = DatabaseFactory.CreateDatabase();
			DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
			database.AddInParameter(sqlStringCommand, "StartNumber", DbType.Int32, num);
			database.AddInParameter(sqlStringCommand, "EndNumber", DbType.Int32, num2);
			using (IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
			{
				models = DataHelper.ReaderToList<T>(dataReader);
				if (isCount && dataReader.NextResult())
				{
					dataReader.Read();
					pageModel.Total = dataReader.GetInt32(0);
				}
			}
			pageModel.Models = models;
			return pageModel;
		}

		public static List<Dictionary<string, object>> DataTableToDictionary(DataTable dt)
		{
			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow row in dt.Rows)
				{
					list.Add(DataHelper.DataRowToDictionary(row));
				}
			}
			return list;
		}

		public static Dictionary<string, object> DataRowToDictionary(DataRow dr)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			foreach (DataColumn column in dr.Table.Columns)
			{
				dictionary.Add(column.ColumnName, dr[column]);
			}
			return dictionary;
		}

		public static List<Dictionary<string, object>> ListToDictionary<T>(IEnumerable<T> data)
		{
			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
			if (data != null && data.Count() > 0)
			{
				foreach (T datum in data)
				{
					list.Add(DataHelper.ConvertEntityToDictionarys(datum));
				}
			}
			return list;
		}

		public static string GetHiddenUsername(string name)
		{
			if (DataHelper.IsMobile(name) || DataHelper.IsTel(name))
			{
				return name.Substring(0, 3) + "****" + name.Substring(name.Length - 3);
			}
			if (DataHelper.IsEmail(name))
			{
				int num = name.IndexOf('@');
				string text = name.Substring(0, num);
				string text2 = "";
				text2 = ((text.Length <= 6) ? ((name.Length <= 3) ? (text.Substring(0, 1) + "***") : (text.Substring(0, 1) + "***" + text.Substring(text.Length - 1))) : (text.Substring(0, 2) + "***" + text.Substring(text.Length - 2)));
				return text2 + name.Substring(num);
			}
			if (name.Length > 6)
			{
				return name.Substring(0, 2) + "***" + name.Substring(name.Length - 2);
			}
			if (name.Length > 3)
			{
				return name.Substring(0, 1) + "***" + name.Substring(name.Length - 1);
			}
			if (name.Length > 1)
			{
				return name.Substring(0, 1) + "***";
			}
			return name;
		}

		public static IList<long> GetSafeIDList(string IdList, char split = '_', bool isDistinct = true)
		{
			IList<long> list = new List<long>();
			if (string.IsNullOrEmpty(IdList))
			{
				return list;
			}
			string[] array = IdList.Split(split);
			if (isDistinct)
			{
				array = array.Distinct().ToArray();
			}
			long num = 0L;
			string[] array2 = array;
			foreach (string s in array2)
			{
				long.TryParse(s, out num);
				if (num >= 0)
				{
					list.Add(num);
				}
			}
			return list;
		}
	}
}
