using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Supplier.system.ashx
{
	public class SystemManage : SupplierAdminHandler
	{
		private new ManagerInfo CurrentManager;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			this.CurrentManager = HiContext.Current.Manager;
			string text = context.Request["action"];
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string a = text;
			if (a == "getlistShippingTemplates")
			{
				this.GetListShippingTemplates(context);
				return;
			}
			throw new HidistroAshxException("错误的参数");
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
			if (query != null)
			{
				DbQueryResult shippingTemplates = SalesHelper.GetShippingTemplates(query);
				List<Dictionary<string, object>> list = DataHelper.DataTableToDictionary(shippingTemplates.Data);
				foreach (Dictionary<string, object> item in list)
				{
					ShippingTemplateInfo shippingTemplateInfo = item.ToObject<ShippingTemplateInfo>();
					item.Add("ShowNumberAndUnit", SalesHelper.GetShowNumberAndUnit((int)shippingTemplateInfo.ValuationMethod, shippingTemplateInfo.DefaultNumber));
					item.Add("AddNumberStr", SalesHelper.GetShowNumberAndUnit(shippingTemplateInfo.ValuationMethod.ToInt(0), shippingTemplateInfo.AddNumber));
					if (shippingTemplateInfo.IsFreeShipping)
					{
						item.Add("IsFreeShippingImg", "<img src=\"../images/da.gif\" />");
					}
					else
					{
						item.Add("IsFreeShippingImg", "<img src=\"../images/del.png\" style=\"margin-left:7px;\" />");
					}
					if (shippingTemplateInfo.ValuationMethod == ValuationMethods.Number)
					{
						item.Add("ValuationMethodStr", "按件数");
					}
					else
					{
						item.Add("ValuationMethodStr", "按重量");
					}
				}
				dataGridViewModel.rows = list;
				dataGridViewModel.total = shippingTemplates.TotalRecords;
			}
			return dataGridViewModel;
		}
	}
}
