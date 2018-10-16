using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.promotion.Ascx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderPromotion)]
	public class EditOrderPromotion : AdminPage
	{
		private int activityId;

		protected HiddenField hidStoreIds;

		protected PromoteTypeRadioButtonList radPromoteType;

		protected TrimTextBox txtPromoteType;

		protected TextBox txtCondition;

		protected TextBox txtDiscountValue;

		protected PromotionView promotionView;

		protected Button btnSave;

		protected HiddenField hidSelectGiftId;

		protected HiddenField hidSelectGifts;

		protected HiddenField hidAllSelectedGifts;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnSave.Click += this.btnSave_Click;
				if (!this.Page.IsPostBack)
				{
					PromotionInfo promotion = PromoteHelper.GetPromotion(this.activityId);
					this.promotionView.Promotion = promotion;
					if (SettingsManager.GetMasterSettings().OpenMultStore)
					{
						List<StoreBase> activityStores = StoreActivityHelper.GetActivityStores(this.activityId, 1, promotion.StoreType);
						if (activityStores.Count > 0)
						{
							this.hidStoreIds.Value = (from t in activityStores
							select t.StoreId.ToString()).Aggregate((string t, string n) => t + "," + n);
						}
					}
					else
					{
						this.hidStoreIds.Value = "";
					}
					this.txtPromoteType.Text = ((int)promotion.PromoteType).ToString();
					if (promotion.PromoteType == PromoteType.FullQuantityDiscount || promotion.PromoteType == PromoteType.FullQuantityReduced)
					{
						this.radPromoteType.IsWholesale = true;
						this.txtCondition.Text = promotion.Condition.ToString("F0");
					}
					else
					{
						this.txtCondition.Text = promotion.Condition.F2ToString("f2");
					}
					this.txtDiscountValue.Text = promotion.DiscountValue.F2ToString("f2");
					if (!string.IsNullOrEmpty(promotion.GiftIds))
					{
						this.BindPromotionGifts(promotion);
					}
				}
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			PromotionInfo promotion = this.promotionView.Promotion;
			promotion.ActivityId = this.activityId;
			if (promotion.MemberGradeIds.Count <= 0)
			{
				this.ShowMsg("必须选择一个适合的客户", false);
			}
			else if (promotion.StartDate.CompareTo(promotion.EndDate) > 0)
			{
				this.ShowMsg("开始日期应该小于结束日期", false);
			}
			else
			{
				promotion.PromoteType = (PromoteType)int.Parse(this.txtPromoteType.Text);
				if (promotion.PromoteType == PromoteType.FullQuantityDiscount || promotion.PromoteType == PromoteType.FullQuantityReduced)
				{
					this.radPromoteType.IsWholesale = true;
				}
				decimal condition = default(decimal);
				decimal discountValue = default(decimal);
				decimal.TryParse(this.txtCondition.Text.Trim(), out condition);
				decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discountValue);
				promotion.Condition = condition;
				promotion.DiscountValue = discountValue;
				promotion.GiftIds = this.hidSelectGiftId.Value;
				if (SettingsManager.GetMasterSettings().OpenMultStore && string.IsNullOrEmpty(base.Request.QueryString["isWholesale"]))
				{
					if (string.IsNullOrEmpty(this.hidStoreIds.Value))
					{
						this.ShowMsg("请选择门店范围", false);
						return;
					}
					promotion.StoreType = 2;
					promotion.StoreIds = this.hidStoreIds.Value;
				}
				else
				{
					promotion.StoreIds = "";
					promotion.StoreType = 0;
				}
				switch (PromoteHelper.EditPromotion(promotion, false))
				{
				case -1:
					this.ShowMsg("编辑促销活动失败，可能是信填写有误，请重试", false);
					break;
				case -2:
					this.ShowMsg("编辑促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
					break;
				case 0:
					this.ShowMsg("编辑促销活动失败，请重试", false);
					break;
				default:
					this.BindPromotionGifts(promotion);
					this.ShowMsg("编辑促销活动成功", true, "EditOrderPromotion?activityId=" + this.activityId);
					break;
				}
			}
		}

		private void BindPromotionGifts(PromotionInfo promotion)
		{
			this.hidSelectGiftId.Value = promotion.GiftIds.ToString();
			IList<GiftInfo> giftDetailsByGiftIds = GiftHelper.GetGiftDetailsByGiftIds(promotion.GiftIds);
			if (giftDetailsByGiftIds.Count != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (GiftInfo item in giftDetailsByGiftIds)
				{
					stringBuilder.Append(item.GiftId + "|||" + item.Name + "|||" + (item.CostPrice.HasValue ? item.CostPrice.Value.F2ToString("f2") : "0") + ",,,");
				}
				if (stringBuilder.ToString().Length > 0)
				{
					HiddenField hiddenField = this.hidAllSelectedGifts;
					HiddenField hiddenField2 = this.hidSelectGifts;
					string text3 = hiddenField.Value = (hiddenField2.Value = stringBuilder.ToString().Substring(0, stringBuilder.Length - 3));
				}
			}
		}
	}
}
