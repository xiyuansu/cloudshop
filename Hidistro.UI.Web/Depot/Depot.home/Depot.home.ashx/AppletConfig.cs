using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class AppletConfig : StoreAdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			switch (action)
			{
			default:
				if (action == "DeleteAppletFloor")
				{
					this.DeleteAppletFloor(context);
				}
				break;
			case "GetAppletFloorList":
				this.GetAppletFloorList(context);
				break;
			case "AddAppletFloor":
				this.AddAppletFloor(context);
				break;
			case "UpdateAppletFloor":
				this.UpdateAppletFloor(context);
				break;
			case "SetDisplaySequence":
				this.SetDisplaySequence(context);
				break;
			}
		}

		public void GetAppletFloorList(HttpContext context)
		{
			int storeId = HiContext.Current.Manager.StoreId;
			IList<StoreFloorInfo> storeFloorList = StoresHelper.GetStoreFloorList(storeId, FloorClientType.O2OApplet);
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			dataGridViewModel.rows = DataHelper.ListToDictionary(storeFloorList);
			dataGridViewModel.total = 10000;
			string s = base.SerializeObjectToJson(dataGridViewModel);
			context.Response.Write(s);
			context.Response.End();
		}

		public void AddAppletFloor(HttpContext context)
		{
			int storeId = HiContext.Current.Manager.StoreId;
			string text = context.Request["FloorName"].ToNullString();
			int imageId = context.Request["ImageId"].ToInt(0);
			string text2 = context.Request["ProductIds"].ToNullString();
			if (text.Trim().Length < 1 || text.Trim().Length > 10)
			{
				throw new HidistroAshxException("楼层名称不能为空，且在1-10个字符之间");
			}
			StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
			storeFloorInfo.StoreId = storeId;
			storeFloorInfo.FloorName = text;
			storeFloorInfo.ImageId = imageId;
			storeFloorInfo.FloorClientType = FloorClientType.O2OApplet;
			int floorId = StoresHelper.AddStoreFloor(storeFloorInfo);
			if (!string.IsNullOrEmpty(text2))
			{
				StoresHelper.BindStoreFloorProducts(floorId, storeId, text2);
			}
			base.ReturnSuccessResult(context, "保存成功", 0, true);
		}

		public void UpdateAppletFloor(HttpContext context)
		{
			int storeId = HiContext.Current.Manager.StoreId;
			int num = context.Request["FloorId"].ToInt(0);
			string text = context.Request["FloorName"].ToNullString();
			int imageId = 0;
			string text2 = context.Request["ProductIds"].ToNullString();
			if (num <= 0)
			{
				throw new HidistroAshxException("错误的楼层ID");
			}
			if (text.Trim().Length < 1 || text.Trim().Length > 10)
			{
				throw new HidistroAshxException("楼层名称不能为空，且在1-10个字符之间");
			}
			StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
			storeFloorInfo = StoresHelper.GetStoreFloorBaseInfo(num);
			if (storeFloorInfo == null || storeFloorInfo.StoreId != storeId || storeFloorInfo.FloorClientType != FloorClientType.O2OApplet)
			{
				throw new HidistroAshxException("错误的楼层ID或者不是门店楼层");
			}
			storeFloorInfo.FloorName = text;
			storeFloorInfo.ImageId = imageId;
			storeFloorInfo.FloorClientType = FloorClientType.O2OApplet;
			StoresHelper.UpdateStoreFloor(storeFloorInfo);
			if (!string.IsNullOrEmpty(text2))
			{
				StoresHelper.BindStoreFloorProducts(num, storeId, text2);
			}
			base.ReturnSuccessResult(context, "修改成功", 0, true);
		}

		public void SetDisplaySequence(HttpContext context)
		{
			int storeId = HiContext.Current.Manager.StoreId;
			int num = context.Request["FloorId"].ToInt(0);
			if (num <= 0)
			{
				throw new HidistroAshxException("错误的楼层ID");
			}
			int displaySequence = context.Request["DisplaySequence"].ToInt(0);
			StoreFloorInfo storeFloorBaseInfo = StoresHelper.GetStoreFloorBaseInfo(num);
			if (storeFloorBaseInfo == null || storeFloorBaseInfo.StoreId != storeId || storeFloorBaseInfo.FloorClientType != FloorClientType.O2OApplet)
			{
				throw new HidistroAshxException("错误的楼层ID或者不是门店楼层");
			}
			storeFloorBaseInfo.DisplaySequence = displaySequence;
			StoresHelper.UpdateStoreFloor(storeFloorBaseInfo);
			base.ReturnSuccessResult(context, "修改成功", 0, true);
		}

		public void DeleteAppletFloor(HttpContext context)
		{
			int storeId = HiContext.Current.Manager.StoreId;
			int num = context.Request["FloorId"].ToInt(0);
			if (num <= 0)
			{
				throw new HidistroAshxException("请选择要删除的楼层");
			}
			StoreFloorInfo storeFloorBaseInfo = StoresHelper.GetStoreFloorBaseInfo(num);
			if (storeFloorBaseInfo == null || storeFloorBaseInfo.StoreId != storeId || storeFloorBaseInfo.FloorClientType != FloorClientType.O2OApplet)
			{
				throw new HidistroAshxException("错误的楼层ID或者不是门店楼层");
			}
			StoresHelper.DeleteStoreFloor(num);
			base.ReturnSuccessResult(context, "删除楼层成功", 0, true);
		}
	}
}
