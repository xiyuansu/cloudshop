using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Core.Urls;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Entities.WeChatApplet;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Members;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Statistics;
using Hidistro.SaleSystem.Store;
using Hidistro.SaleSystem.WeChartApplet;
using Hidistro.SqlDal.WeChatApplet;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.App_Code;
using Hishop.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class StoreAppAPI : IHttpHandler, IRequiresSessionState
	{
		private HttpContext myContext;

		private const string VERIFICATION_CODE_QRCODE_SAVE_RELATIVE_PATH = "/Storage/master/ServiceQRCode/";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			this.myContext = context;
			context.Response.ContentType = "application/json";
			string text = context.Request["action"];
			switch (text)
			{
			case "StoreAdminLogin":
				this.StoreAdminLogin(context);
				break;
			case "OrderStatistical":
				this.OrderStatistics(context);
				break;
			case "GetOrderNumber":
				this.GetOrderNumber(context);
				break;
			case "GetOrderDetailOfTackCode":
				this.GetOrderDetailOfTackCode(context);
				break;
			case "ConfirmTackGoods":
				this.ConfirmTackGoods(context);
				break;
			case "ConfirmOrders":
				this.ConfirmOrders(context);
				break;
			case "GetOrders":
				this.GetOrders(context);
				break;
			case "GetProducts":
				this.GetProducts(context);
				break;
			case "SendGoodsOrderDetail":
				this.SendGoodsOrderDetail(context);
				break;
			case "ExpressType":
				this.ExpressType(context);
				break;
			case "QueryDeliverFee":
				this.QueryDeliverFee(context);
				break;
			case "SendGoods":
				this.SendGoods(context);
				break;
			case "CancelSendGoods":
				this.CancelSendGoods(context);
				break;
			case "GetRefundsList":
				this.GetRefundsList(context);
				break;
			case "GetRefundedList":
				this.GetRefundedList(context);
				break;
			case "GetRefundRefusedList":
				this.GetRefundRefusedList(context);
				break;
			case "GetReturnedList":
				this.GetReturnedList(context);
				break;
			case "GetReturnRefusedList":
				this.GetReturnRefusedList(context);
				break;
			case "GetReturnsList":
				this.GetRefundsList(context);
				break;
			case "GetAfterSaleingList":
				this.GetAfterSaleingList(context);
				break;
			case "GetAfterSaleCompletedList":
				this.GetAfterSaleCompletedList(context);
				break;
			case "GetAfterSaleRefusedList":
				this.GetAfterSaleRefusedList(context);
				break;
			case "GetAllAfterSaleList":
				this.GetAllAfterSaleList(context);
				break;
			case "AgreedRefund":
				this.AgreedRefund(context);
				break;
			case "AgreedReturn":
				this.AgreedReturn(context);
				break;
			case "FinishReturn":
				this.FinishReturn(context);
				break;
			case "GetRefundDetail":
				this.GetRefundDetail(context);
				break;
			case "GetReturnDetail":
				this.GetReturnDetail(context);
				break;
			case "UnShelves":
				this.UnShelvesProducts(context);
				break;
			case "BatchUpdateProductStock":
				this.BatchUpdateProductStock(context);
				break;
			case "BatchUpdateSkuStock":
				this.BatchUpdateSkuStock(context);
				break;
			case "GetOrderDetail":
				this.GetOrderDetail(context);
				break;
			case "GetServiceOrderDetail":
				this.GetServiceOrderDetail(context);
				break;
			case "GetGoodsOnDoorStatistical":
				this.GetGoodsOnDoorStatistical(context);
				break;
			case "GetSkus":
				this.GetSkus(context);
				break;
			case "ReturnOnStore":
				this.ReturnOnStore(context);
				break;
			case "UpdateStorePassword":
				this.UpdateStorePassword(context);
				break;
			case "MessageStatistical":
				this.MessageStatistical(context);
				break;
			case "GetMessageList":
				this.GetMessageList(context);
				break;
			case "GetRegionList":
				this.GetRegionList(context);
				break;
			case "GetExpressList":
				this.GetExpressList(context);
				break;
			case "UpdateStoreBaseInfo":
				this.UpdateStoreBaseInfo(context);
				break;
			case "GetStoreBaseInfo":
				this.GetStoreBaseInfo(context);
				break;
			case "GetNoStockProductList":
				this.GetNoStockProductList(context);
				break;
			case "OfflinePay":
				this.OfflinePay(context);
				break;
			case "GetOfflineOrderList":
				this.GetOfflineOrderList(context);
				break;
			case "BillDetailStatistical":
				this.BillDetailStatistical(context);
				break;
			case "CashierDetailList":
				this.CashierDetailList(context);
				break;
			case "CashierStatistical":
				this.CashierStatistical(context);
				break;
			case "GetSupportPayType":
				this.GetSupportPayType(context);
				break;
			case "GenerateOfflineOrder":
				this.GenerateOfflineOrder(context);
				break;
			case "UploadStoreImage":
				this.UploadStoreImage(context);
				break;
			case "DeleteStoreImage":
				this.DeleteStoreImage(context);
				break;
			case "CheckOrderProductStock":
				this.CheckOrderProductStock(context);
				break;
			case "ShowFileTypeCode":
				this.ShowFileTypeCode(context);
				break;
			case "GetReplaceDetail":
				this.GetReplaceDetail(context);
				break;
			case "AgreedReplace":
				this.AgreedReplace(context);
				break;
			case "GetGoodsAndSendGoods":
				this.GetGoodsAndSendGoods(context);
				break;
			case "GetCategories":
				this.GetCategories(context);
				break;
			case "OnShelves":
				this.OnShelvesProduct(context);
				break;
			case "BatchOnShelvesProducts":
				this.BatchOnShelvesProducts(context);
				break;
			case "GetOnShelvesSkus":
				this.GetOnShelvesSkus(context);
				break;
			case "GetMarketingImages":
				this.GetMarketingImages(context);
				break;
			case "GetImageProducts":
				this.GetImageProducts(context);
				break;
			case "DeleteImageProducts":
				this.DeleteImageProducts(context);
				break;
			case "GetImageNoRelationProducts":
				this.GetImageNoRelationProducts(context);
				break;
			case "ImageRelationProducts":
				this.ImageRelationProducts(context);
				break;
			case "GetStoreFloorList":
				this.GetStoreFloorList(context);
				break;
			case "GetPlatProducts":
				this.GetPlatProducts(context);
				break;
			case "GetStoreFloorDetail":
				this.GetStoreFloorDetail(context);
				break;
			case "GetStoreProductsFloorDisplaySequence":
				this.GetStoreProductsFloorDisplaySequence(context);
				break;
			case "AddStoreFloor":
				this.AddStoreFloor(context);
				break;
			case "UpdateStoreFloor":
				this.UpdateStoreFloor(context);
				break;
			case "UpdateStoreFloorDisplaySequence":
				this.UpdateStoreFloorDisplaySequence(context);
				break;
			case "DeleteStoreFloor":
				this.DeleteStoreFloor(context);
				break;
			case "AddSubAccount":
				this.AddSubAccount(context);
				break;
			case "EditSubAccount":
				this.EditSubAccount(context);
				break;
			case "GetSubAccountDetail":
				this.GetSubAccountDetail(context);
				break;
			case "GetSubAccounts":
				this.GetSubAccounts(context);
				break;
			case "GetShoppingGuiders":
				this.GetShoppingGuiders(context);
				break;
			case "DeleteStoreGuiderHeadImage":
				this.DeleteStoreGuiderHeadImage(context);
				break;
			case "UploadStoreGuiderHeadImage":
				this.UploadStoreGuiderHeadImage(context);
				break;
			case "GetUserGroupStatistics":
				this.GetUserGroupStatistics(context);
				break;
			case "GetStoreUsersStatistics":
				this.GetStoreUsersStatistics(context);
				break;
			case "GetStoreUserConsumeList":
				this.GetStoreUserConsumeList(context);
				break;
			case "GetStoreUserDetails":
				this.GetStoreUserDetails(context);
				break;
			case "UpdateStoreUserInfo":
				this.UpdateStoreUserInfo(context);
				break;
			case "CloseStoreSet":
				this.CloseStoreSet(context);
				break;
			case "GetCloseStoreSetInfo":
				this.GetCloseStoreSetInfo(context);
				break;
			case "TodaySaleStatistics":
				this.TodaySaleStatistics(context);
				break;
			case "SaleAchievementData":
				this.SaleAchievementData(context);
				break;
			case "StoreAbilityStatistics":
				this.StoreAbilityStatistics(context);
				break;
			case "SetTradePassword":
				this.SetTradePassword(context);
				break;
			case "ChangeTradePassword":
				this.ChangeTradePassword(context);
				break;
			case "ValidTradePassword":
				this.ValidTradePassword(context);
				break;
			case "BindDrawCardInfo":
				this.BindDrawCardInfo(context);
				break;
			case "GetDrawCardInfo":
				this.GetDrawCardInfo(context);
				break;
			case "GetStoreUserBaseInfo":
				this.GetStoreUserBaseInfo(context);
				break;
			case "SaveStoreUserBaseInfo":
				this.SaveStoreUserBaseInfo(context);
				break;
			case "StoreBalanceStatistics":
				this.StoreBalanceStatistics(context);
				break;
			case "BalanceDetailList":
				this.BalanceDetailList(context);
				break;
			case "NotOverBalanceOrderList":
				this.NotOverBalanceOrderList(context);
				break;
			case "OverBalanceOrderList":
				this.OverBalanceOrderList(context);
				break;
			case "BalanceOrderDetail":
				this.BalanceOrderDetail(context);
				break;
			case "ApplyBalanceRequest":
				this.ApplyBalanceRequest(context);
				break;
			case "BalanceRequestList":
				this.BalanceRequestList(context);
				break;
			case "GetGuiderReferralCode":
				this.GetGuiderReferralCode(context);
				break;
			case "ProcessAppInit":
				this.ProcessAppInit(context);
				break;
			case "GetStoreSlideImages":
				this.GetStoreSlideImages(context);
				break;
			case "UploadStoreSlideImage":
				this.UploadStoreSlideImage(context);
				break;
			case "DeleteStoreSlideImage":
				this.DeleteStoreSlideImage(context);
				break;
			case "GetChoiceProducts":
				this.GetChoiceProducts(context);
				break;
			case "AddChoiceProducts":
				this.AddChoiceProducts(context);
				break;
			case "RemoveChoiceProduct":
				this.RemoveChoiceProduct(context);
				break;
			case "OrderAppliedVerificationDetail":
				this.OrderAppliedVerificationDetail(context);
				break;
			case "OrderVerification":
				this.OrderVerification(context);
				break;
			case "OrderVerificationDetail":
				this.OrderVerificationDetail(context);
				break;
			case "GetFinishedVerificationRecord":
				this.GetFinishedVerificationRecord(context);
				break;
			case "GetAppletFloorList":
				this.GetAppletFloorList(context);
				break;
			case "GetAppletFloorDetail":
				this.GetAppletFloorDetail(context);
				break;
			case "GetAppletProductsFloorDisplaySequence":
				this.GetAppletProductsFloorDisplaySequence(context);
				break;
			case "AddAppletFloor":
				this.AddAppletFloor(context);
				break;
			case "UpdateAppletFloor":
				this.UpdateAppletFloor(context);
				break;
			case "UpdateAppletFloorDisplaySequence":
				this.UpdateAppletFloorDisplaySequence(context);
				break;
			case "DeleteAppletFloor":
				this.DeleteAppletFloor(context);
				break;
			default:
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
				context.Response.End();
				break;
			}
		}

		public void GetAppletFloorList(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			IList<StoreFloorInfo> storeFloorList = StoresHelper.GetStoreFloorList(storeIdBySessionId, FloorClientType.O2OApplet);
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

		public void GetAppletFloorDetail(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["FloorId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				StoreFloorInfo storeFloorBaseInfo = StoresHelper.GetStoreFloorBaseInfo(num);
				if (storeFloorBaseInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(1007, ""));
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							FloorId = storeFloorBaseInfo.FloorId,
							FloorName = storeFloorBaseInfo.FloorName,
							Products = from d in storeFloorBaseInfo.Products
							select new
							{
								d.ProductId
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void GetAppletProductsFloorDisplaySequence(HttpContext context)
		{
			SiteSettings setting = SettingsManager.GetMasterSettings();
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeId = this.GetStoreIdBySessionId(context);
			int num3 = context.Request["cId"].ToInt(0);
			int floorId = context.Request["floorId"].ToInt(0);
			string productName = Globals.StripAllTags(context.Request["keyword"].ToNullString());
			string productCode = Globals.StripAllTags(context.Request["productCode"].ToNullString());
			bool warningStockNum = true;
			if (context.Request["isWarningStock"].ToNullString() != "1")
			{
				warningStockNum = context.Request["isWarningStock"].ToBool();
			}
			if (num2 < 1)
			{
				num2 = 10;
			}
			StoreProductsQuery storeProductsQuery = new StoreProductsQuery();
			storeProductsQuery.PageIndex = num;
			storeProductsQuery.PageSize = num2;
			storeProductsQuery.productCode = productCode;
			storeProductsQuery.WarningStockNum = warningStockNum;
			storeProductsQuery.ProductName = productName;
			storeProductsQuery.SaleStatus = 1.GetHashCode();
			if (num3 > 0)
			{
				storeProductsQuery.CategoryId = num3;
				storeProductsQuery.MainCategoryPath = CatalogHelper.GetCategory(num3).Path;
			}
			storeProductsQuery.StoreId = storeId;
			PageModel<StoreProductsViewInfo> storeProductsFloorDisplaySequence = StoresHelper.GetStoreProductsFloorDisplaySequence(storeProductsQuery, floorId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeProductsFloorDisplaySequence.Total,
					List = from d in storeProductsFloorDisplaySequence.Models
					select new
					{
						StoreId = storeId,
						ProductId = d.ProductId,
						Stock = d.Stock,
						ProductCode = d.ProductCode,
						ProductName = d.ProductName,
						CategoryId = d.CategoryId,
						SaleStatus = d.SaleStatus,
						SalePrice = d.SalePrice,
						CostPrice = d.CostPrice,
						MarketPrice = d.MarketPrice,
						ThumbnailUrl40 = (string.IsNullOrEmpty(d.ThumbnailUrl40) ? Globals.FullPath(setting.DefaultProductThumbnail8) : Globals.FullPath(d.ThumbnailUrl40.Replace("/thumbs40/40_", "/thumbs410/410_"))),
						MainCategoryPath = d.MainCategoryPath,
						ExtendCategoryPath = d.ExtendCategoryPath,
						ExtendCategoryPath1 = d.ExtendCategoryPath1,
						ExtendCategoryPath2 = d.ExtendCategoryPath2,
						ExtendCategoryPath3 = d.ExtendCategoryPath3,
						ExtendCategoryPath4 = d.ExtendCategoryPath4,
						WarningStockNum = d.WarningStockNum,
						DisplaySequence = d.DisplaySequence,
						IsChecked = ((d.StoreId.ToInt(0) > 0) ? "checked" : "")
					}
				}
			});
			context.Response.Write(s);
		}

		public void AddAppletFloor(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string text = context.Request["FloorName"].ToNullString();
			int imageId = context.Request["ImageId"].ToInt(0);
			string text2 = context.Request["ProductIds"].ToNullString();
			if (text.Trim().Length < 1 || text.Trim().Length > 10)
			{
				context.Response.Write(this.GetErrorJosn(101, "楼层名称不能为空，且在1-10个字符之间"));
			}
			else
			{
				StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
				storeFloorInfo.StoreId = storeIdBySessionId;
				storeFloorInfo.FloorName = text;
				storeFloorInfo.ImageId = imageId;
				storeFloorInfo.FloorClientType = FloorClientType.O2OApplet;
				int floorId = StoresHelper.AddStoreFloor(storeFloorInfo);
				if (!string.IsNullOrEmpty(text2))
				{
					StoresHelper.BindStoreFloorProducts(floorId, storeIdBySessionId, text2);
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						Msg = string.Empty
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void UpdateAppletFloor(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["FloorId"].ToInt(0);
			string text = context.Request["FloorName"].ToNullString();
			int imageId = 0;
			string text2 = context.Request["ProductIds"].ToNullString();
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else if (text.Trim().Length < 1 || text.Trim().Length > 10)
			{
				context.Response.Write(this.GetErrorJosn(101, "楼层名称不能为空，且在1-10个字符之间"));
			}
			else
			{
				StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
				storeFloorInfo = StoresHelper.GetStoreFloorBaseInfo(num);
				if (storeFloorInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(101.GetHashCode(), "错误的楼层ID"));
				}
				else
				{
					storeFloorInfo.FloorName = text;
					storeFloorInfo.ImageId = imageId;
					storeFloorInfo.FloorClientType = FloorClientType.O2OApplet;
					StoresHelper.UpdateStoreFloor(storeFloorInfo);
					if (!string.IsNullOrEmpty(text2))
					{
						StoresHelper.BindStoreFloorProducts(num, storeIdBySessionId, text2);
					}
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = true,
							Msg = string.Empty
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void UpdateAppletFloorDisplaySequence(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string text = context.Request["FloorIds"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				StoresHelper.UpdateStoreFloorDisplaySequence(storeIdBySessionId, text);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						Msg = string.Empty
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void DeleteAppletFloor(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["FloorId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				StoresHelper.DeleteStoreFloor(num);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						Msg = string.Empty
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void GetFinishedVerificationRecord(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			ManagerInfo managerInfoBySessionId = this.GetManagerInfoBySessionId(context);
			int managerId = 0;
			if (managerInfoBySessionId.RoleId == -3)
			{
				managerId = managerInfoBySessionId.ManagerId;
			}
			string keyword = context.Request["keyword"].ToNullString();
			Pagination pagination = new Pagination();
			int num = context.Request["PageIndex"].ToInt(0);
			if (num < 1)
			{
				num = 1;
			}
			int num2 = context.Request["PageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			pagination.PageIndex = num;
			pagination.PageSize = num2;
			pagination.SortOrder = SortAction.Desc;
			pagination.SortBy = "VerificationDate";
			DbQueryResult finishedVerificationRecord = OrderHelper.GetFinishedVerificationRecord(pagination, storeIdBySessionId, keyword, managerId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = finishedVerificationRecord.TotalRecords,
					List = from d in finishedVerificationRecord.Data.AsEnumerable()
					select new
					{
						OrderId = d.Field<string>("OrderId"),
						ProductName = d.Field<string>("ItemDescription"),
						ThumbnailsUrl = Globals.FullPath(d.Field<string>("ThumbnailsUrl")),
						Price = d.Field<decimal>("ItemAdjustedPrice").F2ToString("f2"),
						num = d.Field<int>("num"),
						UserName = d.Field<string>("UserName"),
						VerificationDate = d.Field<DateTime>("VerificationDate").ToString("yyyy-MM-dd HH:mm:ss")
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void OrderAppliedVerificationDetail(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string verificationPassword = context.Request["VerificationPassword"];
			OrderVerificationItemInfo verificationInfoByPassword = OrderHelper.GetVerificationInfoByPassword(verificationPassword);
			DateTime dateTime;
			if (verificationInfoByPassword == null)
			{
				context.Response.Write(this.GetErrorJosn(801, "该核销码无效，请重新输入"));
			}
			else if (verificationInfoByPassword.StoreId != storeIdBySessionId)
			{
				context.Response.Write(this.GetErrorJosn(802, "非本门店核销码，请买家核对信息"));
			}
			else if (verificationInfoByPassword.VerificationStatus == 1)
			{
				HttpResponse response = context.Response;
				dateTime = verificationInfoByPassword.VerificationDate.Value;
				response.Write(this.GetErrorJosn(801, "该核销码 于" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "已核销"));
			}
			else if (verificationInfoByPassword.VerificationStatus == 3)
			{
				context.Response.Write(this.GetErrorJosn(801, "核销码已过期，无法核销"));
			}
			else if (verificationInfoByPassword.VerificationStatus == 5)
			{
				context.Response.Write(this.GetErrorJosn(801, "此核销码已进行售后，无法核销"));
			}
			else if (verificationInfoByPassword.VerificationStatus == 4)
			{
				context.Response.Write(this.GetErrorJosn(801, "此核销码正在进行售后，无法核销"));
			}
			else
			{
				OrderInfo order = OrderHelper.GetServiceProductOrderInfo(verificationInfoByPassword.OrderId);
				if (order == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (order.StoreId != storeIdBySessionId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else
				{
					string text = "0.00";
					decimal? deductionMoney = order.DeductionMoney;
					text = ((deductionMoney.GetValueOrDefault() > default(decimal) && deductionMoney.HasValue) ? "-" : "") + order.DeductionMoney.ToDecimal(0).F2ToString("f2");
					string orderId = order.OrderId;
					string statusText = EnumDescription.GetEnumDescription((Enum)(object)order.OrderStatus, 0).Replace("待发货", "待消费");
					OrderStatus orderStatus = order.OrderStatus;
					OrderItemStatus itemStatus = order.ItemStatus;
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0);
					dateTime = order.OrderDate;
					string s2 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							OrderId = orderId,
							StatusText = statusText,
							Status = (int)orderStatus,
							ItemStatus = (int)itemStatus,
							ItemStatusText = enumDescription,
							OrderDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							OrderAmount = order.GetAmount(false).F2ToString("f2"),
							OrderTotal = order.GetTotal(false).F2ToString("f2"),
							PaymentType = order.PaymentType,
							CouponAmount = ((order.CouponValue > decimal.Zero) ? "-" : "") + order.CouponValue.F2ToString("f2"),
							CouponName = order.CouponName,
							DeductionPoints = order.DeductionPoints,
							DeductionMoney = text,
							RefundAmount = ((order.RefundAmount > decimal.Zero) ? "-" : "") + order.RefundAmount.F2ToString("f2"),
							Remark = order.Remark,
							LineItems = from d in order.LineItems.Keys
							select new
							{
								Status = order.LineItems[d].Status,
								StatusText = order.LineItems[d].StatusText,
								Id = order.LineItems[d].SkuId,
								Name = order.LineItems[d].ItemDescription,
								Price = order.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
								Amount = order.LineItems[d].ShipmentQuantity,
								Image = this.GetImageFullPath(order.LineItems[d].ThumbnailsUrl),
								SkuText = order.LineItems[d].SKUContent,
								ProductId = order.LineItems[d].ProductId,
								PromotionName = order.LineItems[d].PromotionName,
								VerificationItems = from f in order.LineItems[d].VerificationItems
								where f.VerificationStatus == 0
								select f into v
								select new
								{
									v.VerificationPassword
								}
							},
							InputItems = from g in (from t in order.InputItems
							group t by t.InputFieldGroup).ToDictionary((IGrouping<int, OrderInputItemInfo> t) => t.Key, (IGrouping<int, OrderInputItemInfo> t) => t.ToList())
							select new
							{
								InputFieldGroup = from s in g.Value
								select new
								{
									s.InputFieldTitle,
									s.InputFieldType,
									s.InputFieldValue
								}
							}
						}
					});
					context.Response.Write(s2);
					context.Response.End();
				}
			}
		}

		private void OrderVerificationDetail(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string orderId = context.Request["OrderId"];
			string VerificationDate = context.Request["VerificationDate"];
			OrderInfo order = OrderHelper.GetServiceProductOrderInfo(orderId);
			if (order == null)
			{
				context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
			}
			else if (order.StoreId != storeIdBySessionId)
			{
				context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
			}
			else
			{
				string text = "0.00";
				decimal? deductionMoney = order.DeductionMoney;
				text = ((deductionMoney.GetValueOrDefault() > default(decimal) && deductionMoney.HasValue) ? "-" : "") + order.DeductionMoney.ToDecimal(0).F2ToString("f2");
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						OrderId = order.OrderId,
						StatusText = EnumDescription.GetEnumDescription((Enum)(object)order.OrderStatus, 0).Replace("待发货", "待消费"),
						Status = (int)order.OrderStatus,
						ItemStatus = (int)order.ItemStatus,
						ItemStatusText = EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0),
						OrderDate = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
						OrderAmount = order.GetAmount(false).F2ToString("f2"),
						OrderTotal = order.GetTotal(false).F2ToString("f2"),
						PaymentType = order.PaymentType,
						CouponAmount = ((order.CouponValue > decimal.Zero) ? "-" : "") + order.CouponValue.F2ToString("f2"),
						CouponName = order.CouponName,
						DeductionPoints = order.DeductionPoints,
						DeductionMoney = text,
						RefundAmount = ((order.RefundAmount > decimal.Zero) ? "-" : "") + order.RefundAmount.F2ToString("f2"),
						Remark = order.Remark,
						LineItems = from d in order.LineItems.Keys
						select new
						{
							Status = order.LineItems[d].Status,
							StatusText = order.LineItems[d].StatusText,
							Id = order.LineItems[d].SkuId,
							Name = order.LineItems[d].ItemDescription,
							Price = order.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
							Amount = order.LineItems[d].ShipmentQuantity,
							Image = this.GetImageFullPath(order.LineItems[d].ThumbnailsUrl),
							SkuText = order.LineItems[d].SKUContent,
							ProductId = order.LineItems[d].ProductId,
							PromotionName = order.LineItems[d].PromotionName,
							VerificationItems = from f in order.LineItems[d].VerificationItems
							where f.VerificationDate.HasValue && f.VerificationDate.Value.ToString("yyyy-MM-dd HH:mm:ss") == VerificationDate
							select f into v
							select new
							{
								VerificationPassword = v.VerificationPassword,
								UserName = v.UserName,
								VerificationDate = v.VerificationDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
							}
						},
						InputItems = from g in (from t in order.InputItems
						group t by t.InputFieldGroup).ToDictionary((IGrouping<int, OrderInputItemInfo> t) => t.Key, (IGrouping<int, OrderInputItemInfo> t) => t.ToList())
						select new
						{
							InputFieldGroup = from s in g.Value
							select new
							{
								InputFieldTitle = s.InputFieldTitle,
								InputFieldType = s.InputFieldType,
								InputFieldValue = this.GetOrderInputItemValue(s.InputFieldType, s.InputFieldValue)
							}
						}
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
		}

		private string GetOrderInputItemValue(int vtype, string v)
		{
			string text = v;
			if (vtype == 6.GetHashCode() && !string.IsNullOrWhiteSpace(text))
			{
				string[] array = text.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrWhiteSpace(array[i]))
					{
						array[i] = this.GetImageFullPath(array[i]);
					}
				}
				text = string.Join(",", array);
			}
			return text;
		}

		private void OrderVerification(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			ManagerInfo managerInfoBySessionId = this.GetManagerInfoBySessionId(context);
			string text = context.Request["VerificationItems"];
			string[] array = text.Split(',');
			DateTime now = DateTime.Now;
			string text2 = "";
			OrderVerificationItemInfo orderVerificationItemInfo = null;
			decimal num = default(decimal);
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					OrderVerificationItemInfo verificationInfoByPassword = OrderHelper.GetVerificationInfoByPassword(array[i]);
					if (i == 0)
					{
						orderVerificationItemInfo = verificationInfoByPassword;
						orderVerificationItemInfo.VerificationDate = DateTime.Now;
					}
					if (verificationInfoByPassword == null)
					{
						context.Response.Write(this.GetErrorJosn(801, "该核销码无效，请重新输入"));
						return;
					}
					if (verificationInfoByPassword.StoreId != storeIdBySessionId)
					{
						context.Response.Write(this.GetErrorJosn(802, "非本门店核销码，请买家核对信息"));
						return;
					}
					if (verificationInfoByPassword.VerificationStatus == 1)
					{
						context.Response.Write(this.GetErrorJosn(801, "该核销码 于" + verificationInfoByPassword.VerificationDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "已核销"));
						return;
					}
					if (verificationInfoByPassword.VerificationStatus == 3)
					{
						context.Response.Write(this.GetErrorJosn(801, "核销码已过期，无法核销"));
						return;
					}
					if (verificationInfoByPassword.VerificationStatus == 5)
					{
						context.Response.Write(this.GetErrorJosn(801, "此核销码已进行售后，无法核销"));
						return;
					}
					if (verificationInfoByPassword.VerificationStatus == 4)
					{
						context.Response.Write(this.GetErrorJosn(801, "此核销码正在进行售后，无法核销"));
						return;
					}
					verificationInfoByPassword.VerificationStatus = 1;
					verificationInfoByPassword.VerificationDate = now;
					verificationInfoByPassword.ManagerId = managerInfoBySessionId.ManagerId;
					verificationInfoByPassword.UserName = managerInfoBySessionId.UserName;
					OrderHelper.UpdateVerificationItem(verificationInfoByPassword);
					text2 = verificationInfoByPassword.OrderId;
				}
				WXAppletFormDataInfo wxFormData = WeChartAppletHelper.GetWxFormData(WXAppletEvent.ServiceProductValid, text2);
				if (wxFormData != null)
				{
					wxFormData.EventValue += now.ToString("yyyyMMddHHmmss");
					new WeChatAppletDao().Update(wxFormData, null);
				}
			}
			OrderInfo serviceProductOrderInfo = OrderHelper.GetServiceProductOrderInfo(text2);
			if (serviceProductOrderInfo != null)
			{
				num = serviceProductOrderInfo.GetTotal(false) / (decimal)serviceProductOrderInfo.GetBuyQuantity() * (decimal)array.Length;
				MemberInfo user = Users.GetUser(serviceProductOrderInfo.UserId);
				string storeName = "";
				string productName = "";
				if (serviceProductOrderInfo.StoreId > 0)
				{
					storeName = DepotHelper.GetStoreNameByStoreId(serviceProductOrderInfo.StoreId);
				}
				if (serviceProductOrderInfo.LineItems != null && serviceProductOrderInfo.LineItems.Count > 0)
				{
					productName = serviceProductOrderInfo.LineItems.Values.FirstOrDefault().ItemDescription;
				}
				Messenger.ServiceOrderValidSuccess(orderVerificationItemInfo, user, serviceProductOrderInfo, productName, storeName, text, num);
				if (OrderHelper.IsVerificationFinished(text2) && serviceProductOrderInfo.ItemStatus == OrderItemStatus.Nomarl)
				{
					serviceProductOrderInfo.OrderStatus = OrderStatus.Finished;
					serviceProductOrderInfo.FinishDate = DateTime.Now;
					TradeHelper.UpdateOrderInfo(serviceProductOrderInfo);
				}
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "SUCCESS",
					VerificationDate = now.ToString("yyyy-MM-dd HH:mm:ss")
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetChoiceProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["pageIndex"].ToInt(0);
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num < 1)
			{
				num = 1;
			}
			if (num2 < 1)
			{
				num2 = 10;
			}
			DbQueryResult showProductList = WeChartAppletHelper.GetShowProductList(0, num, num2, storeIdBySessionId, ProductType.ServiceProduct);
			DataTable data = showProductList.Data;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = showProductList.TotalRecords,
					List = from d in data.AsEnumerable()
					select new
					{
						ProductId = d.Field<int>("ProductId"),
						ProductName = d.Field<string>("ProductName"),
						SalePrice = d.Field<decimal>("SalePrice").F2ToString("f2"),
						ThumbnailUrl160 = Globals.FullPath(d.Field<string>("ThumbnailUrl410")),
						MarketPrice = d.Field<decimal>("MarketPrice").F2ToString("f2"),
						HasSKU = d.Field<bool>("HasSKU"),
						SkuId = d.Field<string>("SkuId"),
						Stock = d.Field<int>("StoreStock")
					}
				}
			});
			context.Response.Write(s);
		}

		public void AddChoiceProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string idList = context.Request["ProductIds"].ToNullString();
			idList = Globals.GetSafeIDList(idList, ',', true);
			if (string.IsNullOrEmpty(idList))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				WeChartAppletHelper.AddChoiceProdcut(idList, storeIdBySessionId);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void RemoveChoiceProduct(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string idList = context.Request["ProductIds"].ToNullString();
			idList = Globals.GetSafeIDList(idList, ',', true);
			if (string.IsNullOrEmpty(idList))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				WeChartAppletHelper.RemoveChoiceProduct(idList, storeIdBySessionId);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void ProcessAppInit(HttpContext context)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string text = context.Request["version"];
			int num = context.Request["type"].ToInt(0);
			if (num != 1 && num != 2)
			{
				num = 1;
			}
			decimal d = default(decimal);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Paramter_Error, 0)));
			}
			else if (!decimal.TryParse(text, out d))
			{
				context.Response.Write(this.GetErrorJosn(101, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Paramter_Error, 0)));
			}
			else
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Version = siteSettings.StoreAppVersion,
						ExistNew = (siteSettings.StoreAppVersion > d),
						Forcible = true,
						Description = siteSettings.StoreAppDescription.ToNullString(),
						UpgradeUrl = ((num == 2) ? Globals.FullPath(siteSettings.StoreAppAndroidUrl) : Globals.FullPath(siteSettings.StoreAppIOSUrl))
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void BalanceRequestList(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			StoreBalanceDrawRequestQuery storeBalanceDrawRequestQuery = new StoreBalanceDrawRequestQuery();
			int num2 = context.Request["PageIndex"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 1;
			}
			int num3 = context.Request["PageSize"].ToInt(0);
			if (num3 < 1)
			{
				num3 = 10;
			}
			storeBalanceDrawRequestQuery.PageIndex = num2;
			storeBalanceDrawRequestQuery.PageSize = num3;
			storeBalanceDrawRequestQuery.SortBy = "RequestTime";
			storeBalanceDrawRequestQuery.SortOrder = SortAction.Desc;
			storeBalanceDrawRequestQuery.StoreId = storeInfoBySessionId.StoreId;
			PageModel<StoreBalanceDrawRequestInfo> balanceDrawRequests = StoreBalanceHelper.GetBalanceDrawRequests(storeBalanceDrawRequestQuery, false);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = balanceDrawRequests.Total,
					List = balanceDrawRequests.Models.Select(delegate(StoreBalanceDrawRequestInfo b)
					{
						DateTime dateTime = b.RequestTime;
						string requestTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						decimal amount = b.Amount.F2ToString("f2").ToDecimal(0);
						string drawType = (b.IsAlipay.HasValue && b.IsAlipay.Value) ? "支付宝" : "银行卡";
						string statusText = b.IsPass.HasValue ? (b.IsPass.Value ? "已通过审核" : "拒绝") : "审核中";
						string requestError = b.RequestError;
						object accountDate;
						if (!b.AccountDate.HasValue)
						{
							accountDate = "";
						}
						else
						{
							dateTime = b.AccountDate.Value;
							accountDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						}
						return new
						{
							RequestTime = requestTime,
							Amount = amount,
							DrawType = drawType,
							StatusText = statusText,
							RequestError = requestError,
							AccountDate = (string)accountDate,
							Id = b.Id,
							ManagerRemark = b.ManagerRemark
						};
					})
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void ApplyBalanceRequest(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			if (string.IsNullOrEmpty(storeInfoBySessionId.TradePassword))
			{
				context.Response.Write(this.GetErrorJosn(522, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.TradePassword_NoSet, 0)));
				context.Response.End();
			}
			string pass = context.Request["Password"].ToNullString();
			if (storeInfoBySessionId.TradePassword != Users.EncodePassword(pass, storeInfoBySessionId.TradePasswordSalt))
			{
				context.Response.Write(this.GetErrorJosn(521, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.TradePassword_Error, 0)));
				context.Response.End();
			}
			int num2 = context.Request["CardType"].ToInt(0);
			if (num2 != 1 && num2 != 2)
			{
				context.Response.Write(this.GetErrorJosn(133, "帐号类型" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.ValueUndefined, 0)));
				context.Response.End();
			}
			if (num2 == 1 && !HiContext.Current.SiteSettings.EnableBulkPaymentAliPay)
			{
				context.Response.Write(this.GetErrorJosn(134, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.PlatNotOpenAlipayDraw, 0)));
				context.Response.End();
			}
			if (num2 == 1 && (string.IsNullOrEmpty(storeInfoBySessionId.AlipayAccount) || string.IsNullOrEmpty(storeInfoBySessionId.AlipayRealName)))
			{
				context.Response.Write(this.GetErrorJosn(523, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.StoreNotBindAlipayInfo, 0)));
				context.Response.End();
			}
			if (num2 == 2 && (string.IsNullOrEmpty(storeInfoBySessionId.BankAccountName) || string.IsNullOrEmpty(storeInfoBySessionId.BankCardNo) || string.IsNullOrEmpty(storeInfoBySessionId.BankName)))
			{
				context.Response.Write(this.GetErrorJosn(524, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.StoreNotBindBankCardInfo, 0)));
				context.Response.End();
			}
			decimal num3 = context.Request["RequestAmount"].ToDecimal(0);
			if (num3 <= decimal.Zero)
			{
				context.Response.Write(this.GetErrorJosn(526, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.RequestAmountError, 0)));
				context.Response.End();
			}
			if (num3 > storeInfoBySessionId.Balance)
			{
				context.Response.Write(this.GetErrorJosn(525, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.BalanceNotEnough, 0)));
				context.Response.End();
			}
			string text = Globals.StripAllTags(context.Request["Remark"].ToNullString());
			if (text.Length > 2000)
			{
				text = text.Substring(0, 2000);
			}
			StoreBalanceDrawRequestInfo storeBalanceDrawRequestInfo = new StoreBalanceDrawRequestInfo();
			if (num2 == 1)
			{
				storeBalanceDrawRequestInfo.AlipayCode = storeInfoBySessionId.AlipayAccount;
				storeBalanceDrawRequestInfo.AlipayRealName = storeInfoBySessionId.AlipayRealName;
				storeBalanceDrawRequestInfo.IsAlipay = true;
			}
			else
			{
				storeBalanceDrawRequestInfo.BankName = storeInfoBySessionId.BankName;
				storeBalanceDrawRequestInfo.AccountName = storeInfoBySessionId.BankAccountName;
				storeBalanceDrawRequestInfo.MerchantCode = storeInfoBySessionId.BankCardNo;
				storeBalanceDrawRequestInfo.IsAlipay = false;
			}
			storeBalanceDrawRequestInfo.RequestState = 1.ToString();
			storeBalanceDrawRequestInfo.Remark = text;
			storeBalanceDrawRequestInfo.Amount = num3;
			storeBalanceDrawRequestInfo.RequestTime = DateTime.Now;
			storeBalanceDrawRequestInfo.StoreId = storeInfoBySessionId.StoreId;
			if (StoreBalanceHelper.BalanceDrawRequest(storeBalanceDrawRequestInfo))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = "申请提现成功"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
			}
		}

		public void BalanceOrderDetail(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			string text = context.Request["OrderId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(104, "订单ID" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Empty_Error, 0)));
				context.Response.End();
			}
			bool flag = true;
			if (!string.IsNullOrEmpty(context.Request["IsBalanceOver"].ToNullString()))
			{
				flag = context.Request["IsBalanceOver"].ToBool();
			}
			StoreBalanceOrderInfo storeBalanceOrderInfo = StoreBalanceHelper.GetBalanceOrderDetail(text, flag);
			if (storeBalanceOrderInfo == null)
			{
				storeBalanceOrderInfo = new StoreBalanceOrderInfo();
			}
			string orderId = storeBalanceOrderInfo.OrderId;
			decimal overBalance = (flag ? storeBalanceOrderInfo.OverBalance.F2ToString("f2").ToDecimal(0) : storeBalanceOrderInfo.GetShouldOverBalance(storeInfoBySessionId.CommissionRate)).F2ToString("f2").ToDecimal(0);
			DateTime dateTime = storeBalanceOrderInfo.OrderDate;
			string orderDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			object overBalanceDate;
			if (!flag)
			{
				overBalanceDate = "";
			}
			else
			{
				dateTime = storeBalanceOrderInfo.OverBalanceDate;
				overBalanceDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					OrderId = orderId,
					OverBalance = overBalance,
					OrderDate = orderDate,
					OverBalanceDate = (string)overBalanceDate,
					OrderTotal = storeBalanceOrderInfo.OrderTotal.F2ToString("f2").ToDecimal(0),
					Freight = storeBalanceOrderInfo.Freight.F2ToString("f2").ToDecimal(0),
					DeductionMoney = storeBalanceOrderInfo.DeductionMoney.F2ToString("f2").ToDecimal(0),
					CouponValue = storeBalanceOrderInfo.CouponValue.F2ToString("f2").ToDecimal(0),
					RefundAmount = storeBalanceOrderInfo.RefundAmount.F2ToString("f2").ToDecimal(0),
					PlatCommission = (flag ? storeBalanceOrderInfo.PlatCommission.F2ToString("f2").ToDecimal(0) : storeBalanceOrderInfo.GetPlatCommission(storeInfoBySessionId.CommissionRate).F2ToString("f2").ToDecimal(0))
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void OverBalanceOrderList(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			StoreBalanceOrderQuery storeBalanceOrderQuery = new StoreBalanceOrderQuery();
			storeBalanceOrderQuery.IsBalanceOver = true;
			int num2 = context.Request["PageIndex"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 1;
			}
			int num3 = context.Request["PageSize"].ToInt(0);
			if (num3 < 1)
			{
				num3 = 10;
			}
			storeBalanceOrderQuery.PageIndex = num2;
			storeBalanceOrderQuery.PageSize = num3;
			storeBalanceOrderQuery.SortBy = "OrderDate";
			storeBalanceOrderQuery.SortOrder = SortAction.Desc;
			storeBalanceOrderQuery.StoreId = storeInfoBySessionId.StoreId;
			DateTime? endDate = null;
			DateTime? startDate = null;
			DateTime? overStartDate = null;
			DateTime? overEndDate = null;
			if (!string.IsNullOrEmpty(context.Request["CreateEndTime"].ToNullString()) && context.Request["CreateEndTime"].ToNullString() != "null")
			{
				endDate = context.Request["CreateEndTime"].ToDateTime().Value;
			}
			if (!string.IsNullOrEmpty(context.Request["CreateStartTime"].ToNullString()) && context.Request["CreateStartTime"].ToNullString() != "null")
			{
				startDate = context.Request["CreateStartTime"].ToDateTime().Value;
			}
			if (endDate.HasValue && startDate.HasValue && startDate.Value > endDate.Value)
			{
				context.Response.Write(this.GetErrorJosn(101, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Paramter_Error, 0) + "创建时间结束时间必须大于开始时间"));
				context.Response.End();
			}
			storeBalanceOrderQuery.StartDate = startDate;
			storeBalanceOrderQuery.EndDate = endDate;
			if (!string.IsNullOrEmpty(context.Request["OverEndTime"].ToNullString()) && context.Request["OverEndTime"].ToNullString() != "null")
			{
				overEndDate = context.Request["OverEndTime"].ToDateTime().Value;
			}
			if (!string.IsNullOrEmpty(context.Request["OverStartTime"].ToNullString()) && context.Request["OverStartTime"].ToNullString() != "null")
			{
				overStartDate = context.Request["OverStartTime"].ToDateTime().Value;
			}
			if (overEndDate.HasValue && overStartDate.HasValue && overStartDate.Value > overEndDate.Value)
			{
				context.Response.Write(this.GetErrorJosn(101, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Paramter_Error, 0) + "结算时间结束时间必须大于开始时间"));
				context.Response.End();
			}
			storeBalanceOrderQuery.OverStartDate = overStartDate;
			storeBalanceOrderQuery.OverEndDate = overEndDate;
			string text2 = storeBalanceOrderQuery.OrderId = context.Request["OrderId"].ToNullString();
			decimal overBalanceOrdersTotal = StoreBalanceHelper.GetOverBalanceOrdersTotal(storeBalanceOrderQuery);
			PageModel<StoreBalanceOrderInfo> balanceOrders = StoreBalanceHelper.GetBalanceOrders(storeBalanceOrderQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					BalanceTotal = overBalanceOrdersTotal.F2ToString("f2").ToDecimal(0),
					RecordCount = balanceOrders.Total,
					List = from b in balanceOrders.Models
					select new
					{
						OrderId = b.OrderId,
						OrderDate = b.OrderDate.ToString("yyyy-MM-dd HH:mm"),
						OverBalance = b.OverBalance.F2ToString("f2").ToDecimal(0)
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void NotOverBalanceOrderList(HttpContext context)
		{
			int num = 0;
			StoresInfo store = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			StoreBalanceOrderQuery storeBalanceOrderQuery = new StoreBalanceOrderQuery();
			storeBalanceOrderQuery.IsBalanceOver = false;
			int num2 = context.Request["PageIndex"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 1;
			}
			int num3 = context.Request["PageSize"].ToInt(0);
			if (num3 < 1)
			{
				num3 = 10;
			}
			storeBalanceOrderQuery.PageIndex = num2;
			storeBalanceOrderQuery.PageSize = num3;
			storeBalanceOrderQuery.SortBy = "OrderDate";
			storeBalanceOrderQuery.SortOrder = SortAction.Desc;
			string text2 = storeBalanceOrderQuery.OrderId = context.Request["OrderId"].ToNullString();
			storeBalanceOrderQuery.StoreId = store.StoreId;
			decimal notOverBalanceOrdersTotal = StoreBalanceHelper.GetNotOverBalanceOrdersTotal(storeBalanceOrderQuery, store.CommissionRate);
			PageModel<StoreBalanceOrderInfo> balanceOrders = StoreBalanceHelper.GetBalanceOrders(storeBalanceOrderQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					BalanceTotal = notOverBalanceOrdersTotal.F2ToString("f2").ToDecimal(0),
					RecordCount = balanceOrders.Total,
					List = from b in balanceOrders.Models
					select new
					{
						OrderId = b.OrderId,
						OrderDate = b.OrderDate.ToString("yyyy-MM-dd HH:mm"),
						OverBalance = b.GetShouldOverBalance(store.CommissionRate).F2ToString("f2").ToDecimal(0),
						IsServiceOrder = (b.OrderType == OrderType.ServiceOrder).ToString()
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void BalanceDetailList(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			StoreBalanceDetailQuery storeBalanceDetailQuery = new StoreBalanceDetailQuery();
			int num2 = context.Request["PageIndex"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 1;
			}
			int num3 = context.Request["PageSize"].ToInt(0);
			if (num3 < 1)
			{
				num3 = 10;
			}
			storeBalanceDetailQuery.PageIndex = num2;
			storeBalanceDetailQuery.PageSize = num3;
			storeBalanceDetailQuery.SortBy = "CreateTime";
			storeBalanceDetailQuery.SortOrder = SortAction.Desc;
			storeBalanceDetailQuery.StoreId = storeInfoBySessionId.StoreId;
			PageModel<StoreBalanceDetailInfo> balanceDetails = StoreBalanceHelper.GetBalanceDetails(storeBalanceDetailQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Balance = storeInfoBySessionId.Balance.F2ToString("f2").ToDecimal(0),
					RecordCount = balanceDetails.Total,
					List = from b in balanceDetails.Models
					select new
					{
						JournalNumber = b.JournalNumber,
						Income = (b.Income.HasValue ? b.Income.Value.F2ToString("f2").ToDecimal(0) : decimal.Zero),
						Expenses = (b.Expenses.HasValue ? b.Expenses.Value.F2ToString("f2").ToDecimal(0) : decimal.Zero),
						TradeType = b.TradeType,
						TradeTypeText = b.TradeTypeText + ((b.TradeType == StoreTradeTypes.OrderBalance) ? ("(" + b.TradeNo + ")") : ""),
						TradeDate = b.TradeDate.ToString("yyyy-MM-dd HH:mm")
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void StoreBalanceStatistics(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			StoreBalanceInfo storeBalance = StoreBalanceHelper.GetStoreBalance(storeInfoBySessionId.StoreId, storeInfoBySessionId.CommissionRate);
			decimal num2 = storeBalance.Balance - storeBalance.BalanceForzen;
			if (num2 < decimal.Zero)
			{
				num2 = default(decimal);
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Balance = num2.F2ToString("f2").ToDecimal(0),
					BalanceForzen = storeBalance.BalanceForzen.F2ToString("f2").ToDecimal(0),
					BalanceOut = storeBalance.BalanceOut.F2ToString("f2").ToDecimal(0),
					FinishOrderBalance = storeBalance.FinishOrderBalance.F2ToString("f2").ToDecimal(0),
					NoFinishOrderBalance = storeBalance.NoFinishOrderBalance.F2ToString("f2").ToDecimal(0),
					IsRequestBlance = storeInfoBySessionId.IsRequestBlance
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void SaveStoreUserBaseInfo(HttpContext context)
		{
			ManagerInfo managerInfoBySessionId = this.GetManagerInfoBySessionId(context);
			string text = context.Request["HeadImage"].ToNullString();
			if (!string.IsNullOrEmpty(text))
			{
				managerInfoBySessionId.HeadImage = text.ToLower().Replace(Globals.HostPath(context.Request.Url).ToLower(), "").Replace("thum_", "")
					.TrimEnd(',');
			}
			else
			{
				managerInfoBySessionId.HeadImage = "";
			}
			string text2 = Globals.StripAllTags(context.Request["ContactInfo"].ToNullString());
			if (text2.Length > 256)
			{
				context.Response.Write(this.GetErrorJosn(99, "联系信息（手机）" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Paramter_LongerOrShort, 0)));
				context.Response.End();
			}
			managerInfoBySessionId.ContactInfo = text2;
			if (ManagerHelper.Update(managerInfoBySessionId))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = "更新成功"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
			}
		}

		private void GetStoreUserBaseInfo(HttpContext context)
		{
			ManagerInfo managerInfoBySessionId = this.GetManagerInfoBySessionId(context);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					HeadImage = (string.IsNullOrEmpty(managerInfoBySessionId.HeadImage.ToNullString()) ? "" : Globals.FullPath(managerInfoBySessionId.HeadImage.ToNullString())),
					UserName = managerInfoBySessionId.UserName,
					ContactInfo = managerInfoBySessionId.ContactInfo
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetDrawCardInfo(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					IsOpenAlipayDraw = siteSettings.EnableBulkPaymentAliPay,
					AlipayAccount = storeInfoBySessionId.AlipayAccount,
					AlipayRealName = storeInfoBySessionId.AlipayRealName,
					BankName = storeInfoBySessionId.BankName,
					BankAccountName = storeInfoBySessionId.BankAccountName,
					BankCardNo = storeInfoBySessionId.BankCardNo
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void BindDrawCardInfo(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			if (string.IsNullOrEmpty(storeInfoBySessionId.TradePassword))
			{
				context.Response.Write(this.GetErrorJosn(522, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.TradePassword_NoSet, 0)));
				context.Response.End();
			}
			string pass = context.Request["Password"].ToNullString();
			if (storeInfoBySessionId.TradePassword != Users.EncodePassword(pass, storeInfoBySessionId.TradePasswordSalt))
			{
				context.Response.Write(this.GetErrorJosn(521, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.TradePassword_Error, 0)));
				context.Response.End();
			}
			int num2 = context.Request["CardType"].ToInt(0);
			if (num2 != 1 && num2 != 2)
			{
				context.Response.Write(this.GetErrorJosn(133, "帐号类型" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.ValueUndefined, 0)));
				context.Response.End();
			}
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			if (num2 == 1)
			{
				text = context.Request["AlipayAccount"].ToNullString();
				text2 = context.Request["AlipayRealName"].ToNullString();
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
				{
					context.Response.Write(this.GetErrorJosn(104, "支付宝帐号和真实姓名" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Empty_Error, 0)));
					context.Response.End();
				}
				storeInfoBySessionId.AlipayAccount = text;
				storeInfoBySessionId.AlipayRealName = text2;
			}
			else
			{
				text3 = context.Request["BankName"].ToNullString();
				text4 = context.Request["BankAccountName"].ToNullString();
				text5 = context.Request["BankCardNo"].ToNullString();
				if (string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4) || string.IsNullOrEmpty(text5))
				{
					context.Response.Write(this.GetErrorJosn(104, "银行卡帐号" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Empty_Error, 0)));
					context.Response.End();
				}
				storeInfoBySessionId.BankName = text3;
				storeInfoBySessionId.BankAccountName = text4;
				storeInfoBySessionId.BankCardNo = text5;
			}
			if (StoresHelper.UpdateStore(storeInfoBySessionId))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = "更新成功"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
			}
		}

		private void ValidTradePassword(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			if (string.IsNullOrEmpty(storeInfoBySessionId.TradePassword))
			{
				context.Response.Write(this.GetErrorJosn(522, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.TradePassword_NoSet, 0)));
				context.Response.End();
			}
			string pass = context.Request["Password"].ToNullString();
			if (storeInfoBySessionId.TradePassword == Users.EncodePassword(pass, storeInfoBySessionId.TradePasswordSalt))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = "验证成功"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(521, ((Enum)(object)ApiErrorCode.TradePassword_Error).ToDescription()));
			}
		}

		private void ChangeTradePassword(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			string pass = context.Request["oldPassword"].ToNullString();
			string text = context.Request["newPassword"].ToNullString();
			string b = context.Request["rePassword"].ToNullString();
			if (storeInfoBySessionId.TradePassword != Users.EncodePassword(pass, storeInfoBySessionId.TradePasswordSalt))
			{
				context.Response.Write(this.GetErrorJosn(521, ((Enum)(object)ApiErrorCode.TradePassword_Error).ToDescription()));
			}
			else if (string.IsNullOrEmpty(text) || text.Length < 6)
			{
				context.Response.Write(this.GetErrorJosn(212, ((Enum)(object)ApiErrorCode.Password_Error).ToDescription()));
			}
			else if (text != b)
			{
				context.Response.Write(this.GetErrorJosn(213, ((Enum)(object)ApiErrorCode.RePasswordNoEqualsPassword).ToDescription()));
			}
			else
			{
				storeInfoBySessionId.TradePassword = Users.EncodePassword(text, storeInfoBySessionId.TradePasswordSalt);
				if (StoresHelper.UpdateStore(storeInfoBySessionId))
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS",
							Msg = ""
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
				}
			}
		}

		private void SetTradePassword(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			if (!string.IsNullOrEmpty(storeInfoBySessionId.TradePassword))
			{
				context.Response.Write(this.GetErrorJosn(519, ((Enum)(object)ApiErrorCode.TradePasswordAlreadySet).ToDescription()));
			}
			else
			{
				string text = context.Request["password"].ToNullString();
				string b = context.Request["rePassword"].ToNullString();
				if (text != b)
				{
					context.Response.Write(this.GetErrorJosn(213, ((Enum)(object)ApiErrorCode.RePasswordNoEqualsPassword).ToDescription()));
				}
				else if (string.IsNullOrEmpty(text) || text.Length < 6)
				{
					context.Response.Write(this.GetErrorJosn(212, ((Enum)(object)ApiErrorCode.Password_Error).ToDescription()));
				}
				else
				{
					storeInfoBySessionId.TradePasswordSalt = Globals.RndStr(128, true);
					storeInfoBySessionId.TradePassword = Users.EncodePassword(text, storeInfoBySessionId.TradePasswordSalt);
					if (StoresHelper.UpdateStore(storeInfoBySessionId))
					{
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "SUCCESS",
								Msg = ""
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
					else
					{
						context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
					}
				}
			}
		}

		private void StoreAbilityStatistics(HttpContext context)
		{
			int shoppingGuiderId = 0;
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out shoppingGuiderId);
			int num = context.Request["TimeScope"].ToInt(0);
			if (num != 1 && num != 2 && num != 3)
			{
				num = 1;
			}
			DateTime now = DateTime.Now;
			DateTime now2 = DateTime.Now;
			DateTime dateTime;
			switch (num)
			{
			case 1:
				dateTime = DateTime.Now;
				now = dateTime.Date;
				break;
			case 2:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-6.0);
				now = dateTime.Date;
				break;
			default:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-89.0);
				now = dateTime.Date;
				break;
			}
			StoreAbilityStatisticsInfo abilityStatisticsInfo = StoresHelper.GetAbilityStatisticsInfo(storeIdBySessionId, shoppingGuiderId, now, now2);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					SaleQuantity = abilityStatisticsInfo.SaleQuantity,
					PayOrderCount = abilityStatisticsInfo.PayNoRefundOrderCount,
					JointRate = abilityStatisticsInfo.JointRate.F2ToString("f2").ToDecimal(0),
					UnitPrice = abilityStatisticsInfo.UnitPrice.F2ToString("f2").ToDecimal(0),
					GuestUnitPrice = abilityStatisticsInfo.GuestUnitPrice.F2ToString("f2").ToDecimal(0),
					MemberCount = abilityStatisticsInfo.MemberCount
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void SaleAchievementData(HttpContext context)
		{
			int num = 0;
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out num);
			int num2 = context.Request["TimeScope"].ToInt(0);
			if (num2 != 1 && num2 != 2 && num2 != 3)
			{
				num2 = 1;
			}
			DateTime now = DateTime.Now;
			DateTime now2 = DateTime.Now;
			DateTime dateTime;
			switch (num2)
			{
			case 1:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-6.0);
				now = dateTime.Date;
				break;
			case 2:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-29.0);
				now = dateTime.Date;
				break;
			default:
				dateTime = DateTime.Now;
				dateTime = dateTime.AddDays(-89.0);
				now = dateTime.Date;
				break;
			}
			IList<StoreDaySaleAmountModel> saleAmountOfDay = StoresHelper.GetSaleAmountOfDay(storeIdBySessionId, now, now2);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					SaleAmount = saleAmountOfDay.Sum((StoreDaySaleAmountModel sa) => sa.SaleAmount).F2ToString("f2").ToDecimal(0),
					TopAmount = saleAmountOfDay.Max((StoreDaySaleAmountModel sa) => sa.SaleAmount).F2ToString("f2").ToDecimal(0),
					DayAverageAmount = saleAmountOfDay.Average((StoreDaySaleAmountModel sa) => sa.SaleAmount),
					AchievementData = from d in saleAmountOfDay
					select new
					{
						SaleAmount = d.SaleAmount.F2ToString("f2").ToDecimal(0),
						Date = d.OrderDate.ToString("yyyy-MM-dd")
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void TodaySaleStatistics(HttpContext context)
		{
			int num = 0;
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out num);
			StoreSalesStatisticsModel todaySaleStatistics = StoresHelper.GetTodaySaleStatistics(storeIdBySessionId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					SaleAmount = todaySaleStatistics.SaleAmount.F2ToString("f2").ToDecimal(0),
					Views = todaySaleStatistics.Views,
					OrderCount = todaySaleStatistics.OrderCount,
					PayOrderCount = todaySaleStatistics.PayOrderCount
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void CloseStoreSet(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			if (num > 0)
			{
				context.Response.Write(this.GetErrorJosn(132, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NoEnoughRight, 0)));
				context.Response.End();
			}
			bool flag = context.Request["IsOpenSet"].ToBool();
			storeInfoBySessionId.CloseStatus = !flag;
			if (flag)
			{
				if (string.IsNullOrEmpty(context.Request["EndTime"].ToNullString()) || string.IsNullOrEmpty(context.Request["StartTime"].ToNullString()) || context.Request["StartTime"].ToNullString().ToLower() == "null" || context.Request["EndTime"].ToNullString().ToLower() == "null")
				{
					context.Response.Write(this.GetErrorJosn(102, "时间" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Format_Eroor, 0)));
					context.Response.End();
				}
				DateTime value = context.Request["EndTime"].ToDateTime().Value;
				DateTime value2 = context.Request["StartTime"].ToDateTime().Value;
				if (value < value2)
				{
					context.Response.Write(this.GetErrorJosn(101, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Paramter_Error, 0) + "结束时间必须大于开始时间"));
					context.Response.End();
				}
				storeInfoBySessionId.CloseBeginTime = value2;
				storeInfoBySessionId.CloseEndTime = value;
			}
			if (StoresHelper.UpdateStore(storeInfoBySessionId))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = ""
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
			}
		}

		private void GetCloseStoreSetInfo(HttpContext context)
		{
			int num = 0;
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context, out num);
			bool flag = !storeInfoBySessionId.CloseStatus;
			bool isOpenSet = flag;
			object closeStartTime;
			DateTime value;
			if (!storeInfoBySessionId.CloseBeginTime.HasValue)
			{
				closeStartTime = "";
			}
			else
			{
				value = storeInfoBySessionId.CloseBeginTime.Value;
				closeStartTime = value.ToString("yyyy-MM-dd HH:mm");
			}
			object closeEndTime;
			if (!storeInfoBySessionId.CloseBeginTime.HasValue)
			{
				closeEndTime = "";
			}
			else
			{
				value = storeInfoBySessionId.CloseEndTime.Value;
				closeEndTime = value.ToString("yyyy-MM-dd HH:mm");
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					IsOpenSet = isOpenSet,
					CloseStartTime = (string)closeStartTime,
					CloseEndTime = (string)closeEndTime
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void UpdateStoreUserInfo(HttpContext context)
		{
			int num = 0;
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out num);
			int num2 = context.Request["UserId"].ToInt(0);
			if (num2 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(208, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NotExistUser, 0)));
				context.Response.End();
			}
			MemberInfo user = Users.GetUser(num2);
			if (user == null || user.UserId == 0)
			{
				context.Response.Write(this.GetErrorJosn(208, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NotExistUser, 0)));
				context.Response.End();
			}
			if (user.StoreId != storeIdBySessionId || (num > 0 && user.ShoppingGuiderId != num))
			{
				context.Response.Write(this.GetErrorJosn(210, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.UserNotRelationStoreOrSupShoppingGuider, 0)));
				context.Response.End();
			}
			string text = Globals.StripAllTags(context.Request["ContactInfo"].ToNullString());
			if (!string.IsNullOrEmpty(text))
			{
				user.ContactInfo = text;
			}
			string text2 = context.Request["TrueName"].ToNullString();
			if (!string.IsNullOrEmpty(text2))
			{
				user.RealName = text2;
			}
			string text3 = context.Request["Birthday"].ToNullString();
			DateTime? nullable = null;
			if (!string.IsNullOrEmpty(text3))
			{
				nullable = text3.ToDateTime();
				if (!nullable.HasValue)
				{
					context.Response.Write(this.GetErrorJosn(102, "出生日期" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Format_Eroor, 0)));
					context.Response.End();
				}
				else
				{
					user.BirthDate = nullable.Value;
				}
			}
			string text4 = context.Request["UserSex"].ToNullString();
			if (string.IsNullOrEmpty(text4) || (text4 != "男" && text4 != "女"))
			{
				context.Response.Write(this.GetErrorJosn(102, "请选择性别"));
				context.Response.End();
			}
			if (!string.IsNullOrEmpty(text4))
			{
				user.Gender = ((text4 == "男") ? Gender.Male : Gender.Female);
			}
			string text5 = Globals.StripAllTags(context.Request["nickname"].ToNullString());
			if (!string.IsNullOrEmpty(text5))
			{
				user.NickName = text5;
			}
			if (MemberProcessor.UpdateMember(user))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = ""
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
			}
		}

		private void GetStoreUserDetails(HttpContext context)
		{
			int num = 0;
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out num);
			int num2 = context.Request["UserId"].ToInt(0);
			if (num2 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(208, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NotExistUser, 0)));
				context.Response.End();
			}
			MemberInfo user = Users.GetUser(num2);
			if (user == null || user.UserId == 0)
			{
				context.Response.Write(this.GetErrorJosn(208, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NotExistUser, 0)));
				context.Response.End();
			}
			if (user.StoreId != storeIdBySessionId || (num > 0 && user.ShoppingGuiderId != num))
			{
				context.Response.Write(this.GetErrorJosn(210, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.UserNotRelationStoreOrSupShoppingGuider, 0)));
				context.Response.End();
			}
			IList<MemberTagInfo> source = new List<MemberTagInfo>();
			if (!string.IsNullOrEmpty(user.TagIds.ToNullString()))
			{
				string text = user.TagIds.TrimStart(',').TrimEnd(',');
				if (!string.IsNullOrEmpty(text))
				{
					source = MemberTagHelper.GetTagByMember(text);
				}
			}
			MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(user.GradeId);
			string text2 = (memberGrade == null) ? "" : memberGrade.Name;
			DateTime dateTime;
			object obj;
			if (user.BirthDate.HasValue)
			{
				dateTime = user.BirthDate.Value;
				obj = dateTime.ToString("yyyy-MM-dd");
			}
			else
			{
				obj = "";
			}
			string birthday = (string)obj;
			string userName = user.UserName;
			string nickName = user.NickName;
			string realName = user.RealName;
			string cellPhone = user.CellPhone;
			string gradeName = text2;
			string contactInfo = string.IsNullOrEmpty(user.ContactInfo) ? user.CellPhone : user.ContactInfo;
			dateTime = user.CreateDate;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					UserName = userName,
					NickName = nickName,
					TrueName = realName,
					Mobbile = cellPhone,
					GradeName = gradeName,
					ContactInfo = contactInfo,
					RegisterTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
					Birthday = birthday,
					UserSex = ((user.Gender == Gender.Female) ? "女" : ((user.Gender == Gender.Male) ? "男" : "")),
					UserTags = from t in source
					select new
					{
						t.TagId,
						t.TagName
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetStoreUserConsumeList(HttpContext context)
		{
			Pagination pagination = new Pagination();
			int num = 0;
			int num2 = context.Request["PageIndex"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 1;
			}
			int num3 = context.Request["PageSize"].ToInt(0);
			if (num3 < 1)
			{
				num3 = 10;
			}
			pagination.PageIndex = num2;
			pagination.PageSize = num3;
			pagination.SortBy = "PayDate";
			pagination.SortOrder = SortAction.Desc;
			string gradeName = "";
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out num);
			int num4 = context.Request["UserId"].ToInt(0);
			if (num4 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(208, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NotExistUser, 0)));
				context.Response.End();
			}
			MemberInfo user = Users.GetUser(num4);
			if (user == null || user.UserId == 0)
			{
				context.Response.Write(this.GetErrorJosn(208, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NotExistUser, 0)));
				context.Response.End();
			}
			if (user.StoreId != storeIdBySessionId || (num > 0 && user.ShoppingGuiderId != num))
			{
				context.Response.Write(this.GetErrorJosn(210, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.UserNotRelationStoreOrSupShoppingGuider, 0)));
				context.Response.End();
			}
			IList<MemberTagInfo> source = new List<MemberTagInfo>();
			if (num2 == 1)
			{
				if (!string.IsNullOrEmpty(user.TagIds.ToNullString()))
				{
					string text = user.TagIds.TrimStart(',').TrimEnd(',');
					if (!string.IsNullOrEmpty(text))
					{
						source = MemberTagHelper.GetTagByMember(text);
					}
				}
				MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(user.GradeId);
				if (memberGrade != null)
				{
					gradeName = memberGrade.Name;
				}
			}
			MemberConsumeModel memberConsumeList = MemberProcessor.GetMemberConsumeList(pagination, num4, num2 == 1 && true);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = memberConsumeList.OrderList.Total,
					UserName = user.UserName,
					NickName = user.NickName,
					HeadImage = (string.IsNullOrEmpty(user.Picture.ToNullString()) ? Globals.FullPath("/templates/common/images/headerimg.png") : Globals.FullPath(user.Picture.ToNullString())),
					GradeName = gradeName,
					UserSex = ((user.Gender == Gender.Male) ? "男" : ((user.Gender == Gender.Female) ? "女" : "")),
					DormancyDays = memberConsumeList.DormancyDays,
					Last3MonthsConsumeTimes = memberConsumeList.Last3MonthsConsumeTimes,
					Last3MonthsConsumeTotal = memberConsumeList.Last3MonthsConsumeTotal.F2ToString("f2").ToDecimal(0),
					UserTags = from t in source
					select new
					{
						t.TagId,
						t.TagName
					},
					OrderList = from o in memberConsumeList.OrderList.Models
					select new
					{
						OrderId = o.OrderId,
						OrderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
						OrderTotal = o.OrderTotal.F2ToString("f2").ToDecimal(0)
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetStoreUsersStatistics(HttpContext context)
		{
			Pagination pagination = new Pagination();
			int shoppingGuiderId = 0;
			int num = context.Request["PageIndex"].ToInt(0);
			if (num < 1)
			{
				num = 1;
			}
			int num2 = context.Request["PageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			int num3 = context.Request["GroupId"].ToInt(0);
			if (num3 != 1 && num3 != 2 && num3 != 3 && num3 != 0)
			{
				num3 = 0;
			}
			int num4 = context.Request["TimeScope"].ToInt(0);
			if (num4 != 1 && num4 != 3 && num4 != 6 && num4 != 9 && num4 != 12)
			{
				num4 = 1;
			}
			if (num3 == 2 && num4 != 1 && num4 != 3 && num4 != 6)
			{
				num4 = 1;
			}
			pagination.PageIndex = num;
			pagination.PageSize = num2;
			pagination.SortBy = "PayDate";
			pagination.SortOrder = SortAction.Desc;
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out shoppingGuiderId);
			string keyword = Globals.StripAllTags(context.Request["Keyword"].ToNullString());
			StoreMemberStatisticsQuery storeMemberStatisticsQuery = new StoreMemberStatisticsQuery();
			storeMemberStatisticsQuery.Keyword = keyword;
			storeMemberStatisticsQuery.GroupId = num3;
			storeMemberStatisticsQuery.PageIndex = num;
			storeMemberStatisticsQuery.PageSize = num2;
			storeMemberStatisticsQuery.ShoppingGuiderId = shoppingGuiderId;
			storeMemberStatisticsQuery.StoreId = storeIdBySessionId;
			storeMemberStatisticsQuery.TimeScope = num4;
			storeMemberStatisticsQuery.SortBy = "UserId";
			storeMemberStatisticsQuery.SortOrder = SortAction.Desc;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			PageModel<StoreMemberStatisticsModel> storeMemberStatisticsList = MemberProcessor.GetStoreMemberStatisticsList(storeMemberStatisticsQuery, siteSettings.ConsumeTimesInOneMonth, siteSettings.ConsumeTimesInThreeMonth, siteSettings.ConsumeTimesInSixMonth);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeMemberStatisticsList.Total,
					List = from u in storeMemberStatisticsList.Models
					select new
					{
						UserId = u.UserId,
						NickName = (string.IsNullOrEmpty(u.NickName) ? u.RealName : u.NickName),
						UserName = u.UserName,
						LastConsumeDate = (u.LastConsumeDate.HasValue ? u.LastConsumeDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""),
						ConsumeTimes = u.ConsumeTimes,
						ConsumeTotal = u.ConsumeTotal.F2ToString("f2").ToDecimal(0),
						HeadImage = (string.IsNullOrEmpty(u.HeadImage.ToNullString()) ? Globals.FullPath("/templates/common/images/headerimg.png") : Globals.FullPath(u.HeadImage.ToNullString()))
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetUserGroupStatistics(HttpContext context)
		{
			int shoppingGuiderId = 0;
			int storeIdBySessionId = this.GetStoreIdBySessionId(context, out shoppingGuiderId);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			IDictionary<string, int> storeMemberCount = MemberHelper.GetStoreMemberCount(masterSettings.ConsumeTimesInOneMonth, masterSettings.ConsumeTimesInThreeMonth, masterSettings.ConsumeTimesInSixMonth, storeIdBySessionId, shoppingGuiderId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					MemberTotal = storeMemberCount["TotalMember"],
					NotConsume = storeMemberCount["NotConsume"],
					ActiveMemberTotal = storeMemberCount["ActiveInOneMonth"] + storeMemberCount["ActiveInThreeMonth"] + storeMemberCount["ActiveInSixMonth"],
					DormancyMemberTotal = storeMemberCount["DormancyInOneMonth"] + storeMemberCount["DormancyInThreeMonth"] + storeMemberCount["DormancyInSixMonth"] + storeMemberCount["DormancyInNineMonth"] + storeMemberCount["DormancyInOneYear"]
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void DeleteStoreGuiderHeadImage(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string text = context.Request["ImagePath"].ToNullString().ToLower();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				try
				{
					int num = text.IndexOf((HiContext.Current.GetStoragePath() + "/depot/HeadImage/").ToLower().Replace("//", "/"));
					if (num == -1)
					{
						num = text.IndexOf((HiContext.Current.GetStoragePath() + "/depot/").ToLower().Replace("//", "/"));
					}
					if (num > -1)
					{
						text = text.Substring(num);
					}
					text = context.Request.MapPath(text);
				}
				catch (Exception ex)
				{
					Globals.AppendLog(text + "-" + ex.Message, "", "", "DeleteStoreGuiderHeadImage");
				}
				if (File.Exists(text))
				{
					File.Delete(text);
					if (File.Exists(text.Replace("thum_", "")))
					{
						File.Delete(text.Replace("thum_", ""));
					}
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						Msg = ""
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void UploadStoreGuiderHeadImage(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string headImage = "";
			HttpFileCollection files = context.Request.Files;
			string status = "FAIL";
			try
			{
				if (files != null)
				{
					HttpPostedFile httpPostedFile = files[0];
					if (ResourcesHelper.CheckPostedFile(httpPostedFile, "image", null))
					{
						string str = ResourcesHelper.GenerateFilename(Path.GetExtension(httpPostedFile.FileName));
						string virtualPath = HiContext.Current.GetStoragePath() + "/Depot/HeadImage/" + str;
						string text = HiContext.Current.GetStoragePath() + "/Depot/HeadImage/thum_" + str;
						string text2 = HiContext.Current.Context.Request.MapPath(virtualPath);
						httpPostedFile.SaveAs(text2);
						ResourcesHelper.CreateThumbnail(text2, context.Request.MapPath(text), 310, 310);
						headImage = this.GetImageFullPath(text);
						status = "SUCCESS";
						goto end_IL_0021;
					}
					context.Response.Write(this.GetErrorJosn(112, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.NotImageFile, 0)));
					return;
				}
				end_IL_0021:;
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("count", dictionary.Count.ToString());
				Globals.WriteExceptionLog(ex, dictionary, "UploadStoreImage");
				headImage = "upload error";
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = status,
					HeadImage = headImage
				}
			});
			context.Response.Write(s);
		}

		public void AddSubAccount(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string text = Globals.StripAllTags(context.Request["UserName"].ToNullString());
			if (string.IsNullOrEmpty(text) || text.Length > 12)
			{
				context.Response.Write(this.GetErrorJosn(211, ((Enum)(object)ApiErrorCode.UserName_Error).ToDescription()));
			}
			else if (ManagerHelper.FindManagerByUsername(text) != null)
			{
				context.Response.Write(this.GetErrorJosn(203, ((Enum)(object)ApiErrorCode.Username_Exist).ToDescription()));
			}
			else
			{
				string text2 = context.Request["Password"].ToNullString();
				if (string.IsNullOrEmpty(text2) || text2.Length < 6 || text2.Length > 20)
				{
					context.Response.Write(this.GetErrorJosn(212, ((Enum)(object)ApiErrorCode.Password_Error).ToDescription()));
				}
				else
				{
					string value = context.Request["RePassword"].ToNullString();
					if (!text2.Equals(value))
					{
						context.Response.Write(this.GetErrorJosn(213, ((Enum)(object)ApiErrorCode.RePasswordNoEqualsPassword).ToDescription()));
					}
					else
					{
						int num = context.Request["status"].ToInt(0);
						if (!Enum.IsDefined(typeof(SubAccountStatus), num))
						{
							context.Response.Write(this.GetErrorJosn(133, "状态" + ((Enum)(object)ApiErrorCode.ValueUndefined).ToDescription()));
						}
						else
						{
							int roleId = -3;
							ManagerInfo managerInfo = new ManagerInfo();
							managerInfo.RoleId = roleId;
							managerInfo.UserName = text;
							managerInfo.StoreId = storeIdBySessionId;
							string text3 = Globals.RndStr(128, true);
							managerInfo.Password = Users.EncodePassword(text2, text3);
							managerInfo.PasswordSalt = text3;
							managerInfo.SessionId = Globals.RndStr(16, true);
							managerInfo.Status = num;
							managerInfo.CreateDate = DateTime.Now;
							bool flag = false;
							flag = (StoresHelper.AddSubAccount(managerInfo) > 0);
							string s = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = flag,
									Msg = (flag ? "子帐号添加成功" : "子帐号添加失败")
								}
							});
							context.Response.Write(s);
							context.Response.End();
						}
					}
				}
			}
		}

		public void EditSubAccount(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["UserId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "用户ID" + ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				ManagerInfo manager = Users.GetManager(num);
				if (manager == null)
				{
					context.Response.Write(this.GetErrorJosn(208, ((Enum)(object)ApiErrorCode.NotExistUser).ToDescription()));
				}
				else if (manager.StoreId != storeIdBySessionId)
				{
					context.Response.Write(this.GetErrorJosn(209, ((Enum)(object)ApiErrorCode.SubAccountNotRelationStore).ToDescription()));
				}
				else
				{
					string text = context.Request["Password"].ToNullString();
					if (!string.IsNullOrEmpty(text))
					{
						if (text.Length < 6 || text.Length > 20)
						{
							context.Response.Write(this.GetErrorJosn(212, ((Enum)(object)ApiErrorCode.Password_Error).ToDescription()));
							return;
						}
						string value = context.Request["RePassword"].ToNullString();
						if (!string.IsNullOrEmpty(text) && !text.Equals(value))
						{
							context.Response.Write(this.GetErrorJosn(213, ((Enum)(object)ApiErrorCode.RePasswordNoEqualsPassword).ToDescription()));
							return;
						}
					}
					if (string.IsNullOrEmpty(context.Request["Status"].ToNullString()))
					{
						context.Response.Write(this.GetErrorJosn(101, "状态" + ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
					}
					else
					{
						int num2 = context.Request["status"].ToInt(0);
						if (!Enum.IsDefined(typeof(SubAccountStatus), num2))
						{
							context.Response.Write(this.GetErrorJosn(133, "状态" + ((Enum)(object)ApiErrorCode.ValueUndefined).ToDescription()));
						}
						else
						{
							int num4 = manager.RoleId = -3;
							if (!string.IsNullOrEmpty(text))
							{
								string text2 = Globals.RndStr(128, true);
								manager.Password = Users.EncodePassword(text, text2);
								manager.PasswordSalt = text2;
							}
							manager.Status = num2;
							bool flag = false;
							flag = StoresHelper.UpdateSubAccount(manager);
							string s = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = flag,
									Msg = (flag ? "子帐号修改成功" : "子帐号修改失败")
								}
							});
							context.Response.Write(s);
							context.Response.End();
						}
					}
				}
			}
		}

		public void GetSubAccountDetail(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["UserId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, "用户ID" + ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				ManagerInfo manager = Users.GetManager(num);
				if (manager == null)
				{
					context.Response.Write(this.GetErrorJosn(208, ((Enum)(object)ApiErrorCode.NotExistUser).ToDescription()));
				}
				else if (manager.StoreId != storeIdBySessionId)
				{
					context.Response.Write(this.GetErrorJosn(209, ((Enum)(object)ApiErrorCode.SubAccountNotRelationStore).ToDescription()));
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							UserName = manager.UserName,
							AccountType = manager.RoleId,
							Status = manager.Status
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetSubAccounts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["pageIndex"].ToInt(0);
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			if (num2 <= 0)
			{
				num2 = 10;
			}
			StoreManagersQuery storeManagersQuery = new StoreManagersQuery();
			storeManagersQuery.RoleIds.Add(-3);
			storeManagersQuery.SortBy = "RoleId ASC,Status ";
			storeManagersQuery.SortOrder = SortAction.Desc;
			storeManagersQuery.StoreId = storeIdBySessionId;
			storeManagersQuery.PageSize = num2;
			storeManagersQuery.PageIndex = num;
			PageModel<ManagerInfo> managersByStoreId = ManagerHelper.GetManagersByStoreId(storeManagersQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = managersByStoreId.Total,
					List = from m in managersByStoreId.Models
					select new
					{
						UserId = m.ManagerId,
						UserName = m.UserName,
						AccountType = m.RoleId,
						Status = m.Status,
						HeadImage = (string.IsNullOrEmpty(m.HeadImage.ToNullString()) ? "" : Globals.FullPath(m.HeadImage.ToNullString()))
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void GetShoppingGuiders(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["pageIndex"].ToInt(0);
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			if (num2 <= 0)
			{
				num2 = 10;
			}
			int num3 = context.Request["TimeRange"].ToInt(0);
			if (!Enum.IsDefined(typeof(EnumTimeRange), num3))
			{
				context.Response.Write(this.GetErrorJosn(128, ((Enum)(object)ApiErrorCode.TimeRange_NoDefined).ToDescription()));
				context.Response.End();
			}
			DateTime? startTime = context.Request["StartTime"].ToDateTime();
			DateTime? endTime = context.Request["EndTime"].ToDateTime();
			if (num3 == 6 && !startTime.HasValue && !startTime.HasValue)
			{
				context.Response.Write(this.GetErrorJosn(104, ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription() + "：自定义时间范围时开始时间和结束时间至少要一个有值"));
				context.Response.End();
			}
			StoreManagersQuery storeManagersQuery = new StoreManagersQuery();
			DateTime dateTime;
			switch (num3)
			{
			case 1:
			{
				StoreManagersQuery storeManagersQuery6 = storeManagersQuery;
				dateTime = DateTime.Now;
				storeManagersQuery6.StartTime = dateTime.Date;
				storeManagersQuery.EndTime = DateTime.Now;
				break;
			}
			case 3:
			{
				StoreManagersQuery storeManagersQuery5 = storeManagersQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeManagersQuery5.StartTime = dateTime.AddDays((double)(-DateTime.Now.Day));
				storeManagersQuery.EndTime = DateTime.Now;
				break;
			}
			case 2:
			{
				StoreManagersQuery storeManagersQuery4 = storeManagersQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeManagersQuery4.StartTime = dateTime.AddDays(-7.0);
				storeManagersQuery.EndTime = DateTime.Now;
				break;
			}
			case 4:
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				DateTime value = dateTime.AddMonths(-1);
				value = value.AddDays((double)(-value.Day));
				storeManagersQuery.StartTime = value;
				StoreManagersQuery storeManagersQuery3 = storeManagersQuery;
				dateTime = value.AddMonths(1);
				storeManagersQuery3.EndTime = dateTime.AddSeconds(-1.0);
				break;
			}
			case 5:
			{
				StoreManagersQuery storeManagersQuery2 = storeManagersQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeManagersQuery2.StartTime = dateTime.AddMonths(-3);
				storeManagersQuery.EndTime = DateTime.Now;
				break;
			}
			case 6:
				storeManagersQuery.StartTime = startTime;
				storeManagersQuery.EndTime = endTime;
				break;
			}
			storeManagersQuery.RoleIds.Add(-3);
			storeManagersQuery.SortBy = "UserTotal";
			storeManagersQuery.SortOrder = SortAction.Desc;
			storeManagersQuery.StoreId = storeIdBySessionId;
			storeManagersQuery.PageSize = num2;
			storeManagersQuery.PageIndex = num;
			PageModel<ShoppingGuiderModel> shoppingGuiders = StoresHelper.GetShoppingGuiders(storeManagersQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = shoppingGuiders.Total,
					List = from m in shoppingGuiders.Models
					select new
					{
						UserId = m.UserId,
						UserName = m.UserName,
						AccountType = m.AccountType,
						Status = m.Status,
						UserTotal = m.UserTotal,
						UsersOrderTotal = m.UsersOrderTotal,
						HeadImage = (string.IsNullOrEmpty(m.HeadImage.ToNullString()) ? "" : Globals.FullPath(m.HeadImage.ToNullString()))
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetCategories(HttpContext context)
		{
			int num = context.Request["pid"].ToInt(0);
			if (num < 0)
			{
				context.Response.Write(this.GetErrorJosn(102, "数字类型转换错误"));
			}
			else
			{
				IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(num);
				if (subCategories == null)
				{
					context.Response.Write(this.GetErrorJosn(103, "没获取到相应的分类"));
				}
				else
				{
					var result = (from c in subCategories
					select new
					{
						cid = c.CategoryId,
						name = c.Name,
						icon = (string.IsNullOrEmpty(c.Icon) ? "/templates/appshop/images/catedefaulticon.jpg" : Globals.FullPath(c.Icon)),
						bigImageUrl = Globals.FullPath(c.BigImageUrl),
						hasChildren = c.HasChildren.ToString().ToLower()
					}).ToList();
					string s = JsonConvert.SerializeObject(new
					{
						Result = result
					});
					context.Response.Write(s);
				}
			}
		}

		private void BatchOnShelvesProducts(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			if (!storeInfoBySessionId.IsShelvesProduct)
			{
				context.Response.Write(this.GetErrorJosn(531, ((Enum)(object)ApiErrorCode.NotShelvesProduct).ToDescription()));
			}
			else
			{
				string text = context.Request["ProductIds"].ToNullString();
				int num = context.Request["Stock"].ToInt(0);
				decimal num2 = context.Request["StoreSalePrice"].ToDecimal(0);
				int num3 = context.Request["WarningStock"].ToInt(0);
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write(this.GetErrorJosn(104, "商品ID" + ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription()));
				}
				else
				{
					DataTable skuStocks = ProductHelper.GetSkuStocks(text);
					List<StoreSKUInfo> list = new List<StoreSKUInfo>();
					List<StoreStockLogInfo> list2 = new List<StoreStockLogInfo>();
					List<int> list3 = new List<int>();
					int num4 = 0;
					foreach (DataRow row in skuStocks.Rows)
					{
						decimal num5 = row["SalePrice"].ToDecimal(0);
						if (num2 > decimal.Zero && storeInfoBySessionId.IsModifyPrice)
						{
							if (storeInfoBySessionId.MinPriceRate.ToDecimal(0) > decimal.Zero)
							{
								decimal d = num2;
								decimal value = num5;
								decimal? minPriceRate = storeInfoBySessionId.MinPriceRate;
								decimal? nullable = (decimal?)value * minPriceRate;
								if (d < nullable.GetValueOrDefault() && nullable.HasValue)
								{
									context.Response.Write(this.GetErrorJosn(518, "门店价格不能小于平台价格的" + storeInfoBySessionId.MinPriceRate.Value.F2ToString("f2") + "倍！"));
									return;
								}
							}
							if (storeInfoBySessionId.MaxPriceRate.ToDecimal(0) > decimal.Zero)
							{
								decimal d2 = num2;
								decimal value = num5;
								decimal? minPriceRate = storeInfoBySessionId.MaxPriceRate;
								decimal? nullable = (decimal?)value * minPriceRate;
								if (d2 > nullable.GetValueOrDefault() && nullable.HasValue)
								{
									context.Response.Write(this.GetErrorJosn(518, "门店价格不能大于平台价格的" + storeInfoBySessionId.MaxPriceRate.Value.F2ToString("f2") + "倍！"));
									return;
								}
							}
							num5 = num2;
						}
						else
						{
							num5 = default(decimal);
							if (storeInfoBySessionId.MinPriceRate.ToDecimal(0) > decimal.One)
							{
								num5 *= storeInfoBySessionId.MinPriceRate.ToDecimal(0);
							}
							if (storeInfoBySessionId.MaxPriceRate.ToDecimal(0) < decimal.One)
							{
								num5 *= storeInfoBySessionId.MaxPriceRate.ToDecimal(0);
							}
						}
						StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
						storeSKUInfo.Stock = num;
						storeSKUInfo.StoreSalePrice = num5;
						storeSKUInfo.WarningStock = num3;
						storeSKUInfo.ProductID = row["ProductID"].ToInt(0);
						storeSKUInfo.SkuId = row["SkuId"].ToString();
						storeSKUInfo.StoreId = storeInfoBySessionId.StoreId;
						storeSKUInfo.FreezeStock = 0;
						list.Add(storeSKUInfo);
						if (!list3.Contains(storeSKUInfo.ProductID))
						{
							list3.Add(storeSKUInfo.ProductID);
						}
						StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
						storeStockLogInfo.ProductId = storeSKUInfo.ProductID;
						storeStockLogInfo.Remark = DataHelper.CleanSearchString("APP门店管理员上架商品");
						storeStockLogInfo.SkuId = storeSKUInfo.SkuId;
						storeStockLogInfo.Operator = storeInfoBySessionId.StoreName;
						storeStockLogInfo.StoreId = storeInfoBySessionId.StoreId;
						storeStockLogInfo.ChangeTime = DateTime.Now;
						storeStockLogInfo.Content = "APP门店管理员上架" + row["SKU"].ToString();
						StoreStockLogInfo storeStockLogInfo2 = storeStockLogInfo;
						storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 库存【" + num + "】";
						storeStockLogInfo2 = storeStockLogInfo;
						storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 门店警戒【" + num3 + "】;";
						storeStockLogInfo2 = storeStockLogInfo;
						storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 门店售价【" + num5 + "】";
						list2.Add(storeStockLogInfo);
					}
					string text2 = "FAIL";
					if (list.Count > 0 && list.Count > 0 && StoresHelper.AddStoreProduct(list, list2, list3))
					{
						text2 = "SUCCESS";
					}
					if (text2 == "SUCCESS")
					{
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = text2
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
					else
					{
						context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
					}
				}
			}
		}

		private void OnShelvesProduct(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			if (!storeInfoBySessionId.IsShelvesProduct)
			{
				context.Response.Write(this.GetErrorJosn(531, ((Enum)(object)ApiErrorCode.NotShelvesProduct).ToDescription()));
			}
			else
			{
				int num = context.Request["ProductId"].ToInt(0);
				string safeIDList = Globals.GetSafeIDList(context.Request["skuIds"].ToNullString(), ',', false);
				string safeIDList2 = Globals.GetSafeIDList(context.Request["Stocks"].ToNullString(), ',', false);
				string safeIDList3 = Globals.GetSafeIDList(context.Request["StoreSalePrices"].ToNullString(), ',', false);
				string safeIDList4 = Globals.GetSafeIDList(context.Request["WarningStocks"].ToNullString(), ',', false);
				if (num < 0)
				{
					context.Response.Write(this.GetErrorJosn(104, "商品ID" + ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription()));
				}
				else if (safeIDList.Split(',').Length != safeIDList2.Split(',').Length || safeIDList2.Split(',').Length != safeIDList3.Split(',').Length || safeIDList2.Split(',').Length != safeIDList4.Split(',').Length)
				{
					context.Response.Write(this.GetErrorJosn(107, "数目不一致" + ((Enum)(object)ApiErrorCode.Paramter_Diffrent).ToDescription()));
				}
				else
				{
					DataTable skuStocks = ProductHelper.GetSkuStocks(num.ToString());
					if (skuStocks.Rows.Count != safeIDList2.Split(',').Length)
					{
						context.Response.Write(this.GetErrorJosn(107, "数目不一致,请重新获取数据再上架！" + ((Enum)(object)ApiErrorCode.Paramter_Diffrent).ToDescription()));
					}
					else
					{
						List<StoreSKUInfo> list = new List<StoreSKUInfo>();
						List<StoreStockLogInfo> list2 = new List<StoreStockLogInfo>();
						string[] array = safeIDList2.Split(',');
						string[] array2 = safeIDList3.Split(',');
						string[] array3 = safeIDList4.Split(',');
						string[] array4 = safeIDList.Split(',');
						int num2 = 0;
						foreach (DataRow row in skuStocks.Rows)
						{
							StoreSKUInfo storeSKUInfo = new StoreSKUInfo();
							int num3 = 0;
							for (int i = 0; i < array4.Length; i++)
							{
								if (array4[i] == row["SkuId"].ToString())
								{
									num3 = i;
								}
							}
							decimal num4 = row["SalePrice"].ToDecimal(0);
							decimal num5 = array2[num3].ToDecimal(0);
							if (num5 > decimal.Zero && storeInfoBySessionId.IsModifyPrice)
							{
								if (storeInfoBySessionId.MinPriceRate.ToDecimal(0) > decimal.Zero)
								{
									decimal d = num5;
									decimal value = num4;
									decimal? minPriceRate = storeInfoBySessionId.MinPriceRate;
									decimal? nullable = (decimal?)value * minPriceRate;
									if (d < nullable.GetValueOrDefault() && nullable.HasValue)
									{
										context.Response.Write(this.GetErrorJosn(518, "门店价格不能小于平台价格的" + storeInfoBySessionId.MinPriceRate.Value.F2ToString("f2") + "倍！"));
										return;
									}
								}
								if (storeInfoBySessionId.MaxPriceRate.ToDecimal(0) > decimal.Zero)
								{
									decimal d2 = num5;
									decimal value = num4;
									decimal? minPriceRate = storeInfoBySessionId.MaxPriceRate;
									decimal? nullable = (decimal?)value * minPriceRate;
									if (d2 > nullable.GetValueOrDefault() && nullable.HasValue)
									{
										context.Response.Write(this.GetErrorJosn(518, "门店价格不能大于平台价格的" + storeInfoBySessionId.MaxPriceRate.Value.F2ToString("f2") + "倍！"));
										return;
									}
								}
								num4 = num5;
							}
							else
							{
								num4 = default(decimal);
								if (storeInfoBySessionId.MinPriceRate.ToDecimal(0) > decimal.One)
								{
									num4 *= storeInfoBySessionId.MinPriceRate.ToDecimal(0);
								}
								if (storeInfoBySessionId.MaxPriceRate.ToDecimal(0) < decimal.One)
								{
									num4 *= storeInfoBySessionId.MaxPriceRate.ToDecimal(0);
								}
							}
							storeSKUInfo.Stock = array[num3].ToInt(0);
							storeSKUInfo.StoreSalePrice = num4;
							storeSKUInfo.WarningStock = array3[num3].ToInt(0);
							storeSKUInfo.ProductID = row["ProductID"].ToInt(0);
							storeSKUInfo.SkuId = row["SkuId"].ToString();
							storeSKUInfo.StoreId = storeInfoBySessionId.StoreId;
							storeSKUInfo.FreezeStock = 0;
							list.Add(storeSKUInfo);
							StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
							storeStockLogInfo.ProductId = storeSKUInfo.ProductID;
							storeStockLogInfo.Remark = DataHelper.CleanSearchString("APP门店管理员上架商品");
							storeStockLogInfo.SkuId = storeSKUInfo.SkuId;
							storeStockLogInfo.Operator = storeInfoBySessionId.StoreName;
							storeStockLogInfo.StoreId = storeInfoBySessionId.StoreId;
							storeStockLogInfo.ChangeTime = DateTime.Now;
							storeStockLogInfo.Content = "APP门店管理员上架" + row["SKU"].ToString();
							StoreStockLogInfo storeStockLogInfo2 = storeStockLogInfo;
							storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 库存【" + storeSKUInfo.Stock + "】";
							storeStockLogInfo2 = storeStockLogInfo;
							storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 门店警戒【" + storeSKUInfo.WarningStock + "】;";
							storeStockLogInfo2 = storeStockLogInfo;
							storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 门店售价【" + num4 + "】";
							list2.Add(storeStockLogInfo);
						}
						string text = "FAIL";
						if (list.Count > 0 && list.Count > 0 && StoresHelper.AddStoreProduct(list, list2, new List<int>
						{
							num
						}))
						{
							text = "SUCCESS";
						}
						if (text == "SUCCESS")
						{
							string s = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = text
								}
							});
							context.Response.Write(s);
							context.Response.End();
						}
						else
						{
							context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
						}
					}
				}
			}
		}

		public void ImageRelationProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int imageId = context.Request["ImageId"].ToInt(0);
			string idList = context.Request["ProductIds"].ToNullString();
			MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(imageId);
			if (marketingImagesInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
			}
			else
			{
				idList = Globals.GetSafeIDList(idList, ',', true);
				if (string.IsNullOrEmpty(idList))
				{
					context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
				}
				else
				{
					StoreMarketingImagesInfo storeMarketingImagesInfo = MarketingImagesHelper.GetStoreMarketingImages(storeIdBySessionId, imageId);
					string text = "";
					if (storeMarketingImagesInfo != null)
					{
						string safeIDList = Globals.GetSafeIDList(storeMarketingImagesInfo.ProductIds, ',', true);
						if (safeIDList != "")
						{
							string[] array = idList.Split(',');
							foreach (string str in array)
							{
								if (!("," + safeIDList + ",").Contains("," + str + ","))
								{
									text = text + str + ",";
								}
							}
						}
					}
					else
					{
						storeMarketingImagesInfo = new StoreMarketingImagesInfo();
						storeMarketingImagesInfo.StoreId = storeIdBySessionId;
						storeMarketingImagesInfo.ImageId = imageId;
					}
					if (!string.IsNullOrEmpty(text))
					{
						StoreMarketingImagesInfo storeMarketingImagesInfo2 = storeMarketingImagesInfo;
						storeMarketingImagesInfo2.ProductIds += ("," + text).TrimEnd(',');
					}
					else
					{
						storeMarketingImagesInfo.ProductIds = idList.TrimEnd(',');
					}
					MarketingImagesHelper.UpdateStoreMarketingImages(storeMarketingImagesInfo);
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "True",
							Msg = ""
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetImageNoRelationProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int imageId = context.Request["ImageId"].ToInt(0);
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(imageId);
			int num3 = context.Request["CID"].ToInt(0);
			if (marketingImagesInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
			}
			else
			{
				string text = Globals.StripAllTags(context.Request["Keyword"].ToNullString());
				string filterProductIds = "";
				StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(storeIdBySessionId, imageId);
				if (storeMarketingImages != null)
				{
					filterProductIds = Globals.GetSafeIDList(storeMarketingImages.ProductIds, ',', true);
				}
				StoreProductsQuery storeProductsQuery = new StoreProductsQuery();
				if (!string.IsNullOrEmpty(text))
				{
					storeProductsQuery.productCode = text;
					storeProductsQuery.ProductName = text;
				}
				storeProductsQuery.StoreId = storeIdBySessionId;
				storeProductsQuery.FilterProductIds = filterProductIds;
				storeProductsQuery.PageIndex = num;
				storeProductsQuery.PageSize = num2;
				if (num3 > 0)
				{
					storeProductsQuery.CategoryId = num3;
					storeProductsQuery.MainCategoryPath = CatalogHelper.GetCategory(num3).Path;
				}
				PageModel<StoreProductBaseModel> storeProductBaseInfo = StoresHelper.GetStoreProductBaseInfo(storeProductsQuery);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = storeProductBaseInfo.Total,
						List = from p in storeProductBaseInfo.Models
						select new
						{
							ProductId = p.ProductId,
							ProductName = p.ProductName,
							ImageUrl = this.GetImageFullPath(p.ProductImage),
							Stock = p.Stock,
							Price = p.Price.F2ToString("f2")
						}
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void DeleteImageProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int imageId = context.Request["ImageId"].ToInt(0);
			string idList = context.Request["ProductIds"].ToNullString();
			MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(imageId);
			if (marketingImagesInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
			}
			else
			{
				idList = Globals.GetSafeIDList(idList, ',', true);
				if (string.IsNullOrEmpty(idList))
				{
					context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
				}
				else
				{
					StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(storeIdBySessionId, imageId);
					string text = "";
					if (storeMarketingImages != null)
					{
						string safeIDList = Globals.GetSafeIDList(storeMarketingImages.ProductIds, ',', true);
						if (safeIDList != "")
						{
							string[] array = safeIDList.Split(',');
							foreach (string str in array)
							{
								if (!("," + idList + ",").Contains("," + str + ","))
								{
									text = text + str + ",";
								}
							}
						}
					}
					text = text.TrimEnd(',');
					if (storeMarketingImages != null && text != "")
					{
						storeMarketingImages.ProductIds = text;
						MarketingImagesHelper.UpdateStoreMarketingImages(storeMarketingImages);
					}
					if (storeMarketingImages != null && text == "")
					{
						MarketingImagesHelper.DeleteStoreMarketingImages(storeMarketingImages.ImageId, storeMarketingImages.StoreId);
					}
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "True",
							Msg = ""
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetImageProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int imageId = context.Request["ImageId"].ToInt(0);
			MarketingImagesInfo marketingImagesInfo = MarketingImagesHelper.GetMarketingImagesInfo(imageId);
			if (marketingImagesInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(701, ((Enum)(object)ApiErrorCode.ImageIdNotExists_Error).ToDescription()));
			}
			else
			{
				StoreMarketingImagesInfo storeMarketingImages = MarketingImagesHelper.GetStoreMarketingImages(storeIdBySessionId, imageId);
				IList<StoreProductBaseModel> list = null;
				if (storeMarketingImages != null)
				{
					string safeIDList = Globals.GetSafeIDList(storeMarketingImages.ProductIds, ',', true);
					if (safeIDList != "")
					{
						list = StoresHelper.GetStoreProductBaseInfo(storeMarketingImages.ProductIds, storeIdBySessionId);
					}
				}
				if (list == null)
				{
					list = new List<StoreProductBaseModel>();
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						ImageId = marketingImagesInfo.ImageId,
						ImageUrl = this.GetImageFullPath(marketingImagesInfo.ImageUrl),
						ImageName = marketingImagesInfo.ImageName,
						Description = marketingImagesInfo.Description,
						List = from p in list
						select new
						{
							ProductId = p.ProductId,
							ProductName = p.ProductName,
							ImageUrl = this.GetImageFullPath(p.ProductImage),
							Stock = p.Stock,
							Price = p.Price.F2ToString("f2")
						}
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void GetMarketingImages(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			MarketingImagesQuery marketingImagesQuery = new MarketingImagesQuery();
			marketingImagesQuery.PageIndex = num;
			marketingImagesQuery.PageSize = num2;
			marketingImagesQuery.SortOrder = SortAction.Desc;
			marketingImagesQuery.SortBy = "ImageId";
			PageModel<MarketingImagesInfo> marketingImages = MarketingImagesHelper.GetMarketingImages(marketingImagesQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = marketingImages.Total,
					List = from i in marketingImages.Models
					select new
					{
						ImageId = i.ImageId,
						ImageName = i.ImageName,
						ImageUrl = this.GetImageFullPath(i.ImageUrl),
						Description = i.Description
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		public void ShowFileTypeCode(HttpContext context)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(context.Request.MapPath("/cert/"));
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				context.Response.Write(fileInfo.Name + "-" + ResourcesHelper.GetFileClassCode(directoryInfo.FullName + fileInfo.Name) + "/n/r");
			}
		}

		public void CheckOrderProductStock(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string orderId = context.Request["OrderId"].ToNullString();
			int storeId = this.GetStoreIdBySessionId(context);
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
			if (orderInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
			}
			else if (orderInfo.StoreId != storeId)
			{
				context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
			}
			else if (orderInfo.IsConfirm)
			{
				context.Response.Write(this.GetErrorJosn(510, ((Enum)(object)ApiErrorCode.StoreOrder_IsConfirmed).ToDescription()));
			}
			else
			{
				IList<StoreProductModel> list = new List<StoreProductModel>();
				foreach (LineItemInfo value in orderInfo.LineItems.Values)
				{
					StoreProductModel storeProductInfo = StoresHelper.GetStoreProductInfo(storeId, value.ProductId);
					list.Add(storeProductInfo);
				}
				if (OrderHelper.GetNoStockItems(orderInfo) != null)
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							OrderId = orderId,
							Status = "STOCK NOT ENOUGH",
							ProductList = from d in list
							select new
							{
								ProductName = d.ProductName,
								ProductId = d.ProductId,
								Stock = StoresHelper.GetStoreProductStock(storeId, d.ProductId),
								ProductCode = d.ProductCode,
								Image = this.GetImageFullPath(d.ThumbnailUrl410),
								SkuList = from sku in d.Skus.Values
								select new
								{
									SkuId = sku.SkuId,
									StoreStock = sku.Stock,
									SkuText = ProductHelper.GetSkusBySkuId(sku.SkuId, d.ProductId),
									ProductCode = sku.SKU,
									WarningStock = sku.WarningStock
								}
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					string s2 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							OrderId = orderId,
							ProductList = from d in list
							select new
							{
								ProductName = d.ProductName,
								ProductId = d.ProductId,
								Stock = StoresHelper.GetStoreProductStock(storeId, d.ProductId),
								ProductCode = d.ProductCode,
								Image = this.GetImageFullPath(d.ThumbnailUrl410),
								SkuList = from sku in d.Skus.Values
								select new
								{
									SkuId = sku.SkuId,
									StoreStock = sku.Stock,
									SkuText = ProductHelper.GetSkusBySkuId(sku.SkuId, d.ProductId),
									ProductCode = sku.SKU,
									WarningStock = sku.WarningStock
								}
							},
							Status = "SUCCESS"
						}
					});
					context.Response.Write(s2);
					context.Response.End();
				}
			}
		}

		private void DeleteStoreImage(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string text = context.Request["ImagePath"].ToNullString().ToLower();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				try
				{
					int num = text.IndexOf((HiContext.Current.GetStoragePath() + "/depot/images/").ToLower().Replace("//", "/"));
					if (num == -1)
					{
						num = text.IndexOf((HiContext.Current.GetStoragePath() + "/depot/").ToLower().Replace("//", "/"));
					}
					if (num > -1)
					{
						text = text.Substring(num);
					}
					text = context.Request.MapPath(text);
				}
				catch (Exception ex)
				{
					Globals.AppendLog(text + "-" + ex.Message, "", "", "DeleteStoreImage");
				}
				if (File.Exists(text))
				{
					File.Delete(text);
					if (File.Exists(text.Replace("thum_", "")))
					{
						File.Delete(text.Replace("thum_", ""));
					}
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void UploadStoreImage(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			IList<string> list = new List<string>();
			HttpFileCollection files = context.Request.Files;
			try
			{
				if (files != null)
				{
					for (int i = 0; i < files.Count; i++)
					{
						HttpPostedFile httpPostedFile = files[i];
						if (ResourcesHelper.CheckPostedFile(httpPostedFile, "image", null))
						{
							string str = ResourcesHelper.GenerateFilename(Path.GetExtension(httpPostedFile.FileName));
							string virtualPath = HiContext.Current.GetStoragePath() + "/depot/" + str;
							string text = HiContext.Current.GetStoragePath() + "/depot/thum_" + str;
							string text2 = HiContext.Current.Context.Request.MapPath(virtualPath);
							httpPostedFile.SaveAs(text2);
							ResourcesHelper.CreateThumbnail(text2, context.Request.MapPath(text), 160, 160);
							list.Add(this.GetImageFullPath(text));
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("count", dictionary.Count.ToString());
				Globals.WriteExceptionLog(ex, dictionary, "UploadStoreImage");
				list.Add("upload error");
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Count = list.Count,
					Images = from c in list
					select new
					{
						ImageUrl = c
					}
				}
			});
			context.Response.Write(s);
		}

		private void GetStoreSlideImages(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string storeSlideImagesByStoreId = DepotHelper.GetStoreSlideImagesByStoreId(storeIdBySessionId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					StoreSlideImages = string.Join(",", this.GetImagesFullPath(storeSlideImagesByStoreId, ','))
				}
			});
			context.Response.Write(s);
		}

		private void UploadStoreSlideImage(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			IList<string> list = new List<string>();
			HttpFileCollection files = context.Request.Files;
			try
			{
				if (files != null)
				{
					for (int i = 0; i < files.Count; i++)
					{
						HttpPostedFile httpPostedFile = files[i];
						if (ResourcesHelper.CheckPostedFile(httpPostedFile, "image", null))
						{
							string str = ResourcesHelper.GenerateFilename(Path.GetExtension(httpPostedFile.FileName));
							string virtualPath = HiContext.Current.GetStoragePath() + "depot/" + str;
							string text = HiContext.Current.GetStoragePath() + "depot/slide_" + str;
							string text2 = HiContext.Current.Context.Request.MapPath(virtualPath);
							httpPostedFile.SaveAs(text2);
							ResourcesHelper.CreateThumbnail(text2, context.Request.MapPath(text), 735, 480);
							list.Add(this.GetImageFullPath(text));
							string storeSlideImagesByStoreId = DepotHelper.GetStoreSlideImagesByStoreId(storeIdBySessionId);
							List<string> list2 = new List<string>();
							if (!string.IsNullOrEmpty(storeSlideImagesByStoreId))
							{
								list2 = new List<string>(storeSlideImagesByStoreId.Split(','));
							}
							list2.Add(this.GetImageFullPath(text));
							DepotHelper.UpdateStoreSlideImages(storeIdBySessionId, string.Join(",", list2));
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("count", dictionary.Count.ToString());
				Globals.WriteExceptionLog(ex, dictionary, "UploadStoreImage");
				list.Add("upload error");
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Count = list.Count,
					Images = from c in list
					select new
					{
						ImageUrl = c
					}
				}
			});
			context.Response.Write(s);
		}

		private void DeleteStoreSlideImage(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int index = context.Request["index"].ToInt(0);
			string storeSlideImagesByStoreId = DepotHelper.GetStoreSlideImagesByStoreId(storeIdBySessionId);
			List<string> list = new List<string>(storeSlideImagesByStoreId.Split(','));
			list.RemoveAt(index);
			DepotHelper.UpdateStoreSlideImages(storeIdBySessionId, string.Join(",", list));
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					Status = "SUCCESS"
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GenerateOfflineOrder(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			decimal payAmount = context.Request["PayAmount"].ToDecimal(0);
			string text = Globals.StripAllTags(context.Request["Remark"].ToNullString());
			StoreCollectionInfo storeCollectionInfo = new StoreCollectionInfo();
			storeCollectionInfo.CreateTime = DateTime.Now;
			storeCollectionInfo.PayTime = storeCollectionInfo.CreateTime;
			storeCollectionInfo.GateWay = "";
			storeCollectionInfo.OrderType = 2;
			storeCollectionInfo.PayAmount = payAmount;
			storeCollectionInfo.RefundAmount = decimal.Zero;
			storeCollectionInfo.Remark = (string.IsNullOrEmpty(text) ? "门店线下订单在线支付" : text);
			storeCollectionInfo.SerialNumber = Globals.GetGenerateId();
			storeCollectionInfo.Status = 0;
			storeCollectionInfo.StoreId = storeIdBySessionId;
			storeCollectionInfo.UserId = 0;
			if (StoresHelper.AddStoreCollectionInfo(storeCollectionInfo))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						storeCollectionInfo.SerialNumber
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
			}
		}

		private void GetSupportPayType(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest";
			bool isOpenWxPay = false;
			bool isOpenAliPay = false;
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (siteSettings.OpenVstore == 1 && siteSettings.EnableWeiXinRequest && !string.IsNullOrEmpty(siteSettings.WeixinAppId) && !string.IsNullOrEmpty(siteSettings.WeixinAppSecret) && !string.IsNullOrEmpty(siteSettings.WeixinPartnerID) && !string.IsNullOrEmpty(siteSettings.WeixinPartnerKey))
			{
				isOpenWxPay = true;
			}
			PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(gateway);
			if (paymentMode != null)
			{
				isOpenAliPay = true;
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					IsOpenWxPay = isOpenWxPay,
					IsOpenAliPay = isOpenAliPay
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void CashierStatistical(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			bool flag = context.Request["IsToday"].ToBool();
			DateTime? nullable = context.Request["StartTime"].ToDateTime();
			DateTime? nullable2 = context.Request["EndTime"].ToDateTime();
			StoreStatisticsInfo storeStatisticsInfo = new StoreStatisticsInfo();
			if (flag)
			{
				nullable = DateTime.Now.Date;
				nullable2 = DateTime.Now;
			}
			else if (!nullable.HasValue || !nullable2.HasValue)
			{
				context.Response.Write(this.GetErrorJosn(104, ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription() + "：不是统计今日数据时,开始时间和结束时间必须传值"));
				context.Response.End();
			}
			storeStatisticsInfo = StoresHelper.GetStoreCollectionStatistical(storeIdBySessionId, nullable.Value, nullable2.Value);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					StoreCashierTotal = storeStatisticsInfo.StoreCashierTotal.F2ToString("f2").ToDecimal(0),
					OfflineCashierTotal = storeStatisticsInfo.OfflineCashierTotal.F2ToString("f2"),
					TodayOrderTotal = storeStatisticsInfo.TodayOrderTotal,
					TodayCashierTotal = storeStatisticsInfo.TodayCashierTotal.F2ToString("f2"),
					PlatCashierTotal = storeStatisticsInfo.PlatCashierTotal.F2ToString("f2").ToDecimal(0),
					OnlinePayCashierTotal = storeStatisticsInfo.OnlinePayCashierTotal.F2ToString("f2"),
					OnDoorCashierTotal = storeStatisticsInfo.OnDoorCashierTotal.F2ToString("f2"),
					CashCashierTotal = storeStatisticsInfo.CashCashierTotal.F2ToString("f2")
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void CashierDetailList(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["PayTimeRange"].ToInt(0);
			if (!Enum.IsDefined(typeof(EnumTimeRange), num))
			{
				context.Response.Write(this.GetErrorJosn(128, ((Enum)(object)ApiErrorCode.TimeRange_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num2 = context.Request["PayType"].ToInt(0);
			if (num2 != 0 && !Enum.IsDefined(typeof(EnumPaymentType), num2))
			{
				context.Response.Write(this.GetErrorJosn(129, ((Enum)(object)ApiErrorCode.PayType_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num3 = context.Request["OrderType"].ToInt(0);
			if (num3 != 0 && !Enum.IsDefined(typeof(EnumOrderType), num3))
			{
				context.Response.Write(this.GetErrorJosn(130, ((Enum)(object)ApiErrorCode.OrderType_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num4 = context.Request["CollectionType"].ToInt(0);
			if (num4 != 0 && num4 != 1 && num4 != 2)
			{
				num4 = 0;
			}
			int num5 = context.Request["pageIndex"].ToInt(0);
			if (num5 <= 0)
			{
				num5 = 1;
			}
			int num6 = context.Request["pageSize"].ToInt(0);
			if (num6 < 1)
			{
				num6 = 10;
			}
			DateTime? startPayTime = context.Request["StartTime"].ToDateTime();
			DateTime? endPayTime = context.Request["EndTime"].ToDateTime();
			if (num == 6 && !startPayTime.HasValue && !startPayTime.HasValue)
			{
				context.Response.Write(this.GetErrorJosn(104, ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription() + "：自定义时间范围时开始时间和结束时间至少要一个有值"));
				context.Response.End();
			}
			StoreCollectionsQuery storeCollectionsQuery = new StoreCollectionsQuery();
			DateTime dateTime;
			switch (num)
			{
			case 1:
			{
				StoreCollectionsQuery storeCollectionsQuery6 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				storeCollectionsQuery6.StartPayTime = dateTime.Date;
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 3:
			{
				StoreCollectionsQuery storeCollectionsQuery5 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery5.StartPayTime = dateTime.AddDays((double)(-DateTime.Now.Day));
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 2:
			{
				StoreCollectionsQuery storeCollectionsQuery4 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery4.StartPayTime = dateTime.AddDays(-7.0);
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 4:
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				DateTime value = dateTime.AddMonths(-1);
				value = value.AddDays((double)(-value.Day));
				storeCollectionsQuery.StartPayTime = value;
				StoreCollectionsQuery storeCollectionsQuery3 = storeCollectionsQuery;
				dateTime = value.AddMonths(1);
				storeCollectionsQuery3.EndPayTime = dateTime.AddSeconds(-1.0);
				break;
			}
			case 5:
			{
				StoreCollectionsQuery storeCollectionsQuery2 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery2.StartPayTime = dateTime.AddMonths(-3);
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 6:
				storeCollectionsQuery.StartPayTime = startPayTime;
				storeCollectionsQuery.EndPayTime = endPayTime;
				break;
			}
			if (num4 != 0)
			{
				storeCollectionsQuery.CollectionType = num4;
			}
			if (num3 != 0)
			{
				storeCollectionsQuery.OrderType = num3;
			}
			if (num2 != 0)
			{
				storeCollectionsQuery.PaymentTypeId = num2;
			}
			storeCollectionsQuery.PageIndex = num5;
			storeCollectionsQuery.PageSize = num6;
			storeCollectionsQuery.StoreId = storeIdBySessionId;
			decimal amountTotal = default(decimal);
			decimal num7 = default(decimal);
			decimal num8 = default(decimal);
			PageModel<StoreCollectionInfo> storeCollectionInfos = StoresHelper.GetStoreCollectionInfos(storeCollectionsQuery, out amountTotal, out num7, out num8);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeCollectionInfos.Total,
					AmountTotal = amountTotal,
					List = storeCollectionInfos.Models.ToList().Select(delegate(StoreCollectionInfo c)
					{
						string serialNumber = c.SerialNumber;
						DateTime dateTime2 = c.CreateTime;
						string createTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						object finishTime;
						if (!c.FinishTime.HasValue)
						{
							dateTime2 = c.PayTime;
							finishTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						else
						{
							dateTime2 = c.FinishTime.Value;
							finishTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						decimal payAmount = c.PayAmount.F2ToString("f2").ToDecimal(0);
						string orderId = string.IsNullOrEmpty(c.OrderId) ? "" : c.OrderId;
						int? paymentTypeId = c.PaymentTypeId;
						string paymentTypeName = c.PaymentTypeName;
						dateTime2 = c.PayTime;
						string payTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						int? userId = c.UserId;
						bool isStoreCollection = c.GateWay == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1);
						bool isOfflineOrder = string.IsNullOrEmpty(c.OrderId);
						int status = c.Status;
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)(EnumStoreCollectionStatus)c.Status, 0);
						int orderType = c.OrderType;
						string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)(EnumOrderType)c.OrderType, 0);
						string refundAmount = ((c.RefundAmount > decimal.Zero) ? "-" : "") + c.RefundAmount.F2ToString("f2").ToDecimal(0);
						object refundTime;
						if (!c.RefundTime.HasValue)
						{
							refundTime = "";
						}
						else
						{
							dateTime2 = c.RefundTime.Value;
							refundTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						return new
						{
							SerialNumber = serialNumber,
							CreateTime = createTime,
							FinishTime = (string)finishTime,
							PayAmount = payAmount,
							OrderId = orderId,
							PaymentTypeId = paymentTypeId,
							PaymentTypeName = paymentTypeName,
							PayTime = payTime,
							UserId = userId,
							IsStoreCollection = isStoreCollection,
							IsOfflineOrder = isOfflineOrder,
							Status = status,
							StatusText = enumDescription,
							OrderType = orderType,
							OrderTypeText = enumDescription2,
							RefundAmount = refundAmount,
							RefundTime = (string)refundTime,
							Total = (c.PayAmount - c.RefundAmount).F2ToString("f2").ToDecimal(0),
							Remark = c.Remark,
							GateWayOrderId = (string.IsNullOrEmpty(c.GateWayOrderId) ? "" : c.GateWayOrderId)
						};
					})
				}
			});
			context.Response.Write(s);
		}

		private void BillDetailStatistical(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["PayTimeRange"].ToInt(0);
			if (!Enum.IsDefined(typeof(EnumTimeRange), num))
			{
				context.Response.Write(this.GetErrorJosn(128, ((Enum)(object)ApiErrorCode.TimeRange_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num2 = context.Request["PayType"].ToInt(0);
			if (num2 != 0 && !Enum.IsDefined(typeof(EnumPaymentType), num2))
			{
				context.Response.Write(this.GetErrorJosn(129, ((Enum)(object)ApiErrorCode.PayType_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num3 = context.Request["OrderType"].ToInt(0);
			if (num3 != 0 && !Enum.IsDefined(typeof(EnumOrderType), num3))
			{
				context.Response.Write(this.GetErrorJosn(130, ((Enum)(object)ApiErrorCode.OrderType_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num4 = context.Request["CollectionType"].ToInt(0);
			if (num4 != 0 && num4 != 1 && num4 != 2)
			{
				num4 = 0;
			}
			int num5 = context.Request["pageIndex"].ToInt(0);
			if (num5 <= 0)
			{
				num5 = 1;
			}
			int num6 = context.Request["pageSize"].ToInt(0);
			if (num6 < 1)
			{
				num6 = 10;
			}
			DateTime? startPayTime = context.Request["StartTime"].ToDateTime();
			DateTime? endPayTime = context.Request["EndTime"].ToDateTime();
			if (num == 6 && !startPayTime.HasValue && !startPayTime.HasValue)
			{
				context.Response.Write(this.GetErrorJosn(104, ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription() + "：自定义时间范围时开始时间和结束时间至少要一个有值"));
				context.Response.End();
			}
			StoreCollectionsQuery storeCollectionsQuery = new StoreCollectionsQuery();
			if (num3 != 2)
			{
				storeCollectionsQuery.OrderIds = OrderHelper.GetStoreFinishOrderIds(storeIdBySessionId);
			}
			DateTime dateTime;
			switch (num)
			{
			case 1:
			{
				StoreCollectionsQuery storeCollectionsQuery6 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				storeCollectionsQuery6.StartPayTime = dateTime.Date;
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 3:
			{
				StoreCollectionsQuery storeCollectionsQuery5 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery5.StartPayTime = dateTime.AddDays((double)(-DateTime.Now.Day));
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 2:
			{
				StoreCollectionsQuery storeCollectionsQuery4 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery4.StartPayTime = dateTime.AddDays(-7.0);
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 4:
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				DateTime value = dateTime.AddMonths(-1);
				value = value.AddDays((double)(-value.Day));
				storeCollectionsQuery.StartPayTime = value;
				StoreCollectionsQuery storeCollectionsQuery3 = storeCollectionsQuery;
				dateTime = value.AddMonths(1);
				storeCollectionsQuery3.EndPayTime = dateTime.AddSeconds(-1.0);
				break;
			}
			case 5:
			{
				StoreCollectionsQuery storeCollectionsQuery2 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery2.StartPayTime = dateTime.AddMonths(-3);
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 6:
				storeCollectionsQuery.StartPayTime = startPayTime;
				storeCollectionsQuery.EndPayTime = endPayTime;
				break;
			}
			if (num4 != 0)
			{
				storeCollectionsQuery.CollectionType = num4;
			}
			if (num3 != 0)
			{
				storeCollectionsQuery.OrderType = num3;
			}
			if (num2 != 0)
			{
				storeCollectionsQuery.PaymentTypeId = num2;
			}
			storeCollectionsQuery.PageIndex = num5;
			storeCollectionsQuery.PageSize = num6;
			storeCollectionsQuery.StoreId = storeIdBySessionId;
			decimal num7 = default(decimal);
			decimal num8 = default(decimal);
			decimal num9 = default(decimal);
			PageModel<StoreCollectionInfo> storeCollectionInfos = StoresHelper.GetStoreCollectionInfos(storeCollectionsQuery, out num7, out num8, out num9);
			IEnumerable<StoreCollectionInfo> enumerable = from e in storeCollectionInfos.Models.ToList()
			where e.GateWay == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1)
			select e;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeCollectionInfos.Total,
					AmountTotal = num7.F2ToString("f2").ToDecimal(0),
					StoreTotal = num8.F2ToString("f2").ToDecimal(0),
					PlatTotal = num9.F2ToString("f2").ToDecimal(0),
					List = storeCollectionInfos.Models.ToList().Select(delegate(StoreCollectionInfo c)
					{
						string serialNumber = c.SerialNumber;
						DateTime dateTime2 = c.CreateTime;
						string createTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						object finishTime;
						if (!c.FinishTime.HasValue)
						{
							dateTime2 = c.PayTime;
							finishTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						else
						{
							dateTime2 = c.FinishTime.Value;
							finishTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						decimal payAmount = c.PayAmount.F2ToString("f2").ToDecimal(0);
						string orderId = string.IsNullOrEmpty(c.OrderId) ? "" : c.OrderId;
						int? paymentTypeId = c.PaymentTypeId;
						string paymentTypeName = c.PaymentTypeName;
						dateTime2 = c.PayTime;
						string payTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						int? userId = c.UserId;
						bool isStoreCollection = c.GateWay == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1);
						bool isOfflineOrder = string.IsNullOrEmpty(c.OrderId);
						int status = c.Status;
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)(EnumStoreCollectionStatus)c.Status, 0);
						int orderType = c.OrderType;
						string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)(EnumOrderType)c.OrderType, 0);
						decimal refundAmount = c.RefundAmount.F2ToString("f2").ToDecimal(0);
						object refundTime;
						if (!c.RefundTime.HasValue)
						{
							refundTime = "";
						}
						else
						{
							dateTime2 = c.RefundTime.Value;
							refundTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						return new
						{
							SerialNumber = serialNumber,
							CreateTime = createTime,
							FinishTime = (string)finishTime,
							PayAmount = payAmount,
							OrderId = orderId,
							PaymentTypeId = paymentTypeId,
							PaymentTypeName = paymentTypeName,
							PayTime = payTime,
							UserId = userId,
							IsStoreCollection = isStoreCollection,
							IsOfflineOrder = isOfflineOrder,
							Status = status,
							StatusText = enumDescription,
							OrderType = orderType,
							OrderTypeText = enumDescription2,
							RefundAmount = refundAmount,
							RefundTime = (string)refundTime,
							Total = (c.PayAmount - c.RefundAmount).F2ToString("f2").ToDecimal(0),
							Remark = c.Remark,
							GateWayOrderId = (string.IsNullOrEmpty(c.GateWayOrderId) ? "" : c.GateWayOrderId)
						};
					})
				}
			});
			context.Response.Write(s);
		}

		private void GetOfflineOrderList(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["PayTimeRange"].ToInt(0);
			if (!Enum.IsDefined(typeof(EnumTimeRange), num))
			{
				context.Response.Write(this.GetErrorJosn(128, ((Enum)(object)ApiErrorCode.TimeRange_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num2 = context.Request["PayType"].ToInt(0);
			if (num2 != 0 && !Enum.IsDefined(typeof(EnumPaymentType), num2))
			{
				context.Response.Write(this.GetErrorJosn(129, ((Enum)(object)ApiErrorCode.PayType_NoDefined).ToDescription()));
				context.Response.End();
			}
			int num3 = context.Request["CollectionType"].ToInt(0);
			if (num3 != 0 && num3 != 1 && num3 != 2)
			{
				num3 = 0;
			}
			int num4 = context.Request["pageIndex"].ToInt(0);
			if (num4 <= 0)
			{
				num4 = 1;
			}
			int num5 = context.Request["pageSize"].ToInt(0);
			if (num5 < 1)
			{
				num5 = 10;
			}
			DateTime? startPayTime = context.Request["StartTime"].ToDateTime();
			DateTime? endPayTime = context.Request["EndTime"].ToDateTime();
			if (num == 6 && !startPayTime.HasValue && !startPayTime.HasValue)
			{
				context.Response.Write(this.GetErrorJosn(104, ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription() + "：自定义时间范围时开始时间和结束时间至少要一个有值"));
				context.Response.End();
			}
			StoreCollectionsQuery storeCollectionsQuery = new StoreCollectionsQuery();
			DateTime dateTime;
			switch (num)
			{
			case 1:
			{
				StoreCollectionsQuery storeCollectionsQuery6 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				storeCollectionsQuery6.StartPayTime = dateTime.Date;
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 3:
			{
				StoreCollectionsQuery storeCollectionsQuery5 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery5.StartPayTime = dateTime.AddDays((double)(-DateTime.Now.Day));
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 2:
			{
				StoreCollectionsQuery storeCollectionsQuery4 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery4.StartPayTime = dateTime.AddDays(-7.0);
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 4:
			{
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				DateTime value = dateTime.AddMonths(-1);
				value = value.AddDays((double)(-value.Day));
				storeCollectionsQuery.StartPayTime = value;
				StoreCollectionsQuery storeCollectionsQuery3 = storeCollectionsQuery;
				dateTime = value.AddMonths(1);
				storeCollectionsQuery3.EndPayTime = dateTime.AddSeconds(-1.0);
				break;
			}
			case 5:
			{
				StoreCollectionsQuery storeCollectionsQuery2 = storeCollectionsQuery;
				dateTime = DateTime.Now;
				dateTime = dateTime.Date;
				storeCollectionsQuery2.StartPayTime = dateTime.AddMonths(-3);
				storeCollectionsQuery.EndPayTime = DateTime.Now;
				break;
			}
			case 6:
				storeCollectionsQuery.StartPayTime = startPayTime;
				storeCollectionsQuery.EndPayTime = endPayTime;
				break;
			}
			if (num3 != 0)
			{
				storeCollectionsQuery.CollectionType = num3;
			}
			storeCollectionsQuery.OrderType = 2;
			if (num2 != 0)
			{
				storeCollectionsQuery.PaymentTypeId = num2;
			}
			storeCollectionsQuery.PageIndex = num4;
			storeCollectionsQuery.PageSize = num5;
			storeCollectionsQuery.StoreId = storeIdBySessionId;
			decimal num6 = default(decimal);
			decimal num7 = default(decimal);
			decimal num8 = default(decimal);
			PageModel<StoreCollectionInfo> storeCollectionInfos = StoresHelper.GetStoreCollectionInfos(storeCollectionsQuery, out num6, out num7, out num8);
			IEnumerable<StoreCollectionInfo> enumerable = from e in storeCollectionInfos.Models.ToList()
			where e.GateWay == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1)
			select e;
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeCollectionInfos.Total,
					AmountTotal = num6.F2ToString("f2"),
					StoreTotal = num7.F2ToString("f2"),
					PlatTotal = num8.F2ToString("f2"),
					List = storeCollectionInfos.Models.ToList().Select(delegate(StoreCollectionInfo c)
					{
						string serialNumber = c.SerialNumber;
						DateTime dateTime2 = c.CreateTime;
						string createTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						object finishTime;
						if (!c.FinishTime.HasValue)
						{
							dateTime2 = c.PayTime;
							finishTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						else
						{
							dateTime2 = c.FinishTime.Value;
							finishTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						string payAmount = c.PayAmount.F2ToString("f2");
						string orderId = string.IsNullOrEmpty(c.OrderId) ? "" : c.OrderId;
						int? paymentTypeId = c.PaymentTypeId;
						string paymentTypeName = c.PaymentTypeName;
						dateTime2 = c.PayTime;
						string payTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						int? userId = c.UserId;
						bool isStoreCollection = c.GateWay == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1);
						bool isOfflineOrder = string.IsNullOrEmpty(c.OrderId);
						int status = c.Status;
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)(EnumStoreCollectionStatus)c.Status, 0);
						int orderType = c.OrderType;
						string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)(EnumOrderType)c.OrderType, 0);
						string refundAmount = ((c.RefundAmount > decimal.Zero) ? "-" : "") + c.RefundAmount.F2ToString("f2");
						object refundTime;
						if (!c.RefundTime.HasValue)
						{
							refundTime = "";
						}
						else
						{
							dateTime2 = c.RefundTime.Value;
							refundTime = dateTime2.ToString("yyyy-MM-dd HH:mm:ss");
						}
						return new
						{
							SerialNumber = serialNumber,
							CreateTime = createTime,
							FinishTime = (string)finishTime,
							PayAmount = payAmount,
							OrderId = orderId,
							PaymentTypeId = paymentTypeId,
							PaymentTypeName = paymentTypeName,
							PayTime = payTime,
							UserId = userId,
							IsStoreCollection = isStoreCollection,
							IsOfflineOrder = isOfflineOrder,
							Status = status,
							StatusText = enumDescription,
							OrderType = orderType,
							OrderTypeText = enumDescription2,
							RefundAmount = refundAmount,
							RefundTime = (string)refundTime,
							Total = (c.PayAmount - c.RefundAmount).F2ToString("f2"),
							Remark = c.Remark,
							GateWayOrderId = (string.IsNullOrEmpty(c.GateWayOrderId) ? "" : c.GateWayOrderId)
						};
					})
				}
			});
			context.Response.Write(s);
		}

		private void OfflinePay(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			decimal payAmount = context.Request["PayAmount"].ToDecimal(0);
			string text = Globals.StripAllTags(context.Request["Remark"].ToNullString());
			StoreCollectionInfo storeCollectionInfo = new StoreCollectionInfo();
			storeCollectionInfo.CreateTime = DateTime.Now;
			storeCollectionInfo.FinishTime = DateTime.Now;
			storeCollectionInfo.PayTime = DateTime.Now;
			storeCollectionInfo.GateWay = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 1);
			storeCollectionInfo.PaymentTypeId = 3;
			storeCollectionInfo.PaymentTypeName = EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashPay, 0);
			storeCollectionInfo.OrderType = 2;
			storeCollectionInfo.PayAmount = payAmount;
			storeCollectionInfo.RefundAmount = decimal.Zero;
			storeCollectionInfo.Remark = (string.IsNullOrEmpty(text) ? "门店线下收款" : text);
			storeCollectionInfo.SerialNumber = Globals.GetGenerateId();
			storeCollectionInfo.Status = 1;
			storeCollectionInfo.StoreId = storeIdBySessionId;
			storeCollectionInfo.UserId = 0;
			if (StoresHelper.AddStoreCollectionInfo(storeCollectionInfo))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						storeCollectionInfo.SerialNumber
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
			}
		}

		private void GetNoStockProductList(HttpContext context)
		{
			int storeId = this.GetStoreIdBySessionId(context);
			IList<StoreProductModel> noStockStoreProductList = StoresHelper.GetNoStockStoreProductList(storeId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = noStockStoreProductList.Count,
					List = from c in noStockStoreProductList
					select new
					{
						StoreId = storeId,
						ProductId = c.ProductId,
						Stock = c.Stock,
						ProductCode = c.ProductCode,
						ProductName = c.ProductName,
						ImageUrl = this.GetImageFullPath(c.ThumbnailUrl410)
					}
				}
			});
			context.Response.Write(s);
		}

		private void GetStoreBaseInfo(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					StoreId = storeInfoBySessionId.StoreId,
					StoreName = storeInfoBySessionId.StoreName,
					State = storeInfoBySessionId.State,
					Address = storeInfoBySessionId.Address,
					RegionId = storeInfoBySessionId.RegionId,
					RegionName = RegionHelper.GetFullRegion(storeInfoBySessionId.RegionId, " ", true, 0),
					Cellphone = storeInfoBySessionId.Tel,
					ContactMan = storeInfoBySessionId.ContactMan,
					CloseStatus = storeInfoBySessionId.CloseStatus,
					StoreImages = this.GetFullPathStoreImages(storeInfoBySessionId.StoreImages)
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private string GetFullPathStoreImages(string storeImages)
		{
			string text = "";
			string imageUrl = "";
			if (string.IsNullOrEmpty(storeImages))
			{
				return "";
			}
			string[] array = storeImages.Split(',');
			foreach (string text2 in array)
			{
				if (!text2.ToLower().StartsWith("http://") && text2.IndexOf("thum_") == -1)
				{
					int num = text2.LastIndexOf("/");
					imageUrl = text2.Substring(0, num + 1) + "thum_" + text2.Substring(num + 1);
				}
				text = text + this.GetImageFullPath(imageUrl) + ",";
			}
			return text.TrimEnd(',');
		}

		private IList<string> GetImagesFullPath(string images, char splitchar = '|')
		{
			IList<string> list = new List<string>();
			if (string.IsNullOrEmpty(images))
			{
				return list;
			}
			string[] array = images.Split(splitchar);
			foreach (string imageUrl in array)
			{
				list.Add(this.GetImageFullPath(imageUrl));
			}
			return list;
		}

		private void UpdateStoreBaseInfo(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			StoresInfo storeById = DepotHelper.GetStoreById(storeIdBySessionId);
			string text = Globals.StripAllTags(context.Request["CellPhone"].ToNullString());
			int num = context.Request["RegionId"].ToInt(0);
			string text2 = Globals.StripAllTags(context.Request["Address"].ToNullString());
			string text3 = Globals.StripAllTags(context.Request["StoreImages"].ToNullString());
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2) || num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
				context.Response.End();
			}
			if (!DataHelper.IsMobile(text) && !DataHelper.IsPhone(text))
			{
				context.Response.Write(this.GetErrorJosn(102, ((Enum)(object)ApiErrorCode.Format_Eroor).ToDescription()));
				context.Response.End();
			}
			string fullRegion = RegionHelper.GetFullRegion(num, " ", true, 0);
			if (fullRegion == string.Empty)
			{
				context.Response.Write(this.GetErrorJosn(127, ((Enum)(object)ApiErrorCode.RegionId_Error).ToDescription()));
				context.Response.End();
			}
			if (!string.IsNullOrEmpty(text3))
			{
				storeById.StoreImages = text3.ToLower().Replace(Globals.HostPath(context.Request.Url).ToLower(), "").Replace("thum_", "")
					.TrimEnd(',');
			}
			else
			{
				storeById.StoreImages = "";
			}
			storeById.RegionId = num;
			storeById.FullRegionPath = RegionHelper.GetFullPath(storeById.RegionId, true);
			storeById.Address = text2;
			storeById.Tel = text;
			if (StoresHelper.UpdateStore(storeById))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
			}
		}

		private void GetExpressList(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			IList<ExpressCompanyInfo> allExpress = ExpressHelper.GetAllExpress(false);
			string s = JsonConvert.SerializeObject(new
			{
				Result = from c in allExpress
				select new
				{
					ExpressName = c.Name,
					Kuaidi100Code = c.Kuaidi100Code,
					TaobaoCode = c.TaobaoCode
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetRegionList(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["RegionId"].ToInt(0);
			int depth = context.Request["Depth"].ToInt(0);
			IDictionary<int, string> dictionary = null;
			dictionary = ((depth != 0) ? ((depth != 1) ? ((depth != 2) ? ((depth != 3) ? RegionHelper.GetAllProvinces(false) : RegionHelper.GetStreets(num, false)) : RegionHelper.GetCountys(num, false)) : RegionHelper.GetCitys(num, false)) : RegionHelper.GetAllProvinces(false));
			string s = JsonConvert.SerializeObject(new
			{
				Result = from c in dictionary
				select new
				{
					RegionId = c.Key,
					RegionName = c.Value,
					HasSubRegion = RegionHelper.HasChild(c.Key, depth + 1)
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetMessageList(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["MessageType"].ToInt(0);
			if (!Enum.IsDefined(typeof(EnumPushStoreAction), num))
			{
				context.Response.Write(this.GetErrorJosn(515, ((Enum)(object)ApiErrorCode.MsgType_Error).ToDescription()));
			}
			else
			{
				List<IOSAppPushRecord> list = (from c in VShopHelper.GetPushRecordsOfMsgType(storeIdBySessionId, num)
				select new IOSAppPushRecord
				{
					Type = c.PushType,
					PushTitle = c.PushTitle,
					PushContent = c.PushContent,
					PushSendTime = c.PushSendTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
					Extras = c.Extras,
					PushRecordId = c.PushRecordId,
					PushMsgType = c.PushMsgType.Value
				} into c
				orderby c.PushSendTime descending
				select c).ToList();
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = list.Count,
						List = list
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private void MessageStatistical(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					StockWarningMsgCount = VShopHelper.GetPushRecordCountOfMsgType(storeIdBySessionId, 10),
					WaitSendGoodsMsgCount = VShopHelper.GetPushRecordCountOfMsgType(storeIdBySessionId, 8),
					OrderPayedMsgCount = VShopHelper.GetPushRecordCountOfMsgType(storeIdBySessionId, 9),
					WaitConfirmMsgCount = VShopHelper.GetPushRecordCountOfMsgType(storeIdBySessionId, 7),
					RefundApplyMsgCount = VShopHelper.GetPushRecordCountOfMsgType(storeIdBySessionId, 5),
					ReturnApplyMsgCount = VShopHelper.GetPushRecordCountOfMsgType(storeIdBySessionId, 6)
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void UpdateStorePassword(HttpContext context)
		{
			string sessionId = context.Request["SessionId"].ToNullString();
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			ManagerInfo managerBySessionId = ManagerHelper.GetManagerBySessionId(sessionId);
			if (managerBySessionId == null)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
			}
			else
			{
				string pass = context.Request["oldPassword"].ToNullString();
				string pass2 = context.Request["newPassword"].ToNullString();
				if (managerBySessionId.Password != Users.EncodePassword(pass, managerBySessionId.PasswordSalt))
				{
					context.Response.Write(this.GetErrorJosn(514, ((Enum)(object)ApiErrorCode.StoreOldPassword_InValid).ToDescription()));
				}
				else
				{
					managerBySessionId.Password = Users.EncodePassword(pass2, managerBySessionId.PasswordSalt);
					if (ManagerHelper.Update(managerBySessionId))
					{
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "SUCCESS"
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
					else
					{
						context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
					}
				}
			}
		}

		private void ReturnOnStore(HttpContext context)
		{
			string physicalApplicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
			string text = context.Request["OrderId"].ToNullString();
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			string text2 = Globals.StripAllTags(context.Request["SkuId"].ToNullString());
			int num = context.Request["RefundType"].ToInt(0);
			string adminRemark = Globals.StripAllTags(context.Request["Remark"].ToNullString());
			decimal num2 = context.Request["RefundMoney"].ToDecimal(0);
			string text3 = "";
			string text4 = "";
			string text5 = "";
			text3 = Globals.StripAllTags(context.Request["BankName"].ToNullString());
			text4 = Globals.StripAllTags(context.Request["BankAccountName"].ToNullString());
			text5 = Globals.StripAllTags(context.Request["BankAccountNo"].ToNullString());
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else if (!Enum.IsDefined(typeof(RefundTypes), num))
				{
					context.Response.Write(this.GetErrorJosn(125, ((Enum)(object)ApiErrorCode.RefundType_Error).ToDescription()));
				}
				else if (num == 2 && (string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(text4) || string.IsNullOrEmpty(text5)))
				{
					context.Response.Write(this.GetErrorJosn(131, ((Enum)(object)ApiErrorCode.Must_WriteBankInfo).ToDescription()));
				}
				else
				{
					MemberInfo user = Users.GetUser(orderInfo.UserId);
					if (num == 1 && (user == null || !user.IsOpenBalance))
					{
						context.Response.Write(this.GetErrorJosn(205, ((Enum)(object)ApiErrorCode.User_NotOpenBlance).ToDescription()));
					}
					else
					{
						LineItemInfo lineItemInfo = null;
						if (!string.IsNullOrEmpty(text2))
						{
							if (!orderInfo.LineItems.ContainsKey(text2))
							{
								context.Response.Write(this.GetErrorJosn(124, ((Enum)(object)ApiErrorCode.Order_NoSkuId).ToDescription()));
								return;
							}
							lineItemInfo = orderInfo.LineItems[text2];
						}
						if (num2 < decimal.Zero)
						{
							context.Response.Write(this.GetErrorJosn(117, ((Enum)(object)ApiErrorCode.RefundMoney_Error).ToDescription()));
						}
						else
						{
							GroupBuyInfo groupbuy = null;
							if (orderInfo.GroupBuyId > 0)
							{
								groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
							}
							decimal canRefundAmount = orderInfo.GetCanRefundAmount(text2, groupbuy, 0);
							if (num2 == decimal.Zero)
							{
								num2 = canRefundAmount;
							}
							else if (num2 > canRefundAmount)
							{
								context.Response.Write(this.GetErrorJosn(117, ((Enum)(object)ApiErrorCode.RefundMoney_Error).ToDescription()));
								return;
							}
							int num3 = 0;
							num3 = (string.IsNullOrEmpty(text2) ? orderInfo.GetAllQuantity(true) : orderInfo.LineItems[text2].Quantity);
							string returnReason = "用户门店退款";
							string orderId = orderInfo.OrderId;
							if (lineItemInfo != null)
							{
								orderId = lineItemInfo.ItemDescription + lineItemInfo.SKUContent;
							}
							string generateId = Globals.GetGenerateId();
							ReturnInfo returnInfo = new ReturnInfo
							{
								UserRemark = "",
								ReturnReason = returnReason,
								RefundType = (RefundTypes)num,
								RefundGateWay = orderInfo.Gateway,
								RefundOrderId = generateId,
								RefundAmount = num2,
								StoreId = orderInfo.StoreId,
								ApplyForTime = DateTime.Now,
								BankName = text3,
								BankAccountName = text4,
								BankAccountNo = text5,
								HandleStatus = ReturnStatus.Applied,
								OrderId = orderInfo.OrderId,
								SkuId = text2,
								Quantity = num3,
								UserCredentials = "",
								AfterSaleType = AfterSaleTypes.ReturnAndRefund
							};
							if (TradeHelper.StoreApplyForReturn(returnInfo))
							{
								if (orderInfo.IsStoreCollect && OrderHelper.StoreCheckReturn(returnInfo, orderInfo, storeInfoBySessionId.StoreName, num2, adminRemark, true, true))
								{
									Messenger.OrderRefund(user, orderInfo, returnInfo.SkuId);
								}
								string s = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "SUCCESS"
									}
								});
								context.Response.Write(s);
								context.Response.End();
							}
							else
							{
								context.Response.Write(this.GetErrorJosn(126, ((Enum)(object)ApiErrorCode.Return_ApplyError).ToDescription()));
							}
						}
					}
				}
			}
		}

		private void GetSkus(HttpContext context)
		{
			StoresInfo storeInfo = this.GetStoreInfoBySessionId(context);
			string safeIDList = Globals.GetSafeIDList(context.Request["ProductIds"].ToNullString(), ',', true);
			if (string.IsNullOrEmpty(safeIDList))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				IList<StoreProductModel> list = new List<StoreProductModel>();
				string[] array = safeIDList.Split(',');
				foreach (string obj in array)
				{
					StoreProductModel storeProductInfoForStoreApp = StoresHelper.GetStoreProductInfoForStoreApp(storeInfo.StoreId, obj.ToInt(0));
					if (storeProductInfoForStoreApp != null)
					{
						list.Add(storeProductInfoForStoreApp);
					}
				}
				if (list.Count >= 1)
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Products = list.Select(delegate(StoreProductModel c)
							{
								int productId = c.ProductId;
								string productName = c.ProductName;
								int storeId = storeInfo.StoreId;
								string productCode = c.ProductCode;
								object minPriceRate;
								decimal value;
								if (!storeInfo.MinPriceRate.HasValue)
								{
									minPriceRate = "";
								}
								else
								{
									value = storeInfo.MinPriceRate.Value;
									minPriceRate = value.ToString();
								}
								object maxPriceRate;
								if (!storeInfo.MaxPriceRate.HasValue)
								{
									maxPriceRate = "";
								}
								else
								{
									value = storeInfo.MaxPriceRate.Value;
									maxPriceRate = value.ToString();
								}
								return new
								{
									ProductId = productId,
									ProductName = productName,
									StoreId = storeId,
									ProductCode = productCode,
									MinPriceRate = (string)minPriceRate,
									MaxPriceRate = (string)maxPriceRate,
									SkuItems = from d in c.Skus.Values
									select new
									{
										SkuId = d.SkuId,
										ImageUrl = this.GetImageFullPath(c.ThumbnailUrl410),
										Stock = d.Stock,
										StoreStock = d.StoreStock,
										SkuText = ProductHelper.GetSkusBySkuId(d.SkuId, c.ProductId),
										ProductCode = d.SKU,
										SalePrice = d.SalePrice,
										WarningStock = d.WarningStock,
										StoreSalePrice = d.StoreSalePrice.F2ToString("f2")
									}
								};
							})
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					context.Response.Write(this.GetErrorJosn(513, ((Enum)(object)ApiErrorCode.Product_NoRelationStore).ToDescription()));
				}
			}
		}

		private void GetOnShelvesSkus(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			if (storeInfoBySessionId != null && storeInfoBySessionId.CloseStatus)
			{
				int num = context.Request["ProductId"].ToInt(0);
				if (num < 0)
				{
					context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
				}
				else
				{
					DataTable skuStocks = ProductHelper.GetSkuStocks(num.ToString());
					List<ProductSkuModel> list = new List<ProductSkuModel>();
					if (skuStocks.Rows.Count >= 1)
					{
						foreach (DataRow row in skuStocks.Rows)
						{
							ProductSkuModel productSkuModel = new ProductSkuModel();
							productSkuModel.SalePrice = row["SalePrice"].ToDecimal(0).F2ToString("f2");
							productSkuModel.SkuContent = ProductHelper.GetSkusBySkuId(row["SkuId"].ToString(), num);
							productSkuModel.SkuId = row["SkuId"].ToString();
							productSkuModel.Stock = row["Stock"].ToString();
							list.Add(productSkuModel);
						}
						int productId = num;
						object productName = skuStocks.Rows[0]["ProductName"];
						object productCode = skuStocks.Rows[0]["ProductCode"];
						object minPriceRate;
						decimal value;
						if (!storeInfoBySessionId.MinPriceRate.HasValue)
						{
							minPriceRate = "";
						}
						else
						{
							value = storeInfoBySessionId.MinPriceRate.Value;
							minPriceRate = value.ToString();
						}
						object maxPriceRate;
						if (!storeInfoBySessionId.MaxPriceRate.HasValue)
						{
							maxPriceRate = "";
						}
						else
						{
							value = storeInfoBySessionId.MaxPriceRate.Value;
							maxPriceRate = value.ToString();
						}
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Products = new
								{
									ProductId = productId,
									ProductName = productName,
									ProductCode = productCode,
									MinPriceRate = (string)minPriceRate,
									MaxPriceRate = (string)maxPriceRate,
									ProductSku = list
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
					else
					{
						context.Response.Write(this.GetErrorJosn(513, ((Enum)(object)ApiErrorCode.Product_NoRelationStore).ToDescription()));
					}
				}
			}
		}

		private void GetGoodsOnDoorStatistical(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			StoreStatisticsInfo storeOrderStatistics = StoresHelper.GetStoreOrderStatistics(storeIdBySessionId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					WaitPickOnDoorTotal = storeOrderStatistics.WaitPickGoodsTotal,
					WaitConfirmTotal = storeOrderStatistics.WaitConfirmTotal
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetOrderDetail(HttpContext context)
		{
			string text = context.Request["OrderId"].ToNullString();
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
			}
			else
			{
				OrderInfo order = OrderHelper.GetOrderInfo(text);
				if (order == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (order.StoreId != storeIdBySessionId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else
				{
					string text2 = "0.00";
					decimal? deductionMoney = order.DeductionMoney;
					text2 = ((deductionMoney.GetValueOrDefault() > default(decimal) && deductionMoney.HasValue) ? "-" : "") + order.DeductionMoney.ToDecimal(0).F2ToString("f2");
					string orderId = order.OrderId;
					DateTime dateTime;
					object takeCodeUsedTime;
					if (!string.IsNullOrEmpty(order.TakeTime))
					{
						dateTime = order.TakeTime.ToDateTime().Value;
						takeCodeUsedTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					else
					{
						takeCodeUsedTime = "";
					}
					string takeCode = order.TakeCode.ToNullString().ToLower().Replace("ysc", "");
					string statusText = (order.OrderType == OrderType.ServiceOrder) ? ((Enum)(object)this.GetOrderStatus(order, null)).ToDescription() : ((order.ShippingModeId != -2 || (order.OrderStatus != OrderStatus.BuyerAlreadyPaid && (order.OrderStatus != OrderStatus.WaitBuyerPay || !(order.Gateway == "hishop.plugins.payment.payonstore")))) ? ((order.OrderStatus == OrderStatus.WaitBuyerPay && order.Gateway == "hishop.plugins.payment.podrequest") ? "待发货" : this.GetDadaStatus(order.OrderStatus, order.DadaStatus, order.ExpressCompanyName)) : (order.IsConfirm ? "待上门自提" : "待确认"));
					OrderStatus orderStatus = order.OrderStatus;
					OrderItemStatus itemStatus = order.ItemStatus;
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0);
					dateTime = order.OrderDate;
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							OrderId = orderId,
							TakeCodeIsUsed = false,
							TakeCodeUsedTime = (string)takeCodeUsedTime,
							TakeCode = takeCode,
							StatusText = statusText,
							Status = (int)orderStatus,
							ItemStatus = (int)itemStatus,
							ItemStatusText = enumDescription,
							OrderDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							ShipTo = order.ShipTo,
							ShipToDate = order.ShipToDate,
							Cellphone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone),
							Address = order.Address,
							OrderTotal = order.GetTotal(false).F2ToString("f2"),
							FreightFreePromotionName = order.FreightFreePromotionName,
							ReducedPromotionName = order.ReducedPromotionName,
							ReducedPromotionAmount = ((order.ReducedPromotionAmount > decimal.Zero) ? "-" : "") + order.ReducedPromotionAmount.F2ToString("f2"),
							SentTimesPointPromotionName = order.SentTimesPointPromotionName,
							CanBackReturn = TradeHelper.IsCanBackReturn(order),
							CanCashierReturn = order.IsStoreCollect,
							PaymentType = order.PaymentType,
							DeductionPoints = order.DeductionPoints,
							CouponAmount = ((order.CouponValue > decimal.Zero) ? "-" : "") + order.CouponValue.F2ToString("f2"),
							CouponName = order.CouponName,
							DeductionMoney = text2,
							RefundAmount = ((order.RefundAmount > decimal.Zero) ? "-" : "") + order.RefundAmount.F2ToString("f2"),
							Remark = order.Remark,
							InvoiceTitle = order.InvoiceTitle,
							Tax = order.Tax.F2ToString("f2"),
							InvoiceTaxpayerNumber = order.InvoiceTaxpayerNumber,
							AdjustedFreight = order.AdjustedFreight.F2ToString("f2"),
							Freight = order.Freight.F2ToString("f2"),
							ShippingModeId = order.ShippingModeId,
							OrderType = order.OrderType,
							BalanceAmount = order.BalanceAmount,
							LineItems = from d in order.LineItems.Keys
							select new
							{
								Status = order.LineItems[d].Status,
								StatusText = order.LineItems[d].StatusText,
								Id = order.LineItems[d].SkuId,
								Name = order.LineItems[d].ItemDescription,
								Price = order.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
								Amount = order.LineItems[d].ShipmentQuantity,
								Image = this.GetImageFullPath(order.LineItems[d].ThumbnailsUrl),
								SkuText = order.LineItems[d].SKUContent,
								ProductId = order.LineItems[d].ProductId,
								PromotionName = order.LineItems[d].PromotionName
							},
							Gifts = from g in order.Gifts
							select new
							{
								GiftId = g.GiftId,
								GiftName = g.GiftName,
								PromoteType = g.PromoteType,
								Quantity = g.Quantity,
								ImageUrl = this.GetImageFullPath(g.ThumbnailsUrl)
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void GetServiceOrderDetail(HttpContext context)
		{
			string text = context.Request["OrderId"].ToNullString();
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
			}
			else
			{
				OrderInfo order = OrderHelper.GetOrderInfo(text);
				if (order == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (order.StoreId != storeIdBySessionId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else
				{
					IList<OrderVerificationItemInfo> orderVerificationItems = TradeHelper.GetOrderVerificationItems(text);
					string text2 = "0.00";
					decimal? deductionMoney = order.DeductionMoney;
					text2 = ((deductionMoney.GetValueOrDefault() > default(decimal) && deductionMoney.HasValue) ? "-" : "") + order.DeductionMoney.ToDecimal(0).F2ToString("f2");
					ServiceOrderStatus orderStatus = this.GetOrderStatus(order, orderVerificationItems);
					LineItemInfo oneorditem = order.LineItems.FirstOrDefault().Value;
					string s2 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							OrderId = order.OrderId,
							StatusText = ((Enum)(object)orderStatus).ToDescription(),
							Status = orderStatus.GetHashCode(),
							ItemStatus = (int)order.ItemStatus,
							ItemStatusText = EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0),
							OrderDate = order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
							OrderTotal = order.GetTotal(false).F2ToString("f2"),
							OrderAmount = order.GetAmount(false).F2ToString("f2"),
							CanBackReturn = TradeHelper.IsCanBackReturn(order),
							CanCashierReturn = order.IsStoreCollect,
							PaymentType = order.PaymentType,
							DeductionPoints = order.DeductionPoints,
							CouponAmount = order.CouponValue,
							CouponName = order.CouponName,
							DeductionMoney = text2,
							RefundAmount = order.RefundAmount,
							Remark = order.Remark,
							InvoiceTitle = order.InvoiceTitle,
							Tax = order.Tax,
							AdjustedFreight = order.AdjustedFreight.F2ToString("f2"),
							Freight = order.Freight.F2ToString("f2"),
							LineItems = from d in order.LineItems.Keys
							select new
							{
								Status = order.LineItems[d].Status,
								StatusText = order.LineItems[d].StatusText,
								Id = order.LineItems[d].SkuId,
								Name = order.LineItems[d].ItemDescription,
								Price = order.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
								Amount = order.LineItems[d].ShipmentQuantity,
								Image = this.GetImageFullPath(order.LineItems[d].ThumbnailsUrl),
								SkuText = order.LineItems[d].SKUContent,
								ProductId = order.LineItems[d].ProductId,
								PromotionName = order.LineItems[d].PromotionName
							},
							VerificationItems = from d in orderVerificationItems
							select new
							{
								code = this.GetShowVerificationPassword(d),
								status = d.VerificationStatus,
								qrcode = this.GetVerificationCodeQRCodePath(d),
								IsValid = oneorditem.IsValid,
								ValidStartDate = oneorditem.ValidStartDate,
								ValidEndDate = oneorditem.ValidEndDate,
								IsRefund = oneorditem.IsRefund,
								IsOverRefund = oneorditem.IsOverRefund
							},
							InputItems = from g in (from t in order.InputItems
							group t by t.InputFieldGroup).ToDictionary((IGrouping<int, OrderInputItemInfo> t) => t.Key, (IGrouping<int, OrderInputItemInfo> t) => t.ToList())
							select 
								from s in g.Value
								select new
								{
									title = s.InputFieldTitle,
									vtype = s.InputFieldType,
									value = s.InputFieldValue
								}
						}
					});
					context.Response.Write(s2);
					context.Response.End();
				}
			}
		}

		private string GetVerificationCodeQRCodePath(OrderVerificationItemInfo data)
		{
			string result = "";
			if (data != null && !string.IsNullOrWhiteSpace(data.VerificationPassword))
			{
				string format = Globals.FullPath("/Storage/master/ServiceQRCode/{0}_{1}.png");
				result = string.Format(format, data.Id, data.VerificationPassword);
			}
			return result;
		}

		private ServiceOrderStatus GetOrderStatus(OrderInfo order, IList<OrderVerificationItemInfo> orderVerificationItems = null)
		{
			ServiceOrderStatus result = ServiceOrderStatus.Finished;
			if (order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				result = ServiceOrderStatus.WaitBuyerPay;
			}
			else if (order.OrderStatus == OrderStatus.Closed)
			{
				result = ServiceOrderStatus.Closed;
			}
			else if (order.OrderStatus == OrderStatus.Finished)
			{
				result = ServiceOrderStatus.Finished;
			}
			else
			{
				IList<OrderVerificationItemInfo> source = orderVerificationItems;
				if (orderVerificationItems == null)
				{
					source = TradeHelper.GetOrderVerificationItems(order.OrderId);
				}
				if (source.Any((OrderVerificationItemInfo d) => d.VerificationStatus == 0.GetHashCode()))
				{
					result = ServiceOrderStatus.WaitConsumption;
				}
				else if (source.Any((OrderVerificationItemInfo d) => d.VerificationStatus == 3.GetHashCode()))
				{
					result = ServiceOrderStatus.Expired;
				}
				else if (source.Count() > 0 && source.Count(delegate(OrderVerificationItemInfo d)
				{
					int verificationStatus = d.VerificationStatus;
					VerificationStatus verificationStatus2 = VerificationStatus.Refunded;
					int result2;
					if (verificationStatus != verificationStatus2.GetHashCode())
					{
						int verificationStatus3 = d.VerificationStatus;
						verificationStatus2 = VerificationStatus.ApplyRefund;
						result2 = ((verificationStatus3 != verificationStatus2.GetHashCode()) ? 1 : 0);
					}
					else
					{
						result2 = 0;
					}
					return (byte)result2 != 0;
				}) == 0)
				{
					result = ServiceOrderStatus.Refunding;
				}
			}
			return result;
		}

		private string GetShowVerificationPassword(OrderVerificationItemInfo data)
		{
			string result = "";
			if (data != null)
			{
				result = data.VerificationPassword;
				int verificationStatus = data.VerificationStatus;
				VerificationStatus verificationStatus2 = VerificationStatus.Applied;
				int num;
				if (verificationStatus != verificationStatus2.GetHashCode())
				{
					int verificationStatus3 = data.VerificationStatus;
					verificationStatus2 = VerificationStatus.ApplyRefund;
					num = ((verificationStatus3 == verificationStatus2.GetHashCode()) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				result = ((num == 0) ? Regex.Replace(result, "(\\d{4})(\\d{4})(\\d+)", "$1$2$3") : Regex.Replace(result, "(\\d{4})(\\d{4})(\\d+)", "$1****$3"));
			}
			return result;
		}

		private void BatchUpdateSkuStock(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			string text = context.Request["SkuIds"].ToNullString();
			string safeIDList = Globals.GetSafeIDList(context.Request["Stocks"].ToNullString(), ',', false);
			string text2 = context.Request["StoreSalePrices"].ToNullString();
			string safeIDList2 = Globals.GetSafeIDList(context.Request["WarningStocks"].ToNullString(), ',', false);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(104, "规格ID" + ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription()));
			}
			else if (text.Split(',').Length != safeIDList.Split(',').Length || text.Split(',').Length != text2.Split(',').Length || text.Split(',').Length != safeIDList2.Split(',').Length)
			{
				context.Response.Write(this.GetErrorJosn(107, "规格ID与其他修改数目不一致" + ((Enum)(object)ApiErrorCode.Paramter_Diffrent).ToDescription()));
			}
			else
			{
				string text3 = "";
				string[] array = text.Split(',');
				foreach (string str in array)
				{
					text3 += ((text3 == "") ? "" : ",");
					text3 = text3 + "'" + str + "'";
				}
				int num = 0;
				decimal num2 = default(decimal);
				int num3 = 0;
				List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
				List<StoreSKUInfo> list2 = new List<StoreSKUInfo>();
				int num4 = 0;
				decimal num5 = default(decimal);
				decimal num6 = default(decimal);
				IList<StoreSKUInfo> storeStockInfosBySkuIds = StoresHelper.GetStoreStockInfosBySkuIds(storeInfoBySessionId.StoreId, text3);
				int num7 = 0;
				int updateType = 0;
				foreach (StoreSKUInfo item in storeStockInfosBySkuIds)
				{
					num = safeIDList.Split(',')[num7].ToInt(0);
					num4 = item.Stock;
					num5 = item.StoreSalePrice;
					num6 = item.WarningStock;
					num2 = text2.Split(',')[num7].ToDecimal(0);
					num3 = safeIDList2.Split(',')[num7].ToInt(0);
					if (num4 != num || (decimal)num3 != num6 || (num2 != num5 && storeInfoBySessionId.IsModifyPrice))
					{
						SKUItem skuItemBySkuId = ProductHelper.GetSkuItemBySkuId(item.SkuId, item.StoreId);
						decimal salePrice = skuItemBySkuId.SalePrice;
						if (num2 > decimal.Zero && storeInfoBySessionId.IsModifyPrice)
						{
							if (storeInfoBySessionId.MinPriceRate.ToDecimal(0) > decimal.Zero)
							{
								decimal d = num2;
								decimal value = salePrice;
								decimal? minPriceRate = storeInfoBySessionId.MinPriceRate;
								decimal? nullable = (decimal?)value * minPriceRate;
								if (d < nullable.GetValueOrDefault() && nullable.HasValue)
								{
									context.Response.Write(this.GetErrorJosn(518, "门店价格不能小于平台价格的" + storeInfoBySessionId.MinPriceRate.Value.F2ToString("f2") + "倍！"));
									return;
								}
							}
							if (storeInfoBySessionId.MaxPriceRate.ToDecimal(0) > decimal.Zero)
							{
								decimal d2 = num2;
								decimal value = salePrice;
								decimal? minPriceRate = storeInfoBySessionId.MaxPriceRate;
								decimal? nullable = (decimal?)value * minPriceRate;
								if (d2 > nullable.GetValueOrDefault() && nullable.HasValue)
								{
									context.Response.Write(this.GetErrorJosn(518, "门店价格不能大于平台价格的" + storeInfoBySessionId.MaxPriceRate.Value.F2ToString("f2") + "倍！"));
									return;
								}
							}
							salePrice = num2;
						}
						else if (storeInfoBySessionId.IsModifyPrice)
						{
							salePrice = default(decimal);
							if (storeInfoBySessionId.MinPriceRate.ToDecimal(0) > decimal.One)
							{
								salePrice *= storeInfoBySessionId.MinPriceRate.ToDecimal(0);
							}
							if (storeInfoBySessionId.MaxPriceRate.ToDecimal(0) < decimal.One)
							{
								salePrice *= storeInfoBySessionId.MaxPriceRate.ToDecimal(0);
							}
						}
						else
						{
							updateType = 4;
						}
						item.Stock = num;
						item.StoreSalePrice = num2;
						item.WarningStock = num3;
						list2.Add(item);
						StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
						storeStockLogInfo.ProductId = skuItemBySkuId.ProductId;
						storeStockLogInfo.Remark = DataHelper.CleanSearchString("APP门店管理员调整库存");
						storeStockLogInfo.SkuId = item.SkuId;
						storeStockLogInfo.Operator = storeInfoBySessionId.StoreName;
						storeStockLogInfo.StoreId = storeInfoBySessionId.StoreId;
						storeStockLogInfo.ChangeTime = DateTime.Now;
						storeStockLogInfo.Content = skuItemBySkuId.SKU;
						if (num != num4)
						{
							StoreStockLogInfo storeStockLogInfo2 = storeStockLogInfo;
							storeStockLogInfo2.Content = storeStockLogInfo2.Content + "库存由【" + num4 + "】修改为【" + num + "】";
						}
						if ((decimal)num3 != num6)
						{
							StoreStockLogInfo storeStockLogInfo2 = storeStockLogInfo;
							storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 门店警戒库存从" + num6 + "变为" + num3 + ";";
						}
						if (num2 != num5)
						{
							StoreStockLogInfo storeStockLogInfo2 = storeStockLogInfo;
							storeStockLogInfo2.Content = storeStockLogInfo2.Content + " 门店售价从" + num5 + "变为" + num2 + ";";
						}
						list.Add(storeStockLogInfo);
					}
					num7++;
				}
				if (num7 > 0)
				{
					if (list2.Count > 0)
					{
						StoresHelper.SaveStoreStock(list2, list, updateType);
					}
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS"
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					context.Response.Write(this.GetErrorJosn(513, ((Enum)(object)ApiErrorCode.Product_NoRelationStore).ToDescription()));
				}
			}
		}

		private void BatchUpdateProductStock(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			string safeIDList = Globals.GetSafeIDList(context.Request["ProductIds"].ToNullString(), ',', true);
			string safeIDList2 = Globals.GetSafeIDList(context.Request["Stocks"].ToNullString(), ',', false);
			if (string.IsNullOrEmpty(safeIDList))
			{
				context.Response.Write(this.GetErrorJosn(104, "商品ID" + ((Enum)(object)ApiErrorCode.Empty_Error).ToDescription()));
			}
			else if (safeIDList.Split(',').Length != safeIDList2.Split(',').Length)
			{
				context.Response.Write(this.GetErrorJosn(107, "商品ID与库存数目" + ((Enum)(object)ApiErrorCode.Paramter_Diffrent).ToDescription()));
			}
			else
			{
				int num = 0;
				int num2 = 0;
				List<StoreStockLogInfo> list = new List<StoreStockLogInfo>();
				List<StoreSKUInfo> list2 = new List<StoreSKUInfo>();
				string[] array = safeIDList.Split(',');
				foreach (string obj in array)
				{
					Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
					IList<int> list3 = new List<int>();
					ProductInfo productDetails = ProductHelper.GetProductDetails(obj.ToInt(0), out dictionary, out list3);
					num = safeIDList2.Split(',')[num2].ToInt(0);
					IList<StoreSKUInfo> storeStockInfosByProduct = StoresHelper.GetStoreStockInfosByProduct(storeInfoBySessionId.StoreId, obj.ToInt(0));
					if (storeStockInfosByProduct.Count == 0)
					{
						context.Response.Write(this.GetErrorJosn(513, ((Enum)(object)ApiErrorCode.Product_NoRelationStore).ToDescription()));
						return;
					}
					int num3 = 0;
					foreach (StoreSKUInfo item in storeStockInfosByProduct)
					{
						num3 = item.Stock;
						if (item.Stock != num)
						{
							item.Stock = num;
							list2.Add(item);
							SKUItem sKUItem = productDetails.Skus[item.SkuId];
							StoreStockLogInfo storeStockLogInfo = new StoreStockLogInfo();
							storeStockLogInfo.ProductId = obj.ToInt(0);
							storeStockLogInfo.Remark = DataHelper.CleanSearchString("APP门店管理员调整库存");
							storeStockLogInfo.SkuId = item.SkuId;
							storeStockLogInfo.Operator = storeInfoBySessionId.StoreName;
							storeStockLogInfo.StoreId = storeInfoBySessionId.StoreId;
							storeStockLogInfo.ChangeTime = DateTime.Now;
							storeStockLogInfo.Content = sKUItem.SKU + "库存由【" + num3 + "】修改为【" + num + "】";
							list.Add(storeStockLogInfo);
						}
					}
					num2++;
				}
				if (num2 > 0)
				{
					if (list2.Count > 0)
					{
						StoresHelper.SaveStoreStock(list2, list, 0);
					}
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS"
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					context.Response.Write(this.GetErrorJosn(513, ((Enum)(object)ApiErrorCode.Product_NoRelationStore).ToDescription()));
				}
			}
		}

		private void UnShelvesProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string safeIDList = Globals.GetSafeIDList(context.Request["ProductIds"].ToNullString(), ',', true);
			if (string.IsNullOrEmpty(safeIDList))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else if (StoresHelper.DeleteProduct(storeIdBySessionId, safeIDList.Replace(",", "','")))
			{
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS"
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(513, ((Enum)(object)ApiErrorCode.Product_NoRelationStore).ToDescription()));
			}
		}

		private void GetReplaceDetail(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int storeId = storeInfoBySessionId.StoreId;
			int replaceId = context.Request["replaceId"].ToInt(0);
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(replaceInfo.OrderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				}
				else
				{
					string cellphone = replaceInfo.AdminCellPhone;
					string shipAddress = replaceInfo.AdminShipAddress;
					string shipTo = replaceInfo.AdminShipTo;
					int shipRegionId = 0;
					if (replaceInfo.HandleStatus == ReplaceStatus.Applied || replaceInfo.HandleStatus == ReplaceStatus.Refused)
					{
						cellphone = storeInfoBySessionId.Tel;
						shipAddress = RegionHelper.GetFullRegion(storeInfoBySessionId.RegionId, " ", true, 0) + " " + storeInfoBySessionId.Address;
						shipTo = storeInfoBySessionId.ContactMan;
						shipRegionId = storeInfoBySessionId.RegionId;
					}
					if (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery)
					{
						cellphone = orderInfo.CellPhone;
						shipTo = orderInfo.ShipTo;
					}
					IList<LineItemInfo> source = (from i in orderInfo.LineItems.Values
					where i.SkuId == replaceInfo.SkuId
					select i).ToList();
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							SkuId = replaceInfo.SkuId,
							Cellphone = cellphone,
							AdminRemark = replaceInfo.AdminRemark,
							ShipAddress = shipAddress,
							Address = orderInfo.ShippingRegion + " " + orderInfo.Address,
							ShipTo = shipTo,
							ShipRegionId = shipRegionId,
							ApplyForTime = replaceInfo.ApplyForTime,
							Remark = replaceInfo.UserRemark,
							ShipToDate = orderInfo.ShipToDate,
							Status = (int)replaceInfo.HandleStatus,
							StatusText = EnumDescription.GetEnumDescription((Enum)(object)replaceInfo.HandleStatus, 0),
							DealTime = replaceInfo.AgreedOrRefusedTime,
							Reason = replaceInfo.ReplaceReason,
							ReplaceId = replaceInfo.ReplaceId,
							ShipOrderNumber = replaceInfo.ShipOrderNumber,
							AdminShipTo = replaceInfo.AdminShipTo,
							ExpressCompanyAbb = replaceInfo.ExpressCompanyAbb,
							ExpressCompanyName = replaceInfo.ExpressCompanyName,
							UserConfirmGoodsTime = replaceInfo.UserConfirmGoodsTime,
							UserExpressCompanyAbb = replaceInfo.UserExpressCompanyAbb,
							UserExpressCompanyName = replaceInfo.UserExpressCompanyName,
							UserSendGoodsTime = replaceInfo.UserSendGoodsTime,
							UserShipOrderNumber = replaceInfo.UserShipOrderNumber,
							MerchantsConfirmGoodsTime = replaceInfo.MerchantsConfirmGoodsTime,
							OrderId = replaceInfo.OrderId,
							Quantity = replaceInfo.Quantity,
							UserCredentials = this.GetImagesFullPath(replaceInfo.UserCredentials, '|'),
							ShowAgreenBtn = (replaceInfo.HandleStatus == ReplaceStatus.Applied),
							ShowRefuseBtn = (replaceInfo.HandleStatus == ReplaceStatus.Applied),
							ShowGetGoodsBtn = (replaceInfo.HandleStatus == ReplaceStatus.UserDelivery),
							ProductInfo = from l in source
							select new
							{
								ProductName = l.ItemDescription,
								SKU = l.SKU,
								SKUContent = l.SKUContent,
								Price = l.ItemAdjustedPrice,
								Quantity = l.ShipmentQuantity,
								ThumbnailsUrl = this.GetImageFullPath(l.ThumbnailsUrl)
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void AgreedReplace(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int storeId = storeInfoBySessionId.StoreId;
			int replaceId = context.Request["replaceId"].ToInt(0);
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(replaceInfo.OrderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				}
				else if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else
				{
					bool flag = context.Request["IsAgreed"].ToBool();
					string text = context.Request["AdminRemark"].ToNullString();
					string text2 = context.Request["AdminShipAddress"].ToNullString();
					string text3 = context.Request["AdminShipTo"].ToNullString();
					string text4 = context.Request["AdminCellPhone"].ToNullString();
					if (string.IsNullOrEmpty(text2))
					{
						text2 = RegionHelper.GetFullRegion(storeInfoBySessionId.RegionId, " ", true, 0) + storeInfoBySessionId.Address;
					}
					if (string.IsNullOrEmpty(text3))
					{
						text3 = storeInfoBySessionId.ContactMan;
					}
					if (string.IsNullOrEmpty(text4))
					{
						text4 = storeInfoBySessionId.Tel;
					}
					if (!flag && string.IsNullOrEmpty(text))
					{
						context.Response.Write(this.GetErrorJosn(121, ((Enum)(object)ApiErrorCode.Must_WriteRemark).ToDescription()));
					}
					else if (flag)
					{
						if (OrderHelper.AgreedReplace(replaceInfo.ReplaceId, replaceInfo.OrderId, replaceInfo.SkuId, text, text2, text3, text4, true))
						{
							string s = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = "SUCCESS"
								}
							});
							context.Response.Write(s);
							context.Response.End();
						}
						else
						{
							context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
						}
					}
					else if (OrderHelper.CheckReplace(replaceInfo.OrderId, text, false, replaceInfo.SkuId, replaceInfo.AdminShipAddress, replaceInfo.AdminShipTo, replaceInfo.AdminCellPhone, true))
					{
						string s2 = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "SUCCESS"
							}
						});
						context.Response.Write(s2);
						context.Response.End();
					}
					else
					{
						context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
					}
				}
			}
		}

		private void GetGoodsAndSendGoods(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int storeId = storeInfoBySessionId.StoreId;
			int replaceId = context.Request["replaceId"].ToInt(0);
			ReplaceInfo replaceInfo = TradeHelper.GetReplaceInfo(replaceId);
			if (replaceInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				return;
			}
			if (replaceInfo.HandleStatus != ReplaceStatus.UserDelivery)
			{
				context.Response.Write(this.GetErrorJosn(131, ((Enum)(object)ApiErrorCode.Must_WriteBankInfo).ToDescription()));
				return;
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(replaceInfo.OrderId);
			if (orderInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				return;
			}
			if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
			{
				context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				return;
			}
			string text = context.Request["ShipOrderNumber"].ToNullString();
			string text2 = "";
			string text3 = context.Request["ExpressCompanyName"].ToNullString();
			if (string.IsNullOrEmpty(text3))
			{
				context.Response.Write(this.GetErrorJosn(115, ((Enum)(object)ApiErrorCode.ExpressCode_Error).ToDescription()));
				return;
			}
			if (text3 != "店员配送")
			{
				ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(text3);
				if (expressCompanyInfo != null)
				{
					text2 = expressCompanyInfo.Kuaidi100Code;
					text3 = expressCompanyInfo.Name;
					if (string.IsNullOrEmpty(text) || text.Length > 20)
					{
						context.Response.Write(this.GetErrorJosn(111, ((Enum)(object)ApiErrorCode.ShipingOrderNumber_Error).ToDescription()));
						return;
					}
					goto IL_0242;
				}
				context.Response.Write(this.GetErrorJosn(115, ((Enum)(object)ApiErrorCode.ExpressCode_Error).ToDescription()));
				return;
			}
			text2 = "";
			text3 = "";
			goto IL_0242;
			IL_0242:
			if (TradeHelper.ReplaceShopSendGoods(replaceId, text2, text3, text, replaceInfo.OrderId, replaceInfo.SkuId))
			{
				if (text2.ToUpper() == "HTKY")
				{
					ExpressHelper.GetDataByKuaidi100(text2, text);
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS"
					}
				});
				replaceInfo.HandleStatus = ReplaceStatus.MerchantsDelivery;
				MemberInfo user = Users.GetUser(orderInfo.UserId);
				Messenger.AfterSaleDeal(user, orderInfo, null, replaceInfo);
				context.Response.Write(s);
				context.Response.End();
			}
			else
			{
				context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
			}
		}

		private void GetReturnDetail(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int storeId = storeInfoBySessionId.StoreId;
			int returnId = context.Request["returnId"].ToInt(0);
			ReturnInfo returns = TradeHelper.GetReturnInfo(returnId);
			if (returns == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				return;
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(returns.OrderId);
			if (orderInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				return;
			}
			string text = returns.AdminCellPhone;
			string text2 = returns.AdminShipAddress;
			string text3 = returns.AdminShipTo;
			int num = 0;
			if (returns.HandleStatus == ReturnStatus.Applied || returns.HandleStatus == ReturnStatus.Refused)
			{
				text = storeInfoBySessionId.Tel;
				text2 = RegionHelper.GetFullRegion(storeInfoBySessionId.RegionId, " ", true, 0) + " " + storeInfoBySessionId.Address;
				text3 = storeInfoBySessionId.ContactMan;
				num = storeInfoBySessionId.RegionId;
			}
			IList<LineItemInfo> source = (from i in orderInfo.LineItems.Values
			where i.SkuId == returns.SkuId
			select i).ToList();
			string skuId = returns.SkuId;
			string cellphone = text;
			string adminRemark = returns.AdminRemark;
			string shipAddress = text2;
			string shipTo = text3;
			int shipRegionId = num;
			DateTime applyForTime = returns.ApplyForTime;
			string userRemark = returns.UserRemark;
			ReturnStatus handleStatus = returns.HandleStatus;
			string statusText = (returns.AfterSaleType == AfterSaleTypes.OnlyRefund) ? EnumDescription.GetEnumDescription((Enum)(object)returns.HandleStatus, 3) : EnumDescription.GetEnumDescription((Enum)(object)returns.HandleStatus, 0);
			DateTime? agreedOrRefusedTime = returns.AgreedOrRefusedTime;
			string @operator = returns.Operator;
			string returnReason = returns.ReturnReason;
			int returnId2 = returns.ReturnId;
			string shipOrderNumber = returns.ShipOrderNumber;
			string orderId = returns.OrderId;
			int quantity = returns.Quantity;
			decimal total = orderInfo.GetTotal(false);
			bool isOnlyRefund = returns.AfterSaleType == AfterSaleTypes.OnlyRefund;
			string refundMoney = returns.RefundAmount.F2ToString("f2");
			RefundTypes refundType = returns.RefundType;
			IList<string> imagesFullPath = this.GetImagesFullPath(returns.UserCredentials, '|');
			if (returns.AfterSaleType == AfterSaleTypes.OnlyRefund && orderInfo.IsStoreCollect)
			{
				goto IL_029c;
			}
			if (returns.AfterSaleType != AfterSaleTypes.OnlyRefund)
			{
				goto IL_029c;
			}
			int showAgreenBtn = 0;
			goto IL_02ad;
			IL_02d2:
			int showRefuseBtn = (returns.HandleStatus == ReturnStatus.Applied) ? 1 : 0;
			goto IL_02e3;
			IL_029c:
			showAgreenBtn = ((returns.HandleStatus == ReturnStatus.Applied) ? 1 : 0);
			goto IL_02ad;
			IL_02e3:
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					SkuId = skuId,
					Cellphone = cellphone,
					AdminRemark = adminRemark,
					ShipAddress = shipAddress,
					ShipTo = shipTo,
					ShipRegionId = shipRegionId,
					ApplyForTime = applyForTime,
					Remark = userRemark,
					Status = (int)handleStatus,
					StatusText = statusText,
					DealTime = agreedOrRefusedTime,
					Operator = @operator,
					Reason = returnReason,
					ReturnId = returnId2,
					ShipOrderNumber = shipOrderNumber,
					OrderId = orderId,
					Quantity = quantity,
					OrderTotal = total,
					IsOnlyRefund = isOnlyRefund,
					RefundMoney = refundMoney,
					RefundType = refundType,
					UserCredentials = imagesFullPath,
					ShowAgreenBtn = ((byte)showAgreenBtn != 0),
					ShowRefuseBtn = ((byte)showRefuseBtn != 0),
					ShowGetGoodsBtn = (returns.HandleStatus == ReturnStatus.Deliverying && !orderInfo.IsStoreCollect),
					ShowFinishBtn = ((returns.HandleStatus == ReturnStatus.Deliverying || returns.HandleStatus == ReturnStatus.GetGoods) && orderInfo.IsStoreCollect),
					BankAccountName = returns.BankAccountName,
					BankAccountNo = returns.BankAccountNo,
					BankName = returns.BankName,
					ProductInfo = from l in source
					select new
					{
						ProductName = l.ItemDescription,
						SKU = l.SKU,
						SKUContent = l.SKUContent,
						Price = l.ItemAdjustedPrice,
						Quantity = l.ShipmentQuantity,
						ThumbnailsUrl = this.GetImageFullPath(l.ThumbnailsUrl)
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
			return;
			IL_02ad:
			if (returns.AfterSaleType == AfterSaleTypes.OnlyRefund && orderInfo.IsStoreCollect)
			{
				goto IL_02d2;
			}
			if (returns.AfterSaleType != AfterSaleTypes.OnlyRefund)
			{
				goto IL_02d2;
			}
			showRefuseBtn = 0;
			goto IL_02e3;
		}

		private void GetRefundDetail(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int refundId = context.Request["RefundId"].ToInt(0);
			RefundInfo refundInfo = TradeHelper.GetRefundInfo(refundId);
			if (refundInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(refundInfo.OrderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				}
				else
				{
					string text = "";
					if (text.StartsWith(refundInfo.OrderId) && orderInfo.LineItems.Count > 0)
					{
						using (Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator = orderInfo.LineItems.Values.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								LineItemInfo current = enumerator.Current;
								text = current.ItemDescription;
							}
						}
					}
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							SkuId = "",
							AdminRemark = refundInfo.AdminRemark,
							ApplyForTime = refundInfo.ApplyForTime,
							Remark = refundInfo.UserRemark,
							Status = (int)refundInfo.HandleStatus,
							StatusText = EnumDescription.GetEnumDescription((Enum)(object)refundInfo.HandleStatus, 0),
							DealTime = refundInfo.AgreedOrRefusedTime,
							Operator = refundInfo.Operator,
							Reason = refundInfo.RefundReason,
							RefundId = refundInfo.RefundId,
							OrderId = refundInfo.OrderId,
							Quantity = 0,
							RefundMoney = refundInfo.RefundAmount.F2ToString("f2"),
							RefundType = refundInfo.RefundType,
							ProductName = text,
							ShowAgreenBtn = (refundInfo.HandleStatus == RefundStatus.Applied && orderInfo.IsStoreCollect),
							ShowRefuseBtn = (refundInfo.HandleStatus == RefundStatus.Applied && orderInfo.IsStoreCollect),
							BankAccountName = refundInfo.BankAccountName,
							BankAccountNo = refundInfo.BankAccountNo,
							BankName = refundInfo.BankName,
							OrderTotal = orderInfo.GetTotal(false).F2ToString("f2"),
							ValidCodes = refundInfo.ValidCodes,
							ProductInfo = from l in orderInfo.LineItems.Values
							select new
							{
								ProductName = l.ItemDescription,
								SKU = l.SKU,
								SKUContent = l.SKUContent,
								Price = l.ItemAdjustedPrice,
								Quantity = l.ShipmentQuantity,
								ThumbnailsUrl = this.GetImageFullPath(l.ThumbnailsUrl)
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void FinishReturn(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int returnId = context.Request["returnId"].ToInt(0);
			string adminRemark = Globals.StripAllTags(context.Request["adminRemark"].ToNullString());
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
			if (returnInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
				if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (!orderInfo.LineItems.ContainsKey(returnInfo.SkuId))
				{
					context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
				}
				else
				{
					LineItemInfo lineItemInfo = orderInfo.LineItems[returnInfo.SkuId];
					if (lineItemInfo.Status != LineItemStatus.DeliveryForReturn && lineItemInfo.Status != LineItemStatus.GetGoodsForReturn)
					{
						context.Response.Write(this.GetErrorJosn(114, ((Enum)(object)ApiErrorCode.OrderStatus_Error).ToDescription()));
					}
					else if (!orderInfo.IsStoreCollect && lineItemInfo.Status == LineItemStatus.GetGoodsForReturn)
					{
						context.Response.Write(this.GetErrorJosn(122, ((Enum)(object)ApiErrorCode.Order_NoStoreCollect).ToDescription()));
					}
					else
					{
						string storeName = storeInfoBySessionId.StoreName;
						MemberInfo user = Users.GetUser(orderInfo.UserId);
						string str = "";
						if ((RefundHelper.IsBackReturn(orderInfo.Gateway) && returnInfo.RefundType == RefundTypes.BackReturn) || returnInfo.RefundType == RefundTypes.InBankCard)
						{
							if (TradeHelper.FinishGetGoodsForReturn(returnInfo.ReturnId, adminRemark, orderInfo.OrderId, returnInfo.SkuId, returnInfo.RefundAmount))
							{
								MemberInfo user2 = Users.GetUser(orderInfo.UserId);
								returnInfo.HandleStatus = ReturnStatus.GetGoods;
								Messenger.AfterSaleDeal(user2, orderInfo, returnInfo, null);
								string s = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "SUCCESS"
									}
								});
								context.Response.Write(s);
								context.Response.End();
							}
							else
							{
								context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription() + str));
							}
						}
						else if (OrderHelper.CheckReturn(returnInfo, orderInfo, storeInfoBySessionId.StoreName, returnInfo.RefundAmount, adminRemark, true, true))
						{
							string s2 = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = "SUCCESS"
								}
							});
							context.Response.Write(s2);
							context.Response.End();
						}
					}
				}
			}
		}

		private void AgreedReturn(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int returnId = context.Request["returnId"].ToInt(0);
			bool flag = context.Request["IsAgreed"].ToBool();
			string text = Globals.StripAllTags(context.Request["adminRemark"].ToNullString());
			decimal num = context.Request["RefundMoney"].ToDecimal(0);
			string text2 = context.Request["adminShipAddress"].ToNullString();
			string text3 = context.Request["adminShipTo"].ToNullString();
			string text4 = context.Request["adminCellPhone"].ToNullString();
			ReturnInfo returnInfo = TradeHelper.GetReturnInfo(returnId);
			if (returnInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
			}
			else
			{
				if (string.IsNullOrEmpty(text2))
				{
					text2 = RegionHelper.GetFullRegion(storeInfoBySessionId.RegionId, " ", true, 0) + storeInfoBySessionId.Address;
				}
				if (string.IsNullOrEmpty(text3))
				{
					text3 = storeInfoBySessionId.ContactMan;
				}
				if (string.IsNullOrEmpty(text4))
				{
					text4 = storeInfoBySessionId.Tel;
				}
				if (!flag && string.IsNullOrEmpty(text))
				{
					context.Response.Write(this.GetErrorJosn(121, ((Enum)(object)ApiErrorCode.Must_WriteRemark).ToDescription()));
				}
				else
				{
					OrderInfo orderInfo = TradeHelper.GetOrderInfo(returnInfo.OrderId);
					if (orderInfo == null)
					{
						context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
					}
					else if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
					{
						context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
					}
					else if (!orderInfo.LineItems.ContainsKey(returnInfo.SkuId))
					{
						context.Response.Write(this.GetErrorJosn(119, ((Enum)(object)ApiErrorCode.OrderOrProduct_NoExistReturn).ToDescription()));
					}
					else
					{
						LineItemInfo lineItemInfo = orderInfo.LineItems[returnInfo.SkuId];
						if (lineItemInfo.Status != LineItemStatus.ReturnApplied)
						{
							context.Response.Write(this.GetErrorJosn(114, ((Enum)(object)ApiErrorCode.OrderStatus_Error).ToDescription()));
						}
						else
						{
							string storeName = storeInfoBySessionId.StoreName;
							GroupBuyInfo groupbuy = null;
							if (orderInfo.GroupBuyId > 0)
							{
								groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
							}
							if (num < decimal.Zero || num > orderInfo.GetCanRefundAmount(returnInfo.SkuId, groupbuy, 0))
							{
								context.Response.Write(this.GetErrorJosn(117, ((Enum)(object)ApiErrorCode.RefundMoney_Error).ToDescription()));
							}
							else if (!flag)
							{
								if (OrderHelper.CheckReturn(returnInfo, orderInfo, storeName, num, text, false, true))
								{
									string s = JsonConvert.SerializeObject(new
									{
										Result = new
										{
											Status = "SUCCESS"
										}
									});
									context.Response.Write(s);
									context.Response.End();
								}
								else
								{
									context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
								}
							}
							else
							{
								bool isRefund = returnInfo.AfterSaleType == AfterSaleTypes.OnlyRefund;
								bool isRefundToBalance = returnInfo.RefundType == RefundTypes.InBalance;
								if (OrderHelper.AgreedReturns(returnInfo.ReturnId, num, text, orderInfo, returnInfo.SkuId, text2, text3, text4, isRefund, isRefundToBalance))
								{
									if (orderInfo.UserId > 0)
									{
										MemberInfo user = Users.GetUser(orderInfo.UserId);
										Messenger.OrderRefund(user, orderInfo, returnInfo.SkuId);
									}
									string s2 = JsonConvert.SerializeObject(new
									{
										Result = new
										{
											Status = "SUCCESS"
										}
									});
									context.Response.Write(s2);
									context.Response.End();
								}
								else
								{
									context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
								}
							}
						}
					}
				}
			}
		}

		private void AgreedRefund(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int refundId = context.Request["refundId"].ToInt(0);
			bool flag = context.Request["IsAgreed"].ToBool();
			string text = Globals.StripAllTags(context.Request["adminRemark"].ToNullString());
			decimal num = context.Request["RefundMoney"].ToDecimal(0);
			RefundInfo refundInfo = TradeHelper.GetRefundInfo(refundId);
			if (refundInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(118, ((Enum)(object)ApiErrorCode.Order_NoExistRefund).ToDescription()));
			}
			else if (!flag && string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(121, ((Enum)(object)ApiErrorCode.Must_WriteRemark).ToDescription()));
			}
			else if (string.IsNullOrEmpty(refundInfo.OrderId))
			{
				context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(refundInfo.OrderId);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else
				{
					string storeName = storeInfoBySessionId.StoreName;
					if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
					{
						context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
					}
					else if (!orderInfo.IsStoreCollect)
					{
						context.Response.Write(this.GetErrorJosn(122, ((Enum)(object)ApiErrorCode.Order_NoStoreCollect).ToDescription()));
					}
					else
					{
						GroupBuyInfo groupbuy = null;
						if (orderInfo.GroupBuyId > 0)
						{
							groupbuy = ProductBrowser.GetGroupBuy(orderInfo.GroupBuyId);
						}
						if (flag && (num < decimal.Zero || num > orderInfo.GetCanRefundAmount("", groupbuy, 0)))
						{
							context.Response.Write(this.GetErrorJosn(117, ((Enum)(object)ApiErrorCode.RefundMoney_Error).ToDescription()));
						}
						else if (!flag)
						{
							if (OrderHelper.CheckRefund(orderInfo, refundInfo, num, storeName, text, false, true))
							{
								if (HiContext.Current.SiteSettings.OpenMobbile == 1)
								{
									VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
								}
								string text2 = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "SUCCESS"
									}
								});
							}
							else
							{
								context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
							}
						}
						else if (OrderHelper.CanFinishRefund(orderInfo, refundInfo, num, true))
						{
							RefundTypes refundType = refundInfo.RefundType;
							string userRemark = refundInfo.UserRemark;
							MemberInfo user = Users.GetUser(orderInfo.UserId);
							string text3 = "";
							if (RefundHelper.IsBackReturn(orderInfo.Gateway) && refundInfo.RefundType == RefundTypes.BackReturn)
							{
								text3 = RefundHelper.SendRefundRequest(orderInfo, num, refundInfo.RefundOrderId, true);
								if (text3 == "")
								{
									if (OrderHelper.CheckRefund(orderInfo, refundInfo, num, storeName, text, true, true))
									{
										if (HiContext.Current.SiteSettings.OpenMobbile == 1)
										{
											VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
										}
										Messenger.OrderRefund(user, orderInfo, "");
										string s = JsonConvert.SerializeObject(new
										{
											Result = new
											{
												Status = "SUCCESS"
											}
										});
										context.Response.Write(s);
										context.Response.End();
									}
								}
								else
								{
									context.Response.Write(this.GetErrorJosn(120, ((Enum)(object)ApiErrorCode.Refund_BackReturnError).ToDescription()));
								}
							}
							else if (OrderHelper.CheckRefund(orderInfo, refundInfo, num, storeName, text, true, true))
							{
								if (HiContext.Current.SiteSettings.OpenMobbile == 1)
								{
									VShopHelper.AppPushRecordForOrder(orderInfo.OrderId, "", EnumPushOrderAction.OrderRefund);
								}
								Messenger.OrderRefund(user, orderInfo, "");
								string s2 = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "SUCCESS"
									}
								});
								context.Response.Write(s2);
								context.Response.End();
							}
						}
						else
						{
							context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription()));
						}
					}
				}
			}
		}

		private void GetReturnsList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 35);
		}

		private void GetRefundedList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 9);
		}

		private void GetReturnRefusedList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 19);
		}

		private void GetRefundRefusedList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 18);
		}

		private void GetReturnedList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 10);
		}

		private void GetAfterSaleingList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 32);
		}

		private void GetAfterSaleCompletedList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 33);
		}

		private void GetAllAfterSaleList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			int num3 = context.Request["status"].ToInt(0);
			if (num <= 0 || num2 <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				AfterSalesQuery afterSalesQuery = new AfterSalesQuery();
				afterSalesQuery.SupplierId = 0;
				afterSalesQuery.PageIndex = num;
				afterSalesQuery.PageSize = num2;
				if (num3 > 0)
				{
					if (num3 == 4)
					{
						afterSalesQuery.Status = new List<int>
						{
							0,
							4
						};
						afterSalesQuery.AfterSaleType = null;
						AfterSalesQuery afterSalesQuery2 = afterSalesQuery;
						List<int> list = new List<int>();
						AfterSaleTypes afterSaleTypes = AfterSaleTypes.ReturnAndRefund;
						list.Add(afterSaleTypes.GetHashCode());
						afterSaleTypes = AfterSaleTypes.Replace;
						list.Add(afterSaleTypes.GetHashCode());
						afterSalesQuery2.MoreAfterSaleType = list;
					}
					else
					{
						afterSalesQuery.Status = new List<int>
						{
							num3
						};
					}
				}
				afterSalesQuery.StoreId = storeIdBySessionId;
				PageModel<AfterSaleRecordModel> afterSalesList = OrderHelper.GetAfterSalesList(afterSalesQuery);
				List<OrderInfo> list2 = new List<OrderInfo>();
				IList<AfterSaleRecordModel> list3 = afterSalesList.Models.ToList();
				for (int j = 0; j < list3.Count; j++)
				{
					AfterSaleRecordModel afterSaleRecordModel = list3[j];
					list3[j].ProductItems = TradeHelper.GetOrderItems(afterSaleRecordModel.OrderId, afterSaleRecordModel.SkuId);
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = afterSalesList.Total,
						List = from c in list3
						select new
						{
							OrderId = c.OrderId,
							Status = c.HandleStatus,
							StatusText = c.StatusText,
							AdminRemark = c.AdminRemark,
							AfterSaleId = c.AfterSaleId,
							AfterSaleType = c.AfterSaleType,
							ApplyForTime = c.ApplyForTime,
							ExpressCompanyAbb = c.ExpressCompanyAbb,
							ExpressCompanyName = c.ExpressCompanyName,
							RefundAmount = c.RefundAmount,
							RefundType = (int)c.RefundType,
							RefundTypeText = EnumDescription.GetEnumDescription((Enum)(object)c.RefundType, 0),
							ShipOrderNumber = c.ShipOrderNumber,
							SkuId = c.SkuId,
							OrderTotal = c.ProductItems.Sum((LineItemInfo i) => (decimal)i.Quantity * i.ItemAdjustedPrice),
							QuantityTotal = c.ProductItems.Sum((LineItemInfo i) => i.Quantity),
							UserExpressCompanyAbb = c.UserExpressCompanyAbb,
							UserExpressCompanyName = c.UserExpressCompanyName,
							UserRemark = c.UserRemark,
							UserShipOrderNumber = c.UserShipOrderNumber,
							IsRefund = (c.AfterSaleType == AfterSaleTypes.OrderRefund),
							IsReturn = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund || c.AfterSaleType == AfterSaleTypes.OnlyRefund),
							IsReplace = (c.AfterSaleType == AfterSaleTypes.Replace),
							IsWaitToDeal = (c.HandleStatus == 0 && ((c.IsStoreCollect && c.AfterSaleType == AfterSaleTypes.OrderRefund) || (c.AfterSaleType == AfterSaleTypes.OnlyRefund && c.IsStoreCollect) || c.AfterSaleType == AfterSaleTypes.Replace || c.AfterSaleType == AfterSaleTypes.ReturnAndRefund)),
							IsWaitFinishReturn = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund && c.HandleStatus == 4),
							IsShowReturnLogistics = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund && c.HandleStatus == 4),
							IsWaitGetGoodsOfReplace = (c.AfterSaleType == AfterSaleTypes.Replace && c.HandleStatus == 4),
							IsWaitConfirmReplace = (c.AfterSaleType == AfterSaleTypes.Replace && c.HandleStatus == 0),
							LogisticsUrl = this.GetLogisticsUrl(c.AfterSaleType, c.HandleStatus, c.OrderId, c.AfterSaleId, c.OrderStatus),
							LineItems = from d in c.ProductItems
							select new
							{
								Status = d.Status,
								StatusText = d.StatusText,
								SkuId = d.SkuId,
								Name = d.ItemDescription,
								Price = d.ItemAdjustedPrice.F2ToString("f2"),
								Amount = (d.ItemAdjustedPrice * (decimal)d.Quantity).F2ToString("f2"),
								Quantity = d.ShipmentQuantity,
								Image = this.GetImageFullPath(d.ThumbnailsUrl),
								SkuText = d.SKUContent,
								ProductId = d.ProductId
							}
						}
					}
				});
				context.Response.Write(s);
			}
		}

		private void GetAfterSaleRefusedList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 34);
		}

		private string GetLogisticsUrl(AfterSaleTypes afterSaleType, int status, string orderId, int afterSaleId, OrderStatus oStatus)
		{
			string text = "";
			if (afterSaleType == AfterSaleTypes.ReturnAndRefund || afterSaleType == AfterSaleTypes.Replace)
			{
				switch (afterSaleType)
				{
				case AfterSaleTypes.ReturnAndRefund:
					if (status == 4 || status == 5 || status == 1)
					{
						text = "/AppDepot/OrderLogistics?OrderId=" + orderId + "&returnsId=" + afterSaleId;
					}
					break;
				case AfterSaleTypes.Replace:
					if (status == 4 || status == 6 || status == 1)
					{
						text = "/AppDepot/OrderLogistics?OrderId=" + orderId + "&ReplaceId=" + afterSaleId;
					}
					break;
				}
			}
			if (string.IsNullOrEmpty(text) && (oStatus == OrderStatus.SellerAlreadySent || oStatus == OrderStatus.Finished))
			{
				text = "/AppDepot/OrderLogistics?OrderId=" + orderId;
			}
			return text;
		}

		private void GetAfterSaleOrders(HttpContext context, int storeId, int pageIndex, int pageSize, int status)
		{
			if (status != 30 && status != 31 && status != 32 && status != 33 && status != 34 && status != 6 && status != 9 && status != 18 && status != 10 && status != 19 && status != 35 && status != 36 && status != 37 && status != 38 && status != 39)
			{
				status = 0;
			}
			if (pageIndex <= 0 || pageSize <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				AfterSalesQuery afterSalesQuery = new AfterSalesQuery();
				afterSalesQuery.SupplierId = 0;
				afterSalesQuery.PageIndex = pageIndex;
				afterSalesQuery.PageSize = pageSize;
				switch (status)
				{
				case 6:
					afterSalesQuery.Status = new List<int>
					{
						0
					};
					afterSalesQuery.AfterSaleType = 0;
					break;
				case 32:
					afterSalesQuery.Status = new List<int>
					{
						0,
						0,
						4,
						5,
						3,
						0,
						3,
						6,
						4
					};
					break;
				case 33:
					afterSalesQuery.Status = new List<int>
					{
						1,
						1,
						1
					};
					break;
				case 34:
					afterSalesQuery.Status = new List<int>
					{
						2,
						2,
						2
					};
					break;
				case 35:
					afterSalesQuery.Status = new List<int>
					{
						0,
						4,
						5,
						3
					};
					break;
				}
				afterSalesQuery.StoreId = storeId;
				PageModel<AfterSaleRecordModel> afterSalesList = OrderHelper.GetAfterSalesList(afterSalesQuery);
				List<OrderInfo> list = new List<OrderInfo>();
				IList<AfterSaleRecordModel> list2 = afterSalesList.Models.ToList();
				for (int j = 0; j < list2.Count; j++)
				{
					AfterSaleRecordModel afterSaleRecordModel = list2[j];
					list2[j].ProductItems = TradeHelper.GetOrderItems(afterSaleRecordModel.OrderId, afterSaleRecordModel.SkuId);
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						RecordCount = afterSalesList.Total,
						List = from c in list2
						select new
						{
							OrderId = c.OrderId,
							Status = c.HandleStatus,
							StatusText = c.StatusText,
							AdminRemark = c.AdminRemark,
							AfterSaleId = c.AfterSaleId,
							AfterSaleType = c.AfterSaleType,
							ApplyForTime = c.ApplyForTime,
							ExpressCompanyAbb = c.ExpressCompanyAbb,
							ExpressCompanyName = c.ExpressCompanyName,
							RefundAmount = c.RefundAmount,
							RefundType = (int)c.RefundType,
							RefundTypeText = EnumDescription.GetEnumDescription((Enum)(object)c.RefundType, 0),
							ShipOrderNumber = c.ShipOrderNumber,
							SkuId = c.SkuId,
							OrderTotal = c.ProductItems.Sum((LineItemInfo i) => (decimal)i.Quantity * i.ItemAdjustedPrice),
							QuantityTotal = c.ProductItems.Sum((LineItemInfo i) => i.Quantity),
							UserExpressCompanyAbb = c.UserExpressCompanyAbb,
							UserExpressCompanyName = c.UserExpressCompanyName,
							UserRemark = c.UserRemark,
							UserShipOrderNumber = c.UserShipOrderNumber,
							IsRefund = (c.AfterSaleType == AfterSaleTypes.OrderRefund),
							IsReturn = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund || c.AfterSaleType == AfterSaleTypes.OnlyRefund),
							IsReplace = (c.AfterSaleType == AfterSaleTypes.Replace),
							IsWaitToDeal = (c.HandleStatus == 0 && ((c.IsStoreCollect && c.AfterSaleType == AfterSaleTypes.OrderRefund) || (c.AfterSaleType == AfterSaleTypes.OnlyRefund && c.IsStoreCollect) || c.AfterSaleType == AfterSaleTypes.Replace || c.AfterSaleType == AfterSaleTypes.ReturnAndRefund)),
							IsWaitFinishReturn = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund && c.HandleStatus == 4),
							IsShowReturnLogistics = (c.AfterSaleType == AfterSaleTypes.ReturnAndRefund && c.HandleStatus == 4),
							IsWaitGetGoodsOfReplace = (c.AfterSaleType == AfterSaleTypes.Replace && c.HandleStatus == 4),
							IsWaitConfirmReplace = (c.AfterSaleType == AfterSaleTypes.Replace && c.HandleStatus == 0),
							LogisticsUrl = this.GetLogisticsUrl(c.AfterSaleType, c.HandleStatus, c.OrderId, c.AfterSaleId, c.OrderStatus),
							LineItems = from d in c.ProductItems
							select new
							{
								Status = d.Status,
								StatusText = d.StatusText,
								SkuId = d.SkuId,
								Name = d.ItemDescription,
								Price = d.ItemAdjustedPrice.F2ToString("f2"),
								Amount = (d.ItemAdjustedPrice * (decimal)d.Quantity).F2ToString("f2"),
								Quantity = d.ShipmentQuantity,
								Image = this.GetImageFullPath(d.ThumbnailsUrl),
								SkuText = d.SKUContent,
								ProductId = d.ProductId
							}
						}
					}
				});
				context.Response.Write(s);
			}
		}

		private void GetRefundsList(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 6);
		}

		private int GetStoreIdBySessionId(HttpContext context)
		{
			string text = context.Request["SessionId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(511, ((Enum)(object)ApiErrorCode.SessionId_Empty).ToDescription()));
				context.Response.End();
			}
			ManagerInfo managerBySessionId = ManagerHelper.GetManagerBySessionId(text);
			int num = 0;
			if (managerBySessionId != null)
			{
				num = managerBySessionId.StoreId;
			}
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			StoresInfo storeInfoBySessionId = ManagerHelper.GetStoreInfoBySessionId(text);
			if (storeInfoBySessionId == null)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			if (storeInfoBySessionId.State == 0)
			{
				context.Response.Write(this.GetErrorJosn(533, ((Enum)(object)ApiErrorCode.StoreIsClosed).ToDescription()));
				context.Response.End();
			}
			return num;
		}

		private int GetStoreIdBySessionId(HttpContext context, out int shoppingGuiderId)
		{
			shoppingGuiderId = 0;
			string text = context.Request["SessionId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(511, ((Enum)(object)ApiErrorCode.SessionId_Empty).ToDescription()));
				context.Response.End();
			}
			ManagerInfo managerBySessionId = ManagerHelper.GetManagerBySessionId(text);
			int num = 0;
			if (managerBySessionId != null)
			{
				num = managerBySessionId.StoreId;
				if (managerBySessionId.RoleId == -3)
				{
					shoppingGuiderId = managerBySessionId.ManagerId;
				}
			}
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			StoresInfo storeInfoBySessionId = ManagerHelper.GetStoreInfoBySessionId(text);
			if (storeInfoBySessionId == null)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			if (storeInfoBySessionId.State == 0)
			{
				context.Response.Write(this.GetErrorJosn(533, ((Enum)(object)ApiErrorCode.StoreIsClosed).ToDescription()));
				context.Response.End();
			}
			return num;
		}

		private StoresInfo GetStoreInfoBySessionId(HttpContext context)
		{
			string text = context.Request["SessionId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(511, ((Enum)(object)ApiErrorCode.SessionId_Empty).ToDescription()));
				context.Response.End();
			}
			StoresInfo storeInfoBySessionId = ManagerHelper.GetStoreInfoBySessionId(text);
			if (storeInfoBySessionId == null)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			if (storeInfoBySessionId.State == 0)
			{
				context.Response.Write(this.GetErrorJosn(533, ((Enum)(object)ApiErrorCode.StoreIsClosed).ToDescription()));
				context.Response.End();
			}
			return storeInfoBySessionId;
		}

		private StoresInfo GetStoreInfoBySessionId(HttpContext context, out int shoppingGuiderId)
		{
			shoppingGuiderId = 0;
			string text = context.Request["SessionId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(511, ((Enum)(object)ApiErrorCode.SessionId_Empty).ToDescription()));
				context.Response.End();
			}
			ManagerInfo managerBySessionId = ManagerHelper.GetManagerBySessionId(text);
			int num = 0;
			if (managerBySessionId != null)
			{
				num = managerBySessionId.StoreId;
				if (managerBySessionId.RoleId == -3)
				{
					shoppingGuiderId = managerBySessionId.ManagerId;
				}
			}
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			StoresInfo storeInfoBySessionId = ManagerHelper.GetStoreInfoBySessionId(text);
			if (storeInfoBySessionId == null)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			if (storeInfoBySessionId.State == 0)
			{
				context.Response.Write(this.GetErrorJosn(533, ((Enum)(object)ApiErrorCode.StoreIsClosed).ToDescription()));
				context.Response.End();
			}
			return storeInfoBySessionId;
		}

		private ManagerInfo GetManagerInfoBySessionId(HttpContext context)
		{
			string text = context.Request["SessionId"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(511, ((Enum)(object)ApiErrorCode.SessionId_Empty).ToDescription()));
				context.Response.End();
			}
			ManagerInfo managerBySessionId = ManagerHelper.GetManagerBySessionId(text);
			if (managerBySessionId == null)
			{
				context.Response.Write(this.GetErrorJosn(520, ((Enum)(object)ApiErrorCode.SessionId_Error).ToDescription()));
				context.Response.End();
			}
			return managerBySessionId;
		}

		private void ExpressType(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(0, "快递配送");
			dictionary.Add(1, "店员配送");
			if (masterSettings.OpenDadaLogistics)
			{
				dictionary.Add(2, "同城物流配送");
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = from c in dictionary
				select new
				{
					value = c.Key,
					text = c.Value
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void QueryDeliverFee(HttpContext context)
		{
			string text = context.Request["OrderId"].ToNullString();
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else if ((orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid && (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || !(orderInfo.Gateway == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashOnDelivery, 1)))) || orderInfo.RealShippingModeId == -2)
				{
					context.Response.Write(this.GetErrorJosn(114, ((Enum)(object)ApiErrorCode.OrderStatus_Error).ToDescription()));
				}
				else
				{
					DataTable dataTable = DepotHelper.SynchroDadaStoreList(storeInfoBySessionId.StoreId);
					if (!orderInfo.ShippingRegion.Contains(dataTable.Rows[0]["CityName"].ToString()))
					{
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "FAIL",
								Msg = "配送范围超区，无法配送"
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
					else
					{
						string text2 = "";
						try
						{
							string value = DadaHelper.cityCodeList(masterSettings.DadaSourceID);
							JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
							JArray jArray = (JArray)jObject["result"];
							foreach (JToken item in (IEnumerable<JToken>)jArray)
							{
								if (orderInfo.ShippingRegion.Contains(item["cityName"].ToString()))
								{
									text2 = item["cityCode"].ToString();
									break;
								}
							}
						}
						catch
						{
						}
						if (text2 == "")
						{
							string s2 = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = "FAIL",
									Msg = "配送范围超区，无法配送"
								}
							});
							context.Response.Write(s2);
							context.Response.End();
						}
						else
						{
							string shop_no = orderInfo.StoreId.ToNullString();
							string orderId = orderInfo.OrderId;
							string city_code = text2;
							double cargo_price = orderInfo.GetTotal(false).F2ToString("f2").ToDouble(0);
							int is_prepay = 0;
							long expected_fetch_time = Globals.DateTimeToUnixTimestamp(DateTime.Now.AddMinutes(15.0));
							string shipTo = orderInfo.ShipTo;
							string address = orderInfo.Address;
							string latLng = orderInfo.LatLng;
							if (string.IsNullOrWhiteSpace(latLng))
							{
								ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(orderInfo.ShippingId);
								latLng = shippingAddress.LatLng;
							}
							double receiver_lat = latLng.Split(',')[0].ToDouble(0);
							double receiver_lng = latLng.Split(',')[1].ToDouble(0);
							string callback = Globals.FullPath("/pay/dadaOrderNotify");
							string cellPhone = orderInfo.CellPhone;
							string telPhone = orderInfo.TelPhone;
							bool isQueryDeliverFee = true;
							string value2 = DadaHelper.addOrder(masterSettings.DadaSourceID, shop_no, orderId, city_code, cargo_price, is_prepay, expected_fetch_time, shipTo, address, receiver_lat, receiver_lng, callback, cellPhone, telPhone, -1.0, -1.0, -1.0, -1.0, -1L, "", -1, -1.0, -1, -1L, "", "", "", false, isQueryDeliverFee);
							JObject jObject2 = JsonConvert.DeserializeObject(value2) as JObject;
							string a = jObject2["status"].ToString();
							if (a == "success")
							{
								JObject jObject3 = JsonConvert.DeserializeObject(jObject2["result"].ToString()) as JObject;
								string s3 = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "SUCCESS",
										distance = jObject3["distance"].ToNullString(),
										fee = "预计运费：￥" + jObject3["fee"].ToNullString(),
										deliveryNo = jObject3["deliveryNo"].ToNullString()
									}
								});
								context.Response.Write(s3);
								context.Response.End();
							}
							else
							{
								string s4 = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "FAIL",
										Msg = jObject2["msg"].ToNullString()
									}
								});
								context.Response.Write(s4);
								context.Response.End();
							}
						}
					}
				}
			}
		}

		private void SendGoods(HttpContext context)
		{
			string text = context.Request["OrderId"].ToNullString();
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			int num = context.Request["ExpressType"].ToInt(0);
			string text2 = context.Request["ExpressCode"].ToNullString();
			string text3 = context.Request["ExpressName"].ToNullString().Trim();
			string text4 = context.Request["ShippingNumber"].ToNullString();
			string text5 = context.Request["DeliveryNo"].ToNullString();
			if (num == 0 && string.IsNullOrEmpty(text3))
			{
				context.Response.Write(this.GetErrorJosn(115, ((Enum)(object)ApiErrorCode.ExpressCode_Error).ToDescription()));
			}
			else if (num == 0 && string.IsNullOrEmpty(text4))
			{
				context.Response.Write(this.GetErrorJosn(111, ((Enum)(object)ApiErrorCode.ShipingOrderNumber_Error).ToDescription()));
			}
			else
			{
				ExpressCompanyInfo expressCompanyInfo = null;
				if (num == 0)
				{
					expressCompanyInfo = ExpressHelper.FindNode(text3);
				}
				if (num == 0 && expressCompanyInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(115, ((Enum)(object)ApiErrorCode.ExpressCode_Error).ToDescription()));
				}
				else if (string.IsNullOrEmpty(text))
				{
					context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
				}
				else
				{
					OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
					if (orderInfo == null)
					{
						context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
					}
					else if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
					{
						context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
					}
					else if ((orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid && (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || !(orderInfo.Gateway == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashOnDelivery, 1)))) || orderInfo.RealShippingModeId == -2)
					{
						context.Response.Write(this.GetErrorJosn(114, ((Enum)(object)ApiErrorCode.OrderStatus_Error).ToDescription()));
					}
					else
					{
						switch (num)
						{
						case 0:
							orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
							orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
							orderInfo.ShipOrderNumber = text4;
							break;
						case 1:
							orderInfo.ExpressCompanyName = "店员配送";
							orderInfo.ExpressCompanyAbb = "";
							orderInfo.ShipOrderNumber = "";
							break;
						default:
							orderInfo.ExpressCompanyName = "同城物流配送";
							orderInfo.ExpressCompanyAbb = "";
							orderInfo.ShipOrderNumber = "";
							orderInfo.DadaStatus = DadaStatus.WaitOrder;
							break;
						}
						try
						{
							OrderStatus orderStatus = orderInfo.OrderStatus;
							if (OrderHelper.StoreSendGoods(orderInfo, true))
							{
								if (orderInfo.Gateway.ToNullString().ToLower() == "hishop.plugins.payment.podrequest")
								{
									ProductStatisticsHelper.UpdateOrderSaleStatistics(orderInfo);
									TransactionAnalysisHelper.AnalysisOrderTranData(orderInfo);
								}
								ManagerInfo managerBySessionId = ManagerHelper.GetManagerBySessionId(context.Request["SessionId"].ToNullString());
								string userName = string.Empty;
								if (managerBySessionId != null)
								{
									userName = managerBySessionId.UserName;
								}
								if (orderStatus == OrderStatus.WaitBuyerPay)
								{
									OrderHelper.AppChangeStoreStockAndWriteLog(orderInfo, userName);
								}
								if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
								{
									PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.Gateway);
									if (paymentMode != null)
									{
										string hIGW = paymentMode.Gateway.Replace(".", "_");
										PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(false), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(""), Globals.FullPath(RouteConfig.GetRouteUrl(this.myContext, "PaymentReturn_url", new
										{
											HIGW = hIGW
										})), Globals.FullPath(RouteConfig.GetRouteUrl(this.myContext, "PaymentNotify_url", new
										{
											HIGW = hIGW
										})), "");
										paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
									}
								}
								if (!string.IsNullOrEmpty(orderInfo.OuterOrderId))
								{
									if (orderInfo.OuterOrderId.StartsWith("tb_"))
									{
										string text6 = orderInfo.OuterOrderId.Replace("tb_", "");
										try
										{
											string requestUriString = $"http://order2.kuaidiangtong.com/UpdateShipping.ashx?tid={text6}&companycode={expressCompanyInfo.TaobaoCode}&outsid={orderInfo.ShipOrderNumber}&Host={HiContext.Current.SiteUrl}";
											WebRequest webRequest = WebRequest.Create(requestUriString);
											webRequest.GetResponse();
										}
										catch
										{
										}
									}
									else if (orderInfo.OuterOrderId.StartsWith("jd_"))
									{
										string text6 = orderInfo.OuterOrderId.Replace("jd_", "");
										try
										{
											SiteSettings masterSettings = SettingsManager.GetMasterSettings();
											JDHelper.JDOrderOutStorage(masterSettings.JDAppKey, masterSettings.JDAppSecret, masterSettings.JDAccessToken, expressCompanyInfo.JDCode, orderInfo.ShipOrderNumber, text6);
										}
										catch (Exception)
										{
										}
									}
								}
								if (orderInfo.ExpressCompanyName == "同城物流配送" && !string.IsNullOrEmpty(text5))
								{
									SiteSettings masterSettings2 = SettingsManager.GetMasterSettings();
									DadaHelper.addAfterQuery(masterSettings2.DadaSourceID, text5);
								}
								int userId = orderInfo.UserId;
								MemberInfo user = Users.GetUser(userId);
								Messenger.OrderShipping(orderInfo, user);
								orderInfo.OnDeliver();
								string s = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										Status = "SUCCESS"
									}
								});
								context.Response.Write(s);
								context.Response.End();
							}
							else
							{
								context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription() + ":发货失败"));
							}
						}
						catch (Exception ex2)
						{
							if (!(ex2 is ThreadAbortException))
							{
								context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription() + ":" + ex2.Message));
							}
						}
					}
				}
			}
		}

		private void CancelSendGoods(HttpContext context)
		{
			StoresInfo storeInfoBySessionId = this.GetStoreInfoBySessionId(context);
			string text = context.Request["OrderId"].ToNullString();
			int num = context.Request["ReasonId"].ToInt(0);
			string text2 = context.Request["CancelReason"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (orderInfo.StoreId != storeInfoBySessionId.StoreId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else if (num == 0)
				{
					context.Response.Write(this.GetErrorJosn(138, ((Enum)(object)ApiErrorCode.CancelSendGoodsReasonEmpty).ToDescription()));
				}
				else
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					DadaHelper.orderFormalCancel(masterSettings.DadaSourceID, text, num, text2);
					orderInfo.OrderStatus = OrderStatus.BuyerAlreadyPaid;
					orderInfo.CloseReason = text2;
					orderInfo.DadaStatus = DadaStatus.Cancel;
					TradeHelper.UpdateOrderInfo(orderInfo);
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS"
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void SendGoodsOrderDetail(HttpContext context)
		{
			string text = context.Request["OrderId"].ToNullString();
			int storeId = this.GetStoreIdBySessionId(context);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(112, ((Enum)(object)ApiErrorCode.NotImageFile).ToDescription()));
			}
			else
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(text);
				if (orderInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
				}
				else if (orderInfo.StoreId != storeId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else if ((orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid && (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay || !(orderInfo.Gateway == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.CashOnDelivery, 1)))) || orderInfo.RealShippingModeId == -2)
				{
					context.Response.Write(this.GetErrorJosn(114, ((Enum)(object)ApiErrorCode.OrderStatus_Error).ToDescription()));
				}
				else
				{
					IList<StoreProductModel> list = new List<StoreProductModel>();
					if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && OrderHelper.GetNoStockItems(orderInfo) != null)
					{
						foreach (LineItemInfo value in orderInfo.LineItems.Values)
						{
							StoreProductModel storeProductInfo = StoresHelper.GetStoreProductInfo(storeId, value.ProductId);
							list.Add(storeProductInfo);
						}
						string s = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								SendGoodsStatus = "STOCK NOT ENOUGH",
								ProductList = from d in list
								select new
								{
									ProductName = d.ProductName,
									ProductId = d.ProductId,
									Stock = StoresHelper.GetStoreProductStock(storeId, d.ProductId),
									ProductCode = d.ProductCode,
									Image = this.GetImageFullPath(d.ThumbnailUrl410),
									SkuList = from sku in d.Skus.Values
									select new
									{
										SkuId = sku.SkuId,
										StoreStock = sku.Stock,
										SkuText = ProductHelper.GetSkusBySkuId(sku.SkuId, d.ProductId),
										ProductCode = sku.SKU,
										WarningStock = sku.WarningStock
									}
								}
							}
						});
						context.Response.Write(s);
						context.Response.End();
					}
					string s2 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							SendGoodsStatus = "",
							OrderId = orderInfo.OrderId,
							StoreId = orderInfo.StoreId,
							Status = orderInfo.OrderStatus,
							StatusText = EnumDescription.GetEnumDescription((Enum)(object)orderInfo.OrderStatus, 0),
							ShipToDate = orderInfo.ShipToDate,
							Remark = orderInfo.Remark,
							ShipTo = orderInfo.ShipTo,
							Cellphone = (string.IsNullOrEmpty(orderInfo.CellPhone) ? orderInfo.TelPhone : orderInfo.CellPhone),
							Address = orderInfo.ShippingRegion + " " + orderInfo.Address.Replace(" ", ""),
							RegionId = orderInfo.RegionId,
							ShippingModeId = orderInfo.RealModeName,
							ShippingModeName = orderInfo.RealModeName,
							InvoiceTitle = orderInfo.InvoiceTitle,
							Tax = orderInfo.Tax.F2ToString("f2"),
							Gifts = from g in orderInfo.Gifts
							select new
							{
								GiftId = g.GiftId,
								GiftName = g.GiftName,
								PromoteType = g.PromoteType,
								Quantity = g.Quantity,
								ImageUrl = this.GetImageFullPath(g.ThumbnailsUrl)
							},
							ProductList = from d in list
							select new
							{

							}
						}
					});
					context.Response.Write(s2);
				}
			}
		}

		private void GetPlatProducts(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			ProductQuery productQuery = new ProductQuery();
			int num = 1;
			int num2 = 10;
			productQuery.Keywords = context.Request["productName"].ToNullString();
			productQuery.ProductCode = context.Request["productCode"].ToNullString();
			if (!string.IsNullOrEmpty(context.Request["CategoryId"].ToNullString()))
			{
				productQuery.CategoryId = context.Request["categoryId"].ToInt(0);
			}
			if (productQuery.CategoryId.HasValue && productQuery.CategoryId.Value > 0)
			{
				CategoryInfo category = CatalogHelper.GetCategory(productQuery.CategoryId.Value);
				if (category != null)
				{
					productQuery.MaiCategoryPath = category.Path;
				}
			}
			if (context.Request["isWarning"].ToNullString() == "1")
			{
				productQuery.IsWarningStock = true;
			}
			if (!string.IsNullOrEmpty(context.Request["BrandId"].ToNullString()))
			{
				productQuery.BrandId = context.Request["BrandId"].ToInt(0);
			}
			if (!string.IsNullOrEmpty(context.Request["TagId"].ToNullString()))
			{
				productQuery.TagId = context.Request["TagId"].ToInt(0);
			}
			if (!string.IsNullOrEmpty(context.Request["BrandId"].ToNullString()))
			{
				productQuery.TypeId = context.Request["BrandId"].ToInt(0);
			}
			int? nullable = null;
			if (!string.IsNullOrEmpty(context.Request["SaleStatus"].ToNullString()))
			{
				nullable = context.Request["SaleStatus"].ToInt(0);
			}
			if (nullable.HasValue && Enum.IsDefined(typeof(ProductSaleStatus), nullable))
			{
				productQuery.SaleStatus = (ProductSaleStatus)nullable.Value;
			}
			else
			{
				productQuery.SaleStatus = ProductSaleStatus.OnSale;
			}
			num = context.Request["pageindex"].ToInt(0);
			if (num < 1)
			{
				num = 1;
			}
			num2 = context.Request["pagesize"].ToInt(0);
			if (num2 < 1)
			{
				num2 = 10;
			}
			productQuery.ProductType = context.Request["productType"].ToInt(0);
			productQuery.PageSize = num2;
			productQuery.PageIndex = num;
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			productQuery.StoreId = storeIdBySessionId;
			productQuery.IsFilterStoreProducts = true;
			productQuery.SupplierId = 0;
			Globals.EntityCoding(productQuery, true);
			PageModel<StoreProductBaseModel> storeNoRelationProducts = ProductHelper.GetStoreNoRelationProducts(productQuery);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeNoRelationProducts.Total,
					List = from p in storeNoRelationProducts.Models
					select new
					{
						ProductId = p.ProductId,
						ProductName = p.ProductName,
						ImageUrl = this.GetImageFullPath(p.ProductImage),
						Stock = p.Stock,
						Price = p.Price.F2ToString("f2"),
						WarningStock = p.WarningStock,
						ProductType = p.ProductType
					}
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetProducts(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num3 = context.Request["cId"].ToInt(0);
			int num4 = context.Request["productType"].ToInt(0);
			string productName = Globals.StripAllTags(context.Request["keyword"].ToNullString());
			string productCode = Globals.StripAllTags(context.Request["productCode"].ToNullString());
			bool warningStockNum = true;
			if (context.Request["isWarningStock"].ToNullString() != "1")
			{
				warningStockNum = context.Request["isWarningStock"].ToBool();
			}
			if (num2 < 1)
			{
				num2 = 10;
			}
			StoreProductsQuery storeProductsQuery = new StoreProductsQuery();
			storeProductsQuery.PageIndex = num;
			storeProductsQuery.PageSize = num2;
			storeProductsQuery.productCode = productCode;
			storeProductsQuery.WarningStockNum = warningStockNum;
			storeProductsQuery.ProductName = productName;
			if (num3 > 0)
			{
				storeProductsQuery.CategoryId = num3;
				storeProductsQuery.MainCategoryPath = CatalogHelper.GetCategory(num3).Path;
			}
			storeProductsQuery.StoreId = storeIdBySessionId;
			storeProductsQuery.ProductType = num4;
			if (num4 == 1)
			{
				storeProductsQuery.IsChoiceProduct = true;
			}
			PageModel<StoreProductsViewInfo> storeProducts = StoresHelper.GetStoreProducts(storeProductsQuery);
			List<StoreProductsViewInfo> list = new List<StoreProductsViewInfo>();
			foreach (StoreProductsViewInfo model in storeProducts.Models)
			{
				StoreProductsViewInfo storeProductsViewInfo = model;
				storeProductsViewInfo.ThumbnailUrl40 = (string.IsNullOrEmpty(model.ThumbnailUrl40) ? Globals.FullPath(masterSettings.DefaultProductThumbnail8) : Globals.FullPath(model.ThumbnailUrl40));
				storeProductsViewInfo.ThumbnailUrl = (string.IsNullOrEmpty(model.ThumbnailUrl40) ? Globals.FullPath(masterSettings.DefaultProductThumbnail8) : Globals.FullPath(model.ThumbnailUrl40.Replace("/thumbs40/40_", "/thumbs410/410_")));
				list.Add(storeProductsViewInfo);
			}
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeProducts.Total,
					List = list
				}
			});
			context.Response.Write(s);
		}

		private void GetOrders(HttpContext context)
		{
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int num3 = context.Request["status"].ToInt(0);
			if (num3 != 0 && num3 != 1 && num3 != 2 && num3 != 3 && num3 != 5 && num3 != 30 && num3 != 31 && num3 != 32 && num3 != 33 && num3 != 34 && num3 != 6 && num3 != 9 && num3 != 18 && num3 != 10 && num3 != 19 && num3 != 35 && num3 != 36 && num3 != 37 && num3 != 38 && num3 != 39 && num3 != 40 && num3 != 41)
			{
				num3 = 0;
			}
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (num2 < 1)
			{
				num2 = 10;
			}
			OrderQuery orderQuery = new OrderQuery();
			orderQuery.SupplierId = 0;
			orderQuery.PageIndex = num;
			orderQuery.PageSize = num2;
			if (Enum.IsDefined(typeof(OrderStatus), num3))
			{
				orderQuery.Status = (OrderStatus)num3;
				orderQuery.IsServiceOrder = false;
				if (orderQuery.Status == OrderStatus.ApplyForRefund)
				{
					orderQuery.IsStoreCollection = true;
				}
			}
			else
			{
				orderQuery.Status = OrderStatus.All;
			}
			if (num3 == 2 || num3 == 3)
			{
				orderQuery.TakeOnStore = false;
			}
			else
			{
				switch (num3)
				{
				case 30:
					orderQuery.IsConfirm = false;
					orderQuery.ShippingModeId = -2;
					break;
				case 31:
					orderQuery.IsWaitTakeOnStore = true;
					break;
				case 32:
					this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 32);
					return;
				case 33:
					this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 33);
					return;
				case 34:
					this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 34);
					return;
				case 35:
					this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 35);
					return;
				case 36:
					this.GetAfterSaleOrders(context, storeIdBySessionId, num, num2, 36);
					return;
				case 37:
					orderQuery.IsAllTakeOnStore = true;
					break;
				case 38:
					orderQuery.IsTakeOnStoreCompleted = true;
					break;
				case 39:
					orderQuery.IsServiceOrder = false;
					orderQuery.TakeOnStore = false;
					break;
				case 40:
					orderQuery.Status = OrderStatus.Finished;
					orderQuery.ShippingModeId = -2;
					break;
				case 41:
					orderQuery.Status = OrderStatus.Finished;
					orderQuery.TakeOnStore = false;
					break;
				}
			}
			orderQuery.ShowGiftOrder = false;
			orderQuery.StoreId = storeIdBySessionId;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
			List<OrderInfo> list = new List<OrderInfo>();
			DataTable data = orders.Data;
			if (data != null && data.Rows.Count > 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					DataRow dataRow = data.Rows[i];
					OrderInfo orderInfo = TradeHelper.GetOrderInfo(dataRow["OrderId"].ToString());
					if (orderInfo != null)
					{
						list.Add(orderInfo);
					}
				}
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = orders.TotalRecords,
					StoreNeedTakeCode = masterSettings.StoreNeedTakeCode,
					List = list.Select(delegate(OrderInfo c)
					{
						StoreAppAPI storeAppAPI = this;
						string orderId = c.OrderId;
						string shipTo = c.ShipTo;
						OrderStatus orderStatus = c.OrderStatus;
						DadaStatus dadaStatus = c.DadaStatus;
						int allQuantity = c.GetAllQuantity(true);
						string amount = c.GetAmount(false).F2ToString("f2");
						bool isWaitTakeOnDoor = c.ShippingModeId == -2 && c.IsConfirm && (c.OrderStatus == OrderStatus.WaitBuyerPay || c.OrderStatus == OrderStatus.BuyerAlreadyPaid);
						string statusText = (c.OrderType == OrderType.ServiceOrder) ? ((Enum)(object)this.GetOrderStatus(c, null)).ToDescription() : ((c.ShippingModeId != -2 || (c.OrderStatus != OrderStatus.BuyerAlreadyPaid && (c.OrderStatus != OrderStatus.WaitBuyerPay || !(c.Gateway == "hishop.plugins.payment.payonstore")))) ? ((c.OrderStatus == OrderStatus.WaitBuyerPay && c.Gateway == "hishop.plugins.payment.podrequest") ? "待发货" : this.GetDadaStatus(c.OrderStatus, c.DadaStatus, c.ExpressCompanyName)) : (c.IsConfirm ? "待上门自提" : "待确认"));
						OrderItemStatus itemStatus = c.ItemStatus;
						string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)c.ItemStatus, 0);
						string takeCode = c.TakeCode;
						int shippingModeId = c.ShippingModeId;
						string gateway = c.Gateway;
						bool isConfirm = c.IsConfirm;
						bool isCanConfirm = c.CanConfirmOrder();
						string isConfirmText = c.IsConfirm ? "待上门自提" : string.Empty;
						if (c.OrderStatus == OrderStatus.WaitBuyerPay && c.Gateway == "hishop.plugins.payment.podrequest")
						{
							goto IL_0223;
						}
						if (c.OrderStatus == OrderStatus.BuyerAlreadyPaid)
						{
							goto IL_0223;
						}
						int isSendGoods = 0;
						goto IL_0238;
						IL_0238:
						return new
						{
							OrderId = orderId,
							ShipTo = shipTo,
							Status = (int)orderStatus,
							DadaStatus = (int)dadaStatus,
							Quantity = allQuantity,
							Amount = amount,
							IsWaitTakeOnDoor = isWaitTakeOnDoor,
							StatusText = statusText,
							ItemStatus = (int)itemStatus,
							ItemStatusText = enumDescription,
							TakeCode = takeCode,
							ShippingModeId = shippingModeId,
							Gateway = gateway,
							IsConfirm = isConfirm,
							IsCanConfirm = isCanConfirm,
							IsConfirmText = isConfirmText,
							IsSendGoods = ((byte)isSendGoods != 0),
							IsStoreCollect = c.IsStoreCollect,
							StoreId = c.StoreId,
							PaymentTypeId = c.PaymentTypeId,
							PaymentType = c.PaymentType,
							ShipOrderNumber = c.ShipOrderNumber.ToNullString(),
							OrderDate = c.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
							OrderTotal = c.GetTotal(false).F2ToString("f2"),
							FreightFreePromotionName = c.FreightFreePromotionName,
							ReducedPromotionName = c.ReducedPromotionName,
							ReducedPromotionAmount = ((c.ReducedPromotionAmount > decimal.Zero) ? "-" : "") + c.ReducedPromotionAmount.F2ToString("f2"),
							SentTimesPointPromotionName = c.SentTimesPointPromotionName,
							IsCanReturn = false,
							CanBackReturn = TradeHelper.IsCanBackReturn(c),
							CanCashierReturn = c.IsStoreCollect,
							AdjustedFreight = c.AdjustedFreight.F2ToString("f2"),
							Freight = c.Freight.F2ToString("f2"),
							OrderType = c.OrderType.GetHashCode(),
							LineItems = from d in c.LineItems.Keys
							select new
							{
								Status = c.LineItems[d].Status,
								StatusText = c.LineItems[d].StatusText,
								Id = c.LineItems[d].SkuId,
								Name = c.LineItems[d].ItemDescription,
								Price = c.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
								Amount = c.LineItems[d].ShipmentQuantity,
								Image = storeAppAPI.GetImageFullPath(c.LineItems[d].ThumbnailsUrl),
								SkuText = c.LineItems[d].SKUContent,
								ProductId = c.LineItems[d].ProductId,
								PromotionName = c.LineItems[d].PromotionName
							}
						};
						IL_0223:
						isSendGoods = ((c.ShippingModeId != -2) ? 1 : 0);
						goto IL_0238;
					})
				}
			});
			context.Response.Write(s);
		}

		private string GetDadaStatus(OrderStatus orderStatus, DadaStatus dadaStatus, string expressCompanyName)
		{
			if (expressCompanyName == "同城物流配送")
			{
				return EnumDescription.GetEnumDescription((Enum)(object)dadaStatus, 0);
			}
			return EnumDescription.GetEnumDescription((Enum)(object)orderStatus, 0);
		}

		private void ConfirmOrders(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string orderId = context.Request["OrderId"].ToNullString();
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			StoresInfo store = DepotHelper.GetStoreById(storeIdBySessionId);
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
			if (orderInfo == null)
			{
				context.Response.Write(this.GetErrorJosn(113, ((Enum)(object)ApiErrorCode.OrderNumber_Error).ToDescription()));
			}
			else if (orderInfo.StoreId != store.StoreId)
			{
				context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
			}
			else if (orderInfo.PaymentTypeId != -3 && orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid)
			{
				context.Response.Write(this.GetErrorJosn(114, ((Enum)(object)ApiErrorCode.OrderStatus_Error).ToDescription()));
			}
			else if (orderInfo.IsConfirm)
			{
				context.Response.Write(this.GetErrorJosn(510, ((Enum)(object)ApiErrorCode.StoreOrder_IsConfirmed).ToDescription()));
			}
			else
			{
				IList<StoreProductModel> list = new List<StoreProductModel>();
				if (orderInfo.OrderStatus == OrderStatus.WaitBuyerPay && OrderHelper.GetNoStockItems(orderInfo) != null)
				{
					foreach (LineItemInfo value in orderInfo.LineItems.Values)
					{
						StoreProductModel storeProductInfo = StoresHelper.GetStoreProductInfo(store.StoreId, value.ProductId);
						list.Add(storeProductInfo);
					}
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "STOCK NOT ENOUGH",
							ProductList = from d in list
							select new
							{
								ProductName = d.ProductName,
								ProductId = d.ProductId,
								Stock = StoresHelper.GetStoreProductStock(store.StoreId, d.ProductId),
								ProductCode = d.ProductCode,
								Image = this.GetImageFullPath(d.ThumbnailUrl410),
								SkuList = from sku in d.Skus.Values
								select new
								{
									SkuId = sku.SkuId,
									StoreStock = sku.Stock,
									SkuText = ProductHelper.GetSkusBySkuId(sku.SkuId, d.ProductId),
									ProductCode = sku.SKU,
									WarningStock = sku.WarningStock
								}
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				string empty = string.Empty;
				if (OrderHelper.ConfirmTakeOnStoreOrder(orderInfo, out empty, false, store.StoreName, true))
				{
					MemberInfo user = Users.GetUser(orderInfo.UserId);
					if (user == null)
					{
						context.Response.Write(this.GetErrorJosn(603, ((Enum)(object)ApiErrorCode.CouponUserInfo_Error).ToDescription()));
					}
					else
					{
						if (!string.IsNullOrEmpty(masterSettings.HiPOSAppId) && !string.IsNullOrEmpty(masterSettings.HiPOSAppSecret) && !string.IsNullOrEmpty(masterSettings.HiPOSMerchantId) && !string.IsNullOrEmpty(masterSettings.HiPOSExpireAt) && masterSettings.HiPOSExpireAt.ToDateTime() > (DateTime?)DateTime.Now)
						{
							string empty2 = string.Empty;
							string siteUrl = masterSettings.SiteUrl;
							string text = Globals.HIPOSTAKECODEPREFIX + orderInfo.TakeCode;
							empty2 = ((siteUrl.IndexOf("http") >= 0) ? (masterSettings.SiteUrl + "/QRTakeCode.aspx?takeCode=" + text) : ("http://" + masterSettings.SiteUrl + "/QRTakeCode.aspx?takeCode=" + text));
							Messenger.OrderConfirmTakeOnStore(orderInfo, user, store, empty2);
						}
						else
						{
							Messenger.OrderConfirmTakeOnStore(orderInfo, user, store, "");
						}
						string s2 = JsonConvert.SerializeObject(new
						{
							Result = new
							{
								Status = "SUCCESS",
								ProductList = from d in list
								select new
								{

								}
							}
						});
						context.Response.Write(s2);
						context.Response.End();
					}
				}
			}
		}

		private void ConfirmTackGoods(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			int num = context.Request["PaymentType"].ToInt(0);
			if (!Enum.IsDefined(typeof(EnumPaymentType), num))
			{
				num = 3;
			}
			string text = context.Request["TakeCode"].ToNullString();
			if (string.IsNullOrEmpty(text) && masterSettings.StoreNeedTakeCode)
			{
				context.Response.Write(this.GetErrorJosn(508, ((Enum)(object)ApiErrorCode.TakeCode_Error).ToDescription()));
			}
			else
			{
				string text2 = context.Request["OrderId"].ToNullString();
				if (string.IsNullOrEmpty(text) && string.IsNullOrWhiteSpace(text2))
				{
					context.Response.Write(this.GetErrorJosn(508, ((Enum)(object)ApiErrorCode.TakeCode_Error).ToDescription()));
				}
				else
				{
					if (!string.IsNullOrEmpty(text) && text.ToLower().StartsWith("ysc"))
					{
						text = text.Substring(3);
					}
					int storeIdBySessionId = this.GetStoreIdBySessionId(context);
					OrderInfo order = null;
					if (!string.IsNullOrWhiteSpace(text))
					{
						order = OrderHelper.ValidateTakeCode(text, "");
					}
					else
					{
						order = OrderHelper.GetOrderInfo(text2);
					}
					if (order == null)
					{
						context.Response.Write(this.GetErrorJosn(508, ((Enum)(object)ApiErrorCode.TakeCode_Error).ToDescription()));
					}
					else if (order.StoreId != storeIdBySessionId)
					{
						context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
					}
					else if (order.OrderStatus != OrderStatus.WaitBuyerPay && order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
					{
						context.Response.Write(this.GetErrorJosn(509, ((Enum)(object)ApiErrorCode.TakeCode_AlreadyUsed).ToDescription()));
					}
					else
					{
						string gateway = "hishop.plugins.payment.cashreceipts";
						if (masterSettings.StoreNeedTakeCode && order.TakeCode != text)
						{
							context.Response.Write(this.GetErrorJosn(508, ((Enum)(object)ApiErrorCode.TakeCode_Error).ToDescription()));
						}
						else if (order.FightGroupId > 0 && order.FightGroupStatus != FightGroupStatus.FightGroupSuccess)
						{
							context.Response.Write(this.GetErrorJosn(114, "该订单是火拼团订单，但是组团还没未成功,不能提货!"));
						}
						else
						{
							bool flag = true;
							if (order.OrderStatus == OrderStatus.WaitBuyerPay)
							{
								flag = false;
								order.IsStoreCollect = true;
								switch (num)
								{
								case 3:
									order.Gateway = gateway;
									order.PaymentType = "现金支付";
									order.PaymentTypeId = -99;
									break;
								case 1:
								{
									PaymentModeInfo paymentMode2 = SalesHelper.GetPaymentMode("hishop.plugins.payment.ws_wappay.wswappayrequest");
									if (paymentMode2 != null)
									{
										order.Gateway = paymentMode2.Gateway;
										order.PaymentType = paymentMode2.Name;
										order.PaymentTypeId = paymentMode2.ModeId;
									}
									break;
								}
								case 2:
								{
									PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode("hishop.plugins.payment.weixinrequest");
									if (paymentMode != null)
									{
										order.Gateway = paymentMode.Gateway;
										order.PaymentType = paymentMode.Name;
										order.PaymentTypeId = paymentMode.ModeId;
									}
									break;
								}
								}
								OrderHelper.UpdateOrderPaymentTypeOfAPI(order);
							}
							if (OrderHelper.ConfirmTakeGoods(order, true))
							{
								if (!flag)
								{
									StoreCollectionInfo storeCollectionInfo = new StoreCollectionInfo();
									storeCollectionInfo.CreateTime = order.OrderDate;
									storeCollectionInfo.FinishTime = DateTime.Now;
									storeCollectionInfo.PayTime = DateTime.Now;
									storeCollectionInfo.GateWay = order.Gateway;
									storeCollectionInfo.PaymentTypeId = num;
									storeCollectionInfo.PaymentTypeName = EnumDescription.GetEnumDescription((Enum)(object)(EnumPaymentType)num, 0);
									storeCollectionInfo.OrderId = order.OrderId;
									storeCollectionInfo.OrderType = 1;
									storeCollectionInfo.PayAmount = order.GetTotal(false);
									storeCollectionInfo.RefundAmount = decimal.Zero;
									storeCollectionInfo.Remark = "上门自提订单确认提货:" + order.OrderId;
									storeCollectionInfo.SerialNumber = Globals.GetGenerateId();
									storeCollectionInfo.Status = 1;
									storeCollectionInfo.StoreId = storeIdBySessionId;
									storeCollectionInfo.UserId = order.UserId;
									StoresHelper.AddStoreCollectionInfo(storeCollectionInfo);
								}
								string orderId = order.OrderId;
								DateTime dateTime;
								object takeCodeUsedTime;
								if (!string.IsNullOrEmpty(order.TakeTime))
								{
									dateTime = order.TakeTime.ToDateTime().Value;
									takeCodeUsedTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
								}
								else
								{
									takeCodeUsedTime = "";
								}
								string takeCode = order.TakeCode.ToNullString().ToLower().Replace("ysc", "");
								string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)order.OrderStatus, 0);
								OrderStatus orderStatus = order.OrderStatus;
								OrderItemStatus itemStatus = order.ItemStatus;
								string enumDescription2 = EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0);
								dateTime = order.OrderDate;
								string s = JsonConvert.SerializeObject(new
								{
									Result = new
									{
										OrderId = orderId,
										TakeCodeIsUsed = false,
										TakeCodeUsedTime = (string)takeCodeUsedTime,
										TakeCode = takeCode,
										StatusText = enumDescription,
										Status = (int)orderStatus,
										ItemStatus = (int)itemStatus,
										ItemStatusText = enumDescription2,
										OrderDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
										ShipTo = order.ShipTo,
										ShipToDate = order.ShipToDate,
										Cellphone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone),
										Address = order.Address,
										OrderTotal = order.GetTotal(false).F2ToString("f2"),
										FreightFreePromotionName = order.FreightFreePromotionName,
										ReducedPromotionName = order.ReducedPromotionName,
										ReducedPromotionAmount = ((order.ReducedPromotionAmount > decimal.Zero) ? "-" : "") + order.ReducedPromotionAmount.F2ToString("f2"),
										SentTimesPointPromotionName = order.SentTimesPointPromotionName,
										LineItems = from d in order.LineItems.Keys
										select new
										{
											Status = order.LineItems[d].Status,
											StatusText = order.LineItems[d].StatusText,
											Id = order.LineItems[d].SkuId,
											Name = order.LineItems[d].ItemDescription,
											Price = order.LineItems[d].RealTotalPrice.F2ToString("f2"),
											Amount = order.LineItems[d].ShipmentQuantity,
											Image = this.GetImageFullPath(order.LineItems[d].ThumbnailsUrl),
											SkuText = order.LineItems[d].SKUContent,
											ProductId = order.LineItems[d].ProductId,
											PromotionName = order.LineItems[d].PromotionName
										},
										Gifts = from g in order.Gifts
										select new
										{
											GiftId = g.GiftId,
											GiftName = g.GiftName,
											PromoteType = g.PromoteType,
											Quantity = g.Quantity,
											ImageUrl = this.GetImageFullPath(g.ThumbnailsUrl)
										}
									}
								});
								context.Response.Write(s);
								context.Response.End();
							}
							else
							{
								context.Response.Write(this.GetErrorJosn(506, ((Enum)(object)ApiErrorCode.StoreStock_NotEnough).ToDescription()));
							}
						}
					}
				}
			}
		}

		private void GetOrderDetailOfTackCode(HttpContext context)
		{
			string text = context.Request["TakeCode"].ToNullString();
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(508, ((Enum)(object)ApiErrorCode.TakeCode_Error).ToDescription()));
			}
			else
			{
				if (text.ToLower().StartsWith("ysc"))
				{
					text = text.Substring(3);
				}
				OrderInfo order = OrderHelper.ValidateTakeCode(text, "");
				if (order == null)
				{
					context.Response.Write(this.GetErrorJosn(508, ((Enum)(object)ApiErrorCode.TakeCode_Error).ToDescription()));
				}
				else if (order.StoreId != storeIdBySessionId)
				{
					context.Response.Write(this.GetErrorJosn(507, ((Enum)(object)ApiErrorCode.NoStore_Order).ToDescription()));
				}
				else
				{
					bool flag = false;
					if (order.OrderStatus != OrderStatus.WaitBuyerPay && order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
					{
						flag = true;
					}
					string orderId = order.OrderId;
					bool takeCodeIsUsed = flag;
					DateTime dateTime;
					object takeCodeUsedTime;
					if (!string.IsNullOrEmpty(order.TakeTime))
					{
						dateTime = order.TakeTime.ToDateTime().Value;
						takeCodeUsedTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
					}
					else
					{
						takeCodeUsedTime = "";
					}
					string takeCode = order.TakeCode.ToNullString().ToLower().Replace("ysc", "");
					string statusText = (order.ShippingModeId != -2 || (order.OrderStatus != OrderStatus.BuyerAlreadyPaid && (order.OrderStatus != OrderStatus.WaitBuyerPay || !(order.Gateway == "hishop.plugins.payment.payonstore")))) ? EnumDescription.GetEnumDescription((Enum)(object)order.OrderStatus, 0) : (order.IsConfirm ? "待上门自提" : "待确认");
					OrderStatus orderStatus = order.OrderStatus;
					OrderItemStatus itemStatus = order.ItemStatus;
					string enumDescription = EnumDescription.GetEnumDescription((Enum)(object)order.ItemStatus, 0);
					dateTime = order.OrderDate;
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							OrderId = orderId,
							TakeCodeIsUsed = takeCodeIsUsed,
							TakeCodeUsedTime = (string)takeCodeUsedTime,
							TakeCode = takeCode,
							StatusText = statusText,
							Status = (int)orderStatus,
							ItemStatus = (int)itemStatus,
							ItemStatusText = enumDescription,
							OrderDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
							ShipTo = order.ShipTo,
							ShipToDate = order.ShipToDate,
							Cellphone = (string.IsNullOrEmpty(order.CellPhone) ? order.TelPhone : order.CellPhone),
							Address = order.Address,
							OrderTotal = order.GetTotal(false).F2ToString("f2"),
							FreightFreePromotionName = order.FreightFreePromotionName,
							ReducedPromotionName = order.ReducedPromotionName,
							ReducedPromotionAmount = ((order.ReducedPromotionAmount > decimal.Zero) ? "-" : "") + order.ReducedPromotionAmount.F2ToString("f2"),
							SentTimesPointPromotionName = order.SentTimesPointPromotionName,
							Remark = order.Remark,
							InvoiceTitle = order.InvoiceTitle,
							Tax = order.Tax,
							BalanceAmount = order.BalanceAmount,
							LineItems = from d in order.LineItems.Keys
							select new
							{
								Status = order.LineItems[d].Status,
								StatusText = order.LineItems[d].StatusText,
								Id = order.LineItems[d].SkuId,
								Name = order.LineItems[d].ItemDescription,
								Price = order.LineItems[d].ItemAdjustedPrice.F2ToString("f2"),
								Amount = order.LineItems[d].ShipmentQuantity,
								Image = this.GetImageFullPath(order.LineItems[d].ThumbnailsUrl),
								SkuText = order.LineItems[d].SKUContent,
								ProductId = order.LineItems[d].ProductId,
								PromotionName = order.LineItems[d].PromotionName
							},
							Gifts = from g in order.Gifts
							select new
							{
								GiftId = g.GiftId,
								GiftName = g.GiftName,
								PromoteType = g.PromoteType,
								Quantity = g.Quantity,
								ImageUrl = this.GetImageFullPath(g.ThumbnailsUrl)
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetStoreFloorList(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			IList<StoreFloorInfo> storeFloorList = StoresHelper.GetStoreFloorList(storeIdBySessionId, FloorClientType.Mobbile);
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

		public void GetStoreFloorDetail(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["FloorId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				StoreFloorInfo storeFloorBaseInfo = StoresHelper.GetStoreFloorBaseInfo(num);
				if (storeFloorBaseInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(1007, ""));
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							FloorId = storeFloorBaseInfo.FloorId,
							FloorName = storeFloorBaseInfo.FloorName,
							ImageId = storeFloorBaseInfo.ImageId,
							ImgName = (string.IsNullOrEmpty(storeFloorBaseInfo.ImageName) ? "" : storeFloorBaseInfo.ImageName),
							Products = from d in storeFloorBaseInfo.Products
							select new
							{
								d.ProductId
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void GetStoreProductsFloorDisplaySequence(HttpContext context)
		{
			SiteSettings setting = SettingsManager.GetMasterSettings();
			int num = context.Request["pageIndex"].ToInt(0);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = context.Request["pageSize"].ToInt(0);
			int storeId = this.GetStoreIdBySessionId(context);
			int num3 = context.Request["cId"].ToInt(0);
			int floorId = context.Request["floorId"].ToInt(0);
			string productName = Globals.StripAllTags(context.Request["keyword"].ToNullString());
			string productCode = Globals.StripAllTags(context.Request["productCode"].ToNullString());
			bool warningStockNum = true;
			if (context.Request["isWarningStock"].ToNullString() != "1")
			{
				warningStockNum = context.Request["isWarningStock"].ToBool();
			}
			if (num2 < 1)
			{
				num2 = 10;
			}
			StoreProductsQuery storeProductsQuery = new StoreProductsQuery();
			storeProductsQuery.PageIndex = num;
			storeProductsQuery.PageSize = num2;
			storeProductsQuery.productCode = productCode;
			storeProductsQuery.WarningStockNum = warningStockNum;
			storeProductsQuery.ProductName = productName;
			if (num3 > 0)
			{
				storeProductsQuery.CategoryId = num3;
				storeProductsQuery.MainCategoryPath = CatalogHelper.GetCategory(num3).Path;
			}
			storeProductsQuery.StoreId = storeId;
			storeProductsQuery.SaleStatus = 1.GetHashCode();
			PageModel<StoreProductsViewInfo> storeProductsFloorDisplaySequence = StoresHelper.GetStoreProductsFloorDisplaySequence(storeProductsQuery, floorId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					RecordCount = storeProductsFloorDisplaySequence.Total,
					List = from d in storeProductsFloorDisplaySequence.Models
					select new
					{
						StoreId = storeId,
						ProductId = d.ProductId,
						Stock = d.Stock,
						ProductCode = d.ProductCode,
						ProductName = d.ProductName,
						CategoryId = d.CategoryId,
						SaleStatus = d.SaleStatus,
						SalePrice = d.SalePrice,
						CostPrice = d.CostPrice,
						MarketPrice = d.MarketPrice,
						ThumbnailUrl40 = (string.IsNullOrEmpty(d.ThumbnailUrl40) ? Globals.FullPath(setting.DefaultProductThumbnail8) : Globals.FullPath(d.ThumbnailUrl40.Replace("/thumbs40/40_", "/thumbs410/410_"))),
						MainCategoryPath = d.MainCategoryPath,
						ExtendCategoryPath = d.ExtendCategoryPath,
						ExtendCategoryPath1 = d.ExtendCategoryPath1,
						ExtendCategoryPath2 = d.ExtendCategoryPath2,
						ExtendCategoryPath3 = d.ExtendCategoryPath3,
						ExtendCategoryPath4 = d.ExtendCategoryPath4,
						WarningStockNum = d.WarningStockNum,
						DisplaySequence = d.DisplaySequence,
						IsChecked = ((d.StoreId.ToInt(0) > 0) ? "checked" : "")
					}
				}
			});
			context.Response.Write(s);
		}

		public void AddStoreFloor(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string text = context.Request["FloorName"].ToNullString();
			int imageId = context.Request["ImageId"].ToInt(0);
			string text2 = context.Request["ProductIds"].ToNullString();
			if (text.Trim().Length < 1 || text.Trim().Length > 12)
			{
				context.Response.Write(this.GetErrorJosn(101, "楼层名称不能为空，且在1-12个字符之间"));
			}
			else
			{
				StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
				storeFloorInfo.StoreId = storeIdBySessionId;
				storeFloorInfo.FloorName = text;
				storeFloorInfo.ImageId = imageId;
				storeFloorInfo.FloorClientType = FloorClientType.Mobbile;
				int floorId = StoresHelper.AddStoreFloor(storeFloorInfo);
				if (!string.IsNullOrEmpty(text2))
				{
					StoresHelper.BindStoreFloorProducts(floorId, storeIdBySessionId, text2);
				}
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						Msg = string.Empty
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void UpdateStoreFloor(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["FloorId"].ToInt(0);
			string text = context.Request["FloorName"].ToNullString();
			int imageId = context.Request["ImageId"].ToInt(0);
			string text2 = context.Request["ProductIds"].ToNullString();
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else if (text.Trim().Length < 1 || text.Trim().Length > 12)
			{
				context.Response.Write(this.GetErrorJosn(101, "楼层名称不能为空，且在1-12个字符之间"));
			}
			else
			{
				StoreFloorInfo storeFloorInfo = new StoreFloorInfo();
				storeFloorInfo = StoresHelper.GetStoreFloorBaseInfo(num);
				if (storeFloorInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(101.GetHashCode(), "错误的楼层ID"));
				}
				else
				{
					storeFloorInfo.FloorName = text;
					storeFloorInfo.ImageId = imageId;
					storeFloorInfo.FloorClientType = FloorClientType.Mobbile;
					StoresHelper.UpdateStoreFloor(storeFloorInfo);
					if (!string.IsNullOrEmpty(text2))
					{
						StoresHelper.BindStoreFloorProducts(num, storeIdBySessionId, text2);
					}
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = true,
							Msg = string.Empty
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void UpdateStoreFloorDisplaySequence(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			string text = context.Request["FloorIds"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				StoresHelper.UpdateStoreFloorDisplaySequence(storeIdBySessionId, text);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						Msg = string.Empty
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		public void DeleteStoreFloor(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int num = context.Request["FloorId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				StoresHelper.DeleteStoreFloor(num);
				string s = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = true,
						Msg = string.Empty
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
		}

		private string GetImageFullPath(string imageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl))
			{
				return Globals.FullPath(HiContext.Current.SiteSettings.DefaultProductThumbnail8);
			}
			if (imageUrl.StartsWith("http://"))
			{
				return imageUrl;
			}
			return Globals.FullPath(imageUrl);
		}

		private void OrderStatistics(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			if (storeIdBySessionId <= 0)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			StoreStatisticsInfo storeOrderStatistics = StoresHelper.GetStoreOrderStatistics(storeIdBySessionId);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					storeOrderStatistics.WaitConfirmTotal,
					storeOrderStatistics.WaitDealAfterSaleTotal,
					storeOrderStatistics.WaitGetGoodsTotal,
					storeOrderStatistics.WaitPickGoodsTotal,
					storeOrderStatistics.WaitSendGoodsTotal,
					storeOrderStatistics.TodayOrderTotal,
					storeOrderStatistics.StockWarningTotal
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetOrderNumber(HttpContext context)
		{
			int storeIdBySessionId = this.GetStoreIdBySessionId(context);
			int status = context.Request["status"].ToInt(0);
			if (storeIdBySessionId <= 0)
			{
				context.Response.Write(this.GetErrorJosn(512, ((Enum)(object)ApiErrorCode.SessionId_NoRelationStore).ToDescription()));
				context.Response.End();
			}
			int storeOrderStatistics = StoresHelper.GetStoreOrderStatistics(storeIdBySessionId, status);
			string s = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					OrderNumber = storeOrderStatistics
				}
			});
			context.Response.Write(s);
			context.Response.End();
		}

		private void StoreAdminLogin(HttpContext context)
		{
			string text = context.Request["username"].ToNullString();
			string text2 = context.Request["password"].ToNullString();
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write(this.GetErrorJosn(503, ((Enum)(object)ApiErrorCode.StoreAdminName_Empty).ToDescription()));
			}
			else if (string.IsNullOrEmpty(text2))
			{
				context.Response.Write(this.GetErrorJosn(504, ((Enum)(object)ApiErrorCode.StoreAdminPassword_Empty).ToDescription()));
			}
			else if (!HiContext.Current.SiteSettings.OpenMultStore)
			{
				context.Response.Write(this.GetErrorJosn(532, ((Enum)(object)ApiErrorCode.NoOpenMultStore).ToDescription()));
			}
			else
			{
				ManagerInfo managerInfo = null;
				try
				{
					managerInfo = ManagerHelper.StoreValidLogin(text, text2);
				}
				catch (Exception ex)
				{
					context.Response.Write(this.GetErrorJosn(999, ((Enum)(object)ApiErrorCode.Unknown_Error).ToDescription() + ":" + ex.Message));
					return;
				}
				if (managerInfo == null)
				{
					context.Response.Write(this.GetErrorJosn(505, ((Enum)(object)ApiErrorCode.StoreAdminNameOrPassword_Error).ToDescription()));
				}
				else if (managerInfo.Status == 0 && managerInfo.RoleId == -3)
				{
					context.Response.Write(this.GetErrorJosn(527, ((Enum)(object)ApiErrorCode.ShoppingGuiderIsFreezed).ToDescription()));
				}
				else if (managerInfo.RoleId != -1 && managerInfo.RoleId != -3)
				{
					context.Response.Write(this.GetErrorJosn(502, ((Enum)(object)ApiErrorCode.NoStore_Admin).ToDescription()));
				}
				else
				{
					StoresInfo storeById = StoresHelper.GetStoreById(managerInfo.StoreId);
					if (storeById == null)
					{
						context.Response.Write(this.GetErrorJosn(501, ((Enum)(object)ApiErrorCode.Store_NoExist).ToDescription()));
						context.Response.End();
					}
					if (storeById.State == 0)
					{
						context.Response.Write(this.GetErrorJosn(533, ((Enum)(object)ApiErrorCode.StoreIsClosed).ToDescription()));
						context.Response.End();
					}
					object obj;
					DateTime value;
					if (!storeById.CloseStatus)
					{
						obj = "";
					}
					else if (!storeById.CloseBeginTime.HasValue)
					{
						obj = "";
					}
					else
					{
						value = storeById.CloseBeginTime.Value;
						obj = value.ToString("yyyy-MM-dd HH:mm:ss");
					}
					string closeBeginTime = (string)obj;
					object obj2;
					if (!storeById.CloseStatus)
					{
						obj2 = "";
					}
					else if (!storeById.CloseEndTime.HasValue)
					{
						obj2 = "";
					}
					else
					{
						value = storeById.CloseEndTime.Value;
						obj2 = value.ToString("yyyy-MM-dd HH:mm:ss");
					}
					string closeEndTime = (string)obj2;
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							ManagerId = managerInfo.ManagerId,
							UserName = managerInfo.UserName,
							RoleId = ((managerInfo.RoleId == -1) ? 1 : 2),
							CreateDate = managerInfo.CreateDate,
							StoreName = storeById.StoreName,
							StoreId = managerInfo.StoreId,
							SessionId = managerInfo.SessionId,
							AliasName = HiContext.Current.SiteSettings.SiteUrl.ToLower().Replace("https://", "").Replace("http://", "")
								.Replace("www.", "")
								.Replace(".", "") + "_" + managerInfo.StoreId,
							IsAboveSelf = (storeById.IsAboveSelf ? "1" : "0"),
							IsSupportExpress = (storeById.IsSupportExpress ? "1" : "0"),
							IsStoreDelive = (storeById.IsStoreDelive ? "1" : "0"),
							IsShelvesProduct = (storeById.IsShelvesProduct ? "1" : "0"),
							IsModifyPrice = (storeById.IsModifyPrice ? "1" : "0"),
							IsSetTradePassword = (!string.IsNullOrEmpty(storeById.TradePassword) && !string.IsNullOrEmpty(storeById.TradePasswordSalt) && true),
							IsOpenVstore = ((HiContext.Current.SiteSettings.OpenVstore == 1 && !string.IsNullOrEmpty(HiContext.Current.SiteSettings.WeixinAppId) && !string.IsNullOrEmpty(HiContext.Current.SiteSettings.WeixinAppSecret)) ? "1" : "0"),
							CloseStatus = storeById.CloseStatus,
							CloseBeginTime = closeBeginTime,
							CloseEndTime = closeEndTime,
							StoreImg = Globals.GetImageServerUrl("http://", HiContext.Current.GetStoragePath() + "/depot/thum_" + storeById.StoreImages.Substring(storeById.StoreImages.LastIndexOf("/") + 1)),
							OpenWXO2OApplet = HiContext.Current.SiteSettings.OpenWXO2OApplet
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		private void GetGuiderReferralCode(HttpContext context)
		{
			int num = context.Request["ShoppingGuiderId"].ToInt(0);
			if (num <= 0)
			{
				context.Response.Write(this.GetErrorJosn(101, ((Enum)(object)ApiErrorCode.Paramter_Error).ToDescription()));
			}
			else
			{
				string text = "";
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				if (siteSettings.OpenVstore == 1 && !string.IsNullOrEmpty(siteSettings.WeixinAppId) && !string.IsNullOrEmpty(siteSettings.WeixinAppSecret))
				{
					string accesstoken = AccessTokenContainer.TryGetToken(siteSettings.WeixinAppId, siteSettings.WeixinAppSecret, false);
					string qRLIMITSTRSCENETicket = this.GetQRLIMITSTRSCENETicket(accesstoken, "shoppingguiderid:" + num.ToNullString(), true);
					text = $"https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={qRLIMITSTRSCENETicket}";
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							CodeUrl = text
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
				else
				{
					context.Response.Write(this.GetErrorJosn(103, "未开通微商城或配置错误"));
				}
			}
		}

		private string GetResponseResult(string url)
		{
			ServicePointManager.ServerCertificateValidationCallback = StoreAppAPI.CheckValidationResult;
			WebRequest webRequest = WebRequest.Create(url);
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (Stream stream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
					{
						return streamReader.ReadToEnd();
					}
				}
			}
		}

		public string GetResponseResult(string url, string param)
		{
			string result = string.Empty;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/json;charset=UTF-8";
				byte[] bytes = Encoding.UTF8.GetBytes(param);
				httpWebRequest.ContentLength = bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				ServicePointManager.ServerCertificateValidationCallback = StoreAppAPI.CheckValidationResult;
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (Stream stream = httpWebResponse.GetResponseStream())
					{
						using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex)
			{
				IDictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ResponseUrl", url);
				dictionary.Add("ResponseParam", param);
				Globals.WriteExceptionLog(ex, dictionary, "GetResponseResult");
			}
			return result;
		}

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true;
		}

		public string GetQRLIMITSTRSCENETicket(string accesstoken, string scene_str, bool first = true)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string param = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_str + "\"}}}";
			string responseResult = this.GetResponseResult($"https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={accesstoken}", param);
			string result = string.Empty;
			if (responseResult.IndexOf("ticket") != -1)
			{
				JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
				result = jObject["ticket"].ToString();
			}
			else
			{
				Globals.AppendLog(responseResult, accesstoken, "", "GetQRLIMITSTRSCENETicket");
				if (responseResult.Contains("access_token is invalid or not latest") & first)
				{
					accesstoken = AccessTokenContainer.TryGetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, true);
					return this.GetQRLIMITSTRSCENETicket(accesstoken, scene_str, false);
				}
			}
			return result;
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
