using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.CommonAPIs;
using System.Collections.Generic;
using System.Linq;

namespace Hidistro.SaleSystem.Vshop
{
	public class WeiXinTemplateProcessor
	{
		private static string _TokenValue;

		private static string wxAccessTokenKey = "WxAccessToken";

		private static string AccessToken
		{
			get
			{
				WeiXinTemplateProcessor._TokenValue = WeiXinTemplateProcessor.GetAccessToken(true);
				if (WeiXinTemplateProcessor._TokenValue.Contains("access_token is invalid or not latest"))
				{
					WeiXinTemplateProcessor._TokenValue = WeiXinTemplateProcessor.GetAccessToken(false);
				}
				return WeiXinTemplateProcessor._TokenValue;
			}
		}

		private static string GetAccessToken(bool isFirst = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				return "";
			}
			if (isFirst)
			{
				return AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, false);
			}
			return AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, true);
		}

		public static OperationResult SetIndustry()
		{
			string accessToken = WeiXinTemplateProcessor.AccessToken;
			if (string.IsNullOrEmpty(accessToken) || accessToken.StartsWith("error"))
			{
				return new OperationResult(OperationResultType.Error, "获取AccessToken失败！", null);
			}
			string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}";
			var data = new
			{
				industry_id1 = "1",
				industry_id2 = "2"
			};
			WxJsonResult wxJsonResult = WxTemplateHelp.WxCommonSend<WxJsonResult>(accessToken, urlFormat, data, CommonJsonSendType.POST);
			if (wxJsonResult.errcode == ReturnCode.获取access_token时AppSecret错误或者access_token无效)
			{
				accessToken = WeiXinTemplateProcessor.GetAccessToken(false);
				wxJsonResult = WxTemplateHelp.WxCommonSend<WxJsonResult>(accessToken, urlFormat, data, CommonJsonSendType.POST);
			}
			if (wxJsonResult.errcode != 0)
			{
				return new OperationResult(OperationResultType.Error, wxJsonResult.errmsg, null);
			}
			return new OperationResult(OperationResultType.Success, "", null);
		}

		public OperationResult Addtemplate(string template_id_short)
		{
			string accessToken = WeiXinTemplateProcessor.GetAccessToken(true);
			if (string.IsNullOrEmpty(accessToken) || accessToken.StartsWith("error"))
			{
				return new OperationResult(OperationResultType.Error, "获取AccessToken失败！", null);
			}
			string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";
			var data = new
			{
				template_id_short
			};
			AddtemplateJsonResult addtemplateJsonResult = WxTemplateHelp.WxCommonSend<AddtemplateJsonResult>(accessToken, urlFormat, data, CommonJsonSendType.POST);
			if (addtemplateJsonResult.errcode != 0)
			{
				return new OperationResult(OperationResultType.Error, addtemplateJsonResult.errmsg, null);
			}
			return new OperationResult(OperationResultType.Success, addtemplateJsonResult.template_id, null);
		}

		private static AddtemplateJsonResult AddtemplateJsonResult(string accessToken, string shortId)
		{
			string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";
			var data = new
			{
				template_id_short = shortId
			};
			AddtemplateJsonResult addtemplateJsonResult = WxTemplateHelp.WxCommonSend<AddtemplateJsonResult>(accessToken, urlFormat, data, CommonJsonSendType.POST);
			if (addtemplateJsonResult.errcode == ReturnCode.获取access_token时AppSecret错误或者access_token无效)
			{
				accessToken = WeiXinTemplateProcessor.GetAccessToken(false);
				addtemplateJsonResult = WxTemplateHelp.WxCommonSend<AddtemplateJsonResult>(accessToken, urlFormat, data, CommonJsonSendType.POST);
			}
			return addtemplateJsonResult;
		}

		public static OperationResult QuickSetWeixinTemplates()
		{
			string accessToken = WeiXinTemplateProcessor.AccessToken;
			if (string.IsNullOrEmpty(accessToken) || accessToken.StartsWith("error"))
			{
				return new OperationResult(OperationResultType.Error, "获取AccessToken失败！", null);
			}
			GetIndustryJsonResult industryJsonResult = WeiXinTemplateProcessor.GetIndustryJsonResult(accessToken);
			if (industryJsonResult.errcode == ReturnCode.获取access_token时AppSecret错误或者access_token无效)
			{
				accessToken = WeiXinTemplateProcessor.GetAccessToken(false);
				industryJsonResult = WeiXinTemplateProcessor.GetIndustryJsonResult(accessToken);
			}
			if (industryJsonResult.errcode != 0 && industryJsonResult.errcode != ReturnCode.系统繁忙此时请开发者稍候再试)
			{
				return new OperationResult(OperationResultType.Error, industryJsonResult.errmsg, null);
			}
			if (industryJsonResult.errcode == ReturnCode.系统繁忙此时请开发者稍候再试 || industryJsonResult.primary_industry.ConvertToIndustryCode() != IndustryCode.IT科技_互联网_电子商务 || industryJsonResult.secondary_industry.ConvertToIndustryCode() != IndustryCode.IT科技_IT软件与服务)
			{
				OperationResult operationResult = WeiXinTemplateProcessor.SetIndustry();
				if (operationResult.ResultType != OperationResultType.Success)
				{
					return operationResult;
				}
			}
			PrivateTemplateJsonResult privateTemplateJsonResult = WeiXinTemplateProcessor.GetPrivateTemplateJsonResult(accessToken);
			if (privateTemplateJsonResult.errcode != 0)
			{
				return new OperationResult(OperationResultType.Error, privateTemplateJsonResult.errmsg, null);
			}
			List<GetPrivateTemplate_TemplateItem> template_list = privateTemplateJsonResult.template_list;
			List<WxtemplateId> wxtemplateIds = WeiXinTemplateProcessor.GetWxtemplateIds();
			int num = template_list.Count;
			foreach (WxtemplateId item in wxtemplateIds)
			{
				GetPrivateTemplate_TemplateItem getPrivateTemplate_TemplateItem = template_list.FirstOrDefault((GetPrivateTemplate_TemplateItem t) => t.title == item.name && t.primary_industry == "IT科技");
				if (getPrivateTemplate_TemplateItem != null)
				{
					item.templateid = getPrivateTemplate_TemplateItem.template_id;
				}
				else if (num >= 25)
				{
					item.templateid = "公众号已有模板数量越额了！";
				}
				else
				{
					AddtemplateJsonResult addtemplateJsonResult = WeiXinTemplateProcessor.AddtemplateJsonResult(accessToken, item.shortId);
					if (addtemplateJsonResult.errcode != 0)
					{
						item.templateid = addtemplateJsonResult.errmsg;
					}
					else
					{
						num++;
						item.templateid = addtemplateJsonResult.template_id;
					}
				}
			}
			return new OperationResult(OperationResultType.Success, "", wxtemplateIds);
		}

		private static List<WxtemplateId> GetWxtemplateIds()
		{
			List<WxtemplateId> list = new List<WxtemplateId>();
			list.Add(new WxtemplateId
			{
				name = "密码修改提醒",
				shortId = "TM00444",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "找回密码通知",
				shortId = "OPENTM202110965",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "订单关闭通知",
				shortId = "TM00984",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "订单提交成功",
				shortId = "TM00016",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "订单支付成功",
				shortId = "TM00015",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "退款通知",
				shortId = "TM00004",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "订单发货提醒",
				shortId = "OPENTM200565259",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "组团失败提醒",
				shortId = "OPENTM400833482",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "伙拼组团成功提醒",
				shortId = "OPENTM401915640",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "佣金提醒",
				shortId = "OPENTM201812627",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "即将过期提醒",
				shortId = "OPENTM401096849",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "会员加入提醒",
				shortId = "OPENTM207679900",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "售后服务处理进度提醒",
				shortId = "TM00254",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "预售订单通知",
				shortId = "OPENTM401301555",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "产品即将到期提醒",
				shortId = "OPENTM207251323",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "提现结果通知",
				shortId = "OPENTM207601150",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "待办任务提醒",
				shortId = "OPENTM200806215",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "核销成功通知",
				shortId = "OPENTM405896004",
				templateid = ""
			});
			list.Add(new WxtemplateId
			{
				name = "退款失败通知",
				shortId = "OPENTM409830252",
				templateid = ""
			});
			return list;
		}

		public static bool SaveWXTempalteId(IDictionary<string, string> templateIds)
		{
			return new MessageTemplateDao().SaveWXTempalteId(templateIds);
		}

		public static bool SaveWXTempalteIdOfMsgType(IList<MessageTemplate> savedata)
		{
			return new MessageTemplateDao().SaveWXTempalteIdOfMsgType(savedata);
		}

		public static bool SaveAppletTempalteIdOfMsgType(IList<MessageTemplate> savedata)
		{
			return new MessageTemplateDao().SaveAppletTempalteIdOfMsgType(savedata);
		}

		public static bool SaveO2OAppletTempalteIdOfMsgType(IList<MessageTemplate> savedata)
		{
			return new MessageTemplateDao().SaveO2OAppletTempalteIdOfMsgType(savedata);
		}

		private static GetIndustryJsonResult GetIndustryJsonResult(string accessToken)
		{
			string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/get_industry?access_token={0}";
			return WxTemplateHelp.WxCommonSend<GetIndustryJsonResult>(accessToken, urlFormat, null, CommonJsonSendType.GET);
		}

		public static OperationResult GetPrivateTemplate()
		{
			string accessToken = WeiXinTemplateProcessor.AccessToken;
			if (string.IsNullOrEmpty(accessToken) || accessToken.StartsWith("error"))
			{
				return new OperationResult(OperationResultType.Error, "获取AccessToken失败！", null);
			}
			PrivateTemplateJsonResult privateTemplateJsonResult = WeiXinTemplateProcessor.GetPrivateTemplateJsonResult(accessToken);
			if (privateTemplateJsonResult.errcode != 0)
			{
				return new OperationResult(OperationResultType.Error, privateTemplateJsonResult.errmsg, null);
			}
			return new OperationResult(OperationResultType.Success, privateTemplateJsonResult);
		}

		private static PrivateTemplateJsonResult GetPrivateTemplateJsonResult(string accessToken)
		{
			string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token={0}";
			PrivateTemplateJsonResult privateTemplateJsonResult = WxTemplateHelp.WxCommonSend<PrivateTemplateJsonResult>(accessToken, urlFormat, null, CommonJsonSendType.GET);
			if (privateTemplateJsonResult.errcode == ReturnCode.获取access_token时AppSecret错误或者access_token无效)
			{
				accessToken = WeiXinTemplateProcessor.GetAccessToken(false);
				privateTemplateJsonResult = WxTemplateHelp.WxCommonSend<PrivateTemplateJsonResult>(accessToken, urlFormat, null, CommonJsonSendType.GET);
			}
			return privateTemplateJsonResult;
		}

		public static OperationResult DelPrivateTemplate(string template_id)
		{
			string accessToken = WeiXinTemplateProcessor.AccessToken;
			if (string.IsNullOrEmpty(accessToken) || accessToken.StartsWith("error"))
			{
				return new OperationResult(OperationResultType.Error, "获取AccessToken失败！", null);
			}
			string urlFormat = "https://api.weixin.qq.com/cgi-bin/template/del_private_template?access_token={0}";
			var data = new
			{
				template_id
			};
			WxJsonResult wxJsonResult = WxTemplateHelp.WxCommonSend<WxJsonResult>(accessToken, urlFormat, data, CommonJsonSendType.POST);
			if (wxJsonResult.errcode != 0)
			{
				return new OperationResult(OperationResultType.Error, wxJsonResult.errmsg, null);
			}
			return new OperationResult(OperationResultType.Success, "模板删除成功！", null);
		}
	}
}
