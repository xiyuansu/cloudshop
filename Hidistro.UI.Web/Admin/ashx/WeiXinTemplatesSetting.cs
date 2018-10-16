using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.ashx
{
	public class WeiXinTemplatesSetting : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "quicksetweixintemplates"))
			{
				if (action == "saveweixintemplates")
				{
					this.SaveWeiXinTemplates(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.QuickSetWeixinTemplates(context);
		}

		public void QuickSetWeixinTemplates(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsDemoSite)
			{
				string s = JsonConvert.SerializeObject(new
				{
					Status = "FAIL",
					Message = "演示站不允许修改消息模板配置信息。"
				});
				context.Response.Write(s);
				context.Response.End();
			}
			OperationResult operationResult = WeiXinTemplateProcessor.QuickSetWeixinTemplates();
			if (operationResult.ResultType == OperationResultType.Success)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (WxtemplateId template in operationResult.TemplateList)
				{
					dictionary.Add(new KeyValuePair<string, string>(template.name, template.templateid));
				}
				WeiXinTemplateProcessor.SaveWXTempalteId(dictionary);
			}
			string s2 = JsonConvert.SerializeObject(new
			{
				Status = ((operationResult.ResultType == OperationResultType.Success) ? "SUCCESS" : "FAIL"),
				Message = "一键配置" + ((operationResult.ResultType == OperationResultType.Success) ? "成功" : "失败") + "," + operationResult.Msg
			});
			context.Response.Write(s2);
			context.Response.End();
		}

		public void SaveWeiXinTemplates(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsDemoSite)
			{
				string s = JsonConvert.SerializeObject(new
				{
					Status = "FAIL",
					Message = "演示站不允许修改消息模板配置信息。"
				});
				context.Response.Write(s);
				context.Response.End();
			}
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			string parameter = base.GetParameter(context, "TemplateData", false);
			bool flag = false;
			try
			{
				SavePostData savePostData = JsonHelper.ParseFormJson<SavePostData>(parameter);
				if (savePostData.TemplatePostData != null && savePostData.TemplatePostData.Count > 0)
				{
					IList<MessageTemplate> list = new List<MessageTemplate>();
					foreach (WXTemplateInfo templatePostDatum in savePostData.TemplatePostData)
					{
						MessageTemplate messageTemplate = new MessageTemplate();
						messageTemplate.MessageType = templatePostDatum.MessageType;
						messageTemplate.WeixinTemplateId = templatePostDatum.TemplateId;
						list.Add(messageTemplate);
					}
					flag = WeiXinTemplateProcessor.SaveWXTempalteIdOfMsgType(list);
				}
				else
				{
					Globals.AppendLog(parameter, "提交的数据为空", "", "SaveWeiXinTemplates");
				}
			}
			catch (Exception ex)
			{
				dictionary.Add("templateData", parameter);
				Globals.WriteExceptionLog(ex, dictionary, "SaveWeiXinTemplates");
			}
			string s2 = JsonConvert.SerializeObject(new
			{
				Status = (flag ? "SUCCESS" : "FAIL"),
				Message = "手动保存配置" + (flag ? "成功" : "失败")
			});
			context.Response.Write(s2);
			context.Response.End();
		}
	}
}
