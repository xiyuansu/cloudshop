using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.App_Code;
using Hishop.Alipay.OpenHome;
using Hishop.Alipay.OpenHome.AlipayOHException;
using Hishop.Alipay.OpenHome.Model;
using Hishop.Alipay.OpenHome.Request;
using Hishop.Alipay.OpenHome.Response;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliOH
{
	[PrivilegeCheck(Privilege.AliohManageMenu)]
	public class ManageMenu : AdminPage
	{
		private NameValueCollection pageparam = null;

		protected System.Web.UI.WebControls.Button btnSubmit;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.pageparam = new NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.btnSubmit.Click += this.btnSubmit_Click;
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(masterSettings.AliOHAppId))
			{
				base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='AliOHServerConfig.aspx'</script>");
			}
			else
			{
				IList<MenuInfo> initMenus = VShopHelper.GetInitMenus(ClientType.AliOH);
				List<Hishop.Alipay.OpenHome.Model.Button> list = new List<Hishop.Alipay.OpenHome.Model.Button>();
				foreach (MenuInfo item in initMenus)
				{
					if (item.Chilren == null || item.Chilren.Count == 0)
					{
						list.Add(new Hishop.Alipay.OpenHome.Model.Button
						{
							name = item.Name,
							actionParam = (string.IsNullOrEmpty(item.Url) ? "http://javasript:;" : item.Url),
							actionType = item.Type
						});
					}
					else
					{
						Hishop.Alipay.OpenHome.Model.Button button = new Hishop.Alipay.OpenHome.Model.Button
						{
							name = item.Name
						};
						List<Hishop.Alipay.OpenHome.Model.Button> list2 = new List<Hishop.Alipay.OpenHome.Model.Button>();
						foreach (MenuInfo item2 in item.Chilren)
						{
							list2.Add(new Hishop.Alipay.OpenHome.Model.Button
							{
								name = item2.Name,
								actionParam = (string.IsNullOrEmpty(item2.Url) ? "http://javasript:;" : item2.Url),
								actionType = item2.Type
							});
						}
						button.subButton = list2;
						list.Add(button);
					}
				}
				Hishop.Alipay.OpenHome.Model.Menu menu = new Hishop.Alipay.OpenHome.Model.Menu
				{
					button = list
				};
				AlipayOHClient alipayOHClient = AliOHClientHelper.Instance(base.Server.MapPath("~/"));
				bool flag = false;
				try
				{
					AddMenuRequest request = new AddMenuRequest(menu);
					MenuAddResponse menuAddResponse = alipayOHClient.Execute<MenuAddResponse>(request);
					this.ShowMsg("保存到生活号（原支付宝服务窗）成功！", true);
					flag = true;
				}
				catch (AliResponseException ex)
				{
					Globals.WriteExceptionLog_Page(ex, this.pageparam, "AliohMenuSave");
				}
				catch (Exception ex2)
				{
					this.ShowMsg("保存到生活号失败，失败原因：" + ex2.Message, false);
					flag = true;
					Globals.WriteExceptionLog_Page(ex2, this.pageparam, "AliohMenuSave");
				}
				if (!flag)
				{
					try
					{
						UpdateMenuRequest request2 = new UpdateMenuRequest(menu);
						MenuUpdateResponse menuUpdateResponse = alipayOHClient.Execute<MenuUpdateResponse>(request2);
						this.ShowMsg("保存到生活号成功！", true);
					}
					catch (Exception ex3)
					{
						this.ShowMsg("保存到生活号失败，失败原因：" + ex3.Message, false);
						Globals.WriteExceptionLog_Page(ex3, this.pageparam, "AliohMenuSave");
					}
				}
			}
		}
	}
}
