using Hidistro.Core;
using Hidistro.Entities;

namespace Hidistro.SaleSystem.Store
{
	public abstract class APPProvider
	{
		private static readonly APPProvider _defaultInstance;

		static APPProvider()
		{
			APPProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.APPData,Hidistro.SaleSystem.Data") as APPProvider);
		}

		public static APPProvider Instance()
		{
			return APPProvider._defaultInstance;
		}

		public abstract AppVersionRecordInfo GetLatestAppVersionRecord(string device);

		public abstract bool AddAppVersionRecord(AppVersionRecordInfo appVersionRecord);

		public abstract bool IsForcibleUpgrade(string device, decimal version);

		public abstract bool AddAppInstallRecord(AppInstallRecordInfo appInstallRecord);
	}
}
