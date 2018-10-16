using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Coupons)]
	public class ChooseCoupons : AdminPage
	{
		public int havedCouponsNum = 0;

		protected string NotInCouponIds;

		protected string CouponName = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			this.NotInCouponIds = this.Page.Request["CouponIds"].ToNullString();
			this.havedCouponsNum = this.NotInCouponIds.Split(',').Length - 2;
			this.CouponName = Globals.UrlDecode(this.Page.Request["CouponName"].ToNullString());
		}
	}
}
