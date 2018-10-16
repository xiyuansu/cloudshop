using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SubjectProducts)]
	public class ProductTags : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isAjax"]) && base.Request["isAjax"] == "true")
			{
				string text = base.Request["Mode"].ToString();
				string text2 = "";
				string text3 = "false";
				string a = text;
				if (a == "Add")
				{
					text2 = "标签名称不允许为空";
					if (!string.IsNullOrEmpty(base.Request["TagValue"].Trim()))
					{
						text2 = "添加标签名称失败，请确认标签名是否已存在";
						string tagName = Globals.HtmlEncode(base.Request["TagValue"].ToString());
						int num = CatalogHelper.AddTags(tagName);
						if (num > 0)
						{
							text3 = "true";
							text2 = num.ToString();
						}
					}
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{\"Status\":\"" + text3 + "\",\"msg\":\"" + text2 + "\"}");
					base.Response.End();
				}
			}
		}
	}
}
