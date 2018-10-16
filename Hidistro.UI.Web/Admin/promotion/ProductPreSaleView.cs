using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Promotions;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	[PrivilegeCheck(Privilege.ProductPreSale)]
	public class ProductPreSaleView : AdminPage
	{
		private int preSaleId;

		protected HiddenField hidProductId;

		protected HiddenField hidSelectProducts;

		protected HiddenField hidPreSaleId;

		protected HiddenField hidProductName;

		protected HiddenField hidSalePrice;

		protected Label lblProductName;

		protected Label lblSalePrice;

		protected Label lblDeposit;

		protected Label lblPreSaleEndDate;

		protected Label lblPaymentStartDate;

		protected Label lblPaymentEndDate;

		protected Label lblDelivery;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (!int.TryParse(this.Page.Request.QueryString["preSaleId"], out this.preSaleId))
				{
					base.GotoResourceNotFound();
				}
				else
				{
					ProductPreSaleInfo preSaleInfoWithNameAndPrice = ProductPreSaleHelper.GetPreSaleInfoWithNameAndPrice(this.preSaleId);
					if (preSaleInfoWithNameAndPrice != null)
					{
						this.lblProductName.Text = preSaleInfoWithNameAndPrice.ProductName;
						this.lblSalePrice.Text = "￥" + preSaleInfoWithNameAndPrice.SalePrice.F2ToString("f2");
						if (preSaleInfoWithNameAndPrice.DepositPercent > 0)
						{
							this.lblDeposit.Text = preSaleInfoWithNameAndPrice.DepositPercent + "% (￥" + Math.Round(preSaleInfoWithNameAndPrice.SalePrice * (decimal)preSaleInfoWithNameAndPrice.DepositPercent / 100m, 2) + ")";
						}
						else if (preSaleInfoWithNameAndPrice.Deposit > decimal.Zero)
						{
							this.lblDeposit.Text = "￥" + preSaleInfoWithNameAndPrice.Deposit.ToString();
						}
						Label label = this.lblPreSaleEndDate;
						DateTime dateTime = preSaleInfoWithNameAndPrice.PreSaleEndDate;
						label.Text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
						Label label2 = this.lblPaymentStartDate;
						dateTime = preSaleInfoWithNameAndPrice.PaymentStartDate;
						label2.Text = dateTime.ToString("yyyy-MM-dd");
						Label label3 = this.lblPaymentEndDate;
						dateTime = preSaleInfoWithNameAndPrice.PaymentEndDate;
						label3.Text = dateTime.ToString("yyyy-MM-dd");
						if (preSaleInfoWithNameAndPrice.DeliveryDays > 0)
						{
							this.lblDelivery.Text = "尾款支付后" + preSaleInfoWithNameAndPrice.DeliveryDays.ToString() + "天发货";
						}
						else if (preSaleInfoWithNameAndPrice.DeliveryDate.HasValue)
						{
							Label label4 = this.lblDelivery;
							dateTime = preSaleInfoWithNameAndPrice.DeliveryDate.Value;
							label4.Text = dateTime.ToString("yyyy-MM-dd");
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
}
