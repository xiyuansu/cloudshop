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
	public class ProductPreSaleOrderList : AdminPage
	{
		protected int preSaleId;

		public string userName = "";

		protected Literal litProductSaleAmount;

		protected Literal litPayAllProductAmount;

		protected Literal litPayDepositTotal;

		protected Literal litFinalPaymentTotal;

		protected Literal litAllTotal;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["preSaleId"], out this.preSaleId))
			{
				base.GotoResourceNotFound();
			}
			else if (!this.Page.IsPostBack)
			{
				ProductPreSaleInfo productPreSaleInfo = ProductPreSaleHelper.GetProductPreSaleInfo(this.preSaleId);
				if (productPreSaleInfo == null || productPreSaleInfo.ProductId <= 0)
				{
					base.GotoResourceNotFound();
				}
				else
				{
					Literal literal = this.litProductSaleAmount;
					int num = ProductPreSaleHelper.GetPreSaleProductAmount(this.preSaleId);
					literal.Text = num.ToString();
					Literal literal2 = this.litPayAllProductAmount;
					num = ProductPreSaleHelper.GetPreSalePayFinalPaymentAmount(this.preSaleId);
					literal2.Text = num.ToString();
					this.litPayDepositTotal.Text = ProductPreSaleHelper.GetPayDepositTotal(this.preSaleId).F2ToString("f2");
					this.litFinalPaymentTotal.Text = ProductPreSaleHelper.GetPayFinalPaymentTotal(this.preSaleId).F2ToString("f2");
					this.litAllTotal.Text = (ProductPreSaleHelper.GetPayFinalPaymentTotal(this.preSaleId) + ProductPreSaleHelper.GetPayDepositTotal(this.preSaleId)).F2ToString("f2");
				}
			}
		}
	}
}
