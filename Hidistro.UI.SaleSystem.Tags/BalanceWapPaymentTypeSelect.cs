using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class BalanceWapPaymentTypeSelect : WebControl
	{
		public ClientType ClientType
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StringBuilder stringBuilder = new StringBuilder();
			MemberInfo user = HiContext.Current.User;
			bool flag = false;
			if (user.UserId > 0)
			{
				MemberOpenIdInfo memberOpenIdInfo = user.MemberOpenIds.FirstOrDefault((MemberOpenIdInfo item) => item.OpenIdType.ToLower() == "hishop.plugins.openid.weixin");
				if (memberOpenIdInfo != null && !string.IsNullOrWhiteSpace(memberOpenIdInfo.OpenId))
				{
					flag = true;
				}
			}
			if (this.ClientType == ClientType.AliOH)
			{
				stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
				stringBuilder.AppendLine("<ul id=\"selectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
				if (masterSettings.EnableWapAliPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-4\">支付宝H5网页支付</a></li>");
				}
				if (masterSettings.EnableWapShengPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-5\">盛付通手机网页支付</a></li>");
				}
				if (masterSettings.EnableBankUnionPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-7\">银联全渠道支付</a></li>");
				}
				stringBuilder.AppendLine("</ul>");
			}
			else if (this.ClientType == ClientType.VShop)
			{
				stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
				stringBuilder.AppendLine("<ul id=\"selectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
				if (masterSettings.EnableWeiXinRequest)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-2\">微信支付</a></li>");
				}
				if (masterSettings.EnableWapShengPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-5\">盛付通手机网页支付</a></li>");
				}
				stringBuilder.AppendLine("</ul>");
			}
			else if (this.ClientType == ClientType.WAP)
			{
				stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
				stringBuilder.AppendLine("<ul id=\"selectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
				if (masterSettings.EnableWapAliPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-4\">支付宝H5网页支付</a></li>");
				}
				if (masterSettings.EnableWapShengPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-5\">盛付通手机网页支付</a></li>");
				}
				if (masterSettings.EnableBankUnionPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-7\">银联全渠道支付</a></li>");
				}
				if (masterSettings.EnableWapAliPayCrossBorder)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-9\">支付宝境外手机网页支付</a></li>");
				}
				stringBuilder.AppendLine("</ul>");
			}
			else if (this.ClientType == ClientType.App)
			{
				stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
				stringBuilder.AppendLine("<ul id=\"selectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
				if (masterSettings.EnableAppWapAliPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-4\">支付宝H5网页支付</a></li>");
				}
				if (masterSettings.EnableAppAliPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-10\">支付宝app支付</a></li>");
				}
				if (masterSettings.EnableAppShengPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-5\">盛付通手机网页支付</a></li>");
				}
				if (masterSettings.EnableAPPBankUnionPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-7\">银联全渠道支付</a></li>");
				}
				if (masterSettings.OpenAppWxPay)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"-8\">微信支付</a></li>");
				}
				stringBuilder.AppendLine("</ul>");
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
