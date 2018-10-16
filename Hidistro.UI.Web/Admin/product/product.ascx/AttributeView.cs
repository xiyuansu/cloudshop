using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Commodities;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product.ascx
{
	public class AttributeView : UserControl
	{
		protected int typeId = 0;

		protected TextBox txtName;

		protected CheckBox chkMulti_copy;

		protected TextBox txtValues;

		protected Button btnCreate;

		protected CheckBox chkMulti;

		protected HtmlInputHidden currentAttributeId;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.typeId = this.Page.Request.QueryString["typeId"].ToInt(0);
			if (this.typeId <= 0)
			{
				throw new Exception("错误的类型编号");
			}
			this.btnCreate.Click += this.btnCreate_Click;
		}

		private void btnCreate_Click(object sender, EventArgs e)
		{
			if (this.txtName.Text.Trim().Length > 15)
			{
				string str = string.Format("ShowMsg(\"{0}\", {1});", "属性名称不合规范", "false");
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
			}
			else
			{
				AttributeInfo attributeInfo = new AttributeInfo();
				attributeInfo.TypeId = this.typeId;
				attributeInfo.AttributeName = Globals.HtmlEncode(Globals.StripScriptTags(this.txtName.Text.Trim()).Replace("\\", ""));
				if (this.chkMulti.Checked)
				{
					attributeInfo.UsageMode = AttributeUseageMode.MultiView;
				}
				else
				{
					attributeInfo.UsageMode = AttributeUseageMode.View;
				}
				if (!string.IsNullOrEmpty(this.txtValues.Text.Trim()))
				{
					string text = Globals.StripHtmlXmlTags(Globals.StripScriptTags(this.txtValues.Text.Trim()).Replace("，", ",").Replace("\\", ""));
					string[] array = text.Split(',');
					for (int i = 0; i < array.Length && array[i].Length <= 100; i++)
					{
						AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
						if (array[i].Length > 15)
						{
							string str2 = string.Format("ShowMsg(\"{0}\", {1});", "属性值不合规范", "false");
							this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str2 + "},300);</script>");
							return;
						}
						attributeValueInfo.ValueStr = array[i];
						attributeInfo.AttributeValues.Add(attributeValueInfo);
					}
				}
				ValidationResults validationResults = Validation.Validate(attributeInfo, "ValAttribute");
				string str3 = string.Empty;
				if (!validationResults.IsValid)
				{
					foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
					{
						str3 += Formatter.FormatErrorMessage(item.Message);
					}
				}
				else if (ProductTypeHelper.AddAttribute(attributeInfo))
				{
					this.txtName.Text = string.Empty;
					this.txtValues.Text = string.Empty;
					base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
				}
			}
		}
	}
}
