using Hidistro.Context;
using Hidistro.Entities.Members;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_AppPaymentTypeSelect : WebControl
	{
		public enum EnumOrderSalesPromotion
		{
			CountDownBuy
		}

		public EnumOrderSalesPromotion? OrderSalesPromotion;

		private bool _ShowBalancePay = true;

		public new string CssClass
		{
			get;
			set;
		}

		public bool ShowBalancePay
		{
			get
			{
				return this._ShowBalancePay;
			}
			set
			{
				this._ShowBalancePay = value;
			}
		}

		public bool IsFireGroup
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StringBuilder stringBuilder = new StringBuilder();
			MemberInfo user = HiContext.Current.User;
			string text = "icon_wepay";
			string text2 = "icon_alipay";
			string text3 = "icon_appalipay";
			string text4 = "icon_prepay";
			string text5 = "icon_shengpay";
			string text6 = "icon_unionpay";
			string format = "<li><label class=\"label-checkbox item-content\"><input type=\"radio\" name=\"chk_paymentlist\" id=\"chk_paymentlist{0}\" value=\"{1}\" /><div class=\"item-media\"><i class=\"icon icon-form-checkbox\"></i></div><div class=\"pay_name\">{2}</div><span class=\"{3}\"></span></label></li>";
			string text7 = "<li><label class=\"label-checkbox item-content\"><input type=\"radio\" name=\"chk_paymentlist\" id=\"chk_paymentlist{0}\" value=\"{1}\" /><div class=\"item-media\"><i class=\"icon icon-form-checkbox\"></i></div><div class=\"pay_name\">{2}<i>帐户余额 <label>￥{4}</label></i></div><span class=\"{3}\"></span></label></li>";
			if (string.IsNullOrEmpty(this.CssClass))
			{
				stringBuilder.Append("<ul>");
			}
			else
			{
				stringBuilder.Append("<ul class=\"" + this.CssClass + "\">");
			}
			if (masterSettings.OpenAppWxPay)
			{
				stringBuilder.AppendLine(string.Format(format, 6, -22, "微信支付", text));
			}
			if (masterSettings.EnableAppAliPay)
			{
				stringBuilder.AppendLine(string.Format(format, 2, -10, "支付宝app支付", text3));
			}
			if (masterSettings.EnableAppWapAliPay)
			{
				stringBuilder.AppendLine(string.Format(format, 3, -4, "支付宝H5网页支付", text2));
			}
			if (masterSettings.EnableAPPBankUnionPay && !this.IsFireGroup)
			{
				stringBuilder.AppendLine(string.Format(format, 5, -7, "银联全渠道支付", text6));
			}
			if (masterSettings.EnableAppShengPay && !this.IsFireGroup)
			{
				stringBuilder.AppendLine(string.Format(format, 4, -5, "盛付通手机网页支付", text5));
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}

		private string CanAppend(string payType)
		{
			string result = payType;
			if (this.OrderSalesPromotion.HasValue && this.OrderSalesPromotion.Value == EnumOrderSalesPromotion.CountDownBuy)
			{
				result = string.Empty;
			}
			return result;
		}
	}
}
