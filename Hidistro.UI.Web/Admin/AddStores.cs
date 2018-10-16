using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AddStores)]
	public class AddStores : AdminPage
	{
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

		protected TrimTextBox txtUserName;

		protected TrimTextBox txtUserPwd;

		protected TrimTextBox txtUserRePwd;

		protected TrimTextBox txtCommissionRate;

		protected Button btnAdd;

		protected ImageList ImageList;

		protected HiddenField hfLongitude;

		protected HiddenField hfLatitude;

		protected HiddenField hfProvinceCityArea;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnAdd_Click;
			if (!base.IsPostBack && HiContext.Current.SiteSettings.OpenVstore != 0)
			{
				string text = "";
				if (HiContext.Current.Manager != null)
				{
					text = HiContext.Current.Manager.ManagerId.ToString();
				}
				string text2 = Globals.FullPath("/Vshop/GetOpenId.aspx?adminName=" + text);
				string text3 = "/Storage/master/QRCode/" + HiContext.Current.SiteSettings.SiteUrl.ToLower().Replace("https://", "").Replace("http://", "")
					.Replace(".", "") + "_" + text + "_OpenId.png";
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			int num = 0;
			this.SaveStores(out num);
			if (num > 0)
			{
				this.ShowMsg("添加成功", true, "StoresList.aspx");
			}
		}

		private void SaveStores(out int storeId)
		{
			storeId = 0;
			CreateStoreData createStoreData = new CreateStoreData();
			ManagerInfo managerInfo = new ManagerInfo();
			StoresInfo storesInfo = new StoresInfo();
			double num = 0.0;
			int num2 = 0;
			int num3 = 0;
			decimal num4 = default(decimal);
			string text = "";
			string text2 = "";
			string Address = "";
			string userName = DataHelper.CleanSearchString(this.txtUserName.Text.Trim());
			string storeName = Globals.StripAllTags(this.txtStoresName.Text.Trim());
			Address = Globals.StripAllTags(this.txtAddress.Text);
			string text3 = Globals.StripAllTags(this.txtRegionScop.Value.Trim());
			string text4 = Globals.StripAllTags(this.txtRegionScopName.Value.Trim());
			text = this.txtTel.Text;
			text2 = Globals.StripAllTags(this.txtContactMan.Text);
			if (StoresHelper.ExistStoreName(storeName))
			{
				this.ResetForm();
				this.ShowMsg("门店名称已经存在,请重新输入！", false);
			}
			else if (Hidistro.SaleSystem.Store.ManagerHelper.FindManagerByUsername(userName) != null)
			{
				this.ResetForm();
				this.ShowMsg("用户名已经存在,请重新输入！", false);
			}
			else if (this.txtUserPwd.Text.Length < 6 || this.txtUserPwd.Text.Length > 20)
			{
				this.ResetForm();
				this.ShowMsg("密码不能为空！", false);
			}
			else if (string.Compare(this.txtUserPwd.Text, this.txtUserRePwd.Text) != 0)
			{
				this.ResetForm();
				this.ShowMsg("请确保两次输入的密码相同", false);
			}
			else if (!this.dropRegion.GetSelectedRegionId().HasValue)
			{
				this.ResetForm();
				this.ShowMsg("请选择店铺所在区域！", false);
			}
			else if (text2.Length > 8 || text2.Length < 2)
			{
				this.ResetForm();
				this.ShowMsg("请输入联系人,联系人长度必须是2-8位！", false);
			}
			else if (string.IsNullOrEmpty(this.hfLatitude.Value) || string.IsNullOrEmpty(this.hfLongitude.Value))
			{
				this.ResetForm();
				this.ShowMsg("请给门店标注定位！", false);
			}
			else if (text == "" || !DataHelper.IsTel(text))
			{
				this.ResetForm();
				this.ShowMsg("请输入正确的联系电话（手机或者固定电话)！", false);
			}
			else if (!this.chkIsSupportExpress.Checked && !this.chkIsAboveSelf.Checked && !this.chkIsStoreDelive.Checked)
			{
				this.ResetForm();
				this.ShowMsg("请选择一种配送方式！", false);
			}
			else
			{
				if (this.chkIsStoreDelive.Checked)
				{
					if (!double.TryParse(this.txtServeRadius.Text.Trim(), out num) || num > 10000.0 || num <= 0.0)
					{
						this.ResetForm();
						this.ShowMsg("请输入正确的配送半径，为大于0至10000之间的数字！", false);
						return;
					}
					if (!int.TryParse(this.txtStoreFreight.Text.Trim(), out num2) || num2 > 99999999 || num2 < 0)
					{
						this.ResetForm();
						this.ShowMsg("请输入正确的配送费", false);
						return;
					}
					if (!int.TryParse(this.txtMinOrderPrice.Text.Trim(), out num3) || num3 > 99999999 || num3 < 0)
					{
						this.ResetForm();
						this.ShowMsg("请输入正确的起送价", false);
						return;
					}
				}
				if (!decimal.TryParse(this.txtCommissionRate.Text.Trim(), out num4) || num4 > 100m || num4 < decimal.Zero)
				{
					this.ResetForm();
					this.ShowMsg("请输入正确的平台抽佣比例", false);
				}
				else if (!this.chkOfflinePay.Checked && !this.chkOnlinePay.Checked && !this.chkCashOnDelivery.Checked)
				{
					this.ResetForm();
					this.ShowMsg("支付方式请至少选择一种", false);
				}
				else
				{
					storesInfo.IsOfflinePay = this.chkOfflinePay.Checked;
					storesInfo.IsOnlinePay = this.chkOnlinePay.Checked;
					storesInfo.IsCashOnDelivery = this.chkCashOnDelivery.Checked;
					if (string.IsNullOrEmpty(this.txtStoreOpenTimeStartH.Text) || this.txtStoreOpenTimeStartH.Text.ToInt(0) < 0 || this.txtStoreOpenTimeStartH.Text.ToInt(0) >= 24)
					{
						this.ResetForm();
						this.ShowMsg("请输入正确的营业起始小时", false);
					}
					else if (string.IsNullOrEmpty(this.txtStoreOpenTimeStartM.Text) || this.txtStoreOpenTimeStartM.Text.ToInt(0) < 0 || this.txtStoreOpenTimeStartM.Text.ToInt(0) >= 60)
					{
						this.ResetForm();
						this.ShowMsg("请输入正确的营业起始分钟", false);
					}
					else if (string.IsNullOrEmpty(this.txtStoreOpenTimeEndH.Text) || this.txtStoreOpenTimeEndH.Text.ToInt(0) < 0 || this.txtStoreOpenTimeEndH.Text.ToInt(0) >= 24)
					{
						this.ResetForm();
						this.ShowMsg("请输入正确的营业结束小时", false);
					}
					else if (string.IsNullOrEmpty(this.txtStoreOpenTimeEndM.Text) || this.txtStoreOpenTimeEndM.Text.ToInt(0) < 0 || this.txtStoreOpenTimeEndM.Text.ToInt(0) >= 60)
					{
						this.ResetForm();
						this.ShowMsg("请输入正确的营业结束分钟", false);
					}
					else
					{
						string empty = string.Empty;
						DateTime dateTime = DateTime.Now;
						string text5 = dateTime.ToString("yyyy-MM-dd");
						DateTime? nullable = (text5 + " " + this.txtStoreOpenTimeStartH.Text.ToInt(0) + ":" + this.txtStoreOpenTimeStartM.Text.ToInt(0)).ToDateTime();
						DateTime? nullable2 = (text5 + " " + this.txtStoreOpenTimeEndH.Text.ToInt(0) + ":" + this.txtStoreOpenTimeEndM.Text.ToInt(0)).ToDateTime();
						dateTime = nullable.Value;
						string str = dateTime.ToString("HH:mm");
						dateTime = nullable2.Value;
						string text6 = dateTime.ToString("HH:mm");
						if (text6 == "00:00")
						{
							nullable2 = (text5 + " 23:59").ToDateTime();
							text6 = "23:59";
						}
						empty = (storesInfo.StoreOpenTime = str + "-" + text6);
						storesInfo.OpenStartDate = nullable.Value;
						storesInfo.OpenEndDate = nullable2.Value;
						storesInfo.IsSupportExpress = (this.chkIsSupportExpress.Checked && true);
						storesInfo.IsAboveSelf = (this.chkIsAboveSelf.Checked && true);
						storesInfo.IsStoreDelive = (this.chkIsStoreDelive.Checked && true);
						storesInfo.Introduce = this.editDescription.Text;
						storesInfo.ServeRadius = num;
						if (this.chkIsStoreDelive.Checked)
						{
							storesInfo.MinOrderPrice = num3;
							storesInfo.StoreFreight = num2;
						}
						storesInfo.CommissionRate = num4;
						storesInfo.IsShelvesProduct = true;
						storesInfo.IsModifyPrice = true;
						storesInfo.IsRequestBlance = true;
						storesInfo.MinPriceRate = 0.5.ToDecimal(0);
						storesInfo.MaxPriceRate = 2;
						string[] array = text3.Split(',');
						string[] array2 = text4.Split(',');
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
						managerInfo.RoleId = -1;
						managerInfo.UserName = this.txtUserName.Text.Trim();
						string text8 = this.txtUserPwd.Text;
						managerInfo.CreateDate = DateTime.Now;
						string text9 = Globals.RndStr(128, true);
						text8 = (managerInfo.Password = Users.EncodePassword(text8, text9));
						managerInfo.PasswordSalt = text9;
						storesInfo.StoreName = storeName;
						storesInfo.CloseStatus = true;
						storesInfo.State = 1;
						storesInfo.RegionId = this.dropRegion.GetSelectedRegionId().Value;
						storesInfo.TopRegionId = RegionHelper.GetCityId(storesInfo.RegionId);
						storesInfo.Tel = text;
						List<string> list = RegionHelper.GetFullRegion(this.dropRegion.GetSelectedRegionId().Value, ",", true, 0).Split(',').Take(3)
							.ToList();
						list.ForEach(delegate(string c)
						{
							Address = Address.Replace(c, string.Empty);
						});
						storesInfo.Address = Address;
						storesInfo.ContactMan = text2;
						IList<string> list2 = new List<string>();
						string text11 = this.hidUploadImages.Value.Trim();
						string[] array3 = text11.Split(',');
						for (int j = 0; j < array3.Length; j++)
						{
							if (!string.IsNullOrEmpty(array3[j]))
							{
								string text12 = Globals.SaveFile("depot", array3[j], "/Storage/master/", true, false, "");
								string sourceFilename = base.Request.MapPath(text12);
								string virtualPath = HiContext.Current.GetStoragePath() + "/depot/thum_" + text12.Substring(text12.LastIndexOf("/") + 1);
								ResourcesHelper.CreateThumbnail(sourceFilename, base.Request.MapPath(virtualPath), 160, 160);
								list2.Add(text12);
							}
						}
						if (list2.Count == 0)
						{
							this.ResetForm();
							this.ShowMsg("请上传门店logo", false);
						}
						else
						{
							storesInfo.StoreImages = ((list2.Count == 0) ? string.Empty : string.Join(",", list2.ToArray()));
							if (!string.IsNullOrEmpty(this.hfLatitude.Value))
							{
								storesInfo.Latitude = double.Parse(this.hfLatitude.Value);
							}
							if (!string.IsNullOrEmpty(this.hfLongitude.Value))
							{
								storesInfo.Longitude = double.Parse(this.hfLongitude.Value);
							}
							storesInfo.FullRegionPath = RegionHelper.GetFullPath(storesInfo.RegionId, true);
							storeId = StoresHelper.AddStore(storesInfo);
							if (storeId > 0)
							{
								HiCache.Remove("DataCache-StoreInfoDataKey");
								managerInfo.StoreId = storeId;
								Hidistro.SaleSystem.Store.ManagerHelper.Create(managerInfo);
								if (dictionary.Count > 0 && this.chkIsStoreDelive.Checked)
								{
									StoresHelper.AddDeliveryScope(storeId, dictionary);
								}
								if (!string.IsNullOrEmpty(this.txtStoreTag.Text.Trim()))
								{
									IList<int> list3 = new List<int>();
									string text13 = this.txtStoreTag.Text.Trim();
									string[] array4 = null;
									array4 = ((!text13.Contains(",")) ? new string[1]
									{
										text13
									} : text13.Split(','));
									string[] array5 = array4;
									foreach (string value in array5)
									{
										list3.Add(Convert.ToInt32(value));
									}
									StoresHelper.BindStoreTags(storeId, list3);
								}
								SiteSettings masterSettings = SettingsManager.GetMasterSettings();
								if (masterSettings.OpenDadaLogistics)
								{
									DataTable dataTable = DepotHelper.SynchroDadaStoreList(storeId);
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
											string origin_shop_id = dataTable.Rows[l]["StoreId"].ToNullString();
											string text14 = DadaHelper.shopAdd(masterSettings.DadaSourceID, station_name, business, city_name, area_name, station_address, lng, lat, contact_name, phone, origin_shop_id, "", "", "");
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private bool ManagerHelper(string UserName)
		{
			throw new NotImplementedException();
		}

		public void ResetForm()
		{
			this.txtRegionScop.Value = "";
			this.txtRegionScopName.Value = "";
		}

		protected void btnAddToWXStores_Click(object sender, EventArgs e)
		{
			int num = 0;
			this.SaveStores(out num);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			if (!string.IsNullOrEmpty(masterSettings.WeixinAppId) && !string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				this.ShowMsg("添加成功并同步至微信门店", true, "AddWXStores.aspx?storeId=" + num);
			}
			else
			{
				this.ShowMsg("添加成功,请配置微商城", true);
			}
		}
	}
}
