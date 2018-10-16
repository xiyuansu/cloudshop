using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
	public class Admin : IHttpHandler
	{
		private string msg = "";

		private IDictionary<string, string> jsondict = new Dictionary<string, string>();

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			ManagerInfo manager = HiContext.Current.Manager;
			if (manager == null)
			{
				this.jsondict.Add("msg", "权限不足");
				this.WriteJson(context, 0);
			}
			string text = context.Request["action"];
			text = (string.IsNullOrEmpty(text) ? "" : text.ToLower());
			string text2 = text;
			switch (text2)
			{
			default:
				if (text2 == "getadminopenid")
				{
					this.GetAdminOpenId(context);
				}
				break;
			case "getneworders":
				this.ProcessNewOrders(context);
				break;
			case "delextendcategory":
				this.delExtendCategory(context);
				break;
			case "delcertfile":
				this.DelCertFile(context);
				break;
			}
		}

		public void GetAdminOpenId(HttpContext context)
		{
			string text = "";
			string str = "";
			if (HiContext.Current.Manager != null)
			{
				str = HiContext.Current.Manager.ManagerId.ToString();
			}
			HttpContext.Current.Response.Clear();
			int num = 0;
			int num2 = 0;
			while (num != 1)
			{
				num2++;
				Thread.Sleep(1000);
				text = ((HiCache.Get(str + "_OpenId") == null) ? "" : HiCache.Get(str + "_OpenId").ToString());
				num = ((!string.IsNullOrEmpty(text)) ? 1 : 0);
				if (num2 == 25)
				{
					break;
				}
			}
			StringBuilder stringBuilder = new StringBuilder("{");
			stringBuilder.AppendFormat("\"Status\":\"{0}\",", num);
			stringBuilder.AppendFormat("\"OpenId\":\"{0}\"", text);
			stringBuilder.Append("}");
			if (num == 1)
			{
				HiCache.Remove(str + "_OpenId");
			}
			HttpContext.Current.Response.ContentType = "application/json";
			HttpContext.Current.Response.Write(stringBuilder.ToString());
			HttpContext.Current.Response.End();
		}

		public void DelCertFile(HttpContext context)
		{
			this.jsondict.Clear();
			string text = context.Request["FilePath"];
			int num = text.LastIndexOf(".");
			if (num == -1)
			{
				this.jsondict.Add("msg", "错误的文件格式");
				this.WriteJson(context, 0);
			}
			else
			{
				string text2 = text.Substring(num).ToUpper();
				if (!text2.Contains(".CRT") && !text2.Contains(".CER") && !text2.Contains(".P12") && !text2.Contains(".P7B") && !text2.Contains(".P7C") && !text2.Contains(".SPC") && !text2.Contains(".KEY") && !text2.Contains(".DER") && !text2.Contains(".PEM") && !text2.Contains(".PFX"))
				{
					this.jsondict.Add("msg", "错误的文件格式");
					this.WriteJson(context, 0);
				}
			}
			this.jsondict.Clear();
			if (File.Exists(Globals.GetphysicsPath(text)))
			{
				File.Delete(Globals.GetphysicsPath(text));
				this.jsondict.Add("msg", "删除成功");
				this.WriteJson(context, 1);
			}
			else
			{
				this.jsondict.Add("msg", "文件不存在");
				this.WriteJson(context, 0);
			}
		}

		public void delExtendCategory(HttpContext context)
		{
			this.jsondict.Clear();
			int num = 0;
			int.TryParse(context.Request["productId"], out num);
			int num2 = 0;
			int.TryParse(context.Request["extendIndex"], out num2);
			if (num > 0 && num2 > 0)
			{
				if (CatalogHelper.SetProductExtendCategory(num, null, num2))
				{
					this.jsondict.Add("msg", "更新成功");
					this.WriteJson(context, 1);
				}
				else
				{
					this.jsondict.Add("msg", "更新失败");
					this.WriteJson(context, 0);
				}
			}
			else
			{
				this.jsondict.Add("msg", "参数错误");
				this.WriteJson(context, 0);
			}
		}

		public void ProcessNewOrders(HttpContext context)
		{
			DateTime lastTime = DateTime.Now;
			string text = context.Request["lastTime"];
			DateTime now;
			if (!DateTime.TryParse(context.Request["lastTime"], out lastTime))
			{
				now = DateTime.Now;
				lastTime = now.AddHours(-1.0);
			}
			RecentlyOrderStatic recentlyOrderStatic = new RecentlyOrderStatic();
			recentlyOrderStatic.HasOrderSatic = false;
			recentlyOrderStatic = SalesHelper.GetNewlyOrdersCountAndPayCount(lastTime, 0, 0);
			this.jsondict.Clear();
			IDictionary<string, string> dictionary = this.jsondict;
			int num = recentlyOrderStatic.OrdersCount;
			dictionary.Add("OrdersCount", num.ToString());
			IDictionary<string, string> dictionary2 = this.jsondict;
			num = recentlyOrderStatic.PayCount;
			dictionary2.Add("PayCount", num.ToString());
			IDictionary<string, string> dictionary3 = this.jsondict;
			num = recentlyOrderStatic.RefundOrderCount;
			dictionary3.Add("RefundOrderCount", num.ToString());
			IDictionary<string, string> dictionary4 = this.jsondict;
			num = recentlyOrderStatic.ReplacementOrderCount;
			dictionary4.Add("ReplacementOrderCount", num.ToString());
			IDictionary<string, string> dictionary5 = this.jsondict;
			num = recentlyOrderStatic.ReturnsOrderCount;
			dictionary5.Add("ReturnsOrderCount", num.ToString());
			IDictionary<string, string> dictionary6 = this.jsondict;
			now = DateTime.Now;
			dictionary6.Add("lastTime", now.ToString("yyyy/MM/dd HH:mm:ss"));
			if (recentlyOrderStatic.HasOrderSatic)
			{
				this.WriteJson(context, 1);
			}
			else
			{
				this.WriteJson(context, 0);
			}
		}

		public void WriteJson(HttpContext context, int status)
		{
			context.Response.ContentType = "application/json";
			StringBuilder stringBuilder = new StringBuilder("{");
			stringBuilder.Append("\"status\":\"" + status + "\"");
			if (this.jsondict.Count > 0)
			{
				foreach (string key in this.jsondict.Keys)
				{
					stringBuilder.AppendFormat(",\"{0}\":\"{1}\"", key, this.jsondict[key]);
				}
			}
			stringBuilder.Append("}");
			context.Response.Write(stringBuilder.ToString());
			context.Response.End();
		}
	}
}
