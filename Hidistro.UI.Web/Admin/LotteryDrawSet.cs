using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.AppLotteryDrawSet)]
	public class LotteryDrawSet : AdminPage
	{
		private static Random r = new Random();

		protected OnOff onoffEnableShake;

		protected TextBox txtDepletePoints;

		protected DropDownList ddlDrawType1;

		protected TextBox txtPoints1;

		protected CouponDropDownList ddlCoupons1;

		protected TextBox txtPercent1;

		protected DropDownList ddlDrawType2;

		protected TextBox txtPoints2;

		protected CouponDropDownList ddlCoupons2;

		protected TextBox txtPercent2;

		protected DropDownList ddlDrawType3;

		protected TextBox txtPoints3;

		protected CouponDropDownList ddlCoupons3;

		protected TextBox txtPercent3;

		protected DropDownList ddlDrawType4;

		protected TextBox txtPoints4;

		protected CouponDropDownList ddlCoupons4;

		protected TextBox txtPercent4;

		protected Button btnOK;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.onoffEnableShake.Parameter.Add("onSwitchChange", "fuOnOffEnableShake");
			if (!base.IsPostBack)
			{
				this.ddlCoupons1.BindCoupons();
				this.ddlCoupons2.BindCoupons();
				this.ddlCoupons3.BindCoupons();
				this.ddlCoupons4.BindCoupons();
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				TextBox textBox = this.txtDepletePoints;
				int num = masterSettings.AppDepletePoints;
				textBox.Text = num.ToString();
				DropDownList dropDownList = this.ddlDrawType1;
				num = masterSettings.AppFirstPrizeType;
				dropDownList.SelectedValue = num.ToString();
				TextBox textBox2 = this.txtPoints1;
				num = masterSettings.AppFirstPrizePoints;
				textBox2.Text = num.ToString();
				CouponDropDownList couponDropDownList = this.ddlCoupons1;
				num = masterSettings.AppFirstPrizeCouponId;
				couponDropDownList.SelectedValue = num.ToString();
				TextBox textBox3 = this.txtPercent1;
				num = masterSettings.AppFirstPrizePercent;
				textBox3.Text = num.ToString();
				DropDownList dropDownList2 = this.ddlDrawType2;
				num = masterSettings.AppSecondPrizeType;
				dropDownList2.SelectedValue = num.ToString();
				TextBox textBox4 = this.txtPoints2;
				num = masterSettings.AppSecondPrizePoints;
				textBox4.Text = num.ToString();
				CouponDropDownList couponDropDownList2 = this.ddlCoupons2;
				num = masterSettings.AppSecondPrizeCouponId;
				couponDropDownList2.SelectedValue = num.ToString();
				TextBox textBox5 = this.txtPercent2;
				num = masterSettings.AppSecondPrizePercent;
				textBox5.Text = num.ToString();
				DropDownList dropDownList3 = this.ddlDrawType3;
				num = masterSettings.AppThirdPrizeType;
				dropDownList3.SelectedValue = num.ToString();
				TextBox textBox6 = this.txtPoints3;
				num = masterSettings.AppThirdPrizePoints;
				textBox6.Text = num.ToString();
				CouponDropDownList couponDropDownList3 = this.ddlCoupons3;
				num = masterSettings.AppThirdPrizeCouponId;
				couponDropDownList3.SelectedValue = num.ToString();
				TextBox textBox7 = this.txtPercent3;
				num = masterSettings.AppThirdPrizePercent;
				textBox7.Text = num.ToString();
				DropDownList dropDownList4 = this.ddlDrawType4;
				num = masterSettings.AppFourPrizeType;
				dropDownList4.SelectedValue = num.ToString();
				TextBox textBox8 = this.txtPoints4;
				num = masterSettings.AppFourPrizePoints;
				textBox8.Text = num.ToString();
				CouponDropDownList couponDropDownList4 = this.ddlCoupons4;
				num = masterSettings.AppFourPrizeCouponId;
				couponDropDownList4.SelectedValue = num.ToString();
				TextBox textBox9 = this.txtPercent4;
				num = masterSettings.AppFourPrizePercent;
				textBox9.Text = num.ToString();
				this.onoffEnableShake.SelectedValue = masterSettings.EnableAppShake;
			}
		}

		private void InitControls(SiteSettings setting)
		{
			if (setting.AppFirstPrizeType == 1)
			{
				this.txtPoints1.Attributes.Add("style", "display:block;");
				this.ddlCoupons1.Attributes.Add("style", "display:none;");
			}
			else
			{
				this.ddlCoupons1.Attributes.Add("style", "display:block;");
				this.txtPoints1.Attributes.Add("style", "display:none;");
			}
			if (setting.AppSecondPrizeType == 1)
			{
				this.txtPoints2.Attributes.Add("style", "display:block;");
				this.ddlCoupons2.Attributes.Add("style", "display:none;");
			}
			else
			{
				this.ddlCoupons2.Attributes.Add("style", "display:block;");
				this.txtPoints2.Attributes.Add("style", "display:none;");
			}
			if (setting.AppThirdPrizeType == 1)
			{
				this.txtPoints3.Attributes.Add("style", "display:'';");
				this.ddlCoupons3.Attributes.Add("style", "display:none;");
			}
			else
			{
				this.ddlCoupons3.Attributes.Add("style", "display:'';");
				this.txtPoints3.Attributes.Add("style", "display:none;");
			}
			if (setting.AppFourPrizeType == 1)
			{
				this.txtPoints4.Attributes.Add("style", "display:'';");
				this.ddlCoupons4.Attributes.Add("style", "display:none;");
			}
			else
			{
				this.ddlCoupons4.Attributes.Add("style", "display:'';");
				this.txtPoints4.Attributes.Add("style", "display:none;");
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			if (this.ValidateParams())
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings();
				masterSettings.AppFirstPrizeType = this.ddlDrawType1.SelectedValue.ToInt(0);
				masterSettings.AppFirstPrizePoints = this.txtPoints1.Text.Trim().ToInt(0);
				masterSettings.AppFirstPrizeCouponId = this.ddlCoupons1.SelectedValue.ToInt(0);
				masterSettings.AppSecondPrizeType = this.ddlDrawType2.SelectedValue.ToInt(0);
				masterSettings.AppSecondPrizePoints = this.txtPoints2.Text.Trim().ToInt(0);
				masterSettings.AppSecondPrizeCouponId = this.ddlCoupons2.SelectedValue.ToInt(0);
				masterSettings.AppThirdPrizeType = this.ddlDrawType3.SelectedValue.ToInt(0);
				masterSettings.AppThirdPrizePoints = this.txtPoints3.Text.ToInt(0);
				masterSettings.AppThirdPrizeCouponId = this.ddlCoupons3.SelectedValue.ToInt(0);
				masterSettings.AppFourPrizeType = this.ddlDrawType4.SelectedValue.ToInt(0);
				masterSettings.AppFourPrizePoints = this.txtPoints4.Text.ToInt(0);
				masterSettings.AppFourPrizeCouponId = this.ddlCoupons4.SelectedValue.ToInt(0);
				masterSettings.AppFirstPrizePercent = this.txtPercent1.Text.ToInt(0);
				masterSettings.AppSecondPrizePercent = this.txtPercent2.Text.ToInt(0);
				masterSettings.AppThirdPrizePercent = this.txtPercent3.Text.ToInt(0);
				masterSettings.AppFourPrizePercent = this.txtPercent4.Text.ToInt(0);
				masterSettings.AppDepletePoints = this.txtDepletePoints.Text.ToInt(0);
				masterSettings.EnableAppShake = this.onoffEnableShake.SelectedValue;
				SettingsManager.Save(masterSettings);
				this.ShowMsg("保存成功", true);
				this.InitControls(masterSettings);
			}
		}

		private bool ValidateParams()
		{
			if (!this.onoffEnableShake.SelectedValue)
			{
				return true;
			}
			if (!this.txtDepletePoints.Text.Trim().IsDecimal() || this.txtDepletePoints.Text.ToInt(0) < 1)
			{
				this.ShowMsg("每次摇奖消耗积分：数据格式不正确", false);
				return false;
			}
			if (this.ddlDrawType1.SelectedValue == "1" && !this.txtPoints1.Text.Trim().IsInt())
			{
				goto IL_0122;
			}
			if (this.ddlDrawType2.SelectedValue == "1" && !this.txtPoints2.Text.Trim().IsInt())
			{
				goto IL_0122;
			}
			if (this.ddlDrawType3.SelectedValue == "1" && !this.txtPoints3.Text.Trim().IsInt())
			{
				goto IL_0122;
			}
			int num = (this.ddlDrawType4.SelectedValue == "1" && !this.txtPoints4.Text.Trim().IsInt()) ? 1 : 0;
			goto IL_0123;
			IL_0205:
			int num2;
			if (num2 != 0)
			{
				this.ShowMsg("积分必须大于0", false);
				return false;
			}
			if (!this.txtPercent1.Text.Trim().IsPositiveInteger() || !this.txtPercent2.Text.Trim().IsPositiveInteger() || !this.txtPercent3.Text.Trim().IsPositiveInteger() || !this.txtPercent4.Text.Trim().IsPositiveInteger())
			{
				this.ShowMsg("概率数据格式不正确", false);
				return false;
			}
			if (this.txtPercent1.Text.Trim().ToInt(0) + this.txtPercent2.Text.Trim().ToInt(0) + this.txtPercent3.Text.Trim().ToInt(0) + this.txtPercent4.Text.Trim().ToInt(0) != 100)
			{
				this.ShowMsg("概率的总和必须为100", false);
				return false;
			}
			return true;
			IL_0204:
			num2 = 1;
			goto IL_0205;
			IL_0123:
			if (num != 0)
			{
				this.ShowMsg("积分数据格式不正确", false);
				return false;
			}
			if (this.ddlDrawType1.SelectedValue == "1" && int.Parse(this.txtPoints1.Text.Trim()) <= 0)
			{
				goto IL_0204;
			}
			if (this.ddlDrawType2.SelectedValue == "1" && int.Parse(this.txtPoints2.Text.Trim()) <= 0)
			{
				goto IL_0204;
			}
			if (this.ddlDrawType3.SelectedValue == "1" && int.Parse(this.txtPoints3.Text.Trim()) <= 0)
			{
				goto IL_0204;
			}
			num2 = ((this.ddlDrawType4.SelectedValue == "1" && int.Parse(this.txtPoints4.Text.Trim()) <= 0) ? 1 : 0);
			goto IL_0205;
			IL_0122:
			num = 1;
			goto IL_0123;
		}
	}
}
