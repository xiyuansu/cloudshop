using Hidistro.Core.Jobs;
using Hidistro.SaleSystem.Store;
using System.Xml;

namespace Hidistro.Jobs
{
	public class AppPushJob : IJob
	{
		public void Execute(XmlNode node)
		{
			VShopHelper.AppPushRecordForJob();
		}
	}
}
