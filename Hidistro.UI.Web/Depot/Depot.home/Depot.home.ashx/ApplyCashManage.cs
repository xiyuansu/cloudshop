using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Web.ashxBase;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Depot.home.ashx
{
	public class ApplyCashManage : StoreAdminBaseHandler
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
				StoreBalanceDrawRequestQuery storeBalanceDrawRequestQuery = new StoreBalanceDrawRequestQuery();
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
				storeBalanceDrawRequestQuery.PageIndex = num;
				storeBalanceDrawRequestQuery.PageSize = num2;
				storeBalanceDrawRequestQuery.SortBy = "RequestTime";
				storeBalanceDrawRequestQuery.SortOrder = SortAction.Desc;
				storeBalanceDrawRequestQuery.StoreId = base.CurrentManager.StoreId;
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
								ManagerRemark = b.ManagerRemark,
								Remark = b.Remark
							};
						})
					}
				});
				context.Response.Write(s);
				context.Response.End();
			}
			if (context.Request["flag"] == "GetDrawCardInfo")
			{
				StoresInfo storeById = StoresHelper.GetStoreById(base.CurrentManager.StoreId);
				StoreBalanceInfo storeBalance = StoreBalanceHelper.GetStoreBalance(storeById.StoreId, storeById.CommissionRate);
				decimal num3 = storeBalance.Balance - storeBalance.BalanceForzen;
				if (num3 < decimal.Zero)
				{
					num3 = default(decimal);
				}
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				string s2 = JsonConvert.SerializeObject(new
				{
					Result = new
					{
						Balance = num3.F2ToString("f2").ToDecimal(0),
						IsOpenAlipayDraw = siteSettings.EnableBulkPaymentAliPay,
						AlipayAccount = storeById.AlipayAccount,
						AlipayRealName = storeById.AlipayRealName,
						BankName = storeById.BankName,
						BankAccountName = storeById.BankAccountName,
						BankCardNo = storeById.BankCardNo
					}
				});
				context.Response.Write(s2);
				context.Response.End();
			}
			if (context.Request["flag"] == "ValidTradePassword")
			{
				StoresInfo storeById2 = StoresHelper.GetStoreById(base.CurrentManager.StoreId);
				string pass = context.Request["Password"].ToNullString();
				if (storeById2.TradePassword == Users.EncodePassword(pass, storeById2.TradePasswordSalt))
				{
					string s3 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS",
							Msg = "验证成功"
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
							Msg = "验证失败"
						}
					});
					context.Response.Write(s4);
					context.Response.End();
				}
			}
			if (context.Request["flag"] == "ApplyBalanceRequest")
			{
				StoresInfo storeById3 = StoresHelper.GetStoreById(base.CurrentManager.StoreId);
				string pass2 = context.Request["Password"].ToNullString();
				if (storeById3.TradePassword != Users.EncodePassword(pass2, storeById3.TradePasswordSalt))
				{
					context.Response.Write(this.GetErrorJosn(521, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.TradePassword_Error, 0)));
					context.Response.End();
				}
				int num4 = context.Request["CardType"].ToInt(0);
				if (num4 != 1 && num4 != 2)
				{
					context.Response.Write(this.GetErrorJosn(133, "帐号类型" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.ValueUndefined, 0)));
					context.Response.End();
				}
				if (num4 == 1 && !HiContext.Current.SiteSettings.EnableBulkPaymentAliPay)
				{
					context.Response.Write(this.GetErrorJosn(134, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.PlatNotOpenAlipayDraw, 0)));
					context.Response.End();
				}
				if (num4 == 1 && (string.IsNullOrEmpty(storeById3.AlipayAccount) || string.IsNullOrEmpty(storeById3.AlipayRealName)))
				{
					context.Response.Write(this.GetErrorJosn(523, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.StoreNotBindAlipayInfo, 0)));
					context.Response.End();
				}
				if (num4 == 2 && (string.IsNullOrEmpty(storeById3.BankAccountName) || string.IsNullOrEmpty(storeById3.BankCardNo) || string.IsNullOrEmpty(storeById3.BankName)))
				{
					context.Response.Write(this.GetErrorJosn(524, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.StoreNotBindBankCardInfo, 0)));
					context.Response.End();
				}
				decimal num5 = context.Request["RequestAmount"].ToDecimal(0);
				if (num5 <= decimal.Zero)
				{
					context.Response.Write(this.GetErrorJosn(526, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.RequestAmountError, 0)));
					context.Response.End();
				}
				if (num5 > storeById3.Balance)
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
				if (num4 == 1)
				{
					storeBalanceDrawRequestInfo.AlipayCode = storeById3.AlipayAccount;
					storeBalanceDrawRequestInfo.AlipayRealName = storeById3.AlipayRealName;
					storeBalanceDrawRequestInfo.IsAlipay = true;
				}
				else
				{
					storeBalanceDrawRequestInfo.BankName = storeById3.BankName;
					storeBalanceDrawRequestInfo.AccountName = storeById3.BankAccountName;
					storeBalanceDrawRequestInfo.MerchantCode = storeById3.BankCardNo;
					storeBalanceDrawRequestInfo.IsAlipay = false;
				}
				storeBalanceDrawRequestInfo.RequestState = 1.ToString();
				storeBalanceDrawRequestInfo.Remark = text;
				storeBalanceDrawRequestInfo.Amount = num5;
				storeBalanceDrawRequestInfo.RequestTime = DateTime.Now;
				storeBalanceDrawRequestInfo.StoreId = storeById3.StoreId;
				if (StoreBalanceHelper.BalanceDrawRequest(storeBalanceDrawRequestInfo))
				{
					string s5 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS",
							Msg = "申请提现成功"
						}
					});
					context.Response.Write(s5);
					context.Response.End();
					goto IL_0738;
				}
				context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
				return;
			}
			goto IL_0738;
			IL_0738:
			if (context.Request["flag"] == "CheckPasswordInfo")
			{
				StoresInfo storeById4 = StoresHelper.GetStoreById(base.CurrentManager.StoreId);
				if (storeById4.TradePassword == null || storeById4.TradePassword == "")
				{
					string s6 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "FAIL",
							Msg = "未设置交易密码"
						}
					});
					context.Response.Write(s6);
					context.Response.End();
				}
				else
				{
					string s7 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS",
							Msg = "已设置交易密码"
						}
					});
					context.Response.Write(s7);
					context.Response.End();
				}
			}
			if (context.Request["flag"] == "BindDrawCardInfo")
			{
				StoresInfo storeById5 = StoresHelper.GetStoreById(base.CurrentManager.StoreId);
				string pass3 = context.Request["Password"].ToNullString();
				if (storeById5.TradePassword != Users.EncodePassword(pass3, storeById5.TradePasswordSalt))
				{
					context.Response.Write(this.GetErrorJosn(521, EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.TradePassword_Error, 0)));
					context.Response.End();
				}
				string text2 = "";
				string text3 = "";
				string text4 = "";
				string text5 = "";
				string text6 = "";
				if (base.CurrentSiteSetting.EnableBulkPaymentAliPay)
				{
					text2 = Globals.StripAllTags(context.Request["AlipayAccount"].ToNullString());
					text3 = Globals.StripAllTags(context.Request["AlipayRealName"].ToNullString());
					if ((string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3)) || (!string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text3)))
					{
						context.Response.Write(this.GetErrorJosn(104, "支付宝帐号和真实姓名" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Empty_Error, 0)));
						context.Response.End();
					}
					if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
					{
						storeById5.AlipayAccount = text2;
						storeById5.AlipayRealName = text3;
					}
				}
				text4 = Globals.StripAllTags(context.Request["BankName"].ToNullString());
				text5 = Globals.StripAllTags(context.Request["BankAccountName"].ToNullString());
				text6 = Globals.StripAllTags(context.Request["BankCardNo"].ToNullString());
				if (!base.CurrentSiteSetting.EnableBulkPaymentAliPay && (string.IsNullOrEmpty(text4) || string.IsNullOrEmpty(text5) || string.IsNullOrEmpty(text6)))
				{
					context.Response.Write(this.GetErrorJosn(104, "银行卡帐号" + EnumDescription.GetEnumDescription((Enum)(object)ApiErrorCode.Empty_Error, 0)));
					context.Response.End();
				}
				storeById5.BankName = text4;
				storeById5.BankAccountName = text5;
				storeById5.BankCardNo = text6;
				if (StoresHelper.UpdateStore(storeById5))
				{
					string s8 = JsonConvert.SerializeObject(new
					{
						Result = new
						{
							Status = "SUCCESS",
							Msg = "更新成功"
						}
					});
					context.Response.Write(s8);
					context.Response.End();
					goto IL_0af6;
				}
				context.Response.Write(this.GetErrorJosn(0, ((Enum)(object)ApiErrorCode.Failed).ToDescription()));
				return;
			}
			goto IL_0af6;
			IL_0af6:
			if (context.Request["flag"] == "SetTradePassword")
			{
				StoresInfo storeById6 = StoresHelper.GetStoreById(base.CurrentManager.StoreId);
				if (!string.IsNullOrEmpty(storeById6.TradePassword))
				{
					context.Response.Write(this.GetErrorJosn(519, ((Enum)(object)ApiErrorCode.TradePasswordAlreadySet).ToDescription()));
				}
				else
				{
					string text7 = context.Request["Password"].ToNullString();
					string b2 = context.Request["RePassword"].ToNullString();
					if (text7 != b2)
					{
						context.Response.Write(this.GetErrorJosn(213, ((Enum)(object)ApiErrorCode.RePasswordNoEqualsPassword).ToDescription()));
					}
					else if (string.IsNullOrEmpty(text7) || text7.Length < 6)
					{
						context.Response.Write(this.GetErrorJosn(212, ((Enum)(object)ApiErrorCode.Password_Error).ToDescription()));
					}
					else
					{
						storeById6.TradePasswordSalt = Globals.RndStr(128, true);
						storeById6.TradePassword = Users.EncodePassword(text7, storeById6.TradePasswordSalt);
						if (StoresHelper.UpdateStore(storeById6))
						{
							string s9 = JsonConvert.SerializeObject(new
							{
								Result = new
								{
									Status = "SUCCESS",
									Msg = ""
								}
							});
							context.Response.Write(s9);
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
