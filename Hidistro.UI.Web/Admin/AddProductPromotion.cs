using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Web.Admin.promotion.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductPromotion)]
	public class AddProductPromotion : AdminPage
	{
		public bool isWholesale;

		public bool IsMobileExclusive = false;

		protected HtmlGenericControl promoteli;

		protected PromoteTypeRadioButtonList radPromoteType;

		protected TrimTextBox txtPromoteType;

		protected TextBox txtCondition;

		protected TextBox txtDiscountValue;

		protected PromotionView promotionView;

		protected Button btnNext;

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
			else
			{
				bool.TryParse(base.Request.QueryString["IsMobileExclusive"], out this.IsMobileExclusive);
				if (this.IsMobileExclusive)
				{
					this.radPromoteType.IsMobileExclusive = true;
					this.promotionView.FindControl("chklMemberGrade").Visible = false;
					this.txtPromoteType.Text = 7.ToString();
					this.promoteli.Attributes.CssStyle.Add("display", "none");
				}
			}
			this.btnNext.Click += this.btnNext_Click;
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			this.btnNext.Enabled = false;
			PromotionInfo promotion = this.promotionView.Promotion;
			if (promotion.MemberGradeIds.Count <= 0)
			{
				this.btnNext.Enabled = true;
				this.ShowMsg("必须选择一个适合的客户", false);
			}
			else if (promotion.StartDate.CompareTo(promotion.EndDate) > 0)
			{
				this.btnNext.Enabled = true;
				this.ShowMsg("开始日期应该小于结束日期", false);
			}
			else
			{
				promotion.GiftIds = this.hidSelectGiftId.Value;
				promotion.PromoteType = (PromoteType)int.Parse(this.txtPromoteType.Text);
				decimal num = default(decimal);
				decimal discountValue = default(decimal);
				decimal.TryParse(this.txtCondition.Text.Trim(), out num);
				decimal.TryParse(this.txtDiscountValue.Text.Trim(), out discountValue);
				promotion.Condition = num;
				promotion.DiscountValue = discountValue;
				if (promotion.PromoteType == PromoteType.QuantityDiscount && num < decimal.One)
				{
					this.ShowMsg("单品批发的数量必须大于1", false);
				}
				else
				{
					int num2 = PromoteHelper.AddPromotion(promotion, this.IsMobileExclusive);
					switch (num2)
					{
					case -1:
						this.btnNext.Enabled = true;
						this.ShowMsg("添加促销活动失败，可能是信填写有误，请重试", false);
						break;
					case -2:
						this.btnNext.Enabled = true;
						this.ShowMsg("添加促销活动失败，可能是选择的会员等级已经被删除，请重试", false);
						break;
					case 0:
						this.btnNext.Enabled = true;
						this.ShowMsg("添加促销活动失败，请重试", false);
						break;
					default:
						base.Response.Redirect(Globals.GetAdminAbsolutePath("/promotion/SetPromotionProducts.aspx?ActivityId=" + num2 + "&isWholesale=" + this.isWholesale.ToString() + "&IsMobileExclusive=" + this.IsMobileExclusive.ToString()), true);
						break;
					}
				}
			}
		}
	}
}
