using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Store;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class MenuProcess : IHttpHandler, IRequiresSessionState
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			string text = context.Request["action"];
			switch (text)
			{
			case "gettopmenus":
				this.GetTopMenus(context);
				break;
			case "addmenu":
				this.AddMenus(context);
				break;
			case "editmenu":
				this.EditMenus(context);
				break;
			case "updatename":
				this.updatename(context);
				break;
			case "getmenu":
				this.GetMenu(context);
				break;
			case "delmenu":
				this.delmenu(context);
				break;
			case "setenable":
				this.setenable(context);
				break;
			}
		}

		public void delmenu(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = "{\"status\":\"1\"}";
			int menuId = 0;
			if (!int.TryParse(context.Request["MenuId"], out menuId))
			{
				s = "{\"status\":\"1\"}";
			}
			else
			{
				if (ShopMenuHelper.DeleteMenu(menuId))
				{
					s = "{\"status\":\"0\"}";
				}
				context.Response.Write(s);
			}
		}

		public void GetMenu(HttpContext context)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			context.Response.Charset = "utf-8";
			string str = "{";
			ShopMenuInfo shopMenuInfo = new ShopMenuInfo();
			int menuId = 0;
			if (!int.TryParse(context.Request["MenuId"], out menuId))
			{
				str = "\"status\":\"1\"";
			}
			else
			{
				shopMenuInfo = ShopMenuHelper.GetMenu(menuId);
				if (shopMenuInfo != null)
				{
					str += "\"status\":\"0\",\"data\":[";
					str = str + "{\"menuid\": \"" + shopMenuInfo.MenuId + "\",";
					str = str + "\"type\": \"" + shopMenuInfo.Type + "\",";
					str = str + "\"name\": \"" + shopMenuInfo.Name + "\",";
					str = str + "\"shopmenupic\": \"" + shopMenuInfo.ShopMenuPic + "\",";
					str = str + "\"content\": \"" + shopMenuInfo.Content + "\"}";
					str += "]";
				}
				str += "}";
				context.Response.Write(str);
			}
		}

		public void EditMenus(HttpContext context)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			context.Response.Charset = "utf-8";
			string s = "{\"status\":\"1\"}";
			ShopMenuInfo shopMenuInfo = new ShopMenuInfo();
			shopMenuInfo.Content = context.Request["Content"];
			shopMenuInfo.Name = context.Request["Name"];
			shopMenuInfo.Type = context.Request["Type"];
			if (!string.IsNullOrEmpty(context.Request["ParentMenuId"]))
			{
				shopMenuInfo.ParentMenuId = int.Parse(context.Request["ParentMenuId"]);
			}
			else
			{
				shopMenuInfo.ParentMenuId = 0;
			}
			int menuId = 0;
			if (!int.TryParse(context.Request["MenuId"], out menuId))
			{
				s = "{\"status\":\"1\"}";
			}
			else
			{
				shopMenuInfo.MenuId = menuId;
				shopMenuInfo.ShopMenuPic = this.SaveMenuPic(context.Request["ShopMenuPic"]);
				if (ShopMenuHelper.UpdateMenu(shopMenuInfo))
				{
					s = "{\"status\":\"0\"}";
				}
				context.Response.Write(s);
			}
		}

		public void updatename(HttpContext context)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			context.Response.Charset = "utf-8";
			string s = "{\"status\":\"1\"}";
			int num = 0;
			if (!int.TryParse(context.Request["MenuId"], out num))
			{
				s = "{\"status\":\"1\"}";
			}
			else
			{
				if (num > 0)
				{
					ShopMenuInfo menu = ShopMenuHelper.GetMenu(num);
					menu.Name = context.Request["Name"];
					menu.MenuId = num;
					if (ShopMenuHelper.UpdateMenu(menu))
					{
						s = "{\"status\":\"0\"}";
					}
				}
				context.Response.Write(s);
			}
		}

		public void AddMenus(HttpContext context)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			context.Response.Charset = "utf-8";
			string s = "{\"status\":\"1\"}";
			ShopMenuInfo shopMenuInfo = new ShopMenuInfo();
			shopMenuInfo.Content = context.Request["Content"].Trim();
			shopMenuInfo.Name = Globals.UrlDecode(context.Request["Name"].Trim());
			if (context.Request["ParentMenuId"] != null)
			{
				shopMenuInfo.ParentMenuId = ((!(context.Request["ParentMenuId"] == "")) ? int.Parse(context.Request["ParentMenuId"]) : 0);
			}
			else
			{
				shopMenuInfo.ParentMenuId = 0;
			}
			shopMenuInfo.Type = context.Request["Type"];
			shopMenuInfo.ClientType = context.Request["clientType"].ToInt(0);
			shopMenuInfo.ShopMenuPic = this.SaveMenuPic(context.Request["ShopMenuPic"]);
			if (ShopMenuHelper.CanAddMenu(shopMenuInfo.ParentMenuId, 0))
			{
				if (ShopMenuHelper.SaveMenu(shopMenuInfo))
				{
					s = "{\"status\":\"0\"}";
				}
			}
			else
			{
				s = "{\"status\":\"2\"}";
			}
			context.Response.Write(s);
		}

		private string SaveMenuPic(string shopMenuPic)
		{
			string str = Globals.GetStoragePath() + "/temp/";
			string text = HttpContext.Current.Server.MapPath(Globals.GetStoragePath() + "/menu/");
			if (!Globals.PathExist(text, false))
			{
				Globals.CreatePath(text);
			}
			if (shopMenuPic.Trim().Length == 0)
			{
				return string.Empty;
			}
			shopMenuPic = shopMenuPic.Replace("//", "/");
			string text2 = (shopMenuPic.Split('/').Length == 6) ? shopMenuPic.Split('/')[5] : shopMenuPic.Split('/')[4];
			if (File.Exists(text + text2))
			{
				return Globals.GetStoragePath() + "/menu/" + text2;
			}
			if (File.Exists(HttpContext.Current.Server.MapPath(shopMenuPic)))
			{
				File.Copy(HttpContext.Current.Server.MapPath(shopMenuPic), text + text2);
			}
			string path = HttpContext.Current.Server.MapPath(str + text2);
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			return Globals.GetStoragePath() + "/menu/" + text2;
		}

		public void GetTopMenus(HttpContext context)
		{
			context.Response.ContentType = "application/json;charset=utf-8";
			string str = "{";
			IList<ShopMenuInfo> topMenus = ShopMenuHelper.GetTopMenus(0);
			if (topMenus.Count <= 0)
			{
				str += "\"status\":\"-1\"";
			}
			else
			{
				str += "\"status\":\"0\",\"shopmenustyle\":\"\",\"enableshopmenu\":\"True\",\"data\":[";
				foreach (ShopMenuInfo item in topMenus)
				{
					IList<ShopMenuInfo> menusByParentId = ShopMenuHelper.GetMenusByParentId(item.MenuId, 0);
					str = str + "{\"menuid\": \"" + item.MenuId + "\",";
					str += "\"childdata\":[";
					if (menusByParentId.Count > 0)
					{
						foreach (ShopMenuInfo item2 in menusByParentId)
						{
							str = str + "{\"menuid\": \"" + item2.MenuId + "\",";
							str = str + "\"parentmenuid\": \"" + item2.ParentMenuId + "\",";
							str = str + "\"type\": \"" + item2.Type + "\",";
							str = str + "\"name\": \"" + item2.Name + "\",";
							str = str + "\"content\": \"" + item2.Content + "\"},";
						}
						str = str.Substring(0, str.Length - 1);
					}
					str += "],";
					str = str + "\"type\": \"" + item.Type + "\",";
					str = str + "\"name\": \"" + item.Name + "\",";
					str = str + "\"shopmenupic\": \"" + item.ShopMenuPic + "\",";
					str = str + "\"content\": \"" + item.Content + "\"},";
				}
				str = str.Substring(0, str.Length - 1);
				str += "]";
				str += "}";
				context.Response.Write(str);
			}
		}

		public void setenable(HttpContext context)
		{
			string s = "{\"status\":\"1\"}";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.EnableShopMenu = bool.Parse(context.Request["enable"]);
			SettingsManager.Save(masterSettings);
			context.Response.Write(s);
		}
	}
}
