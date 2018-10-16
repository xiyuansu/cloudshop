using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Depot;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class StoreDetails : AdminPage
	{
		private int storeId = 0;

		protected Literal lblUserName;

		protected Literal lblStoreName;

		protected Literal lblRegions;

		protected Literal lblAddress;

		protected Literal lblContactMan;

		protected Literal lblTel;

		protected Repeater repStoreDeliveryScop;

		protected Literal lblServeRadius;

		protected Literal lblStoreOpenTime;

		protected Literal lblIsAboveSelf;

		protected Literal lblWXBusinessName;

		protected Literal lblWXBranchName;

		protected Literal lblCategoryName;

		protected Literal lblWxAddress;

		protected Literal lblImages;

		protected Literal lblWXTelephone;

		protected Literal lblWXAvgPrice;

		protected Literal lblWXOpenTime;

		protected Literal lblWXRecommend;

		protected Literal lblWXSpecial;

		protected Literal lblWXIntroduction;

		protected Literal lblState;

		protected HiddenField hfLongitude;

		protected HiddenField hfLatitude;

		protected HiddenField hfProvince;

		protected HiddenField hfCity;

		protected HiddenField hfDistrict;

		protected HiddenField hfSiteName;

		protected HiddenField hidIsWX;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.storeId = base.Request.QueryString["StoreId"].ToInt(0);
			if (this.storeId == 0)
			{
				base.Response.Redirect("StoresList.aspx");
			}
			if (!this.Page.IsPostBack)
			{
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
				DbQueryResult storeDeliveryScop = StoresHelper.GetStoreDeliveryScop(new DeliveryScopeQuery
				{
					StoreId = storeById.StoreId,
					PageIndex = 1,
					PageSize = 2147483647
				});
				this.repStoreDeliveryScop.DataSource = storeDeliveryScop.Data;
				this.repStoreDeliveryScop.DataBind();
				this.lblState.Text = (storeById.State.Equals(0) ? "关闭" : "开启");
				this.lblUserName.Text = managerInfo.UserName;
				this.lblStoreName.Text = storeById.StoreName;
				this.lblRegions.Text = RegionHelper.GetFullRegion(storeById.RegionId, string.Empty, true, 0);
				this.lblAddress.Text = storeById.Address;
				this.lblContactMan.Text = storeById.ContactMan;
				this.lblTel.Text = storeById.Tel;
				this.lblCategoryName.Text = storeById.WXCategoryName;
				Literal literal = this.lblServeRadius;
				double? nullable = storeById.ServeRadius;
				literal.Text = nullable.ToString();
				this.lblStoreOpenTime.Text = (string.IsNullOrEmpty(storeById.StoreOpenTime) ? "无" : storeById.StoreOpenTime);
				this.lblIsAboveSelf.Text = (storeById.IsAboveSelf ? "是" : "否");
				this.lblWXBusinessName.Text = storeById.WXBusinessName;
				this.lblWXBranchName.Text = storeById.WXBranchName;
				this.lblWxAddress.Text = storeById.WxAddress;
				HiddenField hiddenField = this.hfLongitude;
				nullable = storeById.Longitude;
				hiddenField.Value = nullable.ToString();
				HiddenField hiddenField2 = this.hfLatitude;
				nullable = storeById.Latitude;
				hiddenField2.Value = nullable.ToString();
				this.lblWXTelephone.Text = storeById.WXTelephone;
				Literal literal2 = this.lblWXAvgPrice;
				int? nullable2 = storeById.WXAvgPrice;
				literal2.Text = nullable2.ToString();
				this.lblWXOpenTime.Text = storeById.WXOpenTime;
				this.lblWXRecommend.Text = storeById.WXRecommend;
				this.lblWXSpecial.Text = storeById.WXSpecial;
				this.lblWXIntroduction.Text = storeById.WXIntroduction;
				HiddenField hiddenField3 = this.hidIsWX;
				nullable2 = storeById.WXState;
				hiddenField3.Value = nullable2.ToString();
				if (!string.IsNullOrEmpty(storeById.StoreImages))
				{
					string[] array = storeById.StoreImages.Split(',');
					foreach (string text in array)
					{
						if (!string.IsNullOrEmpty(text.Trim()))
						{
							Literal literal3 = this.lblImages;
							literal3.Text += $"<img src='{text}' width='98' height='98' style='padding:0 10px;'/>";
						}
					}
				}
			}
		}
	}
}
