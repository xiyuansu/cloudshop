using Hishop.Weixin.Pay.Domain;
using LitJson;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace Hishop.Weixin.Pay.Lib
{
	public class WxPayData
	{
		private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

		public void SetValue(string key, object value)
		{
			this.m_values[key] = value;
		}

		public object GetValue(string key)
		{
			object result = null;
			this.m_values.TryGetValue(key, out result);
			return result;
		}

		public bool IsSet(string key)
		{
			object obj = null;
			this.m_values.TryGetValue(key, out obj);
			if (null != obj)
			{
				return true;
			}
			return false;
		}

		public string ToXml()
		{
			if (0 == this.m_values.Count)
			{
				WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "WxPayData数据为空!", LogType.Error);
			}
			StringBuilder stringBuilder = new StringBuilder("<?xml version=\"1.0\" standalone=\"true\"?>");
			stringBuilder.AppendLine("<xml>");
			foreach (KeyValuePair<string, object> value in this.m_values)
			{
				if (value.Value == null)
				{
					WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "WxPayData内部含有值为null的字段!", LogType.Error);
					return "";
				}
				if (value.Value.GetType() == typeof(int))
				{
					stringBuilder.AppendLine("<" + value.Key + ">" + value.Value + "</" + value.Key + ">");
				}
				else if (value.Value.GetType() == typeof(string))
				{
					stringBuilder.AppendLine("<" + value.Key + "><![CDATA[" + value.Value + "]]></" + value.Key + ">");
				}
				else
				{
					WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "WxPayData字段数据类型错误!", LogType.Error);
				}
			}
			stringBuilder.AppendLine("</xml>");
			return stringBuilder.ToString();
		}

		public SortedDictionary<string, object> FromXml(string xml, string key)
		{
			if (string.IsNullOrEmpty(xml))
			{
				WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "将空的xml串转换为WxPayData不合法!", LogType.Error);
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNode firstChild = xmlDocument.FirstChild;
			XmlNodeList childNodes = firstChild.ChildNodes;
			foreach (XmlNode item in childNodes)
			{
				XmlElement xmlElement = (XmlElement)item;
				this.m_values[xmlElement.Name] = xmlElement.InnerText;
			}
			try
			{
				this.CheckSign(key);
			}
			catch (WxPayException ex)
			{
				WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), ex.Message, LogType.Error);
			}
			return this.m_values;
		}

		public string ToUrl()
		{
			string text = "";
			foreach (KeyValuePair<string, object> value in this.m_values)
			{
				if (value.Value == null)
				{
					WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "WxPayData内部含有值为null的字段!", LogType.Error);
				}
				if (value.Key != "sign" && value.Value.ToString() != "")
				{
					object obj = text;
					text = obj + value.Key + "=" + value.Value + "&";
				}
			}
			return text.Trim('&');
		}

		public string ToJson()
		{
			return JsonMapper.ToJson(this.m_values);
		}

		public string ToPrintStr()
		{
			string text = "";
			foreach (KeyValuePair<string, object> value in this.m_values)
			{
				if (value.Value == null)
				{
					WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "WxPayData内部含有值为null的字段!", LogType.Error);
				}
				text += $"{value.Key}={value.Value.ToString()}<br>";
			}
			return text;
		}

		public string MakeSign(string key)
		{
			string str = this.ToUrl();
			str = str + "&key=" + key;
			MD5 mD = MD5.Create();
			byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(str));
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString().ToUpper();
		}

		public bool CheckSign(string key)
		{
			if (!this.IsSet("sign"))
			{
				return true;
			}
			if (this.GetValue("sign") == null || this.GetValue("sign").ToString() == "")
			{
				WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "WxPayData签名存在但不合法!", LogType.Error);
			}
			string b = this.GetValue("sign").ToString();
			string a = this.MakeSign(key);
			if (a == b)
			{
				return true;
			}
			WxPayLog.writeLog(this.GetParam(), "", HttpContext.Current.Request.Url.ToString(), "WxPayData签名验证错误!", LogType.Error);
			return false;
		}

		public SortedDictionary<string, object> GetValues()
		{
			return this.m_values;
		}

		public IDictionary<string, string> GetParam()
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string key in this.m_values.Keys)
			{
				dictionary.Add(key, this.m_values[key].ToString());
			}
			return dictionary;
		}
	}
}
