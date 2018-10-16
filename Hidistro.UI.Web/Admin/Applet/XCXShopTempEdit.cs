using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Ionic.Zlib;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Applet
{
	[PrivilegeCheck(Privilege.AppletProductSetting)]
	public class XCXShopTempEdit : Page
	{
		public string scriptSrc = "/Templates/vshop/";

		protected HtmlForm thisForm;

		protected ProductCategoriesDropDownList dropCategories;

		protected HtmlInputHidden j_pageID;

		protected ImageListView ImageListView;

		protected Literal La_script;

		protected HiddenField hidOpenMultStore;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckAuth();
			this.CheckPageAccess();
			if (!base.IsPostBack)
			{
				this.scriptSrc += "script/head.js";
				this.dropCategories.DataBind();
				this.hidOpenMultStore.Value = (SettingsManager.GetMasterSettings().OpenMultStore ? "1" : "0");
			}
		}

		private void CheckPageAccess()
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null || manager.RoleId == -1 || manager.RoleId == -3)
			{
				base.Response.Write("<script language='javascript'>window.parent.location.href='/Admin/Login.aspx';</script>");
				base.Response.End();
			}
			else
			{
				AdministerCheckAttribute administerCheckAttribute = (AdministerCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(AdministerCheckAttribute));
				if (administerCheckAttribute != null && administerCheckAttribute.AdministratorOnly && manager.RoleId != 0)
				{
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx"));
				}
				PrivilegeCheckAttribute privilegeCheckAttribute = (PrivilegeCheckAttribute)Attribute.GetCustomAttribute(base.GetType(), typeof(PrivilegeCheckAttribute));
				if (privilegeCheckAttribute != null && !ManagerHelper.HasPrivilege((int)privilegeCheckAttribute.Privilege, manager))
				{
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilegeCheckAttribute.Privilege.ToString()));
				}
			}
		}

		protected void CheckAuth()
		{
			if (!Globals.IsTestDomain)
			{
				string domainName = Globals.DomainName;
				string str = "openwxapplet";
				string text = base.Request.FilePath.ToLower();
				try
				{
					//string text2 = this.PostData("http://ysc.huz.cn/valid.ashx", "action=" + str + "&product=2&host=" + domainName);
					//int num = Convert.ToInt32(text2.Replace("{\"state\":\"", "").Replace("\"}", ""));
					//if (num != 1)
					//{
					//	this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=抱歉，您暂未开通此服务！"), true);
					//}
				}
				catch
				{
					this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=抱歉，您暂未开通此服务！"), true);
				}
			}
		}

		public string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				Uri requestUri = new Uri(url);
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
				Encoding uTF = Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = bytes.Length;
				using (Stream stream = httpWebRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream2 = httpWebResponse.GetResponseStream())
					{
						Encoding uTF2 = Encoding.UTF8;
						Stream stream3 = stream2;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream3 = new GZipStream(stream2, CompressionMode.Decompress);
						}
						else if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
						{
							stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
						}
						using (StreamReader streamReader = new StreamReader(stream3, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = $"获取信息错误：{ex.Message}";
			}
			return result;
		}
	}
}
