using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public class EditShippingTemplate : AdminPage
	{
		[Serializable]
		public class Region
		{
			private string regionsId;

			private string regions;

			private decimal regionPrice;

			private decimal regionAddPrice;

			public string Regions
			{
				get
				{
					return this.regions;
				}
				set
				{
					this.regions = value;
				}
			}

			public string RegionsId
			{
				get
				{
					return this.regionsId;
				}
				set
				{
					this.regionsId = value;
				}
			}

			public decimal RegionPrice
			{
				get
				{
					return this.regionPrice;
				}
				set
				{
					this.regionPrice = value;
				}
			}

			public decimal RegionAddPrice
			{
				get
				{
					return this.regionAddPrice;
				}
				set
				{
					this.regionAddPrice = value;
				}
			}
		}

		private int templateId;

		protected TextBox txtModeName;

		protected RadioButtonList radIsFreeShipping;

		protected ValuationMethodsRadioButtonList radValuationMethods;

		protected TextBox txtDefaultNumber;

		protected TextBox txtDefaultPrice;

		protected TextBox txtAddNumber;

		protected TextBox txtAddPrice;

		protected HiddenField hidRegionJson;

		protected HiddenField hidFreeJson;

		protected OnOff radHasFree;

		protected Button btnUpdate;

		private IList<Region> RegionList
		{
			get
			{
				if (this.ViewState["Region"] == null)
				{
					this.ViewState["Region"] = new List<Region>();
				}
				return (IList<Region>)this.ViewState["Region"];
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["TemplateId"], out this.templateId))
			{
				base.GotoResourceNotFound();
			}
			else
			{
				this.btnUpdate.Click += this.btnUpdate_Click;
				if (!this.Page.IsPostBack)
				{
					if (this.Page.Request.QueryString["isUpdate"] != null && this.Page.Request.QueryString["isUpdate"] == "true")
					{
						this.ShowMsg("成功修改了一个配送方式", true);
					}
					ShippingTemplateInfo shippingTemplate = SalesHelper.GetShippingTemplate(this.templateId, true);
					if (shippingTemplate == null)
					{
						base.GotoResourceNotFound();
					}
					else
					{
						this.BindControl(shippingTemplate);
					}
				}
			}
		}

		private void BindControl(ShippingTemplateInfo modeItem)
		{
			if (SalesHelper.ShippingTemplateIsExistProdcutRelation(this.templateId))
			{
				this.radValuationMethods.Enabled = false;
				if (modeItem.IsFreeShipping)
				{
					this.radIsFreeShipping.Enabled = false;
				}
			}
			this.txtModeName.Text = Globals.HtmlDecode(modeItem.TemplateName);
			if (modeItem.ValuationMethod == ValuationMethods.Number)
			{
				TextBox textBox = this.txtDefaultNumber;
				decimal num = modeItem.DefaultNumber;
				textBox.Text = num.ToString("f0");
				TextBox textBox2 = this.txtAddNumber;
				object text;
				if (!modeItem.AddNumber.HasValue)
				{
					text = "0";
				}
				else
				{
					num = modeItem.AddNumber.Value;
					text = num.ToString("f0");
				}
				textBox2.Text = (string)text;
			}
			else if (modeItem.ValuationMethod == ValuationMethods.Volume)
			{
				this.txtDefaultNumber.Text = modeItem.DefaultNumber.F2ToString("f2");
				this.txtAddNumber.Text = (modeItem.AddNumber.HasValue ? modeItem.AddNumber.Value.F2ToString("f2") : "0");
			}
			else
			{
				this.txtDefaultNumber.Text = modeItem.DefaultNumber.F2ToString("f2");
				this.txtAddNumber.Text = (modeItem.AddNumber.HasValue ? modeItem.AddNumber.Value.F2ToString("f2") : "0");
			}
			if (modeItem.AddPrice.HasValue)
			{
				this.txtAddPrice.Text = modeItem.AddPrice.Value.F2ToString("f2");
			}
			this.txtDefaultPrice.Text = modeItem.Price.F2ToString("f2");
			this.radIsFreeShipping.SelectedIndex = (modeItem.IsFreeShipping ? 1 : 0);
			this.radValuationMethods.SelectedValue = modeItem.ValuationMethod;
			IList<ShippingTemplateGroupMode> list = new List<ShippingTemplateGroupMode>();
			if (modeItem.ModeGroup != null && modeItem.ModeGroup.Count > 0)
			{
				foreach (ShippingTemplateGroupInfo item in modeItem.ModeGroup)
				{
					ShippingTemplateGroupMode shippingTemplateGroupMode = new ShippingTemplateGroupMode();
					shippingTemplateGroupMode.AddNumber = (item.AddNumber.HasValue ? item.AddNumber.Value : decimal.Zero);
					shippingTemplateGroupMode.AddPrice = (item.AddPrice.HasValue ? item.AddPrice.Value : decimal.Zero);
					shippingTemplateGroupMode.DefaultNumber = item.DefaultNumber;
					shippingTemplateGroupMode.DefaultPrice = item.Price;
					StringBuilder stringBuilder = new StringBuilder();
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (ShippingRegionInfo modeRegion in item.ModeRegions)
					{
						stringBuilder.Append(modeRegion.RegionId + ",");
						stringBuilder2.Append(RegionHelper.GetFullRegion(modeRegion.RegionId, ",", true, 0).Split(',')[1] + ",");
					}
					if (!string.IsNullOrEmpty(stringBuilder.ToString()))
					{
						shippingTemplateGroupMode.RegionIds = stringBuilder.ToString().Substring(0, stringBuilder.ToString().Length - 1);
					}
					if (!string.IsNullOrEmpty(stringBuilder2.ToString()))
					{
						shippingTemplateGroupMode.RegionNames = stringBuilder2.ToString().Substring(0, stringBuilder2.ToString().Length - 1);
					}
					list.Add(shippingTemplateGroupMode);
				}
			}
			if (list != null && list.Count > 0)
			{
				this.hidRegionJson.Value = JsonHelper.GetJson(list);
			}
			IList<ShippingTemplateFreeGroupMode> list2 = new List<ShippingTemplateFreeGroupMode>();
			if (modeItem.FreeGroup != null && modeItem.FreeGroup.Count > 0)
			{
				this.radHasFree.SelectedValue = true;
				foreach (ShippingTemplateFreeGroupInfo item2 in modeItem.FreeGroup)
				{
					ShippingTemplateFreeGroupMode shippingTemplateFreeGroupMode = new ShippingTemplateFreeGroupMode();
					shippingTemplateFreeGroupMode.ConditionType = item2.ConditionType;
					shippingTemplateFreeGroupMode.ConditionNumber = item2.ConditionNumber;
					StringBuilder stringBuilder3 = new StringBuilder();
					StringBuilder stringBuilder4 = new StringBuilder();
					foreach (ShippingFreeRegionInfo modeRegion2 in item2.ModeRegions)
					{
						stringBuilder3.Append(modeRegion2.RegionId + ",");
						stringBuilder4.Append(RegionHelper.GetFullRegion(modeRegion2.RegionId, ",", true, 0).Split(',')[1] + ",");
					}
					if (!string.IsNullOrEmpty(stringBuilder3.ToString()))
					{
						shippingTemplateFreeGroupMode.RegionIds = stringBuilder3.ToString().Substring(0, stringBuilder3.ToString().Length - 1);
					}
					if (!string.IsNullOrEmpty(stringBuilder4.ToString()))
					{
						shippingTemplateFreeGroupMode.RegionNames = stringBuilder4.ToString().Substring(0, stringBuilder4.ToString().Length - 1);
					}
					list2.Add(shippingTemplateFreeGroupMode);
				}
			}
			if (list2 != null && list2.Count > 0)
			{
				this.hidFreeJson.Value = JsonHelper.GetJson(list2);
			}
		}

		private bool ValidateRegionValues(out decimal weight, out decimal? addWeight, out decimal price, out decimal? addPrice, out ValuationMethods valuationMethod)
		{
			string text = string.Empty;
			addWeight = default(decimal);
			addPrice = default(decimal);
			weight = default(decimal);
			price = default(decimal);
			valuationMethod = ValuationMethods.Weight;
			if (this.radIsFreeShipping.SelectedValue == "")
			{
				text += Formatter.FormatErrorMessage("请选是否包邮");
			}
			if (!this.radValuationMethods.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择计价方式");
			}
			else
			{
				valuationMethod = this.radValuationMethods.SelectedValue.Value;
			}
			if (this.radIsFreeShipping.SelectedValue == "0")
			{
				if (!decimal.TryParse(this.txtDefaultNumber.Text.Trim(), out weight))
				{
					text += Formatter.FormatErrorMessage("默认数量不能为空,必须大于0,限制在0-100000之间");
				}
				if (!string.IsNullOrEmpty(this.txtAddNumber.Text.Trim()))
				{
					decimal value = default(decimal);
					if (decimal.TryParse(this.txtAddNumber.Text.Trim(), out value))
					{
						addWeight = value;
					}
					else
					{
						text += Formatter.FormatErrorMessage("增量数量不能为空,必须为正整数,限制在0-100000之间");
					}
				}
				if (!decimal.TryParse(this.txtDefaultPrice.Text.Trim(), out price))
				{
					text += Formatter.FormatErrorMessage("默认起步价必须为非负数字,限制在0-10000000之间");
				}
				if (!string.IsNullOrEmpty(this.txtAddPrice.Text.Trim()))
				{
					decimal value2 = default(decimal);
					if (decimal.TryParse(this.txtAddPrice.Text.Trim(), out value2))
					{
						addPrice = value2;
					}
					else
					{
						text += Formatter.FormatErrorMessage("默认加价必须为非负数字,限制在0-10000000之间");
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			decimal defaultNumber = default(decimal);
			decimal? addNumber = default(decimal?);
			decimal price = default(decimal);
			decimal? addPrice = default(decimal?);
			ValuationMethods valuationMethod = default(ValuationMethods);
			if (this.ValidateRegionValues(out defaultNumber, out addNumber, out price, out addPrice, out valuationMethod))
			{
				IList<ShippingTemplateGroupInfo> list = new List<ShippingTemplateGroupInfo>();
				string value = this.hidRegionJson.Value;
				if (!string.IsNullOrEmpty(value))
				{
					IList<ShippingTemplateGroupMode> list2 = new List<ShippingTemplateGroupMode>();
					list2 = JsonHelper.ParseFormJson<List<ShippingTemplateGroupMode>>(value);
					if (list2 != null && list2.Count > 0)
					{
						foreach (ShippingTemplateGroupMode item in list2)
						{
							ShippingTemplateGroupInfo shippingTemplateGroupInfo = new ShippingTemplateGroupInfo();
							shippingTemplateGroupInfo.AddPrice = item.AddPrice;
							shippingTemplateGroupInfo.AddNumber = item.AddNumber;
							shippingTemplateGroupInfo.DefaultNumber = item.DefaultNumber;
							shippingTemplateGroupInfo.Price = item.DefaultPrice;
							string safeIDList = Globals.GetSafeIDList(item.RegionIds, ',', true);
							string[] array = safeIDList.Split(',');
							foreach (string obj in array)
							{
								ShippingRegionInfo shippingRegionInfo = new ShippingRegionInfo();
								shippingRegionInfo.RegionId = obj.ToInt(0);
								shippingTemplateGroupInfo.ModeRegions.Add(shippingRegionInfo);
							}
							list.Add(shippingTemplateGroupInfo);
						}
					}
				}
				IList<ShippingTemplateFreeGroupInfo> list3 = new List<ShippingTemplateFreeGroupInfo>();
				string value2 = this.hidFreeJson.Value;
				if (!string.IsNullOrEmpty(value2))
				{
					IList<ShippingTemplateFreeGroupMode> list4 = new List<ShippingTemplateFreeGroupMode>();
					list4 = JsonHelper.ParseFormJson<List<ShippingTemplateFreeGroupMode>>(value2);
					if (list4 != null && list4.Count > 0)
					{
						foreach (ShippingTemplateFreeGroupMode item2 in list4)
						{
							ShippingTemplateFreeGroupInfo shippingTemplateFreeGroupInfo = new ShippingTemplateFreeGroupInfo();
							shippingTemplateFreeGroupInfo.ConditionType = item2.ConditionType;
							shippingTemplateFreeGroupInfo.ConditionNumber = item2.ConditionNumber;
							string safeIDList2 = Globals.GetSafeIDList(item2.RegionIds, ',', true);
							string[] array2 = safeIDList2.Split(',');
							foreach (string obj2 in array2)
							{
								ShippingFreeRegionInfo shippingFreeRegionInfo = new ShippingFreeRegionInfo();
								shippingFreeRegionInfo.RegionId = obj2.ToInt(0);
								shippingTemplateFreeGroupInfo.ModeRegions.Add(shippingFreeRegionInfo);
							}
							list3.Add(shippingTemplateFreeGroupInfo);
						}
					}
				}
				string text = Globals.StripAllTags(this.txtModeName.Text.Trim()).Replace("\\", "").Replace("<", "")
					.Replace(">", "");
				if (text == "" || text.Length > 20)
				{
					this.ShowMsg("模板名称不能为空，长度限制在20字符以内,不允许包含脚本标签和特殊字符,系统会自动过滤", false);
				}
				else if (SalesHelper.IsExistTemplateName(text, this.templateId))
				{
					this.ShowMsg("模板名称重复,请重新输入", false);
				}
				else
				{
					ShippingTemplateInfo shippingTemplateInfo = new ShippingTemplateInfo();
					shippingTemplateInfo.ModeGroup = list;
					shippingTemplateInfo.FreeGroup = list3;
					shippingTemplateInfo.IsFreeShipping = (this.radIsFreeShipping.SelectedIndex != 0 && true);
					shippingTemplateInfo.ValuationMethod = valuationMethod;
					shippingTemplateInfo.TemplateName = Globals.HtmlEncode(text);
					shippingTemplateInfo.DefaultNumber = defaultNumber;
					shippingTemplateInfo.AddNumber = addNumber;
					shippingTemplateInfo.Price = price;
					shippingTemplateInfo.AddPrice = addPrice;
					shippingTemplateInfo.TemplateId = this.templateId;
					if (SalesHelper.UpdateShippingTemplate(shippingTemplateInfo))
					{
						this.ShowMsg("运费模板更新成功", true, "ManageShippingTemplates.aspx");
					}
					else
					{
						this.ShowMsg("您添加的地区有重复", false);
					}
				}
			}
		}
	}
}
