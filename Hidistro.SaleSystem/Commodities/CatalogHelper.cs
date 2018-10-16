using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Commodities;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Hidistro.SaleSystem.Commodities
{
	public sealed class CatalogHelper
	{
		public static IEnumerable<CategoryInfo> GetSubCategories(int parentCategoryId)
		{
			return from item in CatalogHelper.GetCategories()
			where item.ParentCategoryId == parentCategoryId
			select item;
		}

		public static IEnumerable<CategoryInfo> GetMainCategories()
		{
			return from item in CatalogHelper.GetCategories()
			where item.Depth == 1
			select item;
		}

		public static IList<CategoryInfo> GetSequenceCategories(string categoryname = "")
		{
			IList<CategoryInfo> list = null;
			if (!string.IsNullOrEmpty(categoryname))
			{
				CategoriesQuery categoriesQuery = new CategoriesQuery();
				categoriesQuery.Name = categoryname;
				list = CatalogHelper.GetCategoryList(categoriesQuery);
			}
			else
			{
				list = new List<CategoryInfo>();
				IEnumerable<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
				foreach (CategoryInfo item in mainCategories)
				{
					list.Add(item);
					CatalogHelper.LoadSubCategorys(item.CategoryId, list);
				}
			}
			return list;
		}

		private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
		{
			IEnumerable<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(parentCategoryId);
			if (subCategories != null)
			{
				foreach (CategoryInfo item in subCategories)
				{
					categories.Add(item);
					CatalogHelper.LoadSubCategorys(item.CategoryId, categories);
				}
			}
		}

		public static IEnumerable<CategoryInfo> GetCategories()
		{
			IEnumerable<CategoryInfo> enumerable = HiCache.Get<IEnumerable<CategoryInfo>>("DataCache-Categories");
			if (enumerable == null)
			{
				enumerable = new CategoryDao().Gets<CategoryInfo>("DisplaySequence", SortAction.Desc, null);
				HiCache.Insert("DataCache-Categories", enumerable, 360);
			}
			return enumerable;
		}

		public static CategoryInfo GetCategory(int categoryId)
		{
			return CatalogHelper.GetCategories().FirstOrDefault((CategoryInfo item) => item.CategoryId == categoryId);
		}

		public static string GetFullCategory(int categoryId)
		{
			CategoryInfo category = CatalogHelper.GetCategory(categoryId);
			if (category == null)
			{
				return null;
			}
			string text = category.Name;
			while (category != null && category.ParentCategoryId > 0)
			{
				category = CatalogHelper.GetCategory(category.ParentCategoryId);
				if (category != null)
				{
					text = category.Name + " &raquo; " + text;
				}
			}
			return text;
		}

		public static bool AddCategory(CategoryInfo category)
		{
			if (category == null)
			{
				return false;
			}
			CategoryDao categoryDao = new CategoryDao();
			category.CategoryId = categoryDao.GetMaxDisplaySequence<CategoryInfo>();
			int categoryId;
			if (category.ParentCategoryId > 0)
			{
				CategoryInfo category2 = CatalogHelper.GetCategory(category.ParentCategoryId);
				string path = category2.Path;
				categoryId = category.CategoryId;
				category.Path = path + "|" + categoryId.ToString();
				category.Depth = category2.Depth + 1;
			}
			else
			{
				categoryId = category.CategoryId;
				category.Path = categoryId.ToString();
				category.Depth = 1;
			}
			Globals.EntityCoding(category, true);
			bool flag = categoryDao.Add(category, null) > 0;
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.AddProductCategory, string.Format(CultureInfo.InvariantCulture, "创建了一个新的店铺分类:”{0}”", new object[1]
				{
					category.Name
				}), false);
				HiCache.Remove("DataCache-Categories");
			}
			return flag;
		}

		public static CategoryActionStatus UpdateCategory(CategoryInfo category)
		{
			if (category == null)
			{
				return CategoryActionStatus.UnknowError;
			}
			Globals.EntityCoding(category, true);
			CategoryActionStatus categoryActionStatus = (!new CategoryDao().Update(category, null)) ? CategoryActionStatus.UnknowError : CategoryActionStatus.Success;
			if (categoryActionStatus == CategoryActionStatus.Success)
			{
				EventLogs.WriteOperationLog(Privilege.EditProductCategory, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品分类", new object[1]
				{
					category.CategoryId
				}), false);
				HiCache.Remove("DataCache-Categories");
			}
			return categoryActionStatus;
		}

		public static bool SwapCategorySequence(int categoryId, int displaysequence)
		{
			return new CategoryDao().SaveSequence<CategoryInfo>(categoryId, displaysequence, null);
		}

		public static bool DeleteCategory(int categoryId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductCategory);
			CategoryInfo category = CatalogHelper.GetCategory(categoryId);
			if (category == null)
			{
				return false;
			}
			bool flag = new CategoryDao().DeleteCategory(categoryId, category.Path);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductCategory, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的店铺分类", new object[1]
				{
					categoryId
				}), false);
				HiCache.Remove("DataCache-Categories");
			}
			return flag;
		}

		public static int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			return new CategoryDao().DisplaceCategory(oldCategoryId, newCategory);
		}

		public static bool SetProductExtendCategory(int productId, string extendCategoryPath, int extendIndex = 1)
		{
			return new CategoryDao().SetProductExtendCategory(productId, extendCategoryPath, extendIndex);
		}

		public static bool SetCategoryThemes(int categoryId, string themeName)
		{
			if (new CategoryDao().SetCategoryThemes(categoryId, themeName))
			{
				HiCache.Remove("DataCache-Categories");
			}
			return false;
		}

		public static DataTable GetCategoryes(string categroynaem)
		{
			return new CategoryDao().GetCategoryes(categroynaem);
		}

		public static DbQueryResult GetBrandQuery(BrandQuery query)
		{
			return new BrandCategoryDao().Query(query);
		}

		public static DbQueryResult Query(CategoriesQuery query)
		{
			return new CategoryDao().Query(query);
		}

		public static IList<CategoryInfo> GetCategoryList(CategoriesQuery query)
		{
			return new CategoryDao().GetCategoryList(query);
		}

		public static bool AddBrandCategory(BrandCategoryInfo brandCategory)
		{
			BrandCategoryDao brandCategoryDao = new BrandCategoryDao();
			brandCategory.DisplaySequence = brandCategoryDao.GetMaxDisplaySequence<BrandCategoryInfo>();
			int num = (int)brandCategoryDao.Add(brandCategory, null);
			if (num > 0 && brandCategory.ProductTypes.Count > 0)
			{
				brandCategoryDao.AddBrandProductTypes(num, brandCategory.ProductTypes);
			}
			return true;
		}

		public static DataTable GetBrandCategories(int productTypeId = 0)
		{
			return new BrandCategoryDao().GetBrandCategories(productTypeId);
		}

		public static BrandCategoryInfo GetBrandCategory(int brandId)
		{
			return new BrandCategoryDao().GetBrandCategory(brandId);
		}

		public static bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
		{
			BrandCategoryDao brandCategoryDao = new BrandCategoryDao();
			bool flag = brandCategoryDao.Update(brandCategory, null);
			if (flag && brandCategoryDao.DeleteBrandProductTypes(brandCategory.BrandId))
			{
				brandCategoryDao.AddBrandProductTypes(brandCategory.BrandId, brandCategory.ProductTypes);
			}
			return flag;
		}

		public static bool BrandHvaeProducts(int brandId)
		{
			return new BrandCategoryDao().BrandHvaeProducts(brandId);
		}

		public static bool DeleteBrandCategory(int brandId)
		{
			return new BrandCategoryDao().Delete<BrandCategoryInfo>(brandId);
		}

		public static bool UpdateBrandCategoryDisplaySequence(int barndId, int displaysequence)
		{
			return new BrandCategoryDao().SaveSequence<BrandCategoryInfo>(barndId, displaysequence, null);
		}

		public static string UploadBrandCategorieImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/brand/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}

		public static bool SetBrandCategoryThemes(int brandid, string themeName)
		{
			bool flag = new BrandCategoryDao().SetBrandCategoryThemes(brandid, themeName);
			if (flag)
			{
				HiCache.Remove("DataCache-Categories");
			}
			return flag;
		}

		public static DataTable GetBrandCategories(string brandName)
		{
			return new BrandCategoryDao().GetBrandCategories(brandName);
		}

		public static IEnumerable<BrandMode> GetBrandCategories(int categoryId, int maxNum = 1000)
		{
			IEnumerable<BrandMode> enumerable = HiCache.Get<IEnumerable<BrandMode>>($"DataCache-Brands-{categoryId}-{maxNum}");
			if (enumerable == null)
			{
				enumerable = new BrandCategoryDao().GetBrandCategories(CatalogHelper.GetCategory(categoryId), maxNum);
				if (enumerable != null)
				{
					HiCache.Insert($"DataCache-Brands-{categoryId}-{maxNum}", enumerable, 360);
				}
			}
			return enumerable;
		}

		public static IList<TagInfo> GetTags()
		{
			return new TagDao().Gets<TagInfo>("TagID", SortAction.Desc, null);
		}

		public static string GetTagName(int tagId)
		{
			TagInfo tagInfo = new TagDao().Get<TagInfo>(tagId);
			if (tagInfo != null)
			{
				return tagInfo.TagName;
			}
			return "";
		}

		public static int AddTags(string tagName)
		{
			TagDao tagDao = new TagDao();
			int result = 0;
			if (tagDao.GetTags(tagName) <= 0)
			{
				TagInfo model = new TagInfo
				{
					TagName = tagName
				};
				result = (int)tagDao.Add(model, null);
			}
			return result;
		}

		public static bool UpdateTags(int tagId, string tagName)
		{
			TagInfo model = new TagInfo
			{
				TagID = tagId,
				TagName = tagName
			};
			return new TagDao().Update(model, null);
		}

		public static bool DeleteTags(int tagId)
		{
			return new TagDao().DeleteTags(tagId);
		}

		public static IList<CategoryInfo> GetStoreLeafCategory(int storeId)
		{
			return new CategoryDao().GetStoreLeafCategory(storeId);
		}
	}
}
