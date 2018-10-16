using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SetSynJDParam)]
	public class JDSettings : AdminPage
	{
		protected TextBox JdAppKeyTextBox;

		protected TextBox JdAppSecretTextBox;

		protected TextBox JdAccessTokenTextBox;

		protected HyperLink createAccessTokenHyperLink;

		protected Button btnOK;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.JdAppKeyTextBox.Text = masterSettings.JDAppKey;
				this.JdAppSecretTextBox.Text = masterSettings.JDAppSecret;
				this.JdAccessTokenTextBox.Text = masterSettings.JDAccessToken;
				if (string.IsNullOrWhiteSpace(masterSettings.JDAppKey))
				{
					this.createAccessTokenHyperLink.NavigateUrl = "javascript:alert('请先保存AppKey');";
				}
				else
				{
					this.createAccessTokenHyperLink.NavigateUrl = $"https://oauth.jd.com/oauth/authorize?response_type=code&client_id={masterSettings.JDAppKey}&redirect_uri=urn:ietf:wg:oauth:2.0:oob&state=1212";
				}
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings siteSettings = new SiteSettings();
			siteSettings.JDAppKey = this.JdAppKeyTextBox.Text;
			siteSettings.JDAppSecret = this.JdAppSecretTextBox.Text;
			siteSettings.JDAccessToken = this.JdAccessTokenTextBox.Text;
			SettingsManager.Save(siteSettings);
		}
	}
}
