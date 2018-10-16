using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class SkuValue : AdminCallBackPage
	{
		private int attributeId = 0;

		private int valueId = 0;

		protected HtmlGenericControl valueStr;

		protected TextBox txtValueStr;

		protected HtmlInputHidden currentAttributeId;

		protected Button btnCreateValue;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["action"].ToString().Trim()))
				{
					base.GotoResourceNotFound();
					return;
				}
				string a = this.Page.Request.QueryString["action"].ToString().Trim();
				if (a == "add")
				{
					if (!int.TryParse(this.Page.Request.QueryString["attributeId"], out this.attributeId))
					{
						base.GotoResourceNotFound();
						return;
					}
				}
				else
				{
					if (!int.TryParse(this.Page.Request.QueryString["valueId"], out this.valueId))
					{
						base.GotoResourceNotFound();
						return;
					}
					AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(this.valueId);
					this.attributeId = attributeValueInfo.AttributeId;
					this.txtValueStr.Text = Globals.HtmlDecode(attributeValueInfo.ValueStr);
				}
				this.currentAttributeId.Value = this.attributeId.ToString();
			}
			this.btnCreateValue.Click += this.btnCreateValue_Click;
		}

		protected void btnCreateValue_Click(object sender, EventArgs e)
		{
			AttributeValueInfo attributeValue = new AttributeValueInfo();
			IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
			int num = int.Parse(this.currentAttributeId.Value);
			attributeValue.AttributeId = num;
			string a = this.Page.Request.QueryString["action"].ToString().Trim();
			if (a == "add")
			{
				if (!string.IsNullOrEmpty(this.txtValueStr.Text.Trim()))
				{
					string content = this.txtValueStr.Text.Trim();
					content = Globals.StripHtmlXmlTags(Globals.StripScriptTags(content)).Replace("，", ",").Replace("\\", "")
						.Replace("/", "");
					string[] array = content.Split(',');
					for (int i = 0; i < array.Length && array[i].Trim().Length <= 100; i++)
					{
						AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
						if (array[i].Trim().Length > 50)
						{
							this.ShowMsg("属性值限制在50个字符以内", false);
							return;
						}
						attributeValueInfo.ValueStr = Globals.HtmlEncode(array[i].Trim());
						attributeValueInfo.AttributeId = num;
						list.Add(attributeValueInfo);
					}
					foreach (AttributeValueInfo item in list)
					{
						IList<AttributeValueInfo> attributeValues = ProductTypeHelper.GetAttribute(item.AttributeId).AttributeValues;
						if ((from c in attributeValues
						where c.ValueStr == item.ValueStr
						select c).Count() > 0)
						{
							this.ShowMsg("规格值不能重复", false);
							return;
						}
						ProductTypeHelper.AddAttributeValue(item);
					}
					base.CloseWindow(null);
				}
			}
			else
			{
				this.valueId = int.Parse(this.Page.Request.QueryString["valueId"]);
				attributeValue = ProductTypeHelper.GetAttributeValueInfo(this.valueId);
				AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeValue.AttributeId);
				if (!string.IsNullOrEmpty(this.txtValueStr.Text))
				{
					attributeValue.ValueStr = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtValueStr.Text)).Replace("，", ",").Replace("\\", "")
						.Replace("/", "");
				}
				IList<AttributeValueInfo> attributeValues2 = ProductTypeHelper.GetAttribute(attributeValue.AttributeId).AttributeValues;
				if ((from c in attributeValues2
				where c.ValueId != attributeValue.ValueId && c.ValueStr == attributeValue.ValueStr
				select c).Count() > 0)
				{
					this.ShowMsg("规格值不能重复", false);
				}
				else if (ProductTypeHelper.UpdateAttributeValue(attributeValue))
				{
					base.CloseWindow(null);
				}
			}
		}
	}
}
