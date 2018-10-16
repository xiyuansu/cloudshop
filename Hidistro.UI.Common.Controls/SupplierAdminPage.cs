using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.SqlDal.Supplier;
using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class SupplierAdminPage : Page
	{
		protected override void OnInit(EventArgs e)
		{
			this.RegisterFrameScript();
			this.CheckAuth();
			this.CheckPageAccess();
			base.OnInit(e);
		}

		protected string GetParameter(string name)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}

		public string GetCurrentUrl()
		{
			return HttpContext.Current.Request.Url.ToString();
		}

		public void AlertMessage(string msg, bool IsRefresh = false, string PageUrl = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<script type=\"text/javascript\">");
			stringBuilder.AppendFormat("alert(\"{0}\");", msg);
			if (string.IsNullOrEmpty(PageUrl))
			{
				PageUrl = this.GetCurrentUrl();
			}
			if (IsRefresh)
			{
				stringBuilder.AppendFormat("document.location.href=\"{0}\";", PageUrl);
			}
			stringBuilder.Append("</script>");
			HttpContext.Current.Response.Write(stringBuilder.ToString());
			HttpContext.Current.Response.End();
		}

		public string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
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
				result = $"获取信息错误：{ex.Message}";
			}
			return result;
		}

		protected void CheckAuth()
		{
			string domainName = Globals.DomainName;
			string text = "openwapstore";
			string text2 = base.Request.FilePath.ToLower();
			if (text2.IndexOf("/wap/") > 0)
			{
				text = "openwapstore";
			}
			else if (text2.IndexOf("/vshop/") > 0)
			{
				text = "openvstore";
			}
			else if (text2.IndexOf("/alioh/") > 0)
			{
				text = "openaliohstore";
			}
			else
			{
				if (text2.IndexOf("/app/") <= 0)
				{
					return;
				}
				text = "openmobile";
			}
			try
			{
				//string text3 = this.PostData("http://ysc.huz.cn/valid.ashx", "action=" + text + "&product=2&host=" + domainName);
				//int num = Convert.ToInt32(text3.Replace("{\"state\":\"", "").Replace("\"}", ""));
				//if (num != 1)
				//{
				//	this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied?errormsg=抱歉，您暂未开通此服务！"), true);
				//}
			}
			catch
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied?errormsg=抱歉，您暂未开通此服务！"), true);
			}
		}

		protected virtual void RegisterFrameScript()
		{
			string key = "admin-frame";
			string script = string.Format("<script>if(window.parent.frames.length == 0) window.location.href=\"{0}\";</script>", "/supplier/default.html");
			ClientScriptManager clientScript = this.Page.ClientScript;
			if (!clientScript.IsClientScriptBlockRegistered(key))
			{
				clientScript.RegisterClientScriptBlock(base.GetType(), key, script);
			}
		}

		protected virtual void ShowMsg(string msg, bool success)
		{
			string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
			}
		}

		protected virtual void ShowMsg(string msg, bool success, string returnUrl)
		{
			string str = string.Format("ShowMsg(\"{0}\", {1});", msg, success ? "true" : "false");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				string str2 = " setTimeout(function(){ window.location.href='" + returnUrl + "'},1000);";
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + str2 + ";},300);</script>");
			}
		}

		protected virtual void ShowMsgCloseWindow(string msg, bool success)
		{
			string str = string.Format("ShowMsg(\"{0}\", {1});", msg, success ? "true" : "false");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				string str2 = " setTimeout(function(){var win = art.dialog.open.origin; win.location.reload();},1000);";
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + str2 + ";},300);</script>");
			}
		}

		protected virtual void CloseWindow()
		{
			string str = "var win = art.dialog.open.origin; win.location.reload();";
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>" + str + "</script>");
			}
		}

		protected virtual void CloseWindow(string url)
		{
			string str = "var win = art.dialog.open.origin; win.location.href='" + url + "';";
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScriptNew", "<script language='JavaScript' defer='defer'>" + str + "</script>");
			}
		}

		protected void ReloadPage(NameValueCollection queryStrings)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}

		protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
		{
			base.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}

		private string GenericReloadUrl(NameValueCollection queryStrings)
		{
			if (queryStrings == null || queryStrings.Count == 0)
			{
				return base.Request.Url.AbsolutePath;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Request.Url.AbsolutePath).Append("?");
			foreach (string key in queryStrings.Keys)
			{
				string text2 = queryStrings[key].Trim().Replace("'", "");
				if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
				{
					stringBuilder.Append(key).Append("=").Append(base.Server.UrlEncode(text2))
						.Append("&");
				}
			}
			queryStrings.Clear();
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		protected void GotoResourceNotFound()
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("ResourceNotFound"));
		}

		protected bool GetUrlBoolParam(string name)
		{
			string value = base.Request.QueryString.Get(name);
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			try
			{
				return Convert.ToBoolean(value);
			}
			catch (FormatException)
			{
				return false;
			}
		}

		protected int GetUrlIntParam(string name)
		{
			string value = base.Request.QueryString.Get(name);
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			try
			{
				return Convert.ToInt32(value);
			}
			catch (FormatException)
			{
				return 0;
			}
		}

		protected int GetFormIntParam(string name)
		{
			string value = base.Request.Form.Get(name);
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			try
			{
				return Convert.ToInt32(value);
			}
			catch (FormatException)
			{
				return 0;
			}
		}

		protected string CutWords(object obj, int length)
		{
			if (obj == null)
			{
				return string.Empty;
			}
			string text = obj.ToString();
			if (text.Length > length)
			{
				return text.Substring(0, length) + "......";
			}
			return text;
		}

		protected string ModiflyUrl(object count)
		{
			int num = Convert.ToInt32(count);
			if (num > 1)
			{
				return "EditMultiArticle";
			}
			return "EditSingleArticle";
		}

		private void CheckPageAccess()
		{
			if (HiContext.Current.Manager.RoleId != -2)
			{
				string pathAndQuery = base.Request.Url.PathAndQuery;
				string text = null;
				text = base.Request["ReturnUrl"];
				string url = (!string.IsNullOrEmpty(text)) ? $"/User/Login?ReturnUrl={text}" : $"/User/Login?ReturnUrl={base.Request.RawUrl}";
				this.Page.Response.Redirect(url, true);
			}
			else if (!new SupplierDao().IsManangerCanLogin(HiContext.Current.Manager.StoreId))
			{
				this.Page.Response.Redirect("/Admin/Login", true);
			}
		}

		public string ParseSaleStatus(string status)
		{
			if (status == "1")
			{
				return "出售中";
			}
			if (status == "2")
			{
				return "下架中";
			}
			return "仓库中";
		}

		public string ParseDrawStatus(object status)
		{
			if (status == null || string.IsNullOrEmpty(status.ToString()))
			{
				return "审核中";
			}
			if (status.ToString() == "True")
			{
				return "已通过审核";
			}
			return "拒绝";
		}
	}
}
