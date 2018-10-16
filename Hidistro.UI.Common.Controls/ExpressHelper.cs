using Hidistro.Core;
using Hidistro.Entities.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Common.Controls
{
	public static class ExpressHelper
	{
		private static string path = HttpContext.Current.Request.MapPath("~/Express.xml");

		public static ExpressCompanyInfo FindNode(string company)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = $"//company[@name='{company}']";
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = company;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = xmlNode2.Attributes["TaobaoCode"].Value;
				expressCompanyInfo.JDCode = string.Concat(xmlNode2.Attributes["JDCode"]);
				expressCompanyInfo.CloseStatus = xmlNode2.Attributes["CloseStatus"].Value.ToBool();
			}
			return expressCompanyInfo;
		}

		public static ExpressCompanyInfo FindNodeLikeName(string company)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = $"//company[contains(@name,'{company}')]";
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = xmlNode2.Attributes["name"].Value;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = xmlNode2.Attributes["TaobaoCode"].Value;
				expressCompanyInfo.JDCode = xmlNode2.Attributes["JDCode"].Value;
				expressCompanyInfo.CloseStatus = xmlNode2.Attributes["CloseStatus"].Value.ToBool();
			}
			return expressCompanyInfo;
		}

		public static ExpressCompanyInfo FindNodeByCode(string code)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = $"//company[@TaobaoCode='{code}']";
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = xmlNode2.Attributes["name"].Value;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = code;
				expressCompanyInfo.JDCode = xmlNode2.Attributes["JDCode"].Value;
				expressCompanyInfo.CloseStatus = xmlNode2.Attributes["CloseStatus"].Value.ToBool();
			}
			return expressCompanyInfo;
		}

		public static IList<ExpressCompanyInfo> GetAllExpress(bool IsContainClose)
		{
			IList<ExpressCompanyInfo> list = new List<ExpressCompanyInfo>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode childNode in xmlNode2.ChildNodes)
			{
				ExpressCompanyInfo expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = childNode.Attributes["name"].Value;
				expressCompanyInfo.Kuaidi100Code = childNode.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = childNode.Attributes["TaobaoCode"].Value;
				if (childNode.Attributes["JDCode"] != null)
				{
					expressCompanyInfo.JDCode = childNode.Attributes["JDCode"].Value;
				}
				if (childNode.Attributes["CloseStatus"] != null)
				{
					expressCompanyInfo.CloseStatus = childNode.Attributes["CloseStatus"].Value.ToBool();
				}
				if (!IsContainClose)
				{
					if (expressCompanyInfo.CloseStatus)
					{
						list.Add(expressCompanyInfo);
					}
				}
				else
				{
					list.Add(expressCompanyInfo);
				}
			}
			return list;
		}

		public static IList<string> GetAllExpressName(bool IsContainClose)
		{
			IList<string> list = new List<string>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode childNode in xmlNode2.ChildNodes)
			{
				if (!IsContainClose)
				{
					if (childNode.Attributes["CloseStatus"].Value.ToBool())
					{
						list.Add(childNode.Attributes["name"].Value);
					}
				}
				else
				{
					list.Add(childNode.Attributes["name"].Value);
				}
			}
			return list;
		}

		public static DataTable GetExpressTable()
		{
			DataTable dataTable = new DataTable();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			dataTable.Columns.Add("Name");
			dataTable.Columns.Add("Kuaidi100Code");
			dataTable.Columns.Add("TaobaoCode");
			dataTable.Columns.Add("JDCode");
			dataTable.Columns.Add("CloseStatus");
			foreach (XmlNode childNode in xmlNode2.ChildNodes)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["Name"] = childNode.Attributes["name"].Value;
				dataRow["Kuaidi100Code"] = childNode.Attributes["Kuaidi100Code"].Value;
				dataRow["TaobaoCode"] = childNode.Attributes["TaobaoCode"].Value;
				if (childNode.Attributes["JDCode"] != null)
				{
					dataRow["JDCode"] = childNode.Attributes["JDCode"].Value;
				}
				if (childNode.Attributes["CloseStatus"] != null)
				{
					dataRow["CloseStatus"] = childNode.Attributes["CloseStatus"].Value.ToBool();
				}
				dataTable.Rows.Add(dataRow);
			}
			DataView defaultView = dataTable.DefaultView;
			defaultView.Sort = "CloseStatus desc";
			return defaultView.ToTable();
		}

		public static void DeleteExpress(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode childNode in xmlNode2.ChildNodes)
			{
				if (childNode.Attributes["name"].Value == name)
				{
					xmlNode2.RemoveChild(childNode);
					break;
				}
			}
			xmlNode.Save(ExpressHelper.path);
		}

		public static void AddExpress(string name, string kuaidi100Code, string taobaoCode, string JDCode)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			XmlElement xmlElement = xmlNode.CreateElement("company");
			xmlElement.SetAttribute("name", name);
			xmlElement.SetAttribute("Kuaidi100Code", kuaidi100Code);
			xmlElement.SetAttribute("TaobaoCode", taobaoCode);
			xmlElement.SetAttribute("JDCode", JDCode);
			xmlElement.SetAttribute("CloseStatus", "1");
			xmlNode2.AppendChild(xmlElement);
			xmlNode.Save(ExpressHelper.path);
		}

		public static bool IsExitExpress(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode childNode in xmlNode2.ChildNodes)
			{
				if (childNode.Attributes["name"].Value == name)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsExitExpressForDZMD(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode childNode in xmlNode2.ChildNodes)
			{
				if (childNode.Attributes["name"].Value.Contains(name))
				{
					return true;
				}
			}
			return false;
		}

		public static XmlAttribute CreateAttribute(XmlNode node, string attributeName, string value)
		{
			try
			{
				XmlDocument ownerDocument = node.OwnerDocument;
				XmlAttribute xmlAttribute = null;
				xmlAttribute = ownerDocument.CreateAttribute(attributeName);
				xmlAttribute.Value = value;
				node.Attributes.SetNamedItem(xmlAttribute);
				return xmlAttribute;
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return null;
			}
		}

		public static void UpdateExpress(string oldcompanyname, string name, string kuaidi100Code, string taobaoCode, string JDCode)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode childNode in xmlNode2.ChildNodes)
			{
				if (childNode.Attributes["name"].Value == oldcompanyname)
				{
					childNode.Attributes["name"].Value = name;
					childNode.Attributes["Kuaidi100Code"].Value = kuaidi100Code;
					childNode.Attributes["TaobaoCode"].Value = taobaoCode;
					if (childNode.Attributes["JDCode"] != null)
					{
						childNode.Attributes["JDCode"].Value = JDCode;
					}
					else
					{
						childNode.Attributes.Append(ExpressHelper.CreateAttribute(childNode, "JDCode", JDCode));
					}
					break;
				}
			}
			xmlNode.Save(ExpressHelper.path);
		}

		public static bool UpdateStaut(string name, bool Staut)
		{
			try
			{
				bool result = false;
				XmlDocument xmlNode = ExpressHelper.GetXmlNode();
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
				foreach (XmlNode childNode in xmlNode2.ChildNodes)
				{
					if (childNode.Attributes["name"].Value == name)
					{
						if (childNode.Attributes["CloseStatus"] != null)
						{
							childNode.Attributes["CloseStatus"].Value = (Staut ? "1" : "0");
						}
						else
						{
							childNode.Attributes.Append(ExpressHelper.CreateAttribute(childNode, "CloseStatus", Staut ? "1" : "0"));
						}
						result = true;
						break;
					}
				}
				xmlNode.Save(ExpressHelper.path);
				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static string GetDataByKuaidi100(string computer, string expressNo)
		{
			string text = "暂时没有此快递单号的信息";
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			string text2 = "";
			string text3 = "";
			if (xmlNode2 != null)
			{
				text2 = xmlNode2.Attributes["app_key"].Value;
				text3 = xmlNode2.Attributes["appSecret"].Value;
			}
			if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				string text4 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("app_key", text2);
				dictionary.Add("timestamp", text4);
				dictionary.Add("shipperCode", computer);
				dictionary.Add("logisticsCode", expressNo);
				string text5 = HiCryptographer.SignTopRequest(dictionary, text3);
				dictionary.Add("RequestUrl", $"http://wuliu.huz.cn/api/logistics?app_key={text2}&timestamp={text4}&shipperCode={computer}&logisticsCode={expressNo}&sign={text5}");
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"http://wuliu.huz.cn/api/logistics?app_key={text2}&timestamp={text4}&shipperCode={computer}&logisticsCode={expressNo}&sign={text5}");
				httpWebRequest.Timeout = 8000;
				HttpWebResponse httpWebResponse;
				try
				{
					httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				}
				catch (Exception ex)
				{
					Globals.WriteExceptionLog(ex, dictionary, "GetDataByKuaidi100");
					return text;
				}
				if (httpWebResponse.StatusCode == HttpStatusCode.OK)
				{
					Stream responseStream = httpWebResponse.GetResponseStream();
					StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
					text = streamReader.ReadToEnd();
					text = text.Replace("&amp;", "");
					text = text.Replace("&nbsp;", "");
					text = text.Replace("&", "");
					if (!string.IsNullOrEmpty(text))
					{
						text = ExpressHelper.ConvertToObjectJson(text, computer, expressNo);
					}
				}
				if (text == "" || text == "暂时没有此快递单号的信息")
				{
					Globals.WriteLog(dictionary, "未获取到物流信息", "", "", "GetDataByKuaidi100");
				}
			}
			return text;
		}

		private static string ConvertToObjectJson(string content, string computer, string expressNo)
		{
			var anonymousTypeObject = new
			{
				shipperCode = "",
				logisticsCode = "",
				success = "",
				traces = new[]
				{
					new
					{
						acceptTime = "",
						acceptStation = ""
					}
				}
			};
			var anon = JsonConvert.DeserializeAnonymousType(content, anonymousTypeObject);
			if (anon.traces == null || anon.traces.Length == 0)
			{
				return content;
			}
			var enumeration = from a in anon.traces
			where a.acceptTime.ToDateTime().HasValue
			orderby a.acceptTime.ToDateTime().Value descending
			select a;
			string result = "{\"success\":" + anon.success.ToLower() + ",\"shipperCode\":\"" + computer + "\",\"logisticsCode\":\"" + expressNo + "\",\"traces\":[";
			enumeration.ForEach(x =>
			{
				result += "{";
				result = result + "\"acceptStation\":\"" + x.acceptStation + "\",";
				result = result + "\"acceptTime\":\"" + x.acceptTime + "\"";
				result += "},";
			});
			if (result.EndsWith(","))
			{
				result = result.TrimEnd(',');
			}
			result += "]";
			result += "}";
			return result;
		}

		private static XmlDocument GetXmlNode()
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(ExpressHelper.path))
			{
				xmlDocument.Load(ExpressHelper.path);
			}
			return xmlDocument;
		}
	}
}
