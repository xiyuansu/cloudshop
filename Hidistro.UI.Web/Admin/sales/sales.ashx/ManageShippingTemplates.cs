using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.sales.ashx
{
	[PrivilegeCheck(Privilege.ShippingTemplets)]
	public class ManageShippingTemplates : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			string text = context.Request["action"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string a = text;
			if (!(a == "getlistShippingTemplates"))
			{
				if (a == "deleteTemplates")
				{
					this.DeleteTemplates(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetListShippingTemplates(context);
		}

		private void DeleteTemplates(HttpContext context)
		{
			int num = context.Request["TemplateId"].ToInt(0);
			if (num > 0)
			{
				if (SalesHelper.ShippingTemplateIsExistProdcutRelation(num))
				{
					base.ReturnFailResult(context, "运费模板已关联商品,不能进行删除", -1, true);
				}
				else
				{
					SalesHelper.DeleteShippingTemplate(num);
					base.ReturnSuccessResult(context, "已经成功删除选择的运费模版", 0, true);
				}
			}
		}

		public void GetListShippingTemplates(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
			Pagination pagination = new Pagination();
			num = base.GetIntParam(context, "page", false).Value;
			if (num < 1)
			{
				num = 1;
			}
			num2 = base.GetIntParam(context, "rows", false).Value;
			if (num2 < 1)
			{
				num2 = 10;
			}
			pagination.PageIndex = num;
			pagination.PageSize = num2;
			pagination.IsCount = true;
			pagination.SortBy = "TemplateId";
			pagination.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> shippingTemplates = this.GetShippingTemplates(pagination);
			string s = base.SerializeObjectToJson(shippingTemplates);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetShippingTemplates(Pagination query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			string empty = string.Empty;
			if (query != null)
			{
				DbQueryResult shippingTemplates = SalesHelper.GetShippingTemplates(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(shippingTemplates.Data);
				foreach (Dictionary<string, object> item in list)
				{
					ShippingTemplateInfo shippingTemplateInfo = item.ToObject<ShippingTemplateInfo>();
					item.Add("ShowNumberAndUnit", SalesHelper.GetShowNumberAndUnit((int)shippingTemplateInfo.ValuationMethod, shippingTemplateInfo.DefaultNumber));
					item.Add("AddNumberStr", SalesHelper.GetShowNumberAndUnit((int)shippingTemplateInfo.ValuationMethod, shippingTemplateInfo.AddNumber));
					if (shippingTemplateInfo.IsFreeShipping)
					{
						item.Add("IsFreeShippingImg", "<img src=\"../images/da.gif\" />");
					}
					else
					{
						item.Add("IsFreeShippingImg", "<img src=\"../images/del.png\" style=\"margin-left:7px;\" />");
					}
					item.Add("ValuationMethodStr", ((Enum)(object)shippingTemplateInfo.ValuationMethod).ToDescription());
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = shippingTemplates.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
