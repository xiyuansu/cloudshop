using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Shippers)]
	public class EditShipper : AdminPage
	{
		private int shipperId;

		protected HiddenField hidIsDefault;

		protected HiddenField hidIsDefaultGetGoods;

		protected TextBox txtShipperTag;

		protected TextBox txtShipperName;

		protected RegionSelector ddlReggion;

		protected TrimTextBox txtAddress;

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

		protected HiddenField hfLongitude;

		protected HiddenField hfLatitude;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ShipperId"], out this.shipperId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				if (HiContext.Current.SiteSettings.OpenVstore == 0)
				{
					this.showOpenId.Style.Add("display", "none");
					this.getOpenId.Style.Add("display", "none");
				}
				this.btnEditShipper.Click += this.btnEditShipper_Click;
				if (!this.Page.IsPostBack)
				{
					ShippersInfo shipper = SalesHelper.GetShipper(this.shipperId);
					if (shipper == null || shipper.SupplierId != 0)
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
						HiddenField hiddenField3 = this.hfLongitude;
						double? nullable = shipper.Longitude;
						hiddenField3.Value = nullable.ToString();
						HiddenField hiddenField4 = this.hfLatitude;
						nullable = shipper.Latitude;
						hiddenField4.Value = nullable.ToString();
						if (HiContext.Current.SiteSettings.OpenVstore != 0)
						{
							string text = "";
							if (HiContext.Current.Manager != null)
							{
								text = HiContext.Current.Manager.ManagerId.ToString();
							}
							string content = Globals.FullPath("/Vshop/GetOpenId.aspx?adminName=" + text);
							string qrCodeUrl = "/Storage/master/QRCode/" + HiContext.Current.SiteSettings.SiteUrl.ToLower().Replace("https://", "").Replace("http://", "")
								.Replace(".", "") + "_" + text + "_OpenId.png";
							this.OpenIdQrCodeImg.ImageUrl = Globals.CreateQRCode(content, qrCodeUrl, false, ImageFormats.Png);
							if (string.IsNullOrEmpty(shipper.WxOpenId))
							{
								this.reGetOpenId.Style.Add("display", "none");
							}
							else
							{
								this.getOpenId.Style.Add("display", "none");
							}
						}
					}
				}
			}
		}

		private void btnEditShipper_Click(object sender, EventArgs e)
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.shipperId);
			if (shipper == null || shipper.SupplierId != 0)
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
					shipper.WxOpenId = Globals.StripAllTags(this.txtWxOpenId.Text.Trim());
					if (this.ValidationShipper(shipper))
					{
						if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
						{
							this.ShowMsg("手机号码和电话号码必填其一", false);
						}
						else
						{
							string[] array = this.ddlReggion.SelectedRegions.Split('，');
							if (array.Length < 4)
							{
								this.ShowMsg("请将发货地址填写完整", false);
							}
							else if (SalesHelper.UpdateShipper(shipper))
							{
								SiteSettings masterSettings = SettingsManager.GetMasterSettings();
								string phone = (!string.IsNullOrEmpty(shipper.CellPhone)) ? shipper.CellPhone : shipper.TelPhone;
								double lng = shipper.Longitude.ToDouble(0);
								double lat = shipper.Latitude.ToDouble(0);
								string city_name = array[1].Replace("市", "");
								string area_name = array[2];
								string str = array[3];
								string text = DadaHelper.shopAddOrUpdate(masterSettings.DadaSourceID, masterSettings.SiteName, 5, city_name, area_name, str + " " + shipper.Address, lng, lat, shipper.ShipperName, phone, "P_" + shipper.ShipperId, 1);
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
