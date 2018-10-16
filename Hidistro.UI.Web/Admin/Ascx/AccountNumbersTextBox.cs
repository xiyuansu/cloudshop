using Hidistro.Context;
using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Ascx
{
	public class AccountNumbersTextBox : UserControl
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		protected TextBox txtContent;

		public string CssClass
		{
			set
			{
				this.txtContent.CssClass = value;
			}
		}

		public string Text
		{
			get
			{
				string value = this.txtContent.Attributes["v"];
				if (this.siteSettings.IsDemoSite && !string.IsNullOrEmpty(value))
				{
					return HiCryptographer.TryDecypt(this.txtContent.Attributes["v"]);
				}
				return this.txtContent.Text.Trim();
			}
			set
			{
				if (this.siteSettings.IsDemoSite && !string.IsNullOrEmpty(value))
				{
					this.txtContent.Text = "************************";
					this.txtContent.ReadOnly = true;
					this.txtContent.Attributes.Add("v", HiCryptographer.Encrypt(value));
					this.txtContent.Style.Add("border-color", "#0091EA");
					this.txtContent.ToolTip = "演示站点自动隐藏私密信息且不允许修改";
				}
				else
				{
					this.txtContent.Text = value;
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}
