using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.promotion.Ascx;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderPromotion)]
	public class AddOrderPromotion : AdminPage
	{
		public bool isWholesale;

		protected HiddenField hidStoreIds;

		protected PromoteTypeRadioButtonList radPromoteType;

		protected TrimTextBox txtPromoteType;

		protected TextBox txtCondition;

		protected TextBox txtDiscountValue;

		protected PromotionView promotionView;

		protected Button btnAdd;

		protected HiddenField hidSelectGiftId;

		protected HiddenField hidSelectGifts;

		protected HiddenField hidAllSelectedGifts;

		protected void Page_Load(object sender, EventArgs e)
		{
			bool.TryParse(base.Request.QueryString["isWholesale"], out this.isWholesale);
			if (this.isWholesale)
			{
				this.radPromoteType.IsWholesale = true;
			}
			this.btnAdd.Click += this.btnAdd_Click;
			if (this.Page.IsPostBack)
			{
				return;
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			PromotionInfo promotion = this.promotionView.Promotion;
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
					promotion.StoreType = 0;
				}
				switch (PromoteHelper.AddPromotion(promotion, false))
				{
				case -1:
					this.ShowMsg("添加促销活动失败，可能是信填写有误，请重试", false);
					break;
				case -2:
					this.ShowMsg("添加促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
					break;
				case 0:
					this.ShowMsg("添加促销活动失败，请重试", false);
					break;
				default:
					if (this.isWholesale)
					{
						base.Response.Redirect(Globals.GetAdminAbsolutePath("/promotion/OrderPromotions.aspx?isWholesale=true"), true);
					}
					else
					{
						base.Response.Redirect(Globals.GetAdminAbsolutePath("/promotion/OrderPromotions.aspx"), true);
					}
					break;
				}
			}
		}
	}
}
