using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain.Menu;
using Newtonsoft.Json;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.VManageMenu)]
	public class ManageMenu : AdminPage
	{
		private ClientType clientType;

		protected HiddenField hidClientType;

		protected Button btnSubmit;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmit.Click += this.btnSubmit_Click;
			this.clientType = ClientType.VShop;
			if (!string.IsNullOrWhiteSpace(base.Request["client"]))
			{
				this.clientType = (ClientType)int.Parse(base.Request["client"]);
			}
			this.hidClientType.Value = this.clientType.GetHashCode().ToString();
		}

		private SingleButton BuildMenu(MenuInfo menu)
		{
			switch (menu.BindType)
			{
			case BindType.Key:
				return new SingleClickButton
				{
					name = menu.Name,
					key = menu.MenuId.ToString()
				};
			case BindType.Topic:
			case BindType.HomePage:
			case BindType.ProductCategory:
			case BindType.ShoppingCar:
			case BindType.OrderCenter:
			case BindType.MemberCard:
			case BindType.Url:
				return new SingleViewButton
				{
					name = menu.Name,
					url = menu.Url,
                    type=menu.Type
				};
			default:
				return new SingleClickButton
				{
					name = menu.Name,
					key = "None"
				};
			}
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsDemoSite)
			{
				this.ShowMsg("演示站点不允许修改微信自定义菜单", false);
			}
			else
			{
				IList<MenuInfo> initMenus = VShopHelper.GetInitMenus(ClientType.VShop);
				Hishop.Weixin.MP.Domain.Menu.Menu menu = new Hishop.Weixin.MP.Domain.Menu.Menu();
				foreach (MenuInfo item in initMenus)
				{
					if (item.Chilren == null || item.Chilren.Count == 0)
					{
						menu.menu.button.Add(this.BuildMenu(item));
					}
					else
					{
						SubMenu subMenu = new SubMenu
						{
							name = item.Name
						};
						foreach (MenuInfo item2 in item.Chilren)
						{
							subMenu.sub_button.Add(this.BuildMenu(item2));
						}
						menu.menu.button.Add(subMenu);
					}
				}
				string json = JsonConvert.SerializeObject(menu.menu);
				if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
				{
					base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='VServerConfig.aspx'</script>");
				}
				else
				{
					string text = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, false);
					string sign = text;
					try
					{
						if (string.IsNullOrEmpty(text))
						{
							this.ShowMsg("操作失败!您的服务号配置可能存在问题，请您先检查配置！", false);
						}
						else
						{
							string text2 = MenuApi.CreateMenus(text, json);
							if (text2.Contains("\"errmsg\":\"ok\""))
							{
								this.ShowMsg("成功的把自定义菜单保存到了微信", true);
							}
							else
							{
								Globals.AppendLog(text2, sign, "", "SaveMenu");
								this.ShowMsg("操作失败!服务号配置信息错误或没有微信自定义菜单权限，请检查配置信息以及菜单的长度。", false);
							}
						}
					}
					catch (Exception ex)
					{
						base.Response.Write(ex.Message + "---" + text + "---" + masterSettings.WeixinAppId + "---" + masterSettings.WeixinAppSecret);
						base.Response.End();
					}
				}
			}
		}
	}
}
