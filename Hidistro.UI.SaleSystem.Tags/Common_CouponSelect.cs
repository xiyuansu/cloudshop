using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_CouponSelect : WebControl
	{
		public decimal CartTotal
		{
			get;
			set;
		}

		public IList<ShoppingCartItemInfo> itemList
		{
			get;
			set;
		}

		public string className
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			DataTable coupon = ShoppingProcessor.GetCoupon(this.CartTotal, this.itemList, false, false, false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<ul " + (string.IsNullOrEmpty(this.className) ? "" : ("class=\"" + this.className + "\"")) + ">");
			if (coupon != null && coupon.Rows.Count > 0)
			{
				stringBuilder.AppendLine("<li><input type=\"radio\" value=\"\" class=\"icheck\" name=\"radCoupon\" amount=\"0\" discount=\"0\"><i class=\"icon_coupon\"></i>不使用优惠券</li>");
			}
			foreach (DataRow row in coupon.Rows)
			{
				stringBuilder.AppendFormat("<li><input type=\"radio\" value=\"{3}\" class=\"icheck\" name=\"radCoupon\" amount=\"{1}\" discount=\"{2}\"><i class=\"icon_coupon\"></i>{0}(满{1}减{2}</li>", row["CouponName"], ((decimal)row["OrderUseLimit"]).F2ToString("f2"), ((decimal)row["Price"]).F2ToString("f2"), row["ClaimCode"].ToNullString()).AppendLine();
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
