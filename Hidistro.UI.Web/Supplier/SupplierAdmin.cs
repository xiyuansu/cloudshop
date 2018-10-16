using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Supplier;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;

namespace Hidistro.UI.Web.Supplier
{
	public class SupplierAdmin : IHttpHandler
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
			text = ((!string.IsNullOrEmpty(text)) ? text.ToLower() : "");
			string text2 = text;
			switch (text2)
			{
			default:
				if (text2 == "istradepassword")
				{
					this.IsTradePassword(context);
				}
				break;
			case "getneworders":
				this.ProcessNewOrders(context);
				break;
			case "getadminopenid":
				this.GetAdminOpenId(context);
				break;
			case "isloginpassword":
				this.IsLoginPassword(context);
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

		public void IsLoginPassword(HttpContext context)
		{
			string text = context.Request["txtoldPassword"];
			string text2 = "";
			if (string.IsNullOrEmpty(text))
			{
				text2 = "{\"success\":\"false\",\"msg\":\"参数错误！\"}";
				this.ResponseEnd(text2);
			}
			ManagerInfo manager = HiContext.Current.Manager;
			string strA = Users.EncodePassword(text, manager.PasswordSalt);
			text2 = ((string.Compare(strA, manager.Password) == 0) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"输入的当前登录密码与原始登录密码不一致，请正确输入！\"}");
			this.ResponseEnd(text2);
		}

		public void IsTradePassword(HttpContext context)
		{
			string text = context.Request["txtoldTradePassword"];
			string text2 = "";
			if (string.IsNullOrEmpty(text))
			{
				text2 = "{\"success\":\"false\",\"msg\":\"参数错误！\"}";
				this.ResponseEnd(text2);
			}
			ManagerInfo manager = HiContext.Current.Manager;
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(manager.StoreId);
			string strA = Users.EncodePassword(text, supplierById.TradePasswordSalt);
			text2 = ((string.Compare(strA, supplierById.TradePassword) == 0) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"输入的当前交易密码与原始交易密码不一致，请正确输入！\"}");
			this.ResponseEnd(text2);
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
			recentlyOrderStatic = SalesHelper.GetNewlyOrdersCountAndPayCount(lastTime, 0, HiContext.Current.Manager.StoreId);
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

		protected void ResponseEnd(string msg)
		{
			HttpContext.Current.Response.Write(msg);
			HttpContext.Current.Response.End();
		}
	}
}
