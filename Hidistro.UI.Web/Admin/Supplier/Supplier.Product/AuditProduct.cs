using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Supplier.Product
{
	[PrivilegeCheck(Privilege.SupplierAudit)]
	public class AuditProduct : AdminPage
	{
		private string productIds = string.Empty;

		protected HtmlForm form1;

		protected TextBox tbxReson;

		protected Button btnPass;

		protected Button btnFailed;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
		}

		protected void btnPass_Click(object sender, EventArgs e)
		{
			if (ProductHelper.AuditProducts(this.productIds, this.tbxReson.Text, true))
			{
				this.ShowMsg("审核成功,商品已默认上架！", true);
				this.btnFailed.Enabled = false;
				this.btnPass.Enabled = false;
			}
			else
			{
				this.ShowMsg("操作失败，可能商品没有设置一口价，或者供应商状态错误！", false);
			}
		}

		protected void btnFailed_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.tbxReson.Text))
			{
				this.ShowMsg("拒绝时必须填写拒绝理由", false);
			}
			else if (ProductHelper.AuditProducts(this.productIds, this.tbxReson.Text, false))
			{
				this.ShowMsg("操作成功！", true);
				this.btnFailed.Enabled = false;
				this.btnPass.Enabled = false;
				this.CloseWindow();
			}
			else
			{
				this.ShowMsg("操作失败", false);
			}
		}
	}
}
