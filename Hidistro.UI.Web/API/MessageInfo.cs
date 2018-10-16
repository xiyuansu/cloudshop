using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class MessageInfo
	{
		public static string ShowMessageInfo(ApiErrorCode messageenum, string field, string sign = "", Exception ex = null)
		{
			NameValueCollection param = new NameValueCollection
			{
				HttpContext.Current.Request.Form,
				HttpContext.Current.Request.QueryString
			};
			string text = "";
			if (ex != null)
			{
				text = text + "<ErrMsg>" + HttpUtility.HtmlEncode(ex.Message) + "</ErrMsg>";
				text = text + "<StackTrace>" + HttpUtility.HtmlEncode(ex.StackTrace) + "</StackTrace>";
				if (ex.InnerException != null)
				{
					text = text + "<InnerException>" + HttpUtility.HtmlEncode(ex.InnerException.ToString()) + "</InnerException>";
				}
				if (ex.GetBaseException() != null)
				{
					text = text + "<BaseException>" + HttpUtility.HtmlEncode(ex.GetBaseException().Message) + "</BaseException>";
				}
				if (ex.TargetSite != (MethodBase)null)
				{
					text = text + "<TargetSite>" + HttpUtility.HtmlEncode(ex.TargetSite.ToString()) + "</TargetSite>";
				}
				text = text + "<ExSource>" + HttpUtility.HtmlEncode(ex.TargetSite.ToString()) + "</ExSource>";
			}
			string format = "<error_response><code>{0}</code><msg>" + field + " {1}</msg></error_response>";
			string format2 = "<error_response><code>{0}</code><msg>" + field + " {1}</msg><sign>" + sign + "</sign></error_response>";
			string text2 = "<error_response><code>{0}</code><msg>" + field + " {1}</msg>{2}</error_response>";
			switch (messageenum)
			{
			case ApiErrorCode.Paramter_Error:
				format = string.Format(format, 101, "is error");
				break;
			case ApiErrorCode.Format_Eroor:
				format = string.Format(format, 102, "format is error");
				break;
			case ApiErrorCode.Signature_Error:
				format = string.Format(format2, 103, "signature is error");
				break;
			case ApiErrorCode.Empty_Error:
				format = string.Format(format, 104, "is empty");
				break;
			case ApiErrorCode.Exists_Error:
				format = string.Format(format, 105, "is exists");
				break;
			case ApiErrorCode.NoExists_Error:
				format = string.Format(format, 105, "is not exists");
				break;
			case ApiErrorCode.Group_Error:
				format = string.Format(format, 108, "is not the end grouporder");
				break;
			case ApiErrorCode.NoPay_Error:
				format = string.Format(format, 109, "is not pay money");
				break;
			case ApiErrorCode.NoShippingMode:
				format = string.Format(format, 110, "is not shippingmodel");
				break;
			case ApiErrorCode.ShipingOrderNumber_Error:
				format = string.Format(format, 111, "is shippingordernumer error");
				break;
			case ApiErrorCode.Session_Empty:
				format = string.Format(format, 200, "sessionId is no exist");
				break;
			case ApiErrorCode.Session_Error:
				format = string.Format(format, 201, "sessionId is no Invalid");
				break;
			case ApiErrorCode.Session_TimeOut:
				format = string.Format(format, 202, "is timeout");
				break;
			case ApiErrorCode.Username_Exist:
				format = string.Format(format, 203, "account is Exist");
				break;
			case ApiErrorCode.Paramter_Diffrent:
				format = string.Format(format, 107, "is diffrent");
				break;
			case ApiErrorCode.Ban_Register:
				format = string.Format(format, 204, "Prohibition on registration");
				break;
			case ApiErrorCode.SaleState_Error:
				format = string.Format(format, 300, "cant not buy product");
				break;
			case ApiErrorCode.Stock_Error:
				format = string.Format(format, 301, "stock is lack");
				break;
			default:
				format = string.Format(format, 999, "unknown_Error");
				break;
			}
			Globals.WriteLog(param, format, text, "", "API");
			return format;
		}

		public static string GetSkuContent(string skuId)
		{
			string text = "";
			if (!string.IsNullOrEmpty(skuId.Trim()))
			{
				DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(skuId);
				foreach (DataRow row in productInfoBySku.Rows)
				{
					if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
					{
						text = text + row["AttributeName"] + ":" + row["ValueStr"] + "; ";
					}
				}
			}
			return (text == "") ? "不存在" : text;
		}

		public static string GetOrderSkuContent(string skucontent)
		{
			string text = "";
			skucontent = skucontent.Replace("；", "");
			if (!string.IsNullOrEmpty(skucontent) && skucontent.IndexOf("：") >= 0)
			{
				text = skucontent.Substring(0, skucontent.IndexOf("："));
			}
			return (text == "") ? "不存在" : text;
		}

		public static Dictionary<string, string> GetShippingRegion(string regionstr)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{
					"Province",
					""
				},
				{
					"City",
					""
				},
				{
					"District",
					""
				}
			};
			string[] array = regionstr.Split('，');
			if (array.Length >= 1)
			{
				dictionary["Province"] = array[0].ToString();
			}
			if (array.Length >= 2)
			{
				dictionary["City"] = array[1].ToString();
			}
			if (array.Length >= 3)
			{
				dictionary["District"] = array[2].ToString();
			}
			return dictionary;
		}

		public static string GetDesciption(string note, string localhost)
		{
			string text = "";
			text = note.Replace("src=\"/Storage/master/gallery", "src=\"" + localhost + "/Storage/master/gallery");
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			return Convert.ToBase64String(bytes);
		}
	}
}
