using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.store.ashx
{
	public class FriendlyLinks : AdminBaseHandler
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
			case "setshow":
				this.SetShow(context);
				break;
			case "sort":
				this.Sort(context);
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
			IList<FriendlyLinksInfo> friendlyLinks = StoreHelper.GetFriendlyLinks();
			dataGridViewModel.rows = new List<Dictionary<string, object>>();
			foreach (FriendlyLinksInfo item2 in friendlyLinks)
			{
				item2.ImageUrl = base.GetImageOrDefaultImage(item2.ImageUrl, base.CurrentSiteSetting.DefaultProductImage);
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
			FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(value);
			if (StoreHelper.FriendlyLinkDelete(value))
			{
				try
				{
					StoreHelper.DeleteImage(friendlyLink.ImageUrl);
				}
				catch
				{
				}
				base.ReturnSuccessResult(context, "成功删除了选择的友情链接", 0, true);
				return;
			}
			throw new HidistroAshxException("未知错误");
		}

		public void SetShow(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(value);
			friendlyLink.Visible = !friendlyLink.Visible;
			StoreHelper.UpdateFriendlyLink(friendlyLink);
			base.ReturnSuccessResult(context, "操作成功", 0, true);
		}

		public void Sort(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			int value2 = base.GetIntParam(context, "sort", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (StoreHelper.SwapFriendlyLinkSequence(value, value2))
			{
				base.ReturnSuccessResult(context, "更新排序成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}
	}
}
