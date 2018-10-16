using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.WeChatApplet;
using Hidistro.Messages;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.WeChartApplet;
using Hidistro.SqlDal.WeChatApplet;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class ServiceVerification : StoreAdminBaseHandler
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
			if (context.Request["flag"] == "GetFinishedVerificationRecord")
			{
				int storeId = base.CurrentManager.StoreId;
				int managerId = 0;
				if (base.CurrentManager.RoleId == -3)
				{
					managerId = base.CurrentManager.ManagerId;
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
				DbQueryResult finishedVerificationRecord = OrderHelper.GetFinishedVerificationRecord(pagination, storeId, keyword, managerId);
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
			DateTime value;
			if (context.Request["flag"] == "CheckVerification")
			{
				int storeId2 = base.CurrentManager.StoreId;
				string verificationPassword = context.Request["VerificationItems"];
				OrderVerificationItemInfo verificationInfoByPassword = OrderHelper.GetVerificationInfoByPassword(verificationPassword);
				if (verificationInfoByPassword == null)
				{
					context.Response.Write(this.GetErrorJosn(801, "该核销码无效，请重新输入"));
					return;
				}
				if (verificationInfoByPassword.StoreId != storeId2)
				{
					context.Response.Write(this.GetErrorJosn(802, "非本门店核销码，请买家核对信息"));
					return;
				}
				if (verificationInfoByPassword.VerificationStatus == 1)
				{
					HttpResponse response = context.Response;
					value = verificationInfoByPassword.VerificationDate.Value;
					response.Write(this.GetErrorJosn(801, "该核销码 于" + value.ToString("yyyy-MM-dd HH:mm:ss") + "已核销"));
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
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Success = new
						{
							Status = true,
							Msg = "核销码可用"
						}
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
			decimal num3 = default(decimal);
			if (context.Request["flag"] == "OrderVerification")
			{
				int storeId3 = base.CurrentManager.StoreId;
				string text = context.Request["VerificationItems"];
				string[] array = text.Split(',');
				DateTime now = DateTime.Now;
				string text2 = "";
				OrderVerificationItemInfo orderVerificationItemInfo = null;
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						OrderVerificationItemInfo verificationInfoByPassword2 = OrderHelper.GetVerificationInfoByPassword(array[i]);
						if (i == 0)
						{
							orderVerificationItemInfo = verificationInfoByPassword2;
							orderVerificationItemInfo.VerificationDate = DateTime.Now;
						}
						if (verificationInfoByPassword2 == null)
						{
							context.Response.Write(this.GetErrorJosn(801, "该核销码无效，请重新输入"));
							return;
						}
						if (verificationInfoByPassword2.StoreId != storeId3)
						{
							context.Response.Write(this.GetErrorJosn(802, "非本门店核销码，请买家核对信息"));
							return;
						}
						if (verificationInfoByPassword2.VerificationStatus == 1)
						{
							HttpResponse response2 = context.Response;
							value = verificationInfoByPassword2.VerificationDate.Value;
							response2.Write(this.GetErrorJosn(801, "该核销码 于" + value.ToString("yyyy-MM-dd HH:mm:ss") + "已核销"));
							return;
						}
						if (verificationInfoByPassword2.VerificationStatus == 3)
						{
							context.Response.Write(this.GetErrorJosn(801, "核销码已过期，无法核销"));
							return;
						}
						if (verificationInfoByPassword2.VerificationStatus == 5)
						{
							context.Response.Write(this.GetErrorJosn(801, "此核销码已进行售后，无法核销"));
							return;
						}
						if (verificationInfoByPassword2.VerificationStatus == 4)
						{
							context.Response.Write(this.GetErrorJosn(801, "此核销码正在进行售后，无法核销"));
							return;
						}
						verificationInfoByPassword2.VerificationStatus = 1;
						verificationInfoByPassword2.VerificationDate = now;
						verificationInfoByPassword2.ManagerId = base.CurrentManager.ManagerId;
						verificationInfoByPassword2.UserName = base.CurrentManager.UserName;
						OrderHelper.UpdateVerificationItem(verificationInfoByPassword2);
						text2 = verificationInfoByPassword2.OrderId;
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
					num3 = serviceProductOrderInfo.GetTotal(false) / (decimal)serviceProductOrderInfo.GetBuyQuantity() * (decimal)array.Length;
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
					Messenger.ServiceOrderValidSuccess(orderVerificationItemInfo, user, serviceProductOrderInfo, productName, storeName, text, num3);
					if (OrderHelper.IsVerificationFinished(text2) && serviceProductOrderInfo.ItemStatus == OrderItemStatus.Nomarl)
					{
						serviceProductOrderInfo.OrderStatus = OrderStatus.Finished;
						serviceProductOrderInfo.FinishDate = DateTime.Now;
						TradeHelper.UpdateOrderInfo(serviceProductOrderInfo);
					}
				}
				string s3 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Status = "SUCCESS",
						VerificationDate = now.ToString("yyyy-MM-dd HH:mm:ss")
					}
				});
				context.Response.Write(s3);
				context.Response.End();
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
