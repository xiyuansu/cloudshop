using Hidistro.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using ThoughtWorks.QRCode.Codec;

namespace Hidistro.Core
{
	public static class Globals
	{
		public static readonly string HIPOSTAKECODEPREFIX = "YSC";

		public static string IPAddress
		{
			get
			{
				Regex regex = new Regex("^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");
				IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
				string sign = hostEntry.AddressList[0].ToNullString();
				Globals.AppendLog("IPHostEntry", sign, "", "IPAddress");
				if (HttpContext.Current == null)
				{
					IPHostEntry hostEntry2 = Dns.GetHostEntry(Dns.GetHostName());
					sign = hostEntry2.AddressList[0].ToNullString();
				}
				else
				{
					sign = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
					if (sign != null && sign != string.Empty)
					{
						if (sign.IndexOf(".") == -1)
						{
							sign = null;
						}
						else if (sign.IndexOf(",") != -1)
						{
							sign = sign.Replace(" ", "").Replace("'", "");
							string[] array = sign.Split(",;".ToCharArray());
							for (int i = 0; i < array.Length; i++)
							{
								if (regex.IsMatch(array[i]) && array[i].Substring(0, 3) != "10." && array[i].Substring(0, 7) != "192.168" && array[i].Substring(0, 7) != "172.16.")
								{
									return array[i];
								}
							}
						}
						else
						{
							if (regex.IsMatch(sign))
							{
								return sign;
							}
							sign = null;
						}
					}
					string text = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != string.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
					if (sign == null || sign == string.Empty)
					{
						sign = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
					}
					if (sign == null || sign == string.Empty)
					{
						sign = HttpContext.Current.Request.UserHostAddress;
					}
				}
				return sign;
			}
		}

		public static string DomainName
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return string.Empty;
				}
				string text = HttpContext.Current.Request.Url.ToString();
				text = text.Replace("http://", "").Replace("https://", "");
				return (text.Split(':').Length <= 1) ? text.Split('/')[0] : text.Split(':')[0];
			}
		}

		public static bool IsTestDomain
		{
			get
			{
				//if (!string.IsNullOrEmpty(Globals.DomainName))
				//{
				//	if (Globals.DomainName.EndsWith("xm.huz.cn") || Globals.DomainName.StartsWith("localhost") || Globals.DomainName.StartsWith("127.0.0.1") || Globals.DomainName.Contains("ysctest.huz.cn") || Globals.DomainName.Contains("yscssl.huz.cn"))
				//	{
				//		return true;
				//	}
				//	return false;
				//}
				return true;
			}
		}

		public static bool IsIpAddress(string ip)
		{
			Regex regex = new Regex("^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");
			return regex.IsMatch(ip);
		}

		public static string GetIPAddress(HttpContext context)
		{
			Regex regex = new Regex("^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");
			string text;
			if (context == null)
			{
				IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
				text = hostEntry.AddressList[0].ToNullString();
			}
			else
			{
				text = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (text != null && text != string.Empty)
				{
					if (text.IndexOf(".") == -1)
					{
						text = null;
					}
					else if (text.IndexOf(",") != -1)
					{
						text = text.Replace(" ", "").Replace("'", "");
						string[] array = text.Split(",;".ToCharArray());
						for (int i = 0; i < array.Length; i++)
						{
							if (regex.IsMatch(array[i]) && array[i].Substring(0, 3) != "10." && array[i].Substring(0, 7) != "192.168" && array[i].Substring(0, 7) != "172.16.")
							{
								return array[i];
							}
						}
					}
					else
					{
						if (regex.IsMatch(text))
						{
							return text;
						}
						text = null;
					}
				}
				if (text == null || text == string.Empty)
				{
					text = context.Request.ServerVariables["REMOTE_ADDR"];
				}
				if (text == null || text == string.Empty)
				{
					text = context.Request.UserHostAddress;
				}
			}
			return text;
		}

		public static string GetSafeSortField(string allowFields, string field, string defaultField)
		{
			string result = defaultField;
			string[] array = allowFields.Split(',');
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text.ToLower() == field.ToLower())
				{
					result = field;
					break;
				}
			}
			return result;
		}

		public static string GetSafeSortOrder(string sortOrder, string defaultSortOrder = "Desc")
		{
			string result = defaultSortOrder;
			if (!string.IsNullOrEmpty(sortOrder))
			{
				if (sortOrder.ToUpper() != "DESC" && sortOrder.ToUpper() != "ASC")
				{
					result = "Desc";
				}
				else
				{
					if (sortOrder.ToUpper() == "DESC")
					{
						result = "Desc";
					}
					if (sortOrder.ToUpper() == "ASC")
					{
						result = "Asc";
					}
				}
			}
			return result;
		}

		public static string GetSafeIDList(string IdList, char split = '_', bool isDistinct = true)
		{
			string text = "";
			if (string.IsNullOrEmpty(IdList))
			{
				return "";
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
					text = text + ((text == "") ? "" : split.ToString()) + num.ToString();
				}
			}
			return text;
		}

		public static void WriteLog(string fileName, string txt)
		{
			string path = Globals.GetphysicsPath(fileName);
			Globals.CreatePath(path);
			try
			{
				using (StreamWriter streamWriter = File.AppendText(path))
				{
					streamWriter.WriteLine(txt);
					streamWriter.WriteLine("");
					streamWriter.Flush();
					streamWriter.Close();
				}
			}
			catch
			{
			}
		}

		public static string GetStoragePath()
		{
			return "/Storage/master";
		}

		public static string PhysicalPath(string path)
		{
			if (path == null)
			{
				return string.Empty;
			}
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			string text = directorySeparatorChar.ToString();
			baseDirectory = baseDirectory.Replace("/", text);
			string text2 = "/";
			if (text2 != null)
			{
				text2 = text2.Replace("/", text);
				baseDirectory = ((text2.Length <= 0 || !text2.StartsWith(text) || !baseDirectory.EndsWith(text)) ? (baseDirectory + text2) : (baseDirectory + text2.Substring(1)));
			}
			string str = baseDirectory.TrimEnd(Path.DirectorySeparatorChar);
			directorySeparatorChar = Path.DirectorySeparatorChar;
			return str + directorySeparatorChar.ToString() + path.TrimStart(Path.DirectorySeparatorChar);
		}

		public static string MapPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return string.Empty;
			}
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				return current.Request.MapPath(path);
			}
			return Globals.PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
		}

		public static void RedirectToSSL(HttpContext context)
		{
			if (context != null && !context.Request.IsSecureConnection)
			{
				Uri url = context.Request.Url;
				context.Response.Redirect("https://" + url.ToString().Substring(7));
			}
		}

		public static bool IsUrlAbsolute(string url)
		{
			if (url == null)
			{
				return false;
			}
			string[] array = new string[12]
			{
				"about:",
				"file:///",
				"ftp://",
				"gopher://",
				"http://",
				"https://",
				"javascript:",
				"mailto:",
				"news:",
				"res://",
				"telnet://",
				"view-source:"
			};
			string[] array2 = array;
			foreach (string value in array2)
			{
				if (url.StartsWith(value))
				{
					return true;
				}
			}
			return false;
		}

		public static string GetImageServerUrl()
		{
			return HiConfiguration.GetConfig().ImageServerUrl.ToLower();
		}

		public static string GetImageServerUrl(string flag, string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return string.Empty;
			}
			if (!imageUrl.Contains(flag) && !imageUrl.Contains("http://") && !imageUrl.Contains("https://"))
			{
				return Globals.FullPath(Globals.GetImageServerUrl() + imageUrl);
			}
			return imageUrl;
		}

		public static void EntityCoding(object entity, bool encode)
		{
			if (entity != null)
			{
				Type type = entity.GetType();
				PropertyInfo[] properties = type.GetProperties();
				PropertyInfo[] array = properties;
				int num = 0;
				while (true)
				{
					if (num < array.Length)
					{
						PropertyInfo propertyInfo = array[num];
						if (propertyInfo.GetCustomAttributes(typeof(HtmlCodingAttribute), true).Length != 0)
						{
							if (!propertyInfo.CanWrite || !propertyInfo.CanRead)
							{
								throw new Exception("使用HtmlEncodeAttribute修饰的属性必须是可读可写的");
							}
							if (propertyInfo.PropertyType.Equals(typeof(string)))
							{
								string text = propertyInfo.GetValue(entity, null) as string;
								if (!string.IsNullOrEmpty(text))
								{
									if (encode)
									{
										propertyInfo.SetValue(entity, Globals.HtmlEncode(text), null);
									}
									else
									{
										propertyInfo.SetValue(entity, Globals.HtmlDecode(text), null);
									}
								}
								goto IL_00f1;
							}
							break;
						}
						goto IL_00f1;
					}
					return;
					IL_00f1:
					num++;
				}
				throw new Exception("非字符串类型的属性不能使用HtmlEncodeAttribute修饰");
			}
		}

		public static string HtmlDecode(string textToFormat)
		{
			if (string.IsNullOrEmpty(textToFormat))
			{
				return textToFormat;
			}
			return HttpUtility.HtmlDecode(textToFormat);
		}

		public static string HtmlEncode(string textToFormat)
		{
			if (string.IsNullOrEmpty(textToFormat))
			{
				return textToFormat;
			}
			return HttpUtility.HtmlEncode(textToFormat);
		}

		public static string UrlEncode(string urlToEncode)
		{
			if (string.IsNullOrEmpty(urlToEncode))
			{
				return urlToEncode;
			}
			return HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
		}

		public static string UrlDecode(string urlToDecode)
		{
			if (string.IsNullOrEmpty(urlToDecode))
			{
				return urlToDecode;
			}
			return HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
		}

		public static string StripScriptTags(string content)
		{
			content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			content = Regex.Replace(content, "'javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return Regex.Replace(content, "\"javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public static string StripAllTags(string strToStrip)
		{
			if (!string.IsNullOrEmpty(strToStrip))
			{
				strToStrip = Regex.Replace(strToStrip, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				strToStrip = Regex.Replace(strToStrip, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				strToStrip = Globals.StripHtmlXmlTags(strToStrip);
			}
			return strToStrip;
		}

		public static string StripHtmlXmlTags(string content)
		{
			return Regex.Replace(content, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		public static string HostPath(Uri uri)
		{
			if (uri == (Uri)null)
			{
				return string.Empty;
			}
			string text = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString(CultureInfo.InvariantCulture));
			return string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[3]
			{
				uri.Scheme,
				uri.Host,
				text
			});
		}

		public static string HttpsHostPath(Uri uri)
		{
			if (uri == (Uri)null)
			{
				return string.Empty;
			}
			string text = "";
			return string.Format(CultureInfo.InvariantCulture, "{2}://{0}{1}", new object[3]
			{
				uri.Host,
				text,
				"https"
			});
		}

		public static string FullPath(string local)
		{
			if (string.IsNullOrEmpty(local))
			{
				return local;
			}
			if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://") || local.ToLower(CultureInfo.InvariantCulture).StartsWith("https://"))
			{
				return local;
			}
			if (HttpContext.Current == null)
			{
				return local;
			}
			if (local.ToLower(CultureInfo.InvariantCulture).StartsWith(HttpContext.Current.Request.Url.Host.ToLower(CultureInfo.InvariantCulture)))
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}://{1}", new object[2]
				{
					Globals.GetProtocal(null),
					local
				});
			}
			return Globals.FullPath(Globals.HostPath(HttpContext.Current.Request.Url), local);
		}

		public static string HttpsFullPath(string local)
		{
			if (string.IsNullOrEmpty(local))
			{
				return local;
			}
			if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://") || local.ToLower(CultureInfo.InvariantCulture).StartsWith("https://"))
			{
				return local;
			}
			if (HttpContext.Current == null)
			{
				return local;
			}
			if (local.ToLower(CultureInfo.InvariantCulture).StartsWith(HttpContext.Current.Request.Url.Host.ToLower(CultureInfo.InvariantCulture)))
			{
				return string.Format(CultureInfo.InvariantCulture, "https://{0}", new object[1]
				{
					local
				});
			}
			return Globals.FullPath(Globals.HttpsHostPath(HttpContext.Current.Request.Url), local);
		}

		public static string FullPath(string hostPath, string local)
		{
			return hostPath + local;
		}

		public static string GetAdminAbsolutePath(string path)
		{
			if (path.StartsWith("/"))
			{
				return "/" + HiConfiguration.GetConfig().AdminFolder + path;
			}
			return "/" + HiConfiguration.GetConfig().AdminFolder + "/" + path;
		}

		public static string FormatMoney(decimal money)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[1]
			{
				money.F2ToString("f2")
			});
		}

		public static string GetphysicsPath(string path)
		{
			string text = "";
			try
			{
				if (HttpContext.Current == null)
				{
					string text2 = path.Replace("/", "\\");
					if (text2.StartsWith("\\"))
					{
						text2 = text2.TrimStart('\\');
					}
					return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, text2);
				}
				return HttpContext.Current.Request.MapPath(path);
			}
			catch
			{
				return path;
			}
		}

		public static string GetphysicsPath(string path, out bool CheckExist)
		{
			CheckExist = true;
			string text = Globals.GetphysicsPath(path);
			if (CheckExist)
			{
				if (text.EndsWith(Path.DirectorySeparatorChar.ToString()))
				{
					if (!Directory.Exists(text))
					{
						CheckExist = false;
					}
				}
				else if (!File.Exists(text))
				{
					CheckExist = false;
				}
			}
			return text;
		}

		public static bool PathExist(string path, bool CreatePath = false)
		{
			bool flag = false;
			path = Globals.GetphysicsPath(path, out flag);
			string[] array = path.Split(Path.DirectorySeparatorChar);
			string text = array[0];
			int num = array.Length;
			if (array.Length >= 1 && array[array.Length - 1].Contains("."))
			{
				num--;
			}
			if (CreatePath && !flag)
			{
				for (int i = 1; i < num; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						text = text + Path.DirectorySeparatorChar.ToString() + array[i];
						try
						{
							if (!Directory.Exists(text))
							{
								Directory.CreateDirectory(text);
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
			return flag;
		}

		public static bool CreatePath(string path)
		{
			bool result = true;
			path = Globals.GetphysicsPath(path).ToLower();
			string text = AppDomain.CurrentDomain.BaseDirectory.ToLower();
			path = path.Replace(text, "");
			string[] array = path.Split(Path.DirectorySeparatorChar);
			string str = array[0];
			str = text + str;
			int num = array.Length;
			if (array.Length >= 1 && array[array.Length - 1].Contains("."))
			{
				num--;
			}
			try
			{
				for (int i = 1; i < num; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						str = str + Path.DirectorySeparatorChar.ToString() + array[i];
						if (!Directory.Exists(str))
						{
							Directory.CreateDirectory(str);
						}
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static string GetGenerateId()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(ushort)(48 + (ushort)(num % 10))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMdd") + text;
		}

		public static void WriteLog(IDictionary<string, string> param, string msg, string sign = "", string url = "", string logPath = "")
		{
			try
			{
				Globals.CreatePath("/log");
				DateTime now;
				if (string.IsNullOrEmpty(logPath))
				{
					now = DateTime.Now;
					logPath = "/log/error_" + now.ToString("yyyyMMddHHmm") + ".xml";
				}
				else if (logPath.IndexOf('/') == -1 && logPath.IndexOf('.') == -1)
				{
					string[] obj = new string[5]
					{
						"/log/",
						logPath,
						"_",
						null,
						null
					};
					now = DateTime.Now;
					obj[3] = now.ToString("yyyyMMddHHmm");
					obj[4] = ".xml";
					logPath = string.Concat(obj);
				}
				DataTable dataTable = new DataTable();
				dataTable.TableName = "log";
				dataTable.Columns.Add(new DataColumn("HishopOperTime"));
				if (param != null)
				{
					foreach (KeyValuePair<string, string> item in param)
					{
						dataTable.Columns.Add(new DataColumn(item.Key));
					}
				}
				dataTable.Columns.Add(new DataColumn("HishopMsg"));
				if (!string.IsNullOrEmpty(sign))
				{
					dataTable.Columns.Add(new DataColumn("HishopSign"));
				}
				if (!string.IsNullOrEmpty(url))
				{
					dataTable.Columns.Add(new DataColumn("HishopUrl"));
				}
				DataRow dataRow = dataTable.NewRow();
				dataRow["HishopOperTime"] = DateTime.Now;
				foreach (KeyValuePair<string, string> item2 in param)
				{
					dataRow[item2.Key] = item2.Value;
				}
				dataRow["HishopMsg"] = msg;
				if (!string.IsNullOrEmpty(sign))
				{
					dataRow["HishopSign"] = sign;
				}
				if (!string.IsNullOrEmpty(url))
				{
					dataRow["HishopUrl"] = url;
				}
				dataTable.Rows.Add(dataRow);
				string fileName = Globals.GetphysicsPath(logPath);
				dataTable.WriteXml(fileName);
			}
			catch (Exception innerException)
			{
				throw new Exception("写日志失败:" + logPath, innerException);
			}
		}

		public static void WriteLog(NameValueCollection param, string msg, string sign = "", string url = "", string logPath = "")
		{
			try
			{
				Globals.CreatePath("/log");
				DateTime now;
				if (string.IsNullOrEmpty(logPath))
				{
					now = DateTime.Now;
					logPath = "/log/error_" + now.ToString("yyyyMMddHHmm") + ".xml";
				}
				else if (logPath.IndexOf('/') == -1 && logPath.IndexOf('.') == -1)
				{
					string[] obj = new string[5]
					{
						"/log/",
						logPath,
						"_",
						null,
						null
					};
					now = DateTime.Now;
					obj[3] = now.ToString("yyyyMMddHHmm");
					obj[4] = ".xml";
					logPath = string.Concat(obj);
				}
				DataTable dataTable = new DataTable();
				dataTable.TableName = "log";
				dataTable.Columns.Add(new DataColumn("HishopOperTime"));
				if (param != null)
				{
					string[] allKeys = param.AllKeys;
					foreach (string columnName in allKeys)
					{
						dataTable.Columns.Add(new DataColumn(columnName));
					}
				}
				dataTable.Columns.Add(new DataColumn("HishopMsg"));
				if (!string.IsNullOrEmpty(sign))
				{
					dataTable.Columns.Add(new DataColumn("HishopSign"));
				}
				if (!string.IsNullOrEmpty(url))
				{
					dataTable.Columns.Add(new DataColumn("HishopUrl"));
				}
				DataRow dataRow = dataTable.NewRow();
				dataRow["HishopOperTime"] = DateTime.Now;
				string[] allKeys2 = param.AllKeys;
				foreach (string text in allKeys2)
				{
					dataRow[text] = param[text];
				}
				dataRow["HishopMsg"] = msg;
				if (!string.IsNullOrEmpty(sign))
				{
					dataRow["HishopSign"] = sign;
				}
				if (!string.IsNullOrEmpty(url))
				{
					dataRow["HishopUrl"] = url;
				}
				dataTable.Rows.Add(dataRow);
				string fileName = Globals.GetphysicsPath(logPath);
				dataTable.WriteXml(fileName);
			}
			catch (Exception innerException)
			{
				throw new Exception("写日志失败:" + logPath, innerException);
			}
		}

		public static void WriteLog(string msg, string sign = "", string url = "", string logPath = "")
		{
			try
			{
				Globals.CreatePath("/log");
				DateTime now;
				if (string.IsNullOrEmpty(logPath))
				{
					now = DateTime.Now;
					logPath = "/log/error_" + now.ToString("yyyyMMddHHmm") + ".xml";
				}
				else if (logPath.IndexOf('/') == -1 && logPath.IndexOf('.') == -1)
				{
					string[] obj = new string[5]
					{
						"/log/",
						logPath,
						"_",
						null,
						null
					};
					now = DateTime.Now;
					obj[3] = now.ToString("yyyyMMddHHmm");
					obj[4] = ".xml";
					logPath = string.Concat(obj);
				}
				DataTable dataTable = new DataTable();
				dataTable.TableName = "log";
				dataTable.Columns.Add(new DataColumn("HishopOperTime"));
				dataTable.Columns.Add(new DataColumn("HishopMsg"));
				if (!string.IsNullOrEmpty(sign))
				{
					dataTable.Columns.Add(new DataColumn("HishopSign"));
				}
				if (!string.IsNullOrEmpty(url))
				{
					dataTable.Columns.Add(new DataColumn("HishopUrl"));
				}
				DataRow dataRow = dataTable.NewRow();
				dataRow["HishopOperTime"] = DateTime.Now;
				dataRow["HishopMsg"] = msg;
				if (!string.IsNullOrEmpty(sign))
				{
					dataRow["HishopSign"] = sign;
				}
				if (!string.IsNullOrEmpty(url))
				{
					dataRow["HishopUrl"] = url;
				}
				dataTable.Rows.Add(dataRow);
				string fileName = Globals.GetphysicsPath(logPath);
				dataTable.WriteXml(fileName);
			}
			catch (Exception innerException)
			{
				throw new Exception("写日志失败:" + logPath, innerException);
			}
		}

		public static void AppendLog(IDictionary<string, string> param, string msg, string sign = "", string url = "", string logPath = "")
		{
			object obj = new object();
			lock (obj)
			{
				DateTime now;
				if (string.IsNullOrEmpty(logPath))
				{
					now = DateTime.Now;
					logPath = "/log/error_" + now.ToString("yyyyMMddHHmm") + ".txt";
				}
				else if (logPath.IndexOf('/') == -1 && logPath.IndexOf('.') == -1)
				{
					string[] obj2 = new string[5]
					{
						"/log/",
						logPath,
						"_",
						null,
						null
					};
					now = DateTime.Now;
					obj2[3] = now.ToString("yyyyMMddHHmm");
					obj2[4] = ".txt";
					logPath = string.Concat(obj2);
				}
				string path = Globals.GetphysicsPath(logPath);
				Globals.CreatePath(path);
				using (StreamWriter streamWriter = File.AppendText(path))
				{
					StreamWriter streamWriter2 = streamWriter;
					now = DateTime.Now;
					streamWriter2.WriteLine("时间：" + now.ToString());
					if (param != null)
					{
						foreach (KeyValuePair<string, string> item in param)
						{
							streamWriter.WriteLine(item.Key + ":" + item.Value);
						}
					}
					if (!string.IsNullOrEmpty(url))
					{
						streamWriter.WriteLine("HishopLogUrl:" + url);
					}
					if (!string.IsNullOrEmpty(sign))
					{
						streamWriter.WriteLine("HishopLogSign:" + sign);
					}
					streamWriter.WriteLine("HishopLogMsg:" + msg);
					streamWriter.WriteLine("");
					streamWriter.WriteLine("");
				}
			}
		}

		public static void AppendLog(NameValueCollection param, string msg, string sign = "", string url = "", string logPath = "")
		{
			object obj = new object();
			lock (obj)
			{
				DateTime now;
				if (string.IsNullOrEmpty(logPath))
				{
					now = DateTime.Now;
					logPath = "/log/error_" + now.ToString("yyyyMMddHHmm") + ".txt";
				}
				else if (logPath.IndexOf('/') == -1 && logPath.IndexOf('.') == -1)
				{
					string[] obj2 = new string[5]
					{
						"/log/",
						logPath,
						"_",
						null,
						null
					};
					now = DateTime.Now;
					obj2[3] = now.ToString("yyyyMMddHHmm");
					obj2[4] = ".txt";
					logPath = string.Concat(obj2);
				}
				string path = Globals.GetphysicsPath(logPath);
				Globals.CreatePath(path);
				using (StreamWriter streamWriter = File.AppendText(path))
				{
					StreamWriter streamWriter2 = streamWriter;
					now = DateTime.Now;
					streamWriter2.WriteLine("时间：" + now.ToString());
					if (param != null)
					{
						string[] allKeys = param.AllKeys;
						foreach (string text in allKeys)
						{
							streamWriter.WriteLine(text + ":" + param[text]);
						}
					}
					if (!string.IsNullOrEmpty(url))
					{
						streamWriter.WriteLine("HishopUrl:" + url);
					}
					if (!string.IsNullOrEmpty(sign))
					{
						streamWriter.WriteLine("HishopSign:" + sign);
					}
					streamWriter.WriteLine("Hishopmsg:" + msg);
					streamWriter.WriteLine("");
					streamWriter.WriteLine("");
				}
			}
		}

		public static void AppendLog(string msg, string sign = "", string url = "", string logPath = "")
		{
			object obj = new object();
			lock (obj)
			{
				DateTime now;
				if (string.IsNullOrEmpty(logPath))
				{
					now = DateTime.Now;
					logPath = "/log/error_" + now.ToString("yyyyMMddHHmm") + ".txt";
				}
				else if (logPath.IndexOf('/') == -1 && logPath.IndexOf('.') == -1)
				{
					string[] obj2 = new string[5]
					{
						"/log/",
						logPath,
						"_",
						null,
						null
					};
					now = DateTime.Now;
					obj2[3] = now.ToString("yyyyMMddHHmm");
					obj2[4] = ".txt";
					logPath = string.Concat(obj2);
				}
				string path = Globals.GetphysicsPath(logPath);
				Globals.CreatePath(path);
				using (StreamWriter streamWriter = File.AppendText(path))
				{
					StreamWriter streamWriter2 = streamWriter;
					now = DateTime.Now;
					streamWriter2.WriteLine("时间：" + now.ToString());
					if (!string.IsNullOrEmpty(url))
					{
						streamWriter.WriteLine("HishopUrl:" + url);
					}
					if (!string.IsNullOrEmpty(sign))
					{
						streamWriter.WriteLine("HishopSign:" + sign);
					}
					streamWriter.WriteLine("Hishopmsg:" + msg);
					streamWriter.WriteLine("");
					streamWriter.WriteLine("");
				}
			}
		}

		public static string RndStr(int length)
		{
			Random random = new Random();
			string text = "";
			for (int i = 1; i <= length; i++)
			{
				text += random.Next(0, 9).ToString();
			}
			return text;
		}

		public static string GetHiddenStr(int len, string hidStr = "*")
		{
			string text = "";
			for (int i = 0; i < len; i++)
			{
				text += "*";
			}
			return text;
		}

		public static string RndStr(int length, bool IsUpper)
		{
			Random random = new Random();
			string text = "";
			int i = 1;
			char c;
			if (IsUpper)
			{
				for (; i <= length; i++)
				{
					string str = text;
					c = (char)random.Next(65, 91);
					text = str + c.ToString();
				}
			}
			else
			{
				for (; i <= length; i++)
				{
					string str2 = text;
					c = (char)random.Next(97, 122);
					text = str2 + c.ToString();
				}
			}
			return text;
		}

		public static string GetXmlNodeValue(string configXML, string nodeName)
		{
			string result = "";
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.LoadXml(configXML);
				XmlNode documentElement = xmlDocument.DocumentElement;
				XmlNode xmlNode = documentElement.SelectSingleNode(nodeName);
				if (xmlNode != null)
				{
					result = xmlNode.InnerText;
				}
			}
			catch
			{
			}
			return result;
		}

		public static string GetResponseResult(string url)
		{
			try
			{
				WebRequest webRequest = WebRequest.Create(url);
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
				{
					using (Stream stream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Url", url);
				Globals.WriteExceptionLog(ex, dictionary, "GetResponseResult");
				return "";
			}
		}

		public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public static string GetPostResult(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
				ServicePointManager.ServerCertificateValidationCallback = Globals.CheckValidationResult;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = bytes.Length;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream2 = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						Stream stream3 = stream2;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream3 = new GZipStream(stream2, CompressionMode.Decompress);
						}
						else if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
						{
							stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
						}
						using (StreamReader streamReader = new StreamReader(stream3, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Url", url);
				dictionary.Add("PostData", postData);
				Globals.WriteExceptionLog(ex, dictionary, "GetPostResult");
				result = "";
			}
			return result;
		}

		public static string GetStringByRegularExpression(string input, string pattern)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Match item in Regex.Matches(input, pattern))
			{
				stringBuilder.Append(item.Value);
			}
			return stringBuilder.ToString();
		}

		public static long DateTimeToUnixTimestamp(DateTime dateTime)
		{
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			return Convert.ToInt64((dateTime - d).TotalSeconds);
		}

		public static DateTime UnixTimestampToDateTime(this DateTime target, long timestamp)
		{
			DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			long ticks = long.Parse(timestamp + "0000000");
			TimeSpan value = new TimeSpan(ticks);
			return dateTime.Add(value);
		}

		public static void DeleteFolder(string targetFolderName, bool isReservedFolder = false, string defaultFolder = "/Storage/master/")
		{
			string path = defaultFolder + targetFolderName;
			string path2 = $"{HttpContext.Current.Server.MapPath(path)}";
			DirectoryInfo directoryInfo = new DirectoryInfo(path2);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete(true);
				directoryInfo = new DirectoryInfo(path2);
				if (!directoryInfo.Exists & isReservedFolder)
				{
					Directory.CreateDirectory(path2);
				}
			}
		}

		public static string SaveFile(string targetFolderName, string fileURL, string defaultFolder = "/Storage/master/", bool overwrite = true, bool physicalPath = false, string additiveFileName = "")
		{
			if (fileURL.IndexOf("/temp/") < 0)
			{
				return fileURL;
			}
			string empty = string.Empty;
			string text = defaultFolder + targetFolderName + "\\";
			string text2 = $"{HttpContext.Current.Server.MapPath(text)}";
			if (!Globals.PathExist(text2, false))
			{
				Globals.CreatePath(text2);
			}
			string str = (fileURL.Split('/').Length == 6) ? fileURL.Split('/')[5] : fileURL.Split('/')[4];
			if (!string.IsNullOrEmpty(additiveFileName))
			{
				str = additiveFileName + str;
			}
			string text3 = text2 + str;
			string text4 = HttpContext.Current.Server.MapPath(fileURL);
			if (File.Exists(text4))
			{
				File.Copy(text4, text3, overwrite);
			}
			empty = (physicalPath ? text3 : (text + str));
			string path = HttpContext.Current.Server.MapPath(fileURL);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return empty.Replace("\\", "/").Replace("//", "/");
		}

		public static string CreateQRCode(string content, string qrCodeUrl, bool IsOverWrite = false, ImageFormats imgFomrats = ImageFormats.Png)
		{
			try
			{
				ImageFormat png = ImageFormat.Png;
				switch (imgFomrats)
				{
				case ImageFormats.Bmp:
					png = ImageFormat.Bmp;
					break;
				case ImageFormats.Emf:
					png = ImageFormat.Emf;
					break;
				case ImageFormats.Exif:
					png = ImageFormat.Exif;
					break;
				case ImageFormats.Gif:
					png = ImageFormat.Gif;
					break;
				case ImageFormats.Icon:
					png = ImageFormat.Icon;
					break;
				case ImageFormats.Jpeg:
					png = ImageFormat.Jpeg;
					break;
				case ImageFormats.MemoryBmp:
					png = ImageFormat.MemoryBmp;
					break;
				case ImageFormats.Png:
					png = ImageFormat.Png;
					break;
				case ImageFormats.Tiff:
					png = ImageFormat.Tiff;
					break;
				case ImageFormats.Wmf:
					png = ImageFormat.Wmf;
					break;
				default:
					png = ImageFormat.Png;
					break;
				}
				qrCodeUrl = qrCodeUrl.Replace("\\", "/");
				string path = qrCodeUrl.Substring(0, qrCodeUrl.LastIndexOf('/'));
				string text = Globals.GetphysicsPath(qrCodeUrl);
				if (File.Exists(text) && !IsOverWrite)
				{
					return qrCodeUrl;
				}
				QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
				qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
				qRCodeEncoder.QRCodeScale = 10;
				qRCodeEncoder.QRCodeVersion = 0;
				qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
				string path2 = Globals.GetphysicsPath(path);
				Bitmap bitmap = qRCodeEncoder.Encode(content);
				if (Directory.Exists(path2))
				{
					bitmap.Save(text, png);
				}
				else
				{
					Globals.CreatePath(path);
					bitmap.Save(text, png);
				}
				bitmap.Dispose();
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, null, "CreateQrCode");
			}
			return qrCodeUrl;
		}

		public static MemoryStream GenerateTwoDimensionalImage(string url)
		{
			QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
			qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
			qRCodeEncoder.QRCodeScale = 10;
			qRCodeEncoder.QRCodeVersion = 0;
			qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
			Bitmap bitmap = qRCodeEncoder.Encode(url);
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Png);
			return memoryStream;
		}

		public static bool ValidateCertFile(string fileName)
		{
			fileName = fileName.ToUpper();
			if (fileName.Contains(".CRT") || fileName.Contains(".CER") || fileName.Contains(".P12") || fileName.Contains(".P7B") || fileName.Contains(".P7C") || fileName.Contains(".SPC") || fileName.Contains(".KEY") || fileName.Contains(".DER") || fileName.Contains(".PEM") || fileName.Contains(".PFX"))
			{
				return true;
			}
			return false;
		}

		public static string GetEmailSafeStr(string email, int start, int len)
		{
			if (string.IsNullOrEmpty(email))
			{
				return "";
			}
			string str = email.Split('@')[0];
			str = Globals.GetSafeStr(str, start, len);
			if (DataHelper.IsEmail(email))
			{
				return str + "@" + email.Split('@')[1];
			}
			return str;
		}

		public static string GetSafeStr(string str, int start, int len)
		{
			if (start > str.Length || start < 0)
			{
				return str;
			}
			if (str.Length > start + len && len > 0)
			{
				return str.Substring(0, start) + Globals.GetHideStr(len, "*") + str.Substring(start + len);
			}
			if (len == 0)
			{
				len = str.Length - start;
			}
			return str.Substring(0, start) + Globals.GetHideStr(len, "*");
		}

		private static string GetHideStr(int len, string hideChar = "*")
		{
			string text = "";
			for (int i = 0; i < len; i++)
			{
				text += hideChar;
			}
			return text;
		}

		public static string GetHideUserName(string str)
		{
			if (str.Length <= 1)
			{
				return str;
			}
			return str.Substring(0, 1) + Globals.GetHideStr(3, "*") + str.Substring(str.Length - 1);
		}

		public static IDictionary<string, string> NameValueCollectionToDictionary(NameValueCollection collection)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			if (collection == null && collection.AllKeys.Length == 0)
			{
				return dictionary;
			}
			string[] allKeys = collection.AllKeys;
			foreach (string text in allKeys)
			{
				if (!string.IsNullOrEmpty(text) && !dictionary.ContainsKey(text))
				{
					dictionary.Add(new KeyValuePair<string, string>(text, collection[text]));
				}
			}
			return dictionary;
		}

		public static void WriteExceptionLog_Page(Exception ex, NameValueCollection param, string fileName = "Exception")
		{
			IDictionary<string, string> dictionary = Globals.NameValueCollectionToDictionary(param);
			if (!(ex is ThreadAbortException))
			{
				dictionary.Add("ErrorMessage", ex.Message);
				dictionary.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					dictionary.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					dictionary.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					dictionary.Add("TargetSite", ex.TargetSite.ToString());
				}
				dictionary.Add("ExSource", ex.Source);
				string url = "";
				if (HttpContext.Current != null)
				{
					url = HttpContext.Current.Request.Url.ToString();
				}
				Globals.AppendLog(dictionary, "", "", url, fileName);
			}
		}

		public static void WriteExceptionLog(Exception ex, IDictionary<string, string> iParam = null, string fileName = "Exception")
		{
			if (iParam == null)
			{
				iParam = new Dictionary<string, string>();
			}
			if (!(ex is ThreadAbortException))
			{
				iParam.Add("ErrorMessage", ex.Message);
				iParam.Add("StackTrace", ex.StackTrace);
				if (ex.InnerException != null)
				{
					iParam.Add("InnerException", ex.InnerException.ToString());
				}
				if (ex.GetBaseException() != null)
				{
					iParam.Add("BaseException", ex.GetBaseException().Message);
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					iParam.Add("TargetSite", ex.TargetSite.ToString());
				}
				iParam.Add("ExSource", ex.Source);
				string url = "";
				if (HttpContext.Current != null)
				{
					url = HttpContext.Current.Request.Url.ToString();
				}
				Globals.AppendLog(iParam, "", "", url, fileName);
			}
		}

		public static string GetProtocal(HttpContext context = null)
		{
			if (context != null)
			{
				return context.Request.IsSecureConnection ? "https" : "http";
			}
			context = HttpContext.Current;
			return (context == null || !context.Request.IsSecureConnection) ? "http" : "https";
		}
	}
}
