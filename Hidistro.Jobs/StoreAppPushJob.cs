using Hidistro.Core.Jobs;
using Hidistro.SaleSystem.Store;
using System.Xml;

namespace Hidistro.Jobs
{
	public class StoreAppPushJob : IJob
	{
		public void Execute(XmlNode node)
		{
			VShopHelper.AppPushStoreStockForJob();
		}
	}
}
