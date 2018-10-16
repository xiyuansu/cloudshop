using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hidistro.UI.Web.Admin.Supplier.Product
{
	[PrivilegeCheck(Privilege.SupplierEditPd)]
	public class SelectCategory : AdminPage
	{
		private int CategoryId = 0;

		public string rnd = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (base.Request.QueryString["categoryId"] == null || !int.TryParse(base.Request.QueryString["categoryId"], out this.CategoryId))
			{
				goto IL_0041;
			}
			goto IL_0041;
			IL_0041:
			Random random = new Random();
			this.rnd = random.NextDouble().ToString();
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				this.DoCallback();
			}
		}

		private void DoCallback()
		{
			string text = base.Request.QueryString["action"];
			base.Response.Clear();
			base.Response.ContentType = "application/json";
			if (text.Equals("getlist"))
			{
				int num = 0;
				int.TryParse(base.Request.QueryString["parentCategoryId"], out num);
				IEnumerable<CategoryInfo> enumerable = (num == 0) ? CatalogHelper.GetMainCategories() : CatalogHelper.GetSubCategories(num);
				if (enumerable == null)
				{
					base.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					base.Response.Write(this.GenerateJson(enumerable));
				}
			}
			else if (text.Equals("getinfo"))
			{
				int num2 = 0;
				int.TryParse(base.Request.QueryString["categoryId"], out num2);
				if (num2 <= 0)
				{
					base.Response.Write("{\"Status\":\"0\"}");
				}
				else
				{
					CategoryInfo category = CatalogHelper.GetCategory(num2);
					if (category == null)
					{
						base.Response.Write("{\"Status\":\"0\"}");
					}
					else
					{
						base.Response.Write("{\"Status\":\"OK\", \"Name\":\"" + category.Name + "\", \"Path\":\"" + category.Path + "\"}");
					}
				}
			}
			base.Response.End();
		}

		private string GenerateJson(IEnumerable<CategoryInfo> categories)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.Append("\"Categories\":[");
			foreach (CategoryInfo category in categories)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"CategoryId\":\"{0}\",", category.CategoryId.ToString(CultureInfo.InvariantCulture));
				stringBuilder.AppendFormat("\"HasChildren\":\"{0}\",", category.HasChildren ? "true" : "false");
				stringBuilder.AppendFormat("\"CategoryName\":\"{0}\"", category.Name);
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("]}");
			return stringBuilder.ToString();
		}
	}
}
