using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Members;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace Hidistro.UI.Web.OpenID
{
	public class LogisticsAddress : Page
	{
		protected HtmlForm form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			string openIdType = "hishop.plugins.openid.alipay.alipayservice";
			OpenIdSettingInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(openIdType);
			if (openIdSettings != null)
			{
				string value = base.Request.QueryString["alipaytoken"];
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(HiCryptographer.Decrypt(openIdSettings.Settings));
				SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
				sortedDictionary.Add("service", "user.logistics.address.query");
				sortedDictionary.Add("partner", xmlDocument.FirstChild.SelectSingleNode("Partner").InnerText);
				sortedDictionary.Add("_input_charset", "utf-8");
				sortedDictionary.Add("return_url", Globals.FullPath("openid/LogisticsAddress_url"));
				sortedDictionary.Add("token", value);
				Dictionary<string, string> dictionary = OpenIdFunction.FilterPara(sortedDictionary);
				string value2 = OpenIdFunction.BuildMysign(dictionary, xmlDocument.FirstChild.SelectSingleNode("Key").InnerText, "MD5", "utf-8");
				dictionary.Add("sign", value2);
				dictionary.Add("sign_type", "MD5");
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<string, string> item in dictionary)
				{
					stringBuilder.Append(OpenIdFunction.CreateField(item.Key, item.Value));
				}
				sortedDictionary.Clear();
				dictionary.Clear();
				OpenIdFunction.Submit(OpenIdFunction.CreateForm(stringBuilder.ToString(), "https://mapi.alipay.com/gateway.do?_input_charset=utf-8"));
			}
		}
	}
}
