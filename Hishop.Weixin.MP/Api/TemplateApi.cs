using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Util;
using System.Text;

namespace Hishop.Weixin.MP.Api
{
	public class TemplateApi
	{
		public static string SendMessage(string accessTocken, TemplateMessage templateMessage)
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			stringBuilder.AppendFormat("\"touser\":\"{0}\",", templateMessage.Touser);
			stringBuilder.AppendFormat("\"template_id\":\"{0}\",", templateMessage.TemplateId);
			stringBuilder.AppendFormat("\"url\":\"{0}\",", templateMessage.Url);
			stringBuilder.AppendFormat("\"topcolor\":\"{0}\",", templateMessage.Topcolor);
			stringBuilder.Append("\"data\":{");
			foreach (TemplateMessage.MessagePart datum in templateMessage.Data)
			{
				stringBuilder.AppendFormat("\"{0}\":{{\"value\":\"{1}\",\"color\":\"{2}\"}},", datum.Name, datum.Value, datum.Color);
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("}}");
			WebUtils webUtils = new WebUtils();
			string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + accessTocken;
			return webUtils.DoPost(url, stringBuilder.ToString());
		}

		public static string SendAppletMessage(string accessTocken, TemplateMessage templateMessage)
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			stringBuilder.AppendFormat("\"touser\":\"{0}\",", templateMessage.Touser);
			stringBuilder.AppendFormat("\"template_id\":\"{0}\",", templateMessage.TemplateId);
			stringBuilder.AppendFormat("\"page\":\"{0}\",", templateMessage.Page);
			stringBuilder.AppendFormat("\"form_id\":\"{0}\",", templateMessage.FormId);
			stringBuilder.AppendFormat("\"color\":\"{0}\",", templateMessage.Topcolor);
			stringBuilder.Append("\"data\":{");
			foreach (TemplateMessage.MessagePart datum in templateMessage.Data)
			{
				stringBuilder.AppendFormat("\"{0}\":{{\"value\":\"{1}\",\"color\":\"{2}\"}},", datum.Name, datum.Value, datum.Color);
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("}");
			stringBuilder.AppendFormat(",\"emphasis_keyword\":\"{0}\"", templateMessage.EmphasisKeyword);
			stringBuilder.Append("}");
			WebUtils webUtils = new WebUtils();
			string url = "https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token=" + accessTocken;
			return webUtils.DoPost(url, stringBuilder.ToString());
		}
	}
}
