using Hidistro.SaleSystem.Depot;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class BatchEditCommissionRate : AdminCallBackPage
	{
		private string storesIds;

		protected TextBox txtCommissionRate;

		protected Button btnSubmitBatch;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnSubmitBatch.Click += this.btnSubmitBatch_Click;
			this.storesIds = this.Page.Request.QueryString["StoresIds"];
		}

		protected void btnSubmitBatch_Click(object sender, EventArgs e)
		{
			decimal num = default(decimal);
			if (!decimal.TryParse(this.txtCommissionRate.Text.Trim(), out num) || num > 100m || num < decimal.Zero)
			{
				this.ShowMsg("请输入正确的平台抽佣比例", false);
			}
			else if (StoresHelper.BatchEditCommissionRate(num, this.storesIds))
			{
				base.CloseWindow(null);
				this.ShowMsgCloseWindow("批量操作成功", true);
			}
			else
			{
				base.CloseWindow(null);
				this.ShowMsg("批量操作失败", false);
			}
		}
	}
}
