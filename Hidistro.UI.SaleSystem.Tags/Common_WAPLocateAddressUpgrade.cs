using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_WAPLocateAddressUpgrade : WAPTemplatedWebControl
	{
		private Literal txtRegionLocation;

		private Literal txtRegionName;

		private HtmlTextArea txtAddress;

		private HtmlInputHidden hidShippingId;

		private HtmlInputHidden hidChooseCity;

		private HtmlInputHidden hidRegionId;

		private HtmlInputHidden hidLatitude;

		private HtmlInputHidden hidLongitude;

		private HtmlTextArea txtBuildingNumber;

		private bool _IsDeliveryScopeRegion;

		public bool IsDeliveryScopeRegion
		{
			get
			{
				return this._IsDeliveryScopeRegion;
			}
			set
			{
				this._IsDeliveryScopeRegion = value;
			}
		}

		public string BuildingNumber
		{
			get;
			set;
		}

		public static string TagId
		{
			get
			{
				return "WAPLocateAddressUpgrade";
			}
		}

		public int? RegionId
		{
			get;
			set;
		}

		public string LatLng
		{
			get;
			set;
		}

		public string RegionLocation
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/Tags/Skin-Common_LocateAddressUpgrade.html";
			}
			base.ID = Common_WAPLocateAddressUpgrade.TagId;
			string text = HttpContext.Current.Request.Url.ToString().ToLower();
			if (text.Contains("/vshop/"))
			{
				base.ClientType = ClientType.VShop;
			}
			else if (text.Contains("/alioh/"))
			{
				base.ClientType = ClientType.AliOH;
			}
			else
			{
				base.ClientType = ClientType.WAP;
			}
			base.OnInit(e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}

		protected override void AttachChildControls()
		{
			this.txtRegionLocation = (Literal)this.FindControl("regionLocation");
			this.txtRegionName = (Literal)this.FindControl("regionName");
			this.txtAddress = (HtmlTextArea)this.FindControl("address");
			this.txtBuildingNumber = (HtmlTextArea)this.FindControl("txtBuildingNumber");
			this.hidShippingId = (HtmlInputHidden)this.FindControl("hidShippingId");
			this.hidChooseCity = (HtmlInputHidden)this.FindControl("hidChooseCity");
			this.hidRegionId = (HtmlInputHidden)this.FindControl("region");
			this.hidLatitude = (HtmlInputHidden)this.FindControl("hidLatitude");
			this.hidLongitude = (HtmlInputHidden)this.FindControl("hidLongitude");
			int? regionId = this.RegionId;
			if (regionId.HasValue)
			{
				if (this.hidRegionId != null)
				{
					HtmlInputHidden htmlInputHidden = this.hidRegionId;
					regionId = this.RegionId;
					htmlInputHidden.Value = regionId.ToString();
				}
				if (this.txtAddress != null)
				{
					this.txtAddress.Value = this.Address;
				}
				if (this.LatLng != null && this.LatLng.Split(',').Length == 2)
				{
					if (this.hidLatitude != null)
					{
						this.hidLatitude.Value = this.LatLng.Split(',')[0];
					}
					if (this.hidLongitude != null)
					{
						this.hidLongitude.Value = this.LatLng.Split(',')[1];
					}
					if (this.txtRegionName != null)
					{
						Literal literal = this.txtRegionName;
						regionId = this.RegionId;
						literal.Text = RegionHelper.GetFullRegion(regionId.Value, " ", false, 2);
					}
					if (this.txtRegionLocation != null && !string.IsNullOrEmpty(this.RegionLocation))
					{
						this.txtRegionLocation.Text = this.RegionLocation;
					}
					else
					{
						this.txtRegionLocation.Text = "请选择地区";
					}
				}
			}
			int num = this.Page.Request.QueryString["ShippingId"].ToInt(0);
			this.hidShippingId.Value = num.ToNullString();
			double num2 = this.Page.Request.QueryString["region"].ToDouble(0);
			double num3 = this.Page.Request.QueryString["latitude"].ToDouble(0);
			double num4 = this.Page.Request.QueryString["longitude"].ToDouble(0);
			string text = HttpUtility.UrlDecode(this.Page.Request.QueryString["regionLocation"]);
			string value = HttpUtility.UrlDecode(this.Page.Request.QueryString["address"]);
			string value2 = HttpUtility.UrlDecode(this.Page.Request.QueryString["chooseCity"]);
			this.txtBuildingNumber.Value = this.BuildingNumber;
			if (num3 > 0.0 && num4 > 0.0)
			{
				this.hidRegionId.Value = num2.ToNullString();
				this.hidLatitude.Value = num3.ToNullString();
				this.hidLongitude.Value = num4.ToNullString();
				this.txtRegionLocation.Text = text;
				this.txtAddress.Value = value;
				this.hidChooseCity.Value = value2;
			}
		}
	}
}
