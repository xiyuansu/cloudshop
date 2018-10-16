using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.RechargeGift)]
	public class RechargeGift : AdminPage
	{
		protected OnOff ooOpen;

		protected HtmlInputHidden hidIsLoading;

		protected HtmlInputHidden hidJson;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.ooOpen.Parameter.Add("onSwitchChange", "fuCheckEnableRecharge");
			this.btnOK.Click += this.btnOK_Click;
			if (!base.IsPostBack)
			{
				this.BindRechargeList();
			}
		}

		private void BindRechargeList()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.ooOpen.SelectedValue = masterSettings.IsOpenRechargeGift;
			if (masterSettings.IsOpenRechargeGift)
			{
				this.hidIsLoading.Value = "false";
				List<RechargeGiftInfo> rechargeGiftItemList = PromoteHelper.GetRechargeGiftItemList();
				string json = JsonHelper.GetJson(rechargeGiftItemList);
				this.hidJson.Value = json;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.IsOpenRechargeGift = this.ooOpen.SelectedValue;
			if (this.ooOpen.SelectedValue)
			{
				masterSettings.EnableBulkPaymentAdvance = false;
				string value = this.hidJson.Value;
				if (string.IsNullOrEmpty(value))
				{
					this.ShowMsg("充值赠送项不能为空！", false);
					return;
				}
				List<RechargeGiftInfo> list = JsonHelper.ParseFormJson<List<RechargeGiftInfo>>(value);
				if (list.Any())
				{
					PromoteHelper.DeleteRechargeGift();
					foreach (RechargeGiftInfo item in list)
					{
						PromoteHelper.AddRechargeGift(item);
					}
				}
			}
			else
			{
				PromoteHelper.DeleteRechargeGift();
			}
			SettingsManager.Save(masterSettings);
			this.hidIsLoading.Value = "false";
			this.ShowMsg("保存成功", true);
			this.BindRechargeList();
		}
	}
}
