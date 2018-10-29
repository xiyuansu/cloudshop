using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Hishop.Plugins
{
    public abstract class SMSSender : ConfigablePlugin, IPlugin
    {
        public static SMSSender CreateInstance(string name, string configXml)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            Type plugin = SMSPlugins.Instance().GetPlugin("SMSSender", name);
            if (plugin == null)
            {
                return null;
            }
            SMSSender sMSSender = Activator.CreateInstance(plugin) as SMSSender;
            if (sMSSender != null && !string.IsNullOrEmpty(configXml))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.XmlResolver = null;
                xmlDocument.LoadXml(configXml);
                sMSSender.InitConfig(xmlDocument.FirstChild);
            }
            Globals.AppendLog(new Dictionary<string, string>(), "SMSSender CreateInstance" + "--" + name + ":" + configXml, "", "", "/log/SMSSender.txt");
            return sMSSender;
        }

        public static SMSSender CreateInstance(string name)
        {
            return SMSSender.CreateInstance(name, null);
        }

        /// <summary>
        /// 2018-7-8 新增的方法
        /// </summary>
        /// <param name="cellPhone"></param>
        /// <param name="TemplateCode"></param>
        /// <param name="message">短信模板变量替换JSON串,友情提示:如果JSON中需要带换行符,请参照标准的JSON协议对换行符的要求,比如短信内容中包含\r\n的情况在JSON中需要表示成\r\n,否则会导致JSON在服务端解析失败</param>
        /// <param name="returnMsg"></param>
        ///  <param name="OutId">可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者</param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public abstract bool Send(string cellPhone, string TemplateCode, string message, out string returnMsg, string OutId = "", string speed = "0");

        public abstract bool Send(string[] phoneNumbers, string TemplateCode, string message, out string returnMsg, string OutId = "", string speed = "1");

    }
}
