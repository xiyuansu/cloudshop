using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class ShopDecoration : StoreAdminBaseHandler
	{
		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (context.Request["flag"] == "Select")
			{
				IList<StoreFloorInfo> storeFloorList = StoresHelper.GetStoreFloorList(base.CurrentManager.StoreId, FloorClientType.Mobbile);
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = from s in storeFloorList
					select new
					{
						s.FloorId,
						s.FloorName,
						s.DisplaySequence,
						s.Quantity
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
			if (context.Request["flag"] == "Add")
			{
				int storeId = base.CurrentManager.StoreId;
				string text = context.Request["FloorName"].ToNullString();
				int imageId = context.Request["ImageId"].ToInt(0);
				string text2 = context.Request["ProductIds"].ToNullString();
				if (text.Trim().Length < 1 || text.Trim().Length > 12)
				{
					context.Response.Write(this.GetErrorJosn(101, "楼层名称不能为空，且在1-12个字符之间"));
					return;
				}
				StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
				storeFloorInfo.StoreId = storeId;
				storeFloorInfo.FloorName = text;
				storeFloorInfo.ImageId = imageId;
				storeFloorInfo.FloorClientType = FloorClientType.Mobbile;
				int floorId = StoresHelper.AddStoreFloor(storeFloorInfo);
				if (!string.IsNullOrEmpty(text2))
				{
					StoresHelper.BindStoreFloorProducts(floorId, storeId, text2);
				}
				string s3 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Success = new
						{
							Status = true,
							Msg = string.Empty
						}
					}
				});
				context.Response.Write(s3);
				context.Response.End();
			}
			if (context.Request["flag"] == "Mdy")
			{
				int storeId2 = base.CurrentManager.StoreId;
				int num = context.Request["FloorId"].ToInt(0);
				string text3 = context.Request["FloorName"].ToNullString();
				int imageId2 = context.Request["ImageId"].ToInt(0);
				string text4 = context.Request["ProductIds"].ToNullString();
				if (num <= 0)
				{
					context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
					return;
				}
				if (text3.Trim().Length < 1 || text3.Trim().Length > 12)
				{
					context.Response.Write(this.GetErrorJosn(101, "楼层名称不能为空，且在1-12个字符之间"));
					return;
				}
				StoreFloorInfo storeFloorInfo2 = new StoreFloorInfo();
				storeFloorInfo2 = StoresHelper.GetStoreFloorBaseInfo(num);
				storeFloorInfo2.FloorName = text3;
				storeFloorInfo2.ImageId = imageId2;
				storeFloorInfo2.FloorClientType = FloorClientType.Mobbile;
				StoresHelper.UpdateStoreFloor(storeFloorInfo2);
				if (!string.IsNullOrEmpty(text4))
				{
					StoresHelper.BindStoreFloorProducts(num, storeId2, text4);
				}
				string s4 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Success = new
						{
							Status = true,
							Msg = string.Empty
						}
					}
				});
				context.Response.Write(s4);
				context.Response.End();
			}
			if (context.Request["flag"] == "Delete")
			{
				int storeId3 = base.CurrentManager.StoreId;
				int num2 = context.Request["FloorId"].ToInt(0);
				if (num2 <= 0)
				{
					context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
					return;
				}
				StoresHelper.DeleteStoreFloor(num2);
				string s5 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Success = new
						{
							Status = true,
							Msg = string.Empty
						}
					}
				});
				context.Response.Write(s5);
				context.Response.End();
			}
			if (context.Request["flag"] == "SetDisplaySequence")
			{
				int storeId4 = base.CurrentManager.StoreId;
				int num3 = context.Request["FloorId"].ToInt(0);
				if (num3 <= 0)
				{
					context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
				}
				else
				{
					int displaySequence = context.Request["DisplaySequence"].ToInt(0);
					StoreFloorInfo storeFloorBaseInfo = StoresHelper.GetStoreFloorBaseInfo(num3);
					if (storeFloorBaseInfo == null)
					{
						context.Response.Write(this.GetErrorJosn(101, "错误的楼层ID"));
					}
					else
					{
						storeFloorBaseInfo.DisplaySequence = displaySequence;
						StoresHelper.UpdateStoreFloor(storeFloorBaseInfo);
						string s6 = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Success = new
								{
									Status = true,
									Msg = string.Empty
								}
							}
						});
						context.Response.Write(s6);
						context.Response.End();
					}
				}
			}
		}

		private string GetErrorJosn(int errorCode, string errorMsg)
		{
			return JsonConvert.SerializeObject(new
			{
				ErrorResponse = new
				{
					ErrorCode = errorCode,
					ErrorMsg = errorMsg
				}
			});
		}
	}
}
