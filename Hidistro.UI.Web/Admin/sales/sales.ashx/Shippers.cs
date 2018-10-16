using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	public class Shippers : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "setdefaultsend":
				this.SetDefaultSend(context);
				break;
			case "setdefaultreturn":
				this.SetDefaultReturn(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
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
			IList<ShippersInfo> shippersBysupplierId = SalesHelper.GetShippersBysupplierId(0);
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			foreach (ShippersInfo item2 in shippersBysupplierId)
			{
				Dictionary<string, object> item = item2.ToDictionary();
				dataGridViewModel.rows.Add(item);
			}
			dataGridViewModel.total = dataGridViewModel.rows.Count;
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			ShippersInfo shipper = SalesHelper.GetShipper(value);
			if (shipper.SupplierId != 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (shipper.IsDefault || shipper.IsDefaultGetGoods)
			{
				throw new HidistroAshxException("不能删除默认的发货/收货信息");
			}
			if (SalesHelper.DeleteShipper(value))
			{
				base.ReturnSuccessResult(context, "已经成功删除选择的发货信息", 0, true);
				return;
			}
			throw new HidistroAshxException("不能删除默认的发货信息");
		}

		public void SetDefaultSend(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			SalesHelper.SetDefalutShipperBysupplierId(value, 0);
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}

		public void SetDefaultReturn(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			SalesHelper.SetDefalutGetGoodsShipperBysupplierId(value, 0);
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}
	}
}
