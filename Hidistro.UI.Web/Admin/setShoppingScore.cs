using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class setShoppingScore : AdminPage
	{
		protected TextBox txtMemberRegistrationPoint;

		protected TextBox txtSignInPoint;

		protected TextBox txtContinuousDays;

		protected TextBox txtContinuousPoint;

		protected TextBox txtShoppingBounty;

		protected TextBox txtProductCommentPoint;

		protected TextBox txtShoppingDeduction;

		protected TextBox txtShoppingDeductionRatio;

		protected OnOff radCanPointUseWithCoupon;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				TextBox textBox = this.txtMemberRegistrationPoint;
				int num = masterSettings.MemberRegistrationPoint;
				textBox.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox2 = this.txtSignInPoint;
				num = masterSettings.SignInPoint;
				textBox2.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox3 = this.txtContinuousDays;
				num = masterSettings.ContinuousDays;
				textBox3.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox4 = this.txtContinuousPoint;
				num = masterSettings.ContinuousPoint;
				textBox4.Text = num.ToString(CultureInfo.InvariantCulture);
				this.txtShoppingBounty.Text = masterSettings.PointsRate.ToString(CultureInfo.InvariantCulture);
				TextBox textBox5 = this.txtShoppingDeduction;
				num = masterSettings.ShoppingDeduction;
				textBox5.Text = num.ToString(CultureInfo.InvariantCulture);
				TextBox textBox6 = this.txtShoppingDeductionRatio;
				num = masterSettings.ShoppingDeductionRatio;
				textBox6.Text = num.ToString(CultureInfo.InvariantCulture);
				this.radCanPointUseWithCoupon.SelectedValue = masterSettings.CanPointUseWithCoupon;
				TextBox textBox7 = this.txtProductCommentPoint;
				num = masterSettings.ProductCommentPoint;
				textBox7.Text = num.ToString(CultureInfo.InvariantCulture);
			}
			this.btnOK.Click += this.btnOK_Click;
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			int productCommentPoint = 0;
			int memberRegistrationPoint = default(int);
			int signInPoint = default(int);
			int continuousDays = default(int);
			int continuousPoint = default(int);
			decimal d = default(decimal);
			int shoppingDeduction = default(int);
			int num = default(int);
			if (string.IsNullOrEmpty(this.txtMemberRegistrationPoint.Text) || !int.TryParse(this.txtMemberRegistrationPoint.Text.Trim(), out memberRegistrationPoint))
			{
				this.ShowMsg("注册会员奖励分数不能为空且为正数", false);
			}
			else if (string.IsNullOrEmpty(this.txtSignInPoint.Text) || !int.TryParse(this.txtSignInPoint.Text.Trim(), out signInPoint))
			{
				this.ShowMsg("每日签到奖励分数不能为空且为正数", false);
			}
			else if (string.IsNullOrEmpty(this.txtContinuousDays.Text) || !int.TryParse(this.txtContinuousDays.Text.Trim(), out continuousDays))
			{
				this.ShowMsg("连续签到天数不能为空且为正数", false);
			}
			else if (string.IsNullOrEmpty(this.txtContinuousPoint.Text) || !int.TryParse(this.txtContinuousPoint.Text.Trim(), out continuousPoint))
			{
				this.ShowMsg("连续签到天数的奖励积分不能为空且为正数", false);
			}
			else if (string.IsNullOrEmpty(this.txtShoppingBounty.Text) || !decimal.TryParse(this.txtShoppingBounty.Text.Trim(), out d))
			{
				this.ShowMsg("购物消费奖励几元一积分不能为空,为非负数字,范围在0.1-10000000之间", false);
			}
			else if (string.IsNullOrEmpty(this.txtProductCommentPoint.Text) || !int.TryParse(this.txtProductCommentPoint.Text.Trim(), out productCommentPoint))
			{
				this.ShowMsg("商品评论奖励积分不能为空且为正数, 范围在0-99999之间", false);
			}
			else if (string.IsNullOrEmpty(this.txtShoppingDeduction.Text) || !int.TryParse(this.txtShoppingDeduction.Text.Trim(), out shoppingDeduction))
			{
				this.ShowMsg("订单抵扣几积分一元不能为空且为正数", false);
			}
			else if (string.IsNullOrEmpty(this.txtShoppingDeductionRatio.Text) || !int.TryParse(this.txtShoppingDeductionRatio.Text.Trim(), out num))
			{
				this.ShowMsg("可使用积分抵扣的比例必须为整数，且小于或等于100%", false);
			}
			else if (num > 100)
			{
				this.ShowMsg("可使用积分抵扣的比例必须为整数，且小于或等于100%", false);
			}
			else
			{
				try
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings();
					masterSettings.MemberRegistrationPoint = memberRegistrationPoint;
					masterSettings.SignInPoint = signInPoint;
					masterSettings.ContinuousDays = continuousDays;
					masterSettings.ContinuousPoint = continuousPoint;
					masterSettings.PointsRate = Math.Round(d, 2);
					masterSettings.ShoppingDeduction = shoppingDeduction;
					masterSettings.ShoppingDeductionRatio = num;
					masterSettings.CanPointUseWithCoupon = this.radCanPointUseWithCoupon.SelectedValue;
					masterSettings.ProductCommentPoint = productCommentPoint;
					Globals.EntityCoding(masterSettings, true);
					SettingsManager.Save(masterSettings);
					this.ShowMsg("保存成功", true);
				}
				catch (Exception)
				{
					this.ShowMsg("保存失败，请检查设置或联系管理员", false);
				}
			}
		}
	}
}
