using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Member;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RefundTypeRadioList : Literal
	{
		private int _SelectedValue = -1;

		public string OrderGateWay
		{
			get;
			set;
		}

		public int SelectedValue
		{
			get
			{
				return this._SelectedValue;
			}
			set
			{
				this._SelectedValue = value;
			}
		}

		public decimal BalanceAmount
		{
			get;
			set;
		}

		public static bool IsBackReturn(string refundGateway)
		{
			return TradeHelper.AllowRefundGateway.Contains(refundGateway);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string text = this.OrderGateWay.ToNullString();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string text2 = "<span> <label><input type=\"radio\" name=\"rad_refundtype\" id=\"rad_refundtype{0}\" value=\"{0}\" {2}></label><em>{1}</em></span>";
			string text3 = "checked=\"checked\"";
			bool flag = false;
			if (!string.IsNullOrEmpty(this.OrderGateWay) && TradeHelper.AllowRefundGateway.Contains(this.OrderGateWay) && this.BalanceAmount <= decimal.Zero)
			{
				flag = true;
				if ((text == "hishop.plugins.payment.wxqrcode.wxqrcoderequest" || text == "hishop.plugins.payment.weixinrequest") && (string.IsNullOrEmpty(masterSettings.WeixinCertPath) || string.IsNullOrEmpty(masterSettings.WeixinCertPassword)))
				{
					flag = false;
				}
				if (text == "hishop.plugins.payment.wxappletpay" && (string.IsNullOrEmpty(masterSettings.WxApplectPayCert) || string.IsNullOrEmpty(masterSettings.WxApplectPayCertPassword)))
				{
					flag = false;
				}
				if (text == "hishop.plugins.payment.wxo2oappletpay" && (string.IsNullOrEmpty(masterSettings.O2OAppletPayCert) || string.IsNullOrEmpty(masterSettings.O2OAppletPayCertPassword)))
				{
					flag = false;
				}
				if (text == "hishop.plugins.payment.appwxrequest")
				{
					string appWxMchId = masterSettings.AppWxMchId;
					string b = string.IsNullOrEmpty(masterSettings.Main_Mch_ID) ? masterSettings.WeixinPartnerID : masterSettings.Main_Mch_ID;
					if (string.IsNullOrEmpty(masterSettings.AppWxCertPath) || string.IsNullOrEmpty(masterSettings.AppWxCertPass) || (appWxMchId == b && (string.IsNullOrEmpty(masterSettings.WeixinCertPath) || string.IsNullOrEmpty(masterSettings.WeixinCertPassword))))
					{
						flag = false;
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (text.ToLower() == "hishop.plugins.payment.cashreceipts")
			{
				stringBuilder.AppendLine(string.Format(text2, 4, EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.ReturnOnStore, 0), text3));
			}
			else
			{
				int num;
				if (flag)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string format = text2;
					num = 3;
					stringBuilder2.AppendLine(string.Format(format, num.ToString(), EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.BackReturn, 0), (this.SelectedValue == 3 || this.SelectedValue == -1) ? text3 : ""));
				}
				if (HiContext.Current.User.UserId > 0 && HiContext.Current.User.IsOpenBalance)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					string format2 = text2;
					num = 1;
					stringBuilder3.AppendLine(string.Format(format2, num.ToString(), EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.InBalance, 0), (this.SelectedValue == 1 || text.ToLower() == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1)) ? text3 : ""));
				}
				if (text.ToLower() != EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1) && this.BalanceAmount <= decimal.Zero)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					string format3 = text2;
					object[] obj = new object[4];
					num = 2;
					obj[0] = num.ToString();
					obj[1] = EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.InBankCard, 0);
					obj[2] = EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.InBankCard, 0);
					obj[3] = ((this.SelectedValue == 2) ? text3 : "");
					stringBuilder4.AppendLine(string.Format(format3, obj));
				}
			}
			base.Text = stringBuilder.ToString();
			base.Render(writer);
		}
	}
}
