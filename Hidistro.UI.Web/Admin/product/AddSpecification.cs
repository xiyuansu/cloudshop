using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddSpecification : AdminPage
	{
		private int typeId = 0;

		protected TextBox txtName;

		protected Button btnFilish;

		protected void Page_Load(object sender, EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";
			this.btnFilish.Click += this.btnFilish_Click;
			int attributeValueId = default(int);
			if (flag && int.TryParse(base.Request["ValueId"], out attributeValueId))
			{
				if (ProductTypeHelper.DeleteAttributeValue(attributeValueId))
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{\"Status\":\"true\"}");
					base.Response.End();
				}
				else
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{\"Status\":\"false\"}");
					base.Response.End();
				}
			}
			if (!string.IsNullOrEmpty(base.Request["isAjax"]) && base.Request["isAjax"] == "true")
			{
				string text = base.Request["Mode"].ToString();
				string text2 = "";
				string text3 = "false";
				string a = text;
				int attributeId = default(int);
				if (!(a == "Add"))
				{
					if (a == "AddSkuItemValue")
					{
						text2 = "参数缺少";
						if (int.TryParse(base.Request["AttributeId"], out attributeId))
						{
							string text4 = "";
							text2 = "规格值名不允许为空！";
							if (!string.IsNullOrEmpty(base.Request["ValueName"].ToString()))
							{
								text4 = Globals.StripHtmlXmlTags(Globals.StripScriptTags(base.Request["ValueName"].ToString().Replace("+", "").Replace(",", "")));
								text2 = "规格值名长度不允许超过15个字符";
								if (text4.Length < 15)
								{
									AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
									attributeValueInfo.ValueStr = text4;
									attributeValueInfo.AttributeId = attributeId;
									int num = 0;
									text2 = "添加规格值失败";
									num = ProductTypeHelper.AddAttributeValue(attributeValueInfo);
									if (num > 0)
									{
										text2 = num.ToString();
										text3 = "true";
									}
								}
							}
						}
						base.Response.Clear();
						base.Response.ContentType = "application/json";
						base.Response.Write("{\"Status\":\"" + text3 + "\",\"msg\":\"" + text2 + "\"}");
						base.Response.End();
					}
				}
				else
				{
					attributeId = 0;
					text2 = "参数缺少";
					if (int.TryParse(base.Request["AttributeId"], out attributeId))
					{
						string text5 = "";
						text2 = "属性名称不允许为空！";
						if (!string.IsNullOrEmpty(base.Request["ValueName"].ToString()))
						{
							text5 = Globals.HtmlEncode(base.Request["ValueName"].ToString());
							AttributeValueInfo attributeValueInfo2 = new AttributeValueInfo();
							attributeValueInfo2.ValueStr = text5;
							attributeValueInfo2.AttributeId = attributeId;
							int num2 = 0;
							text2 = "添加属性值失败";
							num2 = ProductTypeHelper.AddAttributeValue(attributeValueInfo2);
							if (num2 > 0)
							{
								text2 = num2.ToString();
								text3 = "true";
							}
						}
					}
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{\"Status\":\"" + text3 + "\",\"msg\":\"" + text2 + "\"}");
					base.Response.End();
				}
			}
		}

		private void btnFilish_Click(object server, EventArgs e)
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ProductTypes.aspx"), true);
		}
	}
}
