using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Entities.Members;
using Hidistro.Messages;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Xml;

namespace Hidistro.Jobs
{
	public class CouponJob : IJob
	{
		private Database database = DatabaseFactory.CreateDatabase();

		public void Execute(XmlNode node)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			this.ProcessorCouponMessage(masterSettings);
		}

		public void ProcessorCouponMessage(SiteSettings setting)
		{
			DateTime now = DateTime.Now;
			if ((now.Hour == 12 || now.Hour == 14 || now.Hour == 10) && setting.LastSendCouponsMessengerTime.Date < now.Date)
			{
				this.SendCouponsWillExpireMessage(setting);
			}
		}

		public void SendCouponsWillExpireMessage(SiteSettings setting)
		{
			DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(CouponId) AS CouponNums,UserId FROM  (SELECT UserId,CouponId FROM Hishop_CouponItems WHERE DATEDIFF(DD,GETDATE(),ClosingTime)<=2 AND ClosingTime > GETDATE()) AS STable GROUP BY UserId ");
			using (IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					MemberInfo user = Users.GetUser(((IDataRecord)dataReader)["UserId"].ToInt(0));
					if (user != null)
					{
						Messenger.CouponsWillExpire(user, ((IDataRecord)dataReader)["CouponNums"].ToInt(0));
					}
				}
			}
			setting.LastSendCouponsMessengerTime = DateTime.Now;
			SettingsManager.Save(setting);
		}
	}
}
