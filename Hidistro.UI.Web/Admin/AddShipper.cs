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
	[PrivilegeCheck(Privilege.AddShipper)]
	public class AddShipper : AdminPage
	{
		protected TextBox txtShipperTag;

		protected TextBox txtShipperName;

		protected RegionSelector ddlReggion;

		protected TrimTextBox txtAddress;

		protected TextBox txtCellPhone;

		protected HtmlGenericControl showOpenId;

		protected TextBox txtWxOpenId;

		protected HtmlGenericControl getOpenId;

		protected Image OpenIdQrCodeImg;

		protected TextBox txtTelPhone;

		protected TextBox txtZipcode;

		protected TextBox txtRemark;

		protected Button btnAddShipper;

		protected HiddenField hfLongitude;

		protected HiddenField hfLatitude;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.OnInitComplete(e);
			if (HiContext.Current.SiteSettings.OpenVstore == 0)
			{
				this.showOpenId.Style.Add("display", "none");
				this.getOpenId.Style.Add("display", "none");
			}
			else
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
			}
			this.btnAddShipper.Click += this.btnAddShipper_Click;
		}

		private void btnAddShipper_Click(object sender, EventArgs e)
		{
			ShippersInfo defaultOrFirstShipper = SalesHelper.GetDefaultOrFirstShipper(0);
			ShippersInfo defaultGetGoodsShipperBysupplierId = SalesHelper.GetDefaultGetGoodsShipperBysupplierId(0);
			ShippersInfo shippersInfo = new ShippersInfo();
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
				shippersInfo.WxOpenId = Globals.StripAllTags(this.txtWxOpenId.Text.Trim());
				shippersInfo.SupplierId = 0;
				shippersInfo.Longitude = Math.Round(double.Parse(string.IsNullOrEmpty(this.hfLongitude.Value) ? "0" : this.hfLongitude.Value), 6);
				shippersInfo.Latitude = Math.Round(double.Parse(string.IsNullOrEmpty(this.hfLatitude.Value) ? "0" : this.hfLatitude.Value), 6);
				if (this.ValidationShipper(shippersInfo))
				{
					if (string.IsNullOrEmpty(shippersInfo.CellPhone) && string.IsNullOrEmpty(shippersInfo.TelPhone))
					{
						this.ShowMsg("手机号码和电话号码必填其一", false);
					}
					else
					{
						long num = SalesHelper.AddShipperRetrunID(shippersInfo);
						if (num > 0)
						{
							try
							{
								SiteSettings masterSettings = SettingsManager.GetMasterSettings();
								string phone = (!string.IsNullOrEmpty(shippersInfo.CellPhone)) ? shippersInfo.CellPhone : shippersInfo.TelPhone;
								string text = "0";
								double lng = shippersInfo.Longitude.ToDouble(0);
								double lat = shippersInfo.Latitude.ToDouble(0);
								string[] array = this.ddlReggion.SelectedRegions.Split('，');
								string city_name = array[1].Replace("市", "");
								string area_name = array[2];
								string str = array[3];
								string text2 = DadaHelper.shopAddOrUpdate(masterSettings.DadaSourceID, masterSettings.SiteName, 5, city_name, area_name, str + " " + shippersInfo.Address, lng, lat, shippersInfo.ShipperName, phone, "P_" + num, 1);
							}
							catch (Exception)
							{
								this.ShowMsg("添加达达物流信息失败", false);
							}
							this.ShowMsg("成功添加了一个发货信息", true, "Shippers.aspx");
						}
						else
						{
							this.ShowMsg("添加发货信息失败", false);
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
