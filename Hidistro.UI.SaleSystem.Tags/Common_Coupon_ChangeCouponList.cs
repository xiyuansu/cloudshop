using Hidistro.UI.Common.Controls;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Coupon_ChangeCouponList : AscxTemplatedWebControl
	{
		public delegate void CommandEventHandler(object sender, RepeaterCommandEventArgs e);

		public const string TagID = "Common_Coupon_ChangeCouponList";

		private Repeater repeaterCoupon;

		public override string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
			}
		}

		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this.repeaterCoupon.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.repeaterCoupon.DataSource = value;
			}
		}

		public event CommandEventHandler ItemCommand;

		public Common_Coupon_ChangeCouponList()
		{
			base.ID = "Common_Coupon_ChangeCouponList";
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_ChangeCouponList.ascx";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.repeaterCoupon = (Repeater)this.FindControl("repeaterCoupon");
			this.repeaterCoupon.ItemCommand += this.repeaterCoupon_ItemCommand;
		}

		private void repeaterCoupon_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			this.ItemCommand(source, e);
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
			if (this.repeaterCoupon.DataSource != null)
			{
				this.repeaterCoupon.DataBind();
			}
		}
	}
}
