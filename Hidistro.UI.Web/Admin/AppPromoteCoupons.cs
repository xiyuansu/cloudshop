using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppDownloadCoupons)]
	public class AppPromoteCoupons : AdminPage
	{
		protected OnOff ooOpen;

		protected HiddenField hidSelectCoupons;

		protected HiddenField hidSelectCouponIds;

		protected Repeater rpCoupons;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.ooOpen.Parameter.Add("onSwitchChange", "fuCheckEnableWXPay");
			this.btnOK.Click += this.btnOK_Click;
			if (!base.IsPostBack)
			{
				this.BindCouponList();
			}
		}

		private void BindCouponList()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.ooOpen.SelectedValue = masterSettings.IsOpenAppPromoteCoupons;
			if (masterSettings.IsOpenAppPromoteCoupons)
			{
				this.hidSelectCouponIds.Value = ",";
				string sWhere = "and COUPONID IN ('" + masterSettings.AppPromoteCouponList.Replace(",", "','") + "')";
				DbQueryResult couponInfos = CouponHelper.GetCouponInfos(this.GetCouponsSearch(), sWhere);
				this.rpCoupons.DataSource = couponInfos.Data;
				this.rpCoupons.DataBind();
			}
			else
			{
				this.hidSelectCouponIds.Value = "";
				this.rpCoupons.DataSource = null;
				this.rpCoupons.DataBind();
			}
		}

		private void Refresh()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.hidSelectCouponIds.Value = ",";
			string sWhere = "and COUPONID IN ('" + masterSettings.AppPromoteCouponList.Replace(",", "','") + "')";
			DbQueryResult couponInfos = CouponHelper.GetCouponInfos(this.GetCouponsSearch(), sWhere);
			this.rpCoupons.DataSource = couponInfos.Data;
			this.rpCoupons.DataBind();
		}

		public CouponsSearch GetCouponsSearch()
		{
			return new CouponsSearch
			{
				ObtainWay = 1,
				PageIndex = 1,
				PageSize = 100
			};
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.IsOpenAppPromoteCoupons = this.ooOpen.SelectedValue;
			if (this.ooOpen.SelectedValue)
			{
				masterSettings.AppPromoteCouponList = this.hidSelectCouponIds.Value;
				int num = 0;
				string[] array = masterSettings.AppPromoteCouponList.Split(',');
				foreach (string text in array)
				{
					if (!string.IsNullOrEmpty(text))
					{
						CouponInfo eFCoupon = CouponHelper.GetEFCoupon(text.ToInt(0));
						if (eFCoupon == null)
						{
							this.ShowMsg("保存失败！部分优惠券已过期或剩余数量不足，请删除后重新选择。", false);
							this.Refresh();
							return;
						}
						int couponSurplus = CouponHelper.GetCouponSurplus(text.ToInt(0));
						if (eFCoupon.ClosingTime < DateTime.Now || couponSurplus <= 0)
						{
							this.ShowMsg("保存失败！部分优惠券已过期或剩余数量不足，请删除后重新选择。", false);
							this.Refresh();
							return;
						}
						num++;
					}
				}
				if (num > 10)
				{
					this.ShowMsg("只可以选择10张优惠券，请删除部分优惠券。", false);
					this.Refresh();
					return;
				}
			}
			else
			{
				masterSettings.AppPromoteCouponList = "";
			}
			SettingsManager.Save(masterSettings);
			this.BindCouponList();
			this.ShowMsg("保存成功", true, "");
		}

		public int GetCouponSurplus(int CouponId)
		{
			HiddenField hiddenField = this.hidSelectCouponIds;
			hiddenField.Value = hiddenField.Value + CouponId + ",";
			return CouponHelper.GetCouponSurplus(CouponId);
		}
	}
}
