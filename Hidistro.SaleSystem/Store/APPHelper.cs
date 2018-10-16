using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SqlDal.App;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Store;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Store
{
	public static class APPHelper
	{
		public static AppVersionRecordInfo GetLatestAppVersionRecord(string device)
		{
			return new AppVersionRecordDao().GetLatestAppVersionRecord(device);
		}

		public static bool IsExistNewVersion(string newVersion, string oldVersion)
		{
			bool result = false;
			newVersion = newVersion.Replace(".", "");
			oldVersion = oldVersion.Replace(".", "");
			for (int i = 0; i < newVersion.Length; i++)
			{
				int num = newVersion.Substring(i, 1).ToInt(0);
				int num2 = (oldVersion.Length > i) ? oldVersion.Substring(i, 1).ToInt(0) : 0;
				if (num > num2)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public static bool AddAppVersionRecord(AppVersionRecordInfo appVersionRecord)
		{
			return new AppVersionRecordDao().Add(appVersionRecord, null) > 0;
		}

		public static bool IsForcibleUpgrade(string device, decimal version)
		{
			return new AppVersionRecordDao().IsForcibleUpgrade(device, version);
		}

		public static bool AddAppInstallRecord(AppInstallRecordInfo appInstallRecord)
		{
			return new AppVersionRecordDao().AddAppInstallRecord(appInstallRecord);
		}

		public static int UserSignIn(int userId, int cDays)
		{
			DateTime lastSignDate = DateTime.Parse(DateTime.Now.ToShortDateString());
			UserSignInInfo userSignInInfo = new UserSignInInfo();
			userSignInInfo.UserId = userId;
			userSignInInfo.LastSignDate = lastSignDate;
			return new SignInDal().SaveUserSignIn(userSignInInfo, cDays);
		}

		public static int GetContinuousDays(int userId)
		{
			return new SignInDal().GetContinuousDays(userId);
		}

		public static int SaveAppLotteryDraw(AppLotteryDraw draw)
		{
			return (int)new AppLotteryDrawDao().Add(draw, null);
		}

		public static IList<AppLotteryDraw> GetAppLotteryDraw(int userId)
		{
			return new AppLotteryDrawDao().GetAppLotteryDraw(userId);
		}
	}
}
