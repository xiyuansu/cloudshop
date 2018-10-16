using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SqlDal.Comments;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Comments
{
	public sealed class ProductCommentHelper
	{
		private ProductCommentHelper()
		{
		}

		public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			return new ProductConsultationDao().GetConsultationProducts(consultationQuery);
		}

		public static ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			return new ProductConsultationDao().Get<ProductConsultationInfo>(consultationId);
		}

		public static List<ProductConsultationInfo> GetProductConsultationList(int productId)
		{
			return new ProductConsultationDao().GetProductConsultationList(productId);
		}

		public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			return new ProductConsultationDao().Update(productConsultation, null);
		}

		public static bool DeleteProductConsultation(int consultationId)
		{
			return new ProductConsultationDao().Delete<ProductConsultationInfo>(consultationId);
		}

		public static int DeleteReview(IList<int> reviews)
		{
			if (reviews == null || reviews.Count == 0)
			{
				return 0;
			}
			int num = 0;
			ProductReviewDao productReviewDao = new ProductReviewDao();
			foreach (int review in reviews)
			{
				if (productReviewDao.Delete<ProductReviewInfo>(review))
				{
					num++;
				}
			}
			return num;
		}

		public static bool ReplyProductReview(int reviewId, string replyText)
		{
			Globals.EntityCoding(replyText, true);
			ProductReviewDao productReviewDao = new ProductReviewDao();
			ProductReviewInfo productReviewInfo = productReviewDao.Get<ProductReviewInfo>(reviewId);
			productReviewInfo.ReplyDate = DateTime.Now;
			productReviewInfo.ReplyText = replyText;
			return productReviewDao.Update(productReviewInfo, null);
		}

		public static bool BatchReplyProductReviews(IList<long> reviewIds, string replyText)
		{
			if (reviewIds == null || reviewIds.Count == 0)
			{
				return false;
			}
			return new ProductReviewDao().BatchReplyProductReviews(string.Join(",", reviewIds), replyText);
		}

		public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			return new ProductReviewDao().GetProductReviews(reviewQuery);
		}

		public static ProductReviewInfo GetProductReview(int reviewId)
		{
			return new ProductReviewDao().Get<ProductReviewInfo>(reviewId);
		}

		public static bool DeleteProductReview(long reviewId)
		{
			return new ProductReviewDao().Delete<ProductReviewInfo>(reviewId);
		}
	}
}
