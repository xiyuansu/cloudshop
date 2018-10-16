using Hidistro.Entities;
using Hidistro.SaleSystem;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class MarktingList : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
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

		private void GetList(HttpContext context)
		{
			DataGridViewModel<StoreMarktingInfo> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<StoreMarktingInfo> GetDataList()
		{
			DataGridViewModel<StoreMarktingInfo> dataGridViewModel = new DataGridViewModel<StoreMarktingInfo>();
			List<StoreMarktingInfo> storeMarktingInfoList = StoreMarktingHelper.GetStoreMarktingInfoList();
			dataGridViewModel.rows = storeMarktingInfoList.ToList();
			dataGridViewModel.total = storeMarktingInfoList.Count;
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "ids", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数！");
			}
			if (StoreMarktingHelper.Delete(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败！");
		}

		private void SaveOrder(HttpContext context)
		{
			int value = base.GetIntParam(context, "Value", false).Value;
			if (value >= 0)
			{
				int value2 = base.GetIntParam(context, "ServiceId", false).Value;
				try
				{
					if (StoreMarktingHelper.UpdateDisplaySequence(value2, value))
					{
						base.ReturnSuccessResult(context, "保存排序成功！", 0, true);
					}
				}
				catch (Exception)
				{
					throw new HidistroAshxException("修改排序失败！未知错误！");
				}
				return;
			}
			throw new HidistroAshxException("错误的参数");
		}
	}
}
