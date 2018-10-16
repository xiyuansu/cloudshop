using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class OrderPromotions : AdminBaseHandler
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
			case "getstores":
				this.GetStores(context);
				break;
			case "getenablestores":
				this.GetEnableStores(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		private void GetEnableStores(HttpContext context)
		{
			StoreEntityQuery query = new StoreEntityQuery
			{
				TagId = context.Request.QueryString["tagId"].ToInt(0),
				RegionId = context.Request.QueryString["regionId"].ToInt(0),
				Key = context.Request.QueryString["key"].ToString()
			};
			List<int> allStoresForOrderPromotions = StoreListHelper.GetAllStoresForOrderPromotions(query);
			string s = base.SerializeObjectToJson(allStoresForOrderPromotions);
			context.Response.Write(s);
			context.Response.End();
		}

		private void GetStores(HttpContext context)
		{
			StoreEntityQuery query = new StoreEntityQuery
			{
				TagId = context.Request.QueryString["tagId"].ToInt(0),
				RegionId = context.Request.QueryString["regionId"].ToInt(0),
				PageIndex = context.Request.QueryString["pageIndex"].ToInt(0),
				PageSize = context.Request.QueryString["pageSize"].ToInt(0),
				Key = context.Request.QueryString["key"]
			};
			PageModel<StoreForPromotion> storesForOrderPromotions = StoreListHelper.GetStoresForOrderPromotions(query);
			string s = base.SerializeObjectToJson(storesForOrderPromotions);
			context.Response.Write(s);
			context.Response.End();
		}

		public void GetList(HttpContext context)
		{
			bool value = base.GetBoolParam(context, "isWholesale", false).Value;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(value);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(bool isWholesale)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataTable promotions = PromoteHelper.GetPromotions(false, isWholesale, false);
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(promotions);
			dataGridViewModel.total = promotions.Rows.Count;
			foreach (Dictionary<string, object> row in dataGridViewModel.rows)
			{
				PromoteType promoteType = (PromoteType)row["PromoteType"].ToInt(0);
				row.Add("PromoteTypeText", "");
				row.Add("PromoteInfo", "");
				switch (promoteType)
				{
				case PromoteType.FullAmountReduced:
					row["PromoteTypeText"] = "满额优惠金额";
					row["PromoteInfo"] = string.Format("满足金额：{0} 优惠金额：{1}", row["Condition"].ToDecimal(0).F2ToString("f2"), row["DiscountValue"].ToDecimal(0).F2ToString("f2"));
					break;
				case PromoteType.FullQuantityDiscount:
					row["PromoteTypeText"] = "满量打折";
					row["PromoteInfo"] = string.Format("满足数量：{0} 折扣值：{1}", row["Condition"].ToInt(0).F2ToString("f2"), row["DiscountValue"].ToDecimal(0).F2ToString("f2"));
					break;
				case PromoteType.FullQuantityReduced:
					row["PromoteTypeText"] = "满量优惠金额";
					row["PromoteInfo"] = string.Format("满足数量：{0} 优惠金额：{1}", row["Condition"].ToInt(0).F2ToString("f2"), row["DiscountValue"].ToDecimal(0).F2ToString("f2"));
					break;
				case PromoteType.FullAmountSentGift:
					row["PromoteTypeText"] = "满额送礼品";
					break;
				case PromoteType.FullAmountSentTimesPoint:
					row["PromoteTypeText"] = "满额送倍数积分";
					row["PromoteInfo"] = string.Format("满足金额：{0},倍数：{1}", row["Condition"].ToDecimal(0).F2ToString("f2"), row["DiscountValue"].ToDecimal(0).F2ToString("f2"));
					break;
				case PromoteType.FullAmountSentFreight:
					row["PromoteTypeText"] = "满额免运费";
					row["PromoteInfo"] = string.Format("满足金额：{0}", row["Condition"].ToDecimal(0).F2ToString("f2"));
					break;
				}
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的活动编号");
			}
			if (PromoteHelper.DeletePromotion(value))
			{
				base.ReturnSuccessResult(context, "成功删除了选择的促销活动", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败");
		}
	}
}
