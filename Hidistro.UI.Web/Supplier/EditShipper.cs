using Hidistro.Context;
using Hidistro.Core;
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
	public class EditShipper : SupplierAdminPage
	{
		private int shipperId;

		protected HiddenField hidIsDefault;

		protected HiddenField hidIsDefaultGetGoods;

		protected TextBox txtShipperTag;

		protected TextBox txtShipperName;

		protected RegionSelector ddlReggion;

		protected TextBox txtAddress;

		protected TextBox txtCellPhone;

		protected HtmlGenericControl showOpenId;

		protected TextBox txtWxOpenId;

		protected HtmlAnchor reGetOpenId;

		protected HtmlGenericControl getOpenId;

		protected Image OpenIdQrCodeImg;

		protected TextBox txtTelPhone;

		protected TextBox txtZipcode;

		protected TextBox txtRemark;

		protected Button btnEditShipper;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ShipperId"], out this.shipperId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnEditShipper.Click += this.btnEditShipper_Click;
				if (!this.Page.IsPostBack)
				{
					ShippersInfo shipper = SalesHelper.GetShipper(this.shipperId);
					if (shipper == null || shipper.SupplierId != HiContext.Current.Manager.StoreId)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						Globals.EntityCoding(shipper, false);
						HiddenField hiddenField = this.hidIsDefault;
						bool flag = shipper.IsDefault;
						hiddenField.Value = flag.ToString();
						HiddenField hiddenField2 = this.hidIsDefaultGetGoods;
						flag = shipper.IsDefaultGetGoods;
						hiddenField2.Value = flag.ToString();
						this.txtShipperTag.Text = shipper.ShipperTag;
						this.txtShipperName.Text = shipper.ShipperName;
						this.ddlReggion.SetSelectedRegionId(shipper.RegionId);
						this.txtAddress.Text = shipper.Address;
						this.txtCellPhone.Text = shipper.CellPhone;
						this.txtTelPhone.Text = shipper.TelPhone;
						this.txtZipcode.Text = shipper.Zipcode;
						this.txtRemark.Text = shipper.Remark;
						this.txtWxOpenId.Text = shipper.WxOpenId;
					}
				}
			}
		}

		private void btnEditShipper_Click(object sender, EventArgs e)
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.shipperId);
			if (shipper == null || shipper.SupplierId != HiContext.Current.Manager.StoreId)
			{
				base.GotoResourceNotFound();
			}
			else
			{
				shipper.ShipperId = this.shipperId;
				shipper.ShipperTag = this.txtShipperTag.Text.Trim();
				shipper.ShipperName = this.txtShipperName.Text.Trim();
				if (!this.ddlReggion.GetSelectedRegionId().HasValue)
				{
					this.ShowMsg("请选择地区", false);
				}
				else
				{
					shipper.IsDefault = bool.Parse(this.hidIsDefault.Value);
					shipper.IsDefaultGetGoods = bool.Parse(this.hidIsDefaultGetGoods.Value);
					shipper.RegionId = this.ddlReggion.GetSelectedRegionId().Value;
					shipper.Address = this.txtAddress.Text.Trim();
					shipper.CellPhone = this.txtCellPhone.Text.Trim();
					shipper.TelPhone = this.txtTelPhone.Text.Trim();
					shipper.Zipcode = this.txtZipcode.Text.Trim();
					shipper.Remark = this.txtRemark.Text.Trim();
					if (this.ValidationShipper(shipper))
					{
						if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
						{
							this.ShowMsg("手机号码和电话号码必填其一", false);
						}
						else if (SalesHelper.UpdateShipper(shipper))
						{
							this.ShowMsg("成功修改了一个发货信息", true, "Shippers.aspx");
						}
						else
						{
							this.ShowMsg("修改发货信息失败", false);
						}
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
