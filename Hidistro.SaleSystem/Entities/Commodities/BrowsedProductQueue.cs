using Hidistro.Context;
using Hidistro.Core;
using Hidistro.SqlDal;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.Entities.Commodities
{
	public class BrowsedProductQueue
	{
		private static string browedProductList = "BrowedProductList-Admin";

		public static IList<int> GetBrowedProductList()
		{
			IList<int> result = new List<int>();
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[BrowsedProductQueue.browedProductList];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				result = (Serializer.ConvertToObject(HiContext.Current.Context.Server.UrlDecode(httpCookie.Value), typeof(List<int>)) as List<int>);
			}
			return result;
		}

		public static List<ProductYouLikeModel> GetProductYouLike(int productId, int storeId, List<int> likePids = null, bool showServiceProduct = true)
		{
			List<ProductYouLikeModel> list = new List<ProductYouLikeModel>();
			RelatedProductsDao relatedProductsDao = new RelatedProductsDao();
			list = relatedProductsDao.GetRelatedProducts(productId, storeId, showServiceProduct);
			if (list.Count == 0)
			{
				if (likePids != null && likePids.Count > 0)
				{
					likePids.Remove(productId);
					if (likePids.Count > 0)
					{
						list = relatedProductsDao.GetMyViewProducts(likePids, storeId, showServiceProduct);
					}
				}
				else
				{
					HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[BrowsedProductQueue.browedProductList];
					if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
					{
						List<int> list2 = Serializer.ConvertToObject(HiContext.Current.Context.Server.UrlDecode(httpCookie.Value), typeof(List<int>)) as List<int>;
						list2.Remove(productId);
						if (list2.Count > 0)
						{
							list = relatedProductsDao.GetMyViewProducts(list2, storeId, showServiceProduct);
						}
					}
				}
			}
			BrowsedProductQueue.ProcessProductImg(list);
			return list;
		}

		private static void ProcessProductImg(List<ProductYouLikeModel> result)
		{
			string imagePathTemp = "/storage/master/product/images/";
			string bigThumbPathTemp = "/storage/master/product/thumbs410/410_";
			result.ForEach(delegate(ProductYouLikeModel r)
			{
				if (!string.IsNullOrEmpty(r.ProdImg))
				{
					r.ProdImg = Globals.FullPath(r.ProdImg.ToLower().Replace(imagePathTemp, bigThumbPathTemp));
				}
				else
				{
					r.ProdImg = Globals.FullPath(SettingsManager.GetMasterSettings().DefaultProductThumbnail4);
				}
				if (!string.IsNullOrEmpty(r.ProdImg_S))
				{
					r.ProdImg_S = Globals.FullPath(r.ProdImg_S.ToLower().Replace(imagePathTemp, bigThumbPathTemp));
				}
				else
				{
					r.ProdImg_S = Globals.FullPath(SettingsManager.GetMasterSettings().DefaultProductThumbnail4);
				}
			});
		}

		public static IList<int> GetBrowedProductList(int maxNum)
		{
			IList<int> list = BrowsedProductQueue.GetBrowedProductList();
			int count = list.Count;
			if (list.Count > maxNum)
			{
				for (int i = 0; i < count - maxNum; i++)
				{
					list.RemoveAt(0);
				}
			}
			return list;
		}

		public static void EnQueue(int productId)
		{
			IList<int> list = BrowsedProductQueue.GetBrowedProductList();
			int num = 0;
			foreach (int item in list)
			{
				if (productId == item)
				{
					list.RemoveAt(num);
					break;
				}
				num++;
			}
			if (list.Count <= 20)
			{
				list.Add(productId);
			}
			else
			{
				list.RemoveAt(0);
				list.Add(productId);
			}
			BrowsedProductQueue.SaveCookie(list);
		}

		public static void ClearQueue()
		{
			BrowsedProductQueue.SaveCookie(null);
		}

		private static void SaveCookie(IList<int> productIdList)
		{
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies[BrowsedProductQueue.browedProductList];
			if (httpCookie == null)
			{
				httpCookie = new HttpCookie(BrowsedProductQueue.browedProductList);
			}
			httpCookie.HttpOnly = true;
			httpCookie.Expires = DateTime.Now.AddDays(7.0);
			httpCookie.Value = Globals.UrlEncode(Serializer.ConvertToString(productIdList));
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}
	}
}
