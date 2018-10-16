using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Depot
{
	public class EditStores : StoreAdminPage
	{
		protected TextBox txtUserName;

		protected TextBox txtUserPwd;

		protected TextBox txtUserRePwd;

		protected TextBox txtStoresName;

		protected RegionSelector dropRegion;

		protected TextBox txtAddress;

		protected TextBox txtContactMan;

		protected TextBox txtTel;

		protected Ueditor editDescription;

		protected HtmlGenericControl liStoreTag;

		protected Label lblStoreTag;

		protected Label lblDeliveMode;

		protected HtmlGenericControl liServeRadius;

		protected Label lblServeRadius;

		protected HtmlGenericControl liStoreFreight;

		protected Label lblStoreFreight;

		protected HtmlGenericControl liMinOrderPrice;

		protected Label lblMinOrderPrice;

		protected Label lblStoreOpenTime;

		protected TextBox txtWxOpenId;

		protected HtmlAnchor reGetOpenId;

		protected HtmlGenericControl getOpenId;

		protected Image OpenIdQrCodeImg;

		protected HtmlGenericControl liScopTitle;

		protected HiddenField txtRegionScop;

		protected HiddenField txtRegionScopName;

		protected HtmlGenericControl liScop;

		protected Button btnAdd;

		protected ImageList ImageList;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnAdd.Click += this.btnAdd_Click;
			if (!this.Page.IsPostBack)
			{
				if (HiContext.Current.SiteSettings.OpenVstore == 0)
				{
					this.getOpenId.Style.Add("display", "none");
				}
				else
				{
					string text = "";
					if (HiContext.Current.Manager != null)
					{
						text = HiContext.Current.Manager.ManagerId.ToString();
					}
					string content = Globals.FullPath("/Vshop/GetOpenId.aspx?adminName=" + text);
					string qrCodeUrl = "/Storage/master/QRCode/" + HiContext.Current.SiteSettings.SiteUrl.ToLower().Replace("https://", "").Replace("http://", "")
						.Replace(".", "") + "_" + text + "_OpenId.png";
					this.OpenIdQrCodeImg.ImageUrl = Globals.CreateQRCode(content, qrCodeUrl, false, ImageFormats.Png);
				}
				this.BindData();
			}
		}

		public void BindData()
		{
			ManagerInfo manager = HiContext.Current.Manager;
			StoresInfo storeById = StoresHelper.GetStoreById(manager.StoreId);
			this.txtAddress.Text = storeById.Address;
			this.txtContactMan.Text = storeById.ContactMan;
			IList<DeliveryScopeInfo> storeDeliveryScop = StoresHelper.GetStoreDeliveryScop(storeById.StoreId);
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
			this.txtStoresName.Text = storeById.StoreName;
			this.txtTel.Text = storeById.Tel;
			this.txtUserName.Text = manager.UserName;
			this.txtUserName.Enabled = false;
			this.dropRegion.SetSelectedRegionId(storeById.RegionId);
			if (string.IsNullOrEmpty(storeById.WxOpenId))
			{
				this.reGetOpenId.Style.Add("display", "none");
			}
			else
			{
				this.getOpenId.Style.Add("display", "none");
			}
			this.txtWxOpenId.Text = storeById.WxOpenId;
			IList<string> storeTagNames = StoresHelper.GetStoreTagNames(manager.StoreId);
			string text3 = "";
			foreach (string item2 in storeTagNames)
			{
				text3 = text3 + item2 + "&nbsp;&nbsp;";
			}
			this.lblStoreTag.Text = text3;
			if (text3 == "")
			{
				this.liStoreTag.Visible = false;
			}
			string text4 = "";
			if (storeById.IsSupportExpress)
			{
				text4 += "快递配送&nbsp;&nbsp;";
			}
			if (storeById.IsAboveSelf)
			{
				text4 += "上门自提&nbsp;&nbsp;";
			}
			if (storeById.IsStoreDelive)
			{
				text4 += "门店配送&nbsp;&nbsp;";
			}
			else
			{
				this.liServeRadius.Visible = false;
				this.liStoreFreight.Visible = false;
				this.liMinOrderPrice.Visible = false;
				this.liScopTitle.Visible = false;
				this.liScop.Visible = false;
			}
			this.lblDeliveMode.Text = text4;
			this.editDescription.Text = storeById.Introduce;
			this.lblServeRadius.Text = storeById.ServeRadius.ToString() + "&nbsp;KM（公里）";
			this.lblMinOrderPrice.Text = storeById.MinOrderPrice.ToDecimal(0).F2ToString("f2") + "&nbsp;元";
			this.lblStoreFreight.Text = storeById.StoreFreight.ToDecimal(0).F2ToString("f2") + "&nbsp;元";
			string text5 = (storeById.OpenEndDate < storeById.OpenStartDate) ? "次日" : "";
			Label label = this.lblStoreOpenTime;
			DateTime dateTime = storeById.OpenStartDate;
			string str = dateTime.ToString("HH:mm");
			string str2 = text5;
			dateTime = storeById.OpenEndDate;
			label.Text = str + "-" + str2 + dateTime.ToString("HH:mm");
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			bool flag = false;
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = DataHelper.CleanSearchString(this.txtUserName.Text.Trim());
			string text5 = Globals.StripAllTags(this.txtStoresName.Text.Trim());
			text3 = Globals.StripAllTags(this.txtAddress.Text);
			string text6 = Globals.StripAllTags(this.txtRegionScop.Value.Trim());
			string text7 = Globals.StripAllTags(this.txtRegionScopName.Value.Trim());
			text = this.txtTel.Text;
			text2 = this.txtContactMan.Text;
			string[] array = text6.Split(',');
			string[] array2 = text7.Split(',');
			IDictionary<int, DeliveryScopeInfo> dictionary = new Dictionary<int, DeliveryScopeInfo>();
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				if (int.TryParse(array[i], out num) && array2.Length >= i && dictionary != null && !dictionary.ContainsKey(num))
				{
					DeliveryScopeInfo deliveryScopeInfo = new DeliveryScopeInfo();
					deliveryScopeInfo.RegionId = num;
					deliveryScopeInfo.RegionName = array2[i];
					deliveryScopeInfo.FullRegionPath = RegionHelper.GetFullPath(num, true);
					dictionary.Add(num, deliveryScopeInfo);
				}
			}
			ManagerInfo manager = HiContext.Current.Manager;
			StoresInfo storeById = StoresHelper.GetStoreById(manager.StoreId);
			if (storeById.StoreName != text5 && StoresHelper.ExistStoreName(text5))
			{
				this.ShowMsg("门店名称已经存在,请重新输入！", false);
			}
			else if (string.Compare(this.txtUserPwd.Text, this.txtUserRePwd.Text) != 0)
			{
				this.ShowMsg("请确保两次输入的密码相同", false);
			}
			else if (!this.dropRegion.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择店铺所在区域！", false);
			}
			else if (text2.Length > 8 || text2.Length < 2)
			{
				this.ShowMsg("请输入联系人,联系人长度必须是2-8位！", false);
			}
			else if (text == "" || !DataHelper.IsTel(text))
			{
				this.ShowMsg("请输入正确的联系电话（手机或者固定电话)！", false);
			}
			else
			{
				manager.UserName = this.txtUserName.Text.Trim();
				if (!string.IsNullOrEmpty(this.txtUserPwd.Text))
				{
					if (this.txtUserPwd.Text.Length < 6 || this.txtUserPwd.Text.Length > 20)
					{
						this.ShowMsg("密码长度必须在6到20个字符之间！", false);
						return;
					}
					manager.Password = Users.EncodePassword(this.txtUserPwd.Text, manager.PasswordSalt);
				}
				if (ManagerHelper.Update(manager))
				{
					storeById.StoreName = text5;
					storeById.RegionId = this.dropRegion.GetSelectedRegionId().Value;
					storeById.TopRegionId = RegionHelper.GetCityId(storeById.RegionId);
					storeById.Tel = text;
					storeById.Address = text3;
					storeById.ContactMan = text2;
					storeById.WxOpenId = Globals.StripAllTags(this.txtWxOpenId.Text);
					storeById.FullRegionPath = RegionHelper.GetFullPath(storeById.RegionId, true);
					storeById.Introduce = this.editDescription.Text;
					StoresHelper.UpdateStore(storeById);
					HiCache.Remove("DataCache-StoreInfoDataKey");
					if (dictionary.Count > 0)
					{
						StoresHelper.AddDeliveryScope(storeById.StoreId, dictionary);
					}
				}
				this.ShowMsg("保存成功", true);
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
	}
}
