using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using HiShop.API.HiPOS.AdvancedAPIs.Merchant.MerchantJson;
using System;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class AddQRCodeHiPOS : AdminPage
	{
		public int storeId;

		protected Literal ltStoreName;

		protected Image imgQRCode;

		protected Literal ltHiPOSAlias;

		protected Button btnClose;

		public string Url
		{
			get;
			set;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.storeId = this.Page.Request["storeId"].ToInt(0);
			if (!base.IsPostBack)
			{
				this.CreateQRCode();
			}
		}

		private void CreateQRCode()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
			if (storeById == null)
			{
				this.ShowMsg("门店不存在", false, "BindHiPOS.aspx");
			}
			else
			{
				this.ltStoreName.Text = storeById.StoreName;
				string storeHiPOSLastAlias = ManagerHelper.GetStoreHiPOSLastAlias();
				HiPOSHelper hiPOSHelper = new HiPOSHelper();
				AuthCodeResult authCode = hiPOSHelper.GetAuthCode(masterSettings.HiPOSAppId, masterSettings.HiPOSMerchantId, storeById.StoreName);
				if (authCode.error == null)
				{
					this.Url = authCode.merchant_authcode_response.qr;
				}
				else
				{
					this.ShowMsg(authCode.error.message, false);
				}
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

		protected void btnClose_Click(object sender, EventArgs e)
		{
			this.CloseWindow("BindHiPOS.aspx");
		}
	}
}
