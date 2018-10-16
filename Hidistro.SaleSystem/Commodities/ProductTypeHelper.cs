using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Commodities;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.SaleSystem.Commodities
{
	public sealed class ProductTypeHelper
	{
		private ProductTypeHelper()
		{
		}

		public static DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return new ProductTypeDao().GetProductTypes(query);
		}

		public static IList<ProductTypeInfo> GetProductTypes()
		{
			return new ProductTypeDao().Gets<ProductTypeInfo>("TypeId", SortAction.Asc, null);
		}

		public static ProductTypeInfo GetProductType(int typeId)
		{
			return new ProductTypeDao().GetProductType(typeId);
		}

		public static DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			return new ProductTypeDao().GetBrandCategoriesByTypeId(typeId);
		}

		public static int GetTypeId(string typeName)
		{
			ProductTypeDao productTypeDao = new ProductTypeDao();
			int typeId = productTypeDao.GetTypeId(typeName);
			if (typeId > 0)
			{
				return typeId;
			}
			ProductTypeInfo productTypeInfo = new ProductTypeInfo();
			productTypeInfo.TypeName = typeName;
			return (int)productTypeDao.Add(productTypeInfo, null);
		}

		public static int AddProductType(ProductTypeInfo productType)
		{
			if (productType == null)
			{
				return 0;
			}
			ProductTypeDao productTypeDao = new ProductTypeDao();
			Globals.EntityCoding(productType, true);
			int num = (int)productTypeDao.Add(productType, null);
			if (num > 0)
			{
				if (productType.Brands.Count > 0)
				{
					productTypeDao.AddProductTypeBrands(num, productType.Brands);
				}
				EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "创建了一个新的商品类型:”{0}”", new object[1]
				{
					productType.TypeName
				}), false);
			}
			return num;
		}

		public static bool UpdateProductType(ProductTypeInfo productType)
		{
			if (productType == null)
			{
				return false;
			}
			ProductTypeDao productTypeDao = new ProductTypeDao();
			Globals.EntityCoding(productType, true);
			bool flag = productTypeDao.Update(productType, null);
			if (flag)
			{
				if (productTypeDao.DeleteProductTypeBrands(productType.TypeId))
				{
					productTypeDao.AddProductTypeBrands(productType.TypeId, productType.Brands);
				}
				EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的商品类型", new object[1]
				{
					productType.TypeId
				}), false);
			}
			return flag;
		}

		public static bool DeleteProductType(int typeId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
			bool flag = new ProductTypeDao().DeleteProducType(typeId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的商品类型", new object[1]
				{
					typeId
				}), false);
			}
			return flag;
		}

		public static AttributeInfo GetAttribute(int attributeId)
		{
			return new AttributeDao().GetAttribute(attributeId);
		}

		public static bool AddAttribute(AttributeInfo attribute)
		{
			AttributeDao attributeDao = new AttributeDao();
			attribute.DisplaySequence = attributeDao.GetMaxDisplaySequence<AttributeInfo>();
			int num = (int)attributeDao.Add(attribute, null);
			if (num <= 0)
			{
				return false;
			}
			if (attribute.AttributeValues.Count != 0)
			{
				foreach (AttributeValueInfo attributeValue in attribute.AttributeValues)
				{
					attributeValue.AttributeId = num;
					attributeValue.DisplaySequence = attributeDao.GetMaxDisplaySequence<AttributeValueInfo>();
					attributeDao.Add(attributeValue, null);
				}
			}
			return true;
		}

		public static int GetSpecificationId(int typeId, string specificationName)
		{
			AttributeDao attributeDao = new AttributeDao();
			int specificationId = attributeDao.GetSpecificationId(typeId, specificationName);
			if (specificationId > 0)
			{
				return specificationId;
			}
			AttributeInfo attributeInfo = new AttributeInfo();
			attributeInfo.TypeId = typeId;
			attributeInfo.UsageMode = AttributeUseageMode.Choose;
			attributeInfo.UseAttributeImage = false;
			attributeInfo.AttributeName = specificationName;
			attributeInfo.DisplaySequence = attributeDao.GetMaxDisplaySequence<AttributeInfo>();
			return (int)attributeDao.Add(attributeInfo, null);
		}

		public static bool AddAttributeName(AttributeInfo attribute)
		{
			AttributeDao attributeDao = new AttributeDao();
			attribute.DisplaySequence = attributeDao.GetMaxDisplaySequence<AttributeInfo>();
			return attributeDao.Add(attribute, null) > 0;
		}

		public static bool UpdateAttributeName(AttributeInfo attribute)
		{
			return new AttributeDao().Update(attribute, null);
		}

		public static bool UpdateIsUseAttribute(AttributeInfo attribute)
		{
			return new AttributeDao().UpdateIsUseAttribute(attribute);
		}

		public static int HasSetUseImg(int typeId)
		{
			return new AttributeDao().HasSetUseImg(typeId);
		}

		public static bool DeleteAttribute(int attriubteId)
		{
			return new AttributeDao().DeleteAttribute(attriubteId);
		}

		public static void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
		{
			new AttributeDao().SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
		}

		public static IList<AttributeInfo> GetAttributes(int typeId)
		{
			return new AttributeDao().GetAttributes(typeId);
		}

		public static IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
		{
			return new AttributeDao().GetAttributes(attributeUseageMode);
		}

		public static IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
		{
			return new AttributeDao().GetAttributes(typeId, attributeUseageMode);
		}

		public static int AddAttributeValue(AttributeValueInfo attributeValue)
		{
			AttributeValueDao attributeValueDao = new AttributeValueDao();
			attributeValue.DisplaySequence = attributeValueDao.GetMaxDisplaySequence<AttributeValueInfo>();
			return (int)new AttributeValueDao().Add(attributeValue, null);
		}

		public static int GetSpecificationValueId(int attributeId, string valueStr)
		{
			AttributeValueDao attributeValueDao = new AttributeValueDao();
			int specificationValueId = attributeValueDao.GetSpecificationValueId(attributeId, valueStr);
			if (specificationValueId > 0)
			{
				return specificationValueId;
			}
			AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
			attributeValueInfo.AttributeId = attributeId;
			attributeValueInfo.ValueStr = valueStr;
			attributeValueInfo.DisplaySequence = attributeValueDao.GetMaxDisplaySequence<AttributeValueInfo>();
			return (int)attributeValueDao.Add(attributeValueInfo, null);
		}

		public static bool ClearAttributeValue(int attributeId)
		{
			return new AttributeValueDao().ClearAttributeValue(attributeId);
		}

		public static bool DeleteAttributeValue(int attributeValueId)
		{
			return new AttributeValueDao().DeleteAttributeValue(attributeValueId);
		}

		public static bool UpdateAttributeValue(AttributeValueInfo attributeValue)
		{
			return new AttributeValueDao().Update(attributeValue, null);
		}

		public static void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			new AttributeValueDao().SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}

		public static AttributeValueInfo GetAttributeValueInfo(int valueId)
		{
			return new AttributeValueDao().Get<AttributeValueInfo>(valueId);
		}

		public static IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum = 1000)
		{
			return new AttributeDao().GetAttributeInfoByCategoryId(categoryId, maxNum);
		}

		public static string UploadSKUImage(HttpPostedFile postedFile)
		{
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image", null))
			{
				return string.Empty;
			}
			string text = HiContext.Current.GetStoragePath() + "/sku/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
			postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(text));
			return text;
		}

		public static bool HasSameProductTypeName(string typeName, int typeId = 0)
		{
			return new ProductTypeDao().HasSameProductTypeName(typeName, typeId);
		}
	}
}
