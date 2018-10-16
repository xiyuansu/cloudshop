using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Member;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[ParseChildren(true)]
	public class WAPChangeMapPosition : WAPTemplatedWebControl
	{
		private HtmlInputHidden hidQQMapApiKey;

		private HtmlGenericControl spanCityName;

		private HtmlInputHidden hidShippingId;

		private HtmlInputHidden hidLatitude;

		private HtmlInputHidden hidLongitude;

		private HtmlInputHidden hidShipTo;

		private HtmlInputHidden hidCellphone;

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-ChangeMapPosition.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("设置收货地址");
			this.hidQQMapApiKey = (HtmlInputHidden)this.FindControl("hidQQMapApiKey");
			this.spanCityName = (HtmlGenericControl)this.FindControl("spanCityName");
			this.hidShippingId = (HtmlInputHidden)this.FindControl("hidShippingId");
			this.hidLatitude = (HtmlInputHidden)this.FindControl("hidLatitude");
			this.hidLongitude = (HtmlInputHidden)this.FindControl("hidLongitude");
			this.hidShipTo = (HtmlInputHidden)this.FindControl("hidShipTo");
			this.hidCellphone = (HtmlInputHidden)this.FindControl("hidCellphone");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string value = masterSettings.QQMapAPIKey;
			if (string.IsNullOrEmpty(value))
			{
				value = "SYJBZ-DSLR3-IWX3Q-3XNTM-ELURH-23FTP";
			}
			this.hidQQMapApiKey.Value = value;
			int num = this.Page.Request.QueryString["ShippingId"].ToInt(0);
			this.hidShippingId.Value = num.ToNullString();
			this.hidShipTo.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["shipTo"]);
			this.hidCellphone.Value = HttpUtility.UrlDecode(this.Page.Request.QueryString["cellphone"]);
			this.spanCityName.InnerText = "选择城市";
			string text = HttpUtility.UrlDecode(this.Page.Request.QueryString["cityName"]);
			if (!string.IsNullOrWhiteSpace(text))
			{
				this.spanCityName.InnerText = HttpUtility.UrlDecode(text);
			}
			else
			{
				if (num > 0)
				{
					ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(num);
					if (shippingAddress.LatLng != null && shippingAddress.LatLng.Split(',').Length == 2)
					{
						if (this.hidLatitude != null)
						{
							this.hidLatitude.Value = shippingAddress.LatLng.Split(',')[0];
						}
						if (this.hidLongitude != null)
						{
							this.hidLongitude.Value = shippingAddress.LatLng.Split(',')[1];
						}
						this.spanCityName.InnerText = RegionHelper.GetFullRegion(shippingAddress.RegionId, ",", false, 2).Split(',')[0];
					}
				}
				double num2 = this.Page.Request.QueryString["latitude"].ToDouble(0);
				double num3 = this.Page.Request.QueryString["longitude"].ToDouble(0);
				string text2 = HttpUtility.UrlDecode(this.Page.Request.QueryString["chooseCity"]);
				if (num2 > 0.0 && num3 > 0.0)
				{
					this.hidLatitude.Value = num2.ToNullString();
					this.hidLongitude.Value = num3.ToNullString();
					if (!string.IsNullOrWhiteSpace(text2))
					{
						this.spanCityName.InnerText = HttpUtility.UrlDecode(text2);
					}
				}
			}
		}
	}
}
