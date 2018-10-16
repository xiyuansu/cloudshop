using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class ProductPreSale : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "setover":
				this.SetOver(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
			int num = 1;
			int num2 = 10;
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
			ProductPreSaleQuery productPreSaleQuery = new ProductPreSaleQuery();
			productPreSaleQuery.ProductName = context.Request["ProductName"];
			productPreSaleQuery.PreSaleStatus = base.GetIntParam(context, "PreSaleStatus", true);
			productPreSaleQuery.PageIndex = num;
			productPreSaleQuery.PageSize = num2;
			productPreSaleQuery.SortBy = "PreSaleId";
			productPreSaleQuery.SortOrder = SortAction.Desc;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(productPreSaleQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ProductPreSaleQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				PageModel<ProductPreSaleInfo> preSaleList = ProductPreSaleHelper.GetPreSaleList(query);
				dataGridViewModel.rows = new List<Dictionary<string, object>>();
				dataGridViewModel.total = preSaleList.Total;
				foreach (ProductPreSaleInfo model in preSaleList.Models)
				{
					Dictionary<string, object> dictionary = model.ToDictionary();
					dictionary.Add("IsPreSaleEnd", !this.IsPreSaleEnd(model.PreSaleEndDate));
					dictionary.Add("IsPreSaleHasOrder", ProductPreSaleHelper.IsPreSaleHasOrder(model.PreSaleId));
					dataGridViewModel.rows.Add(dictionary);
				}
			}
			return dataGridViewModel;
		}

		private bool IsPreSaleEnd(DateTime dt)
		{
			if (dt.CompareTo(DateTime.Now) > 0)
			{
				return true;
			}
			return false;
		}

		public void Delete(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "preSaleId", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (ProductPreSaleHelper.IsPreSaleHasOrder(intParam.Value))
			{
				throw new HidistroAshxException("该预售已产生订单，不能删除!");
			}
			if (ProductPreSaleHelper.DeletePreSale(intParam.Value))
			{
				base.ReturnSuccessResult(context, "删除成功", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败!");
		}

		private void SetOver(HttpContext context)
		{
			int? intParam = base.GetIntParam(context, "preSaleId", false);
			if (!intParam.HasValue)
			{
				throw new HidistroAshxException("错误的编号");
			}
			if (ProductPreSaleHelper.SetPreSaleGameOver(intParam.Value))
			{
				base.ReturnSuccessResult(context, "操作成功", 0, true);
				return;
			}
			throw new HidistroAshxException("操作失败");
		}
	}
}
