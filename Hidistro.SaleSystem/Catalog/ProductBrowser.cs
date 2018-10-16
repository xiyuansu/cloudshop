using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Commodities;
using Hidistro.SaleSystem.Depot;
using Hidistro.SaleSystem.Members;
using Hidistro.SqlDal.Comments;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.SaleSystem.Catalog
{
	public static class ProductBrowser
	{
		public static DataTable GetSaleProductRanking(int categoryId, int maxNum)
		{
			return new ProductBrowseDao().GetSaleProductRanking(CatalogHelper.GetCategory(categoryId), maxNum);
		}

		public static DataTable GetSubjectList(SubjectListQuery query)
		{
			return new ProductBrowseDao().GetSubjectList(query);
		}

		public static ProductInfo GetProductSimpleInfo(int productId)
		{
			return new ProductDao().GetProductDetails(productId);
		}

		public static ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxConsultationNum, bool MutiStores = false, int gradeId = 0)
		{
			BrowsedProductQueue.EnQueue(productId);
			ProductBrowseDao productBrowseDao = new ProductBrowseDao();
			productBrowseDao.AddVistiCounts(productId);
			if (gradeId == 0)
			{
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0)
				{
					gradeId = user.GradeId;
				}
			}
			string key = $"DataCache-Product-{productId}-{gradeId}-{MutiStores}";
			ProductBrowseInfo productBrowseInfo = HiCache.Get<ProductBrowseInfo>(key);
			if (productBrowseInfo == null)
			{
				productBrowseInfo = productBrowseDao.GetProductBrowseInfo(productId, gradeId, maxConsultationNum, MutiStores);
				if (productBrowseInfo.DbCorrelatives != null && productBrowseInfo.DbCorrelatives.Rows.Count > 0)
				{
					int num = 0;
					foreach (DataRow row in productBrowseInfo.DbCorrelatives.Rows)
					{
						if (string.IsNullOrEmpty(row["ThumbnailUrl60"].ToNullString()))
						{
							productBrowseInfo.DbCorrelatives.Rows[num]["ThumbnailUrl60"] = HiContext.Current.SiteSettings.DefaultProductThumbnail2;
						}
						if (string.IsNullOrEmpty(row["ThumbnailUrl100"].ToNullString()))
						{
							productBrowseInfo.DbCorrelatives.Rows[num]["ThumbnailUrl100"] = HiContext.Current.SiteSettings.DefaultProductThumbnail3;
						}
						if (string.IsNullOrEmpty(row["ThumbnailUrl160"].ToNullString()))
						{
							productBrowseInfo.DbCorrelatives.Rows[num]["ThumbnailUrl160"] = HiContext.Current.SiteSettings.DefaultProductThumbnail4;
						}
						if (string.IsNullOrEmpty(row["ThumbnailUrl180"].ToNullString()))
						{
							productBrowseInfo.DbCorrelatives.Rows[num]["ThumbnailUrl180"] = HiContext.Current.SiteSettings.DefaultProductThumbnail5;
						}
						if (string.IsNullOrEmpty(row["ThumbnailUrl220"].ToNullString()))
						{
							productBrowseInfo.DbCorrelatives.Rows[num]["ThumbnailUrl220"] = HiContext.Current.SiteSettings.DefaultProductThumbnail6;
						}
						if (string.IsNullOrEmpty(row["ThumbnailUrl310"].ToNullString()))
						{
							productBrowseInfo.DbCorrelatives.Rows[num]["ThumbnailUrl310"] = HiContext.Current.SiteSettings.DefaultProductThumbnail7;
						}
						num++;
					}
				}
				if (productBrowseInfo.ListCorrelatives != null && productBrowseInfo.ListCorrelatives.Count > 0)
				{
					int num2 = 0;
					foreach (ProductInfo listCorrelative in productBrowseInfo.ListCorrelatives)
					{
						if (string.IsNullOrEmpty(listCorrelative.ThumbnailUrl60.ToNullString()))
						{
							productBrowseInfo.ListCorrelatives[num2].ThumbnailUrl60 = HiContext.Current.SiteSettings.DefaultProductThumbnail2;
						}
						if (string.IsNullOrEmpty(listCorrelative.ThumbnailUrl100.ToNullString()))
						{
							productBrowseInfo.ListCorrelatives[num2].ThumbnailUrl100 = HiContext.Current.SiteSettings.DefaultProductThumbnail3;
						}
						if (string.IsNullOrEmpty(listCorrelative.ThumbnailUrl160.ToNullString().ToNullString()))
						{
							productBrowseInfo.ListCorrelatives[num2].ThumbnailUrl160 = HiContext.Current.SiteSettings.DefaultProductThumbnail4;
						}
						if (string.IsNullOrEmpty(listCorrelative.ThumbnailUrl180.ToNullString()))
						{
							productBrowseInfo.ListCorrelatives[num2].ThumbnailUrl180 = HiContext.Current.SiteSettings.DefaultProductThumbnail5;
						}
						if (string.IsNullOrEmpty(listCorrelative.ThumbnailUrl220.ToNullString()))
						{
							productBrowseInfo.ListCorrelatives[num2].ThumbnailUrl220 = HiContext.Current.SiteSettings.DefaultProductThumbnail6;
						}
						if (string.IsNullOrEmpty(listCorrelative.ThumbnailUrl310.ToNullString()))
						{
							productBrowseInfo.ListCorrelatives[num2].ThumbnailUrl310 = HiContext.Current.SiteSettings.DefaultProductThumbnail7;
						}
						num2++;
					}
				}
				HiCache.Insert(key, productBrowseInfo, 600);
			}
			return productBrowseInfo;
		}

		public static ProductBrowseInfo GetWAPProductBrowseInfo(int productId, int? maxConsultationNum, bool MutiStores = false, int gradeId = 0)
		{
			BrowsedProductQueue.EnQueue(productId);
			ProductBrowseDao productBrowseDao = new ProductBrowseDao();
			productBrowseDao.AddVistiCounts(productId);
			if (gradeId == 0)
			{
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0)
				{
					gradeId = user.GradeId;
				}
			}
			return productBrowseDao.GetWAPProductBrowseInfo(productId, gradeId, maxConsultationNum, MutiStores);
		}

		public static ProductBrowseInfo GetAppletProductBrowseInfo(int productId, int gradeId = 0)
		{
			ProductBrowseDao productBrowseDao = new ProductBrowseDao();
			productBrowseDao.AddVistiCounts(productId);
			if (gradeId == 0)
			{
				MemberInfo user = HiContext.Current.User;
				if (user.UserId != 0)
				{
					gradeId = user.GradeId;
				}
			}
			string key = $"DataCache-AppletProduct-{productId}-{gradeId}";
			ProductBrowseInfo productBrowseInfo = HiCache.Get<ProductBrowseInfo>(key);
			if (productBrowseInfo == null)
			{
				productBrowseInfo = productBrowseDao.GetAppletProductBrowseInfo(productId, gradeId);
				HiCache.Insert(key, productBrowseInfo, 600);
			}
			return productBrowseInfo;
		}

		public static ProductInfo GetProductDescription(int productId)
		{
			ProductBrowseDao productBrowseDao = new ProductBrowseDao();
			return productBrowseDao.GetProductDescription(productId);
		}

		public static ProductModel GetStoreProduct(StoreProductQuery query)
		{
			ProductBrowseDao productBrowseDao = new ProductBrowseDao();
			productBrowseDao.AddVistiCounts(query.ProductId);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			ProductModel result = productBrowseDao.GetProductModel(query);
			if (string.IsNullOrEmpty(result.SubmitOrderImg))
			{
				result.SubmitOrderImg = masterSettings.DefaultProductThumbnail4;
			}
			if (result.StoreInfo != null && !string.IsNullOrEmpty(result.StoreInfo.FullRegionPath))
			{
				string tempAllRegions = result.StoreInfo.FullRegionPath + ",";
				Dictionary<string, string> regionName = RegionHelper.GetRegionName(tempAllRegions);
				result.StoreInfo.AddressSimply = StoreListHelper.ProcessAddress(regionName, result.StoreInfo.FullRegionPath, result.StoreInfo.Address);
			}
			if (result.ImgUrlList != null && result.ImgUrlList.Count > 0)
			{
				string[] array = new string[result.ImgUrlList.Count];
				Array.Copy(result.ImgUrlList.ToArray(), array, result.ImgUrlList.Count);
				result.ImgUrlList = new List<string>();
				array.ForEach(delegate(string t)
				{
					if (!string.IsNullOrEmpty(t))
					{
						result.ImgUrlList.Add(t);
					}
				});
			}
			else
			{
				result.ImgUrlList = new List<string>
				{
					masterSettings.DefaultProductImage
				};
			}
			if (result.ImgUrlList.Count == 0)
			{
				result.ImgUrlList = new List<string>
				{
					masterSettings.DefaultProductImage
				};
			}
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				int gradeId = user.GradeId;
				if (query.CountDownId == 0)
				{
					MemberGradeInfo memberGrade = MemberHelper.GetMemberGrade(gradeId);
					if (memberGrade != null)
					{
						result.GradeName = memberGrade.Name;
						result.MinSalePrice = (decimal)memberGrade.Discount * result.MinSalePrice / 100m;
						result.MaxSalePrice = (decimal)memberGrade.Discount * result.MaxSalePrice / 100m;
					}
					result.Skus.ForEach(delegate(SKUItem sk)
					{
						sk.SalePrice = (decimal)memberGrade.Discount * sk.SalePrice / 100m;
					});
				}
				result.IsFavorite = ProductBrowser.ExistsProduct(result.ProductId, user.UserId, result.StoreId);
				if (user != null && user.IsReferral() && (!(masterSettings.SubMemberDeduct <= decimal.Zero) || result.SubMemberDeduct.HasValue))
				{
					if (!result.SubMemberDeduct.HasValue)
					{
						goto IL_0381;
					}
					decimal? subMemberDeduct = result.SubMemberDeduct;
					if (!(subMemberDeduct.GetValueOrDefault() <= default(decimal)) || !subMemberDeduct.HasValue)
					{
						goto IL_0381;
					}
				}
				goto IL_03b9;
			}
			goto IL_0445;
			IL_0445:
			if (result.StoreInfo == null)
			{
				result.ExStatus = DetailException.StopService;
			}
			else if (!result.StoreInfo.IsOpen && result.StoreInfo.CloseEndTime.HasValue && result.StoreInfo.CloseStartTime.HasValue && result.StoreInfo.CloseEndTime.Value > DateTime.Now && result.StoreInfo.CloseStartTime.Value < DateTime.Now)
			{
				result.ExStatus = DetailException.StopService;
			}
			else if (result.Stock == 0)
			{
				result.ExStatus = DetailException.NoStock;
			}
			else if (!result.StoreInfo.IsInServiceArea)
			{
				result.ExStatus = DetailException.OverServiceArea;
			}
			else if (!masterSettings.Store_IsOrderInClosingTime)
			{
				DateTime dateTime = DateTime.Now;
				string str = dateTime.ToString("yyyy-MM-dd");
				dateTime = result.StoreInfo.OpenStartTime;
				DateTime value = (str + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
				dateTime = DateTime.Now;
				string str2 = dateTime.ToString("yyyy-MM-dd");
				dateTime = result.StoreInfo.OpenEndTime;
				DateTime dateTime2 = (str2 + " " + dateTime.ToString("HH:mm")).ToDateTime().Value;
				if (dateTime2 <= value)
				{
					dateTime2 = dateTime2.AddDays(1.0);
				}
				if (DateTime.Now < value || DateTime.Now > dateTime2)
				{
					result.ExStatus = DetailException.IsNotWorkTime;
				}
			}
			BrowsedProductQueue.EnQueue(query.ProductId);
			return result;
			IL_0381:
			int num;
			if (HiContext.Current.SiteSettings.OpenReferral == 1 && HiContext.Current.SiteSettings.ShowDeductInProductPage && user.Referral != null)
			{
				num = (user.Referral.IsRepeled ? 1 : 0);
				goto IL_03ba;
			}
			goto IL_03b9;
			IL_03ba:
			if (num != 0)
			{
				result.ProductReduce = "";
			}
			else
			{
				decimal d = result.SubMemberDeduct.HasValue ? result.SubMemberDeduct.Value : masterSettings.SubMemberDeduct;
				result.ProductReduce = (result.MinSalePrice * d / 100m).F2ToString("f2");
			}
			goto IL_0445;
			IL_03b9:
			num = 1;
			goto IL_03ba;
		}

		public static ProductBrowseInfo GetProductPreSaleBrowseInfo(int productId, bool IsConsul = false)
		{
			ProductBrowseDao productBrowseDao = new ProductBrowseDao();
			productBrowseDao.AddVistiCounts(productId);
			if (IsConsul)
			{
				return productBrowseDao.GetProductBrowseInfo(productId, 0, null, false);
			}
			return productBrowseDao.GetProductPreSaleBrowseInfo(productId);
		}

		public static DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
		{
			return new ProductBrowseDao().GetBrowseProductList(query);
		}

		public static DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
		{
			query.ProductSaleStatus = ProductSaleStatus.UnSale;
			return new ProductBrowseDao().GetBrowseProductList(query);
		}

		public static DataTable GetVistiedProducts(IList<int> productIds)
		{
			return new ProductBrowseDao().GetVistiedProducts(productIds);
		}

		public static IList<AppProductYouLikeModel> GetNewProductYouLikeModel(int productId, int storeId, int count = 0, List<int> likePids = null, bool showServiceProduct = false)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			List<ProductYouLikeModel> productYouLike = BrowsedProductQueue.GetProductYouLike(productId, storeId, likePids, true);
			IList<AppProductYouLikeModel> list = new List<AppProductYouLikeModel>();
			if (productYouLike != null && productYouLike.Count > 0)
			{
				int num = 0;
				foreach (ProductYouLikeModel item in productYouLike)
				{
					AppProductYouLikeModel appProductYouLikeModel = new AppProductYouLikeModel();
					appProductYouLikeModel.MarketPrice = item.MarketPrice;
					appProductYouLikeModel.ProductCode = "";
					appProductYouLikeModel.ProductId = item.ProductId;
					appProductYouLikeModel.ProductName = item.ProductName;
					appProductYouLikeModel.ProductType = item.ProductType;
					appProductYouLikeModel.SaleCounts = 0;
					appProductYouLikeModel.SalePrice = item.SalePrice;
					appProductYouLikeModel.ShortDescription = "";
					appProductYouLikeModel.ThumbnailUrl100 = (string.IsNullOrEmpty(item.ProdImg) ? Globals.FullPath(masterSettings.DefaultProductImage) : Globals.FullPath(item.ProdImg));
					appProductYouLikeModel.ThumbnailUrl40 = appProductYouLikeModel.ThumbnailUrl100;
					appProductYouLikeModel.ThumbnailUrl60 = appProductYouLikeModel.ThumbnailUrl100;
					appProductYouLikeModel.ThumbnailUrl160 = appProductYouLikeModel.ThumbnailUrl100;
					appProductYouLikeModel.ThumbnailUrl180 = appProductYouLikeModel.ThumbnailUrl100;
					list.Add(appProductYouLikeModel);
					if (count > 0 && num == count - 1)
					{
						break;
					}
					num++;
				}
			}
			else
			{
				list = new List<AppProductYouLikeModel>();
			}
			return list;
		}

		public static DataTable GetSkus(int productId)
		{
			return new SkuDao().GetSkus(productId);
		}

		public static DataTable GetSkus(int productId, int storeid)
		{
			return new SkuDao().GetSkus(productId, storeid);
		}

		public static DataTable GetExpandAttributes(int productId)
		{
			return new SkuDao().GetExpandAttributes(productId);
		}

		public static IList<ExtendAttributeInfo> GetExpandAttributeList(int productId)
		{
			DataTable expandAttributes = new SkuDao().GetExpandAttributes(productId);
			IList<ExtendAttributeInfo> list = new List<ExtendAttributeInfo>();
			if (expandAttributes != null && expandAttributes.Rows.Count > 0)
			{
				foreach (DataRow row in expandAttributes.Rows)
				{
					list.Add(new ExtendAttributeInfo
					{
						ExtAttrName = row["AttributeName"].ToNullString(),
						ExtAttrValue = row["ValueStr"].ToNullString()
					});
				}
			}
			return list;
		}

		public static DbQueryResult GetBrandProducts(int? brandId, int pageNumber, int maxNum)
		{
			return new ProductBrowseDao().GetBrandProducts(brandId, pageNumber, maxNum);
		}

		public static bool CheckHasCollect(int memberId, int productId)
		{
			return new FavoriteDao().CheckHasCollect(memberId, productId);
		}

		public static DbQueryResult GetProducts(int categoryId, string keyWord, string productIds, int pageNumber, int maxNum, string sort, string order)
		{
			return new ProductBrowseDao().GetProducts(CatalogHelper.GetCategory(categoryId), keyWord, productIds, pageNumber, maxNum, sort, order == "asc");
		}

		public static DbQueryResult GetFavorites(string keyword, string tags, Pagination page, bool isFromPC = false)
		{
			return new FavoriteDao().GetFavorites(HiContext.Current.UserId, keyword, tags, page, isFromPC);
		}

		public static bool DeleteFavorite(int favoriteId)
		{
			return new FavoriteDao().DeleteFavorite(favoriteId) > 0;
		}

		public static int DeleteFavorite(int userId, int productId, int storeId = 0)
		{
			return new FavoriteDao().DeleteFavorite(userId, productId, storeId);
		}

		public static int DeleteFavoriteTags(string tagname)
		{
			return new FavoriteDao().DeleteFavoriteTags(tagname, HiContext.Current.UserId);
		}

		public static string GetFavoriteTags()
		{
			if (HiContext.Current.UserId == 0)
			{
				return string.Empty;
			}
			string text = string.Empty;
			DataSet favoriteTags = new FavoriteDao().GetFavoriteTags(HiContext.Current.UserId);
			if (favoriteTags.Tables.Count > 0)
			{
				foreach (DataRow row in favoriteTags.Tables[0].Rows)
				{
					text = text + "{\"TagId\":\"" + row["TagId"] + "\",\"TagName\":\"" + row["TagName"] + "\"},";
				}
			}
			if (text.Length > 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		public static bool DeleteFavorites(string ids)
		{
			return new FavoriteDao().DeleteFavorites(ids);
		}

		public static int AddProductToFavorite(int productId, int userId, int storeId = 0)
		{
			FavoriteDao favoriteDao = new FavoriteDao();
			if (favoriteDao.ExistsProduct(productId, userId, storeId))
			{
				return (favoriteDao.DeleteFavorite(userId, productId, storeId) > 0) ? 1 : 0;
			}
			return (ProductBrowser.AddProduct(productId, userId, storeId) > 0) ? 2 : 0;
		}

		public static int AddProduct(int productId, int userId, int storeId = 0)
		{
			FavoriteDao favoriteDao = new FavoriteDao();
			if (favoriteDao.ExistsProduct(productId, userId, storeId))
			{
				return 1;
			}
			FavoriteInfo model = new FavoriteInfo
			{
				ProductId = productId,
				UserId = userId,
				StoreId = storeId,
				Tags = ""
			};
			return (int)favoriteDao.Add(model, null);
		}

		public static DataTable GetFavoritesTypeTags()
		{
			FavoriteDao favoriteDao = new FavoriteDao();
			return favoriteDao.GetTypeTags(HiContext.Current.UserId);
		}

		public static int UpdateFavorite(int favoriteId, string tags, string remark)
		{
			FavoriteDao favoriteDao = new FavoriteDao();
			string text = "";
			string[] array = null;
			array = ((!tags.Contains(",")) ? new string[1]
			{
				tags
			} : tags.Split(','));
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				if (!string.IsNullOrEmpty(DataHelper.CleanSearchString(text2.Trim())))
				{
					favoriteDao.UpdateOrAddFavoriteTags(text2.Trim(), HiContext.Current.UserId);
					string text3 = tags.Replace(text2, "");
					int num = (tags.Length - text3.Length) / text2.Length;
					if (num == 1 || !text.Contains(text2))
					{
						text = text + DataHelper.CleanSearchString(text2) + ",";
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Substring(0, text.Length - 1);
			}
			return new FavoriteDao().UpdateFavorite(favoriteId, tags, remark);
		}

		public static bool ExistsProduct(int productId, int userId, int storeId = 0)
		{
			return new FavoriteDao().ExistsProduct(productId, userId, storeId);
		}

		public static int GetUserFavoriteCount()
		{
			return new FavoriteDao().GetUserFavoriteCount(HiContext.Current.UserId);
		}

		public static bool CheckAllProductReview(string orderId)
		{
			return new ProductReviewDao().CheckAllProductReview(orderId);
		}

		public static bool InsertProductReview(ProductReviewInfo review)
		{
			try
			{
				Globals.EntityCoding(review, true);
				long num = new ProductReviewDao().Add(review, null);
				if (num > 0)
				{
					ProductHelper.ClearProductCahe(review.ProductId);
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
		{
			return new ProductReviewDao().GetProductReviews(reviewQuery);
		}

		public static DataTable GetProductReviewScore(int productId)
		{
			return new ProductReviewDao().GetProductReviewScore(productId);
		}

		public static DataTable GetProductReviewAll(string orderid)
		{
			return new ProductReviewDao().GetProductReviewAll(orderid);
		}

		public static IEnumerable<ProductReviewMode> GetProductReviews(int maxNum)
		{
			IEnumerable<ProductReviewMode> enumerable = HiCache.Get<IEnumerable<ProductReviewMode>>("DataCache-LatestReviews");
			if (enumerable == null)
			{
				enumerable = new ProductReviewDao().GetLastProductReviews(maxNum);
				HiCache.Insert("DataCache-LatestReviews", enumerable, 300);
			}
			return enumerable;
		}

		public static void LoadProductReview(int productId, out int buyNum, out int reviewNum, string OrderId = "")
		{
			new ProductReviewDao().LoadProductReview(productId, HiContext.Current.UserId, out buyNum, out reviewNum, OrderId);
		}

		public static int GetUserProductReviewsCount()
		{
			return new ProductReviewDao().GetUserProductReviewsCount(HiContext.Current.UserId);
		}

		public static DbQueryResult GetUserProductReviewsAndReplys(ProductReviewQuery query)
		{
			query.UserId = HiContext.Current.UserId;
			return new ProductReviewDao().GetProductReviews(query);
		}

		public static bool InsertProductConsultation(ProductConsultationInfo productConsultation)
		{
			Globals.EntityCoding(productConsultation, true);
			bool flag = new ProductConsultationDao().Add(productConsultation, null) > 0;
			if (flag)
			{
				ProductHelper.ClearProductCahe(productConsultation.ProductId);
			}
			return flag;
		}

		public static DbQueryResult GetProductConsultations(ProductConsultationAndReplyQuery page)
		{
			return new ProductConsultationDao().GetConsultationProducts(page);
		}

		public static int GetProductConsultationsCount(int productId, bool includeUnReplied)
		{
			return new ProductConsultationDao().GetProductConsultationsCount(productId, includeUnReplied);
		}

		public static DataTable GetProductDetailConsultations(int ProductId, int MaxNum)
		{
			return new ProductConsultationDao().GetProductDetailConsultations(ProductId, MaxNum);
		}

		public static int GetUserProductConsultaionsCount(int userId)
		{
			return new ProductConsultationDao().GetUserProductConsultaionsCount(userId);
		}

		public static DbQueryResult GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query)
		{
			return new ProductConsultationDao().GetConsultationProducts(query);
		}

		public static DbQueryResult GetGroupByProductList(ProductBrowseQuery query)
		{
			return new GroupBuyDao().GetGroupByProductList(query);
		}

		public static DataTable GetGroupByProductList(int maxnum)
		{
			return new GroupBuyDao().GetGroupByProductList(maxnum);
		}

		public static int GetOrderCount(int groupBuyId)
		{
			return new GroupBuyDao().GetOrderCount(groupBuyId);
		}

		public static decimal GetCurrentPrice(int groupBuyId)
		{
			return new GroupBuyDao().Get<GroupBuyInfo>(groupBuyId).Price;
		}

		public static GroupBuyInfo GetGroupBuy(int groupbuyId)
		{
			return new GroupBuyDao().Get<GroupBuyInfo>(groupbuyId);
		}

		public static DataTable GetGroupBuyProducts(int categoryId, string keywords, int page, int size, out int total, bool onlyUnFinished = true)
		{
			return new GroupBuyDao().GetGroupBuyProducts(CatalogHelper.GetCategory(categoryId), keywords, page, size, out total, onlyUnFinished);
		}

		public static DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
		{
			return new CountDownDao().GetCountDownProductList(query);
		}

		public static CountDownInfo GetCountDownInfoByCountDownId(int countDownId)
		{
			return new CountDownDao().Get<CountDownInfo>(countDownId);
		}

		public static bool IsActiveCountDownByProductId(int productId)
		{
			return new CountDownDao().IsActiveCountDownByProductId(productId);
		}

		public static bool IsActiveGroupByProductId(int productId)
		{
			return new GroupBuyDao().IsActiveGroupByProductId(productId);
		}

		public static GroupBuyInfo GetGroupByProdctId(int productId)
		{
			return new GroupBuyDao().GetGroupByProdctId(productId);
		}

		public static bool IsActiveGroupBuyByProductId(int productId)
		{
			return new CountDownDao().IsActiveGroupBuyByProductId(productId);
		}

		public static int GetActivityStartsImmediatelyAboutCountDown(int productId)
		{
			return new CountDownDao().GetActivityStartsImmediatelyAboutCountDown(productId);
		}

		public static int NearCountDown(int productId)
		{
			return new CountDownDao().NearCountDown(productId);
		}

		public static CountDownInfo GetCountDownInfoNoLimit(int productId)
		{
			return new CountDownDao().GetCountDownInfoNoLimit(productId);
		}

		public static DataTable GetCounDownProducList(int maxnum)
		{
			return new CountDownDao().GetCounDownProducList(maxnum);
		}

		public static PromotionInfo GetProductPromotionInfo(int productid)
		{
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				return new PromotionDao().GetProductPromotionInfo(productid, user.GradeId);
			}
			return ProductBrowser.GetAllProductPromotionInfo(productid);
		}

		public static PromotionInfo GetAllProductPromotionInfo(int productid)
		{
			return new PromotionDao().GetAllProductPromotionInfo(productid, 0);
		}

		public static GiftInfo GetGiftDetails(int giftId)
		{
			return new GiftDao().Get<GiftInfo>(giftId);
		}

		public static DbQueryResult GetOnlineGifts(GiftQuery page)
		{
			page.IsOnline = true;
			return new GiftDao().GetGifts(page);
		}

		public static IList<GiftInfo> GetOnlinePromotionGifts()
		{
			return new GiftDao().GetOnlinePromotionGifts();
		}

		public static GiftInfo GetGift(int giftId)
		{
			return new GiftDao().Get<GiftInfo>(giftId);
		}

		public static IList<GiftInfo> GetGiftDetailsByGiftIds(string giftIds)
		{
			return new GiftDao().GetGiftDetailsByGiftIds(giftIds);
		}

		public static IList<GiftInfo> GetGifts(int maxnum)
		{
			return new GiftDao().Gets<GiftInfo>("GiftId", SortAction.Asc, maxnum);
		}

		public static int GetLineItemNumber(int productId)
		{
			return new LineItemDao().GetLineItemNumber(productId);
		}

		public static DataTable GetLineItems(int productId, int maxNum)
		{
			return new LineItemDao().GetLineItems(productId, maxNum);
		}

		public static DbQueryResult GetLineItems(Pagination page, int productId)
		{
			return new LineItemDao().GetLineItems(page, productId);
		}

		public static DbQueryResult GetLineItems(int productId, int currentPage, int pageSize)
		{
			return new LineItemDao().GetLineItems(productId, currentPage, pageSize);
		}

		public static bool IsBuyProduct(int productId)
		{
			return new LineItemDao().IsBuyProduct(productId, HiContext.Current.UserId);
		}

		public static DbQueryResult GetBatchBuyProducts(ProductQuery query)
		{
			return new ProductBrowseDao().GetBatchBuyProducts(query);
		}

		public static DataTable GetSkusByProductId(int productId)
		{
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			return new ProductBrowseDao().GetSkusByProductId(productId, gradeId);
		}

		public static string GetImageUrl(string url)
		{
			string result = url;
			if (string.IsNullOrEmpty(url))
			{
				result = SettingsManager.GetMasterSettings().DefaultProductImage;
			}
			return result;
		}

		public static DataTable GetHotProductList()
		{
			return new ProductBrowseDao().GetHotProductList();
		}

		public static Dictionary<string, SKUItem> GetProductSkuSaleInfo(int productId, int storeId = 0)
		{
			int gradeId = 0;
			MemberInfo user = HiContext.Current.User;
			if (user.UserId != 0)
			{
				gradeId = user.GradeId;
			}
			ProductDao productDao = new ProductDao();
			if (storeId == 0)
			{
				return productDao.GetProductSkuInfo(productId, gradeId, SettingsManager.GetMasterSettings().OpenMultStore);
			}
			return productDao.GetProductSkuInfo(productId, gradeId, storeId);
		}

		public static Dictionary<string, SKUItem> GetPreSaleProductSkuSaleInfo(int productId)
		{
			ProductDao productDao = new ProductDao();
			return productDao.GetProductSkuInfo(productId, 0, false);
		}

		public static DataTable ApiGetSkusByProductId(int pid)
		{
			return new ProductBrowseDao().ApiGetSkusByProductId(pid);
		}

		public static PageModel<ProductConsultationInfo> GetProductConsultationList(int productId, int pageIndex, int pageSize)
		{
			return new ProductConsultationDao().GetProductConsultationList(productId, pageIndex, pageSize);
		}

		public static ProductModel GetProductSkus(int productId, int gradeId, bool MutiStores = false, int storeId = 0)
		{
			return new ProductBrowseDao().GetProductSkus(productId, gradeId, MutiStores, storeId);
		}
	}
}
