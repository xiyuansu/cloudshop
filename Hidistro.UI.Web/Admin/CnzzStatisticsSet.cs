using Hidistro.Context;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.IO;
using System.Net;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class CnzzStatisticsSet : AdminPage
	{
		protected HtmlGenericControl div_pan1;

		protected LinkButton hlinkCreate;

		protected HtmlGenericControl div_pan2;

		protected OnOff ooOpenCnzz;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.hlinkCreate.Click += this.hlinkCreate_Click;
			if (!base.IsPostBack)
			{
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (string.IsNullOrEmpty(siteSettings.CnzzPassword) || string.IsNullOrEmpty(siteSettings.CnzzUsername))
				{
					this.div_pan1.Visible = true;
					this.div_pan2.Visible = false;
				}
				else
				{
					this.div_pan1.Visible = false;
					this.div_pan2.Visible = true;
					this.ooOpenCnzz.SelectedValue = siteSettings.EnabledCnzz;
				}
			}
		}

		protected void hlinkCreate_Click(object sender, EventArgs e)
		{
			string host = this.Page.Request.Url.Host;
			string str = FormsAuthentication.HashPasswordForStoringInConfigFile(host + "A9jkLUxm", "MD5").ToLower();
			string requestUriString = "http://wss.cnzz.com/user/companion/92hi.php?domain=" + host + "&key=" + str;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			responseStream.ReadTimeout = 100;
			StreamReader streamReader = new StreamReader(responseStream);
			string text = streamReader.ReadToEnd().Trim();
			streamReader.Close();
			if (text.IndexOf("@") == -1)
			{
				this.ShowMsg("创建账号失败", false);
			}
			else
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string[] array = text.Split('@');
				masterSettings.CnzzUsername = array[0];
				masterSettings.CnzzPassword = array[1];
				masterSettings.EnabledCnzz = false;
				this.div_pan1.Visible = false;
				this.div_pan2.Visible = true;
				SettingsManager.Save(masterSettings);
				this.ShowMsg("创建账号成功", true);
			}
		}
	}
}
