using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;

namespace Hishop.Plugins.SMS
{

    [Plugin("短信接口")]
    public class ymSMS : SMSSender
    {
        private const string  product = "Dysmsapi";//短信API产品名称
        private const string domain = "dysmsapi.aliyuncs.com";//短信API产品域名

      
        /// <summary>
        /// 群发
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <param name="TemplateCode"></param>
        /// <param name="message"></param>
        /// <param name="returnMsg"></param>
        /// <param name="OutId"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public override bool Send(string[] phoneNumbers, string TemplateCode, string message, out string returnMsg, string OutId = "", string speed = "1")
        {
            if ((((phoneNumbers == null) || (phoneNumbers.Length == 0)) || string.IsNullOrEmpty(message)) || (message.Trim().Length == 0))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            string phoneNumbersTot = string.Join(",", phoneNumbers);

            return this.Send(phoneNumbersTot, TemplateCode, message, out returnMsg, OutId, "0");
            
        }

        

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="cellPhone">必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”</param>
        /// <param name="TemplateCode">模板code</param>
        /// <param name="OutId">可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者</param>
        /// <param name="message">可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为 "{\"name\":\"Tom\"， \"code\":\"123\"}"</param>
        /// <param name="returnMsg"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public override bool Send(string cellPhone, string TemplateCode,string message, out string returnMsg, string OutId="", string speed = "0")
        {
            if (((string.IsNullOrEmpty(cellPhone)  || (cellPhone.Trim().Length == 0))))
            {
                returnMsg = "手机号码和消息内容不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(message))
            {
                message = "";
            }
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", Appkey, Appsecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            if (string.IsNullOrEmpty(TemplateCode) || TemplateCode.Length<=0)
            {
                TemplateCode = "SMS_139390116";//阿里云自带的
            }
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = cellPhone;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = SignName;// "阿里云短信测试专用";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = TemplateCode;// "SMS_71135039";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为 "{\"name\":\"Tom\"， \"code\":\"123\"}"
                request.TemplateParam = message;// "{\"name\":\"Tom\"， \"code\":\"123\"}"
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                if (string.IsNullOrEmpty(OutId) || OutId.Length <= 0)
                {
                    OutId = DateTime.Now.Ticks.ToString();
                }
                request.OutId = OutId;// "21212121211";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                
                returnMsg = sendSmsResponse.Message;
                if (sendSmsResponse.Code.ToLower() == "ok")
                {
                    return true;
                }
                else
                {
                    writeError(sendSmsResponse.Code, sendSmsResponse.Message);
                    return false;
                }
            

            }
            catch (ServerException e)
            {
                returnMsg = "未知错误(ServerException 1)";
                return false;
            }
            catch (ClientException e)
            {
                returnMsg = "未知错误(ClientException 2)";
                return false;
            }
        }

        public void writeError(string Code, string Message)
        {
            DataTable table = new DataTable {
                TableName = "SMSLog"
            };
            table.Columns.Add(new DataColumn("time"));
            table.Columns.Add(new DataColumn("code"));
            table.Columns.Add(new DataColumn("msg"));
            DataRow row = table.NewRow();
            row["time"] = DateTime.Now;
            row["code"] = Code;
            row["msg"] = Message;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Request.MapPath("/SMSLog.xml"));
        }

       

        [ConfigElement("AccessKeyID", Nullable=false)]
        public string Appkey { get; set; }

        [ConfigElement("AccessKeySecret", Nullable=false)]
        public string Appsecret { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [ConfigElement("SignName", Nullable = false)]
        public string SignName { get; set; }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }
    }
}

