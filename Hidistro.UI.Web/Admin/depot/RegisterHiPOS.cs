using Hidistro.Context;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class RegisterHiPOS : AdminPage
	{
		protected TextBox txtSellerName;

		protected TextBox txtContactName;

		protected TextBox txtContactPhone;

		protected LinkButton btnAdd;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.txtContactName.Text = masterSettings.HiPOSContactName;
				this.txtContactPhone.Text = masterSettings.HiPOSContactPhone;
				this.txtSellerName.Text = masterSettings.HiPOSSellerName;
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			string text = this.txtSellerName.Text.Trim();
			string text2 = this.txtContactName.Text.Trim();
			string text3 = this.txtContactPhone.Text.Trim();
			if (string.IsNullOrEmpty(text) || text.Length > 50)
			{
				this.ShowMsg("商户名称不能为空，长度必须小于或等于50个字", false);
			}
			else if (string.IsNullOrEmpty(text2) || text2.Length > 10)
			{
				this.ShowMsg("联系人姓名不能为空，长度必须小于或等于10个字", false);
			}
			else if (string.IsNullOrEmpty(text3) || text3.Length > 20)
			{
				this.ShowMsg("手机号码不能为空，长度必须小于或等于20个字符", false);
			}
			else
			{
				HiPOSHelper hiPOSHelper = new HiPOSHelper();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				MerchantResult merchantResult = hiPOSHelper.UpdateMerchant(masterSettings.HiPOSAppId, masterSettings.HiPOSMerchantId, text, text2, text3);
				if (merchantResult.error == null)
				{
					masterSettings.HiPOSContactName = text2;
					masterSettings.HiPOSContactPhone = text3;
					masterSettings.HiPOSSellerName = text;
					masterSettings.HiPOSExpireAt = DateTime.Parse(merchantResult.merchant_update_response.expire_at, null, DateTimeStyles.RoundtripKind).ToString("yyyy-MM-dd HH:mm:ss");
					SettingsManager.Save(masterSettings);
					string text4 = "";
					string text5 = "";
					string text6 = "";
					string siteUrl = masterSettings.SiteUrl;
					if (siteUrl.IndexOf("http") < 0)
					{
						text4 = "http://" + masterSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=orderStatus";
						text5 = "http://" + masterSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=orderConfirm";
						text6 = "http://" + masterSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=authqr";
					}
					else
					{
						text4 = masterSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=orderStatus";
						text5 = masterSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=orderConfirm";
						text6 = masterSettings.SiteUrl + "/API/HiPOSAPI.ashx?action=authqr";
					}
					HishopO2OResult hishopO2OResult = hiPOSHelper.SetHishopO2O(masterSettings.HiPOSAppId, masterSettings.HiPOSMerchantId, text4, text5, text6);
					if (hishopO2OResult.error == null)
					{
						this.ShowMsg("提交商户资料成功", true, "RegisterHiPOSPay.aspx");
					}
					else
					{
						this.ShowMsg(merchantResult.error.message, false);
					}
				}
				else
				{
					this.ShowMsg(merchantResult.error.message, false);
				}
			}
		}
	}
}
