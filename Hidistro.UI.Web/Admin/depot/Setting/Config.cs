using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot.Setting
{
	[PrivilegeCheck(Privilege.StoreSetting)]
	public class Config : AdminPage
	{
		protected RadioButtonList rblPositionRouteTo;

		protected RadioButtonList rblPositionNoMatchTo;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected HiddenField hidUrls;

		protected OnOff radAutoAllotOrder;

		protected OnOff radStoreNeedTakeCode;

		protected OnOff radIsRecommend;

		protected OnOff radOrderInClosingTime;

		protected OnOff radMemberVisitOtherStore;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.bindPositionRouteTo();
				this.bindConfig();
			}
		}

		private void bindConfig()
		{
			HiddenField hiddenField = this.hidUrls;
			HiddenField hiddenField2 = this.hidOldImages;
			string text3 = hiddenField.Value = (hiddenField2.Value = "");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(masterSettings.Store_PositionRouteTo))
			{
				this.rblPositionRouteTo.SelectedValue = masterSettings.Store_PositionRouteTo;
			}
			if (!string.IsNullOrEmpty(masterSettings.Store_PositionNoMatchTo))
			{
				this.rblPositionNoMatchTo.SelectedValue = masterSettings.Store_PositionNoMatchTo;
			}
			this.radIsRecommend.SelectedValue = masterSettings.Store_IsRecommend;
			this.radOrderInClosingTime.SelectedValue = masterSettings.Store_IsOrderInClosingTime;
			this.radMemberVisitOtherStore.SelectedValue = masterSettings.Store_IsMemberVisitBelongStore;
			this.radAutoAllotOrder.SelectedValue = masterSettings.AutoAllotOrder;
			this.radStoreNeedTakeCode.SelectedValue = masterSettings.StoreNeedTakeCode;
			if (!string.IsNullOrEmpty(masterSettings.Store_BannerInfo))
			{
				string[] array = masterSettings.Store_BannerInfo.Split(';');
				string[] array2 = array;
				foreach (string text4 in array2)
				{
					if (text4.Contains("$"))
					{
						HiddenField hiddenField3 = this.hidUrls;
						hiddenField3.Value = hiddenField3.Value + text4.Split('$')[1] + ",";
						HiddenField hiddenField4 = this.hidOldImages;
						hiddenField4.Value = hiddenField4.Value + text4.Split('$')[0] + ",";
					}
				}
			}
		}

		private void bindPositionRouteTo()
		{
			foreach (EnumStore_PositionRouteTo value in Enum.GetValues(typeof(EnumStore_PositionRouteTo)))
			{
				ListItem item = new ListItem(((Enum)(object)value).ToDescription(), value.ToString());
				this.rblPositionRouteTo.Items.Add(item);
			}
			this.rblPositionRouteTo.SelectedValue = 1.ToString();
			foreach (EnumStore_PositionNoMatchTo value2 in Enum.GetValues(typeof(EnumStore_PositionNoMatchTo)))
			{
				ListItem item2 = new ListItem(((Enum)(object)value2).ToDescription(), value2.ToString());
				this.rblPositionNoMatchTo.Items.Add(item2);
			}
			this.rblPositionNoMatchTo.SelectedValue = 1.ToString();
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.Store_PositionRouteTo = this.rblPositionRouteTo.SelectedValue;
			masterSettings.Store_PositionNoMatchTo = this.rblPositionNoMatchTo.SelectedValue;
			masterSettings.Store_BannerInfo = this.hidOldImages.Value;
			masterSettings.AutoAllotOrder = this.radAutoAllotOrder.SelectedValue;
			masterSettings.StoreNeedTakeCode = this.radStoreNeedTakeCode.SelectedValue;
			masterSettings.Store_IsRecommend = this.radIsRecommend.SelectedValue;
			masterSettings.Store_IsOrderInClosingTime = this.radOrderInClosingTime.SelectedValue;
			if (masterSettings.Store_PositionRouteTo == 1.ToString())
			{
				masterSettings.Store_IsMemberVisitBelongStore = this.radMemberVisitOtherStore.SelectedValue;
			}
			else
			{
				masterSettings.Store_IsMemberVisitBelongStore = false;
			}
			string[] array = this.hidUrls.Value.Split(',');
			string[] array2 = this.hidUploadImages.Value.Split(',');
			string[] source = this.hidOldImages.Value.Split(',');
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array2.Length; i++)
			{
				string value = array2[i];
				if (!string.IsNullOrEmpty(value))
				{
					if (!source.Contains(value))
					{
						value = base.UploadImage(array2[i], "depot");
					}
					stringBuilder.Append(value);
					stringBuilder.Append("$");
					stringBuilder.Append((array.Length >= i) ? array[i] : "");
					stringBuilder.Append(";");
				}
			}
			masterSettings.Store_BannerInfo = stringBuilder.ToString();
			SettingsManager.Save(masterSettings);
			this.ShowMsg("保存成功", true);
			this.bindConfig();
		}
	}
}
