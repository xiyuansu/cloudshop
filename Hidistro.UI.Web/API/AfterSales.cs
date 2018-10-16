using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using Newtonsoft.Json;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class AfterSales : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			string text = context.Request["action"];
			string text2 = text;
			switch (text2)
			{
			default:
				if (text2 == "checkPay")
				{
					this.CheckPay(context);
				}
				break;
			case "getrefund":
				this.GetRefundInfo(context);
				break;
			case "getreturn":
				this.GetReturnInfo(context);
				break;
			case "getreplace":
				this.GetReplaceInfo(context);
				break;
			}
		}

		public void CheckPay(HttpContext context)
		{
			if (!this.CheckUserRole())
			{
				this.ShowMessage(context, "请先登录会员！", true);
			}
			else
			{
				string orderId = context.Request["orderId"].ToNullString();
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
				string str = "";
				if (orderInfo == null || !TradeHelper.CheckOrderStockBeforePay(orderInfo, out str))
				{
					this.ShowMessage(context, Globals.UrlDecode(str + ",库存不足，不能进行支付"), true);
				}
				else
				{
					this.ShowMessage(context, "库存充足", false);
				}
			}
		}

		public void ShowMessage(HttpContext context, string msg, bool IsError = true)
		{
			if (IsError)
			{
				context.Response.Write("{\"success\":false,\"msg\":\"" + msg + "\"}");
			}
			else
			{
				context.Response.Write("{\"success\":true,\"msg\":\"" + msg + "\"}");
			}
		}

		public void ShowMessage_New(HttpContext context, string msg, bool IsError = true)
		{
			string text = JsonConvert.SerializeObject(new
			{
				Result = new
				{
					success = IsError.ToString().ToLower(),
					msg = msg
				}
			});
		}

		public bool CheckUserRole()
		{
			if (HiContext.Current.UserId == 0)
			{
				return false;
			}
			return true;
		}

		public void GetRefundInfo(HttpContext context)
		{
			if (!this.CheckUserRole())
			{
				this.ShowMessage_New(context, "请先登录会员！", true);
			}
			else
			{
				int num = 0;
				if (context.Request["RefundId"] != null)
				{
					int.TryParse(context.Request["refundId"], out num);
				}
				string text = "";
				text = context.Request["orderId"];
				RefundInfo refundInfo = null;
				if (num > 0)
				{
					refundInfo = TradeHelper.GetRefundInfo(num);
				}
				if (refundInfo == null && !string.IsNullOrEmpty(text))
				{
					refundInfo = TradeHelper.GetRefundInfo(text);
				}
				if (refundInfo == null)
				{
					this.ShowMessage_New(context, "参数不正确", true);
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							success = "true",
							Refund = new
							{
								RefundId = refundInfo.RefundId,
								OrderId = refundInfo.OrderId,
								RefundAmount = refundInfo.RefundAmount.F2ToString("f2"),
								RefundOrderId = refundInfo.RefundOrderId,
								RefundType = (int)refundInfo.RefundType,
								UserRemark = refundInfo.UserRemark,
								AdminRemark = refundInfo.AdminRemark
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetReturnInfo(HttpContext context)
		{
			if (!this.CheckUserRole())
			{
				this.ShowMessage_New(context, "请先登录会员！", true);
			}
			else
			{
				int num = 0;
				if (context.Request["ReturnsId"] != null)
				{
					int.TryParse(context.Request["ReturnsId"], out num);
				}
				string text = "";
				text = context.Request["orderId"];
				string text2 = context.Request["skuId"];
				ReturnInfo returnInfo = null;
				if (num > 0)
				{
					returnInfo = TradeHelper.GetReturnInfo(num);
				}
				if (returnInfo == null && !string.IsNullOrEmpty(text))
				{
					returnInfo = TradeHelper.GetReturnInfo(text, text2);
				}
				if (returnInfo == null)
				{
					this.ShowMessage_New(context, "参数不正确" + text + "-" + text2, true);
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							success = "true",
							Returns = new
							{
								ReturnId = returnInfo.ReturnId,
								OrderId = returnInfo.OrderId,
								SkuId = returnInfo.SkuId,
								ExpressCompanyAbb = returnInfo.ExpressCompanyAbb,
								ExpressCompanyName = returnInfo.ExpressCompanyName,
								AdminCellPhone = returnInfo.AdminCellPhone,
								AdminShipTo = returnInfo.AdminShipTo,
								AdminRemark = returnInfo.AdminRemark,
								AdminShipAddress = returnInfo.AdminShipAddress,
								ShipOrderNumber = returnInfo.ShipOrderNumber,
								UserRemark = returnInfo.UserRemark,
								RefundType = (int)returnInfo.RefundType,
								RefundAmount = returnInfo.RefundAmount.F2ToString("f2")
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}

		public void GetReplaceInfo(HttpContext context)
		{
			if (!this.CheckUserRole())
			{
				this.ShowMessage_New(context, "请先登录会员！", true);
			}
			else
			{
				int num = 0;
				if (context.Request["ReplaceId"] != null)
				{
					int.TryParse(context.Request["ReplaceId"], out num);
				}
				string text = "";
				text = context.Request["orderId"];
				string skuId = context.Request["skuId"];
				ReplaceInfo replaceInfo = null;
				if (num > 0)
				{
					replaceInfo = TradeHelper.GetReplaceInfo(num);
				}
				if (replaceInfo == null && !string.IsNullOrEmpty(text))
				{
					replaceInfo = TradeHelper.GetReplaceInfo(text, skuId);
				}
				if (replaceInfo == null)
				{
					this.ShowMessage_New(context, "参数不正确", true);
				}
				else
				{
					string s = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							success = "true",
							Replace = new
							{
								replaceInfo.ReplaceId,
								replaceInfo.OrderId,
								replaceInfo.SkuId,
								replaceInfo.ExpressCompanyAbb,
								replaceInfo.ExpressCompanyName,
								replaceInfo.AdminCellPhone,
								replaceInfo.AdminRemark,
								replaceInfo.AdminShipAddress,
								replaceInfo.AdminShipTo,
								replaceInfo.ShipOrderNumber,
								replaceInfo.UserRemark
							}
						}
					});
					context.Response.Write(s);
					context.Response.End();
				}
			}
		}
	}
}
