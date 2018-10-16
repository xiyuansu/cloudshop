using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class Login : Page
	{
		private string LastActivyTime;

		private string verifyCodeKey = "VerifyCode";

		private readonly string noticeMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body class=\"body2\">\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的全网商城系统已过授权有效期，无法登录后台管理。请续费。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"/\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2009 huz.com.cn all Rights Reserved. 本产品资源均为 互站网络科技有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";

		private readonly string licenseMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body class=\"body2\">\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的全网商城系统未经官方授权，无法登录后台管理。请联系全网商城官方(www.huz.com.cn)购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"/\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2009 ShopEFX.com all Rights Reserved. 本产品资源均为 互站网络科技有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";

		protected HeadContainer HeadContainer1;

		protected PageTitle PageTitle1;

		protected HtmlForm form1;

		protected TextBox txtAdminName;

		protected TextBox txtAdminPassWord;

		protected HtmlGenericControl imgCode;

		protected TextBox txtCode;

		protected Button btnAdminLogin;

		protected HiddenField ErrorTimes;

		protected Literal lblStatus;

		private string ReferralLink
		{
			get
			{
				return this.ViewState["ReferralLink"] as string;
			}
			set
			{
				this.ViewState["ReferralLink"] = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			string key = "Management-Login";
			string script = string.Format(CultureInfo.InvariantCulture, "<script>if(top.frames.length > 0) top.location.href=\"{0}\";</script>", new object[1]
			{
				this.Page.Request.RawUrl
			});
			ClientScriptManager clientScript = this.Page.ClientScript;
			if (!clientScript.IsClientScriptBlockRegistered(key))
			{
				clientScript.RegisterClientScriptBlock(base.GetType(), key, script);
			}
			base.OnInit(e);
		}
        /// <summary>
        /// 检验验证码是否正确
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        private bool CheckVerifyCode(string verifyCode)
		{
			return HiContext.Current.CheckVerifyCode(verifyCode, "");
		}

		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdminLogin.Click += this.btnAdminLogin_Click;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.GetErrorTimes("username") >= 3)
			{
				this.imgCode.Visible = true;
			}
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				string verifyCode = base.Request["code"];
				string text = "";
				text = (this.CheckVerifyCode(verifyCode) ? "1" : "0");
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{ ");
				base.Response.Write($"\"flag\":\"{text}\"");
				base.Response.Write("}");
				base.Response.End();
			}
			if (!this.Page.IsPostBack)
			{
				Uri urlReferrer = this.Context.Request.UrlReferrer;
				if (urlReferrer != (Uri)null)
				{
					this.ReferralLink = urlReferrer.ToString();
				}
				this.txtAdminName.Focus();
				PageTitle.AddSiteNameTitle("后台登录");
			}
		}

		private void btnAdminLogin_Click(object sender, EventArgs e)
		{
			if (this.imgCode.Visible && !HiContext.Current.CheckVerifyCode(this.txtCode.Text.Trim(), ""))
			{
				this.ShowMessage("验证码不正确");
			}
			else
			{
				ManagerInfo managerInfo = ManagerHelper.ValidLogin(this.txtAdminName.Text, this.txtAdminPassWord.Text);
				if (managerInfo != null)
				{
					if (managerInfo.RoleId == -2)
					{
						SiteSettings siteSettings = HiContext.Current.SiteSettings;
						if (!siteSettings.OpenSupplier)
						{
							this.ShowMessage("未开启供应商模块!");
							return;
						}
					}
					Task task = Task.Factory.StartNew(delegate
					{
						this.DeleteTempFolder();
					});
					HttpCookie httpCookie = new HttpCookie("popTime");
					httpCookie.Value = this.LastActivyTime;
					HttpCookie httpCookie2 = new HttpCookie("popTimes");
					httpCookie2.Value = "0";
					HttpCookie httpCookie3 = new HttpCookie("popData");
					httpCookie3.Value = "";
					HttpContext.Current.Response.Cookies.Add(httpCookie);
					HttpContext.Current.Response.Cookies.Add(httpCookie2);
					HttpContext.Current.Response.Cookies.Add(httpCookie3);
					this.UpdateSiteSettings();
					string userData = string.Empty;
					SystemRoles systemRoles;
					if (managerInfo.RoleId == 0)
					{
						systemRoles = SystemRoles.SystemAdministrator;
						userData = systemRoles.ToString();
					}
					else if (managerInfo.RoleId == -1)
					{
						systemRoles = SystemRoles.StoreAdmin;
						userData = systemRoles.ToString();
					}
					else if (managerInfo.RoleId == -2)
					{
						systemRoles = SystemRoles.SupplierAdmin;
						userData = systemRoles.ToString();
					}
					else if (managerInfo.RoleId == -3)
					{
						systemRoles = SystemRoles.ShoppingGuider;
						userData = systemRoles.ToString();
					}
					FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, managerInfo.ManagerId.ToString(), DateTime.Now, DateTime.Now.AddMinutes(30.0), false, userData, "/");
					string value = FormsAuthentication.Encrypt(ticket);
					HttpCookie httpCookie4 = new HttpCookie(FormsAuthentication.FormsCookieName, value);
					httpCookie4.HttpOnly = true;
					HiContext.Current.Context.Response.Cookies.Add(httpCookie4);
					this.RemoveCache();
					string text = string.Empty;
					if (!string.IsNullOrEmpty(this.Page.Request.QueryString["returnUrl"]))
					{
						text = this.Page.Request.QueryString["returnUrl"];
					}
					if (text == null && this.ReferralLink != null && !string.IsNullOrEmpty(this.ReferralLink.Trim()))
					{
						text = this.ReferralLink;
					}
					if (managerInfo.RoleId == -1 || managerInfo.RoleId == -3)
					{
						if (managerInfo.Status == 0 && managerInfo.RoleId == -3)
						{
							this.ShowMessage("子帐号已被冻结,无法登录!");
							return;
						}
						StoresInfo storeById = StoresHelper.GetStoreById(managerInfo.StoreId);
						if (storeById == null || storeById.State == 0)
						{
							this.ShowMessage("门店已被关闭,无法登录!");
							return;
						}
						if (!string.IsNullOrEmpty(text) && text.ToLower().IndexOf("/depot/") >= 0)
						{
							this.Page.Response.Redirect(text, true);
						}
						else
						{
							base.Response.Write("<script language='javascript'>window.parent.location.href='/depot/default.html?v=" + HiContext.Current.Config.Version + "';</script>");
							base.Response.End();
						}
					}
					else if (managerInfo.RoleId == -2)
					{
						base.Response.Write("<script language='javascript'>window.parent.location.href='/Supplier/default.html?v=" + HiContext.Current.Config.Version + "';</script>");
						base.Response.End();
					}
					else
					{
						base.Response.Write("<script language='javascript'>window.parent.location.href='/Admin/default.html?v=" + HiContext.Current.Config.Version + "';</script>");
						base.Response.End();
					}
					if (!string.IsNullOrEmpty(text) && (text.ToLower().IndexOf("logout") >= 0 || text.ToLower().IndexOf("register") >= 0 || text.ToLower().IndexOf("votelist") >= 0 || text.ToLower().IndexOf("loginexit") >= 0))
					{
						text = null;
					}
					if (text != null)
					{
						this.Page.Response.Redirect(text, true);
					}
					else
					{
						this.Page.Response.Redirect("default.html?v=" + HiContext.Current.Config.Version, true);
					}
				}
				else
				{
					this.SetErrorTimes("username");
					this.ShowMessage("用户名或密码错误!");
				}
			}
		}

		private void DeleteTempFolder()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(base.Server.MapPath("~/Storage/master/temp/"));
			List<FileInfo> list = new List<FileInfo>();
			if (directoryInfo.Exists)
			{
				FileInfo[] files = directoryInfo.GetFiles();
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					if (fileInfo.LastWriteTime < DateTime.Parse(DateTime.Now.ToShortDateString()) && fileInfo.Exists)
					{
						fileInfo.Delete();
					}
				}
			}
		}

		private void ShowMessage(string msg)
		{
			this.lblStatus.Text = msg;
			this.lblStatus.Visible = true;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string domainName = Globals.DomainName;
			if (Globals.IsTestDomain)
			{
				base.Render(writer);
			}
			else
			{
				string text = "";
				string text2 = "";
				int num = 0;
				try
				{
					//text2 = APIHelper.PostData("http://ysc.huz.cn/valid.ashx", "action=serverstatus&product=2&version=5.8&host=" + Globals.DomainName);
					//text = text2.Replace("{\"serverstatus\":\"", "").Replace("\"}", "");
					//if (!string.IsNullOrEmpty(text))
					//{
					//	string[] array = text.Split(',');
					//	int.TryParse(array[0], out num);
					//	switch (num)
					//	{
					//	case 0:
					//		writer.Write(this.licenseMsg);
					//		return;
					//	case -1:
					//		writer.Write(this.noticeMsg);
					//		return;
					//	}
					//}
				}
				catch (Exception ex)
				{
					Login.WriteStatusLog(this.txtAdminName.Text, text, ex.Message);
				}
				base.Render(writer);
			}
		}

		private void UpdateSiteSettings()
		{
			if (!Globals.IsTestDomain)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				string text = "";
				string text2 = "";
				int num12 = 0;
				try
				{
					string text3 = "";
                    //text2 = APIHelper.PostData("http://ysc.huz.cn/valid.ashx", "action=serverstatus&product=2&version=5.8&host=" + Globals.DomainName);
                    //text = text2.Replace("{\"serverstatus\":\"", "").Replace("\"}", "");
                    text = "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,";
                    if (!string.IsNullOrEmpty(text))
					{
						string[] array = text.Split(',');
						if (array.Length >= 2)
						{
							int.TryParse(array[1], out num);
						}
						else
						{
							num = 1;
							text3 = "未获取到淘宝授权信息";
						}
						if (array.Length >= 3)
						{
							int.TryParse(array[2], out num5);
						}
						else
						{
							num5 = 1;
							text3 = "未获取到微商城授权信息";
						}
						if (array.Length >= 4)
						{
							int.TryParse(array[3], out num2);
						}
						else
						{
							num2 = 1;
							text3 = "未获取到APP版授权信息";
						}
						if (array.Length >= 5)
						{
							int.TryParse(array[4], out num4);
						}
						else
						{
							num4 = 1;
							text3 = "未获取触屏版授权";
						}
						if (array.Length >= 6)
						{
							int.TryParse(array[5], out num3);
						}
						else
						{
							num3 = 1;
							text3 = "未获取生活号（原支付宝服务窗）授权";
						}
						if (array.Length >= 7)
						{
							int.TryParse(array[6], out num6);
						}
						else
						{
							num6 = 1;
							text3 = "未获取到分销员授权";
						}
						if (array.Length >= 8)
						{
							int.TryParse(array[7], out num7);
						}
						else
						{
							num7 = 1;
							text3 = "未获取到多门店授权";
						}
						if (array.Length >= 9)
						{
							int.TryParse(array[8], out num8);
						}
						else
						{
							num7 = 1;
							text3 = "未获取到供应商授权";
						}
						if (array.Length >= 10)
						{
							int.TryParse(array[9], out num9);
						}
						else
						{
							num9 = 1;
							text3 = "未获取到微信小程序授权";
						}
						if (array.Length >= 11)
						{
							int.TryParse(array[10], out num10);
						}
						else
						{
							num10 = 1;
							text3 = "未获取到PC商城授权";
						}
						if (array.Length >= 12)
						{
							int.TryParse(array[11], out num11);
						}
						else
						{
							num11 = 1;
							text3 = "未获取到O2O小程序授权";
						}
					}
					else
					{
						num12 = 1;
						num3 = 1;
						num = 1;
						num2 = 1;
						num6 = 1;
						num5 = 1;
						num4 = 1;
						num7 = 1;
						num8 = 1;
						num9 = 1;
						num10 = 1;
						num11 = 1;
						text3 = "获取授权状态失败";
					}
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					if (masterSettings.ServiceStatus != num12 || masterSettings.OpenAliho != num3 || masterSettings.OpenMobbile != num2 || masterSettings.OpenVstore != num5 || masterSettings.OpenWap != num4 || masterSettings.OpenTaobao != num || masterSettings.OpenReferral != num6 || masterSettings.OpenMultStore != (num7 == 1) || masterSettings.OpenSupplier != (num8 == 1) || (masterSettings.OpenWxApplet ? 1 : 0) != ((num9 == 1) ? 1 : 0) || masterSettings.OpenPcShop != (num10 == 1) || masterSettings.OpenWxApplet != (num9 == 1) || masterSettings.OpenWXO2OApplet != (num11 == 1))
					{
						masterSettings.ServiceStatus = num12;
						masterSettings.OpenAliho = num3;
						masterSettings.OpenMobbile = num2;
						masterSettings.OpenVstore = num5;
						masterSettings.OpenTaobao = num;
						masterSettings.OpenWap = num4;
						masterSettings.OpenReferral = num6;
						masterSettings.OpenMultStore = (num7 == 1 && true);
						masterSettings.OpenSupplier = (num8 == 1 && true);
						masterSettings.OpenWxApplet = (num9 == 1 && true);
						masterSettings.OpenPcShop = (num10 == 1 || HiContext.IsInKeepOnRecordDate);
						masterSettings.OpenWXO2OApplet = (num11 == 1);
						SettingsManager.Save(masterSettings);
					}
				}
				catch (Exception ex)
				{
					NameValueCollection nameValueCollection = new NameValueCollection
					{
						base.Request.QueryString,
						base.Request.Form
					};
					nameValueCollection.Add("ServerStatus", text.ToNullString());
					Globals.WriteExceptionLog_Page(ex, nameValueCollection, "ServiceStatus");
				}
			}
		}

		public static void WriteStatusLog(string username, string servicestatus, string error)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("Time", DateTime.Now.ToString());
			nameValueCollection.Add("AdminName", username);
			nameValueCollection.Add("IpAddress", Globals.IPAddress);
			nameValueCollection.Add("Domain", Globals.DomainName);
			nameValueCollection.Add("ServiceStatus", servicestatus);
			Globals.AppendLog(nameValueCollection, error, "", "", "/ServiceStatus.txt");
		}

		private int GetErrorTimes(string username)
		{
			object obj = HiContext.Current.Context.Cache.Get(username);
			return (obj == null) ? 1 : ((int)obj);
		}

		private int SetErrorTimes(string username)
		{
			int num = this.GetErrorTimes(username) + 1;
			HiContext.Current.Context.Cache.Insert(username, num);
			return num;
		}

		private void RemoveCache()
		{
			HiContext.Current.Context.Cache.Remove("username");
		}
	}
}
