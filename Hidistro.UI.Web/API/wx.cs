using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Statistics;
using Hishop.Weixin.MP.Util;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Xml.Linq;

namespace Hidistro.UI.Web.API
{
	public class wx : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			NameValueCollection nameValueCollection = new NameValueCollection
			{
				context.Request.Form,
				context.Request.QueryString
			};
			HttpRequest request = context.Request;
			string weixinToken = SettingsManager.GetMasterSettings().WeixinToken;
			string signature = request["signature"];
			string nonce = request["nonce"];
			string timestamp = request["timestamp"];
			string s = request["echostr"];
			if (request.HttpMethod == "GET")
			{
				if (Hishop.Weixin.MP.Util.CheckSignature.Check(signature, timestamp, nonce, weixinToken))
				{
					context.Response.Write(s);
				}
				else
				{
					context.Response.Write("");
				}
				context.Response.End();
			}
			else
			{
				try
				{
					XDocument xDocument = null;
					if (this.NewWXHandler(request, out xDocument))
					{
						this.SaveMenuClick(xDocument);
					}
					else
					{
						this.SaveMenuClick(xDocument);
						CustomMsgHandler customMsgHandler = new CustomMsgHandler(xDocument.ToString());
						customMsgHandler.Execute();
						context.Response.Write(customMsgHandler.ResponseDocument);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		private void SaveMenuClick(XDocument doc)
		{
			if (doc != null)
			{
				XElement xElement = doc.Root.Element("MsgType");
				if (xElement != null)
				{
					string text = "";
					string obj = "";
					string obj2 = "";
					string obj3 = "";
					int num = 0;
					string value = xElement.Value;
					if (value == "event")
					{
						xElement = doc.Root.Element("Event");
						if (xElement != null)
						{
							obj2 = xElement.Value;
						}
						string a = obj2.ToNullString().ToLower();
						if (a == "click" || a == "view" || a == "link")
						{
							xElement = doc.Root.Element("FromUserName");
							if (xElement != null)
							{
								text = xElement.Value;
							}
							xElement = doc.Root.Element("CreateTime");
							if (xElement != null)
							{
								obj3 = xElement.Value;
							}
							xElement = doc.Root.Element("EventKey");
							if (xElement != null)
							{
								obj = xElement.Value;
							}
							xElement = doc.Root.Element("MenuId");
							if (xElement != null)
							{
								num = xElement.Value.ToInt(0);
							}
							IDictionary<string, string> dictionary = new Dictionary<string, string>();
							dictionary.Add("CreateTime", obj3.ToNullString());
							dictionary.Add("Event", obj2.ToNullString());
							dictionary.Add("EventKey", obj.ToNullString());
							dictionary.Add("FromUserName", text.ToNullString());
							dictionary.Add("MsgType", value.ToNullString());
							dictionary.Add("menuId", num.ToNullString());
							int num2 = obj.ToInt(0);
							if (num2 == 0)
							{
								num2 = num;
							}
							try
							{
								WXFansHelper.UpdateClickStatistical(text, num2);
							}
							catch (Exception ex)
							{
								Globals.WriteExceptionLog(ex, dictionary, "WXMenuClick_Ex");
							}
						}
					}
				}
			}
		}

		private bool NewWXHandler(HttpRequest Request, out XDocument wxDocumentTarget)
		{
			NameValueCollection param = new NameValueCollection
			{
				Request.Form,
				Request.QueryString
			};
			bool result = false;
			wxDocumentTarget = null;
			try
			{
				wxDocumentTarget = Senparc.Weixin.MP.RequestMessageFactory.GetRequestEntityDocument(Request.InputStream);
				RequestMessageBase requestMessageBase = null;
				RequestMsgType requestMsgType = Senparc.Weixin.MP.Helpers.MsgTypeHelper.GetRequestMsgType(wxDocumentTarget);
				RequestMsgType requestMsgType2 = requestMsgType;
				if (requestMsgType2 == RequestMsgType.Event)
				{
					string a = wxDocumentTarget.Root.Element("Event").Value.ToUpper();
					if (a == "POI_CHECK_NOTIFY")
					{
						requestMessageBase = new RequestMessageEvent_Poi_Check_Notify();
						result = true;
					}
				}
				requestMessageBase.FillEntityWithXml(wxDocumentTarget);
				if (requestMessageBase != null)
				{
					IDictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("msgType", requestMsgType.ToNullString());
					dictionary.Add("wxDocumentTarget", wxDocumentTarget.ToNullString());
					dictionary.Add("MsgId", requestMessageBase.MsgId.ToNullString());
					dictionary.Add("MsgType", requestMessageBase.MsgType.ToNullString());
					dictionary.Add("FromUserName", requestMessageBase.FromUserName.ToNullString());
					dictionary.Add("ToUserName", requestMessageBase.ToUserName.ToNullString());
					Globals.AppendLog(dictionary, "微信菜单点击", "", "", "NewWXHandler");
				}
				RequestMsgType requestMsgType3 = requestMsgType;
				if (requestMsgType3 == RequestMsgType.Event)
				{
					string a2 = wxDocumentTarget.Root.Element("Event").Value.ToUpper();
					if (a2 == "POI_CHECK_NOTIFY")
					{
						StoresHelper.UpdateStoreFromWX(requestMessageBase);
					}
				}
			}
			catch (ArgumentException ex)
			{
				Globals.WriteExceptionLog_Page(ex, param, "WXAPI");
			}
			return result;
		}
	}
}
