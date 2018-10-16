using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Store;
using Hidistro.UI.Web.ashxBase;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.comment.ashx
{
	[PrivilegeCheck(Privilege.ProductConsultationsManage)]
	public class ProductReviews : AdminBaseHandler
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
			ProductReviewQuery productReviewQuery = new ProductReviewQuery();
			productReviewQuery.Keywords = context.Request["Keywords"];
			string text = context.Request["Type"];
			if (!string.IsNullOrEmpty(text))
			{
				productReviewQuery.havedReply = text.ToBool();
			}
			productReviewQuery.startDate = base.GetDateTimeParam(context, "startDate");
			productReviewQuery.endDate = base.GetDateTimeParam(context, "endDate");
			if (productReviewQuery.endDate.HasValue)
			{
				productReviewQuery.endDate = productReviewQuery.endDate.Value.AddDays(1.0);
			}
			productReviewQuery.SortOrder = SortAction.Desc;
			productReviewQuery.PageIndex = base.CurrentPageIndex;
			productReviewQuery.PageSize = base.CurrentPageSize;
			productReviewQuery.SortBy = "ReviewDate";
			Globals.EntityCoding(productReviewQuery, true);
			DataGridViewModel<Dictionary<string, object>> dataList = this.GetDataList(productReviewQuery);
			string s = base.SerializeObjectToJson(dataList);
			context.Response.Write(s);
			context.Response.End();
		}

		private DataGridViewModel<Dictionary<string, object>> GetDataList(ProductReviewQuery query)
		{
			DataGridViewModel<Dictionary<string, object>> dataGridViewModel = new DataGridViewModel<Dictionary<string, object>>();
			if (query != null)
			{
				DbQueryResult productReviews = ProductCommentHelper.GetProductReviews(query);
				dataGridViewModel.rows = DataHelper.DataTableToDictionary(productReviews.Data);
				dataGridViewModel.total = productReviews.TotalRecords;
				foreach (Dictionary<string, object> row in dataGridViewModel.rows)
				{
					ProductInfo productById = ProductHelper.GetProductById(row["ProductId"].ToInt(0));
					ProductReviewInfo productReviewInfo = row.ToObject<ProductReviewInfo>();
					row.Add("Type", query.havedReply);
					if (productById.ProductName.Trim().Length > 22)
					{
						row.Add("ProductNameStr", productById.ProductName.Trim().Substring(0, 22) + "...");
					}
					else
					{
						row.Add("ProductNameStr", productById.ProductName.Trim());
					}
					if (productReviewInfo.ReviewText.Trim().Length >= 50)
					{
						row.Add("ReviewTextStr", productReviewInfo.ReviewText.Trim().Substring(0, 50) + "...");
					}
					else
					{
						row.Add("ReviewTextStr", productReviewInfo.ReviewText.Trim());
					}
					if (query.havedReply.ToBool())
					{
						if (productReviewInfo.ReplyText.Trim().Length >= 50)
						{
							row.Add("ReplyTextStr", productReviewInfo.ReplyText.Trim().Substring(0, 50) + "...");
						}
						else
						{
							row.Add("ReplyTextStr", productReviewInfo.ReplyText.Trim());
						}
					}
				}
			}
			return dataGridViewModel;
		}

		private void Delete(HttpContext context)
		{
			int value = base.GetIntParam(context, "ReviewId", false).Value;
			if (ProductCommentHelper.DeleteProductReview(value))
			{
				base.ReturnSuccessResult(context, "成功删除了选择的商品评论回复！", 0, true);
				return;
			}
			throw new HidistroAshxException("删除商品评论失败！");
		}
	}
}
