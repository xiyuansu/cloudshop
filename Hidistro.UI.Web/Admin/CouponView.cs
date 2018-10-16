using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class CouponView : AdminPage
	{
		private int couponId;

		protected Label lblCouponName;

		protected Label lblPrice;

		protected Label lblSendCount;

		protected Label lblUserLimitCount;

		protected Label lblFullPrice;

		protected Label lblCalendarStartDate;

		protected Label lblCalendarEndDate;

		protected RadioButton radAll;

		protected RadioButton radSomeProducts;

		protected Label lblSelectCount;

		protected RadioButton radActiveReceive;

		protected RadioButton radGrant;

		protected RadioButton radExchange;

		protected Label lblNeedPoint;

		protected CheckBox chkPanicBuying;

		protected CheckBox chkGroup;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidProductIds;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["couponId"], out this.couponId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				CouponInfo eFCoupon = CouponHelper.GetEFCoupon(this.couponId);
				if (eFCoupon != null)
				{
					this.lblCouponName.Text = eFCoupon.CouponName;
					this.lblPrice.Text = eFCoupon.Price.F2ToString("f2");
					Label label = this.lblSendCount;
					int num = eFCoupon.SendCount;
					label.Text = num.ToString();
					Label label2 = this.lblUserLimitCount;
					object text;
					if (eFCoupon.UserLimitCount != 0)
					{
						num = eFCoupon.UserLimitCount;
						text = num.ToString();
					}
					else
					{
						text = "不限";
					}
					label2.Text = (string)text;
					Label label3 = this.lblFullPrice;
					decimal? orderUseLimit = eFCoupon.OrderUseLimit;
					label3.Text = ((orderUseLimit.GetValueOrDefault() > default(decimal) && orderUseLimit.HasValue) ? ("满" + $"{eFCoupon.OrderUseLimit:F2}" + "元使用") : "无限制");
					Label label4 = this.lblCalendarEndDate;
					DateTime dateTime = eFCoupon.ClosingTime;
					label4.Text = dateTime.ToString("yyyy-MM-dd");
					Label label5 = this.lblCalendarStartDate;
					dateTime = eFCoupon.StartTime;
					label5.Text = dateTime.ToString("yyyy-MM-dd");
					if (!string.IsNullOrEmpty(eFCoupon.CanUseProducts))
					{
						this.radSomeProducts.Checked = true;
						DataTable productBaseInfo = ProductHelper.GetProductBaseInfo(eFCoupon.CanUseProducts);
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < productBaseInfo.Rows.Count; i++)
						{
							string text2 = productBaseInfo.Rows[i]["ProductId"].ToString();
							string text3 = productBaseInfo.Rows[i]["ProductName"].ToString();
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append(",,," + text2 + "|||" + text3);
							}
							else
							{
								stringBuilder.Append(text2 + "|||" + text3);
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
						Label label6 = this.lblNeedPoint;
						num = eFCoupon.NeedPoint;
						label6.Text = num.ToString();
					}
					if (eFCoupon.UseWithPanicBuying)
					{
						this.chkPanicBuying.Checked = true;
					}
					if (eFCoupon.UseWithGroup)
					{
						this.chkGroup.Checked = true;
					}
				}
				else
				{
					base.GotoResourceNotFound();
				}
			}
		}
	}
}
