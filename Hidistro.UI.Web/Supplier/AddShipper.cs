using Hidistro.Context;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier
{
	[AdministerCheck(true)]
	public class AddShipper : SupplierAdminPage
	{
		protected TextBox txtShipperTag;

		protected TextBox txtShipperName;

		protected RegionSelector ddlReggion;

		protected TextBox txtAddress;

		protected TextBox txtCellPhone;

		protected HtmlGenericControl showOpenId;

		protected TextBox txtWxOpenId;

		protected HtmlGenericControl getOpenId;

		protected Image OpenIdQrCodeImg;

		protected TextBox txtTelPhone;

		protected TextBox txtZipcode;

		protected TextBox txtRemark;

		protected Button btnAddShipper;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAddShipper.Click += this.btnAddShipper_Click;
		}

		private void btnAddShipper_Click(object sender, EventArgs e)
		{
			int storeId = HiContext.Current.Manager.StoreId;
			ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(storeId);
			ShippersInfo defaultGetGoodsShipperBysupplierId = SalesHelper.GetDefaultGetGoodsShipperBysupplierId(storeId);
			ShippersInfo shippersInfo = new ShippersInfo();
			shippersInfo.SupplierId = storeId;
			shippersInfo.ShipperTag = this.txtShipperTag.Text.Trim();
			shippersInfo.ShipperName = this.txtShipperName.Text.Trim();
			if (!this.ddlReggion.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择地区", false);
			}
			else
			{
				shippersInfo.RegionId = this.ddlReggion.GetSelectedRegionId().Value;
				shippersInfo.Address = this.txtAddress.Text.Trim();
				shippersInfo.CellPhone = this.txtCellPhone.Text.Trim();
				shippersInfo.TelPhone = this.txtTelPhone.Text.Trim();
				shippersInfo.Zipcode = this.txtZipcode.Text.Trim();
				shippersInfo.IsDefault = (defaultOrFirstShipper == null || !defaultOrFirstShipper.IsDefault);
				shippersInfo.IsDefaultGetGoods = (defaultGetGoodsShipperBysupplierId == null || !defaultGetGoodsShipperBysupplierId.IsDefaultGetGoods);
				shippersInfo.Remark = this.txtRemark.Text.Trim();
				shippersInfo.WxOpenId = "";
				if (this.ValidationShipper(shippersInfo))
				{
					if (string.IsNullOrEmpty(shippersInfo.CellPhone) && string.IsNullOrEmpty(shippersInfo.TelPhone))
					{
						this.ShowMsg("手机号码和电话号码必填其一", false);
					}
					else if (SalesHelper.AddShipper(shippersInfo))
					{
						this.ShowMsg("成功添加了一个发货信息", true, "Shippers.aspx");
					}
					else
					{
						this.ShowMsg("添加发货信息失败", false);
					}
				}
			}
		}

		private bool ValidationShipper(ShippersInfo shipper)
		{
			ValidationResults validationResults = Validation.Validate(shipper, "Valshipper");
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(item.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
