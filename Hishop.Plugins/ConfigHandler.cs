using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hishop.Plugins
{
    public class ConfigHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string text = context.Request["type"];
                if (text != null)
                {
                    if (!(text == "PaymentRequest"))
                    {
                        if (!(text == "OpenIdService"))
                        {
                            if (!(text == "EmailSender"))
                            {
                                if (!(text == "SMSSender"))
                                {
                                    if (!(text == "Logistics"))
                                    {
                                        goto end_IL_0001;
                                    }
                                }
                                else
                                {
                                    ConfigHandler.ProcessSMSSender(context);
                                }
                            }
                            else
                            {
                                ConfigHandler.ProcessEmailSender(context);
                            }
                        }
                        else
                        {
                            this.ProcessOpenId(context);
                        }
                    }
                    else
                    {
                        ConfigHandler.ProcessPaymentRequest(context);
                    }
                }
            end_IL_0001:;
            }
            catch
            {
            }
        }

        private void ProcessOpenId(HttpContext context)
        {
            if (context.Request["action"] == "getlist")
            {
                OpenIdPlugins openIdPlugins = OpenIdPlugins.Instance();
                context.Response.ContentType = "application/json";
                context.Response.Write(openIdPlugins.GetPlugins().ToJsonString());
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                OpenIdService openIdService = OpenIdService.CreateInstance(context.Request["name"]);
                if (openIdService == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    context.Response.Write(openIdService.GetMetaData().OuterXml);
                }
            }
        }

        private static void ProcessPaymentRequest(HttpContext context)
        {
            if (context.Request["action"] == "getlist")
            {
                PaymentPlugins paymentPlugins = PaymentPlugins.Instance();
                context.Response.ContentType = "application/json";
                context.Response.Write(paymentPlugins.GetPlugins().ToJsonString());
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                PaymentRequest paymentRequest = PaymentRequest.CreateInstance(context.Request["name"]);
                if (paymentRequest == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    context.Response.Write(paymentRequest.GetMetaData().OuterXml);
                }
            }
        }

        private static void ProcessSMSSender(HttpContext context)
        {
            IDictionary<string, string> dictionary = Globals.NameValueCollectionToDictionary(context.Request.Form);
            Globals.AppendLog(dictionary, "ProcessSMSSender action:" + context.Request["action"], "", "", "/log/ConfigHandler.txt");
            if (context.Request["action"] == "getlist")
            {
                try
                {
                    SMSPlugins sMSPlugins = SMSPlugins.Instance();
                    context.Response.ContentType = "application/json";
                    Globals.AppendLog(dictionary, "ProcessSMSSender getlist:" + sMSPlugins.GetPlugins().ToJsonString(), "", "", "/log/ConfigHandler.txt");
                    context.Response.Write(sMSPlugins.GetPlugins().ToJsonString());
                }
                catch (Exception e)
                {
                    Globals.AppendLog(dictionary, "ProcessSMSSender getlist error:" + e.Message + "----" + e.StackTrace, "", "", "/log/ConfigHandler.txt");
                    //context.Response.Write(sMSPlugins.GetPlugins().ToJsonString());
                }
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                SMSSender sMSSender = SMSSender.CreateInstance(context.Request["name"]);
                Globals.AppendLog(dictionary, "ProcessSMSSender getmetadata name:" + context.Request["name"], "", "", "/log/ConfigHandler.txt");
                if (sMSSender == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    Globals.AppendLog(dictionary, "ProcessSMSSender getmetadata:" + sMSSender.GetMetaData().OuterXml.ToString(), "", "", "/log/ConfigHandler.txt");
                    context.Response.Write(sMSSender.GetMetaData().OuterXml);
                }
            }
        }

        private static void ProcessEmailSender(HttpContext context)
        {
            if (context.Request["action"] == "getlist")
            {
                EmailPlugins emailPlugins = EmailPlugins.Instance();
                context.Response.ContentType = "application/json";
                context.Response.Write(emailPlugins.GetPlugins().ToJsonString());
            }
            else if (context.Request["action"] == "getmetadata")
            {
                context.Response.ContentType = "text/xml";
                EmailSender emailSender = EmailSender.CreateInstance(context.Request["name"]);
                if (emailSender == null)
                {
                    context.Response.Write("<xml></xml>");
                }
                else
                {
                    context.Response.Write(emailSender.GetMetaData().OuterXml);
                }
            }
        }
    }
}
