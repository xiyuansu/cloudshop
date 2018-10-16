using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin.depot.ashx
{
	public class IbeaconPageList : AdminBaseHandler
	{
		public List<WXStoreHelper.Page> Pages
		{
			get;
			set;
		}

		public List<WXStoreHelper.ConfigurationDevice> ConfigurationDevices
		{
			get;
			set;
		}

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
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

		private void GetList(HttpContext context)
		{
			DataGridViewModel<SerachPagesInfo> dataList = this.GetDataList(context);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<SerachPagesInfo> GetDataList(HttpContext context)
		{
			DataGridViewModel<SerachPagesInfo> dataGridViewModel = new DataGridViewModel<SerachPagesInfo>();
			int num = 1;
			int num2 = 10;
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			int count = (num - 1) * num2;
			this.Pages = WXStoreHelper.GetAllPages();
			this.ConfigurationDevices = WXStoreHelper.GetAllPagesConfigurationDeviceNumber();
			dataGridViewModel.total = this.Pages.Count;
			dataGridViewModel.rows = (from c in this.Pages
			select new SerachPagesInfo
			{
				page_id = c.page_id,
				comment = c.comment,
				description = c.description,
				icon_url = c.icon_url,
				page_url = c.page_url,
				title = c.title,
				quantity = this.GetDevicesQuantity(c.page_id)
			} into c
			orderby c.quantity descending
			select c).Skip(count).Take(num2).ToList();
			return dataGridViewModel;
		}

		private int GetDevicesQuantity(long pageId)
		{
			int result = 0;
			WXStoreHelper.ConfigurationDevice configurationDevice = this.ConfigurationDevices.FirstOrDefault((WXStoreHelper.ConfigurationDevice c) => c.PageId == pageId);
			if (configurationDevice != null)
			{
				result = configurationDevice.ConfigurationDeviceNumber;
			}
			return result;
		}

		public void Delete(HttpContext context)
		{
			string text = context.Request.Form["ids"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			WxJsonResult wxJsonResult = WXStoreHelper.DeletePage(text.ToLong(0));
			if (wxJsonResult.errcode.Equals(ReturnCode.请求成功))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
			}
		}
	}
}
