using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Lib;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Hishop.Weixin.Pay.Notify
{
	public class Notify
	{
		public Page page
		{
			get;
			set;
		}

		public PayConfig config
		{
			get;
			set;
		}

		public Notify(Page page, PayConfig config)
		{
			this.page = page;
			this.config = config;
		}

		public WxPayData GetNotifyData()
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("AppID", this.config.AppId);
			dictionary.Add("AppSecret", this.config.AppSecret);
			dictionary.Add("Key", this.config.Key);
			dictionary.Add("MchID", this.config.MchID);
			Stream inputStream = this.page.Request.InputStream;
			int num = 0;
			byte[] array = new byte[1024];
			StringBuilder stringBuilder = new StringBuilder();
			while ((num = inputStream.Read(array, 0, 1024)) > 0)
			{
				stringBuilder.Append(Encoding.UTF8.GetString(array, 0, num));
			}
			inputStream.Flush();
			inputStream.Close();
			inputStream.Dispose();
			WxPayData wxPayData = new WxPayData();
			try
			{
				wxPayData.FromXml(stringBuilder.ToString(), this.config.Key);
			}
			catch (WxPayException ex)
			{
				WxPayData wxPayData2 = new WxPayData();
				wxPayData2.SetValue("return_code", "FAIL");
				wxPayData2.SetValue("return_msg", ex.Message);
				dictionary.Add("return_code", "FAIL");
				dictionary.Add("result", wxPayData2.ToXml());
				WxPayLog.writeLog(dictionary, "", HttpContext.Current.Request.Url.ToString(), ex.Message, LogType.Error);
				this.page.Response.Write(wxPayData2.ToXml());
				this.page.Response.End();
			}
			return wxPayData;
		}

		public virtual void ProcessNotify()
		{
		}
	}
}
