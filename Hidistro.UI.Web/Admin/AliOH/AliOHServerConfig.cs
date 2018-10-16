using Hidistro.Context;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.App_Code;
using System;
using System.IO;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohServerConfig)]
	public class AliOHServerConfig : AdminPage
	{
		protected TextBox txtAppId;

		protected TextBox txtAppWelcome;

		protected Literal txtUrl;

		protected TextBox txtPubKey;

		protected Button btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string path = base.Server.MapPath("~/config/rsa_public_key.pem");
				string text = File.Exists(path) ? RsaKeyHelper.GetRSAKeyContent(path, true) : this.CreateRsaKey();
				this.txtAppId.Text = masterSettings.AliOHAppId;
				this.txtAppWelcome.Text = masterSettings.AliOHFollowRelay;
				this.txtUrl.Text = $"http://{base.Request.Url.Host}/api/alipay.ashx";
				this.txtPubKey.Text = text;
			}
		}

		private string CreateRsaKey()
		{
			string keyDirectory = base.Server.MapPath("~/config");
			string generatorPath = base.Server.MapPath("~/config/RSAGenerator/Rsa.exe");
			return RsaKeyHelper.CreateRSAKeyFile(generatorPath, keyDirectory);
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.AliOHAppId = this.txtAppId.Text;
			masterSettings.AliOHFollowRelay = this.txtAppWelcome.Text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("修改成功", true, "AliOHServerConfig");
		}
	}
}
