using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Jobs;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Hidistro.Jobs
{
	public class SiteMapJob : IJob
	{
		private CookieContainer cookie = new CookieContainer();

		public void Execute(XmlNode node)
		{
			try
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string text = masterSettings.SiteUrl;
				if (!text.ToLower().Contains("http://"))
				{
					text = "http://" + text;
				}
				text += "/API/SiteMapJobHandler.ashx";
				this.HttpGet(text + "/API/SiteMapJobHandler.ashx", "");
			}
			catch (Exception ex)
			{
				Globals.WriteLog("SiteMapJob.txt", DateTime.Now.ToString() + "\r\n" + ex.Message + "\r\n" + ex.Source);
			}
		}

		public string HttpGet(string Url, string postDataStr)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url + ((postDataStr == "") ? "" : "?") + postDataStr);
			httpWebRequest.Method = "GET";
			httpWebRequest.ContentType = "text/html;charset=UTF-8";
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			return result;
		}
	}
}
