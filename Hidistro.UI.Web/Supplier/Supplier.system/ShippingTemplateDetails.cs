using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Supplier.system
{
	[AdministerCheck(true)]
	public class ShippingTemplateDetails : SupplierAdminPage
	{
		private int templateId;

		protected Literal ltModeName;

		protected Literal ltIsFreeShipping;

		protected HtmlGenericControl lijjfs;

		protected Literal ltValuationMetnods;

		protected HtmlGenericControl ulysfs;

		protected Literal ltDefaultNumber;

		protected Literal ltUnit;

		protected Literal ltDefaultPrice;

		protected Literal ltAddNumber;

		protected Literal ltUnit2;

		protected Literal ltAddPrice;

		protected HiddenField hidRegionJson;

		protected HtmlGenericControl ulzdcs;

		protected Literal ltUnitDesc;

		protected Literal ltUnit3;

		protected Literal ltUnitDesc2;

		protected Literal ltUnit4;

		protected Repeater rptShippingTypeGroups;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.templateId = base.Request.QueryString["TemplateId"].ToInt(0);
				ShippingTemplateInfo shippingTemplate = SalesHelper.GetShippingTemplate(this.templateId, true);
				if (shippingTemplate == null)
				{
					base.Response.Redirect("ManageShippingTemplates.aspx");
				}
				else
				{
					this.BindControl(shippingTemplate);
				}
			}
		}

		protected string ToDecimalString(object obj)
		{
			return obj.ToDecimal(0).F2ToString("f2");
		}

		protected string ToRegionNameByStr(object obj)
		{
			string result = string.Empty;
			IList<ShippingRegionInfo> list = obj as IList<ShippingRegionInfo>;
			if (list != null && list.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ShippingRegionInfo item in list)
				{
					stringBuilder.Append(RegionHelper.GetFullRegion(item.RegionId, ",", true, 0).Split(',')[1] + ",");
				}
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					result = stringBuilder.ToString().Substring(0, stringBuilder.ToString().Length - 1);
				}
			}
			return result;
		}

		private void BindControl(ShippingTemplateInfo modeItem)
		{
			if (modeItem.IsFreeShipping)
			{
				this.lijjfs.Visible = false;
				this.ulysfs.Visible = false;
				this.ulzdcs.Visible = false;
			}
			this.ltIsFreeShipping.Text = (modeItem.IsFreeShipping ? "卖家承担运费" : "自定义运费");
			switch (modeItem.ValuationMethod)
			{
			case ValuationMethods.Number:
			{
				this.ltValuationMetnods.Text = ((Enum)(object)ValuationMethods.Number).ToDescription();
				Literal literal13 = this.ltUnit;
				Literal literal14 = this.ltUnit2;
				Literal literal15 = this.ltUnit3;
				Literal literal16 = this.ltUnit4;
				string text2 = literal16.Text = "件";
				string text4 = literal15.Text = text2;
				string text7 = literal13.Text = (literal14.Text = text4);
				Literal literal17 = this.ltUnitDesc;
				Literal literal18 = this.ltUnitDesc2;
				text7 = (literal17.Text = (literal18.Text = "件"));
				break;
			}
			case ValuationMethods.Volume:
			{
				this.ltValuationMetnods.Text = ((Enum)(object)ValuationMethods.Volume).ToDescription();
				Literal literal7 = this.ltUnit;
				Literal literal8 = this.ltUnit2;
				Literal literal9 = this.ltUnit3;
				Literal literal10 = this.ltUnit4;
				string text2 = literal10.Text = "m<sup>3</sup>";
				string text4 = literal9.Text = text2;
				string text7 = literal7.Text = (literal8.Text = text4);
				Literal literal11 = this.ltUnitDesc;
				Literal literal12 = this.ltUnitDesc2;
				text7 = (literal11.Text = (literal12.Text = "体积"));
				break;
			}
			default:
			{
				this.ltValuationMetnods.Text = ((Enum)(object)ValuationMethods.Weight).ToDescription();
				Literal literal = this.ltUnit;
				Literal literal2 = this.ltUnit2;
				Literal literal3 = this.ltUnit3;
				Literal literal4 = this.ltUnit4;
				string text2 = literal4.Text = "kg";
				string text4 = literal3.Text = text2;
				string text7 = literal.Text = (literal2.Text = text4);
				Literal literal5 = this.ltUnitDesc;
				Literal literal6 = this.ltUnitDesc2;
				text7 = (literal5.Text = (literal6.Text = "重"));
				break;
			}
			}
			this.ltModeName.Text = modeItem.TemplateName;
			this.ltDefaultNumber.Text = modeItem.DefaultNumber.ToDecimal(0).F2ToString("f2");
			this.ltAddNumber.Text = modeItem.AddNumber.ToDecimal(0).F2ToString("f2");
			this.ltAddPrice.Text = modeItem.AddPrice.ToDecimal(0).F2ToString("f2");
			this.ltDefaultPrice.Text = modeItem.Price.ToDecimal(0).F2ToString("f2");
			if (modeItem.ModeGroup != null && modeItem.ModeGroup.Count > 0)
			{
				this.rptShippingTypeGroups.DataSource = modeItem.ModeGroup;
				this.rptShippingTypeGroups.DataBind();
			}
			else
			{
				this.ulzdcs.Visible = false;
			}
		}
	}
}
