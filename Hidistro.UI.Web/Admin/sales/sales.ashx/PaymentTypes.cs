using Hidistro.Context;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	[PrivilegeCheck(Privilege.PaymentModes)]
	public class PaymentTypes : AdminBaseHandler
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
			case "saveorder":
				this.SaveOrder(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
			DataGridViewModel<PaymentModeInfo> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<PaymentModeInfo> GetDataList()
		{
			DataGridViewModel<PaymentModeInfo> dataGridViewModel = new DataGridViewModel<PaymentModeInfo>();
			IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnPC);
			paymentModes = (from d in paymentModes
			where d.Gateway.ToLower() != "hishop.plugins.payment.wxqrcode.wxqrcoderequest"
			select d).ToList();
			dataGridViewModel.rows = paymentModes.ToList();
			dataGridViewModel.total = paymentModes.Count;
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int modeId = int.Parse(context.Request["ModeId"]);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (masterSettings.IsDemoSite)
			{
				throw new HidistroAshxException("演示站点，不允许删除支付方式！");
			}
			if (SalesHelper.DeletePaymentMode(modeId))
			{
				base.ReturnSuccessResult(context, "成功删除了一个支付方式！", 0, true);
				return;
			}
			throw new HidistroAshxException("未知错误！");
		}

		private void SaveOrder(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			int value2 = base.GetIntParam(context, "sort", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (SalesHelper.UpdateDisplaySequence(value, value2))
			{
				base.ReturnSuccessResult(context, "更新排序成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("更新排序失败！");
		}
	}
}
