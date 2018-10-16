// 云商城3.3 【tm-d】
// Bin: Hishop.Alipay.OpenHome.AlipayOHClient
// Assembly: Hishop.Alipay.OpenHome, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE0CB634-CEBB-4A2F-95A2-557C939CA031
// Assembly location: F:\云商城3.3\Bin\Hishop.Alipay.OpenHome.dll

using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Hishop.Alipay.OpenHome.AlipayOHException;
using Hishop.Alipay.OpenHome.Handle;
using Hishop.Alipay.OpenHome.Model;
using Hishop.Alipay.OpenHome.Request;
using Hishop.Alipay.OpenHome.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hishop.Alipay.OpenHome
{
    public class AlipayOHClient
    {
        private string signType = "RSA";
        private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private WebUtils webUtils = new WebUtils();
        private const string APP_ID = "app_id";
        private const string FORMAT = "format";
        private const string METHOD = "method";
        private const string TIMESTAMP = "timestamp";
        private const string VERSION = "version";
        private const string SIGN_TYPE = "sign_type";
        private const string ACCESS_TOKEN = "auth_token";
        private const string SIGN = "sign";
        private const string TERMINAL_TYPE = "terminal_type";
        private const string TERMINAL_INFO = "terminal_info";
        private const string PROD_CODE = "prod_code";
        private const string BIZ_CONTENT = "biz_content";
        private const string SING = "sign";
        private const string CONTENT = "biz_content";
        private const string SING_TYPE = "sign_type";
        private const string SERVICE = "service";
        private const string CHARSET = "charset";
        private string version;
        private string format;
        private string serverUrl;
        private string appId;
        private string privateKey;
        private string pubKey;
        private string aliPubKey;
        private string charset;
        public AliRequest request;
        private HttpContext context;

        public AlipayOHClient(string url, string appId, string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
        {
            this.serverUrl = url;
            this.appId = appId;
            this.privateKey = priKey;
            this.charset = charset;
            this.pubKey = pubKey;
            this.aliPubKey = aliPubKey;
        }

        public AlipayOHClient(string aliPubKey, string priKey, string pubKey, string charset = "UTF-8")
        {
            this.privateKey = priKey;
            this.charset = charset;
            this.pubKey = pubKey;
            this.aliPubKey = aliPubKey;
        }

        public event Hishop.Alipay.OpenHome.OnUserFollow OnUserFollow;

        internal string FireUserFollowEvent()
        {
            return this.OnUserFollow(this, new EventArgs());
        }

        public void HandleAliOHResponse(HttpContext context)
        {
            this.context = context;
            string str1 = context.Request["sign"];
            string str2 = context.Request["biz_content"];
            string str3 = context.Request["sign_type"];
            string str4 = context.Request["service"];
            this.request = XmlSerialiseHelper.Deserialize<AliRequest>(str2);
            IHandle handle = (IHandle)null;
            switch (this.request.EventType)
            {
                case "verifygw":
                    handle = (IHandle)new VerifyGateWayHandle();
                    break;
                case "follow":
                    handle = (IHandle)new UserFollowHandle();
                    break;
            }
            if (handle == null)
                return;
            handle.client = this;
            handle.LocalRsaPriKey = this.privateKey;
            handle.LocalRsaPubKey = this.pubKey;
            handle.AliRsaPubKey = this.aliPubKey;
            string s = handle.Handle(str2);
            context.Response.AddHeader("Content-Type", "application/xml");
            context.Response.Write(s);
        }

        public T Execute<T>(IRequest request) where T : AliResponse, IAliResponseStatus
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("method", request.GetMethodName());
            dictionary.Add("app_id", this.appId);
            dictionary.Add("timestamp", DateTime.Now.ToString(this.dateTimeFormat));
            dictionary.Add("sign_type", this.signType);
            dictionary.Add("charset", this.charset);
            dictionary.Add("biz_content", request.GetBizContent());
            dictionary.Add("sign", AlipaySignature.RSASign((IDictionary<string, string>)dictionary, this.privateKey, this.charset));
            T obj = JsonConvert.DeserializeObject<T>(this.webUtils.DoPost(this.serverUrl, (IDictionary<string, string>)dictionary, this.charset));
            if (obj.error_response != null && obj.error_response.IsError)
                throw new AliResponseException(obj.error_response.code, obj.error_response.sub_msg);
            if (obj.Code != "200")
                throw new AliResponseException(obj.Code, obj.Message);
            return obj;
        }

        public AlipaySystemOauthTokenResponse OauthTokenRequest(string authCode)
        {
            AlipaySystemOauthTokenRequest oauthTokenRequest = new AlipaySystemOauthTokenRequest();
            oauthTokenRequest.GrantType = AlipaySystemOauthTokenRequest.AllGrantType.authorization_code;
            oauthTokenRequest.Code = authCode;
            AlipaySystemOauthTokenResponse oauthTokenResponse = (AlipaySystemOauthTokenResponse)null;
            try
            {
                oauthTokenResponse = new DefaultAopClient(this.serverUrl, this.appId, this.privateKey).Execute<AlipaySystemOauthTokenResponse>((IAopRequest<AlipaySystemOauthTokenResponse>)oauthTokenRequest);
            }
            catch (AopException ex)
            {
            }
            return oauthTokenResponse;
        }

        public AlipayUserUserinfoShareResponse GetAliUserInfo(string accessToken)
        {
            return new DefaultAopClient(this.serverUrl, this.appId, this.privateKey).Execute<AlipayUserUserinfoShareResponse>((IAopRequest<AlipayUserUserinfoShareResponse>)new AlipayUserUserinfoShareRequest()
            {
                AuthToken = accessToken
            });
        }

        private class EventType
        {
            public const string Verifygw = "verifygw";
            public const string Follow = "follow";
        }
    }
}
