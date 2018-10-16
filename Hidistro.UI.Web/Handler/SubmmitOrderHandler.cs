using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Web.ashxBase;
using Hishop.Components.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class SubmmitOrderHandler : BaseHandler
	{
		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public override void ProcessRequest(HttpContext context)
		{
			try
			{
				string text = context.Request["Action"];
				switch (text)
				{
				case "GetUserShippingAddress":
					this.GetUserShippingAddress(context);
					break;
				case "CalculateFreight":
					this.CalculateFreight(context);
					break;
				case "ProcessorPaymentMode":
					this.ProcessorPaymentMode(context);
					break;
				case "ProcessorUseCoupon":
					this.ProcessorUseCoupon(context);
					break;
				case "GetRegionId":
					this.GetUserRegionId(context);
					break;
				case "AddShippingAddress":
					this.AddUserShippingAddress(context);
					break;
				case "UpdateShippingAddress":
					this.UpdateShippingAddress(context);
					break;
				case "GetCanShipStores":
					this.GetCanShipStores(context);
					break;
				case "GetCountys":
					this.GetCountys(context);
					break;
				case "GetStores":
					this.GetStores(context);
					break;
				case "GetCanShipStoresForOrder":
					this.GetCanShipStoresForOrder(context);
					break;
				case "CheckStock":
					this.CheckStock(context);
					break;
				case "UploadNameVerify":
					this.UploadNameVerify(context);
					break;
				case "GetUserInvoiceList":
					this.GetUserInvoiceList(context);
					break;
				case "DelUserInvoice":
					this.DelUserInvoice(context);
					break;
				case "UpdateUserInvoice":
					this.UpdateUserInvoice(context);
					break;
				}
			}
			catch (Exception ex)
			{
				NameValueCollection param = new NameValueCollection
				{
					HttpContext.Current.Request.QueryString,
					HttpContext.Current.Request.Form
				};
				Globals.WriteExceptionLog_Page(ex, param, "SubmitOrderHandler");
			}
		}

		private void UpdateUserInvoice(HttpContext context)
		{
			int userId = HiContext.Current.UserId;
			ApiErrorCode apiErrorCode;
			if (userId <= 0)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.UserNoLogin;
				response.Write(base.GetErrorJson(apiErrorCode.GetHashCode(), "用户未登录"));
			}
			else
			{
				string text = context.Request["data"].ToNullString();
				UserInvoiceDataInfo userInvoiceDataInfo = new UserInvoiceDataInfo();
				if (string.IsNullOrEmpty(text))
				{
					HttpResponse response2 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response2.Write(base.GetErrorJson(apiErrorCode.GetHashCode(), "发票数据为空"));
				}
				else
				{
					userInvoiceDataInfo = JsonHelper.ParseFormJson<UserInvoiceDataInfo>(text);
					if (userInvoiceDataInfo == null)
					{
						HttpResponse response3 = context.Response;
						apiErrorCode = ApiErrorCode.Paramter_Error;
						response3.Write(base.GetErrorJson(apiErrorCode.GetHashCode(), "发票数据转换失败"));
					}
					else
					{
						userInvoiceDataInfo.LastUseTime = DateTime.Now;
						if (userInvoiceDataInfo.Id > 0)
						{
							UserInvoiceDataInfo userInvoiceDataInfo2 = MemberProcessor.GetUserInvoiceDataInfo(userInvoiceDataInfo.Id);
							if (userInvoiceDataInfo2 == null || userInvoiceDataInfo2.UserId != userId)
							{
								HttpResponse response4 = context.Response;
								apiErrorCode = ApiErrorCode.Paramter_Error;
								response4.Write(base.GetErrorJson(apiErrorCode.GetHashCode(), "错误的发票数据"));
								return;
							}
							if (userInvoiceDataInfo2.InvoiceTitle != userInvoiceDataInfo.InvoiceTitle && userInvoiceDataInfo.InvoiceType != InvoiceType.VATInvoice)
							{
								userInvoiceDataInfo.Id = 0;
							}
						}
						else
						{
							UserInvoiceDataInfo userInvoiceDataInfoByTitle = MemberProcessor.GetUserInvoiceDataInfoByTitle(userInvoiceDataInfo.InvoiceTitle);
							if (userInvoiceDataInfoByTitle != null)
							{
								userInvoiceDataInfo.Id = userInvoiceDataInfoByTitle.Id;
								if (userInvoiceDataInfo.InvoiceType == InvoiceType.Enterprise)
								{
									userInvoiceDataInfo.InvoiceType = userInvoiceDataInfoByTitle.InvoiceType;
								}
							}
						}
						string errorMsg = "";
						if (!this.ValidationInvoice(userInvoiceDataInfo, out errorMsg))
						{
							HttpResponse response5 = context.Response;
							apiErrorCode = ApiErrorCode.Paramter_Error;
							response5.Write(base.GetErrorJson(apiErrorCode.GetHashCode(), errorMsg));
						}
						else
						{
							userInvoiceDataInfo.UserId = userId;
							int newInvoiceId = userInvoiceDataInfo.Id;
							if (userInvoiceDataInfo.Id > 0)
							{
								MemberProcessor.UpdateUserInvoiceDataInfo(userInvoiceDataInfo);
							}
							else
							{
								newInvoiceId = MemberProcessor.AddUserInvoiceDataInfo(userInvoiceDataInfo).ToInt(0);
							}
							IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(userId);
							string s = JsonConvert.SerializeObject(new
							{
								Status = "OK",
								NewInvoiceId = newInvoiceId,
								List = from i in userInvoiceDataList
								select new
								{
									Id = i.Id,
									InvoiceType = i.InvoiceType,
									InvoiceTitle = i.InvoiceTitle.ToNullString(),
									InvoiceTaxpayerNumber = i.InvoiceTaxpayerNumber.ToNullString(),
									OpenBank = i.OpenBank.ToNullString(),
									BankAccount = i.BankAccount.ToNullString(),
									ReceiveAddress = i.ReceiveAddress.ToNullString(),
									ReceiveEmail = i.ReceiveEmail.ToNullString(),
									ReceiveName = i.ReceiveName.ToNullString(),
									ReceivePhone = i.ReceivePhone.ToNullString(),
									ReceiveRegionId = i.ReceiveRegionId,
									ReceiveRegionName = i.ReceiveRegionName.ToNullString(),
									RegisterAddress = i.RegisterAddress.ToNullString(),
									RegisterTel = i.RegisterTel.ToNullString()
								}
							});
							context.Response.Write(s);
						}
					}
				}
			}
		}

		private bool ValidationInvoice(UserInvoiceDataInfo userInvoiceInfo, out string msg)
		{
			msg = "";
			ValidationResults validationResults = Validation.Validate(userInvoiceInfo, "ValInvoice");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += item.Message;
				}
				msg = text;
			}
			bool flag = true;
			if ((userInvoiceInfo.InvoiceType == InvoiceType.Enterprise_Electronic || userInvoiceInfo.InvoiceType == InvoiceType.Personal_Electronic) && (string.IsNullOrEmpty(userInvoiceInfo.ReceiveEmail) || string.IsNullOrEmpty(userInvoiceInfo.ReceivePhone) || !DataHelper.IsEmail(userInvoiceInfo.ReceiveEmail) || !DataHelper.IsMobile(userInvoiceInfo.ReceivePhone)))
			{
				msg += "请输入正确的收票人邮箱和电话";
				flag = false;
			}
			if ((userInvoiceInfo.InvoiceType == InvoiceType.Enterprise_Electronic || userInvoiceInfo.InvoiceType == InvoiceType.Enterprise) && (string.IsNullOrEmpty(userInvoiceInfo.InvoiceTaxpayerNumber) || string.IsNullOrEmpty(userInvoiceInfo.InvoiceTitle)))
			{
				msg += "请输入发票抬头和纳税人识别号";
				flag = false;
			}
			if (userInvoiceInfo.InvoiceType == InvoiceType.VATInvoice && (!DataHelper.IsMobile(userInvoiceInfo.ReceivePhone) || string.IsNullOrEmpty(userInvoiceInfo.BankAccount) || string.IsNullOrEmpty(userInvoiceInfo.OpenBank) || string.IsNullOrEmpty(userInvoiceInfo.ReceiveAddress) || string.IsNullOrEmpty(userInvoiceInfo.ReceiveName) || string.IsNullOrEmpty(userInvoiceInfo.ReceiveRegionName) || string.IsNullOrEmpty(userInvoiceInfo.RegisterAddress) || string.IsNullOrEmpty(userInvoiceInfo.RegisterTel) || string.IsNullOrEmpty(userInvoiceInfo.InvoiceTaxpayerNumber) || string.IsNullOrEmpty(userInvoiceInfo.InvoiceTitle) || string.IsNullOrEmpty(userInvoiceInfo.ReceivePhone)))
			{
				msg += "请输入正确的专票信息";
				flag = false;
			}
			return validationResults.IsValid & flag;
		}

		private void DelUserInvoice(HttpContext context)
		{
			int userId = HiContext.Current.UserId;
			int id = context.Request["invoiceId"].ToInt(0);
			ApiErrorCode apiErrorCode;
			if (userId <= 0)
			{
				HttpResponse response = context.Response;
				apiErrorCode = ApiErrorCode.UserNoLogin;
				response.Write(base.GetErrorJson(apiErrorCode.GetHashCode(), "用户未登录"));
			}
			else
			{
				UserInvoiceDataInfo userInvoiceDataInfo = MemberProcessor.GetUserInvoiceDataInfo(id);
				if (userInvoiceDataInfo == null || userInvoiceDataInfo.UserId != userId)
				{
					HttpResponse response2 = context.Response;
					apiErrorCode = ApiErrorCode.Paramter_Error;
					response2.Write(base.GetErrorJson(apiErrorCode.GetHashCode(), "错误的发票ID"));
				}
				else
				{
					MemberProcessor.DeleteUserInvoiceDataInfo(userId);
					IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(userId);
					string s = JsonConvert.SerializeObject(new
					{
						Status = "OK",
						List = from i in userInvoiceDataList
						select new
						{
							Id = i.Id,
							InvoiceType = i.InvoiceType,
							InvoiceTitle = i.InvoiceTitle.ToNullString(),
							InvoiceTaxpayerNumber = i.InvoiceTaxpayerNumber.ToNullString(),
							OpenBank = i.OpenBank.ToNullString(),
							BankAccount = i.BankAccount.ToNullString(),
							ReceiveAddress = i.ReceiveAddress.ToNullString(),
							ReceiveEmail = i.ReceiveEmail.ToNullString(),
							ReceiveName = i.ReceiveName.ToNullString(),
							ReceivePhone = i.ReceivePhone.ToNullString(),
							ReceiveRegionId = i.ReceiveRegionId,
							ReceiveRegionName = i.ReceiveRegionName.ToNullString(),
							RegisterAddress = i.RegisterAddress.ToNullString(),
							RegisterTel = i.RegisterTel.ToNullString()
						}
					});
					context.Response.Write(s);
				}
			}
		}

		private void GetUserInvoiceList(HttpContext context)
		{
			int userId = HiContext.Current.UserId;
			if (userId <= 0)
			{
				context.Response.Write(base.GetErrorJson(214.GetHashCode(), "错误的用户信息"));
			}
			else
			{
				IList<UserInvoiceDataInfo> userInvoiceDataList = MemberProcessor.GetUserInvoiceDataList(userId);
				string s = JsonConvert.SerializeObject(new
				{
					Status = "OK",
					List = from i in userInvoiceDataList
					select new
					{
						Id = i.Id,
						InvoiceType = i.InvoiceType,
						InvoiceTitle = i.InvoiceTitle.ToNullString(),
						InvoiceTaxpayerNumber = i.InvoiceTaxpayerNumber.ToNullString(),
						OpenBank = i.OpenBank.ToNullString(),
						BankAccount = i.BankAccount.ToNullString(),
						ReceiveAddress = i.ReceiveAddress.ToNullString(),
						ReceiveEmail = i.ReceiveEmail.ToNullString(),
						ReceiveName = i.ReceiveName.ToNullString(),
						ReceivePhone = i.ReceivePhone.ToNullString(),
						ReceiveRegionId = i.ReceiveRegionId,
						ReceiveRegionName = i.ReceiveRegionName.ToNullString(),
						RegisterAddress = i.RegisterAddress.ToNullString(),
						RegisterTel = i.RegisterTel.ToNullString()
					}
				});
				context.Response.Write(s);
			}
		}

		private void CheckStock(HttpContext context)
		{
			string from = context.Request["from"].ToNullString();
			string productSku = Globals.UrlDecode(context.Request["productSku"].ToNullString());
			int buyAmount = context.Request["buyAmount"].ToInt(0);
			int num = context.Request["bundlingid"].ToInt(0);
			int combinationId = context.Request["CombinationId"].ToInt(0);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart(from, productSku, buyAmount, combinationId, false, -1, 0);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			if (shoppingCart != null)
			{
				string str = "";
				if (!TradeHelper.CheckShoppingStock(shoppingCart, out str, 0))
				{
					stringBuilder.Append("\"Status\":\"ERROR\",\"Message\":\"有商品 " + str + " 库存不足\"}");
				}
				else
				{
					stringBuilder.Append("\"Status\":\"SUCCESS\"}");
				}
			}
			else
			{
				stringBuilder.Append("\"Status\":\"ERROR\",\"Message\":\"订单中有商品已删除或已下架，请重新选择商品\"}");
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder);
		}

		private void GetUserRegionId(HttpContext context)
		{
			string text = context.Request["Prov"];
			string text2 = context.Request["City"];
			string text3 = context.Request["Areas"];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				stringBuilder.Append("\"Status\":\"OK\",\"RegionId\":\"" + RegionHelper.GetRegionId(text3, text2, text) + "\"}");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"NO\"}");
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder);
		}

		private void GetCanShipStores(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!masterSettings.OpenMultStore)
			{
				context.Response.ContentType = "text/plain";
				context.Response.Write("[]");
			}
			else
			{
				int regionId = context.Request["regionId"].ToInt(0);
				string text = Globals.UrlDecode(context.Request["productSku"].ToNullString().Trim());
				int num = context.Request["buyAmount"].ToNullString().Trim().ToInt(0);
				ShoppingCartInfo shoppingCart = new ShoppingCartInfo();
				if (!string.IsNullOrEmpty(text))
				{
					if (text.Split(',').Length == 1 && num > 0)
					{
						ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
						shoppingCartItemInfo.SkuId = text;
						shoppingCartItemInfo.Quantity = num;
						shoppingCart.LineItems.Add(shoppingCartItemInfo);
					}
					else
					{
						shoppingCart = ShoppingCartProcessor.GetShoppingCart(text, true, false, -1);
						if (shoppingCart == null || shoppingCart.LineItems.Count == 0)
						{
							context.Response.Write("{\"status\":\"false\",\"msg\":\"订单内无商品信息\"}");
							return;
						}
					}
				}
				IList<StoresInfo> canShipStores = DepotHelper.GetCanShipStores(regionId, false);
				StringBuilder strData = new StringBuilder();
				if (canShipStores.Count == 0)
				{
					strData.Append("[]");
				}
				else
				{
					strData.Append("[");
					canShipStores.ForEach(delegate(StoresInfo x)
					{
						bool flag = true;
						if (shoppingCart != null && shoppingCart.LineItems.Count > 0)
						{
							foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
							{
								if (!StoresHelper.StoreHasProductSku(x.StoreId, lineItem.SkuId))
								{
									flag = false;
									break;
								}
								if (StoresHelper.GetStoreStock(x.StoreId, lineItem.SkuId) < lineItem.Quantity)
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							strData.Append("{");
							strData.AppendFormat("\"StoreId\":{0},", x.StoreId);
							string fullRegion = RegionHelper.GetFullRegion(x.RegionId, "&nbsp;&nbsp;", true, 0);
							strData.AppendFormat("\"StoreInfo\":\"{0}\"", x.StoreName + "&nbsp;&nbsp;&nbsp;&nbsp;" + fullRegion + "&nbsp;&nbsp;" + x.Address + "&nbsp;&nbsp;&nbsp;&nbsp;电话：" + x.Tel);
							strData.Append("},");
						}
					});
					if (strData.ToNullString() != "[")
					{
						strData.Remove(strData.Length - 1, 1);
					}
					strData.Append("]");
				}
				context.Response.ContentType = "text/plain";
				context.Response.Write(strData);
			}
		}

		private void GetUserShippingAddress(HttpContext context)
		{
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(int.Parse(context.Request["ShippingId"], NumberStyles.None));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			if (shippingAddress.UserId == HiContext.Current.UserId)
			{
				if (shippingAddress != null)
				{
					stringBuilder.Append("\"Status\":\"OK\",");
					stringBuilder.AppendFormat("\"ShipTo\":\"{0}\",", Globals.HtmlDecode(shippingAddress.ShipTo));
					stringBuilder.AppendFormat("\"Address\":\"{0}\",", Globals.HtmlDecode(shippingAddress.RegionLocation + shippingAddress.Address));
					stringBuilder.AppendFormat("\"Zipcode\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Zipcode));
					stringBuilder.AppendFormat("\"CellPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.CellPhone));
					stringBuilder.AppendFormat("\"TelPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.TelPhone));
					stringBuilder.AppendFormat("\"RegionId\":\"{0}\",", shippingAddress.RegionId);
					string arg = "0";
					string arg2 = string.Empty;
					if (HiContext.Current.SiteSettings.IsOpenCertification)
					{
						if (HiContext.Current.SiteSettings.CertificationModel == 1 && string.IsNullOrWhiteSpace(shippingAddress.IDNumber))
						{
							arg = "1";
						}
						if (HiContext.Current.SiteSettings.CertificationModel == 2 && (string.IsNullOrWhiteSpace(shippingAddress.IDNumber) || string.IsNullOrWhiteSpace(shippingAddress.IDImage1) || string.IsNullOrWhiteSpace(shippingAddress.IDImage2)))
						{
							arg = "1";
							if (!string.IsNullOrWhiteSpace(shippingAddress.IDNumber))
							{
								arg2 = HiCryptographer.Decrypt(shippingAddress.IDNumber);
							}
						}
					}
					stringBuilder.AppendFormat("\"IsGetIDInfo\":\"{0}\",", arg);
					stringBuilder.AppendFormat("\"CertificationModel\":\"{0}\",", HiContext.Current.SiteSettings.CertificationModel);
					stringBuilder.AppendFormat("\"IDNumber\":\"{0}\"", arg2);
				}
				else
				{
					stringBuilder.Append("\"Status\":\"0\"");
				}
			}
			else
			{
				stringBuilder.Append("\"Status\":\"0\"");
			}
			stringBuilder.Append("}");
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder);
		}

		private void CalculateFreight(HttpContext context)
		{
			decimal money = default(decimal);
			string from = (context.Request["from"] == null) ? "" : context.Request["from"].ToString().ToLower();
			IDictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
			if (!string.IsNullOrEmpty(context.Request["RegionId"]))
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				int num = context.Request["ModeId"].ToInt(0);
				decimal num2 = context.Request["Weight"].ToDecimal(0);
				int regionId = context.Request["RegionId"].ToInt(0);
				ShoppingCartInfo shoppingCartInfo = null;
				int buyAmount = default(int);
				int.TryParse(context.Request["buyAmount"], out buyAmount);
				int num3 = default(int);
				int.TryParse(context.Request["Bundlingid"], out num3);
				int combinationId = context.Request["CombinationId"].ToInt(0);
				string text = Globals.UrlDecode(context.Request["productSku"].ToNullString());
				bool flag = false;
				if (!string.IsNullOrWhiteSpace(text) && text.Split(',').Length > 1)
				{
					flag = true;
				}
				int.TryParse(context.Request["buyAmount"], out buyAmount);
				string productSku = Globals.UrlDecode(context.Request["productSku"].ToNullString());
				shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart(from, productSku, buyAmount, combinationId, false, -1, 0);
				if (shoppingCartInfo != null && ((shoppingCartInfo.LineItems != null && shoppingCartInfo.LineItems.Count > 0) || (shoppingCartInfo.LineGifts != null && shoppingCartInfo.LineGifts.Count > 0)))
				{
					if (masterSettings.OpenMultStore)
					{
						if (num == -2)
						{
							money = default(decimal);
						}
						else if (!shoppingCartInfo.IsFreightFree)
						{
							dictionary = ShoppingProcessor.CalcFreight(regionId, shoppingCartInfo, out money);
						}
					}
					else if (!shoppingCartInfo.IsFreightFree)
					{
						dictionary = ShoppingProcessor.CalcFreight(regionId, shoppingCartInfo, out money);
					}
					else
					{
						money = default(decimal);
					}
				}
				else
				{
					money = default(decimal);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.AppendFormat("\"Price\":\"{0}\",", Globals.FormatMoney(money));
			stringBuilder.Append("\"SupplierPrice\":[");
			StringBuilder stringBuilder2 = new StringBuilder();
			if (dictionary.Count > 0)
			{
				foreach (KeyValuePair<int, decimal> item in dictionary)
				{
					stringBuilder2.Append("{\"SupplierId\":" + item.Key + ",\"FreightPrice\":" + item.Value.F2ToString("f2") + "},");
				}
				stringBuilder.Append(stringBuilder2.ToString().TrimEnd(','));
			}
			stringBuilder.Append("]");
			stringBuilder.Append("}");
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder.ToString());
		}

		private void ProcessorPaymentMode(HttpContext context)
		{
			decimal money = default(decimal);
			if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]))
			{
				int modeId = int.Parse(context.Request["ModeId"], NumberStyles.None);
				decimal num = decimal.Parse(context.Request["CartTotalPrice"]);
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
				money = default(decimal);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.AppendFormat("\"Charge\":\"{0}\"", Globals.FormatMoney(money));
			stringBuilder.Append("}");
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder.ToString());
		}

		private void ProcessorUseCoupon(HttpContext context)
		{
			decimal orderAmount = decimal.Parse(context.Request["CartTotal"]);
			string claimCode = context.Request["CouponCode"];
			CouponItemInfo userCouponInfo = ShoppingProcessor.GetUserCouponInfo(orderAmount, claimCode);
			StringBuilder stringBuilder = new StringBuilder();
			if (userCouponInfo != null)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.AppendFormat("\"CouponName\":\"{0}\",", userCouponInfo.CouponName.Replace("\n", "").Replace("\t", "").Replace("\r", ""));
				stringBuilder.AppendFormat("\"DiscountValue\":\"{0}\",", Globals.FormatMoney(userCouponInfo.Price.Value));
				stringBuilder.AppendFormat("\"Amount\":\"{0}\"", Globals.FormatMoney(userCouponInfo.OrderUseLimit.Value));
				stringBuilder.Append("}");
			}
			else
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"ERROR\"");
				stringBuilder.Append("}");
			}
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder);
		}

		private void AddUserShippingAddress(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string str = "";
			if (this.ValShippingAddress(context, ref str))
			{
				ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo(context, 0);
				int num = MemberProcessor.AddShippingAddress(shippingAddressInfo);
				if (num > 0)
				{
					shippingAddressInfo.ShippingId = num;
					context.Response.Write("{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"" + shippingAddressInfo.ShipTo + "\",\"RegionId\":\"" + RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " ", true, 0) + "\",\"ShippingAddress\":\"" + shippingAddressInfo.Address + "\",\"ShippingId\":\"" + shippingAddressInfo.ShippingId + "\",\"CellPhone\":\"" + shippingAddressInfo.CellPhone + "\",\"Id\":\"" + shippingAddressInfo.RegionId + "\"}}");
				}
				else
				{
					context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经在，请重新输入一次再试\"}");
				}
			}
			else
			{
				context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
			}
		}

		private void UpdateShippingAddress(HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string str = "";
			str = "请选择要修改的收货地址";
			if (this.ValShippingAddress(context, ref str) || string.IsNullOrEmpty(context.Request.Params["ShippingId"]) || Convert.ToInt32(context.Request.Params["ShippingId"]) > 0)
			{
				ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo(context, Convert.ToInt32(context.Request.Params["ShippingId"]));
				if (shippingAddressInfo != null && shippingAddressInfo.ShippingId > 0)
				{
					shippingAddressInfo.ShippingId = Convert.ToInt32(context.Request.Params["ShippingId"]);
					if (MemberProcessor.UpdateShippingAddress(shippingAddressInfo))
					{
						context.Response.Write("{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"" + shippingAddressInfo.ShipTo + "\",\"RegionId\":\"" + RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " ", true, 0) + "\",\"ShippingAddress\":\"" + shippingAddressInfo.Address + "\",\"ShippingId\":\"" + shippingAddressInfo.ShippingId + "\",\"CellPhone\":\"" + shippingAddressInfo.CellPhone + "\"}}");
					}
					else
					{
						context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经存在，请重新输入一次再试\"}");
					}
				}
				else
				{
					context.Response.Write("{\"Status\":\"Error\",\"Result\":\"该收获地址获取错误,请重新填写\"}");
				}
			}
			else
			{
				context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
			}
		}

		private bool ValShippingAddress(HttpContext context, ref string erromsg)
		{
			Regex regex = new Regex("[\\u4e00-\\u9fa5a-zA-Z ]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*");
			string text = context.Request.Params["ShippingTo"].ToNullString().Trim();
			string text2 = context.Request.Params["AddressDetails"].ToNullString().Trim();
			int num = context.Request.Params["RegionId"].ToInt(0);
			if (string.IsNullOrEmpty(text) || !regex.IsMatch(text))
			{
				erromsg = "收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间";
				return false;
			}
			if (string.IsNullOrEmpty(text2))
			{
				erromsg = "详细地址不能为空";
				return false;
			}
			if (text2.Length < 3 || text2.Trim().Length > 60)
			{
				erromsg = "详细地址长度在3-60个字符之间";
				return false;
			}
			if (num <= 0)
			{
				erromsg = "请选择收货地址";
				return false;
			}
			string text3 = context.Request.Params["TelPhone"].ToNullString().Trim();
			string text4 = context.Request.Params["CellHphone"].ToNullString().Trim();
			if (string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text4))
			{
				erromsg = "电话号码和手机二者必填其一";
				return false;
			}
			if (!string.IsNullOrEmpty(text3) && (text3.Length < 3 || text3.Length > 20))
			{
				erromsg = "电话号码长度限制在3-20个字符之间";
				return false;
			}
			if (!string.IsNullOrEmpty(text4) && (text4.Length < 3 || text4.Length > 20))
			{
				erromsg = "手机号码长度限制在3-20个字符之间";
				return false;
			}
			if (!string.IsNullOrEmpty(text4) && !DataHelper.IsMobile(text4))
			{
				erromsg = "请输入正确的手机号码";
				return false;
			}
			if (MemberProcessor.GetShippingAddressCount(HiContext.Current.UserId) >= HiContext.Current.SiteSettings.UserAddressMaxCount)
			{
				erromsg = $"最多只能添加{HiContext.Current.SiteSettings.UserAddressMaxCount}个收货地址";
				return false;
			}
			return true;
		}

		private ShippingAddressInfo GetShippingAddressInfo(HttpContext context, int shippingId = 0)
		{
			ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
			if (shippingId > 0)
			{
				shippingAddressInfo = MemberProcessor.GetShippingAddress(shippingId);
				if (shippingAddressInfo == null)
				{
					return null;
				}
			}
			shippingAddressInfo.UserId = HiContext.Current.UserId;
			shippingAddressInfo.ShipTo = Globals.StripAllTags(context.Request.Params["ShippingTo"].Trim());
			shippingAddressInfo.RegionId = context.Request.Params["RegionId"].ToInt(0);
			shippingAddressInfo.Address = Globals.StripAllTags(context.Request.Params["AddressDetails"].Trim());
			shippingAddressInfo.Zipcode = Globals.StripAllTags(context.Request.Params["ZipCode"].ToNullString().Trim());
			shippingAddressInfo.CellPhone = Globals.StripAllTags(context.Request.Params["CellHphone"].Trim());
			shippingAddressInfo.TelPhone = Globals.StripAllTags(context.Request.Params["TelPhone"].Trim());
			shippingAddressInfo.FullRegionPath = RegionHelper.GetFullPath(shippingAddressInfo.RegionId, true);
			shippingAddressInfo.IsDefault = context.Request.Params["IsDefault"].ToBool();
			shippingAddressInfo.BuildingNumber = Globals.StripAllTags(context.Request.Params["BuilderNumber"].ToNullString());
			shippingAddressInfo.RegionLocation = "";
			return shippingAddressInfo;
		}

		private void GetCanShipStoresForOrder(HttpContext context)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!masterSettings.OpenMultStore)
			{
				context.Response.ContentType = "text/plain";
				context.Response.Write("[]");
			}
			else
			{
				int regionId = context.Request["regionId"].ToInt(0);
				string text = Globals.UrlDecode(context.Request["productSku"].ToNullString().Trim());
				int num = context.Request["buyAmount"].ToNullString().Trim().ToInt(0);
				int num2 = context.Request["CombinationId"].ToInt(0);
				ShoppingCartInfo shoppingCart = new ShoppingCartInfo();
				if (text.Split(',').Length == 1 && num > 0)
				{
					ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = text;
					shoppingCartItemInfo.Quantity = num;
					shoppingCart.LineItems.Add(shoppingCartItemInfo);
				}
				else if (!string.IsNullOrEmpty(text))
				{
					if (num2 > 0)
					{
						shoppingCart = ShoppingCartProcessor.GetCombinationShoppingCart(num2, text, num, true);
					}
					else
					{
						shoppingCart = ShoppingCartProcessor.GetShoppingCart(text, true, false, -1);
					}
				}
				else if (HiContext.Current.User != null)
				{
					HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["ckids"];
					if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
					{
						text = Globals.UrlDecode(httpCookie.Value);
					}
					shoppingCart = ShoppingCartProcessor.GetShoppingCart("", text, num, 0, false, -1, 0);
				}
				if (shoppingCart == null || shoppingCart.LineItems.Count == 0)
				{
					context.Response.Write("[]");
				}
				else
				{
					IList<StoresInfo> canShipStores = DepotHelper.GetCanShipStores(regionId, false);
					StringBuilder strData = new StringBuilder();
					if (canShipStores.Count == 0)
					{
						strData.Append("[]");
					}
					else
					{
						strData.Append("[");
						canShipStores.ForEach(delegate(StoresInfo x)
						{
							bool flag = true;
							if (shoppingCart != null && shoppingCart.LineItems.Count > 0)
							{
								foreach (ShoppingCartItemInfo lineItem in shoppingCart.LineItems)
								{
									if (!StoresHelper.StoreHasProductSku(x.StoreId, lineItem.SkuId))
									{
										flag = false;
										break;
									}
									if (StoresHelper.GetStoreStock(x.StoreId, lineItem.SkuId) < lineItem.Quantity)
									{
										flag = false;
										break;
									}
								}
							}
							if (flag)
							{
								strData.Append("{");
								strData.AppendFormat("\"StoreId\":{0},", x.StoreId);
								string fullRegion = RegionHelper.GetFullRegion(x.RegionId, "&nbsp;&nbsp;", true, 0);
								strData.AppendFormat("\"StoreInfo\":\"{0}\"", x.StoreName + "&nbsp;&nbsp;&nbsp;&nbsp;" + fullRegion + "&nbsp;&nbsp;" + x.Address + "&nbsp;&nbsp;&nbsp;&nbsp;电话：" + x.Tel);
								strData.Append("},");
							}
						});
						if (strData.ToNullString() != "[")
						{
							strData.Remove(strData.Length - 1, 1);
						}
						strData.Append("]");
					}
					context.Response.ContentType = "text/plain";
					context.Response.Write(strData);
				}
			}
		}

		private void GetCountys(HttpContext context)
		{
			int num = context.Request["regionid"].ToInt(0);
			if (num == 0)
			{
				context.Response.Write("{\"status\":\"false\",\"msg\":\"请选择收货地址\"}");
			}
			else
			{
				int cityId = RegionHelper.GetCityId(num);
				Dictionary<int, string> countys = RegionHelper.GetCountys(cityId, true);
				string format = "{{\"status\":\"true\",\"Data\":[{0}]}}";
				string text = "";
				foreach (KeyValuePair<int, string> item in countys)
				{
					text = text + "{\"id\":\"" + item.Key + "\",\"name\":\"" + item.Value + "\"},";
				}
				text = text.TrimEnd(',');
				context.Response.Write(string.Format(format, text));
			}
		}

		private void GetStores(HttpContext context)
		{
			int num = context.Request["regionid"].ToInt(0);
			string text = Globals.UrlDecode(context.Request["productSku"].ToNullString().Trim());
			int num2 = context.Request["buyAmount"].ToInt(0);
			int num3 = context.Request["CombinationId"].ToInt(0);
			if (num == 0)
			{
				context.Response.Write("{\"status\":\"false\",\"msg\":\"请选择收货地址\"}");
			}
			else
			{
				ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
				if (text.Split(',').Length == 1 && num2 > 0)
				{
					shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart("signBuy", text, num2, 0, false, -1, 0);
				}
				else if (!string.IsNullOrEmpty(text))
				{
					shoppingCartInfo = ((num3 <= 0) ? ShoppingCartProcessor.GetShoppingCart(text, true, false, -1) : ShoppingCartProcessor.GetCombinationShoppingCart(num3, text, num2, true));
				}
				else if (HiContext.Current.User != null)
				{
					HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["ckids"];
					if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
					{
						text = Globals.UrlDecode(httpCookie.Value);
					}
					shoppingCartInfo = ShoppingCartProcessor.GetShoppingCart("", text, num2, 0, false, -1, 0);
				}
				if (shoppingCartInfo == null || shoppingCartInfo.LineItems.Count == 0)
				{
					context.Response.Write("{\"status\":\"false\",\"msg\":\"订单内无商品信息，也许是登录已失效\"}");
				}
				else
				{
					int cityId = RegionHelper.GetCityId(num);
					int countyId = RegionHelper.GetCountyId(num);
					int num4 = -1;
					if (cityId != num && countyId != num)
					{
						num4 = num;
					}
					string fullRegion = RegionHelper.GetFullRegion(num, ",", true, 0);
					string value = string.Join(",", fullRegion.Split(',').Take(2));
					IList<StoresInfo> allStores = StoresHelper.GetAllStores();
					string empty = string.Empty;
					string format = "{{\"CountyId\":\"{0}\",\"StreetId\":\"{1}\",\"Name\":\"{2}\",\"Addr\":\"{3}\",\"Tel\":\"{4}\",\"Error\":\"{5}\",\"storeId\":\"{6}\",\"IsOnlinePay\":{7},\"IsOfflinePay\":{8}}},";
					int num5 = 0;
					int num6 = 0;
					string text2 = "";
					string text3 = "";
					List<string> list = new List<string>();
					list.AddRange(new string[5]
					{
						"",
						"",
						"",
						"",
						""
					});
					bool flag = false;
					foreach (StoresInfo item in allStores)
					{
						if (item.IsAboveSelf)
						{
							int num7 = item.RegionId.ToInt(0);
							string fullRegion2 = RegionHelper.GetFullRegion(num7, ",", true, 0);
							if (fullRegion2.Contains(value))
							{
								fullRegion2 += item.Address;
								int countyId2 = RegionHelper.GetCountyId(num7);
								int storeId = item.StoreId;
								string storeName = item.StoreName;
								string tel = item.Tel;
								string text4 = string.Empty;
								num5 = 0;
								num6 = 0;
								text2 = "";
								text3 = "";
								foreach (ShoppingCartItemInfo lineItem in shoppingCartInfo.LineItems)
								{
									if (!StoresHelper.StoreHasProductSku(storeId, lineItem.SkuId))
									{
										num5++;
										text2 = text2 + lineItem.Name.ToNullString() + " " + lineItem.SkuContent.ToNullString() + "&#10;";
									}
									else if (StoresHelper.GetStoreStock(storeId, lineItem.SkuId) < lineItem.Quantity)
									{
										num6++;
										text3 = text3 + lineItem.Name.ToNullString() + " " + lineItem.SkuContent.ToNullString() + "&#10;";
									}
								}
								empty = string.Empty;
								if (!string.IsNullOrEmpty(text2))
								{
									text4 = text4 + "该门店不支持商品 <a href='javascript:void(0);' title='" + text2 + "'>" + ((text2.Length > 8) ? (text2.Substring(0, 8) + "...") : text2) + "</a> 自提";
								}
								if (!string.IsNullOrEmpty(text3))
								{
									text4 = text4 + "<a href='javascript:void(0);'  title='" + text3 + "'>" + ((text3.Length > 8) ? (text3.Substring(0, 8) + "...") : text3) + "</a> 缺货";
								}
								if (string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3))
								{
									flag = true;
								}
								storeName = (string.IsNullOrEmpty(item.StoreOpenTime) ? storeName : (storeName + " [营业时间:" + item.StoreOpenTime + "]"));
								empty = string.Format(format, countyId2.ToNullString(), num7.ToNullString(), storeName.ToNullString(), fullRegion2.ToNullString(), tel.ToNullString(), text4.ToNullString(), storeId.ToNullString(), item.IsOnlinePay.ToNullString().ToLower(), item.IsOfflinePay.ToNullString().ToLower()).Replace("\r\n", "");
								if (num4 == num7)
								{
									if (string.IsNullOrEmpty(text4))
									{
										list.Insert(0, empty);
									}
									else
									{
										list.Insert(1, empty);
									}
								}
								else if (countyId == countyId2 && countyId2 != 0)
								{
									if (string.IsNullOrEmpty(text4))
									{
										list.Insert(2, empty);
									}
									else
									{
										list.Insert(3, empty);
									}
								}
								else if (string.IsNullOrEmpty(text4))
								{
									list.Insert(4, empty);
								}
								else
								{
									list.Add(empty);
								}
							}
						}
					}
					if (flag)
					{
						string format2 = "{{\"status\":\"true\",\"Data\":[{0}]}}";
						string text5 = "";
						foreach (string item2 in list)
						{
							if (!string.IsNullOrEmpty(item2))
							{
								text5 += item2;
							}
						}
						text5 = text5.TrimEnd(',');
						context.Response.Write(string.Format(format2, text5));
					}
					else
					{
						context.Response.Write("{\"status\":\"false\",\"msg\":\"收货地址所在市没有可自提的门店\"}");
					}
				}
			}
		}

		private void UploadNameVerify(HttpContext context)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				if (this.ValIDCard(context, ref empty2))
				{
					int num = context.Request["ShippingId"].ToInt(0);
					if (num > 0)
					{
						empty = ((!this.UpdateShippingAddressInfo(context, num, out empty2)) ? ("{\"Status\":\"Error\",\"Result\":\"" + empty2 + "\"}") : "{\"Status\":\"OK\",\"Result\":\"\"}");
					}
					else if (!string.IsNullOrWhiteSpace(context.Request["OrderId"]))
					{
						OrderInfo orderInfo = TradeHelper.GetOrderInfo(context.Request["OrderId"]);
						if (orderInfo != null)
						{
							orderInfo.IDNumber = HiCryptographer.Encrypt(context.Request["IDNumber"].Trim());
							if (HiContext.Current.SiteSettings.CertificationModel == 2)
							{
								string imageServerUrl = Globals.GetImageServerUrl();
								orderInfo.IDImage1 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage1"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage1"].Trim());
								orderInfo.IDImage2 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage2"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage2"].Trim());
							}
							empty = ((!TradeHelper.UpdateOrderInfo(orderInfo)) ? "{\"Status\":\"Error\",\"Result\":\"保存证件信息失败\"}" : ((!this.UpdateShippingAddressInfo(context, orderInfo.ShippingId, out empty2)) ? ("{\"Status\":\"Error\",\"Result\":\"更新订单证件信息成功," + empty2 + "\"}") : "{\"Status\":\"OK\",\"Result\":\"\"}"));
						}
						else
						{
							empty = "{\"Status\":\"Error\",\"Result\":\"获取模型错误\"}";
						}
					}
					else
					{
						empty = "{\"Status\":\"Error\",\"Result\":\"请求的地址错误\"}";
					}
				}
				else
				{
					empty = "{\"Status\":\"Error\",\"Result\":\"" + empty2 + "\"}";
				}
			}
			catch
			{
				empty = "{\"Status\":\"Error\",\"Result\":\"系统异常\"}";
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(empty);
		}

		private bool UpdateShippingAddressInfo(HttpContext context, int shippingId, out string msg)
		{
			bool result = false;
			msg = string.Empty;
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(shippingId);
			if (shippingAddress != null)
			{
				shippingAddress.IDNumber = HiCryptographer.Encrypt(context.Request["IDNumber"].Trim());
				if (HiContext.Current.SiteSettings.CertificationModel == 2)
				{
					string imageServerUrl = Globals.GetImageServerUrl();
					shippingAddress.IDImage1 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage1"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage1"].Trim());
					shippingAddress.IDImage2 = (string.IsNullOrEmpty(imageServerUrl) ? Globals.SaveFile("user", context.Request["IDImage2"].Trim(), "/Storage/master/", true, false, "") : context.Request["IDImage2"].Trim());
				}
				if (MemberProcessor.UpdateShippingAddress(shippingAddress))
				{
					result = true;
				}
				else
				{
					msg = "更新收货地址证件信息失败";
				}
			}
			else
			{
				msg = "获取收货地址模型错误";
			}
			return result;
		}

		private bool ValIDCard(HttpContext context, ref string erromsg)
		{
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			if (!siteSettings.IsOpenCertification)
			{
				erromsg = "实名认证已关闭";
				return false;
			}
			Regex regex = new Regex("(^\\d{15}$)|(^\\d{18}$)|(^\\d{17}(\\d|X|x)$)");
			if (string.IsNullOrEmpty(context.Request.Params["IDNumber"]) || !regex.IsMatch(context.Request.Params["IDNumber"].Trim()))
			{
				erromsg = "身份证号格式错误";
				return false;
			}
			if (siteSettings.CertificationModel == 2)
			{
				if (string.IsNullOrEmpty(context.Request.Params["IDImage1"]))
				{
					erromsg = "请上传证件照正面";
					return false;
				}
				if (string.IsNullOrEmpty(context.Request.Params["IDImage2"]))
				{
					erromsg = "请上传证件照反面";
					return false;
				}
			}
			return true;
		}
	}
}
