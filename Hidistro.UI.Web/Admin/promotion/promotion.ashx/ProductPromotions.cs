using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Promotions;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion.ashx
{
	public class ProductPromotions : AdminBaseHandler
	{
		public override void OnLoad(HttpContext context)
		{
			base.OnLoad(context);
			if (string.IsNullOrWhiteSpace(base.action))
			{
				throw new HidistroAshxException("错误的参数");
			}
			base.action = base.action.ToLower();
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

		public void GetList(HttpContext context)
		{
			bool value = base.GetBoolParam(context, "isWholesale", false).Value;
			bool value2 = base.GetBoolParam(context, "isMobileExclusive", false).Value;
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(value, value2);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(bool isWholesale, bool isMobileExclusive)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			DataTable promotions = PromoteHelper.GetPromotions(true, isWholesale, isMobileExclusive);
			dataGridViewModel.rows = DataHelper.DataTableToDictionary(promotions);
			dataGridViewModel.total = promotions.Rows.Count;
			foreach (Dictionary<string, object> row in dataGridViewModel.rows)
			{
				PromoteType promoteType = (PromoteType)row["PromoteType"].ToInt(0);
				row.Add("PromoteTypeText", "");
				row.Add("PromoteInfo", "");
				switch (promoteType)
				{
				case PromoteType.QuantityDiscount:
					row["PromoteTypeText"] = "按批发数量打折";
					row["PromoteInfo"] = string.Format("购买数量：{0} 折扣值：{1}", row["Condition"].ToInt(0).F2ToString("f2"), row["DiscountValue"].ToDecimal(0).F2ToString("f2"));
					break;
				case PromoteType.SentGift:
					row["PromoteTypeText"] = "买商品送礼品";
					break;
				case PromoteType.SentProduct:
				{
					row["PromoteTypeText"] = "有买有送";
					Dictionary<string, object> dictionary = row;
					int num = row["Condition"].ToInt(0);
					string arg = num.ToString();
					num = row["DiscountValue"].ToInt(0);
					dictionary["PromoteInfo"] = $"购买数量：{arg} 赠送数量：{num.ToString()}";
					break;
				}
				case PromoteType.MobileExclusive:
					row["PromoteTypeText"] = "手机专享价";
					row["PromoteInfo"] = string.Format("移动端立减{0}元", row["DiscountValue"].ToDecimal(0).F2ToString("f2"));
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
