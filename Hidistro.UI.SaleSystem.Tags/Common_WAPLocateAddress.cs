using Hidistro.Context;
using Hidistro.Entities;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_WAPLocateAddress : WAPTemplatedWebControl
	{
		private Literal txtRegionName;

		private HtmlInputControl txtRegionId;

		private HtmlTextArea txtAddress;

		private HtmlInputHidden txtIsDeliveryScopeRegion;

		private HtmlTextArea txtBuildingNumber;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings();

		private bool _IsDeliveryScopeRegion;

		private bool _ShowAddress = true;

		private bool _OnlyShowInDefault;

		private string _separator = " ";

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

		public static string TagId
		{
			get
			{
				return "WAPLocateAddress";
			}
		}

		private bool IsShow
		{
			get
			{
				string text = HttpContext.Current.Request.ServerVariables["Script_Name"];
				if (this.OnlyShowInDefault && text.IndexOf("default.aspx") == -1)
				{
					goto IL_003d;
				}
				if (!this.siteSettings.OpenMultStore)
				{
					goto IL_003d;
				}
				return true;
				IL_003d:
				return false;
			}
		}

		public int? RegionId
		{
			get;
			set;
		}

		public bool ShowAddress
		{
			get
			{
				return this._ShowAddress;
			}
			set
			{
				this._ShowAddress = value;
			}
		}

		public string Address
		{
			get;
			set;
		}

		public string BuildingNumber
		{
			get;
			set;
		}

		public bool OnlyShowInDefault
		{
			get
			{
				return this._OnlyShowInDefault;
			}
			set
			{
				this._OnlyShowInDefault = value;
			}
		}

		public string separator
		{
			get
			{
				return this._separator;
			}
			set
			{
				this._separator = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "/Tags/Skin-Common_LocateAddress.html";
			}
			base.ID = Common_WAPLocateAddress.TagId;
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

		public void SetLocateAddres(int? regionId, string address, string buildingNumber)
		{
			this.Address = address;
			this.RegionId = regionId;
			this.BuildingNumber = buildingNumber;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}

		protected override void AttachChildControls()
		{
			this.txtRegionName = (Literal)this.FindControl("regionName");
			this.txtRegionId = (HtmlInputControl)this.FindControl("region");
			this.txtAddress = (HtmlTextArea)this.FindControl("address");
			this.txtIsDeliveryScopeRegion = (HtmlInputHidden)this.FindControl("IsDeliveryScopeRegion");
			this.txtBuildingNumber = (HtmlTextArea)this.FindControl("txtBuildingNumber");
			if (this.txtIsDeliveryScopeRegion != null)
			{
				this.txtIsDeliveryScopeRegion.Value = this.IsDeliveryScopeRegion.ToString().ToLower();
			}
			int? regionId = this.RegionId;
			if (regionId.HasValue)
			{
				if (this.txtRegionId != null)
				{
					HtmlInputControl htmlInputControl = this.txtRegionId;
					regionId = this.RegionId;
					htmlInputControl.Value = regionId.ToString();
				}
				if (this.txtRegionName != null)
				{
					Literal literal = this.txtRegionName;
					regionId = this.RegionId;
					literal.Text = RegionHelper.GetFullRegion(regionId.Value, this.separator, true, 0);
				}
			}
			else if (this.txtRegionName != null)
			{
				this.txtRegionName.Text = "请选择省市区";
			}
			if (string.IsNullOrEmpty(this.txtRegionName.Text))
			{
				this.txtRegionName.Text = "请选择省市区";
			}
			if (this.ShowAddress)
			{
				if (this.txtAddress != null)
				{
					this.txtAddress.Value = this.Address;
				}
				if (this.txtBuildingNumber != null)
				{
					this.txtBuildingNumber.Value = this.BuildingNumber;
				}
			}
			else
			{
				this.txtAddress.Visible = false;
			}
		}
	}
}
