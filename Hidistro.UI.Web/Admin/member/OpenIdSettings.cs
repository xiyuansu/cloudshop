using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Hishop.Plugins;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.OpenIdSettings)]
	public class OpenIdSettings : AdminPage
	{
		private string openIdType;

		protected Literal lblDisplayName;

		protected TextBox txtName;

		protected Ueditor fcContent;

		protected Button btnSave;

		protected Literal lblDisplayName2;

		protected ImageList ImageList;

		protected HiddenField txtSelectedName;

		protected HiddenField txtConfigData;

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += this.btnSave_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.openIdType = base.Request.QueryString["t"];
			if (string.IsNullOrEmpty(this.openIdType) || this.openIdType.Trim().Length == 0)
			{
				base.GotoResourceNotFound();
			}
			PluginItem pluginItem = OpenIdPlugins.Instance().GetPluginItem(this.openIdType);
			if (pluginItem == null)
			{
				base.GotoResourceNotFound();
			}
			if (!this.Page.IsPostBack)
			{
				this.txtName.Text = pluginItem.DisplayName;
				this.lblDisplayName.Text = pluginItem.DisplayName;
				this.lblDisplayName2.Text = pluginItem.DisplayName;
				this.txtSelectedName.Value = this.openIdType;
				OpenIdSettingInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(this.openIdType);
				if (openIdSettings != null)
				{
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(openIdSettings.Settings));
					this.txtConfigData.Value = configData.SettingsXml;
					this.txtName.Text = openIdSettings.Name;
					this.fcContent.Text = openIdSettings.Description;
				}
				this.lblDisplayName2.Visible = false;
			}
		}

		private ConfigData LoadConfig()
		{
			this.txtSelectedName.Value = this.openIdType;
			this.txtConfigData.Value = "";
			ConfigablePlugin configablePlugin = OpenIdService.CreateInstance(this.openIdType);
			if (configablePlugin == null)
			{
				return null;
			}
			ConfigData configData = configablePlugin.GetConfigData(base.Request.Form);
			if (configData != null)
			{
				this.txtConfigData.Value = configData.SettingsXml;
			}
			return configData;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			ConfigData configData = default(ConfigData);
			if (this.ValidateValues(out configData))
			{
				OpenIdSettingInfo settings = new OpenIdSettingInfo
				{
					Name = this.txtName.Text.Trim(),
					Description = this.fcContent.Text,
					OpenIdType = this.openIdType,
					Settings = HiCryptographer.Encrypt(configData.SettingsXml)
				};
				OpenIdHelper.SaveSettings(settings);
				base.Response.Redirect("openidservices.aspx");
			}
		}

		private bool ValidateValues(out ConfigData data)
		{
			string text = string.Empty;
			data = this.LoadConfig();
			if (!data.IsValid)
			{
				foreach (string errorMsg in data.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(errorMsg);
				}
			}
			if (string.IsNullOrEmpty(this.txtName.Text) || this.txtName.Text.Trim().Length == 0 || this.txtName.Text.Length > 50)
			{
				text += Formatter.FormatErrorMessage("显示名称不能为空，长度限制在50个字符以内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
