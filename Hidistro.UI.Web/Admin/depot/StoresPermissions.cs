using Hidistro.Core;
using Hidistro.Entities.Depot;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.depot
{
	public class StoresPermissions : AdminPage
	{
		private int storeId = 0;

		protected HiLabel lblStoreName;

		protected OnOff IsShelvesProduct;

		protected OnOff IsModifyPrice;

		protected TextBox txtMinPriceRate;

		protected TextBox txtMaxPriceRate;

		protected OnOff IsRequestBlance;

		protected Button btnOK;

		[AdministerCheck(true)]
		protected void Page_Load(object sender, EventArgs e)
		{
			this.IsModifyPrice.Parameter.Add("onSwitchChange", "fuCheckEnablePrice");
			this.storeId = base.Request.QueryString["StoreId"].ToInt(0);
			if (this.storeId == 0)
			{
				base.Response.Redirect("StoresList.aspx");
			}
			this.btnOK.Click += this.btnOK_Click;
			if (!this.Page.IsPostBack)
			{
				StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
				this.lblStoreName.Text = storeById.StoreName;
				this.IsShelvesProduct.SelectedValue = storeById.IsShelvesProduct;
				this.IsModifyPrice.SelectedValue = storeById.IsModifyPrice;
				this.txtMinPriceRate.Text = storeById.MinPriceRate.ToNullString();
				this.txtMaxPriceRate.Text = storeById.MaxPriceRate.ToNullString();
				this.IsRequestBlance.SelectedValue = storeById.IsRequestBlance;
			}
		}

		protected void btnOK_Click(object sender, EventArgs e)
		{
			if (this.IsModifyPrice.SelectedValue)
			{
				if (this.txtMinPriceRate.Text.ToDecimal(0) > 100m || this.txtMinPriceRate.Text.ToDecimal(0) < decimal.Zero)
				{
					this.ShowMsg("请输入正确的最小价格倍数！", false);
					return;
				}
				if (this.txtMaxPriceRate.Text.ToDecimal(0) > 100m || this.txtMaxPriceRate.Text.ToDecimal(0) < decimal.Zero)
				{
					this.ShowMsg("请输入正确的最大价格倍数！", false);
					return;
				}
				if (this.txtMaxPriceRate.Text.ToDecimal(0) < this.txtMinPriceRate.Text.ToDecimal(0))
				{
					this.ShowMsg("最大价格倍数需大于最小价格倍数！", false);
					return;
				}
			}
			decimal? d = null;
			decimal? d2 = null;
			if (this.IsModifyPrice.SelectedValue)
			{
				d = (string.IsNullOrEmpty(this.txtMinPriceRate.Text) ? null : new decimal?(this.txtMinPriceRate.Text.ToDecimal(0)));
				d2 = (string.IsNullOrEmpty(this.txtMaxPriceRate.Text) ? null : new decimal?(this.txtMaxPriceRate.Text.ToDecimal(0)));
			}
			StoresInfo storeById = StoresHelper.GetStoreById(this.storeId);
			storeById.IsShelvesProduct = this.IsShelvesProduct.SelectedValue;
			storeById.IsModifyPrice = this.IsModifyPrice.SelectedValue;
			storeById.MinPriceRate = Math.Floor((d * (decimal?)100).ToDecimal(0)) / 100m;
			storeById.MaxPriceRate = Math.Floor((d2 * (decimal?)100).ToDecimal(0)) / 100m;
			storeById.IsRequestBlance = this.IsRequestBlance.SelectedValue;
			StoresHelper.UpdateStore(storeById);
			this.ShowMsg("保存成功", true);
		}
	}
}
