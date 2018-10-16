using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class EditStores : AdminPage
	{
		private int storeId = 0;

		protected HtmlInputHidden hidStoreId;

		protected TrimTextBox txtStoresName;

		protected RegionSelector dropRegion;

		protected TrimTextBox txtAddress;

		protected TrimTextBox txtContactMan;

		protected TrimTextBox txtTel;

		protected HtmlGenericControl l_tags;

		protected StoreTagsLiteral litralStoreTag;

		protected TrimTextBox txtStoreTag;

		protected CheckBox chkOnlinePay;

		protected CheckBox chkCashOnDelivery;

		protected CheckBox chkOfflinePay;

		protected CheckBox chkIsSupportExpress;

		protected CheckBox chkIsAboveSelf;

		protected CheckBox chkIsStoreDelive;

		protected TrimTextBox txtServeRadius;

		protected TrimTextBox txtStoreFreight;

		protected TrimTextBox txtMinOrderPrice;

		protected HiddenField txtRegionScop;

		protected HiddenField txtRegionScopName;

		protected TrimTextBox txtStoreOpenTimeStartH;

		protected TrimTextBox txtStoreOpenTimeStartM;

		protected TrimTextBox txtStoreOpenTimeEndH;

		protected TrimTextBox txtStoreOpenTimeEndM;

		protected HiddenField hidUploadImages;

		protected HiddenField hidOldImages;

		protected Ueditor editDescription;

		protected Label labStoreUserName;

		protected TrimTextBox txtUserPwd;

		protected TrimTextBox txtUserRePwd;

		protected TrimTextBox txtCommissionRate;

		protected Button btnAdd;

		protected ImageList ImageList;

		protected HiddenField hfCanUpload;

		protected HiddenField hfSiteName;

		protected HiddenField hfLongitude;

		protected HiddenField hfLatitude;

		protected HiddenField hfProvinceCityArea;

		protected HiddenField hidOldRegion;

		protected HiddenField hidOldAddress;

		protected HiddenField hidOldLongitude;

		protected HiddenField hidOldLatitude;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.storeId = base.Request.QueryString["StoreId"].ToInt(0);
			if (this.storeId == 0)
			{
				base.Response.Redirect("StoresList.aspx");
			}
			this.hidStoreId.Value = this.storeId.ToString();
			this.btnAdd.Click += this.btnAdd_Click;
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				this.hfSiteName.Value = masterSettings.SiteName;
				this.BindData();
			}
		}

		public void BindData()
		{
			StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
			ManagerInfo managerInfo = ManagerHelper.FindManagerByStoreId(this.storeId, SystemRoles.StoreAdmin);
			if (storeById == null)
			{
				base.Response.Redirect("StoresList.aspx");
			}
			else
			{
				string empty = string.Empty;
				string empty2 = string.Empty;
				if (!string.IsNullOrEmpty(storeById.WXCategoryName))
				{
					empty = storeById.WXCategoryName.Split(',')[0];
					empty2 = storeById.WXCategoryName.Split(',')[1];
				}
				this.editDescription.Text = storeById.Introduce;
				HiddenField hiddenField = this.hidOldRegion;
				int num = storeById.RegionId;
				hiddenField.Value = num.ToString();
				IEnumerable<string> values = RegionHelper.GetFullRegion(storeById.RegionId, ",", true, 0).Split(',').Take(3);
				this.hfProvinceCityArea.Value = string.Join(",", values);
				HiddenField hiddenField2 = this.hidOldAddress;
				TrimTextBox trimTextBox = this.txtAddress;
				string text3 = hiddenField2.Value = (trimTextBox.Text = string.Join(string.Empty, values) + storeById.Address);
				this.txtContactMan.Text = storeById.ContactMan;
				IList<DeliveryScopeInfo> storeDeliveryScop = StoresHelper.GetStoreDeliveryScop(storeById.StoreId);
				string text4 = "";
				string text5 = "";
				this.chkOfflinePay.Checked = storeById.IsOfflinePay;
				this.chkOnlinePay.Checked = storeById.IsOnlinePay;
				this.chkCashOnDelivery.Checked = storeById.IsCashOnDelivery;
				foreach (DeliveryScopeInfo item in storeDeliveryScop)
				{
					text4 = text4 + item.RegionId + ",";
					text5 = text5 + item.RegionName + ",";
				}
				text4 = text4.TrimEnd(',');
				text5 = text5.TrimEnd(',');
				this.txtRegionScop.Value = text4;
				this.txtRegionScopName.Value = text5;
				this.txtStoresName.Text = storeById.StoreName;
				this.txtTel.Text = storeById.Tel;
				this.labStoreUserName.Text = managerInfo.UserName;
				this.dropRegion.SetSelectedRegionId(storeById.RegionId);
				this.hidOldImages.Value = storeById.StoreImages;
				HiddenField hiddenField3 = this.hidOldLatitude;
				HiddenField hiddenField4 = this.hfLatitude;
				double? nullable = storeById.Latitude;
				object text6;
				double value;
				if (!nullable.HasValue)
				{
					text6 = string.Empty;
				}
				else
				{
					nullable = storeById.Latitude;
					value = nullable.Value;
					text6 = value.ToString();
				}
				text3 = (string)text6;
				hiddenField4.Value = (string)text6;
				hiddenField3.Value = text3;
				HiddenField hiddenField5 = this.hidOldLongitude;
				HiddenField hiddenField6 = this.hfLongitude;
				nullable = storeById.Longitude;
				object text7;
				if (!nullable.HasValue)
				{
					text7 = string.Empty;
				}
				else
				{
					nullable = storeById.Longitude;
					value = nullable.Value;
					text7 = value.ToString();
				}
				text3 = (string)text7;
				hiddenField6.Value = (string)text7;
				hiddenField5.Value = text3;
				if (!string.IsNullOrEmpty(storeById.StoreOpenTime))
				{
					string[] array = storeById.StoreOpenTime.Split('-');
					string[] array2 = array[0].Split(':');
					this.txtStoreOpenTimeStartH.Text = array2[0];
					this.txtStoreOpenTimeStartM.Text = ((array2.Length > 1) ? array2[1] : "");
					if (array.Length > 1)
					{
						string[] array3 = array[1].Split(':');
						this.txtStoreOpenTimeEndH.Text = array3[0];
						this.txtStoreOpenTimeEndM.Text = ((array3.Length > 1) ? array3[1] : "");
					}
				}
				this.chkIsAboveSelf.Checked = storeById.IsAboveSelf;
				this.chkIsSupportExpress.Checked = storeById.IsSupportExpress;
				this.chkIsStoreDelive.Checked = storeById.IsStoreDelive;
				TrimTextBox trimTextBox2 = this.txtServeRadius;
				nullable = storeById.ServeRadius;
				trimTextBox2.Text = nullable.ToString();
				TrimTextBox trimTextBox3 = this.txtMinOrderPrice;
				object text8;
				if (storeById.MinOrderPrice.HasValue)
				{
					num = storeById.MinOrderPrice.ToInt(0);
					text8 = num.ToString();
				}
				else
				{
					text8 = "";
				}
				trimTextBox3.Text = (string)text8;
				TrimTextBox trimTextBox4 = this.txtStoreFreight;
				object text9;
				if (storeById.StoreFreight.HasValue)
				{
					num = storeById.StoreFreight.ToInt(0);
					text9 = num.ToString();
				}
				else
				{
					text9 = "";
				}
				trimTextBox4.Text = (string)text9;
				this.txtCommissionRate.Text = storeById.CommissionRate.ToString();
				IList<int> storeTags = StoresHelper.GetStoreTags(this.storeId);
				this.litralStoreTag.SelectedValue = storeTags;
				if (storeTags != null && storeTags.Count > 0)
				{
					foreach (int item2 in storeTags)
					{
						TrimTextBox trimTextBox5 = this.txtStoreTag;
						trimTextBox5.Text = trimTextBox5.Text + item2.ToString() + ",";
					}
					this.txtStoreTag.Text = this.txtStoreTag.Text.Substring(0, this.txtStoreTag.Text.Length - 1);
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			this.UpdateStores();
			this.ShowMsg("编辑成功", true, "StoresList.aspx");
		}

		private void UpdateStores()
		{
			StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
			ManagerInfo managerInfo = ManagerHelper.FindManagerByStoreId(this.storeId, SystemRoles.StoreAdmin);
			if (storeById == null)
			{
				base.Response.Redirect("StoresList.aspx");
			}
			double num = 0.0;
			int num2 = 0;
			int num3 = 0;
			decimal num4 = default(decimal);
			string text = "";
			string text2 = "";
			string Address = "";
			string text3 = Globals.StripAllTags(this.txtStoresName.Text.Trim());
			Address = Globals.StripAllTags(this.txtAddress.Text);
			string text4 = Globals.StripAllTags(this.txtRegionScop.Value.Trim());
			string text5 = Globals.StripAllTags(this.txtRegionScopName.Value.Trim());
			text = this.txtTel.Text;
			text2 = this.txtContactMan.Text;
			string[] array = text4.Split(',');
			string[] array2 = text5.Split(',');
			IDictionary<int, DeliveryScopeInfo> dictionary = new Dictionary<int, DeliveryScopeInfo>();
			for (int i = 0; i < array.Length; i++)
			{
				int num5 = 0;
				if (int.TryParse(array[i], out num5) && array2.Length >= i && dictionary != null && !dictionary.ContainsKey(num5))
				{
					DeliveryScopeInfo deliveryScopeInfo = new DeliveryScopeInfo();
					deliveryScopeInfo.RegionId = num5;
					deliveryScopeInfo.RegionName = array2[i];
					deliveryScopeInfo.FullRegionPath = RegionHelper.GetFullPath(num5, true);
					dictionary.Add(num5, deliveryScopeInfo);
				}
			}
			if (storeById.StoreName != text3 && StoresHelper.ExistStoreName(text3))
			{
				this.ResetForm(storeById.StoreId);
				this.ShowMsg("门店名称已经存在,请重新输入！", false);
			}
			else if (string.Compare(this.txtUserPwd.Text, this.txtUserRePwd.Text) != 0)
			{
				this.ResetForm(storeById.StoreId);
				this.ShowMsg("请确保两次输入的密码相同", false);
			}
			else if (!this.dropRegion.GetSelectedRegionId().HasValue)
			{
				this.ResetForm(storeById.StoreId);
				this.ShowMsg("请选择店铺所在区域！", false);
			}
			else if (text2.Length > 8 || text2.Length < 2)
			{
				this.ResetForm(storeById.StoreId);
				this.ShowMsg("请输入联系人,联系人长度必须是2-8位！", false);
			}
			else if (string.IsNullOrEmpty(this.hfLatitude.Value) || string.IsNullOrEmpty(this.hfLongitude.Value))
			{
				this.ResetForm(storeById.StoreId);
				this.ShowMsg("请给门店标注定位！", false);
			}
			else if (text == "" || !DataHelper.IsTel(text))
			{
				this.ResetForm(storeById.StoreId);
				this.ShowMsg("请输入正确的联系电话（手机或者固定电话)！", false);
			}
			else
			{
				if (!string.IsNullOrEmpty(this.txtUserPwd.Text))
				{
					if (this.txtUserPwd.Text.Length < 6 || this.txtUserPwd.Text.Length > 20)
					{
						this.ResetForm(storeById.StoreId);
						this.ShowMsg("密码长度必须在6到20个字符之间！", false);
						return;
					}
					managerInfo.Password = Users.EncodePassword(this.txtUserPwd.Text, managerInfo.PasswordSalt);
				}
				if (!this.chkIsSupportExpress.Checked && !this.chkIsAboveSelf.Checked && !this.chkIsStoreDelive.Checked)
				{
					this.ResetForm(storeById.StoreId);
					this.ShowMsg("请选择一种配送方式！", false);
				}
				else
				{
					if (this.chkIsStoreDelive.Checked)
					{
						if (!double.TryParse(this.txtServeRadius.Text.Trim(), out num) || num > 10000.0 || num <= 0.0)
						{
							this.ResetForm(storeById.StoreId);
							this.ShowMsg("请输入正确的配送半径，为大于0至10000之间的数字！", false);
							return;
						}
						if (!int.TryParse(this.txtStoreFreight.Text.Trim(), out num2) || num2 > 99999999 || num2 < 0)
						{
							this.ResetForm(storeById.StoreId);
							this.ShowMsg("请输入正确的配送费", false);
							return;
						}
						if (!int.TryParse(this.txtMinOrderPrice.Text.Trim(), out num3) || num3 > 99999999 || num3 < 0)
						{
							this.ResetForm(storeById.StoreId);
							this.ShowMsg("请输入正确的起送价", false);
							return;
						}
					}
					if (!decimal.TryParse(this.txtCommissionRate.Text.Trim(), out num4) || num4 > 100m || num4 < decimal.Zero)
					{
						this.ResetForm(storeById.StoreId);
						this.ShowMsg("请输入正确的平台抽佣比例", false);
					}
					else if (!this.chkOfflinePay.Checked && !this.chkOnlinePay.Checked && !this.chkCashOnDelivery.Checked)
					{
						this.ResetForm(storeById.StoreId);
						this.ShowMsg("支付方式请至少选择一种", false);
					}
					else
					{
						storeById.IsOfflinePay = this.chkOfflinePay.Checked;
						storeById.IsOnlinePay = this.chkOnlinePay.Checked;
						storeById.IsCashOnDelivery = this.chkCashOnDelivery.Checked;
						if (string.IsNullOrEmpty(this.txtStoreOpenTimeStartH.Text) || this.txtStoreOpenTimeStartH.Text.ToInt(0) < 0 || this.txtStoreOpenTimeStartH.Text.ToInt(0) >= 24)
						{
							this.ResetForm(storeById.StoreId);
							this.ShowMsg("请输入正确的营业起始小时", false);
						}
						else if (string.IsNullOrEmpty(this.txtStoreOpenTimeStartM.Text) || this.txtStoreOpenTimeStartM.Text.ToInt(0) < 0 || this.txtStoreOpenTimeStartM.Text.ToInt(0) >= 60)
						{
							this.ResetForm(storeById.StoreId);
							this.ShowMsg("请输入正确的营业起始分钟", false);
						}
						else if (string.IsNullOrEmpty(this.txtStoreOpenTimeEndH.Text) || this.txtStoreOpenTimeEndH.Text.ToInt(0) < 0 || this.txtStoreOpenTimeEndH.Text.ToInt(0) >= 24)
						{
							this.ResetForm(storeById.StoreId);
							this.ShowMsg("请输入正确的营业结束小时", false);
						}
						else if (string.IsNullOrEmpty(this.txtStoreOpenTimeEndM.Text) || this.txtStoreOpenTimeEndM.Text.ToInt(0) < 0 || this.txtStoreOpenTimeEndM.Text.ToInt(0) >= 60)
						{
							this.ResetForm(storeById.StoreId);
							this.ShowMsg("请输入正确的营业结束分钟", false);
						}
						else
						{
							string empty = string.Empty;
							DateTime dateTime = DateTime.Now;
							string text6 = dateTime.ToString("yyyy-MM-dd");
							DateTime? nullable = (text6 + " " + this.txtStoreOpenTimeStartH.Text.ToInt(0) + ":" + this.txtStoreOpenTimeStartM.Text.ToInt(0)).ToDateTime();
							DateTime? nullable2 = (text6 + " " + this.txtStoreOpenTimeEndH.Text.ToInt(0) + ":" + this.txtStoreOpenTimeEndM.Text.ToInt(0)).ToDateTime();
							dateTime = nullable.Value;
							string str = dateTime.ToString("HH:mm");
							dateTime = nullable2.Value;
							string text7 = dateTime.ToString("HH:mm");
							if (text7 == "00:00")
							{
								nullable2 = (text6 + " 23:59").ToDateTime();
								text7 = "23:59";
							}
							empty = (storeById.StoreOpenTime = str + "-" + text7);
							storeById.OpenStartDate = nullable.Value;
							storeById.OpenEndDate = nullable2.Value;
							storeById.IsSupportExpress = (this.chkIsSupportExpress.Checked && true);
							storeById.IsAboveSelf = (this.chkIsAboveSelf.Checked && true);
							storeById.IsStoreDelive = (this.chkIsStoreDelive.Checked && true);
							storeById.Introduce = this.editDescription.Text;
							if (this.chkIsStoreDelive.Checked)
							{
								storeById.ServeRadius = num;
								storeById.MinOrderPrice = num3;
								storeById.StoreFreight = num2;
							}
							else
							{
								storeById.ServeRadius = 0.0;
								storeById.MinOrderPrice = null;
								storeById.StoreFreight = null;
							}
							storeById.CommissionRate = num4;
							List<string> list = RegionHelper.GetFullRegion(this.dropRegion.GetSelectedRegionId().Value, ",", true, 0).Split(',').Take(3)
								.ToList();
							list.ForEach(delegate(string c)
							{
								Address = Address.Replace(c, string.Empty);
							});
							IList<string> list2 = new List<string>();
							string text9 = this.hidUploadImages.Value.Trim();
							string[] array3 = text9.Split(',');
							for (int j = 0; j < array3.Length; j++)
							{
								if (!string.IsNullOrEmpty(array3[j]))
								{
									string text10 = Globals.SaveFile("depot", array3[j], "/Storage/master/", true, false, "");
									string text11 = base.Request.MapPath(text10);
									string virtualPath = HiContext.Current.GetStoragePath() + "/depot/thum_" + text10.Substring(text10.LastIndexOf("/") + 1);
									if (!File.Exists(text11))
									{
										this.ShowMsg("缩略图文件夹未创建,请联系管理员", false);
										return;
									}
									ResourcesHelper.CreateThumbnail(text11, base.Request.MapPath(virtualPath), 160, 160);
									list2.Add(text10);
								}
							}
							if (list2.Count == 0)
							{
								this.ResetForm(storeById.StoreId);
								this.ShowMsg("logo已失效或未上传,请上传门店logo", false);
							}
							else if (ManagerHelper.Update(managerInfo))
							{
								int value = this.dropRegion.GetSelectedRegionId().Value;
								storeById.StoreName = text3;
								storeById.RegionId = value;
								storeById.TopRegionId = RegionHelper.GetCityId(value);
								storeById.Tel = text;
								storeById.Address = Address;
								storeById.ContactMan = text2;
								storeById.StoreImages = ((list2.Count == 0) ? string.Empty : string.Join(",", list2.ToArray()));
								storeById.Longitude = Math.Round(double.Parse(string.IsNullOrEmpty(this.hfLongitude.Value) ? "0" : this.hfLongitude.Value), 6);
								storeById.Latitude = Math.Round(double.Parse(string.IsNullOrEmpty(this.hfLatitude.Value) ? "0" : this.hfLatitude.Value), 6);
								storeById.FullRegionPath = RegionHelper.GetFullPath(value, true);
								StoresHelper.UpdateStore(storeById);
								HiCache.Remove("DataCache-StoreInfoDataKey");
								if (dictionary.Count > 0 && this.chkIsStoreDelive.Checked)
								{
									StoresHelper.AddDeliveryScope(this.storeId, dictionary);
								}
								else
								{
									StoresHelper.DeleteDevlieryScope(this.storeId);
								}
								if (!string.IsNullOrEmpty(this.txtStoreTag.Text.Trim()))
								{
									IList<int> list3 = new List<int>();
									string text12 = this.txtStoreTag.Text.Trim();
									string[] array4 = null;
									array4 = ((!text12.Contains(",")) ? new string[1]
									{
										text12
									} : text12.Split(','));
									string[] array5 = array4;
									foreach (string value2 in array5)
									{
										list3.Add(Convert.ToInt32(value2));
									}
									StoresHelper.DeleteStoreTags(this.storeId);
									StoresHelper.BindStoreTags(this.storeId, list3);
								}
								else
								{
									StoresHelper.DeleteStoreTags(this.storeId);
								}
								HiCache.Remove($"DataCache-StoreStateCacheKey-{storeById.StoreId}");
								SiteSettings masterSettings = SettingsManager.GetMasterSettings();
								if (masterSettings.OpenDadaLogistics)
								{
									DataTable dataTable = DepotHelper.SynchroDadaStoreList(this.storeId);
									if (dataTable != null && dataTable.Rows.Count > 0)
									{
										for (int l = 0; l < dataTable.Rows.Count; l++)
										{
											string station_name = dataTable.Rows[l]["StoreName"].ToNullString();
											int business = 5;
											string city_name = dataTable.Rows[l]["CityName"].ToNullString().Replace("市", "");
											string area_name = dataTable.Rows[l]["RegionName"].ToNullString();
											string station_address = dataTable.Rows[l]["Address"].ToNullString();
											double lng = dataTable.Rows[l]["Longitude"].ToDouble(0);
											double lat = dataTable.Rows[l]["Latitude"].ToDouble(0);
											string contact_name = dataTable.Rows[l]["ContactMan"].ToNullString();
											string phone = dataTable.Rows[l]["Tel"].ToNullString();
											string text13 = dataTable.Rows[l]["StoreId"].ToNullString();
											string text14 = DadaHelper.shopUpdate(masterSettings.DadaSourceID, this.storeId.ToString(), "", station_name, business, city_name, area_name, station_address, lng, lat, contact_name, phone, -1);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public void ResetForm(int StoreID)
		{
			IList<DeliveryScopeInfo> storeDeliveryScop = StoresHelper.GetStoreDeliveryScop(StoreID);
			string text = "";
			string text2 = "";
			foreach (DeliveryScopeInfo item in storeDeliveryScop)
			{
				text = text + item.RegionId + ",";
				text2 = text2 + item.RegionName + ",";
			}
			text = text.TrimEnd(',');
			text2 = text2.TrimEnd(',');
			this.txtRegionScop.Value = text;
			this.txtRegionScopName.Value = text2;
		}

		protected void btnAddToWXStores_Click(object sender, EventArgs e)
		{
			this.UpdateStores();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(masterSettings.WeixinAppId) && !string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				this.ShowMsg("编辑成功并同步至微信门店", true, "EditWXStores.aspx?storeId=" + this.storeId);
			}
			else
			{
				this.ShowMsg("编辑成功,请配置微商城", true);
			}
		}
	}
}
