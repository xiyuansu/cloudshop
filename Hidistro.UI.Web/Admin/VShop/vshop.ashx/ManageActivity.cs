using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.vshop.ashx
{
	public class ManageActivity : AdminBaseHandler
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
				if (action == "delete")
				{
					this.Delete(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		public void GetList(HttpContext context)
		{
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			IList<VActivityInfo> allActivity = VShopHelper.GetAllActivity();
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			dataGridViewModel.total = allActivity.Count;
			foreach (VActivityInfo item2 in allActivity)
			{
				Dictionary<string, object> item = item2.ToDictionary();
				dataGridViewModel.rows.Add(item);
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "id", true);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (VShopHelper.DeleteActivity(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}
	}
}
