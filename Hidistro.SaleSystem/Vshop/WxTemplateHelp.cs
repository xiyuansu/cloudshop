using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.CommonAPIs;
using System;

namespace Hidistro.SaleSystem.Vshop
{
	public class WxTemplateHelp
	{
		public static WxTemplateMessageResult SendTemplateMessage(string accessTocken, TempleteModel TempleteModel)
		{
			string urlFormat = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
			return WxTemplateHelp.WxCommonSend<WxTemplateMessageResult>(accessTocken, urlFormat, TempleteModel, CommonJsonSendType.POST);
		}

		public static T WxCommonSend<T>(string accessToken, string urlFormat, object data, CommonJsonSendType sendType) where T : WxJsonResult, new()
		{
			T val = new T();
			try
			{
				val = CommonJsonSend.Send<T>(accessToken, urlFormat, data, sendType, 1000, false);
			}
			catch (ErrorJsonResultException ex)
			{
				val.errcode = ex.JsonResult.errcode;
				val.errmsg = ex.JsonResult.errmsg;
			}
			catch (Exception ex2)
			{
				val.errcode = ReturnCode.系统错误system_error;
				val.errmsg = ex2.Message;
			}
			return val;
		}
	}
}
