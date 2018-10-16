using Hidistro.Context;
using Hidistro.Entities.Store;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class RegionSelector : WebControl
	{
		private static string _IDPrev = "";

		private int? currentRegionId;

		private bool dataLoaded = false;

		private bool _CustomerJs = false;

		private bool _CustomerCss = false;

		private bool _IsShowClear = true;

		private bool _DisplayStreet = true;

		private WebControl ddlProvinces;

		private WebControl ddlCitys;

		private WebControl ddlCountys;

		private WebControl ddlStreets;

		private WebControl proviceSpan;

		private WebControl citySpan;

		private WebControl areaSpan;

		private WebControl streetSpan;

		private WebControl proviceDiv;

		private WebControl cityDiv;

		private WebControl areaDiv;

		private WebControl streetDiv;

		private int? provinceId;

		private int? cityId;

		private int? countyId;

		private int? streetId;

		public bool IsShift
		{
			get;
			set;
		}

		public string ProvinceTitle
		{
			get;
			set;
		}

		public string CityTitle
		{
			get;
			set;
		}

		public string CountyTitle
		{
			get;
			set;
		}

		public string StreetTitle
		{
			get;
			set;
		}

		public int ProvinceWidth
		{
			get;
			set;
		}

		public int CityWidth
		{
			get;
			set;
		}

		public int CountyWidth
		{
			get;
			set;
		}

		public int StreetWidth
		{
			get;
			set;
		}

		public bool IsShowClear
		{
			get
			{
				return this._IsShowClear;
			}
			set
			{
				this._IsShowClear = value;
			}
		}

		public static string IDPrev
		{
			get
			{
				return RegionSelector._IDPrev;
			}
			set
			{
				RegionSelector._IDPrev = value;
			}
		}

		public bool CustomerJs
		{
			get
			{
				return this._CustomerJs;
			}
			set
			{
				this._CustomerJs = value;
			}
		}

		public bool CustomerCss
		{
			get
			{
				return this._CustomerCss;
			}
			set
			{
				this._CustomerCss = value;
			}
		}

		public string Separator
		{
			get;
			set;
		}

		private string SelectedRegionName
		{
			get
			{
				if (this.currentRegionId.HasValue)
				{
					return RegionHelper.GetFullRegion(this.currentRegionId.Value, " ", true, 0);
				}
				return "";
			}
		}

		public string SelectedRegions
		{
			get
			{
				int? selectedRegionId = this.GetSelectedRegionId();
				if (!selectedRegionId.HasValue)
				{
					return "";
				}
				return RegionHelper.GetFullRegion(selectedRegionId.Value, this.Separator, true, 0);
			}
			set
			{
				string[] array = value.Split(',');
				if (array.Length >= 3)
				{
					int? selectedRegionId = RegionHelper.GetRegionId(array[2], array[1], array[0]);
					this.SetSelectedRegionId(selectedRegionId);
				}
			}
		}

		public bool DisplayStreet
		{
			get
			{
				return this._DisplayStreet;
			}
			set
			{
				this._DisplayStreet = value;
			}
		}

		public string NullToDisplay
		{
			get;
			set;
		}

		public override ControlCollection Controls
		{
			get
			{
				base.EnsureChildControls();
				return base.Controls;
			}
		}

		public RegionSelector()
		{
			this.ProvinceTitle = "请选择省";
			this.CityTitle = "市：";
			this.CountyTitle = "区/县：";
			this.NullToDisplay = "-请选择-";
			this.StreetTitle = "街道";
			this.Separator = "，";
			this.IsShift = true;
		}

		public int? GetSelectedRegionId()
		{
			if (!string.IsNullOrEmpty(this.Context.Request.Form[RegionSelector.IDPrev + "regionSelectorValue"]))
			{
				return int.Parse(this.Context.Request.Form[RegionSelector.IDPrev + "regionSelectorValue"]);
			}
			return null;
		}

		public string GetSelectedRegionName()
		{
			if (!string.IsNullOrEmpty(this.Context.Request.Form[RegionSelector.IDPrev + "regionSelectorName"]))
			{
				return this.Context.Request.Form[RegionSelector.IDPrev + "regionSelectorName"];
			}
			return string.Empty;
		}

		public void SetSelectedRegionId(int? selectedRegionId)
		{
			this.currentRegionId = selectedRegionId;
			this.dataLoaded = true;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (!this.dataLoaded)
			{
				if (!string.IsNullOrEmpty(this.Context.Request.Form[RegionSelector.IDPrev + "regionSelectorValue"]))
				{
					this.currentRegionId = int.Parse(this.Context.Request.Form[RegionSelector.IDPrev + "regionSelectorValue"]);
				}
				this.dataLoaded = true;
			}
			if (this.currentRegionId.HasValue)
			{
				Hidistro.Entities.Store.RegionInfo region = RegionHelper.GetRegion(this.currentRegionId.Value, true);
				if (region != null)
				{
					if (region.Depth == 4)
					{
						this.streetId = this.currentRegionId;
						this.countyId = region.ParentRegionId;
						this.cityId = RegionHelper.GetCityId(region.ParentRegionId);
						this.provinceId = RegionHelper.GetTopRegionId(region.ParentRegionId, true);
					}
					if (region.Depth == 3)
					{
						this.countyId = this.currentRegionId.Value;
						this.cityId = region.ParentRegionId;
						this.provinceId = RegionHelper.GetTopRegionId(region.ParentRegionId, true);
					}
					else if (region.Depth == 2)
					{
						this.cityId = this.currentRegionId.Value;
						this.provinceId = region.ParentRegionId;
					}
					else if (region.Depth == 1)
					{
						this.provinceId = this.currentRegionId.Value;
					}
				}
			}
			this.Controls.Add(RegionSelector.CreateTag("<div class=\"address_wap\"><div class=\"dp_border\"></div><div class=\"dp_address\">"));
			this.ddlProvinces = this.CreateHypLink("province_top", "provincename", this.provinceId, this.ProvinceTitle, this.ProvinceWidth, out this.proviceSpan);
			this.FillHypLink(this.proviceSpan, "province", (Dictionary<int, string>)RegionHelper.GetAllProvinces(false), this.provinceId, (int?)0, out this.proviceDiv);
			this.ddlCitys = this.CreateHypLink("city_top", "cityname", this.cityId, this.CityTitle, this.CityWidth, out this.citySpan);
			Dictionary<int, string> regions = new Dictionary<int, string>();
			if (this.provinceId.HasValue)
			{
				regions = RegionHelper.GetCitys(this.provinceId.Value, false);
			}
			this.FillHypLink(this.citySpan, "city", regions, this.cityId, this.provinceId, out this.cityDiv);
			this.ddlCountys = this.CreateHypLink("area_top", "areaname", this.countyId, this.CountyTitle, this.CountyWidth, out this.areaSpan);
			Dictionary<int, string> regions2 = new Dictionary<int, string>();
			if (this.cityId.HasValue)
			{
				regions2 = RegionHelper.GetCountys(this.cityId.Value, false);
			}
			this.FillHypLink(this.areaSpan, "area", regions2, this.countyId, this.cityId, out this.areaDiv);
			if (this.DisplayStreet)
			{
				this.ddlStreets = this.CreateHypLink("street_top", "streetname", this.streetId, this.StreetTitle, this.StreetWidth, out this.streetSpan);
				Dictionary<int, string> regions3 = new Dictionary<int, string>();
				if (this.countyId.HasValue)
				{
					regions3 = RegionHelper.GetStreets(this.countyId.Value, false);
				}
				this.FillHypLink(this.streetSpan, "street", regions3, this.streetId, this.countyId, out this.streetDiv);
			}
			this.Controls.Add(this.ddlProvinces);
			this.Controls.Add(this.ddlCitys);
			this.Controls.Add(this.ddlCountys);
			if (this.DisplayStreet)
			{
				this.Controls.Add(this.ddlStreets);
			}
			this.Controls.Add(RegionSelector.CreateTag("</div>"));
			this.Controls.Add(this.proviceDiv);
			this.Controls.Add(this.cityDiv);
			this.Controls.Add(this.areaDiv);
			if (this.DisplayStreet)
			{
				this.Controls.Add(this.streetDiv);
			}
			this.Controls.Add(RegionSelector.CreateTag("</div>"));
			if (!this.CustomerCss)
			{
				Literal literal = new Literal();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<style type=\"text/css\">");
				stringBuilder.AppendLine(".dropdown_button {" + string.Format("background: url('{0}') no-repeat;", "/Admin/images/combo_arrow.jpg") + "}");
				stringBuilder.AppendLine(".dp_address a:hover .dropdown_button {" + string.Format("background: url('{0}') no-repeat;", "/Admin/images/combo_arrow1.jpg") + "}");
				stringBuilder.AppendLine("</style>");
				literal.Text = stringBuilder.ToString();
				this.Controls.Add(literal);
				WebControl webControl = new WebControl(HtmlTextWriterTag.Link);
				webControl.Attributes.Add("rel", "stylesheet");
				webControl.Attributes.Add("href",  "/Admin/css/region.css");
				webControl.Attributes.Add("type", "text/css");
				webControl.Attributes.Add("media", "screen");
				webControl.ID = "regionStyle";
				this.Controls.Add(webControl);
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
			int num = 0;
			if (this.currentRegionId.HasValue)
			{
				string fullRegion = RegionHelper.GetFullRegion(this.currentRegionId.Value, " ", true, 0);
				if (!string.IsNullOrEmpty(fullRegion))
				{
					num = fullRegion.Split(' ').Length;
				}
			}
			writer.AddAttribute("id", RegionSelector.IDPrev + "regionSelectorValue");
			writer.AddAttribute("name", RegionSelector.IDPrev + "regionSelectorValue");
			writer.AddAttribute("value", this.currentRegionId.HasValue ? this.currentRegionId.Value.ToString(CultureInfo.InvariantCulture) : "");
			writer.AddAttribute("type", "hidden");
			writer.AddAttribute("depth", num.ToString());
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
			writer.AddAttribute("id", RegionSelector.IDPrev + "regionIsShift");
			writer.AddAttribute("name", RegionSelector.IDPrev + "regionIsShift");
			bool flag = this.IsShift;
			writer.AddAttribute("value", flag.ToString().ToLower());
			writer.AddAttribute("type", "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
			writer.AddAttribute("id", RegionSelector.IDPrev + "regionSelectorName");
			writer.AddAttribute("name", RegionSelector.IDPrev + "regionSelectorName");
			writer.AddAttribute("value", this.SelectedRegionName);
			writer.AddAttribute("type", "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
			writer.AddAttribute("id", RegionSelector.IDPrev + "regionSelectorNull");
			writer.AddAttribute("name", RegionSelector.IDPrev + "regionSelectorNull");
			writer.AddAttribute("value", this.NullToDisplay);
			writer.AddAttribute("type", "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
			writer.AddAttribute("id", RegionSelector.IDPrev + "regionDisplayStreet");
			writer.AddAttribute("name", RegionSelector.IDPrev + "regionDisplayStreet");
			flag = this.DisplayStreet;
			writer.AddAttribute("value", flag.ToString().ToLower());
			writer.AddAttribute("type", "hidden");
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag();
			if (!this.CustomerJs && !this.Page.ClientScript.IsStartupScriptRegistered(this.Page.GetType(), "RegionSelectScript"))
			{
				string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>",  "/Admin/js/region.helper.js");
				this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "RegionSelectScript", script, false);
			}
		}

		private static void FillDropDownList(WebControl ddlRegions, Dictionary<int, string> regions, int? selectedId)
		{
			foreach (int key in regions.Keys)
			{
				WebControl webControl = RegionSelector.CreateOption(key.ToString(CultureInfo.InvariantCulture), regions[key]);
				if (selectedId.HasValue && key == selectedId.Value)
				{
					webControl.Attributes.Add("selected", "true");
				}
				ddlRegions.Controls.Add(webControl);
			}
		}

		private void FillHypLink(WebControl spanContrel, string type, Dictionary<int, string> regions, int? selectedId, int? parentId, out WebControl divContrel)
		{
			divContrel = new WebControl(HtmlTextWriterTag.Div);
			divContrel.Attributes.Add("class", "ap_content ap_" + type + " dp_address");
			divContrel.Attributes.Add("id", RegionSelector.IDPrev + type + "_floor");
			divContrel.Controls.Add(RegionSelector.CreateTag("<div class=\"dp_address_list " + type + " clearfix\" id=\"" + RegionSelector.IDPrev + type + "_info\"> <ul id=\"" + RegionSelector.IDPrev + type + "_select\">"));
			foreach (int key in regions.Keys)
			{
				if (selectedId.HasValue && key == selectedId.Value)
				{
					spanContrel.Controls.RemoveAt(0);
					spanContrel.Controls.AddAt(0, RegionSelector.CreateTag(regions[key]));
				}
				divContrel.Controls.Add(RegionSelector.CreateTag("<li><a href=\"javascript:;\" id=\"select_new_" + RegionSelector.IDPrev + type + "_" + key + "\">" + regions[key] + "</a></li>"));
			}
			if (this.IsShowClear)
			{
				if (parentId.HasValue)
				{
					divContrel.Controls.Add(RegionSelector.CreateTag("<li><a href=\"javascript:;\" t=\"clear\" id=\"select_new_" + RegionSelector.IDPrev + type + "_" + parentId + "\">[清空]</a></li>"));
				}
				else
				{
					divContrel.Controls.Add(RegionSelector.CreateTag("<li><a href=\"javascript:;\" t=\"clear\" id=\"select_new_" + RegionSelector.IDPrev + type + "_clear\" >[清空]</a></li>"));
				}
			}
			divContrel.Controls.Add(RegionSelector.CreateTag("</ul></div>"));
		}

		private WebControl CreateHypLink(string controlId, string spanId, int? selectedId, string showname, int width, out WebControl webSpan)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.A);
			webControl.Attributes.Add("id", RegionSelector.IDPrev + controlId);
			webControl.Attributes.Add("href", "javascript:");
			webControl.Attributes.Add("class", "dropdown_box");
			if (width > 0)
			{
				webControl.Attributes.Add("style", "width:" + width + "px");
			}
			webSpan = new WebControl(HtmlTextWriterTag.Span);
			webSpan.Attributes.Add("id", RegionSelector.IDPrev + spanId);
			webSpan.Attributes.Add("class", "dropdown_selected");
			webSpan.Controls.Add(RegionSelector.CreateTag(showname));
			if (selectedId.HasValue)
			{
				webSpan.Attributes.Add("value", selectedId.Value.ToString());
			}
			webControl.Controls.Add(webSpan);
			webControl.Controls.Add(RegionSelector.CreateTag("<span class=\"dropdown_button\"></span>"));
			return webControl;
		}

		private WebControl CreateDropDownList(string controlId)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.Select);
			webControl.Attributes.Add("id", RegionSelector.IDPrev + controlId);
			webControl.Attributes.Add("name", controlId);
			webControl.Attributes.Add("selectset", "regions");
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.Option);
			webControl2.Controls.Add(new LiteralControl(this.NullToDisplay));
			webControl2.Attributes.Add("value", "");
			webControl.Controls.Add(webControl2);
			return webControl;
		}

		private static WebControl CreateOption(string val, string text)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.Option);
			webControl.Attributes.Add("value", val);
			webControl.Controls.Add(new LiteralControl(text.Trim()));
			return webControl;
		}

		private static Label CreateTitleControl(string title)
		{
			Label label = new Label
			{
				Text = title
			};
			label.Attributes.Add("style", "margin-left:5px");
			return label;
		}

		private static Literal CreateTag(string tagName)
		{
			return new Literal
			{
				Text = tagName
			};
		}
	}
}
