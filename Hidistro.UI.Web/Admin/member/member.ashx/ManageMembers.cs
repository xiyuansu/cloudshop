using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.ashxBase;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;

namespace Hidistro.UI.Web.Admin.member.ashx
{
	public class ManageMembers : AdminBaseHandler
	{
		private DataTable dtError = new DataTable("Errortable");

		private int succeedCount = 0;

		private int errorCount = 0;

		private Regex emailR = new Regex("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", RegexOptions.Compiled);

		private Regex cellphoneR = new Regex("^0?(13|15|18|14|17)[0-9]{9}$", RegexOptions.Compiled);

		private MemberDao memberDao = new MemberDao();

		private DataRow[] dr;

		private int drCount = 0;

		private int groupmax = 2000;

		private List<List<string>> rows;

		private int rowsCount = 0;

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
			case "delete":
				this.Delete(context);
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
			case "exportexcel":
				this.ExportExcel(context);
				break;
			case "importmember":
				this.ImportMember(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetList(HttpContext context)
		{
			MemberQuery dataQuery = this.GetDataQuery(context);
			DataGridViewModel<Dictionary<string, object>> listSplittinDraws = this.GetListSplittinDraws(dataQuery);
			string s = base.SerializeObjectToJson(listSplittinDraws);
			context.Response.Write(s);
			context.Response.End();
		}

		private MemberQuery GetDataQuery(HttpContext context)
		{
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.UserName = base.GetParameter(context, "UserName", true);
			memberQuery.RealName = base.GetParameter(context, "RealName", true);
			memberQuery.StartTime = base.GetDateTimeParam(context, "StartTime");
			memberQuery.EndTime = base.GetDateTimeParam(context, "EndTime");
			memberQuery.GradeId = base.GetIntParam(context, "GradeId", true);
			memberQuery.TagsId = base.GetParameter(context, "TagsId", false);
			if (!string.IsNullOrEmpty(memberQuery.TagsId))
			{
				memberQuery.TagsId = context.Request["MemberTag"].ToNullString();
			}
			memberQuery.UserGroupType = base.GetParameter(context, "UserGroupType", true);
			memberQuery.IsApproved = base.GetBoolParam(context, "IsApproved", true);
			memberQuery.RegisteredSource = base.GetIntParam(context, "RegisteredSource", false).Value;
			memberQuery.LastConsumeTime = context.Request["LastConsumeTime"].ToNullString();
			if (memberQuery.LastConsumeTime.ToLower() == "custom")
			{
				memberQuery.LastConsumeStartTime = context.Request["CustomConsumeStartTime"].ToDateTime();
				memberQuery.LastConsumeEndTime = context.Request["CustomConsumeEndTime"].ToDateTime();
			}
			if (!(context.Request["ConsumeTimes"].ToNullString() == "custom"))
			{
				memberQuery.ConsumeMinTimes = context.Request["ConsumeTimes"].ToInt(0);
				memberQuery.ConsumeMaxTimes = memberQuery.ConsumeMinTimes;
			}
			string text = context.Request["ConsumePrice"].ToNullString();
			if (!string.IsNullOrEmpty(text))
			{
				if (text == "custom")
				{
					memberQuery.ConsumeMinPrice = context.Request["CustomStartPrice"].ToDecimal(0);
					memberQuery.ConsumeMaxPrice = context.Request["CustomEndPrice"].ToDecimal(0);
				}
				else
				{
					string[] array = text.Split('_');
					memberQuery.ConsumeMinPrice = array[0].ToDecimal(0);
					if (array.Length >= 2)
					{
						memberQuery.ConsumeMaxPrice = array[1].ToDecimal(0);
					}
				}
			}
			string text2 = context.Request["OrderAvgPrice"].ToNullString();
			if (text2 == "custom")
			{
				memberQuery.OrderAvgMinPrice = context.Request["CustomStartAvgPrice"].ToDecimal(0);
				memberQuery.OrderAvgMaxPrice = context.Request["CustomEndAvgPrice"].ToDecimal(0);
			}
			else if (!string.IsNullOrEmpty(text2))
			{
				string[] array2 = text2.Split('_');
				memberQuery.OrderAvgMinPrice = array2[0].ToDecimal(0);
				if (array2.Length >= 2)
				{
					memberQuery.OrderAvgMaxPrice = array2[1].ToDecimal(0);
				}
			}
			memberQuery.ProductCategoryId = context.Request["ProductCategory"].ToInt(0);
			memberQuery.PageIndex = base.CurrentPageIndex;
			memberQuery.PageSize = base.CurrentPageSize;
			memberQuery.MemberBrithDayVal = base.CurrentSiteSetting.MemberBirthDaySetting;
			string text3 = "Expenditure";
			if (!string.IsNullOrWhiteSpace(base.GetParameter(context, "OrderBy", false)))
			{
				text3 = base.GetParameter(context, "OrderBy", false);
			}
			memberQuery.SortBy = text3;
			if (text3 == "orderbrithDay")
			{
				memberQuery.SortOrder = SortAction.Asc;
			}
			else
			{
				memberQuery.SortOrder = SortAction.Desc;
			}
			return memberQuery;
		}

		private DataGridViewModel<Dictionary<string, object>> GetListSplittinDraws(MemberQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult members = MemberHelper.GetMembers(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(members.Data);
				dataGridViewModel.total = members.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					row.Add("MemberBrithDayVal", base.CurrentSiteSetting.MemberBirthDaySetting);
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			string parameter = base.GetParameter(context, "ids", true);
			if (string.IsNullOrWhiteSpace(parameter))
			{
				throw new HidistroAshxException("请先选择要删除的会员账号");
			}
			int[] array = (from d in parameter.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select int.Parse(d)).ToArray();
			int num = array.Count();
			int num2 = 0;
			int[] array2 = array;
			foreach (int userId in array2)
			{
				if (MemberHelper.Delete(userId))
				{
					num2++;
				}
			}
			if (num2 > 0)
			{
				if (num2 > 1)
				{
					base.ReturnSuccessResult(context, "成功删除了" + num2.ToString() + "个的会员", 0, true);
				}
				else
				{
					base.ReturnSuccessResult(context, "删除会员成功", 0, true);
				}
				return;
			}
			throw new HidistroAshxException("删除会员失败");
		}

		private void ExportExcel(HttpContext context)
		{
			string parameter = base.GetParameter(context, "FileFormat", false);
			string parameter2 = base.GetParameter(context, "Fields", false);
			string parameter3 = base.GetParameter(context, "Headers", false);
			IList<string> list = new List<string>();
			IList<string> list2 = new List<string>();
			IList<string> list3 = new List<string>();
			string[] array = (from d in parameter2.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select d).ToArray();
			string[] array2 = (from d in parameter3.Split(',')
			where !string.IsNullOrWhiteSpace(d)
			select d).ToArray();
			if (array.Count() != array2.Count())
			{
				throw new HidistroAshxException("字段与字段描述数量不对");
			}
			int num = array.Count();
			for (int i = 0; i < num; i++)
			{
				string text = array[i];
				string item = array2[i];
				if (!string.IsNullOrWhiteSpace(text))
				{
					list2.Add(text);
					if (text == "Address")
					{
						list2.Add("RegionId");
					}
					list.Add(text);
					list3.Add(item);
				}
			}
			MemberQuery dataQuery = this.GetDataQuery(context);
			DataTable membersNopage = MemberHelper.GetMembersNopage(dataQuery, list2);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string item2 in list3)
			{
				stringBuilder.Append(item2 + ",");
				if (item2 == list3[list3.Count - 1])
				{
					stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
					stringBuilder.Append("\r\n");
				}
			}
			string text2 = "";
			int num2 = 0;
			string text3 = "";
			foreach (DataRow row in membersNopage.Rows)
			{
				foreach (string item3 in list)
				{
					if (item3 == "Address" && row["RegionId"] != DBNull.Value)
					{
						int.TryParse(row["RegionId"].ToString(), out num2);
						text3 = ((num2 <= 0) ? "" : (RegionHelper.GetFullRegion(num2, " ", true, 0) + " "));
						text2 = text3 + row[item3].ToString() + ",";
					}
					else if (item3 == "TagIds")
					{
						string str = "";
						string text4 = row["TagIds"].ToString();
						if (!string.IsNullOrWhiteSpace(text4))
						{
							if (text4 == ",")
							{
								text4 = "";
							}
							else
							{
								text4 = text4.TrimStart(',');
								text4 = text4.TrimEnd(',');
							}
							if (text4 != "")
							{
								IList<MemberTagInfo> tagByMember = MemberTagHelper.GetTagByMember(text4);
								str = string.Join("|", (from t in tagByMember
								select t.TagName).ToArray());
							}
						}
						text2 = str + ",";
					}
					else if (item3 == "BirthDate")
					{
						string text5 = row["BirthDate"].ToString();
						text2 = (string.IsNullOrEmpty(text5) ? " ," : (DateTime.Parse(text5).ToShortDateString() + ","));
					}
					else
					{
						text2 = row[item3].ToString() + ",";
					}
					stringBuilder.Append(text2);
					if (item3 == list[list3.Count - 1])
					{
						stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
						stringBuilder.Append("\r\n");
					}
				}
			}
			context.Response.Clear();
			context.Response.Buffer = false;
			context.Response.Charset = "GB2312";
			if (parameter == "csv")
			{
				context.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
				context.Response.ContentType = "application/octet-stream";
			}
			else
			{
				context.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.txt");
				context.Response.ContentType = "application/vnd.ms-word";
			}
			context.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
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
				if (!string.IsNullOrEmpty(cellPhone) && Regex.IsMatch(cellPhone, "^(13|14|15|18|17)\\d{9}$"))
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
			int result = 0;
			if (!string.IsNullOrEmpty(settings.SMSSettings))
			{
				string xml = HiCryptographer.TryDecypt(settings.SMSSettings);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
				string postData = "method=getAmount&Appkey=" + innerText;
				string postResult = Globals.GetPostResult("http://sms.huz.cn/getAmount.aspx", postData);
				int num = default(int);
				if (int.TryParse(postResult, out num))
				{
					result = Convert.ToInt32(postResult);
				}
			}
			return result;
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

		private void ImportMember(HttpContext context)
		{
			string text = context.Request["fullPath"];
			int num = context.Request["TypeId"].ToInt(0);
			if (num == 2)
			{
				this.CsvToMember(context, text);
			}
			else
			{
				ExcelHelper excelHelper = new ExcelHelper();
				DataSet dataSet = excelHelper.ExcelToDataSet(text);
				try
				{
					if (File.Exists(text))
					{
						File.Delete(text);
					}
				}
				catch
				{
				}
				if (dataSet == null && dataSet.Tables.Count == 0)
				{
					base.ReturnSuccessResult(context, "未能从您上传的Excel中获取到任何数据！", -1, false);
				}
				else
				{
					this.dr = dataSet.Tables[0].Select();
					this.dtError.Columns.Add("*手机号码", typeof(string));
					this.dtError.Columns.Add("*邮箱", typeof(string));
					this.dtError.Columns.Add("*密码（最少6位）", typeof(string));
					this.dtError.Columns.Add("姓名", typeof(string));
					this.dtError.Columns.Add("性别", typeof(string));
					this.dtError.Columns.Add("昵称", typeof(string));
					this.dtError.Columns.Add("生日", typeof(string));
					this.dtError.Columns.Add("QQ", typeof(string));
					this.dtError.Columns.Add("积分", typeof(int));
					this.dtError.Columns.Add("错误信息", typeof(string));
					if (dataSet.Tables[0].Rows.Count == 0)
					{
						base.ReturnSuccessResult(context, "Excel表为空表,无数据!", -1, false);
					}
					else
					{
						this.drCount = this.dr.Length;
						DataSet dataSet2 = new DataSet();
						if (this.dr.Length <= this.groupmax)
						{
							this.GroupAddMember(0);
						}
						else
						{
							int num2 = 0;
							num2 = ((this.dr.Length % this.groupmax != 0) ? (this.dr.Length / this.groupmax + 1) : (this.dr.Length / this.groupmax));
							for (int i = 0; i < num2; i++)
							{
								Thread thread = new Thread(this.GroupAddMember);
								Thread.Sleep(this.groupmax * i);
								thread.Start(i);
							}
							Thread.Sleep(this.groupmax * num2 + this.groupmax);
						}
						dataSet2.Tables.Add(this.dtError);
						if (dataSet2.Tables[0].Rows.Count > 0)
						{
							excelHelper.DataSetToExcel(dataSet2, "~/Storage/master/ImportMember", "查看导入失败的错误信息.xls");
							base.ReturnSuccessResult(context, " 成功导入" + this.succeedCount + "条，失败<a style=\"color:#ff0000\" >" + this.errorCount + "</a>条!", 1, true);
						}
						else
						{
							base.ReturnSuccessResult(context, " 成功导入" + this.succeedCount + "条，失败<a>" + this.errorCount + "</a>条!", 2, true);
						}
					}
				}
			}
		}

		private void AddMember(DataRow dr)
		{
			DataRow dataRow = this.dtError.NewRow();
			StringBuilder stringBuilder = new StringBuilder();
			string text = dr[0].ToString();
			string text2 = dr[1].ToString();
			string text3 = dr[2].ToString();
			string text4 = dr[3].ToString();
			string a = dr[4].ToString();
			string text5 = dr[5].ToString();
			string text6 = dr[6].ToString();
			string text7 = dr[7].ToString();
			int points = dr[8].ToInt(0);
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
			{
				stringBuilder.Append("手机和邮箱不能同时为空；");
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (!this.cellphoneR.IsMatch(text))
				{
					stringBuilder.Append("手机格式不正确；");
				}
				else if (this.memberDao.FindMemberByCellphone(text) != null)
				{
					stringBuilder.Append("手机号码已经存在；");
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (!this.emailR.IsMatch(text2))
				{
					stringBuilder.Append("邮箱格式不正确；");
				}
				else if (this.memberDao.FindMemberByEmail(text2) != null)
				{
					stringBuilder.Append("邮箱已经存在；");
				}
			}
			if (string.IsNullOrEmpty(text3))
			{
				stringBuilder.Append("密码不能为空；");
			}
			if (text3.Length < 6 || text3.Length > HiConfiguration.GetConfig().PasswordMaxLength)
			{
				stringBuilder.AppendFormat("密码的长度只能在{0}和{1}个字符之间；", 6, HiConfiguration.GetConfig().PasswordMaxLength);
			}
			if (!string.IsNullOrEmpty(text4) && (text4.Trim().Length > 20 || (text4.Trim().Length > 0 && !Regex.IsMatch(text4.Trim(), "^[A-Za-z\\u4e00-\\u9fa5]+$"))))
			{
				stringBuilder.Append("真实姓名限制在20个字符以内,只能由中文或英文组成；");
			}
			if (!string.IsNullOrEmpty(text5) && (text5.Trim().Length > 20 || text5.Trim().Length < 1))
			{
				stringBuilder.Append("请输入正确的昵称，长度在1-20个字符以内；");
			}
			if (!string.IsNullOrEmpty(text6) && !text6.ToDateTime().HasValue)
			{
				stringBuilder.Append("生日格式不正确；");
			}
			if (!string.IsNullOrEmpty(text7) && (text7.Length > 20 || text7.Length < 3) && !text7.IsInt())
			{
				stringBuilder.Append("QQ号长度限制在3-20个字符之间，只能输入数字；");
			}
			if (stringBuilder.Length <= 0)
			{
				MemberInfo memberInfo = new MemberInfo();
				memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
				if (!string.IsNullOrEmpty(text))
				{
					memberInfo.UserName = text;
				}
				else
				{
					memberInfo.UserName = text2;
				}
				memberInfo.CellPhone = text;
				memberInfo.CellPhoneVerification = false;
				memberInfo.Email = text2;
				memberInfo.EmailVerification = false;
				string salt = Globals.RndStr(128, true);
				memberInfo.Password = Users.EncodePassword(text3, salt);
				memberInfo.PasswordSalt = Globals.RndStr(128, true);
				memberInfo.RegisteredSource = 7;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.QQ = text7;
				memberInfo.Points = points;
				memberInfo.RealName = text4;
				memberInfo.NickName = text5;
				if (a == "男")
				{
					memberInfo.Gender = Gender.Male;
				}
				else if (a == "女")
				{
					memberInfo.Gender = Gender.Female;
				}
				else
				{
					memberInfo.Gender = Gender.NotSet;
				}
				if (!string.IsNullOrEmpty(text6))
				{
					memberInfo.BirthDate = DateTime.Parse(text6);
				}
				memberInfo.IsOpenBalance = true;
				try
				{
					int num = (int)this.memberDao.Add(memberInfo, null);
				}
				catch (Exception ex)
				{
					stringBuilder.Append(ex.ToString());
				}
			}
			if (stringBuilder.Length > 0)
			{
				this.errorCount++;
				dataRow[0] = dr[0];
				dataRow[1] = dr[1];
				dataRow[2] = dr[2];
				dataRow[3] = dr[3];
				dataRow[4] = dr[4];
				dataRow[5] = dr[5];
				dataRow[6] = dr[6];
				dataRow[7] = dr[7];
				dataRow[8] = dr[8];
				dataRow[9] = stringBuilder.ToString();
				this.dtError.Rows.Add(dataRow);
			}
			else
			{
				this.succeedCount++;
			}
		}

		private void GroupAddMember(object ob)
		{
			int num = (int)ob;
			int num2 = (num + 1) * this.groupmax;
			if (this.drCount < (num + 1) * this.groupmax)
			{
				num2 = this.drCount;
			}
			for (int i = num * this.groupmax; i < num2; i++)
			{
				int num3 = 0;
				for (int j = 0; j <= 7; j++)
				{
					if (string.IsNullOrEmpty(this.dr[i][j].ToString()))
					{
						num3++;
					}
				}
				if (num3 != 8)
				{
					this.AddMember(this.dr[i]);
				}
			}
		}

		private void CsvToMember(HttpContext context, string csvPath)
		{
			this.dtError.Columns.Add("*手机号码", typeof(string));
			this.dtError.Columns.Add("*邮箱", typeof(string));
			this.dtError.Columns.Add("*密码（最少6位）", typeof(string));
			this.dtError.Columns.Add("姓名", typeof(string));
			this.dtError.Columns.Add("性别", typeof(string));
			this.dtError.Columns.Add("昵称", typeof(string));
			this.dtError.Columns.Add("生日", typeof(string));
			this.dtError.Columns.Add("QQ", typeof(string));
			this.dtError.Columns.Add("积分", typeof(int));
			this.dtError.Columns.Add("错误信息", typeof(string));
			DataSet dataSet = new DataSet();
			ExcelHelper excelHelper = new ExcelHelper();
			this.rows = this.ReadCsv(csvPath, true, '\t', Encoding.GetEncoding("GB2312"));
			try
			{
				if (File.Exists(csvPath))
				{
					File.Delete(csvPath);
				}
			}
			catch
			{
			}
			int num = 0;
			this.rowsCount = this.rows.Count;
			if (this.rowsCount <= this.groupmax)
			{
				this.CsvGroupAddMember(0);
			}
			else
			{
				int num2 = 0;
				num2 = ((this.rowsCount % this.groupmax != 0) ? (this.rowsCount / this.groupmax + 1) : (this.rowsCount / this.groupmax));
				for (int i = 0; i < num2; i++)
				{
					Thread thread = new Thread(this.CsvGroupAddMember);
					Thread.Sleep(this.groupmax * i);
					thread.Start(i);
				}
				Thread.Sleep(this.groupmax * num2 + this.groupmax);
			}
			dataSet.Tables.Add(this.dtError);
			if (dataSet.Tables[0].Rows.Count > 0)
			{
				excelHelper.DataSetToExcel(dataSet, "~/Storage/master/ImportMember", "查看导入失败的错误信息.xls");
				base.ReturnSuccessResult(context, " 成功导入" + this.succeedCount + "条，失败<a style=\"color:#ff0000\" >" + this.errorCount + "</a>条!", 1, true);
			}
			else
			{
				base.ReturnSuccessResult(context, " 成功导入" + this.succeedCount + "条，失败<a>" + this.errorCount + "</a>条!", 2, true);
			}
		}

		private void CsvAddMember(string[] dr)
		{
			DataRow dataRow = this.dtError.NewRow();
			StringBuilder stringBuilder = new StringBuilder();
			string text = dr[0].ToString();
			string text2 = dr[1].ToString();
			string text3 = dr[2].ToString();
			string text4 = dr[3].ToString();
			string a = dr[4].ToString();
			string text5 = dr[5].ToString();
			string text6 = dr[6].ToString();
			string text7 = dr[7].ToString();
			int points = dr[8].ToInt(0);
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
			{
				stringBuilder.Append("手机和邮箱不能同时为空；");
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (!this.cellphoneR.IsMatch(text))
				{
					stringBuilder.Append("手机格式不正确；");
				}
				else if (this.memberDao.FindMemberByCellphone(text) != null)
				{
					stringBuilder.Append("手机号码已经存在；");
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (!this.emailR.IsMatch(text2))
				{
					stringBuilder.Append("邮箱格式不正确；");
				}
				else if (this.memberDao.FindMemberByEmail(text2) != null)
				{
					stringBuilder.Append("邮箱已经存在；");
				}
			}
			if (string.IsNullOrEmpty(text3))
			{
				stringBuilder.Append("密码不能为空；");
			}
			if (text3.Length < 6 || text3.Length > HiConfiguration.GetConfig().PasswordMaxLength)
			{
				stringBuilder.AppendFormat("密码的长度只能在{0}和{1}个字符之间；", 6, HiConfiguration.GetConfig().PasswordMaxLength);
			}
			if (!string.IsNullOrEmpty(text4) && (text4.Trim().Length > 20 || (text4.Trim().Length > 0 && !Regex.IsMatch(text4.Trim(), "^[A-Za-z\\u4e00-\\u9fa5]+$"))))
			{
				stringBuilder.Append("真实姓名限制在20个字符以内,只能由中文或英文组成；");
			}
			if (!string.IsNullOrEmpty(text5) && (text5.Trim().Length > 20 || text5.Trim().Length < 1))
			{
				stringBuilder.Append("请输入正确的昵称，长度在1-20个字符以内；");
			}
			if (!string.IsNullOrEmpty(text6) && !text6.ToDateTime().HasValue)
			{
				stringBuilder.Append("生日格式不正确；");
			}
			if (!string.IsNullOrEmpty(text7) && (text7.Length > 20 || text7.Length < 3) && !text7.IsInt())
			{
				stringBuilder.Append("QQ号长度限制在3-20个字符之间，只能输入数字；");
			}
			if (stringBuilder.Length <= 0)
			{
				MemberInfo memberInfo = new MemberInfo();
				memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
				if (!string.IsNullOrEmpty(text))
				{
					memberInfo.UserName = text;
				}
				else
				{
					memberInfo.UserName = text2;
				}
				memberInfo.CellPhone = text;
				memberInfo.CellPhoneVerification = false;
				memberInfo.Email = text2;
				memberInfo.EmailVerification = false;
				string salt = Globals.RndStr(128, true);
				memberInfo.Password = Users.EncodePassword(text3, salt);
				memberInfo.PasswordSalt = Globals.RndStr(128, true);
				memberInfo.RegisteredSource = 1;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.QQ = text7;
				memberInfo.RealName = text4;
				memberInfo.NickName = text5;
				memberInfo.Points = points;
				if (a == "男")
				{
					memberInfo.Gender = Gender.Male;
				}
				if (a == "女")
				{
					memberInfo.Gender = Gender.Female;
				}
				else
				{
					memberInfo.Gender = Gender.NotSet;
				}
				if (!string.IsNullOrEmpty(text6))
				{
					memberInfo.BirthDate = DateTime.Parse(text6);
				}
				memberInfo.IsOpenBalance = true;
				try
				{
					int num = (int)this.memberDao.Add(memberInfo, null);
				}
				catch (Exception ex)
				{
					stringBuilder.Append(ex.ToString());
				}
			}
			if (stringBuilder.Length > 0)
			{
				this.errorCount++;
				dataRow[0] = dr[0];
				dataRow[1] = dr[1];
				dataRow[2] = dr[2];
				dataRow[3] = dr[3];
				dataRow[4] = dr[4];
				dataRow[5] = dr[5];
				dataRow[6] = dr[6];
				dataRow[7] = dr[7];
				dataRow[8] = stringBuilder;
				this.dtError.Rows.Add(dataRow);
			}
			else
			{
				this.succeedCount++;
			}
		}

		private void CsvGroupAddMember(object ob)
		{
			int num = (int)ob;
			int num2 = (num + 1) * this.groupmax;
			if (this.rowsCount < (num + 1) * this.groupmax)
			{
				num2 = this.rowsCount;
			}
			for (int i = num * this.groupmax; i < num2; i++)
			{
				this.CsvAddMember(this.rows[i][0].ToString().Split(','));
			}
		}

		public List<List<string>> ReadCsv(string csvName, bool hasHeader, char colSplit, Encoding encoding)
		{
			List<List<string>> list = new List<List<string>>();
			string[] array = File.ReadAllLines(csvName, encoding);
			int i = 0;
			if (hasHeader)
			{
				i = 1;
			}
			for (; i < array.Length; i++)
			{
				string text = array[i];
				List<string> list2 = new List<string>();
				string[] array2 = text.Split(colSplit);
				for (int j = 0; j < array2.Length; j++)
				{
					list2.Add(array2[j].Replace("\"", "").Replace("'", ""));
				}
				list.Add(list2);
			}
			return list;
		}
	}
}
