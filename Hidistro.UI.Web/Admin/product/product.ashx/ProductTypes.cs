using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.product.ashx
{
	[PrivilegeCheck(Privilege.ProductTypes)]
	public class ProductTypes : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			string action = base.action;
			if (!(action == "getlist"))
			{
				if (action == "delete")
				{
					this.Delete(context);
					return;
				}
				throw new HidistroAshxException("错误的参数");
			}
			this.GetList(context);
		}

		private void GetList(HttpContext context)
		{
			ProductTypeQuery productTypeQuery = new ProductTypeQuery();
			productTypeQuery.TypeName = context.Request["Keywords"];
			productTypeQuery.SortOrder = SortAction.Desc;
			productTypeQuery.PageIndex = base.CurrentPageIndex;
			productTypeQuery.PageSize = base.CurrentPageSize;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(productTypeQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ProductTypeQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult productTypes = ProductTypeHelper.GetProductTypes(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(productTypes.Data);
				dataGridViewModel.total = productTypes.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "typeId", false).Value;
			if (value <= 0)
			{
				throw new HidistroAshxException("错误的参数");
			}
			if (ProductTypeHelper.DeleteProductType(value))
			{
				base.ReturnSuccessResult(context, "删除成功！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除商品类型失败, 可能有商品正在使用该类型！");
		}
	}
}
