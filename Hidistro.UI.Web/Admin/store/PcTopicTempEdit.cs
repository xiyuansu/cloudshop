using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.store
{
	[PrivilegeCheck(Privilege.DefineTopics)]
	public class PcTopicTempEdit : Page
	{
		protected HtmlForm form1;

		protected ProductCategoriesDropDownList dropCategories;

		protected HtmlInputHidden j_pageID;

		protected HtmlInputHidden topicid;

		protected HiddenField hidUploadImages;

		protected ImageList ImageList;

		protected Literal La_script;

		protected HiddenField hidOpenMultStore;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.CheckAuth();
			this.CheckPageAccess();
			if (!string.IsNullOrEmpty(this.Page.Request["TopicId"]))
			{
				this.topicid.Value = this.Page.Request["TopicId"];
			}
			if (!base.IsPostBack)
			{
				this.hidOpenMultStore.Value = (SettingsManager.GetMasterSettings().OpenMultStore ? "1" : "0");
				this.dropCategories.DataBind();
			}
		}

		private void CheckPageAccess()
		{
			if (HiContext.Current.SiteSettings.OpenPcShop)
			{
				goto IL_0019;
			}
			goto IL_0019;
			IL_0019:
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
			string str = "openpcshop";
			string domainName = Globals.DomainName;
			try
			{
				if (!Globals.IsTestDomain)
				{
					//string postResult = Globals.GetPostResult("http://ysc.huz.cn/valid.ashx", "action=" + str + "&product=2&host=" + domainName);
					//int num = Convert.ToInt32(postResult.Replace("{\"state\":\"", "").Replace("\"}", ""));
					//if (num != 1)
					//{
					//	this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=抱歉，您暂未开通此服务！"), true);
					//}
				}
			}
			catch
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx?errormsg=抱歉，您暂未开通此服务！"), true);
			}
		}
	}
}
