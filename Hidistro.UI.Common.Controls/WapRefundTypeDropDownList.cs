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
	public class WapRefundTypeDropDownList : WebControl
	{
		private string _CssClass = "pay_list";

		private string _nullToDisplay = "请选择退款方式";

		public new string CssClass
		{
			get
			{
				return this._CssClass;
			}
			set
			{
				this._CssClass = value;
			}
		}

		public string NullToDisplay
		{
			get
			{
				return this._nullToDisplay;
			}
			set
			{
				this._nullToDisplay = value;
			}
		}

		public string OrderGateWay
		{
			get;
			set;
		}

		public string SelectedValue
		{
			get;
			set;
		}

		public bool IsServiceOrder
		{
			get;
			set;
		}

		public decimal BalanceAmount
		{
			get;
			set;
		}

		public int preSaleId
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string text = this.OrderGateWay.ToNullString().ToLower();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StringBuilder stringBuilder = new StringBuilder();
			string text2 = "checked=\"checked\"";
			string text3 = "<li><label class=\"label-checkbox item-content\"><input type=\"radio\" name=\"chk_refundtype\" value=\"{0}\" {2}><div class=\"item-media\"><i class=\"icon icon-form-checkbox\"></i></div><div class=\"pay_name\">{1}</li>";
			stringBuilder.AppendLine("<ul class=\"" + this.CssClass + "\">");
			bool flag = false;
			if (TradeHelper.AllowRefundGateway.Contains(text) && this.preSaleId <= 0 && this.BalanceAmount <= decimal.Zero)
			{
				flag = true;
				if ((text == "hishop.plugins.payment.wxqrcode.wxqrcoderequest" || text == "hishop.plugins.payment.weixinrequest") && (string.IsNullOrEmpty(masterSettings.WeixinCertPath) || string.IsNullOrEmpty(masterSettings.WeixinCertPassword)))
				{
					flag = false;
				}
				if (text == "hishop.plugins.payment.wxo2oappletpay" && (string.IsNullOrEmpty(masterSettings.O2OAppletPayCert) || string.IsNullOrEmpty(masterSettings.O2OAppletPayCertPassword)))
				{
					flag = false;
				}
				if (text == "hishop.plugins.payment.wxappletpay" && (string.IsNullOrEmpty(masterSettings.WxApplectPayCert) || string.IsNullOrEmpty(masterSettings.WxApplectPayCertPassword)))
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
			int num;
			if (text.ToLower() == "hishop.plugins.payment.cashreceipts")
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string format = text3;
				num = 4;
				stringBuilder2.AppendLine(string.Format(format, num.ToString(), EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.ReturnOnStore, 0), text2));
			}
			else
			{
				if (flag)
				{
					StringBuilder stringBuilder3 = stringBuilder;
					string format2 = text3;
					num = 3;
					stringBuilder3.AppendLine(string.Format(format2, num.ToString(), EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.BackReturn, 0), (this.SelectedValue.ToInt(0) == 3 || string.IsNullOrEmpty(this.SelectedValue)) ? text2 : ""));
				}
				if (HiContext.Current.User.UserId > 0 && HiContext.Current.User.IsOpenBalance)
				{
					StringBuilder stringBuilder4 = stringBuilder;
					string format3 = text3;
					num = 1;
					stringBuilder4.AppendLine(string.Format(format3, num.ToString(), EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.InBalance, 0), (this.SelectedValue.ToInt(0) == 1 || text.ToLower() == EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1)) ? text2 : ""));
				}
				if (text.ToLower() != EnumDescription.GetEnumDescription((Enum)(object)EnumPaymentType.AdvancePay, 1) && !this.IsServiceOrder && this.BalanceAmount <= decimal.Zero)
				{
					StringBuilder stringBuilder5 = stringBuilder;
					string format4 = text3;
					object[] obj = new object[4];
					num = 2;
					obj[0] = num.ToString();
					obj[1] = EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.InBankCard, 0);
					obj[2] = EnumDescription.GetEnumDescription((Enum)(object)RefundTypes.InBankCard, 0);
					obj[3] = ((this.SelectedValue.ToInt(0) == 2) ? text2 : "");
					stringBuilder5.AppendLine(string.Format(format4, obj));
				}
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}

		public static bool IsBackReturn(string refundGateway)
		{
			return TradeHelper.AllowRefundGateway.Contains(refundGateway);
		}
	}
}
