using Hidistro.Context;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Supplier.system.ashx
{
	[AdministerCheck(true)]
	public class Shippers : SupplierAdminHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			base.CurrentManager = HiContext.Current.Manager;
			string text = context.Request["action"].ToLower();
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			switch (text)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "setfirstinfo":
				this.SetFirstInfo(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
			DataGridViewModel<ShippersInfo> data = this.GetData();
			string s = base.SerializeObjectToJson(data);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<ShippersInfo> GetData()
		{
			DataGridViewModel<ShippersInfo> dataGridViewModel = new DataGridViewModel<ShippersInfo>();
			IList<ShippersInfo> shippersBysupplierId = SalesHelper.GetShippersBysupplierId(HiContext.Current.Manager.StoreId);
			foreach (ShippersInfo item in shippersBysupplierId)
			{
			}
			dataGridViewModel.rows = shippersBysupplierId.ToList();
			dataGridViewModel.total = shippersBysupplierId.Count;
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "shipperId", false).Value;
			ShippersInfo shipper = SalesHelper.GetShipper(value);
			if (shipper == null || shipper.SupplierId != HiContext.Current.Manager.StoreId)
			{
				throw new HidistroAshxException("请误操作！");
			}
			if (shipper.IsDefault || shipper.IsDefaultGetGoods)
			{
				throw new HidistroAshxException("不能删除默认的发货/收货信息！");
			}
			if (SalesHelper.DeleteShipper(value))
			{
				base.ReturnSuccessResult(context, "已经成功删除选择的发货信息！", 0, true);
				return;
			}
			throw new HidistroAshxException("请误操作！");
		}

		private void SetFirstInfo(HttpContext context)
		{
			string parameter = base.GetParameter(context, "type", false);
			int value = base.GetIntParam(context, "shipperId", false).Value;
			if (parameter == "SetYesOrNo")
			{
				ShippersInfo shipper = SalesHelper.GetShipper(value);
				if (!shipper.IsDefault)
				{
					SalesHelper.SetDefalutShipperBysupplierId(value, HiContext.Current.Manager.StoreId);
					goto IL_00c7;
				}
				throw new HidistroAshxException("错误的参数");
			}
			if (parameter == "SetYeNoDefaultGetGoods")
			{
				ShippersInfo shipper2 = SalesHelper.GetShipper(value);
				if (!shipper2.IsDefaultGetGoods)
				{
					SalesHelper.SetDefalutGetGoodsShipperBysupplierId(value, HiContext.Current.Manager.StoreId);
					goto IL_00c7;
				}
				throw new HidistroAshxException("错误的参数");
			}
			goto IL_00c7;
			IL_00c7:
			base.ReturnSuccessResult(context, "", 0, true);
		}
	}
}
