using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class CouponDropDownList : DropDownList
	{
		public void BindCoupons()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem
			{
				Text = "请选择优惠券",
				Value = "0"
			});
			IList<CouponInfo> allUsedCoupons = CouponHelper.GetAllUsedCoupons(1);
			allUsedCoupons.ForEach(delegate(CouponInfo x)
			{
				if (CouponHelper.GetCouponSurplus(x.CouponId) > 0)
				{
					this.Items.Add(new ListItem
					{
						Text = x.CouponName,
						Value = x.CouponId.ToString()
					});
				}
			});
		}
	}
}
