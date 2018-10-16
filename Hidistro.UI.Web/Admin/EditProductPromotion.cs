using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.promotion.Ascx;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductPromotion)]
	public class EditProductPromotion : AdminPage
	{
		private int activityId;

		private bool IsMobileExclusive = false;

		protected HtmlGenericControl promoteli;

		protected PromoteTypeRadioButtonList radPromoteType;

		protected TrimTextBox txtPromoteType;

		protected TextBox txtCondition;

		protected TextBox txtDiscountValue;

		protected TextBox txtGiftNum;

		protected PromotionView promotionView;

		protected Button btnNext;

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
				this.btnNext.Click += this.btnNext_Click;
				if (!this.Page.IsPostBack)
				{
					PromotionInfo promotion = PromoteHelper.GetPromotion(this.activityId);
					if (promotion == null)
					{
						this.ShowMsg("该礼品不存在或已被删除", false);
					}
					else
					{
						this.promotionView.Promotion = promotion;
						this.txtPromoteType.Text = ((int)promotion.PromoteType).ToString();
						if (promotion.PromoteType == PromoteType.QuantityDiscount)
						{
							this.radPromoteType.IsWholesale = true;
						}
						if (promotion.PromoteType == PromoteType.MobileExclusive)
						{
							this.IsMobileExclusive = true;
							this.radPromoteType.IsMobileExclusive = true;
							this.promotionView.FindControl("chklMemberGrade").Visible = false;
							this.promoteli.Attributes.CssStyle.Add("display", "none");
						}
						decimal num;
						if (promotion.Condition != decimal.Zero)
						{
							TextBox textBox = this.txtCondition;
							num = promotion.Condition;
							textBox.Text = num.ToString("F0");
						}
						if (promotion.PromoteType == PromoteType.SentProduct)
						{
							TextBox textBox2 = this.txtDiscountValue;
							num = promotion.DiscountValue;
							textBox2.Text = num.ToString("F0");
						}
						else
						{
							this.txtDiscountValue.Text = promotion.DiscountValue.F2ToString("f2");
						}
						if (!string.IsNullOrEmpty(promotion.GiftIds))
						{
							this.BindPromotionGifts(promotion);
						}
					}
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

		private void btnNext_Click(object sender, EventArgs e)
		{
			PromotionInfo promotion = this.promotionView.Promotion;
			promotion.ActivityId = this.activityId;
			promotion.PromoteType = (PromoteType)int.Parse(this.txtPromoteType.Text);
			if (promotion.PromoteType == PromoteType.MobileExclusive)
			{
				this.radPromoteType.IsMobileExclusive = true;
			}
			else if (promotion.MemberGradeIds.Count <= 0)
			{
				this.ShowMsg("必须选择一个适合的客户", false);
				return;
			}
			if (promotion.StartDate.CompareTo(promotion.EndDate) > 0)
			{
				this.ShowMsg("开始日期应该小于结束日期", false);
			}
			else
			{
				if (promotion.PromoteType == PromoteType.QuantityDiscount)
				{
					this.radPromoteType.IsWholesale = true;
				}
				decimal num = default(decimal);
				decimal discountValue = default(decimal);
				decimal.TryParse(this.txtCondition.Text.Trim(), out num);
				decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discountValue);
				promotion.Condition = num;
				promotion.DiscountValue = discountValue;
				promotion.GiftIds = this.hidSelectGiftId.Value;
				promotion.StoreIds = "";
				if (promotion.PromoteType == PromoteType.QuantityDiscount && num < decimal.One)
				{
					this.ShowMsg("单品批发的数量必须大于1", false);
				}
				else
				{
					switch (PromoteHelper.EditPromotion(promotion, promotion.PromoteType == PromoteType.MobileExclusive))
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
						this.ShowMsg("编辑促销活动成功", true);
						break;
					}
				}
			}
		}
	}
}
