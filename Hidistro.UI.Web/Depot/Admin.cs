using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Sales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;

namespace Hidistro.UI.Web.Depot
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
			string a = text;
			if (!(a == "getneworders"))
			{
				if (!(a == "delextendcategory"))
				{
					if (a == "getadminopenid")
					{
						this.GetAdminOpenId(context);
					}
				}
				else
				{
					this.delExtendCategory(context);
				}
			}
			else
			{
				this.ProcessNewOrders(context);
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
			int storeId = HiContext.Current.Manager.StoreId;
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
			int num = 0;
			while (!recentlyOrderStatic.HasOrderSatic)
			{
				recentlyOrderStatic = SalesHelper.GetNewlyOrdersCountAndPayCount(lastTime, storeId, 0);
				Thread.Sleep(1000);
				num++;
				if (num == 25)
				{
					break;
				}
			}
			this.jsondict.Clear();
			IDictionary<string, string> dictionary = this.jsondict;
			int num2 = recentlyOrderStatic.OrdersCount;
			dictionary.Add("OrdersCount", num2.ToString());
			IDictionary<string, string> dictionary2 = this.jsondict;
			num2 = recentlyOrderStatic.PayCount;
			dictionary2.Add("PayCount", num2.ToString());
			IDictionary<string, string> dictionary3 = this.jsondict;
			num2 = recentlyOrderStatic.RefundOrderCount;
			dictionary3.Add("RefundOrderCount", num2.ToString());
			IDictionary<string, string> dictionary4 = this.jsondict;
			num2 = recentlyOrderStatic.ReplacementOrderCount;
			dictionary4.Add("ReplacementOrderCount", num2.ToString());
			IDictionary<string, string> dictionary5 = this.jsondict;
			num2 = recentlyOrderStatic.ReturnsOrderCount;
			dictionary5.Add("ReturnsOrderCount", num2.ToString());
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
