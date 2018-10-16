using Hidistro.Context;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Supplier;
using Hidistro.SaleSystem.Store;
using Hidistro.SqlDal.Supplier;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Supplier
{
	public sealed class SupplierHelper
	{
		private SupplierHelper()
		{
		}

		public static int AddSupplier(SupplierInfo supplier)
		{
			int num = (int)new SupplierDao().Add(supplier, null);
			if (num > 0)
			{
				GalleryHelper.AddPhotoCategory("默认分类", num);
			}
			return num;
		}

		public static bool UpdateSupplier(SupplierInfo supplier)
		{
			return new SupplierDao().Update(supplier, null);
		}

		public static bool DeleteSupplier(int supplierId)
		{
			return new SupplierDao().Delete<SupplierInfo>(supplierId);
		}

		public static int UpdateSupplier_Frozen(int supplierId)
		{
			return new SupplierDao().UpdateSupplier_Frozen(supplierId);
		}

		public static int UpdateSupplier_Recover(int supplierId)
		{
			return new SupplierDao().UpdateSupplier_Recover(supplierId);
		}

		public static bool ExistSupplierName(int supplierId, string SupplierName)
		{
			return new SupplierDao().ExistSupplierName(supplierId, SupplierName);
		}

		public bool IsManangerCanLogin(int StoreId)
		{
			return new SupplierDao().IsManangerCanLogin(StoreId);
		}

		public static DbQueryResult GetSupplierManagers(SupplierQuery query)
		{
			return new SupplierDao().GetSupplierAdmin(query);
		}

		public static IList<SupplierExportModel> GetSupplierExportData(SupplierQuery query)
		{
			return new SupplierDao().GetSupplierExportData(query);
		}

		public static IList<SupplierInfo> GetSupplierAll(int maxnum = 0)
		{
			int? maxNum = null;
			if (maxnum > 0)
			{
				maxNum = maxnum;
			}
			return new SupplierDao().Gets<SupplierInfo>("SupplierId", SortAction.Asc, maxNum);
		}

		public static SupplierInfo GetSupplierById(int supplierid)
		{
			return new SupplierDao().Get<SupplierInfo>(supplierid);
		}

		public static SupplierStatisticsInfo GetStatisticsInfo(int supplierId)
		{
			return new SupplierDao().Statistics(supplierId);
		}

		public static List<ProductTop10Info> GetTop10Product10Info(int supplierId)
		{
			return new SupplierDao().GetTop10Product10Info(supplierId);
		}

		public static int ValidTradePassword(int supplierId, string tradePassword)
		{
			SupplierInfo supplierById = SupplierHelper.GetSupplierById(supplierId);
			if (string.IsNullOrEmpty(supplierById.TradePassword))
			{
				return -1;
			}
			if (supplierById.TradePassword == Users.EncodePassword(tradePassword, supplierById.TradePasswordSalt) || supplierById.TradePassword == Users.EncodePassword_Old(tradePassword, supplierById.TradePasswordSalt))
			{
				return 1;
			}
			return 0;
		}

		public static string GetSupplierName(int supplierId)
		{
			return new SupplierDao().GetSupplierName(supplierId);
		}
	}
}
