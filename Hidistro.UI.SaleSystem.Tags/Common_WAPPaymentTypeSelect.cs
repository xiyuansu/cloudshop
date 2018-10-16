using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_WAPPaymentTypeSelect : WebControl
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

		public ClientType ClientType
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

		public bool IsServiceProduct
		{
			get;
			set;
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
			string text3 = "icon_prepay";
			string text4 = "icon_shengpay";
			string text5 = "icon_unionpay";
			string format = "<li><label class=\"label-checkbox item-content\"><input type=\"radio\" name=\"chk_paymentlist\" id=\"chk_paymentlist{0}\" value=\"{1}\" /><div class=\"item-media\"><i class=\"icon icon-form-checkbox\"></i></div><div class=\"pay_name\">{2}</div><span class=\"{3}\"></span></label></li>";
			string text6 = "<li><label class=\"label-checkbox item-content\"><input type=\"radio\" name=\"chk_paymentlist\" id=\"chk_paymentlist{0}\" value=\"{1}\" /><div class=\"item-media\"><i class=\"icon icon-form-checkbox\"></i></div><div class=\"pay_name\">{2}<i>帐户余额 <label>￥{4}</label></i></div><span class=\"{3}\"></span></label></li>";
			if (string.IsNullOrEmpty(this.CssClass))
			{
				stringBuilder.Append("<ul>");
			}
			else
			{
				stringBuilder.Append("<ul class=\"" + this.CssClass + "\">");
			}
			bool flag = false;
			if (user.UserId > 0)
			{
				MemberOpenIdInfo memberOpenIdInfo = null;
				if (user.MemberOpenIds != null && user.MemberOpenIds.Count() > 0)
				{
					user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
				}
				if (memberOpenIdInfo != null && !string.IsNullOrWhiteSpace(memberOpenIdInfo.OpenId))
				{
					flag = true;
				}
			}
			if (this.ClientType == ClientType.AliOH)
			{
				if (masterSettings.EnableWapAliPay && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 2, -4, "支付宝H5网页支付", text2));
				}
				if (masterSettings.EnableWapShengPay && !this.IsFireGroup && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 3, -5, "盛付通手机网页支付", text4));
				}
				if (masterSettings.EnableWapWeiXinPay)
				{
					stringBuilder.AppendLine(string.Format(format, 5, -22, "微信H5支付", text));
				}
				if (masterSettings.EnableBankUnionPay && !this.IsFireGroup && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 4, -7, "银联全渠道支付", text5));
				}
			}
			else if (this.ClientType == ClientType.VShop)
			{
				if (masterSettings.EnableWeiXinRequest)
				{
					stringBuilder.AppendLine(string.Format(format, 5, -2, "微信支付", text));
				}
				if (masterSettings.EnableWeixinWapAliPay && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 10, -20, "支付宝支付", text2));
				}
				if (masterSettings.EnableWapShengPay && !this.IsFireGroup && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 3, -5, "盛付通手机网页支付", text4));
				}
			}
			else
			{
				if (masterSettings.EnableWapAliPay && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 2, -4, "支付宝H5网页支付", text2));
				}
				if (masterSettings.EnableBankUnionPay && !this.IsFireGroup && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 4, -7, "银联全渠道支付", text5));
				}
				if (this.ClientType == ClientType.WAP && masterSettings.EnableWapWeiXinPay)
				{
					stringBuilder.AppendLine(string.Format(format, 5, -22, "微信H5支付", text));
				}
				if (masterSettings.EnableWapShengPay && !this.IsFireGroup && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 3, -5, "盛付通手机网页支付", text4));
				}
				if (masterSettings.EnableWapAliPayCrossBorder && !this.IsServiceProduct)
				{
					stringBuilder.AppendLine(string.Format(format, 2, -9, "支付宝境外手机网页支付", text2));
				}
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
