using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.WAPShop
{
	public class BasicConfig : AdminPage
	{
		protected TextBox txtPartner;

		protected TextBox txtKey;

		protected TextBox txtAccount;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnOK.Click += this.btnOK_Click;
			if (!base.IsPostBack)
			{
				IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnVX);
				if (paymentModes != null && paymentModes.Count != 0)
				{
					string xml = HiCryptographer.Decrypt(paymentModes[0].Settings);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					this.txtPartner.Text = xmlDocument.GetElementsByTagName("Partner")[0].InnerText;
					this.txtKey.Text = xmlDocument.GetElementsByTagName("Key")[0].InnerText;
					this.txtAccount.Text = xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText;
				}
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			string text = $"<xml><Partner>{this.txtPartner.Text}</Partner><Key>{this.txtKey.Text}</Key><Seller_account_name>{this.txtAccount.Text}</Seller_account_name></xml>";
			IList<PaymentModeInfo> paymentModes = SalesHelper.GetPaymentModes(PayApplicationType.payOnVX);
			if (paymentModes == null || paymentModes.Count == 0)
			{
				PaymentModeInfo paymentMode = new PaymentModeInfo
				{
					Name = "支付宝手机支付",
					Gateway = "hishop.plugins.payment.ws_wappay.wswappayrequest",
					Description = string.Empty,
					IsUseInpour = true,
					Settings = HiCryptographer.Encrypt(text)
				};
				if (SalesHelper.CreatePaymentMode(paymentMode))
				{
					this.ShowMsg("设置成功", true);
				}
				else
				{
					this.ShowMsg("设置失败", false);
				}
			}
			else
			{
				PaymentModeInfo paymentModeInfo = paymentModes[0];
				paymentModeInfo.Settings = HiCryptographer.Encrypt(text);
				if (SalesHelper.UpdatePaymentMode(paymentModeInfo))
				{
					this.ShowMsg("设置成功", true, "");
				}
				else
				{
					this.ShowMsg("设置失败", false, "");
				}
			}
		}
	}
}
