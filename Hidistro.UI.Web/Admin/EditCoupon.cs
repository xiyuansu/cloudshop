using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class EditCoupon : AdminPage
	{
		private int couponId;

		protected TextBox txtCouponName;

		protected TextBox txtPrice;

		protected TextBox txtSendCount;

		protected Label lblLastCount;

		protected HtmlSelect ddlUserLimitCount;

		protected RadioButton radNoLimit;

		protected RadioButton radUseFullCut;

		protected TextBox txtFullPrice;

		protected CalendarPanel calendarStartDate;

		protected CalendarPanel calendarEndDate;

		protected RadioButton radAll;

		protected RadioButton radSomeProducts;

		protected Label lblSelectCount;

		protected RadioButton radActiveReceive;

		protected RadioButton radGrant;

		protected RadioButton radExchange;

		protected TextBox txtNeedPoint;

		protected CheckBox chkPanicBuying;

		protected CheckBox chkGroup;

		protected CheckBox chkFireGroup;

		protected Button btnEditCoupons;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidProductIds;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditCoupons.Click += this.btnEditCoupons_Click;
				Dictionary<string, object> calendarParameter = this.calendarStartDate.CalendarParameter;
				DateTime now = DateTime.Now;
				calendarParameter.Add("startDate ", now.ToString("yyyy-MM-dd"));
				Dictionary<string, object> calendarParameter2 = this.calendarEndDate.CalendarParameter;
				now = DateTime.Now;
				calendarParameter2.Add("startDate ", now.ToString("yyyy-MM-dd"));
				if (!this.Page.IsPostBack)
				{
					CouponInfo eFCoupon = CouponHelper.GetEFCoupon(this.couponId);
					if (eFCoupon != null)
					{
						this.txtCouponName.Text = eFCoupon.CouponName;
						this.txtPrice.Text = eFCoupon.Price.F2ToString("f2");
						TextBox textBox = this.txtSendCount;
						int num = eFCoupon.SendCount;
						textBox.Text = num.ToString();
						HtmlSelect htmlSelect = this.ddlUserLimitCount;
						num = eFCoupon.UserLimitCount;
						htmlSelect.Value = num.ToString();
						decimal? orderUseLimit = eFCoupon.OrderUseLimit;
						if (orderUseLimit.GetValueOrDefault() > default(decimal) && orderUseLimit.HasValue)
						{
							this.radUseFullCut.Checked = true;
							this.txtFullPrice.Text = $"{eFCoupon.OrderUseLimit:F2}";
						}
						else
						{
							this.radNoLimit.Checked = true;
							this.txtFullPrice.Text = "";
						}
						this.calendarEndDate.SelectedDate = eFCoupon.ClosingTime;
						this.calendarStartDate.SelectedDate = eFCoupon.StartTime;
						if (!string.IsNullOrEmpty(eFCoupon.CanUseProducts))
						{
							this.radSomeProducts.Checked = true;
							DataTable productBaseInfo = ProductHelper.GetProductBaseInfo(eFCoupon.CanUseProducts);
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < productBaseInfo.Rows.Count; i++)
							{
								string str = productBaseInfo.Rows[i]["ProductId"].ToString();
								string str2 = productBaseInfo.Rows[i]["ProductName"].ToString();
								if (stringBuilder.Length > 0)
								{
									stringBuilder.Insert(0, str + "|||" + str2 + ",,,");
								}
								else
								{
									stringBuilder.Append(str + "|||" + str2);
								}
							}
							this.hidSelectProducts.Value = stringBuilder.ToString();
						}
						else
						{
							this.radAll.Checked = true;
						}
						if (eFCoupon.ObtainWay == 0)
						{
							this.radActiveReceive.Checked = true;
						}
						else if (eFCoupon.ObtainWay == 1)
						{
							this.radGrant.Checked = true;
						}
						else if (eFCoupon.ObtainWay == 2)
						{
							this.radExchange.Checked = true;
							TextBox textBox2 = this.txtNeedPoint;
							num = eFCoupon.NeedPoint;
							textBox2.Text = num.ToString();
						}
						if (eFCoupon.UseWithPanicBuying)
						{
							this.chkPanicBuying.Checked = true;
						}
						if (eFCoupon.UseWithGroup)
						{
							this.chkGroup.Checked = true;
						}
						if (eFCoupon.UseWithFireGroup)
						{
							this.chkFireGroup.Checked = true;
						}
						Label label = this.lblLastCount;
						num = CouponHelper.GetCouponObtainNum(this.couponId, 0);
						label.Text = num.ToString();
					}
					else
					{
						base.GotoResourceNotFound();
					}
				}
			}
		}

		private void btnEditCoupons_Click(object sender, EventArgs e)
		{
			int num = 0;
			decimal num2 = default(decimal);
			decimal num3 = default(decimal);
			int num4 = 0;
			if (string.IsNullOrEmpty(this.txtCouponName.Text.Trim()))
			{
				this.ShowMsg("请输入优惠券名称！", false);
			}
			else if (!decimal.TryParse(this.txtPrice.Text, out num2) || num2 > 10000m || num2 < decimal.Zero)
			{
				this.ShowMsg("面值只能是数值，0.01-10000，限2位小数！", false);
			}
			else if (!int.TryParse(this.txtSendCount.Text, out num) || num > 100000000 || num < 0)
			{
				this.ShowMsg("发放总量只能是正整数，0-100000000之间！", false);
			}
			else if (num < int.Parse(this.lblLastCount.Text.Trim()))
			{
				this.ShowMsg("发放总量不能小于已抢数量！", false);
			}
			else if (int.Parse(this.ddlUserLimitCount.Value) > num)
			{
				this.ShowMsg("每人限领张数不能大于发放总量！", false);
			}
			else if (this.radUseFullCut.Checked && (!decimal.TryParse(this.txtFullPrice.Text, out num3) || num3 > 10000000m || num3 < decimal.One))
			{
				this.ShowMsg("使用门槛的订单金额只能为数字，且在1-10000000之间！", false);
			}
			else if (!this.calendarStartDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择有效期的开始日期！", false);
			}
			else if (!this.calendarEndDate.SelectedDate.HasValue)
			{
				this.ShowMsg("请选择有效期的结束日期！", false);
			}
			else
			{
				DateTime dateTime = this.calendarStartDate.SelectedDate.Value;
				if (dateTime.CompareTo(this.calendarEndDate.SelectedDate.Value) > 0)
				{
					this.ShowMsg("有效期的开始日期不能晚于结束日期！", false);
				}
				else if (this.radExchange.Checked && (!int.TryParse(this.txtNeedPoint.Text, out num4) || num4 > 10000000 || num4 <= 0))
				{
					this.ShowMsg("兑换所需积分只能为正整数，在1-10000000之间！", false);
				}
				else
				{
					CouponInfo coupon = CouponHelper.GetCoupon(this.couponId);
					coupon.CouponName = this.txtCouponName.Text;
					coupon.Price = num2;
					coupon.SendCount = num;
					coupon.UserLimitCount = int.Parse(this.ddlUserLimitCount.Value);
					coupon.OrderUseLimit = (this.radNoLimit.Checked ? decimal.Zero : num3);
					coupon.StartTime = this.calendarStartDate.SelectedDate.Value;
					CouponInfo couponInfo = coupon;
					dateTime = this.calendarEndDate.SelectedDate.Value;
					DateTime closingTime;
					if (!dateTime.Equals(this.calendarEndDate.SelectedDate.Value.Date))
					{
						closingTime = this.calendarEndDate.SelectedDate.Value;
					}
					else
					{
						dateTime = this.calendarEndDate.SelectedDate.Value;
						dateTime = dateTime.AddDays(1.0);
						closingTime = dateTime.AddSeconds(-1.0);
					}
					couponInfo.ClosingTime = closingTime;
					if (this.radExchange.Checked)
					{
						coupon.NeedPoint = num4;
					}
					switch (CouponHelper.UpdateCoupon2(coupon))
					{
					case CouponActionStatus.UnknowError:
						this.ShowMsg("未知错误", false);
						break;
					case CouponActionStatus.DuplicateName:
						this.ShowMsg("已经存在相同的优惠券名称", false);
						break;
					default:
						this.ShowMsg("修改优惠券成功", true, "NewCoupons.aspx");
						break;
					}
				}
			}
		}
	}
}
