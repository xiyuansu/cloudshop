using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.Supplier;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Supplier
{
	public class SupplierHandler : IHttpHandler
	{
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
			string text2 = text;
			switch (text2)
			{
			default:
				if (text2 == "EmptySupplierTransactionPass")
				{
					this.EmptySupplierTransactionPass(context);
				}
				break;
			case "CheckSupplierName":
				this.CheckSupplierName(context);
				break;
			case "CheckUserName":
				this.CheckUserName(context);
				break;
			case "EditSupplierStatus":
				this.EditSupplierStatus(context);
				break;
			case "ResetSupplierPass":
				this.ResetSupplierPass(context);
				break;
			}
		}

		private void CheckUserName(HttpContext context)
		{
			string userName = Globals.StripAllTags(context.Request["UserName"]);
			string empty = string.Empty;
			empty = ((ManagerHelper.FindManagerByUsername(userName) == null) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"用户名已存在,请重新输入！\"}");
			this.ResponseEnd(empty);
		}

		private void CheckSupplierName(HttpContext context)
		{
			int supplierId = context.Request["supplierId"].ToInt(0);
			string supplierName = Globals.StripAllTags(context.Request["supplierName"]);
			string empty = string.Empty;
			empty = ((!SupplierHelper.ExistSupplierName(supplierId, supplierName)) ? "{\"success\":\"true\",\"msg\":\"\"}" : "{\"success\":\"false\",\"msg\":\"供应商名称已经存在,请重新输入！\"}");
			this.ResponseEnd(empty);
		}

		private void EditSupplierStatus(HttpContext context)
		{
			int num = context.Request["supplierId"].ToInt(0);
			int num2 = context.Request["statusvalue"].ToInt(0);
			num2 = ((num2 == 1) ? num2 : 2);
			string empty = string.Empty;
			if (num <= 0)
			{
				empty = "{\"success\":\"false\",\"msg\":\"参数错误，请勿操作！\"}";
				this.ResponseEnd(empty);
			}
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(num);
			if (supplierById == null || supplierById.SupplierId <= 0)
			{
				empty = "{\"success\":\"false\",\"msg\":\"供应商已不存在,请重新点击列表进行操作！！\"}";
				this.ResponseEnd(empty);
			}
			if (supplierById.Status.Equals(num2))
			{
				empty = "{\"success\":\"false\",\"msg\":\"编辑状态就是当前状态，不需要重新修改！！\"}";
				this.ResponseEnd(empty);
			}
			supplierById.Status = num2;
			if (num2 == 1)
			{
				empty = ((SupplierHelper.UpdateSupplier_Recover(num) <= 0) ? "{\"success\":\"false\",\"msg\":\"供应商状态“恢复”失败！\"}" : "{\"success\":\"true\",\"msg\":\"供应商状态“恢复”成功！\"}");
			}
			else
			{
				int num3 = SupplierHelper.UpdateSupplier_Frozen(num);
				empty = ((num3 <= 0) ? "{\"success\":\"false\",\"msg\":\"供应商状态“冻结”失败！\"}" : "{\"success\":\"true\",\"msg\":\"供应商状态“冻结”成功！\"}");
			}
			this.ResponseEnd(empty);
		}

		private void ResetSupplierPass(HttpContext context)
		{
			int num = context.Request["managerId"].ToInt(0);
			string empty = string.Empty;
			if (num <= 0)
			{
				empty = "{\"success\":\"false\",\"msg\":\"参数错误，请勿操作！\"}";
				this.ResponseEnd(empty);
			}
			ManagerInfo manager = Users.GetManager(num);
			if (manager == null || manager.ManagerId <= 0)
			{
				empty = "{\"success\":\"false\",\"msg\":\"供应商用户已不存在,请重新加载列表进行操作！！\"}";
				this.ResponseEnd(empty);
			}
			string text = Globals.RndStr(128, true);
			string text2 = Globals.RndStr(6);
			manager.Password = Users.EncodePassword(text2, text);
			manager.PasswordSalt = text;
			empty = ((!ManagerHelper.Update(manager)) ? "{\"success\":\"false\",\"msg\":\"重置后登录密码失败！！\"}" : ("{\"success\":\"true\",\"msg\":\"重置后登录密码：" + text2 + "\"}"));
			this.ResponseEnd(empty);
		}

		private void EmptySupplierTransactionPass(HttpContext context)
		{
			int num = context.Request["supplierId"].ToInt(0);
			string empty = string.Empty;
			if (num <= 0)
			{
				empty = "{\"success\":\"false\",\"msg\":\"参数错误，请勿操作！\"}";
				this.ResponseEnd(empty);
			}
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(num);
			if (supplierById == null || supplierById.SupplierId <= 0)
			{
				empty = "{\"success\":\"false\",\"msg\":\"供应商已不存在,请重新加载列表进行操作！！\"}";
				this.ResponseEnd(empty);
			}
			supplierById.TradePassword = "";
			supplierById.TradePasswordSalt = "";
			empty = ((!SupplierHelper.UpdateSupplier(supplierById)) ? "{\"success\":\"false\",\"msg\":\"清空交易密码失败！！\"}" : "{\"success\":\"true\",\"msg\":\"清空成功，请去供应商后台重新设置交易密码！\"}");
			this.ResponseEnd(empty);
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
