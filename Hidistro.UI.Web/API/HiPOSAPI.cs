using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Orders;
using Hishop.API.HiPOS.Entities.Request;
using HiShop.API.Setting.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class HiPOSAPI : IHttpHandler
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
			string text = context.Request["action"].ToLower().Trim();
			string text2 = text;
			switch (text2)
			{
			default:
				if (text2 == "setstorehipos")
				{
					this.SetStoreHiPOS(context);
				}
				break;
			case "auth":
				this.HiPOSSellerAuth(context);
				break;
			case "orderstatus":
				this.HiPOSOrderStatus(context);
				break;
			case "orderconfirm":
				this.HiPOSOrderConfirm(context);
				break;
			case "authqr":
				this.HiPOSAuthqr(context);
				break;
			case "authqrcheck":
				this.CheckHiPOSAuthqr(context);
				break;
			}
		}

		private void SetStoreHiPOS(HttpContext context)
		{
			int storeHiPOSId = context.Request["storeHiPOSId"].ToInt(0);
			if (StoresHelper.UpdateStoreHiPOS(storeHiPOSId))
			{
				context.Response.Write("{\"Status\":\"1\"}");
			}
			else
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
		}

		private void CheckHiPOSAuthqr(HttpContext context)
		{
			DataTable needActivation = StoresHelper.GetNeedActivation();
			if (needActivation.Rows.Count > 0)
			{
				string text = needActivation.Rows[0]["StoreHiPOSId"].ToNullString();
				string text2 = needActivation.Rows[0]["Alias"].ToNullString();
				int num = needActivation.Rows[0]["Status"].ToInt(0);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("{\"Status\":" + num + ",\"StoreHiPOSId\":" + text + ",\"Alias\":\"" + text2 + "\"}");
				context.Response.Write(stringBuilder.ToString());
			}
		}

		private void HiPOSAuthqr(HttpContext context)
		{
			string deviceId = context.Request["device_id"].ToNullString();
			string storeName = context.Request["store_name"].ToNullString();
			bool newFlag = context.Request["new"].ToBool();
			StoresInfo storesInfo = StoresHelper.GetStoreList(new StoresQuery
			{
				PageIndex = 1,
				PageSize = 2147483647,
				StoreName = storeName
			}).FirstOrDefault();
			if (storesInfo != null)
			{
				string storeHiPOSLastAlias = ManagerHelper.GetStoreHiPOSLastAlias();
				string alias = this.CreateAlias(storeHiPOSLastAlias);
				StoresHelper.UpdateStoreHiPOS(storesInfo.StoreId, deviceId, alias, newFlag);
			}
		}

		private string CreateAlias(string lastAlias)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "pos";
			if (string.IsNullOrEmpty(lastAlias))
			{
				stringBuilder.Append(text + "0001");
			}
			else
			{
				int num = lastAlias.Replace(text, string.Empty).ToInt(0);
				int num2 = num + 1;
				if (num.ToString().Length != num2.ToString().Length)
				{
					stringBuilder.Append(lastAlias.Replace("0" + num.ToString(), num2.ToString()));
				}
				else
				{
					stringBuilder.Append(lastAlias.Replace(num.ToString(), num2.ToString()));
				}
			}
			return stringBuilder.ToString();
		}

		private void HiPOSOrderStatus(HttpContext context)
		{
			string text = context.Request["code"].ToNullString();
			string text2 = context.Request["device_id"].ToNullString();
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				string takeCode = text.Replace(Globals.HIPOSTAKECODEPREFIX, string.Empty).Trim();
				OrderInfo orderInfo = OrderHelper.ValidateTakeCode(takeCode, text2);
				if (orderInfo != null)
				{
					OrderStatus orderStatus = orderInfo.OrderStatus;
					int num;
					if (!orderStatus.Equals(OrderStatus.WaitBuyerPay))
					{
						orderStatus = orderInfo.OrderStatus;
						num = (orderStatus.Equals(OrderStatus.BuyerAlreadyPaid) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					if (num != 0)
					{
						OrderInfoResult orderInfoResult = new OrderInfoResult();
						orderInfoResult.order_info_response = new OrderInfoResponse();
						orderInfoResult.order_info_response.amount = orderInfo.GetTotal(false).F2ToString("f2");
						orderInfoResult.order_info_response.id = orderInfo.OrderId;
						OrderInfoResponse order_info_response = orderInfoResult.order_info_response;
						orderStatus = orderInfo.OrderStatus;
						order_info_response.paid = orderStatus.Equals(OrderStatus.BuyerAlreadyPaid).ToString().ToLower();
						orderInfoResult.order_info_response.detail = ((orderInfo.HiPOSOrderDetails.Length > 30) ? (orderInfo.HiPOSOrderDetails.Substring(0, 30) + "等等") : orderInfo.HiPOSOrderDetails);
						stringBuilder.Append(JsonHelper.GetJson(orderInfoResult));
					}
					else
					{
						HiShopJsonResult obj = new HiShopJsonResult
						{
							error = new Error
							{
								code = 0,
								message = "提货码失效"
							}
						};
						stringBuilder.Append(JsonHelper.GetJson(obj));
					}
				}
				else
				{
					HiShopJsonResult obj2 = new HiShopJsonResult
					{
						error = new Error
						{
							code = 0,
							message = "订单不存在"
						}
					};
					stringBuilder.Append(JsonHelper.GetJson(obj2));
				}
			}
			context.Response.Write(stringBuilder.ToString());
		}

		private void HiPOSOrderConfirm(HttpContext context)
		{
			string text = context.Request["code"].ToNullString();
			string value = context.Request["sign"].ToNullString().ToLower();
			string text2 = context.Request["device_id"].ToNullString();
			string text3 = context.Request["method"].ToNullString();
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				string text4 = this.SetSHA1(context).ToLower();
				if (!text4.Equals(value))
				{
					HiShopJsonResult obj = new HiShopJsonResult
					{
						error = new Error
						{
							code = 0,
							message = "签名错误"
						}
					};
					stringBuilder.Append(JsonHelper.GetJson(obj));
				}
				else
				{
					string takeCode = text.Replace(Globals.HIPOSTAKECODEPREFIX, string.Empty).Trim();
					OrderInfo orderInfo = OrderHelper.ValidateTakeCode(takeCode, text2);
					if (orderInfo != null)
					{
						if (orderInfo.FightGroupId > 0 && orderInfo.FightGroupStatus != FightGroupStatus.FightGroupSuccess)
						{
							HiShopJsonResult obj2 = new HiShopJsonResult
							{
								error = new Error
								{
									code = 0,
									message = "该订单是火拼团订单，但是组团还没未成功,不能提货!"
								}
							};
							stringBuilder.Append(JsonHelper.GetJson(obj2));
						}
						else
						{
							orderInfo.HiPOSUseName = "HiPOS系统";
							if (OrderHelper.ConfirmTakeGoods(orderInfo, true))
							{
								ConfirmingResult confirmingResult = new ConfirmingResult();
								confirmingResult.confirming_response = new ConfirmingResponse();
								confirmingResult.confirming_response.result = "ok";
								stringBuilder.Append(JsonHelper.GetJson(confirmingResult));
								new OrderDao().UpdateOrderPaymentType(orderInfo.OrderId, "HiPOS支付");
								if (!string.IsNullOrEmpty(text3))
								{
									bool isStoreCollect = false;
									switch (text3.Trim().ToLower())
									{
									case "cash":
										isStoreCollect = true;
										break;
									case "weixin":
										isStoreCollect = false;
										break;
									case "alipay":
										isStoreCollect = false;
										break;
									}
									new OrderDao().UpdateOrderIsStoreCollect(orderInfo.OrderId, isStoreCollect);
								}
							}
							else
							{
								HiShopJsonResult obj3 = new HiShopJsonResult
								{
									error = new Error
									{
										code = 0,
										message = "确认提货失败"
									}
								};
								stringBuilder.Append(JsonHelper.GetJson(obj3));
							}
						}
					}
					else
					{
						HiShopJsonResult obj4 = new HiShopJsonResult
						{
							error = new Error
							{
								code = 0,
								message = "提货码失效"
							}
						};
						stringBuilder.Append(JsonHelper.GetJson(obj4));
					}
				}
			}
			else
			{
				HiShopJsonResult obj5 = new HiShopJsonResult
				{
					error = new Error
					{
						code = 0,
						message = "提货码失效"
					}
				};
				stringBuilder.Append(JsonHelper.GetJson(obj5));
			}
			context.Response.Write(stringBuilder.ToString());
		}

		private void HiPOSSellerAuth(HttpContext context)
		{
			string text = context.Request["merchant_id"].ToString();
			string text2 = context.Request["app_id"].ToString();
			string text3 = context.Request["app_secret"].ToString();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!masterSettings.HiPOSMerchantId.Equals(text))
			{
				masterSettings.HiPOSMerchantId = text;
			}
			if (!masterSettings.HiPOSAppId.Equals(text2))
			{
				masterSettings.HiPOSAppId = text2;
			}
			if (!masterSettings.HiPOSAppSecret.Equals(text3))
			{
				masterSettings.HiPOSAppSecret = text3;
			}
			SettingsManager.Save(masterSettings);
		}

		private string SetSHA1(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string empty = string.Empty;
			List<string> list = context.Request.Form.AllKeys.ToList();
			if (list.Contains("sign"))
			{
				list.Remove("sign");
			}
			string[] array = list.ToArray();
			Array.Sort(array);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Count(); i++)
			{
				string text = array[i];
				if (i == 0)
				{
					stringBuilder.AppendFormat("{0}={1}", text, context.Request.Form[text]);
				}
				else
				{
					stringBuilder.AppendFormat("&{0}={1}", text, context.Request.Form[text]);
				}
			}
			if (array.Count() == 0)
			{
				stringBuilder.AppendFormat("sign={0}", masterSettings.HiPOSAppSecret);
			}
			else
			{
				stringBuilder.AppendFormat("&sign={0}", masterSettings.HiPOSAppSecret);
			}
			return this.SHA1_Encrypt(stringBuilder.ToString());
		}

		public string SHA1_Encrypt(string Source_String)
		{
			byte[] bytes = Encoding.Default.GetBytes(Source_String);
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			bytes = sHA1CryptoServiceProvider.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = bytes;
			foreach (byte b in array)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}
	}
}
