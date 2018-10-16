using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	[PrivilegeCheck(Privilege.ExpressComputerpes)]
	public class LogisticsCompany : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "addandupdate")
				{
					this.AddAndUpdate(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void GetList(HttpContext context)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			text = base.GetParameter(context, "companyname", false);
			text2 = base.GetParameter(context, "kuaidi100Code", false);
			text3 = base.GetParameter(context, "taobaoCode", false);
			text4 = base.GetParameter(context, "jdCode", false);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(text, text2, text3, text4);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(string companyname, string kuaidi100Code, string taobaoCode, string jdCode)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataTable expressTable = ExpressHelper.GetExpressTable();
			if (!string.IsNullOrEmpty(companyname))
			{
				expressTable.DefaultView.RowFilter = "Name like '%" + companyname + "%'";
			}
			if (!string.IsNullOrEmpty(kuaidi100Code))
			{
				expressTable.DefaultView.RowFilter = "Kuaidi100Code like '%" + kuaidi100Code + "%'";
			}
			if (!string.IsNullOrEmpty(taobaoCode))
			{
				expressTable.DefaultView.RowFilter = "TaobaoCode like '%" + taobaoCode + "%'";
			}
			if (!string.IsNullOrEmpty(jdCode))
			{
				expressTable.DefaultView.RowFilter = "JDCode like '%" + jdCode + "%'";
			}
			DataTable dataTable = expressTable.DefaultView.ToTable();
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(dataTable);
			dataGridViewModel.total = dataTable.Rows.Count;
			return dataGridViewModel;
		}

		private void AddAndUpdate(HttpContext context)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			text = base.GetParameter(context, "companyname", false);
			text2 = base.GetParameter(context, "kuaidi100Code", false);
			text3 = base.GetParameter(context, "taobaoCode", false);
			text4 = base.GetParameter(context, "jdCode", false);
			if (string.IsNullOrEmpty(text.Trim()))
			{
				throw new HidistroAshxException("物流名称不允许为空！");
			}
			if (string.IsNullOrEmpty(text2.Trim()))
			{
				throw new HidistroAshxException("快递鸟Code不允许为空！");
			}
			if (string.IsNullOrEmpty(text3.Trim()))
			{
				throw new HidistroAshxException("淘宝Code不允许为空！");
			}
			if (string.IsNullOrEmpty(text4.Trim()))
			{
				throw new HidistroAshxException("京东Code不允许为空！");
			}
			string parameter = base.GetParameter(context, "hdcomputers", false);
			if (!string.IsNullOrEmpty(parameter.Trim()))
			{
				ExpressHelper.UpdateExpress(Globals.HtmlEncode(parameter), Globals.HtmlEncode(text.Trim()), Globals.HtmlEncode(text2.Trim()), Globals.HtmlEncode(text3.Trim()), Globals.HtmlEncode(text4.Trim()));
				base.ReturnSuccessResult(context, "修改物流公司信息成功！", 0, true);
			}
			else if (ExpressHelper.IsExitExpress(text.Trim()))
			{
				throw new HidistroAshxException("此物流公司已存在，请重新输入！");
			}
			ExpressHelper.AddExpress(Globals.HtmlEncode(text.Trim()), Globals.HtmlEncode(text2.Trim()), Globals.HtmlEncode(text3.Trim()), Globals.HtmlEncode(text4.Trim()));
			base.ReturnSuccessResult(context, "添加物流公司信息成功！", 0, true);
		}
	}
}
