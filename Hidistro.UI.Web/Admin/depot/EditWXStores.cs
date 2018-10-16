using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs.Poi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class EditWXStores : AdminPage
	{
		private int storeId = 0;

		protected ScriptManager ScriptManager1;

		protected TrimTextBox txtWxAddress;

		protected UpdatePanel UpdatePanel1;

		protected DropDownList ddlCategoryParent;

		protected DropDownList ddlCategoryChild;

		protected TrimTextBox txtWXTelephone;

		protected TrimTextBox txtWXAvgPrice;

		protected TrimTextBox txtWXOpenTime;

		protected TrimTextBox txtWXRecommend;

		protected TrimTextBox txtWXSpecial;

		protected TrimTextBox txtWXIntroduction;

		protected Button btnSave;

		protected HiddenField hfLongitude;

		protected HiddenField hfLatitude;

		protected HiddenField hfProvinceCityArea;

		protected HiddenField hfDistrict;

		protected HiddenField hidUploadImages;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.storeId = base.Request.QueryString["StoreId"].ToInt(0);
			if (this.storeId == 0)
			{
				base.Response.Redirect("StoresList.aspx");
			}
			if (!base.IsPostBack)
			{
				this.BindCategoryParent();
				this.BindCategoryChild();
				this.BindStoreData();
			}
		}

		protected void ddlCategoryParent_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.BindCategoryChild();
		}

		private void BindCategoryParent()
		{
			IEnumerable<WXStoreHelper.WXCategory> category = WXStoreHelper.GetCategory();
			this.ddlCategoryParent.AutoPostBack = true;
			this.ddlCategoryParent.DataTextField = "FirstCategoryName";
			this.ddlCategoryParent.DataValueField = "FirstCategoryName";
			this.ddlCategoryParent.DataSource = (from c in category
			select new
			{
				c.FirstCategoryName
			}).Distinct().ToList();
			this.ddlCategoryParent.DataBind();
		}

		private void BindCategoryChild()
		{
			IEnumerable<WXStoreHelper.WXCategory> category = WXStoreHelper.GetCategory();
			this.ddlCategoryChild.DataTextField = "SecondCategoryName";
			this.ddlCategoryChild.DataValueField = "SecondCategoryName";
			this.ddlCategoryChild.DataSource = (from c in category
			where c.FirstCategoryName == this.ddlCategoryParent.SelectedValue
			select new
			{
				c.SecondCategoryName
			}).Distinct().ToList();
			this.ddlCategoryChild.DataBind();
		}

		private void BindStoreData()
		{
			StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
			if (storeById == null)
			{
				base.Response.Redirect("StoresList.aspx");
			}
			else
			{
				IEnumerable<string> values = RegionHelper.GetFullRegion(storeById.RegionId, ",", true, 0).Split(',').Take(3);
				this.hfProvinceCityArea.Value = string.Join(",", values);
				this.txtWxAddress.Text = string.Join(string.Empty, values) + storeById.Address;
				this.hidUploadImages.Value = (string.IsNullOrEmpty(storeById.StoreImages) ? string.Empty : storeById.StoreImages);
				HiddenField hiddenField = this.hfLatitude;
				object value;
				double value2;
				if (!storeById.Latitude.HasValue)
				{
					value = string.Empty;
				}
				else
				{
					value2 = storeById.Latitude.Value;
					value = value2.ToString();
				}
				hiddenField.Value = (string)value;
				HiddenField hiddenField2 = this.hfLongitude;
				object value3;
				if (!storeById.Longitude.HasValue)
				{
					value3 = string.Empty;
				}
				else
				{
					value2 = storeById.Longitude.Value;
					value3 = value2.ToString();
				}
				hiddenField2.Value = (string)value3;
				TrimTextBox trimTextBox = this.txtWXAvgPrice;
				int? wXAvgPrice = storeById.WXAvgPrice;
				object text;
				if (!wXAvgPrice.HasValue)
				{
					text = string.Empty;
				}
				else
				{
					wXAvgPrice = storeById.WXAvgPrice;
					text = wXAvgPrice.ToString();
				}
				trimTextBox.Text = (string)text;
				this.txtWXIntroduction.Text = (string.IsNullOrEmpty(storeById.WXIntroduction) ? string.Empty : storeById.WXIntroduction);
				this.txtWXOpenTime.Text = (string.IsNullOrEmpty(storeById.WXOpenTime) ? string.Empty : storeById.WXOpenTime);
				this.txtWXRecommend.Text = (string.IsNullOrEmpty(storeById.WXRecommend) ? string.Empty : storeById.WXRecommend);
				this.txtWXSpecial.Text = (string.IsNullOrEmpty(storeById.WXSpecial) ? string.Empty : storeById.WXSpecial);
				this.txtWXTelephone.Text = (string.IsNullOrEmpty(storeById.WXTelephone) ? storeById.Tel : storeById.WXTelephone);
				string text2 = string.IsNullOrEmpty(storeById.WXCategoryName) ? string.Empty : storeById.WXCategoryName;
				if (!string.IsNullOrEmpty(text2))
				{
					string selectedValue = text2.Split(',')[0];
					string selectedValue2 = text2.Split(',')[1];
					this.ddlCategoryParent.SelectedValue = selectedValue;
					this.BindCategoryChild();
					this.ddlCategoryChild.SelectedValue = selectedValue2;
				}
			}
		}

		private IEnumerable<string> BuildImages(StringBuilder sbLocalImage, IList<string> lstFile)
		{
			string text = this.hidUploadImages.Value.Trim();
			string[] array = text.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					string item = Globals.SaveFile("depot", array[i], "/Storage/master/", true, false, "");
					lstFile.Add(item);
				}
			}
			sbLocalImage.Append(string.Join(",", lstFile.ToArray()));
			return WXStoreHelper.ImageUploadForStore(lstFile);
		}

		private void Save()
		{
			StringBuilder sbLocalImage = new StringBuilder();
			IList<string> list = new List<string>();
			StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
			if (!storeById.WXState.HasValue)
			{
				this.AddWXStore();
			}
			else if (storeById.WXState.HasValue && storeById.WXState.Value.Equals(WXStoreState.Pass))
			{
				IEnumerable<string> enumerable = this.BuildImages(sbLocalImage, list);
				UpdateStoreData updateStoreData = new UpdateStoreData();
				foreach (string item in enumerable)
				{
					updateStoreData.business.base_info.photo_list.Add(new Store_Photo
					{
						photo_url = item
					});
				}
				updateStoreData.business.base_info.avg_price = this.txtWXAvgPrice.Text.ToInt(0);
				updateStoreData.business.base_info.introduction = this.txtWXIntroduction.Text.Trim();
				updateStoreData.business.base_info.open_time = this.txtWXOpenTime.Text.Trim();
				updateStoreData.business.base_info.recommend = this.txtWXRecommend.Text.Trim();
				updateStoreData.business.base_info.poi_id = (storeById.WXPoiId.HasValue ? storeById.WXPoiId.Value.ToString() : string.Empty);
				updateStoreData.business.base_info.special = this.txtWXSpecial.Text.Trim();
				updateStoreData.business.base_info.telephone = this.txtWXTelephone.Text.Trim();
				WXStoreHelper.UpdateWXStore(updateStoreData);
				storeById.StoreImages = ((list.Count == 0) ? string.Empty : string.Join(",", list.ToArray()));
				StoresHelper.UpdateWXStore(updateStoreData, storeById.StoreId, storeById.StoreImages);
			}
			else
			{
				this.ShowMsg("微信门店审核中不能修改", false);
			}
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtWxAddress.Text.Trim()) || this.txtWxAddress.Text.Trim().Length > 50)
			{
				this.ShowMsg("微信门店地址不能为空长度必须为2-50个字符", false);
			}
			else
			{
				string text = $"{this.ddlCategoryParent.SelectedValue.Trim()},{this.ddlCategoryChild.SelectedValue.Trim()}";
				if (string.IsNullOrEmpty(text) || text.Length > 50)
				{
					this.ShowMsg("类目不能为空,长度必须为2-50个字符", false);
				}
				else if (string.IsNullOrEmpty(this.txtWXTelephone.Text.Trim()) || this.txtWXTelephone.Text.Trim().Length > 25)
				{
					this.ShowMsg("电话不能为空,长度必须为2-50个字符", false);
				}
				else if (!string.IsNullOrEmpty(this.txtWXAvgPrice.Text.Trim()) && this.txtWXAvgPrice.Text.Trim().ToInt(0) <= 0)
				{
					this.ShowMsg("人均价格必须大于零的整数，须如实填写，默认单位为人民币", false);
				}
				else
				{
					string text2 = this.txtWXOpenTime.Text.Trim();
					if (!string.IsNullOrEmpty(text2) && text2.Split('-').Length != 2)
					{
						this.ShowMsg("营业时间如，10:00-21:00", false);
					}
					else
					{
						if (!string.IsNullOrEmpty(text2))
						{
							DateTime minValue = DateTime.MinValue;
							string[] array = text2.Split('-');
							if (!DateTime.TryParse(array[0], out minValue) || !DateTime.TryParse(array[1], out minValue))
							{
								this.ShowMsg("营业时间如，10:00-21:00", false);
								return;
							}
						}
						if (!string.IsNullOrEmpty(this.txtWXRecommend.Text.Trim()) && this.txtWXRecommend.Text.Trim().Length > 150)
						{
							this.ShowMsg("推荐长度必须为2-150个字符", false);
						}
						else if (!string.IsNullOrEmpty(this.txtWXSpecial.Text.Trim()) && this.txtWXSpecial.Text.Trim().Length > 150)
						{
							this.ShowMsg("特色服务必须为2-150个字符", false);
						}
						else if (!string.IsNullOrEmpty(this.txtWXIntroduction.Text.Trim()) && this.txtWXIntroduction.Text.Trim().Length > 150)
						{
							this.ShowMsg("简介必须为2-150个字符", false);
						}
						else
						{
							this.Save();
						}
					}
				}
			}
		}

		private void AddWXStore()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
			IList<string> list = new List<string>();
			CreateStoreData createStoreData = new CreateStoreData();
			string text = this.hidUploadImages.Value.Trim();
			string[] array = text.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (!string.IsNullOrEmpty(array[i]))
				{
					string item = Globals.SaveFile("depot", array[i], "/Storage/master/", true, false, "");
					list.Add(item);
				}
			}
			IEnumerable<string> enumerable = WXStoreHelper.ImageUploadForStore(list);
			foreach (string item2 in enumerable)
			{
				createStoreData.business.base_info.photo_list.Add(new Store_Photo
				{
					photo_url = item2
				});
			}
			string address = this.txtWxAddress.Text.Trim();
			List<string> list2 = RegionHelper.GetFullRegion(storeById.RegionId, ",", true, 0).Split(',').Take(3)
				.ToList();
			list2.ForEach(delegate(string c)
			{
				address = address.Replace(c, string.Empty);
			});
			createStoreData.business.base_info.address = address;
			createStoreData.business.base_info.avg_price = this.txtWXAvgPrice.Text.ToInt(0);
			createStoreData.business.base_info.branch_name = storeById.StoreName;
			createStoreData.business.base_info.business_name = masterSettings.SiteName;
			createStoreData.business.base_info.categories = new string[1]
			{
				$"{this.ddlCategoryParent.SelectedValue.Trim()},{this.ddlCategoryChild.SelectedValue.Trim()}"
			};
			createStoreData.business.base_info.city = list2[1];
			createStoreData.business.base_info.district = this.hfDistrict.Value;
			createStoreData.business.base_info.introduction = this.txtWXIntroduction.Text.Trim();
			createStoreData.business.base_info.latitude = this.hfLatitude.Value;
			createStoreData.business.base_info.longitude = this.hfLongitude.Value;
			createStoreData.business.base_info.offset_type = 1;
			createStoreData.business.base_info.open_time = this.txtWXOpenTime.Text.Trim();
			createStoreData.business.base_info.province = list2[0];
			createStoreData.business.base_info.recommend = this.txtWXRecommend.Text.Trim();
			createStoreData.business.base_info.special = this.txtWXSpecial.Text.Trim();
			createStoreData.business.base_info.telephone = this.txtWXTelephone.Text.Trim();
			storeById.StoreImages = ((list.Count == 0) ? string.Empty : string.Join(",", list.ToArray()));
			createStoreData.business.base_info.sid = this.storeId.ToString();
			WxJsonResult wxJsonResult = WXStoreHelper.CreateWXStore(createStoreData);
			if (wxJsonResult.errcode.Equals(ReturnCode.请求成功))
			{
				StoresHelper.FirstAddWXStore(createStoreData, this.storeId, storeById.StoreImages);
			}
			this.ShowMsg("添加成功,等待微信审核", true, "StoresList.aspx");
		}
	}
}
