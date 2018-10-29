using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Core.Helper;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.ashxBase;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class MemberSearch : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (!string.IsNullOrWhiteSpace(base.action))
			{
				base.action = base.action.ToLower();
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "sendsms":
				this.SendSMS(context);
				break;
			case "sendemail":
				this.SendEmail(context);
				break;
			case "sendsitemsg":
				this.SendSiteMsg(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			MemberSearchQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private MemberSearchQuery GetDataQuery(HttpContext context)
		{
			MemberSearchQuery memberSearchQuery = new MemberSearchQuery();
			memberSearchQuery.LastConsumeTime = base.GetParameter(context, "LastConsumeTime", true);
			memberSearchQuery.CustomConsumeStartTime = base.GetParameter(context, "CustomConsumeStartTime", false);
			memberSearchQuery.CustomConsumeEndTime = base.GetParameter(context, "CustomConsumeEndTime", false);
			memberSearchQuery.ConsumeTimes = base.GetParameter(context, "ConsumeTimes", false);
			memberSearchQuery.CustomStartTimes = base.GetIntParam(context, "CustomStartTimes", true);
			memberSearchQuery.CustomEndTimes = base.GetIntParam(context, "CustomEndTimes", true);
			memberSearchQuery.ConsumePrice = base.GetParameter(context, "ConsumePrice", false);
			memberSearchQuery.CustomStartPrice = base.GetParameter(context, "CustomStartPrice", (decimal?)null);
			memberSearchQuery.CustomEndPrice = base.GetParameter(context, "CustomEndPrice", (decimal?)null);
			memberSearchQuery.OrderAvgPrice = base.GetParameter(context, "OrderAvgPrice", false);
			memberSearchQuery.CustomStartAvgPrice = base.GetParameter(context, "CustomStartAvgPrice", (decimal?)null);
			memberSearchQuery.CustomEndAvgPrice = base.GetParameter(context, "CustomEndAvgPrice", (decimal?)null);
			memberSearchQuery.ProductCategory = base.GetParameter(context, "ProductCategory", false);
			memberSearchQuery.MemberTag = base.GetParameter(context, "MemberTag", false);
			memberSearchQuery.UserGroupType = base.GetParameter(context, "UserGroupType", false);
			memberSearchQuery.PageIndex = base.CurrentPageIndex;
			memberSearchQuery.PageSize = base.CurrentPageSize;
			memberSearchQuery.ConsumeTimesInOneMonth = base.CurrentSiteSetting.ConsumeTimesInOneMonth;
			memberSearchQuery.ConsumeTimesInThreeMonth = base.CurrentSiteSetting.ConsumeTimesInThreeMonth;
			memberSearchQuery.ConsumeTimesInSixMonth = base.CurrentSiteSetting.ConsumeTimesInSixMonth;
			return memberSearchQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(MemberSearchQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult members = MemberHelper.GetMembers(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(members.Data);
				dataGridViewModel.total = members.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void SendSMS(HttpContext context)
		{
			string parameter = base.GetParameter(context, "content", true);
			string parameter2 = base.GetParameter(context, "ids", true);
            string TemplateCode = "";
            if (string.IsNullOrWhiteSpace(parameter2))
			{
				throw new HidistroAshxException("请先选择要删除的会员账号");
			}
			if (string.IsNullOrEmpty(parameter))
			{
				throw new HidistroAshxException("请先填写发送的内容信息");
			}
			string sMSSender = base.CurrentSiteSetting.SMSSender;
			if (string.IsNullOrEmpty(sMSSender))
			{
				throw new HidistroAshxException("请先选择发送方式");
			}
			ConfigData configData = null;
			if (base.CurrentSiteSetting.SMSEnabled)
			{
				configData = new ConfigData(HiCryptographer.TryDecypt(base.CurrentSiteSetting.SMSSettings));
			}
			if (configData == null)
			{
				throw new HidistroAshxException("请先选择发送方式并填写配置信息");
			}
			if (!configData.IsValid)
			{
				string text = "";
				foreach (string errorMsg in configData.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(errorMsg);
				}
				throw new HidistroAshxException(text);
			}
			int amount = this.GetAmount(base.CurrentSiteSetting);
			int[] array = (from d in parameter2.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select int.Parse(d)).ToArray();
			parameter2 = string.Join(",", array);
			int num = array.Count();
			int num2 = 0;
			string text2 = null;
			IEnumerable<MemberInfo> membersById = MemberHelper.GetMembersById(parameter2);
			foreach (MemberInfo item in membersById)
			{
				string cellPhone = item.CellPhone;
				if (!string.IsNullOrEmpty(cellPhone) && Regex.IsMatch(cellPhone, "^(13|14|15|18)\\d{9}$"))
				{
					text2 = text2 + cellPhone + ",";
				}
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				throw new HidistroAshxException("请先选择要发送的会员或检测所选手机号格式是否正确");
			}
			text2 = text2.Substring(0, text2.Length - 1);
			string[] array2 = null;
			array2 = ((!text2.Contains(",")) ? new string[1]
			{
				text2
			} : text2.Split(','));
			if (amount < array2.Length)
			{
				throw new HidistroAshxException("发送失败，您的剩余短信条数不足");
			}
			SMSSender sMSSender2 = SMSSender.CreateInstance(sMSSender, configData.SettingsXml);
			string message = default(string);
			if (sMSSender2.Send(array2, TemplateCode, parameter, out message))
			{
				base.ReturnSuccessResult(context, (amount - array2.Length).ToString(), 0, true);
				return;
			}
			throw new HidistroAshxException(message);
		}

		private int GetAmount(SiteSettings settings)
		{

            return -1;
            //int result = 0;
            //if (!string.IsNullOrEmpty(settings.SMSSettings))
            //{
            //	string xml = HiCryptographer.TryDecypt(settings.SMSSettings);
            //	XmlDocument xmlDocument = new XmlDocument();
            //  xmlDocument.XmlResolver = null;
            //	xmlDocument.LoadXml(xml);
            //	string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
            //	string postData = "method=getAmount&Appkey=" + innerText;
            //	string postData2 = WebHelper.GetPostData("http://sms.huz.cn/getAmount.aspx", postData);
            //	int num = default(int);
            //	if (int.TryParse(postData2, out num))
            //	{
            //		result = Convert.ToInt32(postData2);
            //	}
            //}
            //return result;
        }

		private void SendEmail(HttpContext context)
		{
			string parameter = base.GetParameter(context, "content", true);
			string parameter2 = base.GetParameter(context, "ids", true);
			if (string.IsNullOrWhiteSpace(parameter2))
			{
				throw new HidistroAshxException("请先选择要删除的会员账号");
			}
			if (string.IsNullOrEmpty(parameter))
			{
				throw new HidistroAshxException("请先填写发送的内容信息");
			}
			string emailSender = base.CurrentSiteSetting.EmailSender;
			if (string.IsNullOrEmpty(emailSender))
			{
				throw new HidistroAshxException("请先选择发送方式");
			}
			ConfigData configData = null;
			if (base.CurrentSiteSetting.EmailEnabled)
			{
				configData = new ConfigData(HiCryptographer.TryDecypt(base.CurrentSiteSetting.EmailSettings));
			}
			if (configData == null)
			{
				throw new HidistroAshxException("请先选择发送方式并填写配置信息");
			}
			if (!configData.IsValid)
			{
				string text = "";
				foreach (string errorMsg in configData.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(errorMsg);
				}
				throw new HidistroAshxException(text);
			}
			int[] array = (from d in parameter2.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select int.Parse(d)).ToArray();
			parameter2 = string.Join(",", array);
			int num = array.Count();
			string text2 = null;
			IEnumerable<MemberInfo> membersById = MemberHelper.GetMembersById(parameter2);
			foreach (MemberInfo item in membersById)
			{
				string email = item.Email;
				if (!string.IsNullOrEmpty(email) && Regex.IsMatch(email, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
				{
					text2 = text2 + email + ",";
				}
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				throw new HidistroAshxException("请先选择要发送的会员或检测邮箱格式是否正确");
			}
			text2 = text2.Substring(0, text2.Length - 1);
			string[] array2 = null;
			array2 = ((!text2.Contains(",")) ? new string[1]
			{
				text2
			} : text2.Split(','));
			MailMessage mailMessage = new MailMessage
			{
				IsBodyHtml = true,
				Priority = MailPriority.High,
				SubjectEncoding = Encoding.UTF8,
				BodyEncoding = Encoding.UTF8,
				Body = parameter,
				Subject = "来自" + base.CurrentSiteSetting.SiteName
			};
			string[] array3 = array2;
			foreach (string addresses in array3)
			{
				mailMessage.To.Add(addresses);
			}
			EmailSender emailSender2 = EmailSender.CreateInstance(emailSender, configData.SettingsXml);
			try
			{
				if (emailSender2.Send(mailMessage, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
				{
					base.ReturnSuccessResult(context, "发送邮件成功", 0, true);
					goto end_IL_02fb;
				}
				throw new HidistroAshxException("发送邮件失败");
				end_IL_02fb:;
			}
			catch (Exception)
			{
				throw new HidistroAshxException("发送邮件成功,但存在无效的邮箱账号");
			}
		}

		private void SendSiteMsg(HttpContext context)
		{
			string text = base.GetParameter(context, "content", true).Trim();
			string parameter = base.GetParameter(context, "ids", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("请先选择要删除的会员账号");
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("请先填写发送的内容信息");
			}
			string title = text;
			if (text.Length > 10)
			{
				title = text.Substring(0, 10) + "……";
			}
			int[] array = (from d in parameter.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select int.Parse(d)).ToArray();
			parameter = string.Join(",", array);
			int num = array.Count();
			IEnumerable<MemberInfo> membersById = MemberHelper.GetMembersById(parameter);
			IList<MessageBoxInfo> list = new List<MessageBoxInfo>();
			foreach (MemberInfo item in membersById)
			{
				MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
				messageBoxInfo.Sernder = "Admin";
				messageBoxInfo.Accepter = item.UserName;
				messageBoxInfo.Title = title;
				messageBoxInfo.Content = text;
				list.Add(messageBoxInfo);
			}
			if (list.Count > 0)
			{
				NoticeHelper.SendMessageToMember(list);
				base.ReturnSuccessResult(context, $"成功给{list.Count}个用户发送了消息.", 0, true);
				return;
			}
			throw new HidistroAshxException("没有要发送的对象");
		}
	}
}
