using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
	public class AddShippingTemplate : AdminPage
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

		protected Button btnCreate;

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
			this.btnCreate.Click += this.btnCreate_Click;
			bool flag = false;
			if (this.Page.Request["IsCallBack"].ToNullString().ToLower() == "true")
			{
				flag = true;
			}
			if (flag)
			{
				this.doCallback();
			}
			if (!this.Page.IsPostBack)
			{
				this.radIsFreeShipping.SelectedIndex = 0;
				this.radValuationMethods.SelectedIndex = 0;
			}
		}

		public void doCallback()
		{
			string text = base.Request["action"].ToNullString();
			string a = text;
			if (a == "ValidTemplateName")
			{
				this.ValidTemplateName();
			}
		}

		public void ValidTemplateName()
		{
			string text = base.Request["templateName"].ToNullString();
			text = Globals.StripAllTags(text.Trim()).Replace("\\", "").Replace("<", "")
				.Replace(">", "");
			int templateId = base.Request["TemplateId"].ToInt(0);
			if (text == "" || text.Length > 20)
			{
				base.Response.Write("{\"Status\":\"EMPTY\",\"Message\":\"模板名称不能为空，长度限制在20字符以内,不允许包含脚本标签和特殊字符,系统会自动过滤\"}");
				base.Response.End();
			}
			else if (SalesHelper.IsExistTemplateName(text, templateId))
			{
				base.Response.Write("{\"Status\":\"EXIST\",\"Message\":\"模板名称已存在,请重新输入一个名称\"}");
				base.Response.End();
			}
			else
			{
				base.Response.Write("{\"Status\":\"OK\",\"Message\":\"\"}");
				base.Response.End();
			}
		}

		private void btnCreate_Click(object sender, EventArgs e)
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
				else if (SalesHelper.IsExistTemplateName(text, 0))
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
					if (SalesHelper.CreateShippingTemplate(shippingTemplateInfo))
					{
						if (!string.IsNullOrEmpty(this.Page.Request.QueryString["source"]) && this.Page.Request.QueryString["source"] == "add")
						{
							this.CloseWindow();
						}
						else
						{
							this.ClearControlValue();
							this.ShowMsg("成功添加了一个配送方式模板", true, "ManageShippingTemplates.aspx");
						}
					}
					else
					{
						this.ShowMsg("您添加的地区有重复", false);
					}
				}
			}
		}

		private void ClearControlValue()
		{
			this.txtAddPrice.Text = string.Empty;
			this.txtAddNumber.Text = string.Empty;
			this.txtModeName.Text = string.Empty;
			this.txtDefaultPrice.Text = string.Empty;
			this.txtDefaultNumber.Text = string.Empty;
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
					text += Formatter.FormatErrorMessage("默认数量不能为空,必须为大于0,限制在0-100000之间");
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
	}
}
