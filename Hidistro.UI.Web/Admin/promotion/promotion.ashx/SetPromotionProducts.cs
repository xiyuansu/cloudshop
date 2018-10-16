using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class SetPromotionProducts : AdminBaseHandler
	{
		private int activityId;

		private PromotionInfo promotion = null;

		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
			this.activityId = base.GetIntParam(context, "activityId", false).Value;
			this.promotion = PromoteHelper.GetPromotion(this.activityId);
			if (this.activityId < 1 || this.promotion == null)
			{
				throw new HidistroAshxException("错误的参数：活动编码");
			}
			switch (base.action)
			{
			case "getlist":
				this.GetList(context);
				break;
			case "delete":
				this.Delete(context);
				break;
			case "clear":
				this.Clear(context);
				break;
			default:
				throw new HidistroAshxException("错误的参数");
			}
		}

		public void GetList(HttpContext context)
		{
			bool value = base.GetBoolParam(context, "isWholesale", false).Value;
			bool value2 = base.GetBoolParam(context, "isMobileExclusive", false).Value;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList();
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList()
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataTable promotionProducts = PromoteHelper.GetPromotionProducts(this.activityId);
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(promotionProducts);
			dataGridViewModel.total = promotionProducts.Rows.Count;
			foreach (Dictionary<string, object> row in dataGridViewModel.rows)
			{
				row["ThumbnailUrl40"] = base.GetImageOrDefaultImage(row["ThumbnailUrl40"], "");
				row["MarketPrice"] = row["MarketPrice"].ToDecimal(0);
			}
			return dataGridViewModel;
		}

		public void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "id", false).Value;
			if (value < 1)
			{
				throw new HidistroAshxException("错误的商品编号");
			}
			if (PromoteHelper.DeletePromotionProducts(this.activityId, value))
			{
				base.ReturnSuccessResult(context, "成功删除了选择的促销活动商品", 0, true);
				return;
			}
			throw new HidistroAshxException("删除失败");
		}

		public void Clear(HttpContext context)
		{
			if (PromoteHelper.DeletePromotionProducts(this.activityId, null))
			{
				base.ReturnSuccessResult(context, "成功清空了促销活动的所有商品", 0, true);
				return;
			}
			throw new HidistroAshxException("清空失败");
		}
	}
}
