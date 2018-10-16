using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DadaSetting)]
	public class DadaLogistics : AdminPage
	{
		protected OnOff ooEnableHtmRewrite;

		protected AccountNumbersTextBox txtAppKey;

		protected AccountNumbersTextBox txtAppSecret;

		protected AccountNumbersTextBox txtSourceID;

		protected Button btnSave;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.ooEnableHtmRewrite.Parameter.Add("onSwitchChange", "fuCheckEnableDaDa");
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.ooEnableHtmRewrite.SelectedValue = masterSettings.OpenDadaLogistics;
				this.txtAppKey.Text = masterSettings.DadaAppKey;
				this.txtAppSecret.Text = masterSettings.DadaAppSecret;
				this.txtSourceID.Text = masterSettings.DadaSourceID;
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.DadaAppKey = this.txtAppKey.Text;
			masterSettings.DadaAppSecret = this.txtAppSecret.Text;
			masterSettings.DadaSourceID = this.txtSourceID.Text;
			if (this.ooEnableHtmRewrite.SelectedValue)
			{
				if (string.IsNullOrEmpty(this.txtAppKey.Text))
				{
					this.ShowMsg("app_key不能为空！", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtAppSecret.Text))
				{
					this.ShowMsg("app_secret不能为空！", false);
					return;
				}
				if (string.IsNullOrEmpty(this.txtSourceID.Text))
				{
					this.ShowMsg("source_id不能为空！", false);
					return;
				}
				DataTable dataTable = DepotHelper.SynchroDadaStoreList(0);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						string station_name = dataTable.Rows[i]["StoreName"].ToNullString();
						int business = 5;
						string city_name = dataTable.Rows[i]["CityName"].ToNullString().Replace("市", "");
						string area_name = dataTable.Rows[i]["RegionName"].ToNullString();
						string station_address = dataTable.Rows[i]["Address"].ToNullString();
						double lng = dataTable.Rows[i]["Longitude"].ToDouble(0);
						double lat = dataTable.Rows[i]["Latitude"].ToDouble(0);
						string contact_name = dataTable.Rows[i]["ContactMan"].ToNullString();
						string phone = dataTable.Rows[i]["Tel"].ToNullString();
						string origin_shop_id = dataTable.Rows[i]["StoreId"].ToNullString();
						string value = DadaHelper.shopAdd(this.txtSourceID.Text, station_name, business, city_name, area_name, station_address, lng, lat, contact_name, phone, origin_shop_id, "", "", "");
						JObject jObject = JsonConvert.DeserializeObject(value) as JObject;
						string a = jObject["status"].ToNullString();
						int num = jObject["code"].ToInt(0);
						if (a == "fail" && num != 7718)
						{
							this.ShowMsg(jObject["msg"].ToNullString(), false);
							return;
						}
					}
				}
			}
			masterSettings.OpenDadaLogistics = this.ooEnableHtmRewrite.SelectedValue;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true, "DadaLogistics");
		}
	}
}
