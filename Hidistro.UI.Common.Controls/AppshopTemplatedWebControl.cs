using Hidistro.Context;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class AppshopTemplatedWebControl : TemplatedWebControl
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
			string text = this.ControlText();
			if (!string.IsNullOrEmpty(text))
			{
				Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}

		private string ControlText()
		{
			if (this.SkinFileExists)
			{
				StringBuilder stringBuilder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
				if (stringBuilder.Length == 0)
				{
					return null;
				}
				stringBuilder.Replace("<%", "").Replace("%>", "");
				string appshopSkinPath = HiContext.Current.GetAppshopSkinPath();
				stringBuilder.Replace("/images/", appshopSkinPath + "/images/");
				stringBuilder.Replace("/script/", appshopSkinPath + "/script/");
				stringBuilder.Replace("/style/", appshopSkinPath + "/style/");
				stringBuilder.Replace("/utility/", "/utility/");
				stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
				stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
				stringBuilder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
				return stringBuilder.ToString();
			}
			return null;
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

		protected virtual void ShowWapMessage(string msg, string goUrl = "")
		{
			HtmlInputHidden htmlInputHidden = new HtmlInputHidden();
			htmlInputHidden.ID = "LoadMessage";
			htmlInputHidden.ClientIDMode = ClientIDMode.Static;
			HtmlInputHidden htmlInputHidden2 = new HtmlInputHidden();
			htmlInputHidden2.ID = "ErrorToPage";
			htmlInputHidden2.ClientIDMode = ClientIDMode.Static;
			htmlInputHidden.Value = msg;
			htmlInputHidden2.Value = goUrl;
			base.Controls.Add(htmlInputHidden);
			base.Controls.Add(htmlInputHidden2);
		}

		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.Page.Response.Redirect("/AppShop/AppResourceNotFound?errorMsg=" + errorMsg);
		}
	}
}
