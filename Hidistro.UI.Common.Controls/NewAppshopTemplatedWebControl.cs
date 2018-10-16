using Hidistro.Context;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class NewAppshopTemplatedWebControl : TemplatedWebControl
	{
		private string skinName;

		protected virtual string SkinPath
		{
			get
			{
				string appshopSkinPath = HiContext.Current.GetAppshopSkinPath();
				if (this.SkinName.StartsWith(appshopSkinPath))
				{
					return this.SkinName;
				}
				if (this.SkinName.StartsWith("skin-apptopichomepage_"))
				{
					return "/Templates/topic/apptopic/" + this.SkinName;
				}
				if (this.SkinName.StartsWith("/"))
				{
					return appshopSkinPath + this.SkinName;
				}
				return appshopSkinPath + "/" + this.SkinName;
			}
		}

		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					value = value.ToLower(CultureInfo.InvariantCulture);
					if (value.EndsWith(".html"))
					{
						this.skinName = value;
					}
				}
			}
		}

		private bool SkinFileExists
		{
			get
			{
				return !string.IsNullOrEmpty(this.SkinName);
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		protected void CheckAuth()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.OpenMobbile != 1)
			{
				this.Page.Response.Redirect("/ResourceNotFound_Mobile?errormsg=抱歉，您暂未开通此服务！");
			}
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.LoadHtmlThemedControl())
			{
				this.AttachChildControls();
				return;
			}
			throw new SkinNotFoundException(this.SkinPath);
		}

		protected bool LoadHtmlThemedControl()
		{
			string text = File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8);
			if (!string.IsNullOrEmpty(text))
			{
				Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}

		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.Page.Response.Redirect("/AppShop/AppResourceNotFound?errorMsg=" + errorMsg);
		}
	}
}
