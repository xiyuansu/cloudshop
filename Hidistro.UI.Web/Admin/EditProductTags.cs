using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Products)]
	public class EditProductTags : AdminCallBackPage
	{
		private string productIds = string.Empty;

		protected ProductTagsLiteral litralProductTag;

		protected TrimTextBox txtProductTag;

		protected Button btnUpdateProductTags;

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btnUpdateProductTags.Click += this.btnUpdateProductTags_Click;
			this.productIds = this.Page.Request.QueryString["productIds"];
		}

		private void btnUpdateProductTags_Click(object sender, EventArgs e)
		{
			string text = base.Request.Form["productTags"];
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("请先选择要关联的商品", false);
			}
			else
			{
				IList<int> list = new List<int>();
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = text;
					string[] array = null;
					array = ((!text2.Contains(",")) ? new string[1]
					{
						text2
					} : text2.Split(','));
					string[] array2 = array;
					foreach (string value in array2)
					{
						list.Add(Convert.ToInt32(value));
					}
				}
				string[] array3 = null;
				array3 = ((!this.productIds.Contains(",")) ? new string[1]
				{
					this.productIds
				} : this.productIds.Split(','));
				int num = 0;
				string[] array4 = array3;
				foreach (string value2 in array4)
				{
					ProductHelper.DeleteProductTags(Convert.ToInt32(value2), null);
					if (list.Count > 0 && ProductHelper.AddProductTags(Convert.ToInt32(value2), list, null))
					{
						num++;
					}
				}
				if (num > 0)
				{
					string text3 = $"已成功修改了{num}件商品的商品标签";
					this.ShowMsg(text3, true);
					base.CloseWindow(text3);
				}
				else
				{
					this.ShowMsg("已成功取消了商品的关联商品标签", true);
				}
			}
		}
	}
}
