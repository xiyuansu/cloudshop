using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public abstract class NewWAPTemplateWebControl : TemplatedWebControl
	{
		private string skinName;

		protected virtual string SkinPath
		{
			get
			{
				string text = HiContext.Current.GetCommonSkinPath();
				if (this.SkinName.StartsWith(text))
				{
					return this.SkinName;
				}
				if (this.SkinName.Trim().Equals("skin-homepage.html"))
				{
					text = HiContext.Current.GetMobileHomePagePath();
				}
				if (this.SkinName.Trim().StartsWith("skin-topichomepage_"))
				{
					text = "/Templates/topic/waptopic";
				}
				if (this.SkinName.StartsWith("/"))
				{
					return text + this.SkinName;
				}
				return text + "/" + this.SkinName;
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
					if (value.EndsWith(".html") || value.EndsWith(".ascx"))
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

		public ClientType ClientType
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			this.CheckAuth();
			base.OnInit(e);
		}

		protected void CheckAuth()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string domainName = Globals.DomainName;
			string text = HttpContext.Current.Request.UserAgent;
			if (string.IsNullOrEmpty(text))
			{
				text = "";
			}
			string text2 = HttpContext.Current.Request.Url.ToString().ToLower();
			bool flag = this.Page.Request.QueryString["source"].ToNullString() != "";
			if (masterSettings.AutoRedirectClient)
			{
				if (text2.IndexOf("/wapshop/") != -1 && text.ToLower().IndexOf("micromessenger") > -1 && masterSettings.OpenVstore == 1)
				{
					string text3 = HttpContext.Current.Request.Url.ToString().ToLower().Replace("/wapshop/", "/vShop/");
					if (!flag)
					{
						text3 = text3 + (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query) ? "?" : "&") + "source=wap";
					}
					this.Page.Response.Redirect(text3, true);
				}
				else if (text2.IndexOf("/vshop/") != -1 && text.ToLower().IndexOf("micromessenger") > -1 && masterSettings.OpenVstore == 0)
				{
					string text4 = HttpContext.Current.Request.Url.ToString().ToLower().Replace("/vshop/", "/wapShop/");
					if (!flag)
					{
						text4 = text4 + (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query) ? "?" : "&") + "source=vshop";
					}
					this.Page.Response.Redirect(text4, true);
				}
				else if (text2.IndexOf("/vshop/") != -1 && text.ToLower().IndexOf("micromessenger") == -1 && masterSettings.OpenWap == 1)
				{
					string text5 = HttpContext.Current.Request.Url.ToString().ToLower().Replace("/vshop/", "/wapShop/");
					if (!flag)
					{
						text5 = text5 + (string.IsNullOrEmpty(HttpContext.Current.Request.Url.Query) ? "?" : "&") + "source=vshop";
					}
					this.Page.Response.Redirect(text5, true);
				}
			}
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			this.LoadHtmlThemedControl();
		}

		protected bool LoadHtmlThemedControl()
		{
			string text = File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8);
			if (!string.IsNullOrEmpty(text))
			{
				text = TemplatedWebControl.ReplaceImageServerUrl(text);
				Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}

		public void ReloadPage(NameValueCollection queryStrings)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}

		public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}

		private string GenericReloadUrl(NameValueCollection queryStrings)
		{
			if (queryStrings == null || queryStrings.Count == 0)
			{
				return this.Page.Request.Url.AbsolutePath;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
			foreach (string key in queryStrings.Keys)
			{
				if (queryStrings[key] != null)
				{
					string text2 = queryStrings[key].Trim();
					if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
					{
						stringBuilder.Append(key).Append("=").Append(this.Page.Server.UrlEncode(text2))
							.Append("&");
					}
				}
			}
			queryStrings.Clear();
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.Page.Response.Redirect("/ResourceNotFound_Mobile?errorMsg=" + errorMsg);
		}
	}
}
