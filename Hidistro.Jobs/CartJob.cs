using Hidistro.Core.Jobs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace Hidistro.Jobs
{
	public class CartJob : IJob
	{
		public void Execute(XmlNode node)
		{
			int num = 5;
			XmlAttribute xmlAttribute = node.Attributes["expires"];
			if (xmlAttribute != null)
			{
				int.TryParse(xmlAttribute.Value, out num);
			}
			Database database = DatabaseFactory.CreateDatabase();
			DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE AddTime <= @CurrentTime;");
			database.AddInParameter(sqlStringCommand, "CurrentTime", DbType.DateTime, DateTime.Now.AddDays((double)(-num)));
			database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
