using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Urls;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true)]
	[PersistChildren(false)]
	public abstract class HtmlTemplatedWebControl : TemplatedWebControl
	{
		private string skinName;

		protected virtual string SkinPath
		{
			get
			{
				string text = HiContext.Current.GetSkinPath();
				if (this.SkinName.StartsWith(text))
				{
					return this.SkinName;
				}
				if (this.SkinName.ToLower().Trim().Equals("skin-default.html") || this.SkinName.ToLower().Trim().Contains("skin-desig_"))
				{
					text = HiContext.Current.GetPCHomePageSkinPath();
				}
				if (this.SkinName.Trim().StartsWith("skin-pctopichomepage_"))
				{
					text = "/Templates/topic/pctopic";
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
			string text = HiContext.Current.Context.Request.Url.ToNullString().ToLower();
			if (!HiContext.Current.SiteSettings.OpenPcShop && text.IndexOf("/productdetails") == -1 && text.IndexOf("/product_detail") == -1 && text.IndexOf("/fightgroupactivitydetails") == -1 && text.IndexOf("/countdownproductsdetails") == -1 && text.IndexOf("/groupbuyproducts") == -1 && text.IndexOf("/countdownproducts") == -1)
			{
				this.Page.Response.Redirect("/Admin/Login");
			}
			if (HiContext.Current.UserId > 0 && HiContext.Current.User.IsReferral() && !HiContext.Current.User.Referral.IsRepeled && this.Page.Request.QueryString["ReferralUserId"] == null)
			{
				string text2 = this.Page.Request.CurrentExecutionFilePath.ToNullString().ToLower();
				int num = this.Page.Request.QueryString["TopicId"].ToInt(0);
				string text3 = this.Page.Request.Url.ToString().ToLower();
				if (text3.IndexOf("/default") > -1 && text2.StartsWith("/topics") && num == HiContext.Current.SiteSettings.HomePageTopicId && text3.IndexOf("referraluserid=") == -1)
				{
					this.Page.Response.Redirect((text3.IndexOf("?") > -1) ? (text3 + "&ReferralUserId=" + HiContext.Current.User.UserId) : (text3 + "?ReferralUserId=" + HiContext.Current.User.UserId));
				}
				else
				{
					int num2 = text3.IndexOf("returnurl");
					string[] array = new string[6]
					{
						"/product_detail",
						"/productdetails",
						"/countdownproductsdetails",
						"/groupbuyproductdetails",
						"/default",
						"/topics"
					};
					string[] array2 = array;
					foreach (string value in array2)
					{
						int num3 = text3.IndexOf(value);
						if (num3 > -1 && (num3 < num2 || num2 == -1) && text2.StartsWith(value))
						{
							string text4 = this.Page.Request.QueryString["ReturnUrl"].ToNullString();
							if (num2 > -1)
							{
								text3 = text3.Replace("?returnurl=" + text4, "").Replace("&returnurl=" + text4, "");
							}
							if (string.IsNullOrEmpty(text4))
							{
								this.Page.Response.Redirect((text3.IndexOf("?") > -1) ? (text3 + "&ReferralUserId=" + HiContext.Current.User.UserId) : (text3 + "?ReferralUserId=" + HiContext.Current.User.UserId));
							}
							else
							{
								this.Page.Response.Redirect(((text3.IndexOf("?") > -1) ? (text3 + "&ReferralUserId=" + HiContext.Current.User.UserId) : (text3 + "?ReferralUserId=" + HiContext.Current.User.UserId)) + "&returnUrl=" + text4);
							}
						}
					}
				}
			}
			base.OnInit(e);
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
				text = TemplatedWebControl.ReplaceImageServerUrl(text);
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
				string str = (this.SkinName.ToLower().Trim().Equals("skin-default.html") || this.SkinName.ToLower().Trim().Contains("skin-desig_")) ? HiContext.Current.GetPCHomePageSkinPath() : HiContext.Current.GetSkinPath();
				string skinPath = HiContext.Current.GetSkinPath();
				if (!this.SkinName.ToLower().Trim().Contains("skin-pctopichomepage"))
				{
					stringBuilder.Replace("/images/", str + "/images/");
				}
				stringBuilder.Replace("/script/", skinPath + "/script/");
				stringBuilder.Replace("/style/", skinPath + "/style/");
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

		protected void GotoResourceNotFound()
		{
			this.Page.Response.Redirect("/ResourceNotFound");
		}

		protected string GetParameter(string name, bool getCookie = false)
		{
			return RouteConfig.GetParameter(this.Page, name, false);
		}
	}
}
